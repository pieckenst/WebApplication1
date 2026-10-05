using System;
using System.Collections.Generic;
using BRU.WEBFORMS.ASPNET.APP.DataAccess;
using BRU.WEBFORMS.ASPNET.APP.Models;

namespace BRU.WEBFORMS.ASPNET.APP.Services
{
    /// <summary>
    /// Business logic layer for Employee operations
    /// </summary>
    public class EmployeeService : IDisposable
    {
        private EmployeeRepository _repository;

        public EmployeeService()
        {
            _repository = new EmployeeRepository();
        }

        /// <summary>
        /// Get all employees with job and department information
        /// </summary>
        public List<Employee> GetAllEmployees()
        {
            try
            {
                return _repository.GetAllEmployees();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving all employees", ex);
            }
        }

        /// <summary>
        /// Get only active employees
        /// </summary>
        public List<Employee> GetActiveEmployees()
        {
            try
            {
                return _repository.GetActiveEmployees();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving active employees", ex);
            }
        }

        /// <summary>
        /// Get employee by ID
        /// </summary>
        public Employee GetEmployeeById(int employeeId)
        {
            try
            {
                Employee employee = _repository.GetEmployeeDirectoryById(employeeId);
                if (employee == null)
                {
                    throw new ServiceException("Employee not found");
                }
                return employee;
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving employee", ex);
            }
        }

        /// <summary>
        /// Get employees by job
        /// </summary>
        public List<Employee> GetEmployeesByJob(int jobId)
        {
            try
            {
                return _repository.GetEmployeesByJob(jobId);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving employees by job", ex);
            }
        }

        /// <summary>
        /// Get employees by department
        /// </summary>
        public List<Employee> GetEmployeesByDepartment(int departmentId)
        {
            try
            {
                return _repository.GetEmployeesByDepartment(departmentId);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving employees by department", ex);
            }
        }

        /// <summary>
        /// Create a new employee with validation
        /// </summary>
        public int CreateEmployee(Employee employee)
        {
            try
            {
                ValidateEmployee(employee);
                return _repository.InsertEmployee(employee);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error creating employee", ex);
            }
        }

        /// <summary>
        /// Update an existing employee with validation
        /// </summary>
        public bool UpdateEmployee(Employee employee)
        {
            try
            {
                ValidateEmployee(employee);
                
                // Check if employee exists
                Employee existingEmployee = _repository.GetEmployeeById(employee.EmployeeId);
                if (existingEmployee == null)
                {
                    throw new ServiceException("Employee not found");
                }

                return _repository.UpdateEmployee(employee);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error updating employee", ex);
            }
        }

        /// <summary>
        /// Delete an employee
        /// </summary>
        public bool DeleteEmployee(int employeeId)
        {
            try
            {
                Employee employee = _repository.GetEmployeeById(employeeId);
                if (employee == null)
                {
                    throw new ServiceException("Employee not found");
                }

                // Business rule: Cannot delete active employees
                if (employee.Status == EmployeeStatus.Working)
                {
                    throw new ServiceException("Cannot delete active employee. Change status to dismissed first.");
                }

                return _repository.DeleteEmployee(employeeId);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error deleting employee", ex);
            }
        }

        /// <summary>
        /// Get all available jobs
        /// </summary>
        public List<Job> GetAllJobs()
        {
            try
            {
                return _repository.GetAllJobs();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving jobs", ex);
            }
        }

        /// <summary>
        /// Get all available departments
        /// </summary>
        public List<Department> GetAllDepartments()
        {
            try
            {
                return _repository.GetAllDepartments();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving departments", ex);
            }
        }

        public List<Department> GetDepartmentsForManagement()
        {
            try
            {
                return _repository.GetDepartmentsForManagement();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving department records", ex);
            }
        }

        public int CreateDepartment(Department department)
        {
            ValidateDepartment(department);
            try
            {
                return _repository.InsertDepartment(department);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Department could not be created. Check that its code is unique.", ex);
            }
        }

        public bool UpdateDepartment(Department department)
        {
            ValidateDepartment(department);
            try
            {
                if (!_repository.UpdateDepartment(department))
                    throw new ServiceException("Department not found.");
                return true;
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Department could not be updated. Check that its code is unique.", ex);
            }
        }

        public bool SetDepartmentActive(int departmentId, bool isActive)
        {
            try
            {
                return _repository.SetDepartmentActive(departmentId, isActive);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Department status could not be changed.", ex);
            }
        }

        private static void ValidateDepartment(Department department)
        {
            if (department == null)
                throw new ServiceException("Department details are required.");
            department.DepartmentName = (department.DepartmentName ?? string.Empty).Trim();
            department.DepartmentCode = (department.DepartmentCode ?? string.Empty).Trim().ToUpperInvariant();
            department.Description = string.IsNullOrWhiteSpace(department.Description) ? null : department.Description.Trim();
            if (department.DepartmentName.Length < 3 || department.DepartmentName.Length > 100)
                throw new ServiceException("Department name must contain between 3 and 100 characters.");
            if (department.DepartmentCode.Length == 0 || department.DepartmentCode.Length > 20)
                throw new ServiceException("Department code is required and cannot exceed 20 characters.");
            foreach (char character in department.DepartmentCode)
                if (!char.IsLetterOrDigit(character) && character != '-' && character != '_')
                    throw new ServiceException("Department code may contain only letters, numbers, hyphens, and underscores.");
            if (department.Description != null && department.Description.Length > 500)
                throw new ServiceException("Department description cannot exceed 500 characters.");
        }

        /// <summary>
        /// Get drivers (employees with driver job)
        /// </summary>
        public List<Employee> GetDrivers()
        {
            try
            {
                List<Employee> drivers = _repository.GetEmployeesByJob(1);
                return drivers.Count > 0 ? drivers : _repository.GetEmployeesByJobTitle("Водитель автобуса");
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving drivers", ex);
            }
        }

        /// <summary>
        /// Get mechanics (employees with mechanic job)
        /// </summary>
        public List<Employee> GetMechanics()
        {
            try
            {
                // Assuming job_id 2 is for mechanics based on the schema
                return _repository.GetEmployeesByJob(2);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving mechanics", ex);
            }
        }

        /// <summary>
        /// Get cashiers (employees with cashier job)
        /// </summary>
        public List<Employee> GetCashiers()
        {
            try
            {
                // Assuming job_id 4 is for cashiers based on the schema
                return _repository.GetEmployeesByJob(4);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving cashiers", ex);
            }
        }

        /// <summary>
        /// Validate employee data
        /// </summary>
        private void ValidateEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ServiceException("Employee object cannot be null");
            }

            if (string.IsNullOrEmpty(employee.Surname))
            {
                throw new ServiceException("Surname is required");
            }

            if (string.IsNullOrEmpty(employee.Name))
            {
                throw new ServiceException("Name is required");
            }

            if (employee.EmployedDate == default(DateTime))
            {
                throw new ServiceException("Employment date is required");
            }

            if (employee.EmployedDate > DateTime.Today)
            {
                throw new ServiceException("Employment date cannot be in the future");
            }

            if (employee.JobId <= 0)
            {
                throw new ServiceException("Valid job ID is required");
            }

            if (!IsValidEmployeeStatus(employee.Status))
            {
                throw new ServiceException("Invalid employee status");
            }

            // Email validation if provided
            if (!string.IsNullOrEmpty(employee.Email))
            {
                if (!IsValidEmail(employee.Email))
                {
                    throw new ServiceException("Invalid email format");
                }
            }
        }

        /// <summary>
        /// Check if employee status is valid
        /// </summary>
        private bool IsValidEmployeeStatus(string status)
        {
            return status == EmployeeStatus.Working ||
                   status == EmployeeStatus.OnVacation ||
                   status == EmployeeStatus.OnSickLeave ||
                   status == EmployeeStatus.Dismissed;
        }

        /// <summary>
        /// Simple email validation
        /// </summary>
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
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