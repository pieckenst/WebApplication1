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
                    j.job_title,
                    (SELECT STRING_AGG(CAST(r.role_name AS nvarchar(max)), N', ')
                                WITHIN GROUP (ORDER BY r.role_name)
                     FROM dbo.user_role AS ur
                     INNER JOIN dbo.app_role AS r ON r.role_id = ur.role_id
                     WHERE ur.user_id = u.user_id AND r.is_active = 1) AS active_role_names,
                    (SELECT STRING_AGG(CAST(r.role_name AS nvarchar(max)), N', ')
                                WITHIN GROUP (ORDER BY r.role_name)
                     FROM dbo.user_role AS ur
                     INNER JOIN dbo.app_role AS r ON r.role_id = ur.role_id
                     WHERE ur.user_id = u.user_id AND r.is_active = 0) AS inactive_role_names
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
        /// Atomically saves a user, optional password and role assignments.
        /// Protects the current and last active administrator and validates every selected role.
        /// </summary>
        public int SaveManagedUser(User user, string password, List<int> roleIds, string assignedBy, int actorUserId)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (actorUserId <= 0) throw new InvalidOperationException("A valid administrator identity is required.");

            bool isNew = user.UserId <= 0;
            if (isNew && string.IsNullOrWhiteSpace(password))
                throw new InvalidOperationException("A password is required when creating a user.");
            string passwordHash = string.IsNullOrWhiteSpace(password) ? null : HashPassword(password);
            List<int> uniqueRoleIds = roleIds == null ? new List<int>() : new List<int>(new HashSet<int>(roleIds));

            _db.BeginTransaction(IsolationLevel.Serializable);
            try
            {
                int userId = user.UserId;
                bool currentlyActive = false;
                if (!isNew)
                {
                    object loginValue = _db.ExecuteScalar(
                        "SELECT login FROM dbo.app_user WITH (UPDLOCK, HOLDLOCK) WHERE user_id = @user_id",
                        new SqlParameter[] { DatabaseHelper.CreateParameter("@user_id", userId, SqlDbType.Int) });
                    if (loginValue == null || loginValue == DBNull.Value)
                        throw new InvalidOperationException("The user no longer exists. Refresh the list and try again.");

                    currentlyActive = Convert.ToBoolean(_db.ExecuteScalar(
                        "SELECT is_active FROM dbo.app_user WITH (UPDLOCK, HOLDLOCK) WHERE user_id = @user_id",
                        new SqlParameter[] { DatabaseHelper.CreateParameter("@user_id", userId, SqlDbType.Int) }));
                    if (userId == actorUserId && !user.IsActive)
                        throw new InvalidOperationException("You cannot deactivate your own account.");
                }
                else
                {
                    int duplicateLogin = Convert.ToInt32(_db.ExecuteScalar(
                        "SELECT COUNT(*) FROM dbo.app_user WITH (UPDLOCK, HOLDLOCK) WHERE login = @login",
                        new SqlParameter[] { DatabaseHelper.CreateParameter("@login", user.Login, SqlDbType.VarChar, 80) }));
                    if (duplicateLogin > 0)
                        throw new InvalidOperationException("A user with this login already exists.");
                }

                if (user.EmployeeId.HasValue)
                {
                    int employeeLinkedElsewhere = Convert.ToInt32(_db.ExecuteScalar(
                        @"SELECT COUNT(*) FROM dbo.app_user WITH (UPDLOCK, HOLDLOCK)
                          WHERE employee_id = @employee_id AND user_id <> @user_id",
                        new SqlParameter[]
                        {
                            DatabaseHelper.CreateParameter("@employee_id", user.EmployeeId.Value, SqlDbType.Int),
                            DatabaseHelper.CreateParameter("@user_id", userId, SqlDbType.Int)
                        }));
                    if (employeeLinkedElsewhere > 0)
                        throw new InvalidOperationException("This employee is already linked to another account.");
                }

                int activeRoleCount = 0;
                foreach (int roleId in uniqueRoleIds)
                {
                    if (roleId <= 0) throw new InvalidOperationException("One or more selected roles are invalid.");
                    object activeValue = _db.ExecuteScalar(
                        "SELECT is_active FROM dbo.app_role WITH (UPDLOCK, HOLDLOCK) WHERE role_id = @role_id",
                        new SqlParameter[] { DatabaseHelper.CreateParameter("@role_id", roleId, SqlDbType.Int) });
                    if (activeValue == null || activeValue == DBNull.Value)
                        throw new InvalidOperationException("One or more selected roles no longer exist.");

                    bool activeRole = Convert.ToBoolean(activeValue);
                    bool previouslyAssigned = !isNew && Convert.ToInt32(_db.ExecuteScalar(
                        "SELECT COUNT(*) FROM dbo.user_role WITH (UPDLOCK, HOLDLOCK) WHERE user_id = @user_id AND role_id = @role_id",
                        new SqlParameter[]
                        {
                            DatabaseHelper.CreateParameter("@user_id", userId, SqlDbType.Int),
                            DatabaseHelper.CreateParameter("@role_id", roleId, SqlDbType.Int)
                        })) > 0;
                    if (!activeRole && !previouslyAssigned)
                        throw new InvalidOperationException("An inactive role cannot be newly assigned. Activate the role first.");
                    if (activeRole) activeRoleCount++;
                }

                if (user.IsActive && activeRoleCount == 0)
                    throw new InvalidOperationException("An active account must have at least one active role.");

                object adminRoleValue = _db.ExecuteScalar(
                    "SELECT role_id FROM dbo.app_role WITH (UPDLOCK, HOLDLOCK) WHERE role_name = 'administrator' AND is_active = 1");
                int adminRoleId = adminRoleValue == null || adminRoleValue == DBNull.Value ? 0 : Convert.ToInt32(adminRoleValue);
                bool currentlyAdministrator = !isNew && currentlyActive && adminRoleId > 0 &&
                    Convert.ToInt32(_db.ExecuteScalar(
                        "SELECT COUNT(*) FROM dbo.user_role WHERE user_id = @user_id AND role_id = @role_id",
                        new SqlParameter[]
                        {
                            DatabaseHelper.CreateParameter("@user_id", userId, SqlDbType.Int),
                            DatabaseHelper.CreateParameter("@role_id", adminRoleId, SqlDbType.Int)
                        })) > 0;
                bool willBeAdministrator = user.IsActive && adminRoleId > 0 && uniqueRoleIds.Contains(adminRoleId);

                if (userId == actorUserId && currentlyAdministrator && !willBeAdministrator)
                    throw new InvalidOperationException("You cannot remove your own administrator role.");

                if (currentlyAdministrator && !willBeAdministrator)
                {
                    int otherAdministrators = Convert.ToInt32(_db.ExecuteScalar(
                        @"SELECT COUNT(DISTINCT u.user_id)
                          FROM dbo.app_user AS u WITH (UPDLOCK, HOLDLOCK)
                          INNER JOIN dbo.user_role AS ur ON ur.user_id = u.user_id
                          INNER JOIN dbo.app_role AS r ON r.role_id = ur.role_id
                          WHERE u.is_active = 1 AND r.is_active = 1
                            AND r.role_name = 'administrator' AND u.user_id <> @user_id",
                        new SqlParameter[] { DatabaseHelper.CreateParameter("@user_id", userId, SqlDbType.Int) }));
                    if (otherAdministrators == 0)
                        throw new InvalidOperationException("The last active administrator cannot be deactivated or stripped of the administrator role.");
                }

                if (isNew)
                {
                    object insertedId = _db.ExecuteScalar(
                        @"INSERT INTO dbo.app_user
                            (login, password_hash, email, phone_number, employee_id, is_active)
                          VALUES
                            (@login, @password_hash, @email, @phone_number, @employee_id, @is_active);
                          SELECT CAST(SCOPE_IDENTITY() AS int);",
                        new SqlParameter[]
                        {
                            DatabaseHelper.CreateParameter("@login", user.Login, SqlDbType.VarChar, 80),
                            DatabaseHelper.CreateParameter("@password_hash", passwordHash, SqlDbType.VarChar, 256),
                            DatabaseHelper.CreateParameter("@email", user.Email ?? (object)DBNull.Value, SqlDbType.VarChar, 150),
                            DatabaseHelper.CreateParameter("@phone_number", user.PhoneNumber ?? (object)DBNull.Value, SqlDbType.VarChar, 30),
                            DatabaseHelper.CreateParameter("@employee_id", user.EmployeeId ?? (object)DBNull.Value, SqlDbType.Int),
                            DatabaseHelper.CreateParameter("@is_active", user.IsActive, SqlDbType.Bit)
                        });
                    userId = Convert.ToInt32(insertedId);
                }
                else
                {
                    _db.ExecuteNonQuery(
                        @"UPDATE dbo.app_user
                          SET email = @email,
                              phone_number = @phone_number,
                              employee_id = @employee_id,
                              is_active = @is_active,
                              password_hash = COALESCE(@password_hash, password_hash)
                          WHERE user_id = @user_id",
                        new SqlParameter[]
                        {
                            DatabaseHelper.CreateParameter("@user_id", userId, SqlDbType.Int),
                            DatabaseHelper.CreateParameter("@email", user.Email ?? (object)DBNull.Value, SqlDbType.VarChar, 150),
                            DatabaseHelper.CreateParameter("@phone_number", user.PhoneNumber ?? (object)DBNull.Value, SqlDbType.VarChar, 30),
                            DatabaseHelper.CreateParameter("@employee_id", user.EmployeeId ?? (object)DBNull.Value, SqlDbType.Int),
                            DatabaseHelper.CreateParameter("@is_active", user.IsActive, SqlDbType.Bit),
                            DatabaseHelper.CreateParameter("@password_hash", passwordHash ?? (object)DBNull.Value, SqlDbType.VarChar, 256)
                        });
                }

                _db.ExecuteNonQuery("DELETE FROM dbo.user_role WHERE user_id = @user_id",
                    new SqlParameter[] { DatabaseHelper.CreateParameter("@user_id", userId, SqlDbType.Int) });
                foreach (int roleId in uniqueRoleIds)
                    _db.ExecuteNonQuery(
                        @"INSERT INTO dbo.user_role (user_id, role_id, assigned_by)
                          VALUES (@user_id, @role_id, @assigned_by)",
                        new SqlParameter[]
                        {
                            DatabaseHelper.CreateParameter("@user_id", userId, SqlDbType.Int),
                            DatabaseHelper.CreateParameter("@role_id", roleId, SqlDbType.Int),
                            DatabaseHelper.CreateParameter("@assigned_by", assignedBy, SqlDbType.VarChar, 80)
                        });

                _db.CommitTransaction();
                return userId;
            }
            catch { _db.RollbackTransaction(); throw; }
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
            if (user == null)
            {
                VerifyPassword(password, DummyPasswordHash);
                return null;
            }

            if (VerifyPassword(password, user.PasswordHash) && user.IsActive)
            {
                if (IsLegacyHash(user.PasswordHash))
                    ChangePassword(user.UserId, password);
                UpdateLastLogin(user.UserId);
                return user;
            }

            return null;
        }

        /// <summary>
        /// Hashes a password using versioned PBKDF2 with a per-password random salt.
        /// </summary>
        private string HashPassword(string password)
        {
            const int iterations = 210000;
            byte[] salt = new byte[16];
            using (RandomNumberGenerator random = RandomNumberGenerator.Create())
                random.GetBytes(salt);

            using (Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                byte[] hash = deriveBytes.GetBytes(32);
                return "PBKDF2-SHA1$" + iterations + "$" + Convert.ToBase64String(salt) + "$" + Convert.ToBase64String(hash);
            }
        }

        /// <summary>
        /// Verifies a password against a hash.
        /// </summary>
        private bool VerifyPassword(string password, string hash)
        {
            if (string.IsNullOrEmpty(hash))
                return false;

            string[] parts = hash.Split('$');
            if (parts.Length == 4 && parts[0] == "PBKDF2-SHA1")
            {
                int iterations;
                byte[] salt;
                byte[] expected;
                try
                {
                    if (!int.TryParse(parts[1], out iterations) || iterations < 100000 || iterations > 1000000)
                        return false;
                    salt = Convert.FromBase64String(parts[2]);
                    expected = Convert.FromBase64String(parts[3]);
                    if (salt.Length < 16 || expected.Length != 32)
                        return false;
                }
                catch (FormatException)
                {
                    return false;
                }

                using (Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(password, salt, iterations))
                    return FixedTimeEquals(expected, deriveBytes.GetBytes(expected.Length));
            }

            if (!IsLegacyHash(hash))
                return false;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] actual = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                byte[] expected = new byte[actual.Length];
                for (int i = 0; i < expected.Length; i++)
                    expected[i] = Convert.ToByte(hash.Substring(i * 2, 2), 16);
                return FixedTimeEquals(expected, actual);
            }
        }

        private static bool IsLegacyHash(string hash)
        {
            if (hash == null || hash.Length != 64)
                return false;
            foreach (char character in hash)
                if (!Uri.IsHexDigit(character)) return false;
            return true;
        }

        private static bool FixedTimeEquals(byte[] left, byte[] right)
        {
            if (left == null || right == null || left.Length != right.Length)
                return false;
            int difference = 0;
            for (int i = 0; i < left.Length; i++)
                difference |= left[i] ^ right[i];
            return difference == 0;
        }

        private static readonly string DummyPasswordHash = CreateDummyPasswordHash();

        private static string CreateDummyPasswordHash()
        {
            byte[] salt = new byte[16];
            using (RandomNumberGenerator random = RandomNumberGenerator.Create())
                random.GetBytes(salt);
            using (Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes("invalid-credential", salt, 210000))
                return "PBKDF2-SHA1$210000$" + Convert.ToBase64String(salt) + "$" + Convert.ToBase64String(deriveBytes.GetBytes(32));
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

        public bool SetUserRoles(int userId, List<int> roleIds, string assignedBy)
        {
            List<int> uniqueRoleIds = roleIds == null
                ? new List<int>()
                : new List<int>(new HashSet<int>(roleIds));
            _db.BeginTransaction(IsolationLevel.Serializable);
            try
            {
                _db.ExecuteNonQuery("DELETE FROM dbo.user_role WHERE user_id = @user_id",
                    new SqlParameter[] { DatabaseHelper.CreateParameter("@user_id", userId, SqlDbType.Int) });

                foreach (int roleId in uniqueRoleIds)
                {
                    _db.ExecuteNonQuery(@"
                        INSERT INTO dbo.user_role (user_id, role_id, assigned_by)
                        VALUES (@user_id, @role_id, @assigned_by)",
                        new SqlParameter[]
                        {
                            DatabaseHelper.CreateParameter("@user_id", userId, SqlDbType.Int),
                            DatabaseHelper.CreateParameter("@role_id", roleId, SqlDbType.Int),
                            DatabaseHelper.CreateParameter("@assigned_by", assignedBy, SqlDbType.VarChar, 80)
                        });
                }

                _db.CommitTransaction();
                return true;
            }
            catch
            {
                _db.RollbackTransaction();
                throw;
            }
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
                    r.role_name,
                    r.is_active
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
                        RoleName = reader.GetString(5),
                        IsRoleActive = reader.GetBoolean(6)
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
                user.EmployeeJobTitle = reader.GetString(jobTitleOrdinal);

            int activeRolesOrdinal = TryGetOrdinal(reader, "active_role_names");
            if (activeRolesOrdinal >= 0 && !reader.IsDBNull(activeRolesOrdinal))
                user.ActiveRoleNames = reader.GetString(activeRolesOrdinal);
            int inactiveRolesOrdinal = TryGetOrdinal(reader, "inactive_role_names");
            if (inactiveRolesOrdinal >= 0 && !reader.IsDBNull(inactiveRolesOrdinal))
                user.InactiveRoleNames = reader.GetString(inactiveRolesOrdinal);

            return user;
        }

        private static int TryGetOrdinal(SqlDataReader reader, string columnName)
        {
            for (int ordinal = 0; ordinal < reader.FieldCount; ordinal++)
                if (string.Equals(reader.GetName(ordinal), columnName, StringComparison.OrdinalIgnoreCase))
                    return ordinal;
            return -1;
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
