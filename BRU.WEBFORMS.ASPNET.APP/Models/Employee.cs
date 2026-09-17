using System;

namespace BRU.WEBFORMS.ASPNET.APP.Models
{
    /// <summary>
    /// Represents an employee in the autopark
    /// </summary>
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronym { get; set; }
        public DateTime EmployedDate { get; set; }
        public int JobId { get; set; }
        public int? DepartmentId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string EmployeeName { get; set; }
        public string JobTitle { get; set; }
        public string DepartmentName { get; set; }
        public int ServiceYears { get; set; }
    }

    /// <summary>
    /// Employee status constants
    /// </summary>
    public static class EmployeeStatus
    {
        public const string Working = "Работает";
        public const string OnVacation = "Отпуск";
        public const string OnSickLeave = "Больничный";
        public const string Dismissed = "Уволен";
    }

    /// <summary>
    /// Represents a job position
    /// </summary>
    public class Job
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; }
        public string Internship { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// Represents a department
    /// </summary>
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}