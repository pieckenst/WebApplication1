using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BRU.WEBFORMS.ASPNET.APP.Models;

namespace BRU.WEBFORMS.ASPNET.APP.DataAccess
{
    /// <summary>
    /// Enterprise-grade data access layer for Role and Permission operations.
    /// </summary>
    public class RoleRepository : IDisposable
    {
        private DatabaseHelper _db;

        public RoleRepository()
        {
            _db = new DatabaseHelper();
        }

        #region Role Operations

        /// <summary>
        /// Gets all roles with user and permission counts.
        /// </summary>
        public List<Role> GetAllRoles()
        {
            List<Role> roles = new List<Role>();

            string sql = @"
                SELECT 
                    r.role_id,
                    r.role_name,
                    r.description,
                    r.is_active,
                    COUNT(DISTINCT ur.user_id) AS user_count,
                    COUNT(DISTINCT rp.permission_id) AS permission_count
                FROM dbo.app_role r
                LEFT JOIN dbo.user_role ur ON r.role_id = ur.role_id
                LEFT JOIN dbo.role_permission rp ON r.role_id = rp.role_id
                GROUP BY r.role_id, r.role_name, r.description, r.is_active
                ORDER BY r.role_name";

            using (SqlDataReader reader = _db.ExecuteReader(sql))
            {
                while (reader.Read())
                {
                    roles.Add(MapRoleFromReader(reader));
                }
            }

            return roles;
        }

        /// <summary>
        /// Gets a role by ID.
        /// </summary>
        public Role GetRoleById(int roleId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@role_id", roleId, SqlDbType.Int)
            };

            string sql = @"
                SELECT 
                    r.role_id,
                    r.role_name,
                    r.description,
                    r.is_active,
                    COUNT(DISTINCT ur.user_id) AS user_count,
                    COUNT(DISTINCT rp.permission_id) AS permission_count
                FROM dbo.app_role r
                LEFT JOIN dbo.user_role ur ON r.role_id = ur.role_id
                LEFT JOIN dbo.role_permission rp ON r.role_id = rp.role_id
                WHERE r.role_id = @role_id
                GROUP BY r.role_id, r.role_name, r.description, r.is_active";

            using (SqlDataReader reader = _db.ExecuteReader(sql, parameters))
            {
                if (reader.Read())
                {
                    return MapRoleFromReader(reader);
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a role by name.
        /// </summary>
        public Role GetRoleByName(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return null;

            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@role_name", roleName, SqlDbType.VarChar, 50)
            };

            string sql = @"
                SELECT 
                    r.role_id,
                    r.role_name,
                    r.description,
                    r.is_active,
                    COUNT(DISTINCT ur.user_id) AS user_count,
                    COUNT(DISTINCT rp.permission_id) AS permission_count
                FROM dbo.app_role r
                LEFT JOIN dbo.user_role ur ON r.role_id = ur.role_id
                LEFT JOIN dbo.role_permission rp ON r.role_id = rp.role_id
                WHERE r.role_name = @role_name
                GROUP BY r.role_id, r.role_name, r.description, r.is_active";

            using (SqlDataReader reader = _db.ExecuteReader(sql, parameters))
            {
                if (reader.Read())
                {
                    return MapRoleFromReader(reader);
                }
            }

            return null;
        }

        /// <summary>
        /// Inserts a new role.
        /// </summary>
        public int InsertRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@role_name", role.RoleName, SqlDbType.VarChar, 50),
                DatabaseHelper.CreateParameter("@description", role.Description ?? (object)DBNull.Value, SqlDbType.NVarChar, 300),
                DatabaseHelper.CreateParameter("@is_active", role.IsActive, SqlDbType.Bit)
            };

            string sql = @"
                INSERT INTO dbo.app_role (role_name, description, is_active)
                VALUES (@role_name, @description, @is_active);
                SELECT CAST(SCOPE_IDENTITY() AS int);";

            object result = _db.ExecuteScalar(sql, parameters);
            return Convert.ToInt32(result);
        }

        /// <summary>
        /// Updates an existing role.
        /// </summary>
        public bool UpdateRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@role_id", role.RoleId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@role_name", role.RoleName, SqlDbType.VarChar, 50),
                DatabaseHelper.CreateParameter("@description", role.Description ?? (object)DBNull.Value, SqlDbType.NVarChar, 300),
                DatabaseHelper.CreateParameter("@is_active", role.IsActive, SqlDbType.Bit)
            };

            string sql = @"
                UPDATE dbo.app_role
                SET role_name = @role_name,
                    description = @description,
                    is_active = @is_active
                WHERE role_id = @role_id";

            int rowsAffected = _db.ExecuteNonQuery(sql, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Deletes a role (only if no users are assigned).
        /// </summary>
        public bool DeleteRole(int roleId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@role_id", roleId, SqlDbType.Int)
            };

            // First check if any users have this role
            string checkSql = "SELECT COUNT(*) FROM dbo.user_role WHERE role_id = @role_id";
            object result = _db.ExecuteScalar(checkSql, parameters);
            int userCount = Convert.ToInt32(result);

            if (userCount > 0)
            {
                throw new InvalidOperationException($"Cannot delete role because {userCount} user(s) are assigned to it.");
            }

            // Delete role permissions first
            string deletePermissionsSql = "DELETE FROM dbo.role_permission WHERE role_id = @role_id";
            _db.ExecuteNonQuery(deletePermissionsSql, parameters);

            // Then delete the role
            string deleteSql = "DELETE FROM dbo.app_role WHERE role_id = @role_id";
            int rowsAffected = _db.ExecuteNonQuery(deleteSql, parameters);
            return rowsAffected > 0;
        }

        #endregion

        #region Permission Operations

        /// <summary>
        /// Gets all permissions.
        /// </summary>
        public List<Permission> GetAllPermissions()
        {
            List<Permission> permissions = new List<Permission>();

            string sql = @"
                SELECT 
                    permission_id,
                    permission_name,
                    description
                FROM dbo.permission
                ORDER BY permission_name";

            using (SqlDataReader reader = _db.ExecuteReader(sql))
            {
                while (reader.Read())
                {
                    permissions.Add(MapPermissionFromReader(reader));
                }
            }

            return permissions;
        }

        /// <summary>
        /// Gets a permission by ID.
        /// </summary>
        public Permission GetPermissionById(int permissionId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@permission_id", permissionId, SqlDbType.Int)
            };

            string sql = @"
                SELECT 
                    permission_id,
                    permission_name,
                    description
                FROM dbo.permission
                WHERE permission_id = @permission_id";

            using (SqlDataReader reader = _db.ExecuteReader(sql, parameters))
            {
                if (reader.Read())
                {
                    return MapPermissionFromReader(reader);
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a permission by name.
        /// </summary>
        public Permission GetPermissionByName(string permissionName)
        {
            if (string.IsNullOrWhiteSpace(permissionName))
                return null;

            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@permission_name", permissionName, SqlDbType.VarChar, 100)
            };

            string sql = @"
                SELECT 
                    permission_id,
                    permission_name,
                    description
                FROM dbo.permission
                WHERE permission_name = @permission_name";

            using (SqlDataReader reader = _db.ExecuteReader(sql, parameters))
            {
                if (reader.Read())
                {
                    return MapPermissionFromReader(reader);
                }
            }

            return null;
        }

        /// <summary>
        /// Inserts a new permission.
        /// </summary>
        public int InsertPermission(Permission permission)
        {
            if (permission == null)
                throw new ArgumentNullException(nameof(permission));

            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@permission_name", permission.PermissionName, SqlDbType.VarChar, 100),
                DatabaseHelper.CreateParameter("@description", permission.Description ?? (object)DBNull.Value, SqlDbType.NVarChar, 300)
            };

            string sql = @"
                INSERT INTO dbo.permission (permission_name, description)
                VALUES (@permission_name, @description);
                SELECT CAST(SCOPE_IDENTITY() AS int);";

            object result = _db.ExecuteScalar(sql, parameters);
            return Convert.ToInt32(result);
        }

        /// <summary>
        /// Updates an existing permission.
        /// </summary>
        public bool UpdatePermission(Permission permission)
        {
            if (permission == null)
                throw new ArgumentNullException(nameof(permission));

            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@permission_id", permission.PermissionId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@description", permission.Description ?? (object)DBNull.Value, SqlDbType.NVarChar, 300)
            };

            string sql = @"
                UPDATE dbo.permission
                SET description = @description
                WHERE permission_id = @permission_id";

            int rowsAffected = _db.ExecuteNonQuery(sql, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Deletes a permission (only if not assigned to any role).
        /// </summary>
        public bool DeletePermission(int permissionId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@permission_id", permissionId, SqlDbType.Int)
            };

            // First check if any roles have this permission
            string checkSql = "SELECT COUNT(*) FROM dbo.role_permission WHERE permission_id = @permission_id";
            object result = _db.ExecuteScalar(checkSql, parameters);
            int roleCount = Convert.ToInt32(result);

            if (roleCount > 0)
            {
                throw new InvalidOperationException($"Cannot delete permission because it is assigned to {roleCount} role(s).");
            }

            string deleteSql = "DELETE FROM dbo.permission WHERE permission_id = @permission_id";
            int rowsAffected = _db.ExecuteNonQuery(deleteSql, parameters);
            return rowsAffected > 0;
        }

        #endregion

        #region Role-Permission Operations

        /// <summary>
        /// Gets all permissions for a specific role.
        /// </summary>
        public List<Permission> GetRolePermissions(int roleId)
        {
            List<Permission> permissions = new List<Permission>();

            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@role_id", roleId, SqlDbType.Int)
            };

            string sql = @"
                SELECT 
                    p.permission_id,
                    p.permission_name,
                    p.description
                FROM dbo.role_permission rp
                INNER JOIN dbo.permission p ON rp.permission_id = p.permission_id
                WHERE rp.role_id = @role_id
                ORDER BY p.permission_name";

            using (SqlDataReader reader = _db.ExecuteReader(sql, parameters))
            {
                while (reader.Read())
                {
                    permissions.Add(MapPermissionFromReader(reader));
                }
            }

            return permissions;
        }

        public List<DatabaseSecurityGrant> GetDatabaseSecuritySnapshot()
        {
            List<DatabaseSecurityGrant> grants = new List<DatabaseSecurityGrant>();
            string sql = @"
                SELECT DB_NAME() AS database_name,
                       USER_NAME() AS database_user,
                       SUSER_SNAME() AS server_login,
                       COALESCE(role_principal.name, N'(no explicit database role)') AS database_role,
                       CAST(IS_MEMBER(N'db_owner') AS bit) AS is_database_owner,
                       CAST(IS_SRVROLEMEMBER(N'sysadmin') AS bit) AS is_server_admin,
                       COALESCE(OBJECT_SCHEMA_NAME(permission.major_id) + N'.' + OBJECT_NAME(permission.major_id), N'(database scope)') AS securable,
                       COALESCE(permission.permission_name, N'(no explicit grant)') AS permission_name,
                       COALESCE(permission.state_desc, N'INHERITED') AS permission_state
                FROM sys.database_principals AS current_principal
                LEFT JOIN sys.database_role_members AS membership
                    ON membership.member_principal_id = current_principal.principal_id
                LEFT JOIN sys.database_principals AS role_principal
                    ON role_principal.principal_id = membership.role_principal_id
                LEFT JOIN sys.database_permissions AS permission
                    ON permission.grantee_principal_id = current_principal.principal_id
                    OR permission.grantee_principal_id = role_principal.principal_id
                WHERE current_principal.principal_id = DATABASE_PRINCIPAL_ID()
                ORDER BY database_role, securable, permission_name";

            using (SqlDataReader reader = _db.ExecuteReader(sql))
            {
                while (reader.Read())
                {
                    grants.Add(new DatabaseSecurityGrant
                    {
                        DatabaseName = reader.GetString(reader.GetOrdinal("database_name")),
                        DatabaseUser = reader.GetString(reader.GetOrdinal("database_user")),
                        ServerLogin = reader.IsDBNull(reader.GetOrdinal("server_login")) ? null : reader.GetString(reader.GetOrdinal("server_login")),
                        DatabaseRole = reader.GetString(reader.GetOrdinal("database_role")),
                        IsDatabaseOwner = reader.GetBoolean(reader.GetOrdinal("is_database_owner")),
                        IsServerAdmin = reader.GetBoolean(reader.GetOrdinal("is_server_admin")),
                        Securable = reader.GetString(reader.GetOrdinal("securable")),
                        PermissionName = reader.GetString(reader.GetOrdinal("permission_name")),
                        PermissionState = reader.GetString(reader.GetOrdinal("permission_state"))
                    });
                }
            }
            return grants;
        }

        /// <summary>
        /// Gets all roles that have a specific permission.
        /// </summary>
        public List<Role> GetRolesWithPermission(int permissionId)
        {
            List<Role> roles = new List<Role>();

            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@permission_id", permissionId, SqlDbType.Int)
            };

            string sql = @"
                SELECT 
                    r.role_id,
                    r.role_name,
                    r.description,
                    r.is_active,
                    0 AS user_count,
                    0 AS permission_count
                FROM dbo.role_permission rp
                INNER JOIN dbo.app_role r ON rp.role_id = r.role_id
                WHERE rp.permission_id = @permission_id
                ORDER BY r.role_name";

            using (SqlDataReader reader = _db.ExecuteReader(sql, parameters))
            {
                while (reader.Read())
                {
                    roles.Add(MapRoleFromReader(reader));
                }
            }

            return roles;
        }

        /// <summary>
        /// Assigns a permission to a role.
        /// </summary>
        public bool AssignPermissionToRole(int roleId, int permissionId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@role_id", roleId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@permission_id", permissionId, SqlDbType.Int)
            };

            string sql = @"
                IF NOT EXISTS (SELECT 1 FROM dbo.role_permission WHERE role_id = @role_id AND permission_id = @permission_id)
                BEGIN
                    INSERT INTO dbo.role_permission (role_id, permission_id)
                    VALUES (@role_id, @permission_id)
                END";

            int rowsAffected = _db.ExecuteNonQuery(sql, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Removes a permission from a role.
        /// </summary>
        public bool RemovePermissionFromRole(int roleId, int permissionId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@role_id", roleId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@permission_id", permissionId, SqlDbType.Int)
            };

            string sql = @"
                DELETE FROM dbo.role_permission
                WHERE role_id = @role_id AND permission_id = @permission_id";

            int rowsAffected = _db.ExecuteNonQuery(sql, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Replaces all permissions for a role (clears and re-adds).
        /// </summary>
        public bool SetRolePermissions(int roleId, List<int> permissionIds)
        {
            List<int> uniquePermissionIds = permissionIds == null
                ? new List<int>()
                : new List<int>(new HashSet<int>(permissionIds));
            _db.BeginTransaction(IsolationLevel.Serializable);
            try
            {
                _db.ExecuteNonQuery("DELETE FROM dbo.role_permission WHERE role_id = @role_id",
                    new SqlParameter[] { DatabaseHelper.CreateParameter("@role_id", roleId, SqlDbType.Int) });

                foreach (int permissionId in uniquePermissionIds)
                {
                    _db.ExecuteNonQuery(@"
                        INSERT INTO dbo.role_permission (role_id, permission_id)
                        VALUES (@role_id, @permission_id)",
                        new SqlParameter[]
                        {
                            DatabaseHelper.CreateParameter("@role_id", roleId, SqlDbType.Int),
                            DatabaseHelper.CreateParameter("@permission_id", permissionId, SqlDbType.Int)
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

        #endregion

        #region Helper Methods

        private Role MapRoleFromReader(SqlDataReader reader)
        {
            return new Role
            {
                RoleId = reader.GetInt32(reader.GetOrdinal("role_id")),
                RoleName = reader.GetString(reader.GetOrdinal("role_name")),
                Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                UserCount = reader.GetInt32(reader.GetOrdinal("user_count")),
                PermissionCount = reader.GetInt32(reader.GetOrdinal("permission_count"))
            };
        }

        private Permission MapPermissionFromReader(SqlDataReader reader)
        {
            return new Permission
            {
                PermissionId = reader.GetInt32(reader.GetOrdinal("permission_id")),
                PermissionName = reader.GetString(reader.GetOrdinal("permission_name")),
                Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description"))
            };
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
