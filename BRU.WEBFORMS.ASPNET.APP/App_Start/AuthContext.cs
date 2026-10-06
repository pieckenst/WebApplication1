using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using BRU.WEBFORMS.ASPNET.APP.DataAccess;
using BRU.WEBFORMS.ASPNET.APP.Models;

namespace BRU.WEBFORMS.ASPNET.APP
{
    /// <summary>
    /// Enterprise-grade authentication and authorization context.
    /// Manages user sessions, roles, permissions, and security operations.
    /// Integrates with FormsAuthentication and Session state.
    /// Thread-safe and designed for high-concurrency scenarios.
    /// </summary>
    public static class AuthContext
    {
        #region Session Keys

        private const string SESSION_KEY_USER_ID = "AuthContext_UserId";
        private const string SESSION_KEY_USERNAME = "AuthContext_Username";
        private const string SESSION_KEY_EMPLOYEE_ID = "AuthContext_EmployeeId";
        private const string SESSION_KEY_ROLES = "AuthContext_Roles";
        private const string SESSION_KEY_PERMISSIONS = "AuthContext_Permissions";
        private const string SESSION_KEY_LOGIN_TIME = "AuthContext_LoginTime";
        private const string SESSION_KEY_LAST_ACTIVITY = "AuthContext_LastActivity";
        private const string SESSION_KEY_SESSION_ID = "AuthContext_SessionId";

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets whether the current user is authenticated.
        /// </summary>
        public static bool IsAuthenticated
        {
            get
            {
                return HttpContext.Current?.User?.Identity?.IsAuthenticated ?? false;
            }
        }

        /// <summary>
        /// Gets the current user's ID from session.
        /// Returns null if not authenticated.
        /// </summary>
        public static int? CurrentUserId
        {
            get
            {
                if (!IsAuthenticated)
                    return null;

                object userId = GetSessionValue(SESSION_KEY_USER_ID);
                if (userId != null && userId is int)
                    return (int)userId;

                FormsIdentity identity = HttpContext.Current.User.Identity as FormsIdentity;
                int ticketUserId;
                if (identity != null && int.TryParse(identity.Ticket.UserData, out ticketUserId))
                    return ticketUserId;

                return null;
            }
        }

        /// <summary>
        /// Gets the current username.
        /// </summary>
        public static string CurrentUsername
        {
            get
            {
                if (!IsAuthenticated)
                    return null;

                return HttpContext.Current?.User?.Identity?.Name;
            }
        }

        /// <summary>
        /// Gets the current user's employee ID (if linked to an employee).
        /// </summary>
        public static int? CurrentEmployeeId
        {
            get
            {
                if (!IsAuthenticated)
                    return null;

                object employeeId = GetSessionValue(SESSION_KEY_EMPLOYEE_ID);
                if (employeeId != null && employeeId is int)
                    return (int)employeeId;

                return null;
            }
        }

        /// <summary>
        /// Gets the time when the user logged in.
        /// </summary>
        public static DateTime? LoginTime
        {
            get
            {
                object loginTime = GetSessionValue(SESSION_KEY_LOGIN_TIME);
                return loginTime as DateTime?;
            }
        }

        /// <summary>
        /// Gets the last activity time.
        /// </summary>
        public static DateTime? LastActivityTime
        {
            get
            {
                object lastActivity = GetSessionValue(SESSION_KEY_LAST_ACTIVITY);
                return lastActivity as DateTime?;
            }
        }

        /// <summary>
        /// Gets the session duration in minutes.
        /// </summary>
        public static int SessionDurationMinutes
        {
            get
            {
                if (!LoginTime.HasValue)
                    return 0;

                return (int)(DateTime.Now - LoginTime.Value).TotalMinutes;
            }
        }

        /// <summary>
        /// Gets the client IP address.
        /// </summary>
        public static string ClientIpAddress
        {
            get
            {
                if (HttpContext.Current == null)
                    return null;

                string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(ip))
                    ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                return ip;
            }
        }

        #endregion

        #region Authentication Methods

        public static bool RefreshAuthorization()
        {
            if (!IsAuthenticated || HttpContext.Current == null || HttpContext.Current.Session == null)
                return false;

            int? userId = CurrentUserId;
            if (!userId.HasValue)
                return false;

            User user;
            List<string> roles;
            List<string> permissions;
            using (UserRepository repository = new UserRepository())
            {
                user = repository.GetUserById(userId.Value);
                if (user == null || !user.IsActive)
                    return false;

                roles = repository.GetUserRoles(user.UserId);
                permissions = repository.GetUserPermissions(user.UserId);
            }

            SetSessionValue(SESSION_KEY_USER_ID, user.UserId);
            SetSessionValue(SESSION_KEY_USERNAME, user.Login);
            SetSessionValue(SESSION_KEY_EMPLOYEE_ID, user.EmployeeId);
            SetSessionValue(SESSION_KEY_ROLES, roles);
            SetSessionValue(SESSION_KEY_PERMISSIONS, permissions);

            IPrincipal currentPrincipal = HttpContext.Current.User;
            GenericPrincipal principal = new GenericPrincipal(currentPrincipal.Identity, roles.ToArray());
            HttpContext.Current.User = principal;
            Thread.CurrentPrincipal = principal;
            return true;
        }

        /// <summary>
        /// Authenticates a user and creates a session.
        /// </summary>
        /// <param name="user">The user object to authenticate</param>
        /// <param name="roles">User's roles</param>
        /// <param name="permissions">User's permissions</param>
        /// <param name="rememberMe">Whether to create a persistent cookie</param>
        /// <param name="employeeId">Optional linked employee ID</param>
        public static void SignIn(User user, List<string> roles, List<string> permissions, 
            bool rememberMe = false, int? employeeId = null)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (HttpContext.Current == null)
                throw new InvalidOperationException("HttpContext is not available");

            if (HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
                SessionIDManager sessionIdManager = new SessionIDManager();
                string sessionId = sessionIdManager.CreateSessionID(HttpContext.Current);
                bool redirected;
                bool cookieAdded;
                sessionIdManager.SaveSessionID(HttpContext.Current, sessionId, out redirected, out cookieAdded);
            }

            // Create Forms Authentication ticket
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                version: 1,
                name: user.Login,
                issueDate: DateTime.Now,
                expiration: DateTime.Now.AddMinutes(rememberMe ? 43200 : 480), // 30 days or 8 hours
                isPersistent: rememberMe,
                userData: user.UserId.ToString(),
                cookiePath: FormsAuthentication.FormsCookiePath
            );

            // Encrypt the ticket
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);

            // Create the cookie
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL,
                Path = FormsAuthentication.FormsCookiePath
            };

            if (rememberMe)
                authCookie.Expires = DateTime.Now.AddDays(30);

            HttpContext.Current.Response.Cookies.Add(authCookie);

            // Store user data in session
            SetSessionValue(SESSION_KEY_USER_ID, user.UserId);
            SetSessionValue(SESSION_KEY_USERNAME, user.Login);
            SetSessionValue(SESSION_KEY_EMPLOYEE_ID, employeeId);
            SetSessionValue(SESSION_KEY_ROLES, roles ?? new List<string>());
            SetSessionValue(SESSION_KEY_PERMISSIONS, permissions ?? new List<string>());
            SetSessionValue(SESSION_KEY_LOGIN_TIME, DateTime.Now);
            SetSessionValue(SESSION_KEY_LAST_ACTIVITY, DateTime.Now);
            SetSessionValue(SESSION_KEY_SESSION_ID, Guid.NewGuid().ToString("N"));

            // Log the login
            LogAuthenticationEvent($"User '{user.Login}' signed in successfully from IP {ClientIpAddress}");
        }

        /// <summary>
        /// Signs out the current user and clears the session.
        /// </summary>
        public static void SignOut()
        {
            if (HttpContext.Current == null)
                return;

            string username = CurrentUsername;

            // Clear Forms Authentication
            FormsAuthentication.SignOut();

            // Clear session
            if (HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
            }

            // Remove authentication cookie
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, string.Empty)
            {
                Expires = DateTime.Now.AddYears(-1),
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL,
                Path = FormsAuthentication.FormsCookiePath
            };
            HttpContext.Current.Response.Cookies.Add(authCookie);

            // Log the logout
            LogAuthenticationEvent($"User '{username ?? "Unknown"}' signed out from IP {ClientIpAddress}");
        }

        /// <summary>
        /// Updates the last activity timestamp.
        /// </summary>
        public static void UpdateActivity()
        {
            if (IsAuthenticated)
            {
                SetSessionValue(SESSION_KEY_LAST_ACTIVITY, DateTime.Now);
            }
        }

        /// <summary>
        /// Checks if the session has timed out based on inactivity.
        /// </summary>
        /// <param name="timeoutMinutes">Timeout in minutes</param>
        /// <returns>True if timed out</returns>
        public static bool IsSessionTimedOut(int timeoutMinutes = 30)
        {
            if (!IsAuthenticated)
                return true;

            DateTime? lastActivity = LastActivityTime;
            if (!lastActivity.HasValue)
                return true;

            return (DateTime.Now - lastActivity.Value).TotalMinutes > timeoutMinutes;
        }

        #endregion

        #region Role-Based Authorization

        /// <summary>
        /// Checks if the current user has the specified role.
        /// </summary>
        /// <param name="roleName">Role name to check</param>
        /// <returns>True if user has the role</returns>
        public static bool IsInRole(string roleName)
        {
            if (!IsAuthenticated || string.IsNullOrWhiteSpace(roleName))
                return false;

            List<string> roles = GetSessionValue(SESSION_KEY_ROLES) as List<string>;
            if (roles == null || roles.Count == 0)
                return false;

            return roles.Contains(roleName, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Checks if the current user has any of the specified roles.
        /// </summary>
        /// <param name="roleNames">Role names to check</param>
        /// <returns>True if user has at least one role</returns>
        public static bool IsInAnyRole(params string[] roleNames)
        {
            if (!IsAuthenticated || roleNames == null || roleNames.Length == 0)
                return false;

            return roleNames.Any(role => IsInRole(role));
        }

        /// <summary>
        /// Checks if the current user has all of the specified roles.
        /// </summary>
        /// <param name="roleNames">Role names to check</param>
        /// <returns>True if user has all roles</returns>
        public static bool IsInAllRoles(params string[] roleNames)
        {
            if (!IsAuthenticated || roleNames == null || roleNames.Length == 0)
                return false;

            return roleNames.All(role => IsInRole(role));
        }

        /// <summary>
        /// Gets all roles for the current user.
        /// </summary>
        /// <returns>List of role names</returns>
        public static List<string> GetUserRoles()
        {
            if (!IsAuthenticated)
                return new List<string>();

            List<string> roles = GetSessionValue(SESSION_KEY_ROLES) as List<string>;
            return roles ?? new List<string>();
        }

        /// <summary>
        /// Requires that the user has the specified role.
        /// Throws UnauthorizedAccessException if not.
        /// </summary>
        /// <param name="roleName">Required role name</param>
        public static void RequireRole(string roleName)
        {
            if (!IsInRole(roleName))
            {
                LogAuthorizationFailure($"Role '{roleName}' required but user '{CurrentUsername}' does not have it");
                throw new UnauthorizedAccessException($"Access denied. Role '{roleName}' is required.");
            }
        }

        /// <summary>
        /// Requires that the user has any of the specified roles.
        /// </summary>
        /// <param name="roleNames">Required role names</param>
        public static void RequireAnyRole(params string[] roleNames)
        {
            if (!IsInAnyRole(roleNames))
            {
                string roles = string.Join(", ", roleNames);
                LogAuthorizationFailure($"One of roles [{roles}] required but user '{CurrentUsername}' has none");
                throw new UnauthorizedAccessException($"Access denied. One of the following roles is required: {roles}");
            }
        }

        #endregion

        #region Permission-Based Authorization

        /// <summary>
        /// Checks if the current user has the specified permission.
        /// </summary>
        /// <param name="permissionName">Permission name to check</param>
        /// <returns>True if user has the permission</returns>
        public static bool HasPermission(string permissionName)
        {
            if (!IsAuthenticated || string.IsNullOrWhiteSpace(permissionName))
                return false;

            List<string> permissions = GetSessionValue(SESSION_KEY_PERMISSIONS) as List<string>;
            if (permissions == null || permissions.Count == 0)
                return false;

            return permissions.Contains(permissionName, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Checks if the current user has any of the specified permissions.
        /// </summary>
        /// <param name="permissionNames">Permission names to check</param>
        /// <returns>True if user has at least one permission</returns>
        public static bool HasAnyPermission(params string[] permissionNames)
        {
            if (!IsAuthenticated || permissionNames == null || permissionNames.Length == 0)
                return false;

            return permissionNames.Any(perm => HasPermission(perm));
        }

        /// <summary>
        /// Checks if the current user has all of the specified permissions.
        /// </summary>
        /// <param name="permissionNames">Permission names to check</param>
        /// <returns>True if user has all permissions</returns>
        public static bool HasAllPermissions(params string[] permissionNames)
        {
            if (!IsAuthenticated || permissionNames == null || permissionNames.Length == 0)
                return false;

            return permissionNames.All(perm => HasPermission(perm));
        }

        /// <summary>
        /// Gets all permissions for the current user.
        /// </summary>
        /// <returns>List of permission names</returns>
        public static List<string> GetUserPermissions()
        {
            if (!IsAuthenticated)
                return new List<string>();

            List<string> permissions = GetSessionValue(SESSION_KEY_PERMISSIONS) as List<string>;
            return permissions ?? new List<string>();
        }

        /// <summary>
        /// Requires that the user has the specified permission.
        /// Throws UnauthorizedAccessException if not.
        /// </summary>
        /// <param name="permissionName">Required permission name</param>
        public static void RequirePermission(string permissionName)
        {
            if (!HasPermission(permissionName))
            {
                LogAuthorizationFailure($"Permission '{permissionName}' required but user '{CurrentUsername}' does not have it");
                throw new UnauthorizedAccessException($"Access denied. Permission '{permissionName}' is required.");
            }
        }

        /// <summary>
        /// Requires that the user has any of the specified permissions.
        /// </summary>
        /// <param name="permissionNames">Required permission names</param>
        public static void RequireAnyPermission(params string[] permissionNames)
        {
            if (!HasAnyPermission(permissionNames))
            {
                string perms = string.Join(", ", permissionNames);
                LogAuthorizationFailure($"One of permissions [{perms}] required but user '{CurrentUsername}' has none");
                throw new UnauthorizedAccessException($"Access denied. One of the following permissions is required: {perms}");
            }
        }

        /// <summary>
        /// Requires that the user has all of the specified permissions.
        /// </summary>
        /// <param name="permissionNames">Required permission names</param>
        public static void RequireAllPermissions(params string[] permissionNames)
        {
            if (!HasAllPermissions(permissionNames))
            {
                string perms = string.Join(", ", permissionNames);
                LogAuthorizationFailure($"All permissions [{perms}] required but user '{CurrentUsername}' does not have them");
                throw new UnauthorizedAccessException($"Access denied. All of the following permissions are required: {perms}");
            }
        }

        #endregion

        #region Helper Methods

        private static object GetSessionValue(string key)
        {
            if (HttpContext.Current?.Session == null)
                return null;

            try
            {
                return HttpContext.Current.Session[key];
            }
            catch
            {
                return null;
            }
        }

        private static void SetSessionValue(string key, object value)
        {
            if (HttpContext.Current?.Session == null)
                return;

            try
            {
                HttpContext.Current.Session[key] = value;
            }
            catch
            {
                // Silently fail if session is not available
            }
        }

        private static void LogAuthenticationEvent(string message)
        {
            System.Diagnostics.Trace.TraceInformation($"[AUTH] {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}");
        }

        private static void LogAuthorizationFailure(string message)
        {
            System.Diagnostics.Trace.TraceWarning($"[AUTHZ] {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}");
        }

        #endregion
    }
}
