/*
    AUTOPARKDB - курсовой проект по дисциплине «Системы управления базами данных».

    Модель ориентирована на тему «Разработка программы для работы автопарка».
    Учетная запись приложения и сотрудник предприятия разделены.

    SQL Server / T-SQL.
*/

USE master;
GO

IF DB_ID(N'AutoparkDB') IS NOT NULL
BEGIN
    ALTER DATABASE [AutoparkDB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [AutoparkDB];
END;
GO

CREATE DATABASE [AutoparkDB];
GO

USE [AutoparkDB];
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_PADDING ON;
GO

/* ================================================================
   1. ОСНОВНЫЕ ТАБЛИЦЫ ПРЕДМЕТНОЙ ОБЛАСТИ
   ================================================================ */

-- Организационные подразделения автопарка.
CREATE TABLE dbo.department
(
    department_id   int IDENTITY(1,1) NOT NULL,
    department_name nvarchar(100) NOT NULL,
    department_code varchar(20) NOT NULL,
    description     nvarchar(500) NULL,
    is_active       bit NOT NULL CONSTRAINT df_department_is_active DEFAULT (1),
    CONSTRAINT pk_department PRIMARY KEY (department_id),
    CONSTRAINT uq_department_code UNIQUE (department_code),
    CONSTRAINT ck_department_name CHECK (LEN(LTRIM(RTRIM(department_name))) >= 3)
);
GO

-- Должности сотрудников.
CREATE TABLE dbo.job
(
    job_id      int IDENTITY(1,1) NOT NULL,
    job_title   nvarchar(100) NOT NULL,
    internship  nvarchar(200) NULL,
    is_active   bit NOT NULL CONSTRAINT df_job_is_active DEFAULT (1),
    CONSTRAINT pk_job PRIMARY KEY (job_id),
    CONSTRAINT uq_job_title UNIQUE (job_title)
);
GO

-- Сотрудники предприятия.
CREATE TABLE dbo.employee
(
    employee_id   int IDENTITY(1,1) NOT NULL,
    surname       nvarchar(60) NOT NULL,
    name          nvarchar(60) NOT NULL,
    patronym      nvarchar(60) NULL,
    employed_date date NOT NULL,
    job_id        int NOT NULL,
    department_id int NULL,
    phone         varchar(30) NULL,
    email         varchar(120) NULL,
    status        nvarchar(30) NOT NULL CONSTRAINT df_employee_status DEFAULT (N'Работает'),
    CONSTRAINT pk_employee PRIMARY KEY (employee_id),
    CONSTRAINT fk_employee_job FOREIGN KEY (job_id) REFERENCES dbo.job(job_id),
    CONSTRAINT fk_employee_department FOREIGN KEY (department_id) REFERENCES dbo.department(department_id),
    CONSTRAINT ck_employee_status CHECK (status IN (N'Работает', N'Отпуск', N'Больничный', N'Уволен')),
    CONSTRAINT ck_employee_employed_date CHECK (employed_date <= CAST(GETDATE() AS date))
);
GO

-- Автобусы автопарка.
CREATE TABLE dbo.bus
(
    bus_id            int IDENTITY(1,1) NOT NULL,
    fleet_number      varchar(20) NOT NULL,
    registration_num  varchar(20) NOT NULL,
    model             nvarchar(80) NOT NULL,
    manufacturer      nvarchar(80) NOT NULL,
    manufacture_year  smallint NOT NULL,
    capacity          smallint NOT NULL,
    status            nvarchar(30) NOT NULL CONSTRAINT df_bus_status DEFAULT (N'Исправен'),
    mileage_km        int NOT NULL CONSTRAINT df_bus_mileage DEFAULT (0),
    CONSTRAINT pk_bus PRIMARY KEY (bus_id),
    CONSTRAINT uq_bus_fleet_number UNIQUE (fleet_number),
    CONSTRAINT uq_bus_registration_num UNIQUE (registration_num),
    CONSTRAINT ck_bus_year CHECK (manufacture_year BETWEEN 1980 AND 2100),
    CONSTRAINT ck_bus_capacity CHECK (capacity > 0),
    CONSTRAINT ck_bus_mileage CHECK (mileage_km >= 0),
    CONSTRAINT ck_bus_status CHECK (status IN (N'Исправен', N'На ремонте', N'Списан', N'Резерв'))
);
GO

-- Маршруты.
CREATE TABLE dbo.route
(
    route_id   int IDENTITY(1,1) NOT NULL,
    route_num  varchar(20) NOT NULL,
    route_name nvarchar(120) NOT NULL,
    start_stop nvarchar(120) NOT NULL,
    end_stop   nvarchar(120) NOT NULL,
    is_active  bit NOT NULL CONSTRAINT df_route_is_active DEFAULT (1),
    CONSTRAINT pk_route PRIMARY KEY (route_id),
    CONSTRAINT uq_route_num UNIQUE (route_num),
    CONSTRAINT ck_route_endpoints CHECK (start_stop <> end_stop)
);
GO

-- Остановки.
CREATE TABLE dbo.stop
(
    stop_id     int IDENTITY(1,1) NOT NULL,
    stop_name   nvarchar(120) NOT NULL,
    location    nvarchar(200) NULL,
    latitude    decimal(9,6) NULL,
    longitude   decimal(9,6) NULL,
    is_active   bit NOT NULL CONSTRAINT df_stop_is_active DEFAULT (1),
    CONSTRAINT pk_stop PRIMARY KEY (stop_id),
    CONSTRAINT uq_stop_name UNIQUE (stop_name),
    CONSTRAINT ck_stop_latitude CHECK (latitude IS NULL OR latitude BETWEEN -90 AND 90),
    CONSTRAINT ck_stop_longitude CHECK (longitude IS NULL OR longitude BETWEEN -180 AND 180)
);
GO

-- Связь M:N между маршрутами и остановками.
CREATE TABLE dbo.route_stop
(
    route_id      int NOT NULL,
    stop_id       int NOT NULL,
    stop_sequence smallint NOT NULL,
    distance_km   decimal(7,2) NULL,
    arrival_offset_min smallint NULL,
    CONSTRAINT pk_route_stop PRIMARY KEY (route_id, stop_id),
    CONSTRAINT uq_route_stop_sequence UNIQUE (route_id, stop_sequence),
    CONSTRAINT fk_route_stop_route FOREIGN KEY (route_id) REFERENCES dbo.route(route_id),
    CONSTRAINT fk_route_stop_stop FOREIGN KEY (stop_id) REFERENCES dbo.stop(stop_id),
    CONSTRAINT ck_route_stop_sequence CHECK (stop_sequence > 0),
    CONSTRAINT ck_route_stop_distance CHECK (distance_km IS NULL OR distance_km >= 0),
    CONSTRAINT ck_route_stop_offset CHECK (arrival_offset_min IS NULL OR arrival_offset_min >= 0)
);
GO

-- Расписание рейсов.
CREATE TABLE dbo.route_schedule
(
    schedule_id       int IDENTITY(1,1) NOT NULL,
    route_id           int NOT NULL,
    bus_id             int NOT NULL,
    driver_id          int NOT NULL,
    service_date       date NOT NULL,
    departure_time     time(0) NOT NULL,
    arrival_time       time(0) NOT NULL,
    available_seat_num smallint NOT NULL,
    schedule_status    nvarchar(30) NOT NULL CONSTRAINT df_route_schedule_status DEFAULT (N'Запланирован'),
    created_at         datetime2(0) NOT NULL CONSTRAINT df_route_schedule_created_at DEFAULT (SYSDATETIME()),
    CONSTRAINT pk_route_schedule PRIMARY KEY (schedule_id),
    CONSTRAINT fk_route_schedule_route FOREIGN KEY (route_id) REFERENCES dbo.route(route_id),
    CONSTRAINT fk_route_schedule_bus FOREIGN KEY (bus_id) REFERENCES dbo.bus(bus_id),
    CONSTRAINT fk_route_schedule_driver FOREIGN KEY (driver_id) REFERENCES dbo.employee(employee_id),
    CONSTRAINT uq_route_schedule UNIQUE (route_id, bus_id, service_date, departure_time),
    CONSTRAINT ck_route_schedule_seats CHECK (available_seat_num >= 0),
    CONSTRAINT ck_route_schedule_time CHECK (arrival_time > departure_time),
    CONSTRAINT ck_route_schedule_status CHECK (schedule_status IN (N'Запланирован', N'Выполнен', N'Отменен', N'В пути'))
);
GO

-- Учет технического обслуживания автобусов.
CREATE TABLE dbo.maintenance
(
    maintenance_id       int IDENTITY(1,1) NOT NULL,
    bus_id               int NOT NULL,
    employee_id          int NULL,
    maintenance_date     date NOT NULL,
    next_maintenance_date date NULL,
    maintenance_type    nvarchar(100) NOT NULL,
    found_issue          nvarchar(500) NULL,
    service_result       nvarchar(500) NULL,
    mileage_km           int NULL,
    roadworthiness       nvarchar(30) NOT NULL CONSTRAINT df_maintenance_roadworthiness DEFAULT (N'Исправен'),
    maintenance_cost    decimal(12,2) NOT NULL CONSTRAINT df_maintenance_cost DEFAULT (0),
    CONSTRAINT pk_maintenance PRIMARY KEY (maintenance_id),
    CONSTRAINT fk_maintenance_bus FOREIGN KEY (bus_id) REFERENCES dbo.bus(bus_id),
    CONSTRAINT fk_maintenance_employee FOREIGN KEY (employee_id) REFERENCES dbo.employee(employee_id),
    CONSTRAINT ck_maintenance_cost CHECK (maintenance_cost >= 0),
    CONSTRAINT ck_maintenance_mileage CHECK (mileage_km IS NULL OR mileage_km >= 0),
    CONSTRAINT ck_maintenance_date CHECK (next_maintenance_date IS NULL OR next_maintenance_date >= maintenance_date),
    CONSTRAINT ck_maintenance_status CHECK (roadworthiness IN (N'Исправен', N'Требует внимания', N'Неисправен'))
);
GO

-- Типы билетов.
CREATE TABLE dbo.ticket
(
    ticket_id        int IDENTITY(1,1) NOT NULL,
    ticket_name      nvarchar(120) NOT NULL,
    ticket_type      nvarchar(40) NOT NULL,
    zone             nvarchar(40) NULL,
    price            decimal(10,2) NOT NULL,
    valid_days       smallint NOT NULL,
    available_count  int NOT NULL CONSTRAINT df_ticket_available_count DEFAULT (0),
    issue_date       date NOT NULL CONSTRAINT df_ticket_issue_date DEFAULT (CAST(GETDATE() AS date)),
    expiry_date      date NULL,
    is_active        bit NOT NULL CONSTRAINT df_ticket_is_active DEFAULT (1),
    CONSTRAINT pk_ticket PRIMARY KEY (ticket_id),
    CONSTRAINT uq_ticket_name UNIQUE (ticket_name),
    CONSTRAINT ck_ticket_price CHECK (price >= 0),
    CONSTRAINT ck_ticket_valid_days CHECK (valid_days > 0),
    CONSTRAINT ck_ticket_available_count CHECK (available_count >= 0),
    CONSTRAINT ck_ticket_expiry CHECK (expiry_date IS NULL OR expiry_date >= issue_date)
);
GO

-- Продажи билетов. Идентификация покупателя в записи продажи не является обязательной.
CREATE TABLE dbo.sale
(
    sale_id          bigint IDENTITY(1,1) NOT NULL,
    schedule_id      int NULL,
    ticket_id        int NOT NULL,
    cashier_id       int NULL,
    sale_date        datetime2(0) NOT NULL CONSTRAINT df_sale_date DEFAULT (SYSDATETIME()),
    sale_price       decimal(10,2) NOT NULL,
    sale_status      nvarchar(30) NOT NULL CONSTRAINT df_sale_status DEFAULT (N'Создана'),
    payment_status   nvarchar(30) NOT NULL CONSTRAINT df_sale_payment_status DEFAULT (N'Ожидает'),
    sale_channel     nvarchar(30) NOT NULL,
    ticket_quantity  int NOT NULL CONSTRAINT df_sale_ticket_quantity DEFAULT (1),
    CONSTRAINT pk_sale PRIMARY KEY (sale_id),
    CONSTRAINT fk_sale_schedule FOREIGN KEY (schedule_id) REFERENCES dbo.route_schedule(schedule_id),
    CONSTRAINT fk_sale_ticket FOREIGN KEY (ticket_id) REFERENCES dbo.ticket(ticket_id),
    CONSTRAINT fk_sale_cashier FOREIGN KEY (cashier_id) REFERENCES dbo.employee(employee_id),
    CONSTRAINT ck_sale_price CHECK (sale_price >= 0),
    CONSTRAINT ck_sale_quantity CHECK (ticket_quantity > 0),
    CONSTRAINT ck_sale_status CHECK (sale_status IN (N'Создана', N'Завершена', N'Отменена', N'Возврат')),
    CONSTRAINT ck_sale_payment_status CHECK (payment_status IN (N'Ожидает', N'Оплачена', N'Ошибка', N'Возврат')),
    CONSTRAINT ck_sale_channel CHECK (sale_channel IN (N'Касса', N'Кондуктор', N'Валидатор', N'QR', N'Онлайн'))
);
GO

-- Платежи.
CREATE TABLE dbo.payment
(
    payment_id      bigint IDENTITY(1,1) NOT NULL,
    sale_id         bigint NOT NULL,
    payment_date    datetime2(0) NOT NULL CONSTRAINT df_payment_date DEFAULT (SYSDATETIME()),
    amount          decimal(10,2) NOT NULL,
    payment_method  nvarchar(40) NOT NULL,
    payment_status  nvarchar(30) NOT NULL CONSTRAINT df_payment_status DEFAULT (N'Успешно'),
    transaction_id  varchar(80) NULL,
    CONSTRAINT pk_payment PRIMARY KEY (payment_id),
    CONSTRAINT fk_payment_sale FOREIGN KEY (sale_id) REFERENCES dbo.sale(sale_id),
    CONSTRAINT uq_payment_transaction UNIQUE (transaction_id),
    CONSTRAINT ck_payment_amount CHECK (amount >= 0),
    CONSTRAINT ck_payment_status CHECK (payment_status IN (N'Успешно', N'Ошибка', N'Возврат', N'Ожидает'))
);
GO

-- Отчеты, сформированные сотрудниками.
CREATE TABLE dbo.report
(
    report_id       int IDENTITY(1,1) NOT NULL,
    employee_id     int NOT NULL,
    report_date     date NOT NULL,
    report_type     nvarchar(60) NOT NULL,
    created_at      datetime2(0) NOT NULL CONSTRAINT df_report_created_at DEFAULT (SYSDATETIME()),
    total_sales     int NOT NULL CONSTRAINT df_report_total_sales DEFAULT (0),
    total_amount    decimal(14,2) NOT NULL CONSTRAINT df_report_total_amount DEFAULT (0),
    file_path       nvarchar(500) NULL,
    CONSTRAINT pk_report PRIMARY KEY (report_id),
    CONSTRAINT fk_report_employee FOREIGN KEY (employee_id) REFERENCES dbo.employee(employee_id),
    CONSTRAINT ck_report_sales CHECK (total_sales >= 0),
    CONSTRAINT ck_report_amount CHECK (total_amount >= 0)
);
GO

-- Системные пользователи.
CREATE TABLE dbo.app_user
(
    user_id        int IDENTITY(1,1) NOT NULL,
    employee_id    int NULL,
    login          varchar(80) NOT NULL,
    password_hash  varchar(256) NOT NULL,
    email          varchar(150) NULL,
    phone_number   varchar(30) NULL,
    is_active      bit NOT NULL CONSTRAINT df_app_user_is_active DEFAULT (1),
    created_at     datetime2(0) NOT NULL CONSTRAINT df_app_user_created_at DEFAULT (SYSDATETIME()),
    last_login_at  datetime2(0) NULL,
    CONSTRAINT pk_app_user PRIMARY KEY (user_id),
    CONSTRAINT uq_app_user_login UNIQUE (login),
    CONSTRAINT uq_app_user_employee UNIQUE (employee_id),
    CONSTRAINT fk_app_user_employee FOREIGN KEY (employee_id) REFERENCES dbo.employee(employee_id)
);
GO

-- Роли приложения.
CREATE TABLE dbo.app_role
(
    role_id       int IDENTITY(1,1) NOT NULL,
    role_name     varchar(50) NOT NULL,
    description   nvarchar(300) NULL,
    is_active     bit NOT NULL CONSTRAINT df_app_role_is_active DEFAULT (1),
    CONSTRAINT pk_app_role PRIMARY KEY (role_id),
    CONSTRAINT uq_app_role_name UNIQUE (role_name)
);
GO

-- Разрешения приложения.
CREATE TABLE dbo.permission
(
    permission_id int IDENTITY(1,1) NOT NULL,
    permission_name varchar(100) NOT NULL,
    description   nvarchar(300) NULL,
    CONSTRAINT pk_permission PRIMARY KEY (permission_id),
    CONSTRAINT uq_permission_name UNIQUE (permission_name)
);
GO

-- Связь M:N пользователь-роли.
CREATE TABLE dbo.user_role
(
    user_id     int NOT NULL,
    role_id     int NOT NULL,
    assigned_at datetime2(0) NOT NULL CONSTRAINT df_user_role_assigned_at DEFAULT (SYSDATETIME()),
    assigned_by varchar(80) NULL,
    CONSTRAINT pk_user_role PRIMARY KEY (user_id, role_id),
    CONSTRAINT fk_user_role_user FOREIGN KEY (user_id) REFERENCES dbo.app_user(user_id),
    CONSTRAINT fk_user_role_role FOREIGN KEY (role_id) REFERENCES dbo.app_role(role_id)
);
GO

-- Связь M:N роль-разрешение.
CREATE TABLE dbo.role_permission
(
    role_id       int NOT NULL,
    permission_id int NOT NULL,
    granted_at    datetime2(0) NOT NULL CONSTRAINT df_role_permission_granted_at DEFAULT (SYSDATETIME()),
    CONSTRAINT pk_role_permission PRIMARY KEY (role_id, permission_id),
    CONSTRAINT fk_role_permission_role FOREIGN KEY (role_id) REFERENCES dbo.app_role(role_id),
    CONSTRAINT fk_role_permission_permission FOREIGN KEY (permission_id) REFERENCES dbo.permission(permission_id)
);
GO

-- Документы сотрудников.
CREATE TABLE dbo.employee_document
(
    document_id   int IDENTITY(1,1) NOT NULL,
    employee_id   int NOT NULL,
    document_type nvarchar(80) NOT NULL,
    document_num  varchar(80) NOT NULL,
    issue_date    date NULL,
    expiry_date   date NULL,
    issued_by     nvarchar(200) NULL,
    CONSTRAINT pk_employee_document PRIMARY KEY (document_id),
    CONSTRAINT fk_employee_document_employee FOREIGN KEY (employee_id) REFERENCES dbo.employee(employee_id),
    CONSTRAINT ck_employee_document_dates CHECK (expiry_date IS NULL OR issue_date IS NULL OR expiry_date >= issue_date)
);
GO

-- Обучение сотрудников.
CREATE TABLE dbo.employee_training
(
    training_id      int IDENTITY(1,1) NOT NULL,
    employee_id      int NOT NULL,
    training_name    nvarchar(150) NOT NULL,
    completion_date  date NOT NULL,
    expiry_date      date NULL,
    certificate_num  varchar(80) NULL,
    is_mandatory     bit NOT NULL CONSTRAINT df_employee_training_mandatory DEFAULT (0),
    CONSTRAINT pk_employee_training PRIMARY KEY (training_id),
    CONSTRAINT fk_employee_training_employee FOREIGN KEY (employee_id) REFERENCES dbo.employee(employee_id),
    CONSTRAINT ck_employee_training_dates CHECK (expiry_date IS NULL OR expiry_date >= completion_date)
);
GO

-- Заявки на отпуск сотрудников.
CREATE TABLE dbo.vacation_request
(
    vacation_request_id int IDENTITY(1,1) NOT NULL,
    employee_id         int NOT NULL,
    start_date          date NOT NULL,
    end_date            date NOT NULL,
    vacation_type       nvarchar(80) NOT NULL,
    status              nvarchar(30) NOT NULL CONSTRAINT df_vacation_request_status DEFAULT (N'Ожидает'),
    reason              nvarchar(300) NULL,
    approved_by_user_id int NULL,
    approval_date       date NULL,
    created_at          datetime2(0) NOT NULL CONSTRAINT df_vacation_request_created_at DEFAULT (SYSDATETIME()),
    CONSTRAINT pk_vacation_request PRIMARY KEY (vacation_request_id),
    CONSTRAINT fk_vacation_request_employee FOREIGN KEY (employee_id) REFERENCES dbo.employee(employee_id),
    CONSTRAINT fk_vacation_request_approved_by FOREIGN KEY (approved_by_user_id) REFERENCES dbo.app_user(user_id),
    CONSTRAINT ck_vacation_request_dates CHECK (end_date >= start_date),
    CONSTRAINT ck_vacation_request_status CHECK (status IN (N'Ожидает', N'Одобрена', N'Отклонена'))
);
GO

/* ================================================================
   2. ИНДЕКСЫ (10+)
   ================================================================ */

CREATE INDEX ix_employee_job_id ON dbo.employee(job_id);
CREATE INDEX ix_employee_department_id ON dbo.employee(department_id);
CREATE INDEX ix_bus_status ON dbo.bus(status);
CREATE INDEX ix_route_schedule_route_date ON dbo.route_schedule(route_id, service_date);
CREATE INDEX ix_route_schedule_bus_date ON dbo.route_schedule(bus_id, service_date);
CREATE INDEX ix_route_schedule_driver_date ON dbo.route_schedule(driver_id, service_date);
CREATE INDEX ix_maintenance_bus_date ON dbo.maintenance(bus_id, maintenance_date DESC);
CREATE INDEX ix_sale_sale_date ON dbo.sale(sale_date);
CREATE INDEX ix_sale_ticket_id ON dbo.sale(ticket_id);
CREATE INDEX ix_sale_schedule_id ON dbo.sale(schedule_id);
CREATE INDEX ix_sale_cashier_id ON dbo.sale(cashier_id);
CREATE INDEX ix_payment_sale_id ON dbo.payment(sale_id);
CREATE INDEX ix_payment_date ON dbo.payment(payment_date);
CREATE INDEX ix_user_role_role_id ON dbo.user_role(role_id);
CREATE INDEX ix_role_permission_permission_id ON dbo.role_permission(permission_id);
CREATE INDEX ix_employee_document_employee_id ON dbo.employee_document(employee_id);
CREATE INDEX ix_employee_training_employee_id ON dbo.employee_training(employee_id);
CREATE INDEX ix_vacation_request_employee_dates ON dbo.vacation_request(employee_id, start_date, end_date);
GO

/* ================================================================
   3. ДЕМО-ДАННЫЕ
   ================================================================ */

INSERT INTO dbo.department (department_name, department_code, description)
VALUES
(N'Отдел эксплуатации', 'EXP', N'Управление автобусами, водителями и рейсами'),
(N'Ремонтно-механический цех', 'RMC', N'Техническое обслуживание и ремонт'),
(N'Диспетчерская служба', 'DISP', N'Планирование и контроль движения'),
(N'Билетная служба', 'TICK', N'Продажа и учет проездных документов'),
(N'Отдел кадров', 'HR', N'Кадровый учет предприятия'),
(N'Администрация', 'ADM', N'Общее управление предприятием');
GO

INSERT INTO dbo.job (job_title, internship)
VALUES
(N'Водитель автобуса', N'2 года'),
(N'Механик', N'3 года'),
(N'Диспетчер', N'1 год'),
(N'Кассир', N'6 месяцев'),
(N'Начальник автопарка', N'5 лет'),
(N'Контролер', N'1 год'),
(N'Инженер по безопасности', N'3 года'),
(N'Системный администратор', N'1 год');
GO

INSERT INTO dbo.employee
    (surname, name, patronym, employed_date, job_id, department_id, phone, email, status)
VALUES
(N'Иванов', N'Иван', N'Иванович', '2020-01-15', 1, 1, '+375291000001', 'ivanov@autopark.local', N'Работает'),
(N'Петров', N'Петр', N'Петрович', '2019-03-20', 1, 1, '+375291000002', 'petrov@autopark.local', N'Работает'),
(N'Сидоров', N'Алексей', N'Михайлович', '2018-06-10', 2, 2, '+375291000003', 'sidorov@autopark.local', N'Работает'),
(N'Козлов', N'Дмитрий', N'Сергеевич', '2021-02-05', 3, 3, '+375291000004', 'kozlov@autopark.local', N'Работает'),
(N'Морозов', N'Андрей', N'Владимирович', '2017-08-25', 5, 6, '+375291000005', 'morozov@autopark.local', N'Работает'),
(N'Новиков', N'Сергей', N'Александрович', '2022-04-12', 4, 4, '+375291000006', 'novikov@autopark.local', N'Работает'),
(N'Волков', N'Михаил', N'Дмитриевич', '2020-11-30', 1, 1, '+375291000007', 'volkov@autopark.local', N'Работает'),
(N'Соловьев', N'Артем', N'Игоревич', '2019-09-15', 2, 2, '+375291000008', 'solovyev@autopark.local', N'Работает'),
(N'Васильев', N'Николай', N'Андреевич', '2021-07-08', 1, 1, '+375291000009', 'vasiliev@autopark.local', N'Работает'),
(N'Зайцев', N'Владимир', N'Петрович', '2018-12-03', 3, 3, '+375291000010', 'zaitsev@autopark.local', N'Работает'),
(N'Громов', N'Олег', N'Анатольевич', '2020-05-18', 6, 4, '+375291000011', 'gromov@autopark.local', N'Работает'),
(N'Федоров', N'Максим', N'Ильич', '2021-10-12', 7, 6, '+375291000012', 'fedorov@autopark.local', N'Работает');
GO

INSERT INTO dbo.bus
    (fleet_number, registration_num, model, manufacturer, manufacture_year, capacity, status, mileage_km)
VALUES
('A-001', '1234-MO-6', N'МАЗ-103.065', N'МАЗ', 2018, 96, N'Исправен', 184500),
('A-002', '1235-MO-6', N'МАЗ-107.066', N'МАЗ', 2019, 100, N'Исправен', 161200),
('A-003', '1236-MO-6', N'МАЗ-203.069', N'МАЗ', 2020, 96, N'Исправен', 122700),
('A-004', '1237-MO-6', N'МАЗ-203.169', N'МАЗ', 2021, 96, N'Резерв', 98200),
('A-005', '1238-MO-6', N'МАЗ-105.065', N'МАЗ', 2017, 150, N'На ремонте', 241000),
('A-006', '1239-MO-6', N'МАЗ-206.068', N'МАЗ', 2022, 72, N'Исправен', 60400),
('A-007', '1240-MO-6', N'МАЗ-215.069', N'МАЗ', 2020, 100, N'Исправен', 130800),
('A-008', '1241-MO-6', N'МАЗ-203.069', N'МАЗ', 2022, 96, N'Исправен', 51700),
('A-009', '1242-MO-6', N'МАЗ-103.065', N'МАЗ', 2018, 96, N'Исправен', 197300),
('A-010', '1243-MO-6', N'МАЗ-107.066', N'МАЗ', 2019, 100, N'Резерв', 149800),
('A-011', '1244-MO-6', N'МАЗ-203.069', N'МАЗ', 2023, 96, N'Исправен', 31200),
('A-012', '1245-MO-6', N'МАЗ-206.068', N'МАЗ', 2023, 72, N'Исправен', 28700),
('A-013', '1246-MO-6', N'МАЗ-203.169', N'МАЗ', 2019, 96, N'Исправен', 175600),
('A-014', '1247-MO-6', N'МАЗ-105.065', N'МАЗ', 2016, 150, N'Исправен', 282100),
('A-015', '1248-MO-6', N'МАЗ-103.065', N'МАЗ', 2021, 96, N'Исправен', 87300);
GO

INSERT INTO dbo.route (route_num, route_name, start_stop, end_stop)
VALUES
('1',  N'Вейнянка – Фатина', N'Вейнянка', N'Фатина'),
('2',  N'Малая Боровка – Солтановка', N'Малая Боровка', N'Солтановка'),
('3',  N'Вокзал – Спутник', N'Вокзал', N'Спутник'),
('4',  N'Мясокомбинат – Заводская', N'Мясокомбинат', N'Заводская'),
('5',  N'Броды – Казимировка', N'Броды', N'Казимировка'),
('6',  N'Гребеневский рынок – Холмы', N'Гребеневский рынок', N'Холмы'),
('7',  N'Автовокзал – Полыковичи', N'Автовокзал', N'Полыковичи'),
('8',  N'Центр – Сидоровичи', N'Центр', N'Сидоровичи'),
('9',  N'Площадь Славы – Буйничи', N'Площадь Славы', N'Буйничи'),
('10', N'Заднепровье – Химволокно', N'Заднепровье', N'Химволокно'),
('11', N'Вокзал – Соломинка', N'Вокзал', N'Соломинка'),
('12', N'Площадь Ленина – Чаусы', N'Площадь Ленина', N'Чаусы'),
('13', N'Могилев-2 – Дашковка', N'Могилев-2', N'Дашковка'),
('14', N'Кожзавод – Сухари', N'Кожзавод', N'Сухари'),
('15', N'Гребеневский рынок – Любуж', N'Гребеневский рынок', N'Любуж');
GO

INSERT INTO dbo.stop (stop_name, location, latitude, longitude)
VALUES
(N'Вейнянка', N'Могилев', 53.910100, 30.350100),
(N'Площадь Орджоникидзе', N'Могилев', 53.894500, 30.323500),
(N'Областная больница', N'Могилев', 53.888000, 30.337000),
(N'Фатина', N'Могилев', 53.880000, 30.350000),
(N'Малая Боровка', N'Могилев', 53.920000, 30.390000),
(N'Машековка', N'Могилев', 53.900000, 30.380000),
(N'Центр', N'Могилев', 53.900500, 30.332000),
(N'Солтановка', N'Могилев', 53.930000, 30.410000),
(N'Вокзал', N'Могилев', 53.897000, 30.327000),
(N'Площадь Ленина', N'Могилев', 53.894000, 30.335000),
(N'Универмаг', N'Могилев', 53.897500, 30.339000),
(N'Спутник', N'Могилев', 53.882000, 30.360000),
(N'Мясокомбинат', N'Могилев', 53.870000, 30.350000),
(N'Димитрова', N'Могилев', 53.880000, 30.330000),
(N'Юбилейный', N'Могилев', 53.890000, 30.315000),
(N'Заводская', N'Могилев', 53.875000, 30.300000),
(N'Броды', N'Могилев', 53.915000, 30.310000),
(N'Площадь Славы', N'Могилев', 53.898000, 30.323000),
(N'Казимировка', N'Могилев', 53.920000, 30.300000),
(N'Гребеневский рынок', N'Могилев', 53.905000, 30.345000),
(N'Мир', N'Могилев', 53.910000, 30.355000),
(N'Холмы', N'Могилев', 53.925000, 30.365000),
(N'Автовокзал', N'Могилев', 53.902000, 30.348000),
(N'Полыковичи', N'Могилев', 53.945000, 30.380000),
(N'Заднепровье', N'Могилев', 53.905000, 30.390000),
(N'Сидоровичи', N'Могилев', 53.930000, 30.440000),
(N'Зоосад', N'Могилев', 53.860000, 30.350000),
(N'Буйничи', N'Могилевский район', 53.850000, 30.280000),
(N'Химволокно', N'Могилев', 53.890000, 30.420000),
(N'Соломинка', N'Могилев', 53.915000, 30.410000),
(N'Чаусы', N'Могилевский район', 53.810000, 30.970000),
(N'Могилев-2', N'Могилев', 53.925000, 30.360000),
(N'Дашковка', N'Могилевский район', 53.970000, 30.260000),
(N'Кожзавод', N'Могилев', 53.885000, 30.350000),
(N'Сухари', N'Могилевский район', 53.840000, 30.390000),
(N'Любуж', N'Могилевский район', 53.900000, 30.470000);
GO

-- M:N маршрут-остановка: по четыре остановки на каждый маршрут.
INSERT INTO dbo.route_stop (route_id, stop_id, stop_sequence, distance_km, arrival_offset_min)
VALUES
(1,1,1,0,0),(1,2,2,2.5,8),(1,3,3,4.8,16),(1,4,4,6.3,25),
(2,5,1,0,0),(2,6,2,2.1,8),(2,7,3,4.6,16),(2,8,4,7.0,26),
(3,9,1,0,0),(3,10,2,2.0,7),(3,11,3,3.7,13),(3,12,4,6.4,22),
(4,13,1,0,0),(4,14,2,2.4,8),(4,15,3,4.2,15),(4,16,4,6.8,24),
(5,17,1,0,0),(5,7,2,2.8,10),(5,18,3,4.6,17),(5,19,4,7.5,27),
(6,20,1,0,0),(6,2,2,2.6,9),(6,21,3,4.9,17),(6,22,4,7.1,25),
(7,23,1,0,0),(7,10,2,2.2,8),(7,14,3,4.4,16),(7,24,4,7.2,27),
(8,7,1,0,0),(8,18,2,2.3,8),(8,25,3,4.8,17),(8,26,4,7.2,26),
(9,18,1,0,0),(9,3,2,2.5,8),(9,27,3,4.2,15),(9,28,4,7.0,26),
(10,25,1,0,0),(10,7,2,2.0,7),(10,15,3,4.4,15),(10,29,4,7.4,26),
(11,9,1,0,0),(11,7,2,2.0,7),(11,14,3,4.1,15),(11,30,4,6.9,25),
(12,10,1,0,0),(12,7,2,2.3,8),(12,25,3,4.9,17),(12,31,4,28.0,55),
(13,32,1,0,0),(13,7,2,2.6,9),(13,15,3,4.8,17),(13,33,4,8.1,29),
(14,34,1,0,0),(14,7,2,2.1,8),(14,18,3,4.7,17),(14,35,4,7.5,26),
(15,20,1,0,0),(15,7,2,2.2,8),(15,25,3,4.8,17),(15,36,4,7.6,26);
GO

-- Расписания на ближайшие 20 дней по нескольким маршрутам.
DECLARE @d date = CAST(GETDATE() AS date);

INSERT INTO dbo.route_schedule
    (route_id, bus_id, driver_id, service_date, departure_time, arrival_time, available_seat_num, schedule_status)
VALUES
(1, 1, 1, @d, DATEADD(MINUTE, 360, CAST('00:00' AS time)), DATEADD(MINUTE, 405, CAST('00:00' AS time)), 96, N'Запланирован'),
(2, 2, 2, @d, DATEADD(MINUTE, 390, CAST('00:00' AS time)), DATEADD(MINUTE, 440, CAST('00:00' AS time)), 100, N'Запланирован'),
(3, 3, 7, @d, DATEADD(MINUTE, 420, CAST('00:00' AS time)), DATEADD(MINUTE, 460, CAST('00:00' AS time)), 96, N'В пути'),
(4, 4, 9, @d, DATEADD(MINUTE, 450, CAST('00:00' AS time)), DATEADD(MINUTE, 485, CAST('00:00' AS time)), 96, N'Запланирован'),
(5, 6, 1, @d, DATEADD(MINUTE, 480, CAST('00:00' AS time)), DATEADD(MINUTE, 535, CAST('00:00' AS time)), 72, N'Запланирован'),
(6, 7, 2, @d, DATEADD(MINUTE, 510, CAST('00:00' AS time)), DATEADD(MINUTE, 555, CAST('00:00' AS time)), 100, N'Запланирован'),
(7, 8, 7, @d, DATEADD(MINUTE, 540, CAST('00:00' AS time)), DATEADD(MINUTE, 580, CAST('00:00' AS time)), 96, N'Запланирован'),
(8, 9, 9, @d, DATEADD(MINUTE, 570, CAST('00:00' AS time)), DATEADD(MINUTE, 630, CAST('00:00' AS time)), 96, N'Запланирован'),
(9, 11, 1, @d, DATEADD(MINUTE, 600, CAST('00:00' AS time)), DATEADD(MINUTE, 650, CAST('00:00' AS time)), 96, N'Запланирован'),
(10,12, 2, @d, DATEADD(MINUTE, 630, CAST('00:00' AS time)), DATEADD(MINUTE, 670, CAST('00:00' AS time)), 72, N'Запланирован');
GO

INSERT INTO dbo.ticket
    (ticket_name, ticket_type, zone, price, valid_days, available_count, issue_date, expiry_date)
VALUES
(N'Разовый билет город', N'Разовый', N'Город', 0.80, 1, 1000, CAST(GETDATE() AS date), DATEADD(DAY,30,CAST(GETDATE() AS date))),
(N'Разовый билет зона 1', N'Разовый', N'Зона 1', 0.90, 1, 900, CAST(GETDATE() AS date), DATEADD(DAY,30,CAST(GETDATE() AS date))),
(N'Разовый билет зона 2', N'Разовый', N'Зона 2', 1.10, 1, 800, CAST(GETDATE() AS date), DATEADD(DAY,30,CAST(GETDATE() AS date))),
(N'Проездной на месяц', N'Проездной', N'Город', 35.00, 30, 500, CAST(GETDATE() AS date), DATEADD(DAY,60,CAST(GETDATE() AS date))),
(N'Проездной льготный', N'Проездной', N'Город', 18.00, 30, 500, CAST(GETDATE() AS date), DATEADD(DAY,60,CAST(GETDATE() AS date))),
(N'Разовый билет экспресс', N'Разовый', N'Экспресс', 1.50, 1, 500, CAST(GETDATE() AS date), DATEADD(DAY,30,CAST(GETDATE() AS date))),
(N'Билет выходного дня', N'Разовый', N'Город', 0.70, 1, 700, CAST(GETDATE() AS date), DATEADD(DAY,30,CAST(GETDATE() AS date))),
(N'Проездной студенческий', N'Проездной', N'Город', 15.00, 30, 450, CAST(GETDATE() AS date), DATEADD(DAY,60,CAST(GETDATE() AS date))),
(N'Билет пригородный', N'Разовый', N'Пригород', 2.20, 1, 400, CAST(GETDATE() AS date), DATEADD(DAY,30,CAST(GETDATE() AS date))),
(N'Билет багажный', N'Разовый', N'Город', 0.80, 1, 600, CAST(GETDATE() AS date), DATEADD(DAY,30,CAST(GETDATE() AS date))),
(N'Проездной на 15 дней', N'Проездной', N'Город', 20.00, 15, 350, CAST(GETDATE() AS date), DATEADD(DAY,45,CAST(GETDATE() AS date))),
(N'Билет ночной', N'Разовый', N'Город', 0.90, 1, 300, CAST(GETDATE() AS date), DATEADD(DAY,30,CAST(GETDATE() AS date))),
(N'Билет до Буйнич', N'Разовый', N'Пригород', 1.80, 1, 250, CAST(GETDATE() AS date), DATEADD(DAY,30,CAST(GETDATE() AS date))),
(N'Билет межрайонный', N'Разовый', N'Межрайон', 2.50, 1, 200, CAST(GETDATE() AS date), DATEADD(DAY,30,CAST(GETDATE() AS date))),
(N'Проездной корпоративный', N'Проездной', N'Город', 40.00, 30, 100, CAST(GETDATE() AS date), DATEADD(DAY,60,CAST(GETDATE() AS date)));
GO

INSERT INTO dbo.maintenance
    (bus_id, employee_id, maintenance_date, next_maintenance_date, maintenance_type, found_issue, service_result, mileage_km, roadworthiness, maintenance_cost)
VALUES
(1,3,DATEADD(DAY,-30,CAST(GETDATE() AS date)),DATEADD(DAY,60,CAST(GETDATE() AS date)),N'Плановое ТО',N'Износ фильтров',N'Фильтры и масло заменены',184000,N'Исправен',420.00),
(2,8,DATEADD(DAY,-20,CAST(GETDATE() AS date)),DATEADD(DAY,70,CAST(GETDATE() AS date)),N'Диагностика тормозов',N'Износ колодок',N'Колодки заменены',160500,N'Исправен',780.00),
(3,3,DATEADD(DAY,-10,CAST(GETDATE() AS date)),DATEADD(DAY,80,CAST(GETDATE() AS date)),N'Плановое ТО',N'Нет',N'Работы выполнены',122000,N'Исправен',350.00),
(4,8,DATEADD(DAY,-45,CAST(GETDATE() AS date)),DATEADD(DAY,45,CAST(GETDATE() AS date)),N'Электрика',N'Ошибка блока освещения',N'Контакты восстановлены',97500,N'Исправен',210.00),
(5,3,DATEADD(DAY,-5,CAST(GETDATE() AS date)),DATEADD(DAY,5,CAST(GETDATE() AS date)),N'Двигатель',N'Повышенный расход масла',N'Автобус оставлен в ремонте',240800,N'Неисправен',1600.00),
(6,8,DATEADD(DAY,-12,CAST(GETDATE() AS date)),DATEADD(DAY,78,CAST(GETDATE() AS date)),N'Плановое ТО',N'Нет',N'Работы выполнены',60000,N'Исправен',330.00),
(7,3,DATEADD(DAY,-25,CAST(GETDATE() AS date)),DATEADD(DAY,65,CAST(GETDATE() AS date)),N'Ходовая часть',N'Износ втулок',N'Втулки заменены',130000,N'Исправен',520.00),
(8,8,DATEADD(DAY,-15,CAST(GETDATE() AS date)),DATEADD(DAY,75,CAST(GETDATE() AS date)),N'Плановое ТО',N'Нет',N'Работы выполнены',51000,N'Исправен',310.00),
(9,3,DATEADD(DAY,-35,CAST(GETDATE() AS date)),DATEADD(DAY,55,CAST(GETDATE() AS date)),N'Тормозная система',N'Повышенный износ',N'Регулировка',196500,N'Требует внимания',640.00),
(10,8,DATEADD(DAY,-8,CAST(GETDATE() AS date)),DATEADD(DAY,82,CAST(GETDATE() AS date)),N'Плановое ТО',N'Нет',N'Работы выполнены',149300,N'Исправен',300.00),
(11,3,DATEADD(DAY,-17,CAST(GETDATE() AS date)),DATEADD(DAY,73,CAST(GETDATE() AS date)),N'Диагностика двигателя',N'Нет',N'Замечаний нет',31000,N'Исправен',220.00),
(12,8,DATEADD(DAY,-22,CAST(GETDATE() AS date)),DATEADD(DAY,68,CAST(GETDATE() AS date)),N'Плановое ТО',N'Нет',N'Работы выполнены',28200,N'Исправен',280.00);
GO

-- 60 продаж: основной объем данных для отчетности и требований курсовой.
DECLARE @i int = 1;
WHILE @i <= 60
BEGIN
    INSERT INTO dbo.sale
        (schedule_id, ticket_id, cashier_id, sale_date, sale_price, sale_status, payment_status, sale_channel, ticket_quantity)
    VALUES
    ((@i % 10) + 1,
     ((@i - 1) % 15) + 1,
     CASE WHEN @i % 5 = 0 THEN NULL ELSE ((@i - 1) % 4) + 1 END,
     DATEADD(DAY, -(@i % 20), DATEADD(MINUTE, @i * 17, CAST(GETDATE() AS datetime2(0)))),
     (SELECT price * CASE WHEN @i % 7 = 0 THEN 0.9 ELSE 1.0 END FROM dbo.ticket WHERE ticket_id = ((@i - 1) % 15) + 1),
     CASE WHEN @i % 13 = 0 THEN N'Возврат' ELSE N'Завершена' END,
     CASE WHEN @i % 13 = 0 THEN N'Возврат' ELSE N'Оплачена' END,
     CASE WHEN @i % 4 = 0 THEN N'QR'
          WHEN @i % 4 = 1 THEN N'Касса'
          WHEN @i % 4 = 2 THEN N'Кондуктор'
          ELSE N'Валидатор' END,
     CASE WHEN @i % 10 = 0 THEN 2 ELSE 1 END);
    SET @i += 1;
END;
GO

-- Платежи для 60 продаж; часть операций возвращена.
INSERT INTO dbo.payment
    (sale_id, payment_date, amount, payment_method, payment_status, transaction_id)
SELECT
    s.sale_id,
    DATEADD(SECOND, 30, s.sale_date),
    s.sale_price * s.ticket_quantity,
    CASE s.sale_channel
        WHEN N'QR' THEN N'QR'
        WHEN N'Валидатор' THEN N'Банковская карта'
        ELSE N'Наличные'
    END,
    CASE WHEN s.sale_status = N'Возврат' THEN N'Возврат' ELSE N'Успешно' END,
    CONCAT('TX-', s.sale_id)
FROM dbo.sale AS s;
GO

INSERT INTO dbo.employee_document
    (employee_id, document_type, document_num, issue_date, expiry_date, issued_by)
VALUES
(1,N'Водительское удостоверение',N'3AC100001','2018-05-10',DATEADD(YEAR,5,GETDATE()),N'ГАИ'),
(2,N'Водительское удостоверение',N'3AC100002','2017-06-20',DATEADD(YEAR,5,GETDATE()),N'ГАИ'),
(3,N'Удостоверение механика',N'MEC-001','2020-02-15',NULL,N'Автопарк'),
(4,N'Должностное удостоверение',N'DISP-001','2021-02-05',NULL,N'Автопарк'),
(5,N'Паспорт',N'MP1234501','2019-08-25','2029-08-25',N'ОВД'),
(6,N'Кассовое удостоверение',N'KAS-001','2022-04-12',NULL,N'Автопарк');
GO

INSERT INTO dbo.employee_training
    (employee_id, training_name, completion_date, expiry_date, certificate_num, is_mandatory)
VALUES
(1,N'Безопасность дорожного движения',DATEADD(MONTH,-8,GETDATE()),DATEADD(MONTH,4,GETDATE()),N'БДД-001',1),
(1,N'Первая помощь',DATEADD(MONTH,-5,GETDATE()),DATEADD(YEAR,2,GETDATE()),N'ПМП-001',1),
(2,N'Безопасность дорожного движения',DATEADD(MONTH,-10,GETDATE()),DATEADD(MONTH,2,GETDATE()),N'БДД-002',1),
(7,N'Безопасность дорожного движения',DATEADD(MONTH,-6,GETDATE()),DATEADD(MONTH,6,GETDATE()),N'БДД-003',1),
(8,N'Ремонт автобусов МАЗ',DATEADD(MONTH,-14,GETDATE()),NULL,N'RTO-001',0),
(12,N'Охрана труда',DATEADD(MONTH,-3,GETDATE()),DATEADD(YEAR,1,GETDATE()),N'OT-001',1);
GO

INSERT INTO dbo.vacation_request
    (employee_id, start_date, end_date, vacation_type, status, reason)
VALUES
(1,DATEADD(DAY,30,CAST(GETDATE() AS date)),DATEADD(DAY,44,CAST(GETDATE() AS date)),N'Ежегодный оплачиваемый отпуск',N'Ожидает',N'Плановый отпуск'),
(2,DATEADD(DAY,60,CAST(GETDATE() AS date)),DATEADD(DAY,74,CAST(GETDATE() AS date)),N'Ежегодный оплачиваемый отпуск',N'Одобрена',N'Летний отпуск'),
(3,DATEADD(DAY,90,CAST(GETDATE() AS date)),DATEADD(DAY,94,CAST(GETDATE() AS date)),N'Отпуск без сохранения',N'Ожидает',N'Семейные обстоятельства'),
(6,DATEADD(DAY,-30,CAST(GETDATE() AS date)),DATEADD(DAY,-28,CAST(GETDATE() AS date)),N'Ежегодный оплачиваемый отпуск',N'Одобрена',N'Плановый отпуск');
GO

INSERT INTO dbo.report (employee_id, report_date, report_type, total_sales, total_amount, file_path)
SELECT
    5,
    CAST(GETDATE() AS date),
    N'Дневной отчет по продажам',
    COUNT(*),
    COALESCE(SUM(CASE WHEN sale_status <> N'Возврат' THEN sale_price * ticket_quantity ELSE 0 END),0),
    N'reports/daily_sales.rpt'
FROM dbo.sale;
GO

/* Роли и прикладные пользователи. Пароли здесь учебные хэши-заглушки. */
INSERT INTO dbo.app_role (role_name, description)
VALUES
('administrator', N'Полный доступ к информационной системе'),
('dispatcher', N'Работа с маршрутами, расписанием и отчетами'),
('cashier', N'Работа с билетами, продажами и платежами');
GO

INSERT INTO dbo.permission (permission_name, description)
VALUES
('bus.read', N'Просмотр автобусов'),
('bus.write', N'Изменение автобусов'),
('employee.read', N'Просмотр сотрудников'),
('route.read', N'Просмотр маршрутов'),
('route.write', N'Изменение маршрутов и расписания'),
('ticket.read', N'Просмотр билетов'),
('ticket.write', N'Изменение билетов'),
('sale.read', N'Просмотр продаж'),
('sale.write', N'Создание и изменение продаж'),
('payment.read', N'Просмотр платежей'),
('report.read', N'Просмотр отчетов'),
('report.write', N'Создание отчетов');
GO

INSERT INTO dbo.app_user (employee_id, login, password_hash, email)
VALUES
(5,'admin','HASH_ADMIN','admin@autopark.local'),
(4,'dispatcher','HASH_DISPATCHER','dispatcher@autopark.local'),
(6,'cashier','HASH_CASHIER','cashier@autopark.local');
GO

INSERT INTO dbo.user_role(user_id, role_id, assigned_by)
VALUES
(1,1,'system'),
(2,2,'system'),
(3,3,'system');
GO

INSERT INTO dbo.role_permission(role_id, permission_id)
SELECT 1, permission_id FROM dbo.permission;
GO
INSERT INTO dbo.role_permission(role_id, permission_id)
SELECT 2, permission_id FROM dbo.permission
WHERE permission_name IN ('bus.read','employee.read','route.read','route.write','ticket.read','report.read','report.write','sale.read');
GO
INSERT INTO dbo.role_permission(role_id, permission_id)
SELECT 3, permission_id FROM dbo.permission
WHERE permission_name IN ('ticket.read','ticket.write','sale.read','sale.write','payment.read');
GO

/* ================================================================
   4. 15 ПРЕДСТАВЛЕНИЙ
   ================================================================ */

-- 1. Список автобусов и их техническое состояние.
CREATE VIEW dbo.v_bus_status
AS
SELECT
    b.bus_id,
    b.fleet_number,
    b.registration_num,
    b.model,
    b.capacity,
    b.status,
    b.mileage_km,
    CASE
        WHEN b.mileage_km >= 250000 THEN N'Высокий пробег'
        WHEN b.mileage_km >= 150000 THEN N'Средний пробег'
        ELSE N'Низкий пробег'
    END AS mileage_category
FROM dbo.bus AS b;
GO

-- 2. Сотрудники с должностью и подразделением.
CREATE VIEW dbo.v_employee_directory
AS
SELECT
    e.employee_id,
    CONCAT(e.surname, N' ', e.name, N' ', COALESCE(e.patronym, N'')) AS employee_name,
    j.job_title,
    d.department_name,
    e.status,
    DATEDIFF(YEAR, e.employed_date, GETDATE()) AS service_years
FROM dbo.employee AS e
INNER JOIN dbo.job AS j ON j.job_id = e.job_id
LEFT JOIN dbo.department AS d ON d.department_id = e.department_id;
GO

-- 3. Активные маршруты.
CREATE VIEW dbo.v_active_route
AS
SELECT
    r.route_id,
    r.route_num,
    r.route_name,
    UPPER(r.start_stop) AS start_stop,
    UPPER(r.end_stop) AS end_stop
FROM dbo.route AS r
WHERE r.is_active = 1;
GO

-- 4. Расписание с автобусом и водителем.
CREATE VIEW dbo.v_route_schedule
AS
SELECT
    rs.schedule_id,
    rs.service_date,
    r.route_num,
    r.route_name,
    b.fleet_number,
    b.model,
    CONCAT(e.surname, N' ', e.name) AS driver_name,
    rs.departure_time,
    rs.arrival_time,
    DATEDIFF(MINUTE, rs.departure_time, rs.arrival_time) AS trip_minutes,
    rs.available_seat_num,
    rs.schedule_status
FROM dbo.route_schedule AS rs
INNER JOIN dbo.route AS r ON r.route_id = rs.route_id
INNER JOIN dbo.bus AS b ON b.bus_id = rs.bus_id
INNER JOIN dbo.employee AS e ON e.employee_id = rs.driver_id;
GO

-- 5. Состав маршрутов по остановкам.
CREATE VIEW dbo.v_route_stops
AS
SELECT
    r.route_num,
    r.route_name,
    s.stop_sequence,
    st.stop_name,
    st.location,
    ROUND(COALESCE(s.distance_km,0),2) AS distance_km,
    COALESCE(s.arrival_offset_min,0) AS arrival_offset_min
FROM dbo.route_stop AS s
INNER JOIN dbo.route AS r ON r.route_id = s.route_id
INNER JOIN dbo.stop AS st ON st.stop_id = s.stop_id;
GO

-- 6. Последнее техническое обслуживание каждого автобуса.
CREATE VIEW dbo.v_latest_maintenance
AS
SELECT
    b.bus_id,
    b.fleet_number,
    m.maintenance_date,
    m.maintenance_type,
    m.roadworthiness,
    m.maintenance_cost,
    ROW_NUMBER() OVER (PARTITION BY b.bus_id ORDER BY m.maintenance_date DESC) AS maintenance_seq
FROM dbo.bus AS b
INNER JOIN dbo.maintenance AS m ON m.bus_id = b.bus_id;
GO

-- 7. Продажи с расшифровкой билета.
CREATE VIEW dbo.v_sale_details
AS
SELECT
    s.sale_id,
    s.sale_date,
    t.ticket_name,
    t.ticket_type,
    s.ticket_quantity,
    s.sale_price,
    ROUND(s.sale_price * s.ticket_quantity,2) AS sale_total,
    s.sale_channel,
    s.sale_status,
    s.payment_status,
    COALESCE(CONCAT(e.surname, N' ', e.name), N'Обезличенная продажа') AS cashier_name
FROM dbo.sale AS s
INNER JOIN dbo.ticket AS t ON t.ticket_id = s.ticket_id
LEFT JOIN dbo.employee AS e ON e.employee_id = s.cashier_id;
GO

-- 8. Продажи по каналам.
CREATE VIEW dbo.v_sales_by_channel
AS
SELECT
    sale_channel,
    COUNT(*) AS sale_count,
    SUM(ticket_quantity) AS ticket_count,
    ROUND(SUM(sale_price * ticket_quantity),2) AS amount_total,
    AVG(sale_price) AS average_ticket_price
FROM dbo.sale
WHERE sale_status <> N'Возврат'
GROUP BY sale_channel;
GO

-- 9. Продажи по маршрутам.
CREATE VIEW dbo.v_sales_by_route
AS
SELECT
    r.route_num,
    r.route_name,
    COUNT(s.sale_id) AS sale_count,
    SUM(COALESCE(s.ticket_quantity,0)) AS ticket_count,
    ROUND(COALESCE(SUM(CASE WHEN s.sale_status <> N'Возврат' THEN s.sale_price * s.ticket_quantity ELSE 0 END),0),2) AS amount_total
FROM dbo.route AS r
LEFT JOIN dbo.route_schedule AS rs ON rs.route_id = r.route_id
LEFT JOIN dbo.sale AS s ON s.schedule_id = rs.schedule_id
GROUP BY r.route_num, r.route_name;
GO

-- 10. Выручка сотрудников.
CREATE VIEW dbo.v_sales_by_employee
AS
SELECT
    e.employee_id,
    CONCAT(e.surname, N' ', e.name) AS employee_name,
    j.job_title,
    COUNT(s.sale_id) AS sale_count,
    ROUND(COALESCE(SUM(CASE WHEN s.sale_status <> N'Возврат' THEN s.sale_price * s.ticket_quantity ELSE 0 END),0),2) AS amount_total
FROM dbo.employee AS e
INNER JOIN dbo.job AS j ON j.job_id = e.job_id
LEFT JOIN dbo.sale AS s ON s.cashier_id = e.employee_id
GROUP BY e.employee_id, e.surname, e.name, j.job_title;
GO

-- 11. Список платежей и отклонений.
CREATE VIEW dbo.v_payment_details
AS
SELECT
    p.payment_id,
    p.sale_id,
    p.payment_date,
    p.amount,
    p.payment_method,
    p.payment_status,
    p.transaction_id,
    CASE WHEN p.payment_status = N'Успешно' THEN N'Подтвержден' ELSE N'Требует проверки' END AS control_status
FROM dbo.payment AS p;
GO

-- 12. Актуальные документы сотрудников.
CREATE VIEW dbo.v_employee_documents
AS
SELECT
    d.document_id,
    d.employee_id,
    CONCAT(e.surname, N' ', e.name) AS employee_name,
    d.document_type,
    d.document_num,
    d.issue_date,
    d.expiry_date,
    CASE
        WHEN d.expiry_date IS NULL THEN N'Бессрочно'
        WHEN d.expiry_date < CAST(GETDATE() AS date) THEN N'Просрочен'
        WHEN DATEDIFF(DAY, CAST(GETDATE() AS date), d.expiry_date) <= 30 THEN N'Истекает в течение 30 дней'
        ELSE N'Действителен'
    END AS document_status
FROM dbo.employee_document AS d
INNER JOIN dbo.employee AS e ON e.employee_id = d.employee_id;
GO

-- 13. Обучение сотрудников с контролем сроков.
CREATE VIEW dbo.v_employee_training_status
AS
SELECT
    t.training_id,
    t.employee_id,
    CONCAT(e.surname, N' ', e.name) AS employee_name,
    t.training_name,
    t.completion_date,
    t.expiry_date,
    CASE
        WHEN t.expiry_date IS NULL THEN N'Без срока'
        WHEN t.expiry_date < CAST(GETDATE() AS date) THEN N'Просрочено'
        WHEN t.expiry_date <= DATEADD(DAY,30,CAST(GETDATE() AS date)) THEN N'Истекает скоро'
        ELSE N'Действительно'
    END AS training_status
FROM dbo.employee_training AS t
INNER JOIN dbo.employee AS e ON e.employee_id = t.employee_id;
GO

-- 14. Заявки на отпуск с вычислением количества дней.
CREATE VIEW dbo.v_vacation_requests
AS
SELECT
    v.vacation_request_id,
    CONCAT(e.surname, N' ', e.name) AS employee_name,
    v.vacation_type,
    v.start_date,
    v.end_date,
    DATEDIFF(DAY, v.start_date, v.end_date) + 1 AS days_requested,
    v.status,
    COALESCE(v.reason, N'Не указана') AS reason
FROM dbo.vacation_request AS v
INNER JOIN dbo.employee AS e ON e.employee_id = v.employee_id;
GO

-- 15. Общая оперативная сводка по автопарку.
CREATE VIEW dbo.v_autopark_summary
AS
SELECT
    (SELECT COUNT(*) FROM dbo.bus WHERE status <> N'Списан') AS active_bus_count,
    (SELECT COUNT(*) FROM dbo.employee WHERE status = N'Работает') AS active_employee_count,
    (SELECT COUNT(*) FROM dbo.route WHERE is_active = 1) AS active_route_count,
    (SELECT COUNT(*) FROM dbo.route_schedule WHERE service_date = CAST(GETDATE() AS date) AND schedule_status <> N'Отменен') AS today_schedule_count,
    (SELECT COUNT(*) FROM dbo.sale WHERE CAST(sale_date AS date) = CAST(GETDATE() AS date)) AS today_sale_count,
    (SELECT ROUND(COALESCE(SUM(sale_price * ticket_quantity),0),2) FROM dbo.sale WHERE CAST(sale_date AS date) = CAST(GETDATE() AS date) AND sale_status <> N'Возврат') AS today_amount_total,
    (SELECT COUNT(*) FROM dbo.maintenance WHERE roadworthiness <> N'Исправен') AS bus_need_attention_count;
GO

/* ================================================================
   5. 20 ХРАНИМЫХ ПРОЦЕДУР
   ================================================================ */

-- 1. Список активных автобусов.
CREATE PROCEDURE dbo.get_active_bus
AS
BEGIN
    SET NOCOUNT ON;
    SELECT *
    FROM dbo.v_bus_status
    WHERE status = N'Исправен'
    ORDER BY fleet_number;
END;
GO

-- 2. Поиск автобуса по номеру автопарка.
CREATE PROCEDURE dbo.get_bus_by_fleet_number
    @fleet_number varchar(20)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT *
    FROM dbo.bus
    WHERE fleet_number = @fleet_number;
END;
GO

-- 3. Расписание маршрута на дату.
CREATE PROCEDURE dbo.get_route_schedule
    @route_id int,
    @service_date date
AS
BEGIN
    SET NOCOUNT ON;
    SELECT *
    FROM dbo.v_route_schedule
    WHERE route_id = @route_id
      AND service_date = @service_date
    ORDER BY departure_time;
END;
GO

-- 4. Поиск расписания в диапазоне дат.
CREATE PROCEDURE dbo.get_schedule_range
    @date_from date,
    @date_to date
AS
BEGIN
    SET NOCOUNT ON;
    SELECT *
    FROM dbo.v_route_schedule
    WHERE service_date BETWEEN @date_from AND @date_to
    ORDER BY service_date, route_num, departure_time;
END;
GO

-- 5. Продажи за период.
CREATE PROCEDURE dbo.get_sales_by_date_range
    @date_from datetime2(0),
    @date_to datetime2(0)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT *
    FROM dbo.v_sale_details
    WHERE sale_date >= @date_from
      AND sale_date < DATEADD(SECOND,1,@date_to)
    ORDER BY sale_date;
END;
GO

-- 6. Продажи по каналу.
CREATE PROCEDURE dbo.get_sales_by_channel
    @sale_channel nvarchar(30)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT *
    FROM dbo.v_sale_details
    WHERE sale_channel = @sale_channel
    ORDER BY sale_date DESC;
END;
GO

-- 7. Продажи определенного кассира.
CREATE PROCEDURE dbo.get_sales_by_cashier
    @cashier_id int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT *
    FROM dbo.v_sale_details
    WHERE cashier_name = COALESCE((SELECT CONCAT(surname,N' ',name) FROM dbo.employee WHERE employee_id = @cashier_id),N'')
    ORDER BY sale_date DESC;
END;
GO

-- 8. Статистика продаж по маршруту.
CREATE PROCEDURE dbo.get_route_sales
    @route_id int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        r.route_num,
        r.route_name,
        COUNT(s.sale_id) AS sale_count,
        COALESCE(SUM(s.ticket_quantity),0) AS ticket_count,
        ROUND(COALESCE(SUM(CASE WHEN s.sale_status <> N'Возврат' THEN s.sale_price * s.ticket_quantity ELSE 0 END),0),2) AS amount_total
    FROM dbo.route AS r
    LEFT JOIN dbo.route_schedule AS rs ON rs.route_id = r.route_id
    LEFT JOIN dbo.sale AS s ON s.schedule_id = rs.schedule_id
    WHERE r.route_id = @route_id
    GROUP BY r.route_num, r.route_name;
END;
GO

-- 9. История обслуживания автобуса.
CREATE PROCEDURE dbo.get_bus_maintenance
    @bus_id int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        m.*,
        DATEDIFF(DAY, m.maintenance_date, CAST(GETDATE() AS date)) AS days_from_last_service
    FROM dbo.maintenance AS m
    WHERE m.bus_id = @bus_id
    ORDER BY m.maintenance_date DESC;
END;
GO

-- 10. Обновление статуса автобуса.
CREATE PROCEDURE dbo.set_bus_status
    @bus_id int,
    @status nvarchar(30)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.bus
       SET status = @status
     WHERE bus_id = @bus_id;

    SELECT bus_id, fleet_number, status
    FROM dbo.bus
    WHERE bus_id = @bus_id;
END;
GO

-- 11. Регистрация технического обслуживания.
CREATE PROCEDURE dbo.add_maintenance
    @bus_id int,
    @employee_id int = NULL,
    @maintenance_type nvarchar(100),
    @found_issue nvarchar(500) = NULL,
    @service_result nvarchar(500) = NULL,
    @maintenance_cost decimal(12,2) = 0
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.maintenance
        (bus_id, employee_id, maintenance_date, maintenance_type, found_issue, service_result, maintenance_cost)
    VALUES
        (@bus_id, @employee_id, CAST(GETDATE() AS date), @maintenance_type, @found_issue, @service_result, @maintenance_cost);

    SELECT CAST(SCOPE_IDENTITY() AS int) AS maintenance_id;
END;
GO

-- 12. Добавление маршрута.
CREATE PROCEDURE dbo.add_route
    @route_num varchar(20),
    @route_name nvarchar(120),
    @start_stop nvarchar(120),
    @end_stop nvarchar(120)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.route(route_num, route_name, start_stop, end_stop)
    VALUES (@route_num, @route_name, @start_stop, @end_stop);

    SELECT CAST(SCOPE_IDENTITY() AS int) AS route_id;
END;
GO

-- 13. Получение маршрута и количества его остановок.
CREATE PROCEDURE dbo.get_route_details
    @route_id int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        r.route_id,
        r.route_num,
        r.route_name,
        r.start_stop,
        r.end_stop,
        COUNT(rs.stop_id) AS stop_count
    FROM dbo.route AS r
    LEFT JOIN dbo.route_stop AS rs ON rs.route_id = r.route_id
    WHERE r.route_id = @route_id
    GROUP BY r.route_id, r.route_num, r.route_name, r.start_stop, r.end_stop;
END;
GO

-- 14. Получение билетов в заданном диапазоне цен.
CREATE PROCEDURE dbo.get_ticket_by_price_range
    @price_from decimal(10,2),
    @price_to decimal(10,2)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        ticket_id,
        ticket_name,
        ticket_type,
        price,
        ROUND(price * valid_days,2) AS period_value
    FROM dbo.ticket
    WHERE price BETWEEN @price_from AND @price_to
    ORDER BY price, ticket_name;
END;
GO

-- 15. Регистрация продажи.
CREATE PROCEDURE dbo.add_sale
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

    BEGIN TRANSACTION;

    INSERT INTO dbo.sale
        (schedule_id, ticket_id, cashier_id, sale_price, sale_channel, ticket_quantity, sale_status, payment_status)
    VALUES
        (@schedule_id, @ticket_id, @cashier_id, @sale_price, @sale_channel, @ticket_quantity, N'Завершена', N'Ожидает');

    SET @sale_id = CAST(SCOPE_IDENTITY() AS bigint);

    COMMIT TRANSACTION;

    SELECT @sale_id AS sale_id;
END;
GO

-- 16. Регистрация оплаты за продажу.
CREATE PROCEDURE dbo.add_payment
    @sale_id bigint,
    @amount decimal(10,2),
    @payment_method nvarchar(40),
    @transaction_id varchar(80) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.payment
        (sale_id, amount, payment_method, payment_status, transaction_id)
    VALUES
        (@sale_id, @amount, @payment_method, N'Успешно', @transaction_id);

    UPDATE dbo.sale
       SET payment_status = N'Оплачена'
     WHERE sale_id = @sale_id;

    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS payment_id;
END;
GO

-- 17. Итог продаж за дату.
CREATE PROCEDURE dbo.get_daily_sales_total
    @sale_date date
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        COUNT(*) AS sale_count,
        SUM(ticket_quantity) AS ticket_count,
        ROUND(COALESCE(SUM(CASE WHEN sale_status <> N'Возврат' THEN sale_price * ticket_quantity ELSE 0 END),0),2) AS amount_total,
        AVG(sale_price) AS average_price
    FROM dbo.sale
    WHERE CAST(sale_date AS date) = @sale_date;
END;
GO

-- 18. Контроль технических документов, истекающих в ближайшие дни.
CREATE PROCEDURE dbo.get_expiring_documents
    @days int = 30
AS
BEGIN
    SET NOCOUNT ON;
    SELECT *
    FROM dbo.v_employee_documents
    WHERE expiry_date IS NOT NULL
      AND expiry_date BETWEEN CAST(GETDATE() AS date) AND DATEADD(DAY,@days,CAST(GETDATE() AS date))
    ORDER BY expiry_date;
END;
GO

-- 19. Получение пользователей и их ролей.
CREATE PROCEDURE dbo.get_users_with_roles
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        u.user_id,
        u.login,
        u.is_active,
        STRING_AGG(r.role_name, N', ') WITHIN GROUP (ORDER BY r.role_name) AS roles
    FROM dbo.app_user AS u
    LEFT JOIN dbo.user_role AS ur ON ur.user_id = u.user_id
    LEFT JOIN dbo.app_role AS r ON r.role_id = ur.role_id
    GROUP BY u.user_id, u.login, u.is_active;
END;
GO

-- 20. Формирование агрегированного отчета по продажам.
CREATE PROCEDURE dbo.create_sales_report
    @employee_id int,
    @report_type nvarchar(60),
    @date_from date,
    @date_to date
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.report(employee_id, report_date, report_type, total_sales, total_amount, file_path)
    SELECT
        @employee_id,
        CAST(GETDATE() AS date),
        @report_type,
        COUNT(*),
        ROUND(COALESCE(SUM(CASE WHEN sale_status <> N'Возврат' THEN sale_price * ticket_quantity ELSE 0 END),0),2),
        NULL
    FROM dbo.sale
    WHERE CAST(sale_date AS date) BETWEEN @date_from AND @date_to;

    SELECT CAST(SCOPE_IDENTITY() AS int) AS report_id;
END;
GO