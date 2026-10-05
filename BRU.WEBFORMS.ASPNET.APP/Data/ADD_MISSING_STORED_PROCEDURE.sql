USE [AutoparkDB]
GO

/****** Missing Stored Procedure: get_schedule_range ******/
CREATE OR ALTER PROCEDURE [dbo].[get_schedule_range]
    @date_from date,
    @date_to date
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM dbo.v_route_schedule
    WHERE service_date >= @date_from
      AND service_date < @date_to
    ORDER BY service_date, route_num, departure_time;
END;
GO
