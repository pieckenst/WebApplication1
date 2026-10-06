using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BRU.WEBFORMS.ASPNET.APP.Models;

namespace BRU.WEBFORMS.ASPNET.APP.DataAccess
{
    /// <summary>
    /// Data access layer for Bus operations
    /// </summary>
    public class BusRepository : IDisposable
    {
        private DatabaseHelper _db;

        public BusRepository()
        {
            _db = new DatabaseHelper();
        }

        /// <summary>
        /// Get all buses from the view
        /// </summary>
        public List<Bus> GetAllBuses()
        {
            List<Bus> buses = new List<Bus>();
            
            using (SqlDataReader reader = _db.ExecuteReader("SELECT * FROM dbo.v_bus_status ORDER BY fleet_number"))
            {
                while (reader.Read())
                {
                    buses.Add(MapBusFromReader(reader));
                }
            }
            
            return buses;
        }

        /// <summary>
        /// Get active buses only
        /// </summary>
        public List<Bus> GetActiveBuses()
        {
            List<Bus> buses = new List<Bus>();
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@status", BusStatus.Operational, SqlDbType.NVarChar, 30)
            };
            
            using (SqlDataReader reader = _db.ExecuteReader(
                "SELECT * FROM dbo.v_bus_status WHERE status = @status ORDER BY fleet_number", 
                parameters))
            {
                while (reader.Read())
                {
                    buses.Add(MapBusFromReader(reader));
                }
            }
            
            return buses;
        }

        /// <summary>
        /// Get bus by ID
        /// </summary>
        public Bus GetBusById(int busId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@bus_id", busId, SqlDbType.Int)
            };
            
            using (SqlDataReader reader = _db.ExecuteReader(
                "SELECT * FROM dbo.bus WHERE bus_id = @bus_id", 
                parameters))
            {
                if (reader.Read())
                {
                    return MapBusFromReader(reader);
                }
            }
            
            return null;
        }

        /// <summary>
        /// Get bus by fleet number
        /// </summary>
        public Bus GetBusByFleetNumber(string fleetNumber)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@fleet_number", fleetNumber, SqlDbType.VarChar, 20)
            };
            
            using (SqlDataReader reader = _db.ExecuteReader(
                "SELECT * FROM dbo.bus WHERE fleet_number = @fleet_number", 
                parameters))
            {
                if (reader.Read())
                {
                    return MapBusFromReader(reader);
                }
            }
            
            return null;
        }

        /// <summary>
        /// Insert a new bus
        /// </summary>
        public int InsertBus(Bus bus)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@fleet_number", bus.FleetNumber, SqlDbType.VarChar, 20),
                DatabaseHelper.CreateParameter("@registration_num", bus.RegistrationNum, SqlDbType.VarChar, 20),
                DatabaseHelper.CreateParameter("@model", bus.Model, SqlDbType.NVarChar, 80),
                DatabaseHelper.CreateParameter("@manufacturer", bus.Manufacturer, SqlDbType.NVarChar, 80),
                DatabaseHelper.CreateParameter("@manufacture_year", bus.ManufactureYear, SqlDbType.SmallInt),
                DatabaseHelper.CreateParameter("@capacity", bus.Capacity, SqlDbType.SmallInt),
                DatabaseHelper.CreateParameter("@status", bus.Status, SqlDbType.NVarChar, 30),
                DatabaseHelper.CreateParameter("@mileage_km", bus.MileageKm, SqlDbType.Int)
            };
            
            string sql = @"INSERT INTO dbo.bus (fleet_number, registration_num, model, manufacturer, manufacture_year, capacity, status, mileage_km)
                          VALUES (@fleet_number, @registration_num, @model, @manufacturer, @manufacture_year, @capacity, @status, @mileage_km);
                          SELECT CAST(SCOPE_IDENTITY() AS int)";
            
            object result = _db.ExecuteScalar(sql, parameters);
            return Convert.ToInt32(result);
        }

        /// <summary>
        /// Update an existing bus
        /// </summary>
        public bool UpdateBus(Bus bus)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@bus_id", bus.BusId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@fleet_number", bus.FleetNumber, SqlDbType.VarChar, 20),
                DatabaseHelper.CreateParameter("@registration_num", bus.RegistrationNum, SqlDbType.VarChar, 20),
                DatabaseHelper.CreateParameter("@model", bus.Model, SqlDbType.NVarChar, 80),
                DatabaseHelper.CreateParameter("@manufacturer", bus.Manufacturer, SqlDbType.NVarChar, 80),
                DatabaseHelper.CreateParameter("@manufacture_year", bus.ManufactureYear, SqlDbType.SmallInt),
                DatabaseHelper.CreateParameter("@capacity", bus.Capacity, SqlDbType.SmallInt),
                DatabaseHelper.CreateParameter("@status", bus.Status, SqlDbType.NVarChar, 30),
                DatabaseHelper.CreateParameter("@mileage_km", bus.MileageKm, SqlDbType.Int)
            };
            
            string sql = @"UPDATE dbo.bus 
                          SET fleet_number = @fleet_number, 
                              registration_num = @registration_num, 
                              model = @model, 
                              manufacturer = @manufacturer, 
                              manufacture_year = @manufacture_year, 
                              capacity = @capacity, 
                              status = @status, 
                              mileage_km = @mileage_km
                          WHERE bus_id = @bus_id";
            
            int rowsAffected = _db.ExecuteNonQuery(sql, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Update bus status
        /// </summary>
        public bool UpdateBusStatus(int busId, string status)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@bus_id", busId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@status", status, SqlDbType.NVarChar, 30)
            };
            
            string sql = "UPDATE dbo.bus SET status = @status WHERE bus_id = @bus_id";
            int rowsAffected = _db.ExecuteNonQuery(sql, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Delete a bus
        /// </summary>
        public bool DeleteBus(int busId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@bus_id", busId, SqlDbType.Int)
            };
            
            int rowsAffected = _db.ExecuteNonQuery("DELETE FROM dbo.bus WHERE bus_id = @bus_id", parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Map SqlDataReader to Bus object
        /// </summary>
        private Bus MapBusFromReader(SqlDataReader reader)
        {
            Bus bus = new Bus();
            
            bus.BusId = reader.GetInt32(reader.GetOrdinal("bus_id"));
            bus.FleetNumber = reader.GetString(reader.GetOrdinal("fleet_number"));
            bus.RegistrationNum = reader.GetString(reader.GetOrdinal("registration_num"));
            bus.Model = reader.GetString(reader.GetOrdinal("model"));
            bus.Manufacturer = reader.GetString(reader.GetOrdinal("manufacturer"));
            bus.ManufactureYear = reader.GetInt16(reader.GetOrdinal("manufacture_year"));
            bus.Capacity = reader.GetInt16(reader.GetOrdinal("capacity"));
            bus.Status = reader.GetString(reader.GetOrdinal("status"));
            bus.MileageKm = reader.GetInt32(reader.GetOrdinal("mileage_km"));
            
            int mileageCategoryOrdinal = TryGetOrdinal(reader, "mileage_category");
            if (mileageCategoryOrdinal >= 0 && !reader.IsDBNull(mileageCategoryOrdinal))
            {
                bus.MileageCategory = reader.GetString(mileageCategoryOrdinal);
            }
            
            return bus;
        }

        private static int TryGetOrdinal(SqlDataReader reader, string columnName)
        {
            for (int ordinal = 0; ordinal < reader.FieldCount; ordinal++)
            {
                if (string.Equals(reader.GetName(ordinal), columnName, StringComparison.OrdinalIgnoreCase))
                    return ordinal;
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