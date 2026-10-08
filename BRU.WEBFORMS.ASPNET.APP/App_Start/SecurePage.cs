using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BRU.WEBFORMS.ASPNET.APP
{
    /// <summary>
    /// Secure base page that enforces authentication and authorization.
    /// All protected pages should inherit from this class.
    /// Provides declarative and programmatic security checks.
    /// </summary>
    public class SecurePage : BasePage
    {
        private bool _requestAuthorized = true;

        #region Protected Properties

        /// <summary>
        /// Override to specify required roles for this page.
        /// Leave empty to allow any authenticated user.
        /// </summary>
        protected virtual string[] RequiredRoles
        {
            get { return new string[0]; }
        }

        /// <summary>
        /// Override to specify required permissions for this page.
        /// Leave empty to skip permission check.
        /// </summary>
        protected virtual string[] RequiredPermissions
        {
            get { return new string[0]; }
        }

        /// <summary>
        /// Override to specify if ANY of the required roles is sufficient (OR logic).
        /// Default is false, meaning ALL roles are required (AND logic).
        /// </summary>
        protected virtual bool AllowAnyRole
        {
            get { return true; }
        }

        /// <summary>
        /// Override to specify if ANY of the required permissions is sufficient (OR logic).
        /// Default is false, meaning ALL permissions are required (AND logic).
        /// </summary>
        protected virtual bool AllowAnyPermission
        {
            get { return true; }
        }

        /// <summary>
        /// Override to specify the redirect URL when authorization fails.
        /// Default is "/Login.aspx".
        /// </summary>
        protected virtual string LoginUrl
        {
            get { return "~/Login.aspx"; }
        }

        /// <summary>
        /// Override to specify the redirect URL when user is authenticated but lacks permissions.
        /// Default is "/Default.aspx".
        /// </summary>
        protected virtual string AccessDeniedUrl
        {
            get { return "~/Default.aspx"; }
        }

        /// <summary>
        /// Override to allow anonymous access to this page.
        /// Default is false.
        /// </summary>
        protected virtual bool AllowAnonymous
        {
            get { return false; }
        }

        #endregion

        #region Page Lifecycle

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Perform security checks before page initialization
            if (!AllowAnonymous)
            {
                if (!AuthContext.RefreshAuthorization())
                {
                    _requestAuthorized = false;
                    RedirectToLogin("You must be logged in to access this page.");
                    return;
                }

                CheckAuthorization();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!_requestAuthorized)
                return;

            // Check for session timeout BEFORE updating activity timestamp.
            // Otherwise the just-updated LastActivity can never be older than the threshold.
            if (!AllowAnonymous && AuthContext.IsAuthenticated)
            {
                if (AuthContext.IsSessionTimedOut())
                {
                    LogWarning($"Session timed out for user '{AuthContext.CurrentUsername}'");
                    AuthContext.SignOut();
                    RedirectToLogin("Your session has expired. Please log in again.");
                    return;
                }

                AuthContext.UpdateActivity();
            }

            base.OnLoad(e);
        }

        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            if (!_requestAuthorized)
                return;

            base.RaisePostBackEvent(sourceControl, eventArgument);
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (_requestAuthorized)
                base.OnPreRender(e);
        }

        /// <summary>
        /// Checks the effective write permission for a page operation.
        /// Keeps the administrator and payment/sale compatibility rules that
        /// previously existed in the postback mutation gate, but requires
        /// each write entry point to opt in explicitly.
        /// </summary>
        protected bool HasWritePermission(string permissionName)
        {
            if (string.IsNullOrWhiteSpace(permissionName))
                return false;

            if (AuthContext.IsInRole("administrator"))
                return true;

            if (string.Equals(permissionName, "payment.write", StringComparison.OrdinalIgnoreCase))
            {
                return AuthContext.HasPermission(permissionName)
                    || AuthContext.HasPermission("sale.write");
            }

            return AuthContext.HasPermission(permissionName);
        }

        /// <summary>
        /// Requires the effective write permission for an explicit action.
        /// </summary>
        protected void RequireWritePermission(string permissionName)
        {
            if (!HasWritePermission(permissionName))
            {
                LogWarning($"User '{AuthContext.CurrentUsername}' denied write operation - missing permission '{permissionName}'");
                throw new UnauthorizedAccessException($"You do not have permission to perform this write operation: {permissionName}");
            }

            LogInformation($"User '{AuthContext.CurrentUsername}' authorized for write permission '{permissionName}'");
        }

        protected void RequireAuthentication()
        {
            if (!AuthContext.RefreshAuthorization())
                throw new UnauthorizedAccessException("Authentication is required.");
        }

        protected void RequireRole(string roleName)
        {
            AuthContext.RequireRole(roleName);
        }

        protected void RequireAnyRole(params string[] roleNames)
        {
            AuthContext.RequireAnyRole(roleNames);
        }

        protected void RequirePermission(string permissionName)
        {
            AuthContext.RequirePermission(permissionName);
        }

        protected void RequireAnyPermission(params string[] permissionNames)
        {
            AuthContext.RequireAnyPermission(permissionNames);
        }

        #endregion

        #region Security Methods

        /// <summary>
        /// Checks if the user is authenticated. Redirects to login if not.
        /// </summary>
        protected virtual void CheckAuthentication()
        {
            if (!AuthContext.IsAuthenticated)
            {
                _requestAuthorized = false;
                LogWarning($"Unauthenticated access attempt to {Request.Url?.AbsolutePath} from {ClientIpAddress}");
                RedirectToLogin("You must be logged in to access this page.");
            }
        }

        /// <summary>
        /// Checks if the user has required roles and permissions.
        /// </summary>
        protected virtual void CheckAuthorization()
        {
            // Check roles
            if (RequiredRoles != null && RequiredRoles.Length > 0)
            {
                bool hasRole = AllowAnyRole
                    ? AuthContext.IsInAnyRole(RequiredRoles)
                    : AuthContext.IsInAllRoles(RequiredRoles);

                if (!hasRole)
                {
                    _requestAuthorized = false;
                    string roles = string.Join(", ", RequiredRoles);
                    LogWarning($"User '{AuthContext.CurrentUsername}' attempted to access {Request.Url?.AbsolutePath} but lacks required roles: {roles}");
                    RedirectToAccessDenied($"You do not have the required role(s) to access this page: {roles}");
                    return;
                }
            }

            // Check permissions
            if (RequiredPermissions != null && RequiredPermissions.Length > 0)
            {
                bool hasPermission = AllowAnyPermission
                    ? AuthContext.HasAnyPermission(RequiredPermissions)
                    : AuthContext.HasAllPermissions(RequiredPermissions);

                if (!hasPermission)
                {
                    _requestAuthorized = false;
                    string permissions = string.Join(", ", RequiredPermissions);
                    LogWarning($"User '{AuthContext.CurrentUsername}' attempted to access {Request.Url?.AbsolutePath} but lacks required permissions: {permissions}");
                    RedirectToAccessDenied($"You do not have the required permission(s) to access this page: {permissions}");
                    return;
                }
            }
        }

        /// <summary>
        /// Redirects to the login page with return URL.
        /// </summary>
        protected void RedirectToLogin(string message = null)
        {
            _requestAuthorized = false;
            string returnUrl = Request.Url?.PathAndQuery;
            string loginUrl = ResolveUrl(LoginUrl);

            if (!string.IsNullOrEmpty(returnUrl))
                loginUrl += "?ReturnUrl=" + Server.UrlEncode(returnUrl);

            if (!string.IsNullOrEmpty(message))
            {
                Session["LoginMessage"] = message;
            }

            Response.Redirect(loginUrl, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Redirects to the access denied page.
        /// </summary>
        protected void RedirectToAccessDenied(string message = null)
        {
            _requestAuthorized = false;
            if (!string.IsNullOrEmpty(message))
            {
                SetDeferredError(message);
            }

            Response.Redirect(ResolveUrl(AccessDeniedUrl), false);
            Context.ApplicationInstance.CompleteRequest();
        }

        #endregion

        #region Permission-Based UI Control

        /// <summary>
        /// Hides a control if the user doesn't have the specified permission.
        /// </summary>
        protected void HideIfNoPermission(Control control, string permissionName)
        {
            if (control == null || string.IsNullOrEmpty(permissionName))
                return;

            if (!AuthContext.HasPermission(permissionName))
            {
                control.Visible = false;
            }
        }

        /// <summary>
        /// Disables a control if the user doesn't have the specified permission.
        /// </summary>
        protected void DisableIfNoPermission(WebControl control, string permissionName)
        {
            if (control == null || string.IsNullOrEmpty(permissionName))
                return;

            if (!AuthContext.HasPermission(permissionName))
            {
                control.Enabled = false;
                control.CssClass += " disabled";
            }
        }

        /// <summary>
        /// Hides a control if the user doesn't have the specified role.
        /// </summary>
        protected void HideIfNotInRole(Control control, string roleName)
        {
            if (control == null || string.IsNullOrEmpty(roleName))
                return;

            if (!AuthContext.IsInRole(roleName))
            {
                control.Visible = false;
            }
        }

        /// <summary>
        /// Disables a control if the user doesn't have the specified role.
        /// </summary>
        protected void DisableIfNotInRole(WebControl control, string roleName)
        {
            if (control == null || string.IsNullOrEmpty(roleName))
                return;

            if (!AuthContext.IsInRole(roleName))
            {
                control.Enabled = false;
                control.CssClass += " disabled";
            }
        }

        /// <summary>
        /// Shows or hides a control based on permission.
        /// </summary>
        protected void SetVisibilityByPermission(Control control, string permissionName, bool defaultVisible = false)
        {
            if (control == null || string.IsNullOrEmpty(permissionName))
                return;

            control.Visible = AuthContext.HasPermission(permissionName) ? true : defaultVisible;
        }

        /// <summary>
        /// Enables or disables a control based on permission.
        /// </summary>
        protected void SetEnabledByPermission(WebControl control, string permissionName, bool defaultEnabled = false)
        {
            if (control == null || string.IsNullOrEmpty(permissionName))
                return;

            control.Enabled = AuthContext.HasPermission(permissionName) ? true : defaultEnabled;
        }

        /// <summary>
        /// Applies permission-based visibility to multiple controls.
        /// </summary>
        protected void ApplyPermissionVisibility(Dictionary<Control, string> controlPermissions)
        {
            if (controlPermissions == null)
                return;

            foreach (var kvp in controlPermissions)
            {
                HideIfNoPermission(kvp.Key, kvp.Value);
            }
        }

        /// <summary>
        /// Checks if the current user can perform a specific action.
        /// Logs the check for audit purposes.
        /// </summary>
        protected bool CanPerformAction(string actionName, string requiredPermission = null, string requiredRole = null)
        {
            bool canPerform = true;

            if (!string.IsNullOrEmpty(requiredPermission))
            {
                canPerform = AuthContext.HasPermission(requiredPermission);
                if (!canPerform)
                {
                    LogWarning($"User '{AuthContext.CurrentUsername}' attempted action '{actionName}' but lacks permission '{requiredPermission}'");
                }
            }

            if (canPerform && !string.IsNullOrEmpty(requiredRole))
            {
                canPerform = AuthContext.IsInRole(requiredRole);
                if (!canPerform)
                {
                    LogWarning($"User '{AuthContext.CurrentUsername}' attempted action '{actionName}' but lacks role '{requiredRole}'");
                }
            }

            if (canPerform)
            {
                LogInformation($"User '{AuthContext.CurrentUsername}' performed action '{actionName}'");
            }

            return canPerform;
        }

        /// <summary>
        /// Requires permission to perform an action. Throws exception if not authorized.
        /// </summary>
        protected void RequireActionPermission(string actionName, string permissionName)
        {
            if (!AuthContext.HasPermission(permissionName))
            {
                LogWarning($"User '{AuthContext.CurrentUsername}' denied action '{actionName}' - missing permission '{permissionName}'");
                throw new UnauthorizedAccessException($"You do not have permission to perform this action: {actionName}");
            }

            LogInformation($"User '{AuthContext.CurrentUsername}' authorized for action '{actionName}'");
        }

        /// <summary>
        /// Requires role to perform an action. Throws exception if not authorized.
        /// </summary>
        protected void RequireActionRole(string actionName, string roleName)
        {
            if (!AuthContext.IsInRole(roleName))
            {
                LogWarning($"User '{AuthContext.CurrentUsername}' denied action '{actionName}' - missing role '{roleName}'");
                throw new UnauthorizedAccessException($"You do not have the required role to perform this action: {actionName}");
            }

            LogInformation($"User '{AuthContext.CurrentUsername}' authorized for action '{actionName}'");
        }

        #endregion

        #region Audit Logging

        /// <summary>
        /// Logs a security-related event with full context.
        /// </summary>
        protected void LogSecurityEvent(string eventType, string description, Dictionary<string, string> additionalData = null)
        {
            System.Text.StringBuilder log = new System.Text.StringBuilder();
            log.AppendLine($"[SECURITY] {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
            log.AppendLine($"Event: {eventType}");
            log.AppendLine($"User: {AuthContext.CurrentUsername ?? "Anonymous"}");
            log.AppendLine($"UserId: {AuthContext.CurrentUserId?.ToString() ?? "N/A"}");
            log.AppendLine($"IP: {ClientIpAddress}");
            log.AppendLine($"Page: {Request.Url?.AbsolutePath}");
            log.AppendLine($"Description: {description}");

            if (additionalData != null && additionalData.Count > 0)
            {
                log.AppendLine("Additional Data:");
                foreach (var kvp in additionalData)
                {
                    log.AppendLine($"  {kvp.Key}: {kvp.Value}");
                }
            }

            System.Diagnostics.Trace.TraceInformation(log.ToString());
        }

        /// <summary>
        /// Logs a data access event for audit trails.
        /// </summary>
        protected void LogDataAccess(string entityType, string entityId, string operation, string result = "Success")
        {
            LogSecurityEvent("DataAccess", $"{operation} on {entityType}", new Dictionary<string, string>
            {
                { "EntityType", entityType },
                { "EntityId", entityId },
                { "Operation", operation },
                { "Result", result }
            });
        }

        #endregion
    }
}
