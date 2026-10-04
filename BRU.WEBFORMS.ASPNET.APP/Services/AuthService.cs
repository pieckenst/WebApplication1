using System;
using System.Collections.Generic;
using BRU.WEBFORMS.ASPNET.APP.DataAccess;
using BRU.WEBFORMS.ASPNET.APP.Models;

namespace BRU.WEBFORMS.ASPNET.APP.Services
{
    /// <summary>
    /// Enterprise-grade authentication service.
    /// Wraps UserRepository and RoleRepository with business logic validation.
    /// </summary>
    public class AuthService : IDisposable
    {
        private UserRepository _userRepo;
        private RoleRepository _roleRepo;

        public AuthService()
        {
            _userRepo = new UserRepository();
            _roleRepo = new RoleRepository();
        }

        #region Authentication

        /// <summary>
        /// Authenticates a user and returns user with roles and permissions.
        /// </summary>
        public UserWithRolesAndPermissions AuthenticateUser(string login, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(login))
                    throw new ServiceException("Login cannot be empty.", "Логин не может быть пустым.");

                if (string.IsNullOrWhiteSpace(password))
                    throw new ServiceException("Password cannot be empty.", "Пароль не может быть пустым.");

                User user = _userRepo.AuthenticateUser(login, password);
                if (user == null)
                {
                    throw new ServiceException("Invalid login or password.", "Неверный логин или пароль.");
                }

                if (!user.IsActive)
                {
                    throw new ServiceException("User account is inactive.", "Учетная запись пользователя неактивна.");
                }

                // Get roles and permissions
                List<string> roles = _userRepo.GetUserRoles(user.UserId);
                List<string> permissions = _userRepo.GetUserPermissions(user.UserId);

                return new UserWithRolesAndPermissions
                {
                    User = user,
                    Roles = roles,
                    Permissions = permissions
                };
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Authentication failed.", "Ошибка аутентификации.", ex);
            }
        }

        #endregion

        #region User Management

        /// <summary>
        /// Gets all users.
        /// </summary>
        public List<User> GetAllUsers()
        {
            try
            {
                return _userRepo.GetAllUsers();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Failed to retrieve users.", "Ошибка при получении пользователей.", ex);
            }
        }

        /// <summary>
        /// Gets a user by ID.
        /// </summary>
        public User GetUserById(int userId)
        {
            try
            {
                return _userRepo.GetUserById(userId);
            }
            catch (Exception ex)
            {
                throw new ServiceException($"Failed to retrieve user {userId}.", $"Ошибка при получении пользователя {userId}.", ex);
            }
        }

        /// <summary>
        /// Gets a user by login.
        /// </summary>
        public User GetUserByLogin(string login)
        {
            try
            {
                return _userRepo.GetUserByLogin(login);
            }
            catch (Exception ex)
            {
                throw new ServiceException($"Failed to retrieve user '{login}'.", $"Ошибка при получении пользователя '{login}'.", ex);
            }
        }

        /// <summary>
        /// Creates a new user with validation.
        /// </summary>
        public int CreateUser(User user, string password)
        {
            try
            {
                // Validation
                ValidateUser(user);
                ValidatePassword(password);

                // Check for duplicate login
                User existing = _userRepo.GetUserByLogin(user.Login);
                if (existing != null)
                {
                    throw new ServiceException($"User with login '{user.Login}' already exists.", $"Пользователь с логином '{user.Login}' уже существует.");
                }

                // Check if employee is already linked to another user
                if (user.EmployeeId.HasValue)
                {
                    List<User> users = _userRepo.GetAllUsers();
                    foreach (User u in users)
                    {
                        if (u.EmployeeId.HasValue && u.EmployeeId.Value == user.EmployeeId.Value)
                        {
                            throw new ServiceException($"Employee is already linked to user '{u.Login}'.", $"Сотрудник уже привязан к пользователю '{u.Login}'.");
                        }
                    }
                }

                return _userRepo.InsertUser(user, password);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Failed to create user.", "Ошибка при создании пользователя.", ex);
            }
        }

        /// <summary>
        /// Updates an existing user with validation.
        /// </summary>
        public bool UpdateUser(User user)
        {
            try
            {
                ValidateUser(user);

                // Check if employee is already linked to another user
                if (user.EmployeeId.HasValue)
                {
                    List<User> users = _userRepo.GetAllUsers();
                    foreach (User u in users)
                    {
                        if (u.UserId != user.UserId && u.EmployeeId.HasValue && u.EmployeeId.Value == user.EmployeeId.Value)
                        {
                            throw new ServiceException($"Employee is already linked to user '{u.Login}'.", $"Сотрудник уже привязан к пользователю '{u.Login}'.");
                        }
                    }
                }

                return _userRepo.UpdateUser(user);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Failed to update user.", "Ошибка при обновлении пользователя.", ex);
            }
        }

        /// <summary>
        /// Changes a user's password with validation.
        /// </summary>
        public bool ChangePassword(int userId, string newPassword)
        {
            try
            {
                ValidatePassword(newPassword);
                return _userRepo.ChangePassword(userId, newPassword);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Failed to change password.", "Ошибка при смене пароля.", ex);
            }
        }

        /// <summary>
        /// Deletes (deactivates) a user.
        /// </summary>
        public bool DeleteUser(int userId)
        {
            try
            {
                return _userRepo.DeleteUser(userId);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Failed to delete user.", "Ошибка при удалении пользователя.", ex);
            }
        }

        #endregion

        #region Role Management

        /// <summary>
        /// Gets all roles.
        /// </summary>
        public List<Role> GetAllRoles()
        {
            try
            {
                return _roleRepo.GetAllRoles();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Failed to retrieve roles.", "Ошибка при получении ролей.", ex);
            }
        }

        /// <summary>
        /// Gets a role by ID.
        /// </summary>
        public Role GetRoleById(int roleId)
        {
            try
            {
                return _roleRepo.GetRoleById(roleId);
            }
            catch (Exception ex)
            {
                throw new ServiceException($"Failed to retrieve role {roleId}.", $"Ошибка при получении роли {roleId}.", ex);
            }
        }

        /// <summary>
        /// Creates a new role with validation.
        /// </summary>
        public int CreateRole(Role role)
        {
            try
            {
                ValidateRole(role);

                // Check for duplicate name
                Role existing = _roleRepo.GetRoleByName(role.RoleName);
                if (existing != null)
                {
                    throw new ServiceException($"Role '{role.RoleName}' already exists.", $"Роль '{role.RoleName}' уже существует.");
                }

                return _roleRepo.InsertRole(role);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Failed to create role.", "Ошибка при создании роли.", ex);
            }
        }

        /// <summary>
        /// Updates an existing role with validation.
        /// </summary>
        public bool UpdateRole(Role role)
        {
            try
            {
                ValidateRole(role);
                return _roleRepo.UpdateRole(role);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Failed to update role.", "Ошибка при обновлении роли.", ex);
            }
        }

        /// <summary>
        /// Deletes a role.
        /// </summary>
        public bool DeleteRole(int roleId)
        {
            try
            {
                return _roleRepo.DeleteRole(roleId);
            }
            catch (InvalidOperationException ex)
            {
                throw new ServiceException(ex.Message, "Невозможно удалить роль, назначенную пользователям.");
            }
            catch (Exception ex)
            {
                throw new ServiceException("Failed to delete role.", "Ошибка при удалении роли.", ex);
            }
        }

        #endregion

        #region Permission Management

        /// <summary>
        /// Gets all permissions.
        /// </summary>
        public List<Permission> GetAllPermissions()
        {
            try
            {
                return _roleRepo.GetAllPermissions();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Failed to retrieve permissions.", "Ошибка при получении разрешений.", ex);
            }
        }

        /// <summary>
        /// Gets permissions for a specific role.
        /// </summary>
        public List<Permission> GetRolePermissions(int roleId)
        {
            try
            {
                return _roleRepo.GetRolePermissions(roleId);
            }
            catch (Exception ex)
            {
                throw new ServiceException($"Failed to retrieve permissions for role {roleId}.", $"Ошибка при получении разрешений для роли {roleId}.", ex);
            }
        }

        /// <summary>
        /// Creates a new permission with validation.
        /// </summary>
        public int CreatePermission(Permission permission)
        {
            try
            {
                ValidatePermission(permission);

                // Check for duplicate name
                Permission existing = _roleRepo.GetPermissionByName(permission.PermissionName);
                if (existing != null)
                {
                    throw new ServiceException($"Permission '{permission.PermissionName}' already exists.", $"Разрешение '{permission.PermissionName}' уже существует.");
                }

                return _roleRepo.InsertPermission(permission);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Failed to create permission.", "Ошибка при создании разрешения.", ex);
            }
        }

        /// <summary>
        /// Updates an existing permission with validation.
        /// </summary>
        public bool UpdatePermission(Permission permission)
        {
            try
            {
                ValidatePermission(permission);
                return _roleRepo.UpdatePermission(permission);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Failed to update permission.", "Ошибка при обновлении разрешения.", ex);
            }
        }

        /// <summary>
        /// Deletes a permission.
        /// </summary>
        public bool DeletePermission(int permissionId)
        {
            try
            {
                return _roleRepo.DeletePermission(permissionId);
            }
            catch (InvalidOperationException ex)
            {
                throw new ServiceException(ex.Message, "Невозможно удалить разрешение, назначенное ролям.");
            }
            catch (Exception ex)
            {
                throw new ServiceException("Failed to delete permission.", "Ошибка при удалении разрешения.", ex);
            }
        }

        #endregion

        #region User-Role Management

        /// <summary>
        /// Gets roles for a specific user.
        /// </summary>
        public List<string> GetUserRoles(int userId)
        {
            try
            {
                return _userRepo.GetUserRoles(userId);
            }
            catch (Exception ex)
            {
                throw new ServiceException($"Failed to retrieve roles for user {userId}.", $"Ошибка при получении ролей для пользователя {userId}.", ex);
            }
        }

        /// <summary>
        /// Assigns a role to a user.
        /// </summary>
        public bool AssignRoleToUser(int userId, int roleId, string assignedBy = null)
        {
            try
            {
                return _userRepo.AssignRoleToUser(userId, roleId, assignedBy);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Failed to assign role to user.", "Ошибка при назначении роли пользователю.", ex);
            }
        }

        /// <summary>
        /// Removes a role from a user.
        /// </summary>
        public bool RemoveRoleFromUser(int userId, int roleId)
        {
            try
            {
                return _userRepo.RemoveRoleFromUser(userId, roleId);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Failed to remove role from user.", "Ошибка при удалении роли у пользователя.", ex);
            }
        }

        #endregion

        #region Role-Permission Management

        /// <summary>
        /// Assigns a permission to a role.
        /// </summary>
        public bool AssignPermissionToRole(int roleId, int permissionId)
        {
            try
            {
                return _roleRepo.AssignPermissionToRole(roleId, permissionId);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Failed to assign permission to role.", "Ошибка при назначении разрешения роли.", ex);
            }
        }

        /// <summary>
        /// Removes a permission from a role.
        /// </summary>
        public bool RemovePermissionFromRole(int roleId, int permissionId)
        {
            try
            {
                return _roleRepo.RemovePermissionFromRole(roleId, permissionId);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Failed to remove permission from role.", "Ошибка при удалении разрешения у роли.", ex);
            }
        }

        /// <summary>
        /// Sets all permissions for a role (replaces existing).
        /// </summary>
        public bool SetRolePermissions(int roleId, List<int> permissionIds)
        {
            try
            {
                return _roleRepo.SetRolePermissions(roleId, permissionIds);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Failed to set role permissions.", "Ошибка при установке разрешений роли.", ex);
            }
        }

        #endregion

        #region Validation

        private void ValidateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(user.Login))
                throw new ServiceException("Login is required.", "Логин обязателен.");

            if (user.Login.Length < 3)
                throw new ServiceException("Login must be at least 3 characters.", "Логин должен содержать минимум 3 символа.");

            if (user.Login.Length > 80)
                throw new ServiceException("Login cannot exceed 80 characters.", "Логин не может превышать 80 символов.");

            if (!string.IsNullOrEmpty(user.Email) && user.Email.Length > 150)
                throw new ServiceException("Email cannot exceed 150 characters.", "Email не может превышать 150 символов.");

            if (!string.IsNullOrEmpty(user.PhoneNumber) && user.PhoneNumber.Length > 30)
                throw new ServiceException("Phone number cannot exceed 30 characters.", "Телефон не может превышать 30 символов.");
        }

        private void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ServiceException("Password is required.", "Пароль обязателен.");

            if (password.Length < 6)
                throw new ServiceException("Password must be at least 6 characters.", "Пароль должен содержать минимум 6 символов.");

            if (password.Length > 100)
                throw new ServiceException("Password cannot exceed 100 characters.", "Пароль не может превышать 100 символов.");
        }

        private void ValidateRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            if (string.IsNullOrWhiteSpace(role.RoleName))
                throw new ServiceException("Role name is required.", "Название роли обязательно.");

            if (role.RoleName.Length > 50)
                throw new ServiceException("Role name cannot exceed 50 characters.", "Название роли не может превышать 50 символов.");

            if (!string.IsNullOrEmpty(role.Description) && role.Description.Length > 300)
                throw new ServiceException("Role description cannot exceed 300 characters.", "Описание роли не может превышать 300 символов.");
        }

        private void ValidatePermission(Permission permission)
        {
            if (permission == null)
                throw new ArgumentNullException(nameof(permission));

            if (string.IsNullOrWhiteSpace(permission.PermissionName))
                throw new ServiceException("Permission name is required.", "Название разрешения обязательно.");

            if (permission.PermissionName.Length > 100)
                throw new ServiceException("Permission name cannot exceed 100 characters.", "Название разрешения не может превышать 100 символов.");

            if (!string.IsNullOrEmpty(permission.Description) && permission.Description.Length > 300)
                throw new ServiceException("Permission description cannot exceed 300 characters.", "Описание разрешения не может превышать 300 символов.");
        }

        #endregion

        public void Dispose()
        {
            if (_userRepo != null)
            {
                _userRepo.Dispose();
                _userRepo = null;
            }

            if (_roleRepo != null)
            {
                _roleRepo.Dispose();
                _roleRepo = null;
            }
        }
    }
}
