using System;

namespace BRU.WEBFORMS.ASPNET.APP.Models
{
    /// <summary>
    /// Represents a bus route
    /// </summary>
    public class Route
    {
        public int RouteId { get; set; }
        public string RouteNum { get; set; }
        public string RouteName { get; set; }
        public string StartStop { get; set; }
        public string EndStop { get; set; }
        public bool IsActive { get; set; }
        public int StopCount { get; set; }
    }

    /// <summary>
    /// Represents a bus stop
    /// </summary>
    public class Stop
    {
        public int StopId { get; set; }
        public string StopName { get; set; }
        public string Location { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public bool IsActive { get; set; }
        public int RouteCount { get; set; }

        public string Coordinates
        {
            get { return Latitude.HasValue && Longitude.HasValue ? Latitude.Value.ToString("F6") + ", " + Longitude.Value.ToString("F6") : "-"; }
        }
    }

    /// <summary>
    /// Represents a route stop mapping
    /// </summary>
    public class RouteStop
    {
        public int RouteId { get; set; }
        public int StopId { get; set; }
        public short StopSequence { get; set; }
        public decimal? DistanceKm { get; set; }
        public short? ArrivalOffsetMin { get; set; }
        public string RouteNum { get; set; }
        public string RouteName { get; set; }
        public string StopName { get; set; }
        public string StopLocation { get; set; }
    }

    /// <summary>
    /// Represents a route schedule
    /// </summary>
    public class RouteSchedule
    {
        public int ScheduleId { get; set; }
        public int RouteId { get; set; }
        public int BusId { get; set; }
        public int DriverId { get; set; }
        public DateTime ServiceDate { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public short AvailableSeatNum { get; set; }
        public string ScheduleStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public short AvailableSeats { get { return AvailableSeatNum; } }
        
        // View fields
        public string RouteNum { get; set; }
        public string RouteName { get; set; }
        public string FleetNumber { get; set; }
        public string BusModel { get; set; }
        public string DriverName { get; set; }
        public int TripMinutes { get; set; }
    }

    /// <summary>
    /// Schedule status constants
    /// </summary>
    public static class ScheduleStatus
    {
        public const string Planned = "Запланирован";
        public const string Completed = "Выполнен";
        public const string Cancelled = "Отменен";
        public const string InProgress = "В пути";
    }
}