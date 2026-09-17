using System;

namespace BRU.WEBFORMS.ASPNET.APP.Models
{
    /// <summary>
    /// Represents a bus in the autopark
    /// </summary>
    public class Bus
    {
        public int BusId { get; set; }
        public string FleetNumber { get; set; }
        public string RegistrationNum { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public short ManufactureYear { get; set; }
        public short Capacity { get; set; }
        public string Status { get; set; }
        public int MileageKm { get; set; }
        public string MileageCategory { get; set; }
    }

    /// <summary>
    /// Bus status constants
    /// </summary>
    public static class BusStatus
    {
        public const string Operational = "Исправен";
        public const string InRepair = "На ремонте";
        public const string Retired = "Списан";
        public const string Reserve = "Резерв";
    }

    /// <summary>
    /// Mileage category constants
    /// </summary>
    public static class MileageCategory
    {
        public const string Low = "Низкий пробег";
        public const string Medium = "Средний пробег";
        public const string High = "Высокий пробег";
    }
}