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

    This patch changes database object definitions, not existing business
    rows. It does not reconstruct ticket.available_count values already
    affected by past cancellations; review those separately if necessary.

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
END;
GO

/* Mileage >= 250,000 km has one consistent outcome: a bus marked Исправен is
   automatically moved to Резерв. Check this on any update, not only when the
   mileage column is named in the UPDATE statement. */
CREATE OR ALTER TRIGGER dbo.trg_bus_update_mileage_check
ON dbo.bus
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE b
       SET status = N'Резерв'
    FROM dbo.bus AS b
    INNER JOIN inserted AS i ON i.bus_id = b.bus_id
    WHERE i.mileage_km >= 250000
      AND i.status = N'Исправен'
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
