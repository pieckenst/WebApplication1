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
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@route_id", routeId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@service_date", serviceDate, SqlDbType.Date)
            };
            
            return GetSchedules(
                "rs.route_id = @route_id AND rs.service_date = @service_date",
                parameters);
        }

        /// <summary>
        /// Get schedule for date range
        /// </summary>
        public List<RouteSchedule> GetScheduleRange(DateTime dateFrom, DateTime dateTo)
        {
            if (dateFrom >= dateTo)
                throw new ArgumentException("End date must be after start date.", "dateTo");

            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@date_from", dateFrom, SqlDbType.Date),
                DatabaseHelper.CreateParameter("@date_to", dateTo, SqlDbType.Date)
            };

            return GetSchedules(
                "rs.service_date >= @date_from AND rs.service_date < @date_to",
                parameters);
        }

        public RouteSchedule GetScheduleById(int scheduleId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@schedule_id", scheduleId, SqlDbType.Int)
            };
            List<RouteSchedule> schedules = GetSchedules("rs.schedule_id = @schedule_id", parameters);
            return schedules.Count == 0 ? null : schedules[0];
        }

        public int InsertSchedule(RouteSchedule schedule)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@route_id", schedule.RouteId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@bus_id", schedule.BusId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@driver_id", schedule.DriverId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@service_date", schedule.ServiceDate, SqlDbType.Date),
                DatabaseHelper.CreateParameter("@departure_time", schedule.DepartureTime, SqlDbType.Time),
                DatabaseHelper.CreateParameter("@arrival_time", schedule.ArrivalTime, SqlDbType.Time),
                DatabaseHelper.CreateParameter("@available_seat_num", schedule.AvailableSeatNum, SqlDbType.SmallInt),
                DatabaseHelper.CreateParameter("@schedule_status", schedule.ScheduleStatus, SqlDbType.NVarChar, 30)
            };
            return Convert.ToInt32(_db.ExecuteScalar(@"
                INSERT INTO dbo.route_schedule
                    (route_id, bus_id, driver_id, service_date, departure_time, arrival_time, available_seat_num, schedule_status)
                VALUES
                    (@route_id, @bus_id, @driver_id, @service_date, @departure_time, @arrival_time, @available_seat_num, @schedule_status);
                SELECT CAST(SCOPE_IDENTITY() AS int);", parameters));
        }

        public int SaveSchedule(RouteSchedule schedule)
        {
            _db.BeginTransaction(IsolationLevel.Serializable);
            try
            {
                SqlParameter[] validationParameters = new SqlParameter[]
                {
                    DatabaseHelper.CreateParameter("@schedule_id", schedule.ScheduleId, SqlDbType.Int),
                    DatabaseHelper.CreateParameter("@route_id", schedule.RouteId, SqlDbType.Int),
                    DatabaseHelper.CreateParameter("@bus_id", schedule.BusId, SqlDbType.Int),
                    DatabaseHelper.CreateParameter("@driver_id", schedule.DriverId, SqlDbType.Int),
                    DatabaseHelper.CreateParameter("@service_date", schedule.ServiceDate, SqlDbType.Date),
                    DatabaseHelper.CreateParameter("@departure_time", schedule.DepartureTime, SqlDbType.Time),
                    DatabaseHelper.CreateParameter("@arrival_time", schedule.ArrivalTime, SqlDbType.Time),
                    DatabaseHelper.CreateParameter("@cancelled_status", ScheduleStatus.Cancelled, SqlDbType.NVarChar, 30),
                    DatabaseHelper.CreateParameter("@working_status", EmployeeStatus.Working, SqlDbType.NVarChar, 30),
                    DatabaseHelper.CreateParameter("@operational_status", BusStatus.Operational, SqlDbType.NVarChar, 30),
                    DatabaseHelper.CreateParameter("@driver_job_id", 1, SqlDbType.Int),
                    DatabaseHelper.CreateParameter("@refunded_sale_status", SaleStatus.Refunded, SqlDbType.NVarChar, 30),
                    DatabaseHelper.CreateParameter("@cancelled_sale_status", SaleStatus.Cancelled, SqlDbType.NVarChar, 30)
                };
                int valid = Convert.ToInt32(_db.ExecuteScalar(@"
                    SELECT CASE WHEN
                        EXISTS (SELECT 1 FROM dbo.route WHERE route_id = @route_id AND is_active = 1)
                        AND EXISTS (SELECT 1 FROM dbo.bus WITH (UPDLOCK, HOLDLOCK)
                                    WHERE bus_id = @bus_id AND status = @operational_status)
                        AND EXISTS (SELECT 1 FROM dbo.employee AS e WITH (UPDLOCK, HOLDLOCK)
                                    INNER JOIN dbo.job AS j ON j.job_id = e.job_id
                                    WHERE e.employee_id = @driver_id AND e.status = @working_status
                                                                            AND (j.job_id = @driver_job_id OR j.job_title = N'Водитель автобуса'))
                        AND NOT EXISTS (SELECT 1 FROM dbo.maintenance WITH (UPDLOCK, HOLDLOCK)
                                        WHERE bus_id = @bus_id
                                          AND (maintenance_date = @service_date OR next_maintenance_date = @service_date))
                                                AND (@schedule_id = 0 OR EXISTS
                                                (
                                                        SELECT 1
                                                        FROM dbo.route_schedule AS current_schedule WITH (UPDLOCK, HOLDLOCK)
                                                        INNER JOIN dbo.bus AS target_bus ON target_bus.bus_id = @bus_id
                                                        OUTER APPLY
                                                        (
                                                                SELECT SUM(s.ticket_quantity) AS seats_sold
                                                                FROM dbo.sale AS s WITH (UPDLOCK, HOLDLOCK)
                                                                WHERE s.schedule_id = current_schedule.schedule_id
                                                                    AND s.sale_status NOT IN (@refunded_sale_status, @cancelled_sale_status)
                                                        ) AS sold
                                                        WHERE current_schedule.schedule_id = @schedule_id
                                                            AND target_bus.capacity >= COALESCE(sold.seats_sold, 0)
                                                ))
                        AND NOT EXISTS
                        (
                            SELECT 1 FROM dbo.route_schedule AS rs WITH (UPDLOCK, HOLDLOCK)
                            WHERE rs.service_date = @service_date
                              AND rs.schedule_id <> @schedule_id
                              AND rs.schedule_status <> @cancelled_status
                              AND (rs.bus_id = @bus_id OR rs.driver_id = @driver_id)
                              AND DATEDIFF(MINUTE, CAST('00:00' AS time), @departure_time) <
                                  DATEDIFF(MINUTE, CAST('00:00' AS time), rs.arrival_time) + 15
                              AND DATEDIFF(MINUTE, CAST('00:00' AS time), rs.departure_time) <
                                  DATEDIFF(MINUTE, CAST('00:00' AS time), @arrival_time) + 15
                        )
                    THEN 1 ELSE 0 END", validationParameters));

                if (valid == 0)
                    throw new InvalidOperationException("A route, resource, maintenance, or booking conflict changed while saving. Refresh and review the schedule.");

                int scheduleId;
                if (schedule.ScheduleId == 0)
                {
                    scheduleId = InsertSchedule(schedule);
                }
                else
                {
                    if (!UpdateSchedule(schedule))
                        throw new InvalidOperationException("Only unchanged planned schedules can be edited. Refresh and try again.");
                    scheduleId = schedule.ScheduleId;
                }

                _db.CommitTransaction();
                return scheduleId;
            }
            catch
            {
                _db.RollbackTransaction();
                throw;
            }
        }

        public bool UpdateSchedule(RouteSchedule schedule)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@schedule_id", schedule.ScheduleId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@route_id", schedule.RouteId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@bus_id", schedule.BusId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@driver_id", schedule.DriverId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@service_date", schedule.ServiceDate, SqlDbType.Date),
                DatabaseHelper.CreateParameter("@departure_time", schedule.DepartureTime, SqlDbType.Time),
                DatabaseHelper.CreateParameter("@arrival_time", schedule.ArrivalTime, SqlDbType.Time),
                DatabaseHelper.CreateParameter("@refunded_sale_status", SaleStatus.Refunded, SqlDbType.NVarChar, 30),
                DatabaseHelper.CreateParameter("@cancelled_sale_status", SaleStatus.Cancelled, SqlDbType.NVarChar, 30)
            };
            int rowsAffected = _db.ExecuteNonQuery(@"
                UPDATE rs
                SET route_id = @route_id, bus_id = @bus_id, driver_id = @driver_id,
                    service_date = @service_date, departure_time = @departure_time,
                    arrival_time = @arrival_time,
                    available_seat_num = CAST(CASE
                        WHEN b.capacity - COALESCE(sold.seats_sold, 0) > 0
                        THEN b.capacity - COALESCE(sold.seats_sold, 0) ELSE 0 END AS smallint)
                FROM dbo.route_schedule AS rs
                INNER JOIN dbo.bus AS b ON b.bus_id = @bus_id
                OUTER APPLY
                (
                    SELECT SUM(s.ticket_quantity) AS seats_sold
                    FROM dbo.sale AS s
                    WHERE s.schedule_id = rs.schedule_id
                      AND s.sale_status NOT IN (@refunded_sale_status, @cancelled_sale_status)
                ) AS sold
                WHERE rs.schedule_id = @schedule_id AND rs.schedule_status = @planned_status",
                AddPlannedStatusParameter(parameters));
            return rowsAffected > 0;
        }

        public bool UpdateScheduleStatus(int scheduleId, string status)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@schedule_id", scheduleId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@status", status, SqlDbType.NVarChar, 30),
                DatabaseHelper.CreateParameter("@cancelled_status", ScheduleStatus.Cancelled, SqlDbType.NVarChar, 30)
            };
            return _db.ExecuteNonQuery(@"
                UPDATE dbo.route_schedule SET schedule_status = @status
                WHERE schedule_id = @schedule_id AND schedule_status <> @cancelled_status",
                parameters) > 0;
        }

        private static SqlParameter[] AddPlannedStatusParameter(SqlParameter[] parameters)
        {
            List<SqlParameter> result = new List<SqlParameter>(parameters);
            result.Add(DatabaseHelper.CreateParameter("@planned_status", ScheduleStatus.Planned, SqlDbType.NVarChar, 30));
            return result.ToArray();
        }

        private List<RouteSchedule> GetSchedules(string predicate, SqlParameter[] parameters)
        {
            List<RouteSchedule> schedules = new List<RouteSchedule>();
            string sql = @"
                SELECT rs.schedule_id, rs.route_id, rs.bus_id, rs.driver_id, rs.service_date,
                       rs.departure_time, rs.arrival_time,
                       CAST(CASE WHEN b.capacity - COALESCE(sold.seats_sold, 0) > 0
                                 THEN b.capacity - COALESCE(sold.seats_sold, 0) ELSE 0 END AS smallint) AS available_seat_num,
                       rs.schedule_status, rs.created_at,
                       r.route_num, r.route_name, b.fleet_number, b.model,
                       CONCAT(e.surname, N' ', e.name) AS driver_name,
                       DATEDIFF(MINUTE, rs.departure_time, rs.arrival_time) AS trip_minutes
                FROM dbo.route_schedule AS rs
                INNER JOIN dbo.route AS r ON r.route_id = rs.route_id
                INNER JOIN dbo.bus AS b ON b.bus_id = rs.bus_id
                INNER JOIN dbo.employee AS e ON e.employee_id = rs.driver_id
                OUTER APPLY
                (
                    SELECT SUM(s.ticket_quantity) AS seats_sold
                    FROM dbo.sale AS s
                    WHERE s.schedule_id = rs.schedule_id
                      AND s.sale_status NOT IN (N'Отменена', N'Возврат')
                ) AS sold
                WHERE " + predicate + @"
                ORDER BY rs.service_date, r.route_num, rs.departure_time";

            using (SqlDataReader reader = _db.ExecuteReader(sql, parameters))
            {
                while (reader.Read())
                    schedules.Add(MapRouteScheduleFromReader(reader));
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

        public List<Stop> GetStopsForManagement()
        {
            List<Stop> stops = new List<Stop>();
            using (SqlDataReader reader = _db.ExecuteReader(@"
                SELECT st.stop_id, st.stop_name, st.location, st.latitude, st.longitude, st.is_active,
                       COUNT(DISTINCT rs.route_id) AS route_count
                FROM dbo.stop AS st
                LEFT JOIN dbo.route_stop AS rs ON rs.stop_id = st.stop_id
                GROUP BY st.stop_id, st.stop_name, st.location, st.latitude, st.longitude, st.is_active
                ORDER BY st.stop_name"))
            {
                while (reader.Read()) stops.Add(MapStopFromReader(reader));
            }
            return stops;
        }

        public int InsertStop(Stop stop)
        {
            SqlParameter[] parameters = CreateStopParameters(stop, false);
            return Convert.ToInt32(_db.ExecuteScalar(@"
                INSERT INTO dbo.stop (stop_name, location, latitude, longitude)
                VALUES (@stop_name, @location, @latitude, @longitude);
                SELECT CAST(SCOPE_IDENTITY() AS int);", parameters));
        }

        public bool UpdateStop(Stop stop)
        {
            SqlParameter[] parameters = CreateStopParameters(stop, true);
            return _db.ExecuteNonQuery(@"
                UPDATE dbo.stop
                SET stop_name = @stop_name, location = @location, latitude = @latitude, longitude = @longitude
                WHERE stop_id = @stop_id", parameters) > 0;
        }

        public bool SetStopActive(int stopId, bool isActive)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@stop_id", stopId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@is_active", isActive, SqlDbType.Bit)
            };
            return _db.ExecuteNonQuery("UPDATE dbo.stop SET is_active = @is_active WHERE stop_id = @stop_id", parameters) > 0;
        }

        private static SqlParameter[] CreateStopParameters(Stop stop, bool includeId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (includeId) parameters.Add(DatabaseHelper.CreateParameter("@stop_id", stop.StopId, SqlDbType.Int));
            parameters.Add(DatabaseHelper.CreateParameter("@stop_name", stop.StopName, SqlDbType.NVarChar, 120));
            parameters.Add(DatabaseHelper.CreateParameter("@location", stop.Location ?? (object)DBNull.Value, SqlDbType.NVarChar, 200));
            SqlParameter latitude = DatabaseHelper.CreateParameter("@latitude", stop.Latitude ?? (decimal?)null, SqlDbType.Decimal);
            latitude.Precision = 9;
            latitude.Scale = 6;
            SqlParameter longitude = DatabaseHelper.CreateParameter("@longitude", stop.Longitude ?? (decimal?)null, SqlDbType.Decimal);
            longitude.Precision = 9;
            longitude.Scale = 6;
            parameters.Add(latitude);
            parameters.Add(longitude);
            return parameters.ToArray();
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
            schedule.RouteId = reader.GetInt32(reader.GetOrdinal("route_id"));
            schedule.BusId = reader.GetInt32(reader.GetOrdinal("bus_id"));
            schedule.DriverId = reader.GetInt32(reader.GetOrdinal("driver_id"));
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
            schedule.CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"));
            
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
            int routeCountOrdinal = TryGetOrdinal(reader, "route_count");
            if (routeCountOrdinal >= 0 && !reader.IsDBNull(routeCountOrdinal))
                stop.RouteCount = reader.GetInt32(routeCountOrdinal);
            
            return stop;
        }

        private static int TryGetOrdinal(SqlDataReader reader, string columnName)
        {
            for (int ordinal = 0; ordinal < reader.FieldCount; ordinal++)
                if (string.Equals(reader.GetName(ordinal), columnName, StringComparison.OrdinalIgnoreCase))
                    return ordinal;
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