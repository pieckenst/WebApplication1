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

}
