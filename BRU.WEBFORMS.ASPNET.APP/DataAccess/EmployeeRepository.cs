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
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@job_id", jobId, SqlDbType.Int)
            };

            return GetEmployeesByJobQuery("e.job_id = @job_id", parameters);
        }

        public List<Employee> GetEmployeesByJobTitle(string jobTitle)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@job_title", jobTitle, SqlDbType.NVarChar, 100)
            };

            return GetEmployeesByJobQuery("j.job_title = @job_title", parameters);
        }

        private List<Employee> GetEmployeesByJobQuery(string predicate, SqlParameter[] parameters)
        {
            List<Employee> employees = new List<Employee>();
            string sql = @"
                SELECT e.*, CONCAT(e.surname, N' ', e.name, N' ', COALESCE(e.patronym, N'')) AS employee_name,
                       j.job_title, d.department_name,
                       DATEDIFF(YEAR, e.employed_date, GETDATE()) AS service_years
                FROM dbo.employee AS e
                INNER JOIN dbo.job AS j ON j.job_id = e.job_id
                LEFT JOIN dbo.department AS d ON d.department_id = e.department_id
                WHERE " + predicate + @"
                ORDER BY employee_name";

            using (SqlDataReader reader = _db.ExecuteReader(sql, parameters))
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

        public List<Department> GetDepartmentsForManagement()
        {
            List<Department> departments = new List<Department>();
            using (SqlDataReader reader = _db.ExecuteReader(@"
                SELECT d.department_id, d.department_name, d.department_code, d.description, d.is_active,
                       COUNT(e.employee_id) AS employee_count
                FROM dbo.department AS d
                LEFT JOIN dbo.employee AS e ON e.department_id = d.department_id
                GROUP BY d.department_id, d.department_name, d.department_code, d.description, d.is_active
                ORDER BY d.department_name"))
            {
                while (reader.Read())
                {
                    departments.Add(new Department
                    {
                        DepartmentId = reader.GetInt32(reader.GetOrdinal("department_id")),
                        DepartmentName = reader.GetString(reader.GetOrdinal("department_name")),
                        DepartmentCode = reader.GetString(reader.GetOrdinal("department_code")),
                        Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                        IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                        EmployeeCount = reader.GetInt32(reader.GetOrdinal("employee_count"))
                    });
                }
            }
            return departments;
        }

        public int InsertDepartment(Department department)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@department_name", department.DepartmentName, SqlDbType.NVarChar, 100),
                DatabaseHelper.CreateParameter("@department_code", department.DepartmentCode, SqlDbType.VarChar, 20),
                DatabaseHelper.CreateParameter("@description", department.Description ?? (object)DBNull.Value, SqlDbType.NVarChar, 500)
            };
            return Convert.ToInt32(_db.ExecuteScalar(@"
                INSERT INTO dbo.department (department_name, department_code, description)
                VALUES (@department_name, @department_code, @description);
                SELECT CAST(SCOPE_IDENTITY() AS int);", parameters));
        }

        public bool UpdateDepartment(Department department)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@department_id", department.DepartmentId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@department_name", department.DepartmentName, SqlDbType.NVarChar, 100),
                DatabaseHelper.CreateParameter("@department_code", department.DepartmentCode, SqlDbType.VarChar, 20),
                DatabaseHelper.CreateParameter("@description", department.Description ?? (object)DBNull.Value, SqlDbType.NVarChar, 500)
            };
            return _db.ExecuteNonQuery(@"
                UPDATE dbo.department
                SET department_name = @department_name, department_code = @department_code, description = @description
                WHERE department_id = @department_id", parameters) > 0;
        }

        public bool SetDepartmentActive(int departmentId, bool isActive)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@department_id", departmentId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@is_active", isActive, SqlDbType.Bit)
            };
            return _db.ExecuteNonQuery(
                "UPDATE dbo.department SET is_active = @is_active WHERE department_id = @department_id", parameters) > 0;
        }

        /// <summary>
        /// Map SqlDataReader to Employee object
        /// </summary>
        private Employee MapEmployeeFromReader(SqlDataReader reader)
        {
            Employee employee = new Employee();
            employee.EmployeeId = reader.GetInt32(reader.GetOrdinal("employee_id"));
            ReadEmployeeColumn(reader, "surname", delegate(string value) { employee.Surname = value; });
            ReadEmployeeColumn(reader, "name", delegate(string value) { employee.Name = value; });
            ReadEmployeeColumn(reader, "patronym", delegate(string value) { employee.Patronym = value; });
            ReadEmployeeColumn(reader, "status", delegate(string value) { employee.Status = value; });
            ReadEmployeeColumn(reader, "job_title", delegate(string value) { employee.JobTitle = value; });
            ReadEmployeeColumn(reader, "department_name", delegate(string value) { employee.DepartmentName = value; });
            ReadEmployeeColumn(reader, "employee_name", delegate(string value) { employee.EmployeeName = value; });

            int jobIdOrdinal = TryGetOrdinal(reader, "job_id");
            if (jobIdOrdinal >= 0 && !reader.IsDBNull(jobIdOrdinal)) employee.JobId = reader.GetInt32(jobIdOrdinal);
            int departmentIdOrdinal = TryGetOrdinal(reader, "department_id");
            if (departmentIdOrdinal >= 0 && !reader.IsDBNull(departmentIdOrdinal)) employee.DepartmentId = reader.GetInt32(departmentIdOrdinal);
            int employedDateOrdinal = TryGetOrdinal(reader, "employed_date");
            if (employedDateOrdinal >= 0 && !reader.IsDBNull(employedDateOrdinal)) employee.EmployedDate = reader.GetDateTime(employedDateOrdinal);
            int serviceYearsOrdinal = TryGetOrdinal(reader, "service_years");
            if (serviceYearsOrdinal >= 0 && !reader.IsDBNull(serviceYearsOrdinal)) employee.ServiceYears = reader.GetInt32(serviceYearsOrdinal);
            return employee;
        }

        private static void ReadEmployeeColumn(SqlDataReader reader, string columnName, Action<string> assign)
        {
            int ordinal = TryGetOrdinal(reader, columnName);
            if (ordinal >= 0 && !reader.IsDBNull(ordinal)) assign(reader.GetString(ordinal));
        }

        private static int TryGetOrdinal(SqlDataReader reader, string columnName)
        {
            for (int ordinal = 0; ordinal < reader.FieldCount; ordinal++)
            {
                if (string.Equals(reader.GetName(ordinal), columnName, StringComparison.OrdinalIgnoreCase))
                    return ordinal;
            }
            return -1;
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