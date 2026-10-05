using System;
using System.Data;
using System.Data.SqlClient;
using BRU.WEBFORMS.ASPNET.APP.Models;

namespace BRU.WEBFORMS.ASPNET.APP.DataAccess
{
    public sealed class ReportsRepository : IDisposable
    {
        private readonly DatabaseHelper _db = new DatabaseHelper();

        public SalesReportData GetSalesReport(DateTime dateFrom, DateTime dateTo, string channel, string status, int pageIndex, int pageSize)
        {
            int offset = checked(pageIndex * pageSize);
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@date_from", dateFrom, SqlDbType.DateTime2),
                DatabaseHelper.CreateParameter("@date_to", dateTo, SqlDbType.DateTime2),
                DatabaseHelper.CreateParameter("@channel", string.IsNullOrWhiteSpace(channel) ? (object)DBNull.Value : channel, SqlDbType.NVarChar, 30),
                DatabaseHelper.CreateParameter("@status", string.IsNullOrWhiteSpace(status) ? (object)DBNull.Value : status, SqlDbType.NVarChar, 30),
                DatabaseHelper.CreateParameter("@offset", offset, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@page_size", pageSize, SqlDbType.Int)
            };

            string filter = @"
                FROM dbo.sale AS s
                WHERE s.sale_date >= @date_from AND s.sale_date < @date_to
                  AND (@channel IS NULL OR s.sale_channel = @channel)
                  AND (@status IS NULL OR s.sale_status = @status)";
            string sql = @"
                SELECT COUNT_BIG(*) AS sale_count,
                       COALESCE(SUM(CASE WHEN s.sale_status NOT IN (N'Возврат', N'Отменена') THEN s.ticket_quantity ELSE 0 END), 0) AS ticket_count,
                       COALESCE(SUM(s.sale_price * s.ticket_quantity), 0) AS gross_amount,
                       COALESCE(SUM(CASE WHEN s.sale_status = N'Возврат' THEN s.sale_price * s.ticket_quantity ELSE 0 END), 0) AS refunded_amount,
                       COALESCE(SUM(CASE WHEN s.sale_status NOT IN (N'Возврат', N'Отменена') THEN s.sale_price * s.ticket_quantity ELSE 0 END), 0) AS net_amount
                " + filter + @";
                SELECT COUNT_BIG(*) AS total_rows " + filter + @";
                SELECT s.sale_id, s.sale_date, COALESCE(r.route_num, N'Unassigned') AS route_number,
                       t.ticket_name, s.ticket_quantity, s.sale_price,
                       s.sale_price * s.ticket_quantity AS total_amount,
                       s.sale_channel, s.sale_status, s.payment_status
                FROM dbo.sale AS s
                INNER JOIN dbo.ticket AS t ON t.ticket_id = s.ticket_id
                LEFT JOIN dbo.route_schedule AS rs ON rs.schedule_id = s.schedule_id
                LEFT JOIN dbo.route AS r ON r.route_id = rs.route_id
                WHERE s.sale_date >= @date_from AND s.sale_date < @date_to
                  AND (@channel IS NULL OR s.sale_channel = @channel)
                  AND (@status IS NULL OR s.sale_status = @status)
                ORDER BY s.sale_date DESC, s.sale_id DESC
                OFFSET @offset ROWS FETCH NEXT @page_size ROWS ONLY;";

            DataSet data = _db.ExecuteDataSet(sql, parameters);
            SalesReportData result = new SalesReportData();
            if (data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
            {
                DataRow row = data.Tables[0].Rows[0];
                result.Summary = new SalesReportSummary
                {
                    SaleCount = Convert.ToInt64(row["sale_count"]),
                    TicketCount = Convert.ToInt64(row["ticket_count"]),
                    GrossAmount = Convert.ToDecimal(row["gross_amount"]),
                    RefundedAmount = Convert.ToDecimal(row["refunded_amount"]),
                    NetAmount = Convert.ToDecimal(row["net_amount"])
                };
            }
            if (data.Tables.Count > 1 && data.Tables[1].Rows.Count > 0)
                result.TotalRows = Convert.ToInt64(data.Tables[1].Rows[0]["total_rows"]);
            if (data.Tables.Count > 2)
            {
                foreach (DataRow row in data.Tables[2].Rows)
                {
                    result.Rows.Add(new SalesReportRow
                    {
                        SaleId = Convert.ToInt64(row["sale_id"]),
                        SaleDate = Convert.ToDateTime(row["sale_date"]),
                        RouteNumber = Convert.ToString(row["route_number"]),
                        TicketName = Convert.ToString(row["ticket_name"]),
                        TicketQuantity = Convert.ToInt32(row["ticket_quantity"]),
                        SalePrice = Convert.ToDecimal(row["sale_price"]),
                        TotalAmount = Convert.ToDecimal(row["total_amount"]),
                        SaleChannel = Convert.ToString(row["sale_channel"]),
                        SaleStatus = Convert.ToString(row["sale_status"]),
                        PaymentStatus = Convert.ToString(row["payment_status"])
                    });
                }
            }
            return result;
        }

        public MaintenanceReportData GetMaintenanceReport(DateTime dateFrom, DateTime dateTo, int? busId, string roadworthiness, int pageIndex, int pageSize)
        {
            int offset = checked(pageIndex * pageSize);
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@date_from", dateFrom, SqlDbType.Date),
                DatabaseHelper.CreateParameter("@date_to", dateTo, SqlDbType.Date),
                DatabaseHelper.CreateParameter("@bus_id", busId ?? (object)DBNull.Value, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@roadworthiness", string.IsNullOrWhiteSpace(roadworthiness) ? (object)DBNull.Value : roadworthiness, SqlDbType.NVarChar, 30),
                DatabaseHelper.CreateParameter("@offset", offset, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@page_size", pageSize, SqlDbType.Int)
            };
            string filter = @"
                FROM dbo.maintenance AS m
                WHERE m.maintenance_date >= @date_from AND m.maintenance_date < @date_to
                  AND (@bus_id IS NULL OR m.bus_id = @bus_id)
                  AND (@roadworthiness IS NULL OR m.roadworthiness = @roadworthiness)";
            string sql = @"
                SELECT COUNT_BIG(*) AS record_count,
                       COALESCE(SUM(m.maintenance_cost), 0) AS total_cost,
                       COALESCE(SUM(CASE WHEN m.roadworthiness <> N'Исправен' THEN 1 ELSE 0 END), 0) AS not_roadworthy_count,
                       COALESCE(SUM(CASE WHEN m.next_maintenance_date >= CAST(GETDATE() AS date)
                                              AND m.next_maintenance_date < DATEADD(DAY, 30, CAST(GETDATE() AS date)) THEN 1 ELSE 0 END), 0) AS upcoming_count
                " + filter + @";
                SELECT COUNT_BIG(*) AS total_rows " + filter + @";
                SELECT m.maintenance_id, b.fleet_number, b.model, m.maintenance_date,
                       m.next_maintenance_date, m.maintenance_type, m.roadworthiness,
                       m.maintenance_cost, COALESCE(CONCAT(e.surname, N' ', e.name), N'Unassigned') AS employee_name
                FROM dbo.maintenance AS m
                INNER JOIN dbo.bus AS b ON b.bus_id = m.bus_id
                LEFT JOIN dbo.employee AS e ON e.employee_id = m.employee_id
                WHERE m.maintenance_date >= @date_from AND m.maintenance_date < @date_to
                  AND (@bus_id IS NULL OR m.bus_id = @bus_id)
                  AND (@roadworthiness IS NULL OR m.roadworthiness = @roadworthiness)
                ORDER BY m.maintenance_date DESC, m.maintenance_id DESC
                OFFSET @offset ROWS FETCH NEXT @page_size ROWS ONLY;";

            DataSet data = _db.ExecuteDataSet(sql, parameters);
            MaintenanceReportData result = new MaintenanceReportData();
            if (data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
            {
                DataRow row = data.Tables[0].Rows[0];
                result.Summary = new MaintenanceReportSummary
                {
                    RecordCount = Convert.ToInt64(row["record_count"]),
                    TotalCost = Convert.ToDecimal(row["total_cost"]),
                    NotRoadworthyCount = Convert.ToInt64(row["not_roadworthy_count"]),
                    UpcomingCount = Convert.ToInt64(row["upcoming_count"])
                };
            }
            if (data.Tables.Count > 1 && data.Tables[1].Rows.Count > 0)
                result.TotalRows = Convert.ToInt64(data.Tables[1].Rows[0]["total_rows"]);
            if (data.Tables.Count > 2)
            {
                foreach (DataRow row in data.Tables[2].Rows)
                {
                    result.Rows.Add(new MaintenanceReportRow
                    {
                        MaintenanceId = Convert.ToInt32(row["maintenance_id"]),
                        FleetNumber = Convert.ToString(row["fleet_number"]),
                        BusModel = Convert.ToString(row["model"]),
                        MaintenanceDate = Convert.ToDateTime(row["maintenance_date"]),
                        NextMaintenanceDate = row.IsNull("next_maintenance_date") ? (DateTime?)null : Convert.ToDateTime(row["next_maintenance_date"]),
                        MaintenanceType = Convert.ToString(row["maintenance_type"]),
                        Roadworthiness = Convert.ToString(row["roadworthiness"]),
                        MaintenanceCost = Convert.ToDecimal(row["maintenance_cost"]),
                        EmployeeName = Convert.ToString(row["employee_name"])
                    });
                }
            }
            return result;
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
