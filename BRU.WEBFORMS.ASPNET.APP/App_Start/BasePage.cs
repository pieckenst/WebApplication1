using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BRU.WEBFORMS.ASPNET.APP.Controls;
using BRU.WEBFORMS.ASPNET.APP.Services;

namespace BRU.WEBFORMS.ASPNET.APP
{
    /// <summary>
    /// Enterprise-grade base page class for all content pages.
    /// Provides:
    /// - PageContent and SiteConfig integration
    /// - Common lifecycle management
    /// - Comprehensive error handling with categorization
    /// - Logging and diagnostics
    /// - User feedback mechanisms (success/error messages)
    /// - Helper methods for common operations
    /// - Performance monitoring
    /// - Request context tracking
    /// </summary>
    public class BasePage : Page
    {
        #region Private Fields

        private Stopwatch _pageLoadTimer;
        private readonly List<string> _validationErrors = new List<string>();
        private readonly List<string> _businessRuleViolations = new List<string>();

        #endregion

        #region Protected Properties

        /// <summary>
        /// Gets the current user's username from the authentication context.
        /// Returns null if not authenticated.
        /// </summary>
        protected string CurrentUsername
        {
            get { return User?.Identity?.Name; }
        }

        /// <summary>
        /// Gets whether the current user is authenticated.
        /// </summary>
        protected bool IsAuthenticated
        {
            get { return User?.Identity?.IsAuthenticated ?? false; }
        }

        /// <summary>
        /// Gets the client IP address for logging and audit purposes.
        /// </summary>
        protected string ClientIpAddress
        {
            get
            {
                string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(ip))
                    ip = Request.ServerVariables["REMOTE_ADDR"];
                return ip;
            }
        }

        /// <summary>
        /// Gets the current request ID for correlation across logs.
        /// </summary>
        protected string RequestId
        {
            get
            {
                if (Context.Items["RequestId"] == null)
                    Context.Items["RequestId"] = Guid.NewGuid().ToString("N");
                return Context.Items["RequestId"].ToString();
            }
        }

        #endregion

        #region Page Lifecycle

        protected override void InitializeCulture()
        {
            base.InitializeCulture();
            Culture = Localization.Culture.Name;
            UICulture = Localization.Culture.Name;
        }

        protected override void OnPreInit(EventArgs e)
        {
            _pageLoadTimer = Stopwatch.StartNew();
            
            LogPageRequest();
            
            base.OnPreInit(e);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Apply page title from configuration
            string configTitle = SiteConfig.GetPageTitle(PageContentKey);
            if (!string.IsNullOrEmpty(configTitle))
                Title = configTitle;
            else if (string.IsNullOrEmpty(Title) || Title == "Untitled Page")
                Title = SiteConfig.DefaultPageTitle;

            Title = Localization.GetPageTitle(PageContentKey, Title);

            // Wire up error event
            Error += BasePage_Error;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                try
                {
                    LoadPageContent();
                    ApplyPageContentFromConfig();
                    ApplyLiteralOverridesFromConfig();
                    SetLastUpdatedIfPresent();
                }
                catch (Exception ex)
                {
                    LogException(ex, "Error during page content loading");
                    HandleGenericError(ex);
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // Apply any deferred messages
            ApplyDeferredMessages();
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);

            if (_pageLoadTimer != null)
            {
                _pageLoadTimer.Stop();
                LogPerformanceMetric("PageLoad", _pageLoadTimer.ElapsedMilliseconds);
            }
        }

        #endregion

        #region Page Content Management

        /// <summary>
        /// Key used for page content overrides (PageContent.xml and
        /// Web.config Page.{PageContentKey}.{controlId}.{property}).
        /// Resolves to the code-behind class name without a leading underscore
        /// (e.g. "Default", "About").
        ///
        /// At runtime ASP.NET serves a generated type such as "default_aspx"
        /// that derives from the code-behind class, so GetType().Name is not
        /// the class name; we walk up to the code-behind type first.
        /// </summary>
        protected virtual string PageContentKey
        {
            get
            {
                Type type = GetType();
                while (type != null &&
                       type.Name.EndsWith("_aspx", StringComparison.OrdinalIgnoreCase))
                {
                    type = type.BaseType;
                }

                string name = (type ?? GetType()).Name;
                return name.StartsWith("_") ? name.Substring(1) : name;
            }
        }

        /// <summary>
        /// Override to set default page content in code-behind.
        /// Web.config values override these defaults when present.
        /// </summary>
        protected virtual void LoadPageContent()
        {
        }

        /// <summary>
        /// Applies ContentBox HeaderText and ContentHtml from Web.config.
        /// </summary>
        protected virtual void ApplyPageContentFromConfig()
        {
            ApplyContentBoxConfig(this, PageContentKey);
        }

        /// <summary>
        /// Logs (via <see cref="System.Diagnostics.Trace"/>) that this page is
        /// loading its content. Call at the top of LoadPageContent.
        /// </summary>
        protected void LogPageContentLoad()
        {
            System.Diagnostics.Trace.TraceInformation(
                "LoadPageContent: page '{0}'", PageContentKey);
        }

        /// <summary>
        /// Returns the centrally-configured ContentHtml for a control on this
        /// page (App_Data/PageContent.xml first, then Web.config) if present;
        /// otherwise returns <paramref name="defaultHtml"/>. Use this inside
        /// LoadPageContent so a page keeps an inline default while the central
        /// content file takes priority. Logs which source was used.
        /// </summary>
        protected string GetConfiguredContent(string controlId, string defaultHtml)
        {
            string configured = SiteConfig.GetContentHtml(PageContentKey, controlId);
            bool fromConfig = !string.IsNullOrWhiteSpace(configured);
            LogContentSource(controlId, "ContentHtml", fromConfig);
            return fromConfig ? configured : defaultHtml;
        }

        /// <summary>
        /// Returns the centrally-configured HeaderText for a control on this
        /// page (App_Data/PageContent.xml first, then Web.config) if present;
        /// otherwise returns <paramref name="defaultHeader"/>. Logs which source
        /// was used.
        /// </summary>
        protected string GetConfiguredHeader(string controlId, string defaultHeader)
        {
            string configured = SiteConfig.GetHeaderText(PageContentKey, controlId);
            bool fromConfig = !string.IsNullOrWhiteSpace(configured);
            LogContentSource(controlId, "HeaderText", fromConfig);
            return fromConfig ? configured : defaultHeader;
        }

        private void LogContentSource(string controlId, string propertyName, bool fromConfig)
        {
            System.Diagnostics.Trace.TraceInformation(
                "PageContent: {0}.{1}.{2} <- {3}",
                PageContentKey,
                controlId,
                propertyName,
                fromConfig ? "central config (PageContent.xml/Web.config)" : "code-behind default");
        }

        /// <summary>
        /// Sets the content of a ContentBox control by ID.
        /// </summary>
        protected void SetContent(string contentBoxId, string headerText, string contentHtml)
        {
            var control = FindControlRecursive(this, contentBoxId) as ContentBox;
            if (control == null)
                return;

            if (headerText != null)
                control.HeaderText = headerText;
            if (contentHtml != null)
                control.ContentHtml = contentHtml;
        }

        /// <summary>
        /// Sets the content of a ContentBox with specified colors.
        /// </summary>
        protected void SetContent(string contentBoxId, string headerText, string contentHtml,
            HeaderColorStyle headerColor, ContentColorStyle contentColor)
        {
            var control = FindControlRecursive(this, contentBoxId) as ContentBox;
            if (control == null)
                return;

            if (headerText != null)
                control.HeaderText = headerText;
            if (contentHtml != null)
                control.ContentHtml = contentHtml;

            control.HeaderColor = headerColor;
            control.ContentColor = contentColor;
        }

        /// <summary>
        /// Sets a literal control's text by ID.
        /// </summary>
        protected void SetLiteral(string literalId, string text)
        {
            var control = FindControlRecursive(this, literalId) as Literal;
            if (control != null)
                control.Text = text;
        }

        private void ApplyContentBoxConfig(Control root, string pageKey)
        {
            if (root is ContentBox contentBox && !string.IsNullOrEmpty(root.ID))
            {
                string headerText = SiteConfig.GetHeaderText(pageKey, root.ID);
                if (!string.IsNullOrEmpty(headerText))
                    contentBox.HeaderText = headerText;

                string contentHtml = SiteConfig.GetContentHtml(pageKey, root.ID);
                if (!string.IsNullOrEmpty(contentHtml))
                    contentBox.ContentHtml = contentHtml;
            }

            foreach (Control child in root.Controls)
                ApplyContentBoxConfig(child, pageKey);
        }

        private void ApplyLiteralOverridesFromConfig()
        {
            ApplyLiteralConfig(this, PageContentKey);
        }

        private void ApplyLiteralConfig(Control root, string pageKey)
        {
            var literal = root as Literal;
            if (literal != null && !string.IsNullOrEmpty(root.ID))
            {
                string text = SiteConfig.GetLiteralText(pageKey, root.ID);
                if (!string.IsNullOrEmpty(text))
                    literal.Text = text;
            }

            foreach (Control child in root.Controls)
                ApplyLiteralConfig(child, pageKey);
        }

        private void SetLastUpdatedIfPresent()
        {
            var literal = FindControlRecursive(this, "litLastUpdated") as Literal;
            if (literal == null || !string.IsNullOrEmpty(literal.Text))
                return;

            literal.Text = "Last Updated: " + DateTime.Now.ToString("MM/dd/yyyy");
        }

        #endregion

        #region Error Handling

        private void BasePage_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex != null)
            {
                LogException(ex, "Unhandled page error");
                Server.ClearError();
                HandleGenericError(ex);
            }
        }

        /// <summary>
        /// Handles database-specific errors with specialized messaging.
        /// </summary>
        protected void HandleDatabaseError(SqlException sqlEx)
        {
            LogException(sqlEx, "Database error");

            string userMessage;
            switch (sqlEx.Number)
            {
                case -1: // Timeout
                case -2:
                    userMessage = "Database operation timed out. Please try again.";
                    break;
                case 2627: // Unique constraint violation
                case 2601:
                    userMessage = "A record with these values already exists.";
                    break;
                case 547: // Foreign key violation
                    userMessage = "Cannot perform this operation due to related records.";
                    break;
                case 515: // NOT NULL constraint violation
                    userMessage = "Required information is missing.";
                    break;
                case 1205: // Deadlock victim
                    userMessage = "Operation conflicted with another user's changes. Please try again.";
                    break;
                case 18456: // Login failed
                    userMessage = "Database authentication failed.";
                    break;
                default:
                    userMessage = "A database error occurred. Please contact support if the problem persists.";
                    break;
            }

            ShowError(userMessage);
        }

        /// <summary>
        /// Handles service-layer exceptions with appropriate user messaging.
        /// </summary>
        protected void HandleServiceError(ServiceException svcEx)
        {
            LogException(svcEx, "Service layer error");

            // ServiceException uses the base Message property
            ShowError(!string.IsNullOrEmpty(svcEx.Message) 
                ? svcEx.Message 
                : "An error occurred while processing your request. Please try again.");
        }

        /// <summary>
        /// Handles general exceptions with logging and generic user messaging.
        /// </summary>
        protected void HandleGenericError(Exception ex)
        {
            LogException(ex, "Generic error");
            ShowError("An unexpected error occurred. Please try again or contact support if the problem persists.");
        }

        /// <summary>
        /// Handles validation errors by collecting and displaying them.
        /// </summary>
        protected void HandleValidationErrors(List<string> errors)
        {
            if (errors == null || errors.Count == 0)
                return;

            _validationErrors.AddRange(errors);
            
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<strong>Please correct the following:</strong>");
            sb.AppendLine("<ul>");
            foreach (string error in errors)
            {
                sb.AppendLine($"<li>{HttpUtility.HtmlEncode(error)}</li>");
            }
            sb.AppendLine("</ul>");

            ShowError(sb.ToString());
        }

        /// <summary>
        /// Handles business rule violations with detailed messaging.
        /// </summary>
        protected void HandleBusinessRuleViolations(List<string> violations)
        {
            if (violations == null || violations.Count == 0)
                return;

            _businessRuleViolations.AddRange(violations);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<strong>The following business rules were violated:</strong>");
            sb.AppendLine("<ul>");
            foreach (string violation in violations)
            {
                sb.AppendLine($"<li>{HttpUtility.HtmlEncode(violation)}</li>");
            }
            sb.AppendLine("</ul>");

            ShowError(sb.ToString());
        }

        #endregion

        #region User Feedback

        /// <summary>
        /// Displays an error message to the user.
        /// Finds pnlError and litError controls recursively.
        /// </summary>
        protected void ShowError(string message)
        {
            Panel pnlError = FindControlRecursive(this, "pnlError") as Panel;
            Literal litError = FindControlRecursive(this, "litError") as Literal;

            if (pnlError != null && litError != null)
            {
                pnlError.Visible = true;
                litError.Text = message;
            }

            // Hide success panel if present
            Panel pnlSuccess = FindControlRecursive(this, "pnlSuccess") as Panel;
            if (pnlSuccess != null)
                pnlSuccess.Visible = false;
        }

        /// <summary>
        /// Displays a success message to the user.
        /// Finds pnlSuccess and litSuccess controls recursively.
        /// </summary>
        protected void ShowSuccess(string message)
        {
            Panel pnlSuccess = FindControlRecursive(this, "pnlSuccess") as Panel;
            Literal litSuccess = FindControlRecursive(this, "litSuccess") as Literal;

            if (pnlSuccess != null && litSuccess != null)
            {
                pnlSuccess.Visible = true;
                litSuccess.Text = message;
            }

            // Hide error panel if present
            Panel pnlError = FindControlRecursive(this, "pnlError") as Panel;
            if (pnlError != null)
                pnlError.Visible = false;
        }

        /// <summary>
        /// Hides all message panels.
        /// </summary>
        protected void HideMessages()
        {
            Panel pnlError = FindControlRecursive(this, "pnlError") as Panel;
            Panel pnlSuccess = FindControlRecursive(this, "pnlSuccess") as Panel;

            if (pnlError != null)
                pnlError.Visible = false;
            if (pnlSuccess != null)
                pnlSuccess.Visible = false;
        }

        /// <summary>
        /// Shows a confirmation message before a destructive action.
        /// Returns JavaScript for client-side confirmation.
        /// </summary>
        protected string GetConfirmationScript(string message)
        {
            return $"return confirm('{HttpUtility.JavaScriptStringEncode(message)}');";
        }

        private void ApplyDeferredMessages()
        {
            // Check for messages stored in session (e.g., after redirect)
            if (Session["SuccessMessage"] != null)
            {
                ShowSuccess(Session["SuccessMessage"].ToString());
                Session.Remove("SuccessMessage");
            }

            if (Session["ErrorMessage"] != null)
            {
                ShowError(Session["ErrorMessage"].ToString());
                Session.Remove("ErrorMessage");
            }
        }

        /// <summary>
        /// Stores a success message in session for display after redirect.
        /// </summary>
        protected void SetDeferredSuccess(string message)
        {
            Session["SuccessMessage"] = message;
        }

        /// <summary>
        /// Stores an error message in session for display after redirect.
        /// </summary>
        protected void SetDeferredError(string message)
        {
            Session["ErrorMessage"] = message;
        }

        #endregion

        #region Logging and Diagnostics

        /// <summary>
        /// Logs an exception with full context information.
        /// </summary>
        protected void LogException(Exception ex, string context)
        {
            StringBuilder logEntry = new StringBuilder();
            logEntry.AppendLine($"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
            logEntry.AppendLine($"Context: {context}");
            logEntry.AppendLine($"RequestId: {RequestId}");
            logEntry.AppendLine($"Page: {Request.Url?.AbsolutePath}");
            logEntry.AppendLine($"User: {CurrentUsername ?? "Anonymous"}");
            logEntry.AppendLine($"IP: {ClientIpAddress}");
            logEntry.AppendLine($"Exception: {ex.GetType().Name}");
            logEntry.AppendLine($"Message: {ex.Message}");
            logEntry.AppendLine($"StackTrace: {ex.StackTrace}");

            if (ex.InnerException != null)
            {
                logEntry.AppendLine($"InnerException: {ex.InnerException.GetType().Name}");
                logEntry.AppendLine($"InnerMessage: {ex.InnerException.Message}");
            }

            System.Diagnostics.Trace.TraceError(logEntry.ToString());
            
            // Also log to event log for critical errors
            if (ex is SqlException || ex is NullReferenceException || ex is InvalidOperationException)
            {
                try
                {
                    EventLog.WriteEntry("Autopark Application", logEntry.ToString(), 
                        EventLogEntryType.Error);
                }
                catch
                {
                    // Silently fail if event log is not accessible
                }
            }
        }

        /// <summary>
        /// Logs informational messages.
        /// </summary>
        protected void LogInformation(string message)
        {
            System.Diagnostics.Trace.TraceInformation($"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} RequestId:{RequestId} User:{CurrentUsername ?? "Anonymous"} - {message}");
        }

        /// <summary>
        /// Logs warning messages.
        /// </summary>
        protected void LogWarning(string message)
        {
            System.Diagnostics.Trace.TraceWarning($"[WARN] {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} RequestId:{RequestId} User:{CurrentUsername ?? "Anonymous"} - {message}");
        }

        /// <summary>
        /// Logs performance metrics.
        /// </summary>
        protected void LogPerformanceMetric(string operation, long milliseconds)
        {
            if (milliseconds > 1000) // Log slow operations
            {
                LogWarning($"Slow operation: {operation} took {milliseconds}ms");
            }
            else
            {
                System.Diagnostics.Trace.TraceInformation($"[PERF] {operation}: {milliseconds}ms");
            }
        }

        private void LogPageRequest()
        {
            StringBuilder logEntry = new StringBuilder();
            logEntry.Append($"[REQUEST] {Request.HttpMethod} {Request.Url?.AbsolutePath}");
            logEntry.Append($" | RequestId:{RequestId}");
            logEntry.Append($" | User:{CurrentUsername ?? "Anonymous"}");
            logEntry.Append($" | IP:{ClientIpAddress}");
            logEntry.Append($" | UserAgent:{Request.UserAgent}");
            
            System.Diagnostics.Trace.TraceInformation(logEntry.ToString());
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Recursively finds a control by ID within the control tree.
        /// </summary>
        protected Control FindControlRecursive(Control root, string id)
        {
            if (root == null || string.IsNullOrEmpty(id))
                return null;

            if (root.ID == id)
                return root;

            foreach (Control child in root.Controls)
            {
                Control found = FindControlRecursive(child, id);
                if (found != null)
                    return found;
            }

            return null;
        }

        /// <summary>
        /// Finds a strongly-typed control recursively.
        /// </summary>
        protected T FindControl<T>(string id) where T : Control
        {
            return FindControlRecursive(this, id) as T;
        }

        /// <summary>
        /// Validates that required fields are not empty.
        /// </summary>
        protected bool ValidateRequired(Dictionary<string, string> fields)
        {
            List<string> errors = new List<string>();

            foreach (var field in fields)
            {
                if (string.IsNullOrWhiteSpace(field.Value))
                {
                    errors.Add($"{field.Key} is required.");
                }
            }

            if (errors.Count > 0)
            {
                HandleValidationErrors(errors);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Safely parses an integer from a string.
        /// </summary>
        protected bool TryParseInt(string value, out int result, string fieldName = "Value")
        {
            if (int.TryParse(value, out result))
                return true;

            HandleValidationErrors(new List<string> { $"{fieldName} must be a valid integer." });
            return false;
        }

        /// <summary>
        /// Safely parses a decimal from a string.
        /// </summary>
        protected bool TryParseDecimal(string value, out decimal result, string fieldName = "Value")
        {
            if (decimal.TryParse(value, out result))
                return true;

            HandleValidationErrors(new List<string> { $"{fieldName} must be a valid decimal number." });
            return false;
        }

        /// <summary>
        /// Safely parses a date from a string.
        /// </summary>
        protected bool TryParseDate(string value, out DateTime result, string fieldName = "Date")
        {
            if (DateTime.TryParse(value, out result))
                return true;

            HandleValidationErrors(new List<string> { $"{fieldName} must be a valid date." });
            return false;
        }

        /// <summary>
        /// Redirects with a success message.
        /// </summary>
        protected void RedirectWithSuccess(string url, string message)
        {
            SetDeferredSuccess(message);
            Response.Redirect(url, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Redirects with an error message.
        /// </summary>
        protected void RedirectWithError(string url, string message)
        {
            SetDeferredError(message);
            Response.Redirect(url, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Formats a currency value for display.
        /// </summary>
        protected string FormatCurrency(decimal value)
        {
            return value.ToString("N2") + " руб.";
        }

        /// <summary>
        /// Formats a date for display.
        /// </summary>
        protected string FormatDate(DateTime? date)
        {
            return date?.ToString("dd.MM.yyyy") ?? "-";
        }

        /// <summary>
        /// Formats a datetime for display.
        /// </summary>
        protected string FormatDateTime(DateTime? dateTime)
        {
            return dateTime?.ToString("dd.MM.yyyy HH:mm") ?? "-";
        }

        /// <summary>
        /// HTML-encodes a string safely.
        /// </summary>
        protected string Encode(string value)
        {
            return HttpUtility.HtmlEncode(value ?? string.Empty);
        }

        /// <summary>
        /// Escapes a string for use in JavaScript.
        /// </summary>
        protected string EncodeJs(string value)
        {
            return HttpUtility.JavaScriptStringEncode(value ?? string.Empty);
        }

        #endregion
    }
}
