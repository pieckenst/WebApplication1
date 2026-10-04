using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using BRU.WEBFORMS.ASPNET.APP.Models;

namespace BRU.WEBFORMS.ASPNET.APP.DataAccess
{
    /// <summary>
    /// Enterprise-grade data access layer for User operations.
    /// Handles authentication, user management, role assignments, and security.
    /// </summary>
    public class UserRepository : IDisposable
    {
        private DatabaseHelper _db;

        public UserRepository()
        {
            _db = new DatabaseHelper();
        }

        #region User CRUD Operations

        /// <summary>
        /// Gets all users with their employee information.
        /// </summary>
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            string sql = @"
                SELECT 
                    u.user_id,
                    u.login,
                    u.password_hash,
                    u.email,
                    u.phone_number,
                    u.employee_id,
                    u.is_active,
                    u.created_at,
                    u.last_login_at,
                    e.surname + ' ' + e.name + ISNULL(' ' + e.patronym, '') AS employee_name,
                    j.job_title
                FROM dbo.app_user u
                LEFT JOIN dbo.employee e ON u.employee_id = e.employee_id
                LEFT JOIN dbo.job j ON e.job_id = j.job_id
                ORDER BY u.login";

            using (SqlDataReader reader = _db.ExecuteReader(sql))
            {
                while (reader.Read())
                {
                    users.Add(MapUserFromReader(reader));
                }
            }

            return users;
        }

        /// <summary>
        /// Gets a user by their login.
        /// </summary>
        public User GetUserByLogin(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
                return null;

            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@login", login, SqlDbType.VarChar, 80)
            };

            string sql = @"
                SELECT 
                    u.user_id,
                    u.login,
                    u.password_hash,
                    u.email,
                    u.phone_number,
                    u.employee_id,
                    u.is_active,
                    u.created_at,
                    u.last_login_at,
                    e.surname + ' ' + e.name + ISNULL(' ' + e.patronym, '') AS employee_name,
                    j.job_title
                FROM dbo.app_user u
                LEFT JOIN dbo.employee e ON u.employee_id = e.employee_id
                LEFT JOIN dbo.job j ON e.job_id = j.job_id
                WHERE u.login = @login";

            using (SqlDataReader reader = _db.ExecuteReader(sql, parameters))
            {
                if (reader.Read())
                {
                    return MapUserFromReader(reader);
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a user by their ID.
        /// </summary>
        public User GetUserById(int userId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@user_id", userId, SqlDbType.Int)
            };

            string sql = @"
                SELECT 
                    u.user_id,
                    u.login,
                    u.password_hash,
                    u.email,
                    u.phone_number,
                    u.employee_id,
                    u.is_active,
                    u.created_at,
                    u.last_login_at,
                    e.surname + ' ' + e.name + ISNULL(' ' + e.patronym, '') AS employee_name,
                    j.job_title
                FROM dbo.app_user u
                LEFT JOIN dbo.employee e ON u.employee_id = e.employee_id
                LEFT JOIN dbo.job j ON e.job_id = j.job_id
                WHERE u.user_id = @user_id";

            using (SqlDataReader reader = _db.ExecuteReader(sql, parameters))
            {
                if (reader.Read())
                {
                    return MapUserFromReader(reader);
                }
            }

            return null;
        }

        /// <summary>
        /// Inserts a new user.
        /// </summary>
        public int InsertUser(User user, string password)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty", nameof(password));

            string passwordHash = HashPassword(password);

            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@login", user.Login, SqlDbType.VarChar, 80),
                DatabaseHelper.CreateParameter("@password_hash", passwordHash, SqlDbType.VarChar, 256),
                DatabaseHelper.CreateParameter("@email", user.Email ?? (object)DBNull.Value, SqlDbType.VarChar, 150),
                DatabaseHelper.CreateParameter("@phone_number", user.PhoneNumber ?? (object)DBNull.Value, SqlDbType.VarChar, 30),
                DatabaseHelper.CreateParameter("@employee_id", user.EmployeeId ?? (object)DBNull.Value, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@is_active", user.IsActive, SqlDbType.Bit)
            };

            string sql = @"
                INSERT INTO dbo.app_user (login, password_hash, email, phone_number, employee_id, is_active)
                VALUES (@login, @password_hash, @email, @phone_number, @employee_id, @is_active);
                SELECT CAST(SCOPE_IDENTITY() AS int);";

            object result = _db.ExecuteScalar(sql, parameters);
            return Convert.ToInt32(result);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        public bool UpdateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@user_id", user.UserId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@email", user.Email ?? (object)DBNull.Value, SqlDbType.VarChar, 150),
                DatabaseHelper.CreateParameter("@phone_number", user.PhoneNumber ?? (object)DBNull.Value, SqlDbType.VarChar, 30),
                DatabaseHelper.CreateParameter("@employee_id", user.EmployeeId ?? (object)DBNull.Value, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@is_active", user.IsActive, SqlDbType.Bit)
            };

            string sql = @"
                UPDATE dbo.app_user
                SET email = @email,
                    phone_number = @phone_number,
                    employee_id = @employee_id,
                    is_active = @is_active
                WHERE user_id = @user_id";

            int rowsAffected = _db.ExecuteNonQuery(sql, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Changes a user's password.
        /// </summary>
        public bool ChangePassword(int userId, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("Password cannot be empty", nameof(newPassword));

            string passwordHash = HashPassword(newPassword);

            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@user_id", userId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@password_hash", passwordHash, SqlDbType.VarChar, 256)
            };

            string sql = @"
                UPDATE dbo.app_user
                SET password_hash = @password_hash
                WHERE user_id = @user_id";

            int rowsAffected = _db.ExecuteNonQuery(sql, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Updates the last login timestamp for a user.
        /// </summary>
        public bool UpdateLastLogin(int userId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@user_id", userId, SqlDbType.Int)
            };

            string sql = @"
                UPDATE dbo.app_user
                SET last_login_at = SYSDATETIME()
                WHERE user_id = @user_id";

            int rowsAffected = _db.ExecuteNonQuery(sql, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Deletes a user (sets inactive rather than physical delete).
        /// </summary>
        public bool DeleteUser(int userId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@user_id", userId, SqlDbType.Int)
            };

            string sql = @"
                UPDATE dbo.app_user
                SET is_active = 0
                WHERE user_id = @user_id";

            int rowsAffected = _db.ExecuteNonQuery(sql, parameters);
            return rowsAffected > 0;
        }

        #endregion

        #region Authentication

        /// <summary>
        /// Authenticates a user by login and password.
        /// </summary>
        public User AuthenticateUser(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                return null;

            User user = GetUserByLogin(login);
            if (user == null || !user.IsActive)
                return null;

            if (VerifyPassword(password, user.PasswordHash))
            {
                UpdateLastLogin(user.UserId);
                return user;
            }

            return null;
        }

        /// <summary>
        /// Hashes a password using SHA256.
        /// </summary>
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Verifies a password against a hash.
        /// </summary>
        private bool VerifyPassword(string password, string hash)
        {
            string passwordHash = HashPassword(password);
            return string.Equals(passwordHash, hash, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region User Roles

        /// <summary>
        /// Gets all roles for a specific user.
        /// </summary>
        public List<string> GetUserRoles(int userId)
        {
            List<string> roles = new List<string>();

            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@user_id", userId, SqlDbType.Int)
            };

            string sql = @"
                SELECT r.role_name
                FROM dbo.user_role ur
                INNER JOIN dbo.app_role r ON ur.role_id = r.role_id
                WHERE ur.user_id = @user_id AND r.is_active = 1
                ORDER BY r.role_name";

            using (SqlDataReader reader = _db.ExecuteReader(sql, parameters))
            {
                while (reader.Read())
                {
                    roles.Add(reader.GetString(0));
                }
            }

            return roles;
        }

        /// <summary>
        /// Gets all permissions for a specific user (through their roles).
        /// </summary>
        public List<string> GetUserPermissions(int userId)
        {
            List<string> permissions = new List<string>();

            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@user_id", userId, SqlDbType.Int)
            };

            string sql = @"
                SELECT DISTINCT p.permission_name
                FROM dbo.user_role ur
                INNER JOIN dbo.role_permission rp ON ur.role_id = rp.role_id
                INNER JOIN dbo.permission p ON rp.permission_id = p.permission_id
                INNER JOIN dbo.app_role r ON ur.role_id = r.role_id
                WHERE ur.user_id = @user_id AND r.is_active = 1
                ORDER BY p.permission_name";

            using (SqlDataReader reader = _db.ExecuteReader(sql, parameters))
            {
                while (reader.Read())
                {
                    permissions.Add(reader.GetString(0));
                }
            }

            return permissions;
        }

        /// <summary>
        /// Assigns a role to a user.
        /// </summary>
        public bool AssignRoleToUser(int userId, int roleId, string assignedBy = null)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@user_id", userId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@role_id", roleId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@assigned_by", assignedBy ?? (object)DBNull.Value, SqlDbType.VarChar, 80)
            };

            string sql = @"
                IF NOT EXISTS (SELECT 1 FROM dbo.user_role WHERE user_id = @user_id AND role_id = @role_id)
                BEGIN
                    INSERT INTO dbo.user_role (user_id, role_id, assigned_by)
                    VALUES (@user_id, @role_id, @assigned_by)
                END";

            int rowsAffected = _db.ExecuteNonQuery(sql, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Removes a role from a user.
        /// </summary>
        public bool RemoveRoleFromUser(int userId, int roleId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@user_id", userId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@role_id", roleId, SqlDbType.Int)
            };

            string sql = @"
                DELETE FROM dbo.user_role
                WHERE user_id = @user_id AND role_id = @role_id";

            int rowsAffected = _db.ExecuteNonQuery(sql, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Gets user-role assignments with details.
        /// </summary>
        public List<UserRole> GetUserRoleAssignments(int userId)
        {
            List<UserRole> assignments = new List<UserRole>();

            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@user_id", userId, SqlDbType.Int)
            };

            string sql = @"
                SELECT 
                    ur.user_id,
                    ur.role_id,
                    ur.assigned_at,
                    ur.assigned_by,
                    u.login,
                    r.role_name
                FROM dbo.user_role ur
                INNER JOIN dbo.app_user u ON ur.user_id = u.user_id
                INNER JOIN dbo.app_role r ON ur.role_id = r.role_id
                WHERE ur.user_id = @user_id
                ORDER BY ur.assigned_at DESC";

            using (SqlDataReader reader = _db.ExecuteReader(sql, parameters))
            {
                while (reader.Read())
                {
                    UserRole assignment = new UserRole
                    {
                        UserId = reader.GetInt32(0),
                        RoleId = reader.GetInt32(1),
                        AssignedAt = reader.GetDateTime(2),
                        AssignedBy = reader.IsDBNull(3) ? null : reader.GetString(3),
                        Login = reader.GetString(4),
                        RoleName = reader.GetString(5)
                    };
                    assignments.Add(assignment);
                }
            }

            return assignments;
        }

        /// <summary>
        /// Gets all users with a specific role.
        /// </summary>
        public List<User> GetUsersByRole(int roleId)
        {
            List<User> users = new List<User>();

            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@role_id", roleId, SqlDbType.Int)
            };

            string sql = @"
                SELECT 
                    u.user_id,
                    u.login,
                    u.password_hash,
                    u.email,
                    u.phone_number,
                    u.employee_id,
                    u.is_active,
                    u.created_at,
                    u.last_login_at,
                    e.surname + ' ' + e.name + ISNULL(' ' + e.patronym, '') AS employee_name,
                    j.job_title
                FROM dbo.user_role ur
                INNER JOIN dbo.app_user u ON ur.user_id = u.user_id
                LEFT JOIN dbo.employee e ON u.employee_id = e.employee_id
                LEFT JOIN dbo.job j ON e.job_id = j.job_id
                WHERE ur.role_id = @role_id
                ORDER BY u.login";

            using (SqlDataReader reader = _db.ExecuteReader(sql, parameters))
            {
                while (reader.Read())
                {
                    users.Add(MapUserFromReader(reader));
                }
            }

            return users;
        }

        #endregion

        #region Helper Methods

        private User MapUserFromReader(SqlDataReader reader)
        {
            User user = new User
            {
                UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                Login = reader.GetString(reader.GetOrdinal("login")),
                PasswordHash = reader.GetString(reader.GetOrdinal("password_hash")),
                Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email")),
                PhoneNumber = reader.IsDBNull(reader.GetOrdinal("phone_number")) ? null : reader.GetString(reader.GetOrdinal("phone_number")),
                EmployeeId = reader.IsDBNull(reader.GetOrdinal("employee_id")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("employee_id")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                LastLoginAt = reader.IsDBNull(reader.GetOrdinal("last_login_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("last_login_at"))
            };

            // Navigation properties
            int employeeNameOrdinal = reader.GetOrdinal("employee_name");
            if (!reader.IsDBNull(employeeNameOrdinal))
            {
                user.EmployeeName = reader.GetString(employeeNameOrdinal);
            }

            int jobTitleOrdinal = reader.GetOrdinal("job_title");
            if (!reader.IsDBNull(jobTitleOrdinal))
            {
                user.EmployeeJobTitle = reader.GetString(jobTitleOrdinal);
            }

            return user;
        }

        #endregion

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
