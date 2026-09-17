using System;

namespace BRU.WEBFORMS.ASPNET.APP.Models
{
    /// <summary>
    /// Represents a ticket type
    /// </summary>
    public class Ticket
    {
        public int TicketId { get; set; }
        public string TicketName { get; set; }
        public string TicketType { get; set; }
        public string Zone { get; set; }
        public decimal Price { get; set; }
        public short ValidDays { get; set; }
        public int AvailableCount { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public decimal PeriodValue { get; set; }
    }

    /// <summary>
    /// Represents a sale
    /// </summary>
    public class Sale
    {
        public long SaleId { get; set; }
        public int? ScheduleId { get; set; }
        public int TicketId { get; set; }
        public int? CashierId { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal SalePrice { get; set; }
        public string SaleStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string SaleChannel { get; set; }
        public int TicketQuantity { get; set; }
        
        // View fields
        public string TicketName { get; set; }
        public string TicketType { get; set; }
        public decimal SaleTotal { get; set; }
        public string CashierName { get; set; }
    }

    /// <summary>
    /// Represents a payment
    /// </summary>
    public class Payment
    {
        public long PaymentId { get; set; }
        public long SaleId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string TransactionId { get; set; }
        public string ControlStatus { get; set; }
    }

    /// <summary>
    /// Sale status constants
    /// </summary>
    public static class SaleStatus
    {
        public const string Created = "Создана";
        public const string Completed = "Завершена";
        public const string Cancelled = "Отменена";
        public const string Refunded = "Возврат";
    }

    /// <summary>
    /// Payment status constants
    /// </summary>
    public static class PaymentStatus
    {
        public const string Pending = "Ожидает";
        public const string Paid = "Оплачена";
        public const string Error = "Ошибка";
        public const string Refunded = "Возврат";
    }

    /// <summary>
    /// Sale channel constants
    /// </summary>
    public static class SaleChannel
    {
        public const string CashDesk = "Касса";
        public const string Conductor = "Кондуктор";
        public const string Validator = "Валидатор";
        public const string QR = "QR";
        public const string Online = "Онлайн";
    }
}