using System;

namespace BRU.WEBFORMS.ASPNET.APP.Models
{
    /// <summary>
    /// Represents a department within the autopark organization (department table).
    /// </summary>
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        // Calculated properties
        public int EmployeeCount { get; set; }

        /// <summary>
        /// Gets a user-friendly status description.
        /// </summary>
        public string StatusDescription
        {
            get { return IsActive ? "Активен" : "Неактивен"; }
        }
    }

    /// <summary>
    /// Represents a stop on a route (stop table).
    /// </summary>
    public class Stop
    {
        public int StopId { get; set; }
        public string StopName { get; set; }
        public string Location { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public bool IsActive { get; set; }

        // Calculated properties
        public int RouteCount { get; set; }

        /// <summary>
        /// Gets a user-friendly status description.
        /// </summary>
        public string StatusDescription
        {
            get { return IsActive ? "Активна" : "Неактивна"; }
        }

        /// <summary>
        /// Gets GPS coordinates as a formatted string.
        /// </summary>
        public string Coordinates
        {
            get
            {
                if (Latitude.HasValue && Longitude.HasValue)
                    return $"{Latitude.Value:F6}, {Longitude.Value:F6}";
                return "-";
            }
        }
    }

    /// <summary>
    /// Represents the association between a route and a stop (route_stop table).
    /// </summary>
    public class RouteStop
    {
        public int RouteId { get; set; }
        public int StopId { get; set; }
        public int StopSequence { get; set; }
        public decimal? DistanceKm { get; set; }
        public int? ArrivalOffsetMin { get; set; }

        // Navigation properties
        public string RouteNum { get; set; }
        public string RouteName { get; set; }
        public string StopName { get; set; }
        public string StopLocation { get; set; }

        /// <summary>
        /// Gets the arrival offset formatted as minutes.
        /// </summary>
        public string ArrivalOffsetFormatted
        {
            get
            {
                if (!ArrivalOffsetMin.HasValue)
                    return "-";
                return $"{ArrivalOffsetMin.Value} мин";
            }
        }
    }
}
