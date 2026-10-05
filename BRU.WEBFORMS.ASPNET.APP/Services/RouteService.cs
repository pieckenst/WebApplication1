using System;
using System.Collections.Generic;
using BRU.WEBFORMS.ASPNET.APP.DataAccess;
using BRU.WEBFORMS.ASPNET.APP.Models;

namespace BRU.WEBFORMS.ASPNET.APP.Services
{
    /// <summary>
    /// Business logic layer for Route operations
    /// </summary>
    public class RouteService : IDisposable
    {
        private RouteRepository _repository;

        public RouteService()
        {
            _repository = new RouteRepository();
        }

        /// <summary>
        /// Get all active routes
        /// </summary>
        public List<Route> GetAllActiveRoutes()
        {
            try
            {
                return _repository.GetAllActiveRoutes();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving active routes", ex);
            }
        }

        /// <summary>
        /// Get route by ID
        /// </summary>
        public Route GetRouteById(int routeId)
        {
            try
            {
                Route route = _repository.GetRouteById(routeId);
                if (route == null)
                {
                    throw new ServiceException("Route not found");
                }
                return route;
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving route", ex);
            }
        }

        /// <summary>
        /// Get route details with stop count
        /// </summary>
        public Route GetRouteDetails(int routeId)
        {
            try
            {
                Route route = _repository.GetRouteDetails(routeId);
                if (route == null)
                {
                    throw new ServiceException("Route not found");
                }
                return route;
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving route details", ex);
            }
        }

        /// <summary>
        /// Get route stops
        /// </summary>
        public List<RouteStop> GetRouteStops(int routeId)
        {
            try
            {
                return _repository.GetRouteStops(routeId);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving route stops", ex);
            }
        }

        /// <summary>
        /// Get route schedule for a specific date
        /// </summary>
        public List<RouteSchedule> GetRouteSchedule(int routeId, DateTime serviceDate)
        {
            try
            {
                return _repository.GetRouteSchedule(routeId, serviceDate);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving route schedule", ex);
            }
        }

        /// <summary>
        /// Get schedule for date range
        /// </summary>
        public List<RouteSchedule> GetScheduleRange(DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                if (dateFrom >= dateTo)
                {
                    throw new ServiceException("End date must be after start date");
                }

                return _repository.GetScheduleRange(dateFrom, dateTo);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving schedule range", ex);
            }
        }

        /// <summary>
        /// Create a new route with validation
        /// </summary>
        public int CreateRoute(Route route)
        {
            try
            {
                ValidateRoute(route);
                return _repository.InsertRoute(route);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error creating route", ex);
            }
        }

        /// <summary>
        /// Update an existing route with validation
        /// </summary>
        public bool UpdateRoute(Route route)
        {
            try
            {
                ValidateRoute(route);
                
                // Check if route exists
                Route existingRoute = _repository.GetRouteById(route.RouteId);
                if (existingRoute == null)
                {
                    throw new ServiceException("Route not found");
                }

                return _repository.UpdateRoute(route);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error updating route", ex);
            }
        }

        /// <summary>
        /// Delete a route
        /// </summary>
        public bool DeleteRoute(int routeId)
        {
            try
            {
                Route route = _repository.GetRouteById(routeId);
                if (route == null)
                {
                    throw new ServiceException("Route not found");
                }

                // Business rule: Cannot delete active routes
                if (route.IsActive)
                {
                    throw new ServiceException("Cannot delete active route. Deactivate it first.");
                }

                return _repository.DeleteRoute(routeId);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error deleting route", ex);
            }
        }

        /// <summary>
        /// Get all stops
        /// </summary>
        public List<Stop> GetAllStops()
        {
            try
            {
                return _repository.GetAllStops();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving stops", ex);
            }
        }

        /// <summary>
        /// Get today's schedule
        /// </summary>
        public List<RouteSchedule> GetTodaySchedule()
        {
            try
            {
                DateTime today = DateTime.Today;
                DateTime tomorrow = today.AddDays(1);
                return _repository.GetScheduleRange(today, tomorrow);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving today's schedule", ex);
            }
        }

        /// <summary>
        /// Validate route data
        /// </summary>
        private void ValidateRoute(Route route)
        {
            if (route == null)
            {
                throw new ServiceException("Route object cannot be null");
            }

            if (string.IsNullOrEmpty(route.RouteNum))
            {
                throw new ServiceException("Route number is required");
            }

            if (string.IsNullOrEmpty(route.RouteName))
            {
                throw new ServiceException("Route name is required");
            }

            if (string.IsNullOrEmpty(route.StartStop))
            {
                throw new ServiceException("Start stop is required");
            }

            if (string.IsNullOrEmpty(route.EndStop))
            {
                throw new ServiceException("End stop is required");
            }

            // Business rule: Start and end stops must be different
            if (route.StartStop == route.EndStop)
            {
                throw new ServiceException("Start stop and end stop must be different");
            }
        }

        public void Dispose()
        {
            if (_repository != null)
            {
                _repository.Dispose();
                _repository = null;
            }
        }
    }
}