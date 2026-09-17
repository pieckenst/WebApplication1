using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BRU.WEBFORMS.ASPNET.APP.Models;

namespace BRU.WEBFORMS.ASPNET.APP.DataAccess
{
    /// <summary>
    /// Data access layer for Employee operations
    /// </summary>
    public class EmployeeRepository : IDisposable
    {
        private DatabaseHelper _db;

        public EmployeeRepository()
        {
            _db = new DatabaseHelper();
        }

        /// <summary>
        /// Get all employees from the view
        /// </summary>
        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();
            
            using (SqlDataReader reader = _db.ExecuteReader("SELECT * FROM dbo.v_employee_directory ORDER BY employee_name"))
            {
                while (reader.Read())
                {
                    employees.Add(MapEmployeeFromReader(reader));
                }
            }
            
            return employees;
        }

        /// <summary>
        /// Get active employees only
        /// </summary>
        public List<Employee> GetActiveEmployees()
        {
            List<Employee> employees = new List<Employee>();
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@status", EmployeeStatus.Working, SqlDbType.NVarChar, 30)
            };
            
            using (SqlDataReader reader = _db.ExecuteReader(
                "SELECT * FROM dbo.v_employee_directory WHERE status = @status ORDER BY employee_name", 
                parameters))
            {
                while (reader.Read())
                {
                    employees.Add(MapEmployeeFromReader(reader));
                }
            }
            
            return employees;
        }

        /// <summary>
        /// Get employee by ID
        /// </summary>
        public Employee GetEmployeeById(int employeeId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@employee_id", employeeId, SqlDbType.Int)
            };
            
            using (SqlDataReader reader = _db.ExecuteReader(
                "SELECT * FROM dbo.employee WHERE employee_id = @employee_id", 
                parameters))
            {
                if (reader.Read())
                {
                    return MapEmployeeFromReader(reader);
                }
            }
            
            return null;
        }

        /// <summary>
        /// Get employee directory (with job and department info)
        /// </summary>
        public Employee GetEmployeeDirectoryById(int employeeId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@employee_id", employeeId, SqlDbType.Int)
            };
            
            using (SqlDataReader reader = _db.ExecuteReader(
                "SELECT * FROM dbo.v_employee_directory WHERE employee_id = @employee_id", 
                parameters))
            {
                if (reader.Read())
                {
                    return MapEmployeeFromReader(reader);
                }
            }
            
            return null;
        }

        /// <summary>
        /// Get employees by job ID
        /// </summary>
        public List<Employee> GetEmployeesByJob(int jobId)
        {
            List<Employee> employees = new List<Employee>();
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@job_id", jobId, SqlDbType.Int)
            };
            
            using (SqlDataReader reader = _db.ExecuteReader(
                "SELECT * FROM dbo.v_employee_directory WHERE job_id = @job_id ORDER BY employee_name", 
                parameters))
            {
                while (reader.Read())
                {
                    employees.Add(MapEmployeeFromReader(reader));
                }
            }
            
            return employees;
        }

        /// <summary>
        /// Get employees by department ID
        /// </summary>
        public List<Employee> GetEmployeesByDepartment(int departmentId)
        {
            List<Employee> employees = new List<Employee>();
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@department_id", departmentId, SqlDbType.Int)
            };
            
            using (SqlDataReader reader = _db.ExecuteReader(
                "SELECT * FROM dbo.v_employee_directory WHERE department_id = @department_id ORDER BY employee_name", 
                parameters))
            {
                while (reader.Read())
                {
                    employees.Add(MapEmployeeFromReader(reader));
                }
            }
            
            return employees;
        }

        /// <summary>
        /// Insert a new employee
        /// </summary>
        public int InsertEmployee(Employee employee)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@surname", employee.Surname, SqlDbType.NVarChar, 60),
                DatabaseHelper.CreateParameter("@name", employee.Name, SqlDbType.NVarChar, 60),
                DatabaseHelper.CreateParameter("@patronym", employee.Patronym ?? (object)DBNull.Value, SqlDbType.NVarChar, 60),
                DatabaseHelper.CreateParameter("@employed_date", employee.EmployedDate, SqlDbType.Date),
                DatabaseHelper.CreateParameter("@job_id", employee.JobId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@department_id", employee.DepartmentId ?? (object)DBNull.Value, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@phone", employee.Phone ?? (object)DBNull.Value, SqlDbType.VarChar, 30),
                DatabaseHelper.CreateParameter("@email", employee.Email ?? (object)DBNull.Value, SqlDbType.VarChar, 120),
                DatabaseHelper.CreateParameter("@status", employee.Status, SqlDbType.NVarChar, 30)
            };
            
            string sql = @"INSERT INTO dbo.employee (surname, name, patronym, employed_date, job_id, department_id, phone, email, status)
                          VALUES (@surname, @name, @patronym, @employed_date, @job_id, @department_id, @phone, @email, @status);
                          SELECT CAST(SCOPE_IDENTITY() AS int)";
            
            object result = _db.ExecuteScalar(sql, parameters);
            return Convert.ToInt32(result);
        }

        /// <summary>
        /// Update an existing employee
        /// </summary>
        public bool UpdateEmployee(Employee employee)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@employee_id", employee.EmployeeId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@surname", employee.Surname, SqlDbType.NVarChar, 60),
                DatabaseHelper.CreateParameter("@name", employee.Name, SqlDbType.NVarChar, 60),
                DatabaseHelper.CreateParameter("@patronym", employee.Patronym ?? (object)DBNull.Value, SqlDbType.NVarChar, 60),
                DatabaseHelper.CreateParameter("@job_id", employee.JobId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@department_id", employee.DepartmentId ?? (object)DBNull.Value, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@phone", employee.Phone ?? (object)DBNull.Value, SqlDbType.VarChar, 30),
                DatabaseHelper.CreateParameter("@email", employee.Email ?? (object)DBNull.Value, SqlDbType.VarChar, 120),
                DatabaseHelper.CreateParameter("@status", employee.Status, SqlDbType.NVarChar, 30)
            };
            
            string sql = @"UPDATE dbo.employee 
                          SET surname = @surname, 
                              name = @name, 
                              patronym = @patronym, 
                              job_id = @job_id, 
                              department_id = @department_id, 
                              phone = @phone, 
                              email = @email, 
                              status = @status
                          WHERE employee_id = @employee_id";
            
            int rowsAffected = _db.ExecuteNonQuery(sql, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Delete an employee
        /// </summary>
        public bool DeleteEmployee(int employeeId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@employee_id", employeeId, SqlDbType.Int)
            };
            
            int rowsAffected = _db.ExecuteNonQuery("DELETE FROM dbo.employee WHERE employee_id = @employee_id", parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Get all jobs
        /// </summary>
        public List<Job> GetAllJobs()
        {
            List<Job> jobs = new List<Job>();
            
            using (SqlDataReader reader = _db.ExecuteReader("SELECT * FROM dbo.job WHERE is_active = 1 ORDER BY job_title"))
            {
                while (reader.Read())
                {
                    Job job = new Job();
                    job.JobId = reader.GetInt32(reader.GetOrdinal("job_id"));
                    job.JobTitle = reader.GetString(reader.GetOrdinal("job_title"));
                    job.Internship = reader.IsDBNull(reader.GetOrdinal("internship")) ? null : reader.GetString(reader.GetOrdinal("internship"));
                    job.IsActive = reader.GetBoolean(reader.GetOrdinal("is_active"));
                    jobs.Add(job);
                }
            }
            
            return jobs;
        }

        /// <summary>
        /// Get all departments
        /// </summary>
        public List<Department> GetAllDepartments()
        {
            List<Department> departments = new List<Department>();
            
            using (SqlDataReader reader = _db.ExecuteReader("SELECT * FROM dbo.department WHERE is_active = 1 ORDER BY department_name"))
            {
                while (reader.Read())
                {
                    Department dept = new Department();
                    dept.DepartmentId = reader.GetInt32(reader.GetOrdinal("department_id"));
                    dept.DepartmentName = reader.GetString(reader.GetOrdinal("department_name"));
                    dept.DepartmentCode = reader.GetString(reader.GetOrdinal("department_code"));
                    dept.Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description"));
                    dept.IsActive = reader.GetBoolean(reader.GetOrdinal("is_active"));
                    departments.Add(dept);
                }
            }
            
            return departments;
        }

        /// <summary>
        /// Map SqlDataReader to Employee object
        /// </summary>
        private Employee MapEmployeeFromReader(SqlDataReader reader)
        {
            Employee employee = new Employee();
            
            employee.EmployeeId = reader.GetInt32(reader.GetOrdinal("employee_id"));
            
            // Handle both base table and view
            if (reader.FieldCount > 9) // View with additional fields
            {
                employee.EmployeeName = reader.GetString(reader.GetOrdinal("employee_name"));
                employee.JobTitle = reader.GetString(reader.GetOrdinal("job_title"));
                employee.DepartmentName = reader.IsDBNull(reader.GetOrdinal("department_name")) ? null : reader.GetString(reader.GetOrdinal("department_name"));
                employee.Status = reader.GetString(reader.GetOrdinal("status"));
                employee.ServiceYears = reader.GetInt32(reader.GetOrdinal("service_years"));
            }
            else // Base table
            {
                employee.Surname = reader.GetString(reader.GetOrdinal("surname"));
                employee.Name = reader.GetString(reader.GetOrdinal("name"));
                employee.Patronym = reader.IsDBNull(reader.GetOrdinal("patronym")) ? null : reader.GetString(reader.GetOrdinal("patronym"));
                employee.EmployedDate = reader.GetDateTime(reader.GetOrdinal("employed_date"));
                employee.JobId = reader.GetInt32(reader.GetOrdinal("job_id"));
                employee.DepartmentId = reader.IsDBNull(reader.GetOrdinal("department_id")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("department_id"));
                employee.Phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString(reader.GetOrdinal("phone"));
                employee.Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email"));
                employee.Status = reader.GetString(reader.GetOrdinal("status"));
            }
            
            return employee;
        }

        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
                _db = null;
            }
        }
    }
}