/*
    AUTOPARKDB — corrective patch for the existing database schema.

    Target: the AutoparkDB object set captured by the database export dated
    2026-10-11. This is NOT a database creation/reset script and is NOT a
    replacement for AutoparkDB_Schema.sql, which is explicitly historical.

    Before running:
      1. Back up the database.
      2. Confirm that the existing database has the objects named below and
         matches the 2026-10-11 export. Do not run this against the old
         historical reset script without reconciling the schemas.
      3. Review and run on the intended database.

    The patch does not delete or rewrite transactional history. It does
    reconcile derived sale/payment status, trip-seat availability, and bus
    status/mileage from the existing records. It does not reconstruct
    ticket.available_count values already affected by past cancellations:
    the original stock baseline cannot be inferred reliably from current rows.

    Fixes:
      - Remove the duplicate INSERT payment trigger which marks partial
        payments as fully paid.
      - Make cancelled/refunded sales consistently release or avoid consuming
        ticket stock.
      - Remove the conflicting high-mileage bus trigger rules.
      - Exclude cancelled/refunded sales from the sales views and report
        procedures, matching the application reporting rules.
*/

USE [AutoparkDB];
GO

/* An explicit exception is required to keep a high-mileage bus operational.
   Existing buses default to no exception, preserving the automatic reserve rule. */
IF COL_LENGTH(N'dbo.bus', N'high_mileage_override') IS NULL
BEGIN
    ALTER TABLE dbo.bus
        ADD high_mileage_override bit NOT NULL
            CONSTRAINT df_bus_high_mileage_override DEFAULT (0);
END;
GO

/* Remove the duplicate trigger. trg_payment_insert_sync_sale_status already
   recalculates payment_status from the total of successful payments. */
IF OBJECT_ID(N'dbo.trg_payment_insert_mark_sale_paid', N'TR') IS NOT NULL
    DROP TRIGGER dbo.trg_payment_insert_mark_sale_paid;
GO

/* Sale INSERT: consume inventory only for sales that are not cancelled/refunded. */
CREATE OR ALTER TRIGGER dbo.trg_sale_insert_update_ticket_stock
ON dbo.sale
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM dbo.ticket AS t
        INNER JOIN
        (
            SELECT
                ticket_id,
                SUM(CASE
                        WHEN sale_status NOT IN (N'Отменена', N'Возврат')
                        THEN ticket_quantity
                        ELSE 0
                    END) AS qty
            FROM inserted
            GROUP BY ticket_id
        ) AS d ON d.ticket_id = t.ticket_id
        WHERE d.qty > t.available_count
    )
    BEGIN
        THROW 51101, N'Продажа превышает доступный остаток билетов.', 1;
    END;

    ;WITH demand AS
    (
        SELECT
            ticket_id,
            SUM(CASE
                    WHEN sale_status NOT IN (N'Отменена', N'Возврат')
                    THEN ticket_quantity
                    ELSE 0
                END) AS qty
        FROM inserted
        GROUP BY ticket_id
    )
    UPDATE t
       SET available_count = t.available_count - d.qty
    FROM dbo.ticket AS t
    INNER JOIN demand AS d ON d.ticket_id = t.ticket_id
    WHERE d.qty > 0;

    ;WITH affected_schedules AS
    (
        SELECT DISTINCT schedule_id
        FROM inserted
        WHERE schedule_id IS NOT NULL
    )
    UPDATE rs
       SET available_seat_num = CAST
       (
           CASE
               WHEN b.capacity - COALESCE(sold.seats_sold, 0) > 0
               THEN b.capacity - COALESCE(sold.seats_sold, 0)
               ELSE 0
           END
           AS smallint
       )
    FROM dbo.route_schedule AS rs
    INNER JOIN affected_schedules AS a ON a.schedule_id = rs.schedule_id
    INNER JOIN dbo.bus AS b ON b.bus_id = rs.bus_id
    OUTER APPLY
    (
        SELECT SUM(s.ticket_quantity) AS seats_sold
        FROM dbo.sale AS s
        WHERE s.schedule_id = rs.schedule_id
          AND s.sale_status NOT IN (N'Отменена', N'Возврат')
    ) AS sold;
END;
GO

/* Sale UPDATE: recompute the inventory delta when status, ticket, or quantity
   changes. Cancelled/refunded sales do not consume inventory. */
CREATE OR ALTER TRIGGER dbo.trg_sale_update_ticket_stock
ON dbo.sale
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM dbo.ticket AS t
        INNER JOIN
        (
            SELECT ticket_id, SUM(qty_delta) AS qty_delta
            FROM
            (
                SELECT
                    ticket_id,
                    SUM(CASE
                            WHEN sale_status NOT IN (N'Отменена', N'Возврат')
                            THEN ticket_quantity
                            ELSE 0
                        END) AS qty_delta
                FROM deleted
                GROUP BY ticket_id

                UNION ALL

                SELECT
                    ticket_id,
                    -SUM(CASE
                             WHEN sale_status NOT IN (N'Отменена', N'Возврат')
                             THEN ticket_quantity
                             ELSE 0
                         END) AS qty_delta
                FROM inserted
                GROUP BY ticket_id
            ) AS change_rows
            GROUP BY ticket_id
        ) AS n ON n.ticket_id = t.ticket_id
        WHERE t.available_count + n.qty_delta < 0
    )
    BEGIN
        THROW 51102, N'Изменение продажи приводит к отрицательному остатку билетов.', 1;
    END;

    ;WITH delta AS
    (
        SELECT
            ticket_id,
            SUM(CASE
                    WHEN sale_status NOT IN (N'Отменена', N'Возврат')
                    THEN ticket_quantity
                    ELSE 0
                END) AS qty_delta
        FROM deleted
        GROUP BY ticket_id

        UNION ALL

        SELECT
            ticket_id,
            -SUM(CASE
                     WHEN sale_status NOT IN (N'Отменена', N'Возврат')
                     THEN ticket_quantity
                     ELSE 0
                 END) AS qty_delta
        FROM inserted
        GROUP BY ticket_id
    ),
    net AS
    (
        SELECT ticket_id, SUM(qty_delta) AS qty_delta
        FROM delta
        GROUP BY ticket_id
    )
    UPDATE t
       SET available_count = t.available_count + n.qty_delta
    FROM dbo.ticket AS t
    INNER JOIN net AS n ON n.ticket_id = t.ticket_id
    WHERE n.qty_delta <> 0;

    ;WITH affected_schedules AS
    (
        SELECT schedule_id FROM inserted WHERE schedule_id IS NOT NULL
        UNION
        SELECT schedule_id FROM deleted WHERE schedule_id IS NOT NULL
    )
    UPDATE rs
       SET available_seat_num = CAST
       (
           CASE
               WHEN b.capacity - COALESCE(sold.seats_sold, 0) > 0
               THEN b.capacity - COALESCE(sold.seats_sold, 0)
               ELSE 0
           END
           AS smallint
       )
    FROM dbo.route_schedule AS rs
    INNER JOIN affected_schedules AS a ON a.schedule_id = rs.schedule_id
    INNER JOIN dbo.bus AS b ON b.bus_id = rs.bus_id
    OUTER APPLY
    (
        SELECT SUM(s.ticket_quantity) AS seats_sold
        FROM dbo.sale AS s
        WHERE s.schedule_id = rs.schedule_id
          AND s.sale_status NOT IN (N'Отменена', N'Возврат')
    ) AS sold;
END;
GO

/* Sale DELETE: restore only inventory that the deleted sale had consumed. */
CREATE OR ALTER TRIGGER dbo.trg_sale_delete_restore_ticket_stock
ON dbo.sale
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH returned AS
    (
        SELECT
            ticket_id,
            SUM(CASE
                    WHEN sale_status NOT IN (N'Отменена', N'Возврат')
                    THEN ticket_quantity
                    ELSE 0
                END) AS qty
        FROM deleted
        GROUP BY ticket_id
    )
    UPDATE t
       SET available_count = t.available_count + r.qty
    FROM dbo.ticket AS t
    INNER JOIN returned AS r ON r.ticket_id = t.ticket_id
    WHERE r.qty > 0;

    ;WITH affected_schedules AS
    (
        SELECT DISTINCT schedule_id
        FROM deleted
        WHERE schedule_id IS NOT NULL
    )
    UPDATE rs
       SET available_seat_num = CAST
       (
           CASE
               WHEN b.capacity - COALESCE(sold.seats_sold, 0) > 0
               THEN b.capacity - COALESCE(sold.seats_sold, 0)
               ELSE 0
           END
           AS smallint
       )
    FROM dbo.route_schedule AS rs
    INNER JOIN affected_schedules AS a ON a.schedule_id = rs.schedule_id
    INNER JOIN dbo.bus AS b ON b.bus_id = rs.bus_id
    OUTER APPLY
    (
        SELECT SUM(s.ticket_quantity) AS seats_sold
        FROM dbo.sale AS s
        WHERE s.schedule_id = rs.schedule_id
          AND s.sale_status NOT IN (N'Отменена', N'Возврат')
    ) AS sold;
END;
GO

/* Mileage >= 250,000 km automatically moves a bus to Резерв unless
   an authorized user explicitly released it. The override is persisted so an
   ordinary edit does not silently put the bus back into reserve. */
CREATE OR ALTER TRIGGER dbo.trg_bus_update_mileage_check
ON dbo.bus
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE b
       SET status = N'Резерв'
    FROM dbo.bus AS b
    INNER JOIN inserted AS i ON i.bus_id = b.bus_id
    WHERE i.mileage_km >= 250000
      AND i.status = N'Исправен'
      AND i.high_mileage_override = 0
      AND b.high_mileage_override = 0
      AND b.status = N'Исправен';
END;
GO

/* Keep the independent disposed-bus rule here. The mileage rule is handled
   by trg_bus_update_mileage_check, avoiding contradictory AFTER triggers. */
CREATE OR ALTER TRIGGER dbo.trg_bus_update_validate_status
ON dbo.bus
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM inserted AS i
        INNER JOIN deleted AS d ON d.bus_id = i.bus_id
        WHERE d.status = N'Списан'
          AND i.status <> N'Списан'
    )
    BEGIN
        THROW 51108, N'Списанный автобус нельзя вернуть в эксплуатацию.', 1;
    END;
END;
GO

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER VIEW dbo.v_sales_by_channel
AS
SELECT
    sale_channel,
    COUNT(*) AS sale_count,
    SUM(ticket_quantity) AS ticket_count,
    ROUND(SUM(sale_price * ticket_quantity), 2) AS amount_total,
    AVG(sale_price) AS average_ticket_price
FROM dbo.sale
WHERE sale_status NOT IN (N'Отменена', N'Возврат')
GROUP BY sale_channel;
GO

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER VIEW dbo.v_sales_by_route
AS
SELECT
    r.route_num,
    r.route_name,
    COUNT(CASE
              WHEN s.sale_status NOT IN (N'Отменена', N'Возврат') THEN 1
          END) AS sale_count,
    COALESCE(SUM(CASE
                     WHEN s.sale_status NOT IN (N'Отменена', N'Возврат')
                     THEN s.ticket_quantity
                     ELSE 0
                 END), 0) AS ticket_count,
    ROUND
    (
        COALESCE
        (
            SUM(CASE
                    WHEN s.sale_status NOT IN (N'Отменена', N'Возврат')
                    THEN s.sale_price * s.ticket_quantity
                    ELSE 0
                END),
            0
        ),
        2
    ) AS amount_total
FROM dbo.route AS r
LEFT JOIN dbo.route_schedule AS rs ON rs.route_id = r.route_id
LEFT JOIN dbo.sale AS s ON s.schedule_id = rs.schedule_id
GROUP BY r.route_num, r.route_name;
GO

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER VIEW dbo.v_sales_by_employee
AS
SELECT
    e.employee_id,
    CONCAT(e.surname, N' ', e.name) AS employee_name,
    j.job_title,
    COUNT(CASE
              WHEN s.sale_status NOT IN (N'Отменена', N'Возврат') THEN 1
          END) AS sale_count,
    ROUND
    (
        COALESCE
        (
            SUM(CASE
                    WHEN s.sale_status NOT IN (N'Отменена', N'Возврат')
                    THEN s.sale_price * s.ticket_quantity
                    ELSE 0
                END),
            0
        ),
        2
    ) AS amount_total
FROM dbo.employee AS e
INNER JOIN dbo.job AS j ON j.job_id = e.job_id
LEFT JOIN dbo.sale AS s ON s.cashier_id = e.employee_id
GROUP BY e.employee_id, e.surname, e.name, j.job_title;
GO

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER VIEW dbo.v_autopark_summary
AS
SELECT
    (SELECT COUNT(*) FROM dbo.bus WHERE status <> N'Списан') AS active_bus_count,
    (SELECT COUNT(*) FROM dbo.employee WHERE status = N'Работает') AS active_employee_count,
    (SELECT COUNT(*) FROM dbo.route WHERE is_active = 1) AS active_route_count,
    (
        SELECT COUNT(*)
        FROM dbo.route_schedule
        WHERE service_date = CAST(GETDATE() AS date)
          AND schedule_status <> N'Отменен'
    ) AS today_schedule_count,
    (
        SELECT COUNT(*)
        FROM dbo.sale
        WHERE CAST(sale_date AS date) = CAST(GETDATE() AS date)
          AND sale_status NOT IN (N'Отменена', N'Возврат')
    ) AS today_sale_count,
    (
        SELECT ROUND(COALESCE(SUM(sale_price * ticket_quantity), 0), 2)
        FROM dbo.sale
        WHERE CAST(sale_date AS date) = CAST(GETDATE() AS date)
          AND sale_status NOT IN (N'Отменена', N'Возврат')
    ) AS today_amount_total,
    (
        SELECT COUNT(*)
        FROM dbo.v_latest_maintenance
        WHERE roadworthiness <> N'Исправен'
    ) AS bus_need_attention_count;
GO

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE dbo.create_sales_report
    @employee_id int,
    @report_type nvarchar(60),
    @date_from date,
    @date_to date
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.report
    (
        employee_id,
        report_date,
        report_type,
        total_sales,
        total_amount,
        file_path
    )
    SELECT
        @employee_id,
        CAST(GETDATE() AS date),
        @report_type,
        COUNT(CASE
                  WHEN sale_status NOT IN (N'Отменена', N'Возврат') THEN 1
              END),
        ROUND
        (
            COALESCE
            (
                SUM(CASE
                        WHEN sale_status NOT IN (N'Отменена', N'Возврат')
                        THEN sale_price * ticket_quantity
                        ELSE 0
                    END),
                0
            ),
            2
        ),
        NULL
    FROM dbo.sale
    WHERE CAST(sale_date AS date) BETWEEN @date_from AND @date_to;

    SELECT CAST(SCOPE_IDENTITY() AS int) AS report_id;
END;
GO

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE dbo.get_daily_sales_total
    @sale_date date
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        COUNT(CASE
                  WHEN sale_status NOT IN (N'Отменена', N'Возврат') THEN 1
              END) AS sale_count,
        COALESCE(SUM(CASE
                         WHEN sale_status NOT IN (N'Отменена', N'Возврат')
                         THEN ticket_quantity
                         ELSE 0
                     END), 0) AS ticket_count,
        ROUND
        (
            COALESCE
            (
                SUM(CASE
                        WHEN sale_status NOT IN (N'Отменена', N'Возврат')
                        THEN sale_price * ticket_quantity
                        ELSE 0
                    END),
                0
            ),
            2
        ) AS amount_total,
        AVG(CASE
                WHEN sale_status NOT IN (N'Отменена', N'Возврат')
                THEN sale_price
            END) AS average_price
    FROM dbo.sale
    WHERE CAST(sale_date AS date) = @sale_date;
END;
GO

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE dbo.get_route_sales
    @route_id int
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        r.route_num,
        r.route_name,
        COUNT(CASE
                  WHEN s.sale_status NOT IN (N'Отменена', N'Возврат') THEN 1
              END) AS sale_count,
        COALESCE(SUM(CASE
                         WHEN s.sale_status NOT IN (N'Отменена', N'Возврат')
                         THEN s.ticket_quantity
                         ELSE 0
                     END), 0) AS ticket_count,
        ROUND
        (
            COALESCE
            (
                SUM(CASE
                        WHEN s.sale_status NOT IN (N'Отменена', N'Возврат')
                        THEN s.sale_price * s.ticket_quantity
                        ELSE 0
                    END),
                0
            ),
            2
        ) AS amount_total
    FROM dbo.route AS r
    LEFT JOIN dbo.route_schedule AS rs ON rs.route_id = r.route_id
    LEFT JOIN dbo.sale AS s ON s.schedule_id = rs.schedule_id
    WHERE r.route_id = @route_id
    GROUP BY r.route_num, r.route_name;
END;
GO


/* Keep the cached seat count consistent regardless of whether a sale is
   created through the Web Forms repository or by executing add_sale directly.
   The application also checks capacity inside its transaction; these checks
   make the stored procedure a safe database entry point as well. */
CREATE OR ALTER PROCEDURE dbo.add_sale
    @schedule_id int = NULL,
    @ticket_id int,
    @cashier_id int = NULL,
    @sale_price decimal(10,2),
    @sale_channel nvarchar(30),
    @ticket_quantity int = 1,
    @sale_id bigint OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @ticket_quantity IS NULL OR @ticket_quantity <= 0
        THROW 51120, N'Количество билетов должно быть больше нуля.', 1;

    IF @sale_price IS NULL OR @sale_price < 0
        THROW 51121, N'Цена продажи не может быть отрицательной.', 1;

    IF @sale_channel IS NULL OR LTRIM(RTRIM(@sale_channel)) = N''
        THROW 51129, N'Не указан канал продажи.', 1;

    BEGIN TRANSACTION;

    DECLARE @schedule_status nvarchar(30),
            @bus_capacity int,
            @seats_sold int,
            @available_count int,
            @is_active bit;

    /* Lock the schedule before the ticket row. Web Forms already uses this
       lock order when selling for a scheduled trip. */
    IF @schedule_id IS NOT NULL
    BEGIN
        SELECT
            @schedule_status = rs.schedule_status,
            @bus_capacity = b.capacity
        FROM dbo.route_schedule AS rs WITH (UPDLOCK, HOLDLOCK)
        INNER JOIN dbo.bus AS b ON b.bus_id = rs.bus_id
        WHERE rs.schedule_id = @schedule_id;

        IF @schedule_status IS NULL
            THROW 51125, N'Указанное расписание не существует.', 1;

        IF @schedule_status NOT IN (N'Запланирован', N'В пути')
            THROW 51130, N'Нельзя продавать билеты на отменённый или завершённый рейс.', 1;

        SELECT @seats_sold = COALESCE(SUM(s.ticket_quantity), 0)
        FROM dbo.sale AS s
        WHERE s.schedule_id = @schedule_id
          AND s.sale_status NOT IN (N'Отменена', N'Возврат');

        IF @seats_sold + @ticket_quantity > @bus_capacity
            THROW 51131, N'Для рейса недостаточно свободных мест.', 1;
    END;

    SELECT
        @available_count = available_count,
        @is_active = is_active
    FROM dbo.ticket WITH (UPDLOCK, HOLDLOCK)
    WHERE ticket_id = @ticket_id;

    IF @is_active IS NULL
        THROW 51122, N'Указанный билет не существует.', 1;

    IF @is_active = 0
        THROW 51123, N'Нельзя продавать неактивный билет.', 1;

    IF @available_count < @ticket_quantity
        THROW 51124, N'Недостаточно билетов на остатке.', 1;

    INSERT INTO dbo.sale
    (
        schedule_id,
        ticket_id,
        cashier_id,
        sale_price,
        sale_status,
        payment_status,
        sale_channel,
        ticket_quantity
    )
    VALUES
    (
        @schedule_id,
        @ticket_id,
        @cashier_id,
        @sale_price,
        CASE WHEN @sale_price * @ticket_quantity = 0 THEN N'Завершена' ELSE N'Создана' END,
        CASE WHEN @sale_price * @ticket_quantity = 0 THEN N'Оплачена' ELSE N'Ожидает' END,
        @sale_channel,
        @ticket_quantity
    );

    SET @sale_id = CAST(SCOPE_IDENTITY() AS bigint);

    IF @schedule_id IS NOT NULL
    BEGIN
        UPDATE rs
           SET available_seat_num = CAST
           (
               CASE
                   WHEN b.capacity - COALESCE(sold.seats_sold, 0) > 0
                   THEN b.capacity - COALESCE(sold.seats_sold, 0)
                   ELSE 0
               END
               AS smallint
           )
        FROM dbo.route_schedule AS rs
        INNER JOIN dbo.bus AS b ON b.bus_id = rs.bus_id
        OUTER APPLY
        (
            SELECT SUM(s.ticket_quantity) AS seats_sold
            FROM dbo.sale AS s
            WHERE s.schedule_id = rs.schedule_id
              AND s.sale_status NOT IN (N'Отменена', N'Возврат')
        ) AS sold
        WHERE rs.schedule_id = @schedule_id;
    END;

    COMMIT TRANSACTION;

    SELECT @sale_id AS sale_id;
END;
GO

/* Never accept a new payment for a cancelled or refunded sale. The sale row
   remains locked for the entire check/insert transaction to serialize payment
   attempts against the same sale. */
CREATE OR ALTER PROCEDURE dbo.add_payment
    @sale_id bigint,
    @amount decimal(10,2),
    @payment_method nvarchar(40),
    @transaction_id varchar(80) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @amount IS NULL OR @amount <= 0
        THROW 51126, N'Сумма платежа должна быть больше нуля.', 1;

    IF @payment_method IS NULL OR LTRIM(RTRIM(@payment_method)) = N''
        THROW 51132, N'Не указан способ оплаты.', 1;

    BEGIN TRANSACTION;

    DECLARE @sale_total decimal(14,2),
            @already_paid decimal(14,2),
            @sale_status nvarchar(30);

    SELECT
        @sale_total = s.sale_price * s.ticket_quantity,
        @sale_status = s.sale_status
    FROM dbo.sale AS s WITH (UPDLOCK, HOLDLOCK)
    WHERE s.sale_id = @sale_id;

    IF @sale_total IS NULL
        THROW 51127, N'Продажа не найдена.', 1;

    IF @sale_status IN (N'Отменена', N'Возврат')
        THROW 51133, N'Нельзя принимать оплату за отменённую или возвращённую продажу.', 1;

    SELECT @already_paid = COALESCE(SUM(p.amount), 0)
    FROM dbo.payment AS p
    WHERE p.sale_id = @sale_id
      AND p.payment_status = N'Успешно';

    IF @already_paid + @amount > @sale_total
        THROW 51128, N'Сумма платежей превышает стоимость продажи.', 1;

    INSERT INTO dbo.payment
    (
        sale_id,
        amount,
        payment_method,
        payment_status,
        transaction_id
    )
    VALUES
    (
        @sale_id,
        @amount,
        @payment_method,
        N'Успешно',
        @transaction_id
    );

    DECLARE @payment_id bigint = CAST(SCOPE_IDENTITY() AS bigint);

    COMMIT TRANSACTION;

    SELECT @payment_id AS payment_id;
END;
GO

/* The UI passes an inclusive end timestamp (23:59:59). Keep this procedure's
   contract inclusive; the report page has a separate, half-open date range. */
CREATE OR ALTER PROCEDURE dbo.get_sales_by_date_range
    @date_from datetime2(0),
    @date_to datetime2(0)
AS
BEGIN
    SET NOCOUNT ON;

    IF @date_from IS NULL OR @date_to IS NULL OR @date_to < @date_from
        THROW 51134, N'Некорректный диапазон дат.', 1;

    SELECT *
    FROM dbo.v_sale_details
    WHERE sale_date >= @date_from
      AND sale_date <= @date_to
    ORDER BY sale_date;
END;
GO

/* Synchronize the bus summary from the remaining maintenance history after
   INSERT, UPDATE, and DELETE. The date-default trigger remains responsible for
   next_maintenance_date; this trigger owns bus mileage/status synchronization. */
CREATE OR ALTER TRIGGER dbo.trg_maintenance_insert_sync_bus
ON dbo.maintenance
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @affectedBusIds TABLE
    (
        bus_id int NOT NULL PRIMARY KEY
    );

    INSERT INTO @affectedBusIds (bus_id)
    SELECT bus_id FROM inserted
    UNION
    SELECT bus_id FROM deleted;

    IF NOT EXISTS (SELECT 1 FROM @affectedBusIds)
        RETURN;

    ;WITH maintenance_mileage AS
    (
        SELECT m.bus_id, MAX(m.mileage_km) AS max_mileage
        FROM dbo.maintenance AS m
        INNER JOIN @affectedBusIds AS affected ON affected.bus_id = m.bus_id
        GROUP BY m.bus_id
    ),
    ranked_maintenance AS
    (
        SELECT
            m.bus_id,
            m.roadworthiness,
            ROW_NUMBER() OVER
            (
                PARTITION BY m.bus_id
                ORDER BY m.maintenance_date DESC, m.maintenance_id DESC
            ) AS row_num
        FROM dbo.maintenance AS m
        INNER JOIN @affectedBusIds AS affected ON affected.bus_id = m.bus_id
    ),
    latest_maintenance AS
    (
        SELECT bus_id, roadworthiness
        FROM ranked_maintenance
        WHERE row_num = 1
    )
    UPDATE b
       SET mileage_km =
               CASE
                   WHEN mm.max_mileage > b.mileage_km THEN mm.max_mileage
                   ELSE b.mileage_km
               END,
           status =
               CASE
                   WHEN b.status = N'Списан' THEN b.status
                   WHEN lm.roadworthiness IN (N'Неисправен', N'Требует внимания')
                       THEN N'На ремонте'
                   WHEN lm.roadworthiness = N'Исправен' AND b.status = N'На ремонте'
                       THEN N'Исправен'
                   ELSE b.status
               END
    FROM dbo.bus AS b
    INNER JOIN @affectedBusIds AS affected ON affected.bus_id = b.bus_id
    LEFT JOIN maintenance_mileage AS mm ON mm.bus_id = b.bus_id
    LEFT JOIN latest_maintenance AS lm ON lm.bus_id = b.bus_id;
END;
GO


/* Complete a sale when its successful payments cover the full amount.
   Re-evaluate both fields after payment INSERT, UPDATE, or DELETE; partial
   payments leave the sale in Создана/Ожидает. Refund/cancellation states are
   terminal and are not overwritten by payment recalculation. */
CREATE OR ALTER TRIGGER dbo.trg_payment_insert_sync_sale_status
ON dbo.payment
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE s
       SET payment_status =
               CASE
                   WHEN s.sale_status = N'Возврат' THEN N'Возврат'
                   WHEN COALESCE(p.paid_total, 0) >= s.sale_price * s.ticket_quantity
                       THEN N'Оплачена'
                   ELSE N'Ожидает'
               END,
           sale_status =
               CASE
                   WHEN s.sale_status IN (N'Отменена', N'Возврат') THEN s.sale_status
                   WHEN COALESCE(p.paid_total, 0) >= s.sale_price * s.ticket_quantity
                       THEN N'Завершена'
                   WHEN s.sale_status = N'Завершена' THEN N'Создана'
                   ELSE s.sale_status
               END
    FROM dbo.sale AS s
    INNER JOIN (SELECT DISTINCT sale_id FROM inserted) AS affected
        ON affected.sale_id = s.sale_id
    OUTER APPLY
    (
        SELECT SUM(amount) AS paid_total
        FROM dbo.payment AS p
        WHERE p.sale_id = s.sale_id
          AND p.payment_status = N'Успешно'
    ) AS p;
END;
GO

CREATE OR ALTER TRIGGER dbo.trg_payment_update_sync_sale_status
ON dbo.payment
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE s
       SET payment_status =
               CASE
                   WHEN s.sale_status = N'Возврат' THEN N'Возврат'
                   WHEN COALESCE(p.paid_total, 0) >= s.sale_price * s.ticket_quantity
                       THEN N'Оплачена'
                   ELSE N'Ожидает'
               END,
           sale_status =
               CASE
                   WHEN s.sale_status IN (N'Отменена', N'Возврат') THEN s.sale_status
                   WHEN COALESCE(p.paid_total, 0) >= s.sale_price * s.ticket_quantity
                       THEN N'Завершена'
                   WHEN s.sale_status = N'Завершена' THEN N'Создана'
                   ELSE s.sale_status
               END
    FROM dbo.sale AS s
    INNER JOIN
    (
        SELECT sale_id FROM inserted
        UNION
        SELECT sale_id FROM deleted
    ) AS affected
        ON affected.sale_id = s.sale_id
    OUTER APPLY
    (
        SELECT SUM(amount) AS paid_total
        FROM dbo.payment AS p
        WHERE p.sale_id = s.sale_id
          AND p.payment_status = N'Успешно'
    ) AS p;
END;
GO

CREATE OR ALTER TRIGGER dbo.trg_payment_delete_sync_sale_status
ON dbo.payment
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE s
       SET payment_status =
               CASE
                   WHEN s.sale_status = N'Возврат' THEN N'Возврат'
                   WHEN COALESCE(p.paid_total, 0) >= s.sale_price * s.ticket_quantity
                       THEN N'Оплачена'
                   ELSE N'Ожидает'
               END,
           sale_status =
               CASE
                   WHEN s.sale_status IN (N'Отменена', N'Возврат') THEN s.sale_status
                   WHEN COALESCE(p.paid_total, 0) >= s.sale_price * s.ticket_quantity
                       THEN N'Завершена'
                   WHEN s.sale_status = N'Завершена' THEN N'Создана'
                   ELSE s.sale_status
               END
    FROM dbo.sale AS s
    INNER JOIN (SELECT DISTINCT sale_id FROM deleted) AS affected
        ON affected.sale_id = s.sale_id
    OUTER APPLY
    (
        SELECT SUM(amount) AS paid_total
        FROM dbo.payment AS p
        WHERE p.sale_id = s.sale_id
          AND p.payment_status = N'Успешно'
    ) AS p;
END;
GO


/* Idempotent reconciliation of derived values already stored before this
   patch was installed. This does not rewrite payment or maintenance history. */
;WITH successful_payments AS
(
    SELECT
        p.sale_id,
        SUM(p.amount) AS paid_total
    FROM dbo.payment AS p
    WHERE p.payment_status = N'Успешно'
    GROUP BY p.sale_id
)
UPDATE s
   SET payment_status =
           CASE
               WHEN s.sale_status = N'Возврат' THEN N'Возврат'
               WHEN s.sale_status = N'Отменена' THEN s.payment_status
               WHEN s.sale_price * s.ticket_quantity = 0
                    OR COALESCE(p.paid_total, 0) >= s.sale_price * s.ticket_quantity
                   THEN N'Оплачена'
               ELSE N'Ожидает'
           END,
       sale_status =
           CASE
               WHEN s.sale_status IN (N'Отменена', N'Возврат') THEN s.sale_status
               WHEN s.sale_price * s.ticket_quantity = 0
                    OR COALESCE(p.paid_total, 0) >= s.sale_price * s.ticket_quantity
                   THEN N'Завершена'
               WHEN s.sale_status = N'Завершена' THEN N'Создана'
               ELSE s.sale_status
           END
FROM dbo.sale AS s
LEFT JOIN successful_payments AS p ON p.sale_id = s.sale_id;
GO

/* Recalculate cached free seats from the current sale rows. */
UPDATE rs
   SET available_seat_num = CAST
   (
       CASE
           WHEN b.capacity - COALESCE(sold.seats_sold, 0) > 0
           THEN b.capacity - COALESCE(sold.seats_sold, 0)
           ELSE 0
       END
       AS smallint
   )
FROM dbo.route_schedule AS rs
INNER JOIN dbo.bus AS b ON b.bus_id = rs.bus_id
OUTER APPLY
(
    SELECT SUM(s.ticket_quantity) AS seats_sold
    FROM dbo.sale AS s
    WHERE s.schedule_id = rs.schedule_id
      AND s.sale_status NOT IN (N'Отменена', N'Возврат')
) AS sold;
GO

/* Synchronize bus mileage/status once using the entire current maintenance
   history, not just rows inserted after this patch. Keep odometer mileage
   monotonic, and do not revive buses already marked as scrapped. */
;WITH maintenance_mileage AS
(
    SELECT bus_id, MAX(mileage_km) AS max_mileage
    FROM dbo.maintenance
    GROUP BY bus_id
),
ranked_maintenance AS
(
    SELECT
        bus_id,
        roadworthiness,
        ROW_NUMBER() OVER
        (
            PARTITION BY bus_id
            ORDER BY maintenance_date DESC, maintenance_id DESC
        ) AS row_num
    FROM dbo.maintenance
),
latest_maintenance AS
(
    SELECT bus_id, roadworthiness
    FROM ranked_maintenance
    WHERE row_num = 1
)
UPDATE b
   SET mileage_km =
           CASE
               WHEN mm.max_mileage > b.mileage_km THEN mm.max_mileage
               ELSE b.mileage_km
           END,
       status =
           CASE
               WHEN b.status = N'Списан' THEN b.status
               WHEN lm.roadworthiness IN (N'Неисправен', N'Требует внимания')
                   THEN N'На ремонте'
               WHEN lm.roadworthiness = N'Исправен' AND b.status = N'На ремонте'
                   THEN N'Исправен'
               ELSE b.status
           END
FROM dbo.bus AS b
LEFT JOIN maintenance_mileage AS mm ON mm.bus_id = b.bus_id
LEFT JOIN latest_maintenance AS lm ON lm.bus_id = b.bus_id
WHERE
    b.mileage_km <> CASE
                        WHEN mm.max_mileage > b.mileage_km THEN mm.max_mileage
                        ELSE b.mileage_km
                    END
    OR b.status <> CASE
                       WHEN b.status = N'Списан' THEN b.status
                       WHEN lm.roadworthiness IN (N'Неисправен', N'Требует внимания')
                           THEN N'На ремонте'
                       WHEN lm.roadworthiness = N'Исправен' AND b.status = N'На ремонте'
                           THEN N'Исправен'
                       ELSE b.status
                   END;
GO

/* Ensure high-mileage operational buses that predate the trigger correction
   are brought to the same reserve state as future updates. */
UPDATE dbo.bus
   SET status = N'Резерв'
WHERE status = N'Исправен'
  AND mileage_km >= 250000
  AND high_mileage_override = 0;
GO
