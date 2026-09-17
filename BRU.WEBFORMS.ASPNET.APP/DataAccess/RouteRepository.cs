using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BRU.WEBFORMS.ASPNET.APP.Models;

namespace BRU.WEBFORMS.ASPNET.APP.DataAccess
{
    /// <summary>
    /// Data access layer for Route operations
    /// </summary>
    public class RouteRepository : IDisposable
    {
        private DatabaseHelper _db;

        public RouteRepository()
        {
            _db = new DatabaseHelper();
        }

        /// <summary>
        /// Get all active routes
        /// </summary>
        public List<Route> GetAllActiveRoutes()
        {
            List<Route> routes = new List<Route>();
            
            using (SqlDataReader reader = _db.ExecuteReader("SELECT * FROM dbo.v_active_route ORDER BY route_num"))
            {
                while (reader.Read())
                {
                    routes.Add(MapRouteFromReader(reader));
                }
            }
            
            return routes;
        }

        /// <summary>
        /// Get route by ID
        /// </summary>
        public Route GetRouteById(int routeId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@route_id", routeId, SqlDbType.Int)
            };
            
            using (SqlDataReader reader = _db.ExecuteReader(
                "SELECT * FROM dbo.route WHERE route_id = @route_id", 
                parameters))
            {
                if (reader.Read())
                {
                    return MapRouteFromReader(reader);
                }
            }
            
            return null;
        }

        /// <summary>
        /// Get route details with stop count
        /// </summary>
        public Route GetRouteDetails(int routeId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@route_id", routeId, SqlDbType.Int)
            };
            
            using (SqlDataReader reader = _db.ExecuteReader(
                "EXEC dbo.get_route_details @route_id", 
                parameters))
            {
                if (reader.Read())
                {
                    Route route = new Route();
                    route.RouteId = reader.GetInt32(reader.GetOrdinal("route_id"));
                    route.RouteNum = reader.GetString(reader.GetOrdinal("route_num"));
                    route.RouteName = reader.GetString(reader.GetOrdinal("route_name"));
                    route.StartStop = reader.GetString(reader.GetOrdinal("start_stop"));
                    route.EndStop = reader.GetString(reader.GetOrdinal("end_stop"));
                    route.StopCount = reader.GetInt32(reader.GetOrdinal("stop_count"));
                    return route;
                }
            }
            
            return null;
        }

        /// <summary>
        /// Get route stops
        /// </summary>
        public List<RouteStop> GetRouteStops(int routeId)
        {
            List<RouteStop> routeStops = new List<RouteStop>();
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@route_id", routeId, SqlDbType.Int)
            };
            
            using (SqlDataReader reader = _db.ExecuteReader(
                "SELECT * FROM dbo.v_route_stops WHERE route_id = @route_id ORDER BY stop_sequence", 
                parameters))
            {
                while (reader.Read())
                {
                    routeStops.Add(MapRouteStopFromReader(reader));
                }
            }
            
            return routeStops;
        }

        /// <summary>
        /// Get route schedule for a specific date
        /// </summary>
        public List<RouteSchedule> GetRouteSchedule(int routeId, DateTime serviceDate)
        {
            List<RouteSchedule> schedules = new List<RouteSchedule>();
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@route_id", routeId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@service_date", serviceDate, SqlDbType.Date)
            };
            
            using (SqlDataReader reader = _db.ExecuteReader(
                "EXEC dbo.get_route_schedule @route_id, @service_date", 
                parameters))
            {
                while (reader.Read())
                {
                    schedules.Add(MapRouteScheduleFromReader(reader));
                }
            }
            
            return schedules;
        }

        /// <summary>
        /// Get schedule for date range
        /// </summary>
        public List<RouteSchedule> GetScheduleRange(DateTime dateFrom, DateTime dateTo)
        {
            List<RouteSchedule> schedules = new List<RouteSchedule>();
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@date_from", dateFrom, SqlDbType.Date),
                DatabaseHelper.CreateParameter("@date_to", dateTo, SqlDbType.Date)
            };
            
            using (SqlDataReader reader = _db.ExecuteReader(
                "EXEC dbo.get_schedule_range @date_from, @date_to", 
                parameters))
            {
                while (reader.Read())
                {
                    schedules.Add(MapRouteScheduleFromReader(reader));
                }
            }
            
            return schedules;
        }

        /// <summary>
        /// Insert a new route
        /// </summary>
        public int InsertRoute(Route route)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@route_num", route.RouteNum, SqlDbType.VarChar, 20),
                DatabaseHelper.CreateParameter("@route_name", route.RouteName, SqlDbType.NVarChar, 120),
                DatabaseHelper.CreateParameter("@start_stop", route.StartStop, SqlDbType.NVarChar, 120),
                DatabaseHelper.CreateParameter("@end_stop", route.EndStop, SqlDbType.NVarChar, 120)
            };
            
            string sql = @"EXEC dbo.add_route @route_num, @route_name, @start_stop, @end_stop";
            object result = _db.ExecuteScalar(sql, parameters);
            return Convert.ToInt32(result);
        }

        /// <summary>
        /// Update an existing route
        /// </summary>
        public bool UpdateRoute(Route route)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@route_id", route.RouteId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@route_num", route.RouteNum, SqlDbType.VarChar, 20),
                DatabaseHelper.CreateParameter("@route_name", route.RouteName, SqlDbType.NVarChar, 120),
                DatabaseHelper.CreateParameter("@start_stop", route.StartStop, SqlDbType.NVarChar, 120),
                DatabaseHelper.CreateParameter("@end_stop", route.EndStop, SqlDbType.NVarChar, 120),
                DatabaseHelper.CreateParameter("@is_active", route.IsActive, SqlDbType.Bit)
            };
            
            string sql = @"UPDATE dbo.route 
                          SET route_num = @route_num, 
                              route_name = @route_name, 
                              start_stop = @start_stop, 
                              end_stop = @end_stop,
                              is_active = @is_active
                          WHERE route_id = @route_id";
            
            int rowsAffected = _db.ExecuteNonQuery(sql, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Delete a route
        /// </summary>
        public bool DeleteRoute(int routeId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@route_id", routeId, SqlDbType.Int)
            };
            
            int rowsAffected = _db.ExecuteNonQuery("DELETE FROM dbo.route WHERE route_id = @route_id", parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Get all stops
        /// </summary>
        public List<Stop> GetAllStops()
        {
            List<Stop> stops = new List<Stop>();
            
            using (SqlDataReader reader = _db.ExecuteReader("SELECT * FROM dbo.stop WHERE is_active = 1 ORDER BY stop_name"))
            {
                while (reader.Read())
                {
                    stops.Add(MapStopFromReader(reader));
                }
            }
            
            return stops;
        }

        /// <summary>
        /// Map SqlDataReader to Route object
        /// </summary>
        private Route MapRouteFromReader(SqlDataReader reader)
        {
            Route route = new Route();
            
            route.RouteId = reader.GetInt32(reader.GetOrdinal("route_id"));
            route.RouteNum = reader.GetString(reader.GetOrdinal("route_num"));
            route.RouteName = reader.GetString(reader.GetOrdinal("route_name"));
            route.StartStop = reader.GetString(reader.GetOrdinal("start_stop"));
            route.EndStop = reader.GetString(reader.GetOrdinal("end_stop"));
            
            if (reader.FieldCount > 5 && !reader.IsDBNull(reader.GetOrdinal("is_active")))
            {
                route.IsActive = reader.GetBoolean(reader.GetOrdinal("is_active"));
            }
            
            return route;
        }

        /// <summary>
        /// Map SqlDataReader to RouteStop object
        /// </summary>
        private RouteStop MapRouteStopFromReader(SqlDataReader reader)
        {
            RouteStop routeStop = new RouteStop();
            
            routeStop.RouteNum = reader.GetString(reader.GetOrdinal("route_num"));
            routeStop.RouteName = reader.GetString(reader.GetOrdinal("route_name"));
            routeStop.StopSequence = reader.GetInt16(reader.GetOrdinal("stop_sequence"));
            routeStop.StopName = reader.GetString(reader.GetOrdinal("stop_name"));
            routeStop.StopLocation = reader.IsDBNull(reader.GetOrdinal("location")) ? null : reader.GetString(reader.GetOrdinal("location"));
            routeStop.DistanceKm = reader.IsDBNull(reader.GetOrdinal("distance_km")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("distance_km"));
            routeStop.ArrivalOffsetMin = reader.IsDBNull(reader.GetOrdinal("arrival_offset_min")) ? (short?)null : reader.GetInt16(reader.GetOrdinal("arrival_offset_min"));
            
            return routeStop;
        }

        /// <summary>
        /// Map SqlDataReader to RouteSchedule object
        /// </summary>
        private RouteSchedule MapRouteScheduleFromReader(SqlDataReader reader)
        {
            RouteSchedule schedule = new RouteSchedule();
            
            schedule.ScheduleId = reader.GetInt32(reader.GetOrdinal("schedule_id"));
            schedule.ServiceDate = reader.GetDateTime(reader.GetOrdinal("service_date"));
            schedule.RouteNum = reader.GetString(reader.GetOrdinal("route_num"));
            schedule.RouteName = reader.GetString(reader.GetOrdinal("route_name"));
            schedule.FleetNumber = reader.GetString(reader.GetOrdinal("fleet_number"));
            schedule.BusModel = reader.GetString(reader.GetOrdinal("model"));
            schedule.DriverName = reader.GetString(reader.GetOrdinal("driver_name"));
            schedule.DepartureTime = reader.GetTimeSpan(reader.GetOrdinal("departure_time"));
            schedule.ArrivalTime = reader.GetTimeSpan(reader.GetOrdinal("arrival_time"));
            schedule.TripMinutes = reader.GetInt32(reader.GetOrdinal("trip_minutes"));
            schedule.AvailableSeatNum = reader.GetInt16(reader.GetOrdinal("available_seat_num"));
            schedule.ScheduleStatus = reader.GetString(reader.GetOrdinal("schedule_status"));
            
            return schedule;
        }

        /// <summary>
        /// Map SqlDataReader to Stop object
        /// </summary>
        private Stop MapStopFromReader(SqlDataReader reader)
        {
            Stop stop = new Stop();
            
            stop.StopId = reader.GetInt32(reader.GetOrdinal("stop_id"));
            stop.StopName = reader.GetString(reader.GetOrdinal("stop_name"));
            stop.Location = reader.IsDBNull(reader.GetOrdinal("location")) ? null : reader.GetString(reader.GetOrdinal("location"));
            stop.Latitude = reader.IsDBNull(reader.GetOrdinal("latitude")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("latitude"));
            stop.Longitude = reader.IsDBNull(reader.GetOrdinal("longitude")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("longitude"));
            stop.IsActive = reader.GetBoolean(reader.GetOrdinal("is_active"));
            
            return stop;
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