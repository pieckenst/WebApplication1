using System;

namespace BRU.WEBFORMS.ASPNET.APP.Models
{
    /// <summary>
    /// Represents a maintenance record
    /// </summary>
    public class Maintenance
    {
        public int MaintenanceId { get; set; }
        public int BusId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public string MaintenanceType { get; set; }
        public string FoundIssue { get; set; }
        public string ServiceResult { get; set; }
        public int? MileageKm { get; set; }
        public string Roadworthiness { get; set; }
        public decimal MaintenanceCost { get; set; }
        
        // View fields
        public string FleetNumber { get; set; }
        public int DaysFromLastService { get; set; }
    }

    /// <summary>
    /// Roadworthiness status constants
    /// </summary>
    public static class RoadworthinessStatus
    {
        public const string Operational = "Исправен";
        public const string NeedsAttention = "Требует внимания";
        public const string NotOperational = "Неисправен";
    }

    /// <summary>
    /// Represents an autopark summary for dashboard
    /// </summary>
    public class AutoparkSummary
    {
        public int ActiveBusCount { get; set; }
        public int ActiveEmployeeCount { get; set; }
        public int ActiveRouteCount { get; set; }
        public int TodayScheduleCount { get; set; }
        public int TodaySaleCount { get; set; }
        public decimal TodayAmountTotal { get; set; }
        public int BusNeedAttentionCount { get; set; }
    }

    /// <summary>
    /// Represents sales by channel
    /// </summary>
    public class SalesByChannel
    {
        public string SaleChannel { get; set; }
        public int SaleCount { get; set; }
        public int TicketCount { get; set; }
        public decimal AmountTotal { get; set; }
        public decimal AverageTicketPrice { get; set; }
    }

    /// <summary>
    /// Represents sales by route
    /// </summary>
    public class SalesByRoute
    {
        public string RouteNum { get; set; }
        public string RouteName { get; set; }
        public int SaleCount { get; set; }
        public int TicketCount { get; set; }
        public decimal AmountTotal { get; set; }
    }

    /// <summary>
    /// Represents sales by employee
    /// </summary>
    public class SalesByEmployee
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string JobTitle { get; set; }
        public int SaleCount { get; set; }
        public decimal AmountTotal { get; set; }
    }
}