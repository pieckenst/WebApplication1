using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BRU.WEBFORMS.ASPNET.APP.Models;

namespace BRU.WEBFORMS.ASPNET.APP.DataAccess
{
    /// <summary>
    /// Data access layer for Maintenance operations
    /// </summary>
    public class MaintenanceRepository : IDisposable
    {
        private DatabaseHelper _db;

        public MaintenanceRepository()
        {
            _db = new DatabaseHelper();
        }

        /// <summary>
        /// Get all maintenance records
        /// </summary>
        public List<Maintenance> GetAllMaintenance()
        {
            List<Maintenance> maintenanceList = new List<Maintenance>();
            
            using (SqlDataReader reader = _db.ExecuteReader(
                @"SELECT m.*, b.fleet_number,
                         DATEDIFF(DAY, m.maintenance_date, GETDATE()) AS days_from_last_service
                  FROM dbo.maintenance m
                  LEFT JOIN dbo.bus b ON m.bus_id = b.bus_id
                  ORDER BY m.maintenance_date DESC"))
            {
                while (reader.Read())
                {
                    maintenanceList.Add(MapMaintenanceFromReader(reader));
                }
            }
            
            return maintenanceList;
        }

        /// <summary>
        /// Get maintenance by bus ID
        /// </summary>
        public List<Maintenance> GetMaintenanceByBus(int busId)
        {
            List<Maintenance> maintenanceList = new List<Maintenance>();
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@bus_id", busId, SqlDbType.Int)
            };
            
            using (SqlDataReader reader = _db.ExecuteReader(
                "EXEC dbo.get_bus_maintenance @bus_id", 
                parameters))
            {
                while (reader.Read())
                {
                    maintenanceList.Add(MapMaintenanceFromReader(reader));
                }
            }
            
            return maintenanceList;
        }

        /// <summary>
        /// Get latest maintenance for all buses
        /// </summary>
        public List<Maintenance> GetLatestMaintenance()
        {
            List<Maintenance> maintenanceList = new List<Maintenance>();
            
            using (SqlDataReader reader = _db.ExecuteReader("SELECT * FROM dbo.v_latest_maintenance WHERE maintenance_seq = 1 ORDER BY fleet_number"))
            {
                while (reader.Read())
                {
                    maintenanceList.Add(MapMaintenanceFromReader(reader));
                }
            }
            
            return maintenanceList;
        }

        /// <summary>
        /// Insert a new maintenance record
        /// </summary>
        public int InsertMaintenance(Maintenance maintenance)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@bus_id", maintenance.BusId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@employee_id", maintenance.EmployeeId ?? (object)DBNull.Value, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@maintenance_type", maintenance.MaintenanceType, SqlDbType.NVarChar, 100),
                DatabaseHelper.CreateParameter("@found_issue", maintenance.FoundIssue ?? (object)DBNull.Value, SqlDbType.NVarChar, 500),
                DatabaseHelper.CreateParameter("@service_result", maintenance.ServiceResult ?? (object)DBNull.Value, SqlDbType.NVarChar, 500),
                DatabaseHelper.CreateParameter("@maintenance_cost", maintenance.MaintenanceCost, SqlDbType.Decimal)
            };
            
            string sql = @"EXEC dbo.add_maintenance @bus_id, @employee_id, @maintenance_type, @found_issue, @service_result, @maintenance_cost";
            object result = _db.ExecuteScalar(sql, parameters);
            return Convert.ToInt32(result);
        }

        /// <summary>
        /// Update an existing maintenance record
        /// </summary>
        public bool UpdateMaintenance(Maintenance maintenance)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@maintenance_id", maintenance.MaintenanceId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@bus_id", maintenance.BusId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@employee_id", maintenance.EmployeeId ?? (object)DBNull.Value, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@maintenance_date", maintenance.MaintenanceDate, SqlDbType.Date),
                DatabaseHelper.CreateParameter("@next_maintenance_date", maintenance.NextMaintenanceDate ?? (object)DBNull.Value, SqlDbType.Date),
                DatabaseHelper.CreateParameter("@maintenance_type", maintenance.MaintenanceType, SqlDbType.NVarChar, 100),
                DatabaseHelper.CreateParameter("@found_issue", maintenance.FoundIssue ?? (object)DBNull.Value, SqlDbType.NVarChar, 500),
                DatabaseHelper.CreateParameter("@service_result", maintenance.ServiceResult ?? (object)DBNull.Value, SqlDbType.NVarChar, 500),
                DatabaseHelper.CreateParameter("@mileage_km", maintenance.MileageKm ?? (object)DBNull.Value, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@roadworthiness", maintenance.Roadworthiness, SqlDbType.NVarChar, 30),
                DatabaseHelper.CreateParameter("@maintenance_cost", maintenance.MaintenanceCost, SqlDbType.Decimal)
            };
            
            string sql = @"UPDATE dbo.maintenance 
                          SET bus_id = @bus_id, 
                              employee_id = @employee_id, 
                              maintenance_date = @maintenance_date, 
                              next_maintenance_date = @next_maintenance_date,
                              maintenance_type = @maintenance_type, 
                              found_issue = @found_issue, 
                              service_result = @service_result,
                              mileage_km = @mileage_km,
                              roadworthiness = @roadworthiness,
                              maintenance_cost = @maintenance_cost
                          WHERE maintenance_id = @maintenance_id";
            
            int rowsAffected = _db.ExecuteNonQuery(sql, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Delete a maintenance record
        /// </summary>
        public bool DeleteMaintenance(int maintenanceId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@maintenance_id", maintenanceId, SqlDbType.Int)
            };
            
            int rowsAffected = _db.ExecuteNonQuery("DELETE FROM dbo.maintenance WHERE maintenance_id = @maintenance_id", parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Get autopark summary for dashboard
        /// </summary>
        public AutoparkSummary GetAutoparkSummary()
        {
            AutoparkSummary summary = new AutoparkSummary();
            
            using (SqlDataReader reader = _db.ExecuteReader("SELECT * FROM dbo.v_autopark_summary"))
            {
                if (reader.Read())
                {
                    summary.ActiveBusCount = reader.GetInt32(reader.GetOrdinal("active_bus_count"));
                    summary.ActiveEmployeeCount = reader.GetInt32(reader.GetOrdinal("active_employee_count"));
                    summary.ActiveRouteCount = reader.GetInt32(reader.GetOrdinal("active_route_count"));
                    summary.TodayScheduleCount = reader.GetInt32(reader.GetOrdinal("today_schedule_count"));
                    summary.TodaySaleCount = reader.GetInt32(reader.GetOrdinal("today_sale_count"));
                    summary.TodayAmountTotal = reader.GetDecimal(reader.GetOrdinal("today_amount_total"));
                    summary.BusNeedAttentionCount = reader.GetInt32(reader.GetOrdinal("bus_need_attention_count"));
                }
            }
            
            return summary;
        }

        /// <summary>
        /// Get sales by channel
        /// </summary>
        public List<SalesByChannel> GetSalesByChannel()
        {
            List<SalesByChannel> salesList = new List<SalesByChannel>();
            
            using (SqlDataReader reader = _db.ExecuteReader("SELECT * FROM dbo.v_sales_by_channel ORDER BY amount_total DESC"))
            {
                while (reader.Read())
                {
                    SalesByChannel sales = new SalesByChannel();
                    sales.SaleChannel = reader.GetString(reader.GetOrdinal("sale_channel"));
                    sales.SaleCount = reader.GetInt32(reader.GetOrdinal("sale_count"));
                    sales.TicketCount = reader.GetInt32(reader.GetOrdinal("ticket_count"));
                    sales.AmountTotal = reader.GetDecimal(reader.GetOrdinal("amount_total"));
                    sales.AverageTicketPrice = reader.GetDecimal(reader.GetOrdinal("average_ticket_price"));
                    salesList.Add(sales);
                }
            }
            
            return salesList;
        }

        /// <summary>
        /// Get sales by route
        /// </summary>
        public List<SalesByRoute> GetSalesByRoute()
        {
            List<SalesByRoute> salesList = new List<SalesByRoute>();
            
            using (SqlDataReader reader = _db.ExecuteReader("SELECT * FROM dbo.v_sales_by_route ORDER BY amount_total DESC"))
            {
                while (reader.Read())
                {
                    SalesByRoute sales = new SalesByRoute();
                    sales.RouteNum = reader.GetString(reader.GetOrdinal("route_num"));
                    sales.RouteName = reader.GetString(reader.GetOrdinal("route_name"));
                    sales.SaleCount = reader.GetInt32(reader.GetOrdinal("sale_count"));
                    sales.TicketCount = reader.GetInt32(reader.GetOrdinal("ticket_count"));
                    sales.AmountTotal = reader.GetDecimal(reader.GetOrdinal("amount_total"));
                    salesList.Add(sales);
                }
            }
            
            return salesList;
        }

        /// <summary>
        /// Get sales by employee
        /// </summary>
        public List<SalesByEmployee> GetSalesByEmployee()
        {
            List<SalesByEmployee> salesList = new List<SalesByEmployee>();
            
            using (SqlDataReader reader = _db.ExecuteReader("SELECT * FROM dbo.v_sales_by_employee ORDER BY amount_total DESC"))
            {
                while (reader.Read())
                {
                    SalesByEmployee sales = new SalesByEmployee();
                    sales.EmployeeId = reader.GetInt32(reader.GetOrdinal("employee_id"));
                    sales.EmployeeName = reader.GetString(reader.GetOrdinal("employee_name"));
                    sales.JobTitle = reader.GetString(reader.GetOrdinal("job_title"));
                    sales.SaleCount = reader.GetInt32(reader.GetOrdinal("sale_count"));
                    sales.AmountTotal = reader.GetDecimal(reader.GetOrdinal("amount_total"));
                    salesList.Add(sales);
                }
            }
            
            return salesList;
        }

        /// <summary>
        /// Map SqlDataReader to Maintenance object
        /// </summary>
        private Maintenance MapMaintenanceFromReader(SqlDataReader reader)
        {
            Maintenance maintenance = new Maintenance();
            
            maintenance.MaintenanceId = reader.GetInt32(reader.GetOrdinal("maintenance_id"));
            maintenance.BusId = reader.GetInt32(reader.GetOrdinal("bus_id"));
            maintenance.EmployeeId = reader.IsDBNull(reader.GetOrdinal("employee_id")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("employee_id"));
            maintenance.MaintenanceDate = reader.GetDateTime(reader.GetOrdinal("maintenance_date"));
            maintenance.NextMaintenanceDate = reader.IsDBNull(reader.GetOrdinal("next_maintenance_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("next_maintenance_date"));
            maintenance.MaintenanceType = reader.GetString(reader.GetOrdinal("maintenance_type"));
            maintenance.FoundIssue = reader.IsDBNull(reader.GetOrdinal("found_issue")) ? null : reader.GetString(reader.GetOrdinal("found_issue"));
            maintenance.ServiceResult = reader.IsDBNull(reader.GetOrdinal("service_result")) ? null : reader.GetString(reader.GetOrdinal("service_result"));
            maintenance.MileageKm = reader.IsDBNull(reader.GetOrdinal("mileage_km")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("mileage_km"));
            maintenance.Roadworthiness = reader.GetString(reader.GetOrdinal("roadworthiness"));
            maintenance.MaintenanceCost = reader.GetDecimal(reader.GetOrdinal("maintenance_cost"));
            
            // Optional fields (present when joined with bus table or returned from views/SPs)
            int fleetNumberOrdinal = TryGetOrdinal(reader, "fleet_number");
            if (fleetNumberOrdinal >= 0 && !reader.IsDBNull(fleetNumberOrdinal))
            {
                maintenance.FleetNumber = reader.GetString(fleetNumberOrdinal);
            }

            int daysOrdinal = TryGetOrdinal(reader, "days_from_last_service");
            if (daysOrdinal >= 0 && !reader.IsDBNull(daysOrdinal))
            {
                maintenance.DaysFromLastService = reader.GetInt32(daysOrdinal);
            }
            
            return maintenance;
        }

        /// <summary>
        /// Safely get the column ordinal without throwing if the column is absent.
        /// </summary>
        private static int TryGetOrdinal(SqlDataReader reader, string columnName)
        {
            if (string.IsNullOrEmpty(columnName)) return -1;
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (string.Equals(reader.GetName(i), columnName, StringComparison.OrdinalIgnoreCase))
                    return i;
            }
            return -1;
        }

        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
                _db = null;
            }
        }
    }
}