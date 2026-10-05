using System;
using System.Collections.Generic;

namespace BRU.WEBFORMS.ASPNET.APP.Models
{
    public class SalesReportSummary
    {
        public long SaleCount { get; set; }
        public long TicketCount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal RefundedAmount { get; set; }
        public decimal NetAmount { get; set; }
    }

    public class SalesReportRow
    {
        public long SaleId { get; set; }
        public DateTime SaleDate { get; set; }
        public string RouteNumber { get; set; }
        public string TicketName { get; set; }
        public int TicketQuantity { get; set; }
        public decimal SalePrice { get; set; }
        public decimal TotalAmount { get; set; }
        public string SaleChannel { get; set; }
        public string SaleStatus { get; set; }
        public string PaymentStatus { get; set; }
    }

    public class SalesReportData
    {
        public SalesReportSummary Summary { get; set; }
        public List<SalesReportRow> Rows { get; set; }
        public long TotalRows { get; set; }

        public SalesReportData()
        {
            Summary = new SalesReportSummary();
            Rows = new List<SalesReportRow>();
        }
    }

    public class MaintenanceReportSummary
    {
        public long RecordCount { get; set; }
        public decimal TotalCost { get; set; }
        public long NotRoadworthyCount { get; set; }
        public long UpcomingCount { get; set; }
    }

    public class MaintenanceReportRow
    {
        public int MaintenanceId { get; set; }
        public string FleetNumber { get; set; }
        public string BusModel { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public string MaintenanceType { get; set; }
        public string Roadworthiness { get; set; }
        public decimal MaintenanceCost { get; set; }
        public string EmployeeName { get; set; }
    }

    public class MaintenanceReportData
    {
        public MaintenanceReportSummary Summary { get; set; }
        public List<MaintenanceReportRow> Rows { get; set; }
        public long TotalRows { get; set; }

        public MaintenanceReportData()
        {
            Summary = new MaintenanceReportSummary();
            Rows = new List<MaintenanceReportRow>();
        }
    }
}
