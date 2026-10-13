using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Web;
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
        private static readonly object LoginAttemptLock = new object();
        private static readonly Dictionary<string, LoginAttempt> LoginAttempts = new Dictionary<string, LoginAttempt>(StringComparer.Ordinal);
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

                string attemptKey = GetLoginAttemptKey(login);
                if (IsLoginLocked(attemptKey))
                    throw new ServiceException("Invalid login or password.", "Неверный логин или пароль.");

                User user = _userRepo.AuthenticateUser(login.Trim(), password);
                if (user == null)
                {
                    RecordLoginFailure(attemptKey);
                    throw new ServiceException("Invalid login or password.", "Неверный логин или пароль.");
                }

                ClearLoginFailures(attemptKey);

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


        public int SaveUserAdministration(User user, string password, List<int> roleIds, string assignedBy, int actorUserId)
        {
            try
            {
                EnsureAdministrator(actorUserId);
                if (user == null) throw new ServiceException("A user record is required.", "Не указаны данные пользователя.");
                bool isNew = user.UserId <= 0;
                if (!isNew)
                {
                    User existing = _userRepo.GetUserById(user.UserId);
                    if (existing == null) throw new ServiceException("The user no longer exists.", "Пользователь больше не существует.");
                    user.Login = existing.Login;
                }
                else user.Login = (user.Login ?? string.Empty).Trim();

                user.Email = string.IsNullOrWhiteSpace(user.Email) ? null : user.Email.Trim();
                user.PhoneNumber = string.IsNullOrWhiteSpace(user.PhoneNumber) ? null : user.PhoneNumber.Trim();
                ValidateUser(user);
                if (isNew || !string.IsNullOrEmpty(password)) ValidatePassword(password);

                if (user.EmployeeId.HasValue)
                {
                    foreach (User existing in _userRepo.GetAllUsers())
                        if (existing.UserId != user.UserId && existing.EmployeeId.HasValue &&
                            existing.EmployeeId.Value == user.EmployeeId.Value)
                            throw new ServiceException(
                                "This employee is already linked to user '" + existing.Login + "'.",
                                "Этот сотрудник уже связан с учётной записью '" + existing.Login + "'.");
                }
                return _userRepo.SaveManagedUser(user, password, roleIds, assignedBy, actorUserId);
            }
            catch (ServiceException) { throw; }
            catch (InvalidOperationException ex) { throw new ServiceException(ex.Message, ex.Message, ex); }
            catch (Exception ex) { throw new ServiceException("Failed to save the user.", "Не удалось сохранить пользователя.", ex); }
        }

        private static void EnsureAdministrator(int actorUserId)
        {
            int? currentId = BRU.WEBFORMS.ASPNET.APP.AuthContext.CurrentUserId;
            if (!BRU.WEBFORMS.ASPNET.APP.AuthContext.IsInRole("administrator") || !currentId.HasValue || actorUserId != currentId.Value)
                throw new ServiceException(
                    "Only the authenticated administrator can perform this operation.",
                    "Выполнять эту операцию может только авторизованный администратор.");
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
                Role existing = _roleRepo.GetRoleByName(role.RoleName);
                if (existing != null && existing.RoleId != role.RoleId)
                    throw new ServiceException("A role with this name already exists.", "Роль с таким названием уже существует.");
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


        public List<UserRole> GetUserRoleAssignments(int userId)
        {
            try { return _userRepo.GetUserRoleAssignments(userId); }
            catch (Exception ex) { throw new ServiceException("Failed to retrieve role assignments.", "Не удалось получить назначения ролей.", ex); }
        }

        public int SaveRoleAdministration(Role role, List<int> permissionIds, int actorUserId)
        {
            try
            {
                EnsureAdministrator(actorUserId);
                ValidateRole(role);
                role.RoleName = role.RoleName.Trim().ToLowerInvariant();
                Role existing = role.RoleId > 0 ? _roleRepo.GetRoleById(role.RoleId) : null;
                if (role.RoleId > 0 && existing == null)
                    throw new ServiceException("The role no longer exists.", "Роль больше не существует.");

                bool isSystemAdmin = existing != null && string.Equals(existing.RoleName, "administrator", StringComparison.OrdinalIgnoreCase);
                if (isSystemAdmin && (!role.IsActive || role.RoleName != "administrator"))
                    throw new ServiceException("The built-in administrator role cannot be renamed or deactivated.", "Системную роль администратора нельзя переименовать или отключить.");
                if (!isSystemAdmin && string.Equals(role.RoleName, "administrator", StringComparison.OrdinalIgnoreCase))
                    throw new ServiceException("The built-in administrator role name is reserved.", "Имя системной роли администратора зарезервировано.");

                Role duplicate = _roleRepo.GetRoleByName(role.RoleName);
                if (duplicate != null && duplicate.RoleId != role.RoleId)
                    throw new ServiceException("A role with this name already exists.", "Роль с таким названием уже существует.");
                return _roleRepo.SaveRoleAdministration(role, permissionIds);
            }
            catch (ServiceException) { throw; }
            catch (InvalidOperationException ex) { throw new ServiceException(ex.Message, ex.Message, ex); }
            catch (Exception ex) { throw new ServiceException("Failed to save the role.", "Не удалось сохранить роль.", ex); }
        }

        /// <summary>
        /// Deletes a role.
        /// </summary>
        public bool DeleteRole(int roleId)
        {
            try
            {
                if (!BRU.WEBFORMS.ASPNET.APP.AuthContext.CurrentUserId.HasValue ||
                    !BRU.WEBFORMS.ASPNET.APP.AuthContext.IsInRole("administrator"))
                    throw new ServiceException("Administrator access is required.", "Требуются права администратора.");
                Role role = _roleRepo.GetRoleById(roleId);
                if (role == null)
                    throw new ServiceException("The role no longer exists.", "Роль больше не существует.");
                if (string.Equals(role.RoleName, "administrator", StringComparison.OrdinalIgnoreCase))
                    throw new ServiceException("The built-in administrator role cannot be deleted.", "Системную роль администратора нельзя удалить.");
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

        public List<DatabaseSecurityGrant> GetDatabaseSecuritySnapshot()
        {
            try
            {
                return _roleRepo.GetDatabaseSecuritySnapshot();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Failed to retrieve SQL Server security details.", "Ошибка при получении сведений о безопасности SQL Server.", ex);
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

        public bool SetUserRoles(int userId, List<int> roleIds, string assignedBy)
        {
            try
            {
                return _userRepo.SetUserRoles(userId, roleIds, assignedBy);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Failed to update user roles.", "Ошибка при обновлении ролей пользователя.", ex);
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
            if (!Regex.IsMatch(user.Login, @"^[A-Za-z0-9][A-Za-z0-9._-]{2,79}$"))
                throw new ServiceException("Login contains unsupported characters.", "Логин содержит недопустимые символы.");

            if (!string.IsNullOrEmpty(user.Email))
            {
                if (user.Email.Length > 150)
                    throw new ServiceException("Email cannot exceed 150 characters.", "Email не может превышать 150 символов.");
                try
                {
                    MailAddress address = new MailAddress(user.Email);
                    if (!string.Equals(address.Address, user.Email, StringComparison.OrdinalIgnoreCase)) throw new FormatException();
                }
                catch { throw new ServiceException("Enter a valid email address.", "Укажите корректный адрес электронной почты."); }
            }
            if (!string.IsNullOrEmpty(user.PhoneNumber))
            {
                if (user.PhoneNumber.Length > 30)
                    throw new ServiceException("Phone number cannot exceed 30 characters.", "Телефон не может превышать 30 символов.");
                if (!Regex.IsMatch(user.PhoneNumber, @"^[0-9+() .-]+$"))
                    throw new ServiceException("Phone number contains unsupported characters.", "Телефон содержит недопустимые символы.");
            }
        }

        private void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ServiceException("Password is required.", "Пароль обязателен.");

            if (password.Length < 12)
                throw new ServiceException("Password must be at least 12 characters.", "Пароль должен содержать минимум 12 символов.");

            if (password.Length > 100)
                throw new ServiceException("Password cannot exceed 100 characters.", "Пароль не может превышать 100 символов.");

            bool hasUpper = false;
            bool hasLower = false;
            bool hasDigit = false;
            bool hasSymbol = false;
            foreach (char character in password)
            {
                hasUpper |= char.IsUpper(character);
                hasLower |= char.IsLower(character);
                hasDigit |= char.IsDigit(character);
                hasSymbol |= !char.IsLetterOrDigit(character);
            }
            if (!hasUpper || !hasLower || !hasDigit || !hasSymbol)
                throw new ServiceException("Password must contain uppercase and lowercase letters, a number, and a symbol.",
                    "Пароль должен содержать заглавную и строчную буквы, цифру и символ.");
        }

        private static string GetLoginAttemptKey(string login)
        {
            string address = HttpContext.Current == null ? "unknown" : HttpContext.Current.Request.UserHostAddress ?? "unknown";
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] digest = sha256.ComputeHash(Encoding.UTF8.GetBytes(login.Trim().ToUpperInvariant() + "|" + address));
                return Convert.ToBase64String(digest);
            }
        }

        private static bool IsLoginLocked(string key)
        {
            lock (LoginAttemptLock)
            {
                LoginAttempt attempt;
                if (!LoginAttempts.TryGetValue(key, out attempt)) return false;
                if (attempt.LockedUntilUtc <= DateTime.UtcNow)
                {
                    LoginAttempts.Remove(key);
                    return false;
                }
                return true;
            }
        }

        private static void RecordLoginFailure(string key)
        {
            lock (LoginAttemptLock)
            {
                DateTime now = DateTime.UtcNow;
                if (LoginAttempts.Count > 10000)
                {
                    List<string> expired = new List<string>();
                    foreach (KeyValuePair<string, LoginAttempt> entry in LoginAttempts)
                        if (entry.Value.LastAttemptUtc < now.AddHours(-1)) expired.Add(entry.Key);
                    foreach (string expiredKey in expired) LoginAttempts.Remove(expiredKey);
                }

                LoginAttempt attempt;
                if (!LoginAttempts.TryGetValue(key, out attempt) || attempt.LastAttemptUtc < now.AddMinutes(-15))
                    attempt = new LoginAttempt();
                attempt.Failures++;
                attempt.LastAttemptUtc = now;
                if (attempt.Failures >= 5) attempt.LockedUntilUtc = now.AddMinutes(15);
                LoginAttempts[key] = attempt;
            }
        }

        private static void ClearLoginFailures(string key)
        {
            lock (LoginAttemptLock)
                LoginAttempts.Remove(key);
        }

        private sealed class LoginAttempt
        {
            public int Failures { get; set; }
            public DateTime LastAttemptUtc { get; set; }
            public DateTime LockedUntilUtc { get; set; }
        }

        private void ValidateRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            if (string.IsNullOrWhiteSpace(role.RoleName))
                throw new ServiceException("Role name is required.", "Название роли обязательно.");
            role.RoleName = role.RoleName.Trim();
            if (role.RoleName.Length < 3 || role.RoleName.Length > 50)
                throw new ServiceException("Role name must contain between 3 and 50 characters.", "Название роли должно содержать от 3 до 50 символов.");
            if (!Regex.IsMatch(role.RoleName, @"^[A-Za-z][A-Za-z0-9._-]{2,49}$"))
                throw new ServiceException("Role name contains unsupported characters.", "Имя роли содержит недопустимые символы.");

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
