using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using BRU.WEBFORMS.ASPNET.APP.Services;
using BRU.WEBFORMS.ASPNET.APP.Models;

namespace BRU.WEBFORMS.ASPNET.APP
{
    /// <summary>
    /// Main dashboard page for Autopark Management System.
    /// Provides comprehensive overview of fleet operations, employee status,
    /// route schedules, sales performance, and maintenance requirements.
    /// Implements enterprise-grade error handling and data validation.
    /// </summary>
    public partial class Default : System.Web.UI.Page
    {
        #region Control Declarations
        
        protected global::System.Web.UI.WebControls.Panel pnlError;
        protected global::System.Web.UI.WebControls.Literal litError;
        protected global::System.Web.UI.WebControls.Literal litLastUpdated;
        protected global::System.Web.UI.WebControls.Literal litActiveBuses;
        protected global::System.Web.UI.WebControls.Literal litActiveEmployees;
        protected global::System.Web.UI.WebControls.Literal litActiveRoutes;
        protected global::System.Web.UI.WebControls.Literal litTodaySchedule;
        protected global::System.Web.UI.WebControls.Literal litTodaySales;
        protected global::System.Web.UI.WebControls.Literal litTodayRevenue;
        protected global::System.Web.UI.WebControls.Literal litBusesAttention;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbAlerts;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbRecentActivity;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbMaintenance;
        protected global::System.Web.UI.WebControls.HyperLink hlBuses;
        protected global::System.Web.UI.WebControls.HyperLink hlEmployees;
        protected global::System.Web.UI.WebControls.HyperLink hlRoutes;
        protected global::System.Web.UI.WebControls.HyperLink hlSales;
        protected global::System.Web.UI.WebControls.HyperLink hlReports;
        
        #endregion

        #region Page Lifecycle Methods

        /// <summary>
        /// Page load event handler with comprehensive data loading and error handling.
        /// Implements idempotent loading pattern to prevent duplicate data fetching on postbacks.
        /// </summary>
        /// <param name="sender">Event source</param>
        /// <param name="e">Event arguments</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    InitializeDashboardComponents();
                    LoadDashboardData();
                    LoadSystemAlerts();
                    LoadRecentActivity();
                    LoadMaintenanceOverview();
                    UpdateNavigationLinks();
                    SetPageMetadata();
                }
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                HandleDatabaseError(sqlEx);
            }
            catch (ServiceException serviceEx)
            {
                HandleServiceError(serviceEx);
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        #endregion

        #region Data Loading Methods

        /// <summary>
        /// Initializes dashboard UI components with default values and styling.
        /// Ensures consistent visual presentation before data loading.
        /// </summary>
        private void InitializeDashboardComponents()
        {
            try
            {
                // Initialize statistics literals with loading indicators
                litActiveBuses.Text = "<span class='status-warning'>Loading...</span>";
                litActiveEmployees.Text = "<span class='status-warning'>Loading...</span>";
                litActiveRoutes.Text = "<span class='status-warning'>Loading...</span>";
                litTodaySchedule.Text = "<span class='status-warning'>Loading...</span>";
                litTodaySales.Text = "<span class='status-warning'>Loading...</span>";
                litTodayRevenue.Text = "<span class='status-warning'>Loading...</span>";
                litBusesAttention.Text = "<span class='status-warning'>Loading...</span>";
                
                // Set last updated timestamp
                litLastUpdated.Text = "Last updated: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
                
                
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error initializing dashboard components", ex);
            }
        }

        /// <summary>
        /// Loads comprehensive dashboard statistics from the maintenance service.
        /// Implements retry logic for transient failures and validates data integrity.
        /// </summary>
        private void LoadDashboardData()
        {
            const int maxRetries = 3;
            int retryCount = 0;
            bool success = false;
            
            while (!success && retryCount < maxRetries)
            {
                try
                {
                    using (MaintenanceService maintenanceService = new MaintenanceService())
                    {
                        AutoparkSummary summary = maintenanceService.GetAutoparkSummary();
                        
                        // Validate data integrity
                        if (summary == null)
                        {
                            throw new ServiceException("Dashboard summary data is null");
                        }
                        
                        // Update statistics with proper formatting
                        litActiveBuses.Text = FormatNumberWithCommas(summary.ActiveBusCount);
                        litActiveEmployees.Text = FormatNumberWithCommas(summary.ActiveEmployeeCount);
                        litActiveRoutes.Text = FormatNumberWithCommas(summary.ActiveRouteCount);
                        litTodaySchedule.Text = FormatNumberWithCommas(summary.TodayScheduleCount);
                        litTodaySales.Text = FormatNumberWithCommas(summary.TodaySaleCount);
                        litTodayRevenue.Text = summary.TodayAmountTotal.ToString("F2");
                        
                        // Apply warning styling for buses needing attention
                        if (summary.BusNeedAttentionCount > 0)
                        {
                            litBusesAttention.Text = $"<span class='status-warning'>{FormatNumberWithCommas(summary.BusNeedAttentionCount)}</span>";
                        }
                        else
                        {
                            litBusesAttention.Text = $"<span class='status-good'>{FormatNumberWithCommas(summary.BusNeedAttentionCount)}</span>";
                        }
                        
                        success = true;
                    }
                }
                catch (ServiceException serviceEx)
                {
                    retryCount++;
                    if (retryCount >= maxRetries)
                    {
                        throw new ServiceException($"Failed to load dashboard data after {maxRetries} attempts", serviceEx);
                    }
                    
                    // Exponential backoff for retries
                    System.Threading.Thread.Sleep(1000 * retryCount);
                }
            }
        }

        /// <summary>
        /// Loads and analyzes system alerts including maintenance overdue notices
        /// and operational warnings. Implements comprehensive alert categorization.
        /// </summary>
        private void LoadSystemAlerts()
        {
            try
            {
                StringBuilder alertsHtml = new StringBuilder();
                bool hasCriticalAlerts = false;
                int totalAlertCount = 0;
                
                using (MaintenanceService maintenanceService = new MaintenanceService())
                {
                    // Check for buses overdue for maintenance
                    List<int> overdueBuses = maintenanceService.GetBusesOverdueForMaintenance();
                    if (overdueBuses != null && overdueBuses.Count > 0)
                    {
                        hasCriticalAlerts = true;
                        totalAlertCount += overdueBuses.Count;
                        alertsHtml.Append("<div class='alert-item'>");
                        alertsHtml.Append("<span class='alert-icon'>⚠</span>");
                        alertsHtml.Append($"<strong>{overdueBuses.Count} buses overdue for maintenance!</strong> ");
                        alertsHtml.Append($"(Bus IDs: {string.Join(", ", overdueBuses)})");
                        alertsHtml.Append("</div>");
                    }
                    
                    // Check for buses requiring immediate attention
                    AutoparkSummary summary = maintenanceService.GetAutoparkSummary();
                    if (summary != null && summary.BusNeedAttentionCount > 0)
                    {
                        totalAlertCount += summary.BusNeedAttentionCount;
                        alertsHtml.Append("<div class='alert-item'>");
                        alertsHtml.Append("<span class='alert-icon'>⚠</span>");
                        alertsHtml.Append($"<strong>{summary.BusNeedAttentionCount} buses require attention</strong> ");
                        alertsHtml.Append("due to high mileage or repair status.");
                        alertsHtml.Append("</div>");
                    }
                    
                    // Check for today's schedule status
                    if (summary != null && summary.TodayScheduleCount == 0)
                    {
                        totalAlertCount++;
                        alertsHtml.Append("<div class='alert-item'>");
                        alertsHtml.Append("<span class='alert-icon'>ℹ</span>");
                        alertsHtml.Append("<strong>No scheduled trips for today.</strong> ");
                        alertsHtml.Append("Please verify route schedule configuration.");
                        alertsHtml.Append("</div>");
                    }
                }
                
                // Set alert content based on alert status
                if (totalAlertCount > 0)
                {
                    string alertSummary = hasCriticalAlerts ? 
                        $"<strong>Critical: {totalAlertCount} alerts require immediate attention</strong>" :
                        $"<strong>Warning: {totalAlertCount} alerts detected</strong>";
                    
                    alertsHtml.Insert(0, $"<p>{alertSummary}</p>");
                    cbAlerts.ContentHtml = alertsHtml.ToString();
                }
                else
                {
                    cbAlerts.ContentHtml = "<p><span class='status-good'>✓ No system alerts at this time. All systems operating normally.</span></p>";
                }
            }
            catch (ServiceException serviceEx)
            {
                cbAlerts.ContentHtml = $"<p><span class='status-error'>Error loading system alerts: {serviceEx.Message}</span></p>";
            }
            catch (Exception ex)
            {
                cbAlerts.ContentHtml = $"<p><span class='status-error'>Unexpected error loading alerts: {ex.Message}</span></p>";
            }
        }

        /// <summary>
        /// Loads recent sales activity with comprehensive data transformation
        /// and performance optimization for large datasets.
        /// </summary>
        private void LoadRecentActivity()
        {
            try
            {
                using (TicketService ticketService = new TicketService())
                {
                    // Load sales for the past 7 days
                    DateTime startDate = DateTime.Today.AddDays(-7);
                    DateTime endDate = DateTime.Today.AddDays(1);
                    
                    List<Sale> recentSales = ticketService.GetSalesByDateRange(startDate, endDate);
                    
                    if (recentSales != null && recentSales.Count > 0)
                    {
                        StringBuilder activityHtml = new StringBuilder();
                        activityHtml.Append("<table class='data-table'>");
                        activityHtml.Append("<thead><tr><th>Date</th><th>Ticket</th><th>Channel</th><th>Amount</th><th>Status</th></tr></thead>");
                        activityHtml.Append("<tbody>");
                        
                        int displayCount = Math.Min(recentSales.Count, 10); // Show up to 10 recent sales
                        decimal totalRevenue = 0;
                        int totalTickets = 0;
                        
                        for (int i = 0; i < displayCount; i++)
                        {
                            Sale sale = recentSales[i];
                            
                            // Calculate totals
                            totalRevenue += sale.SaleTotal;
                            totalTickets += sale.TicketQuantity;
                            
                            // Apply status-based styling
                            string statusClass = GetStatusCssClass(sale.SaleStatus);
                            
                            activityHtml.Append("<tr>");
                            activityHtml.Append($"<td>{sale.SaleDate:dd.MM.yyyy HH:mm}</td>");
                            activityHtml.Append($"<td>{sale.TicketName}</td>");
                            activityHtml.Append($"<td>{sale.SaleChannel}</td>");
                            activityHtml.Append($"<td>{sale.SaleTotal:F2} BYN</td>");
                            activityHtml.Append($"<td class='{statusClass}'>{sale.SaleStatus}</td>");
                            activityHtml.Append("</tr>");
                        }
                        
                        activityHtml.Append("</tbody></table>");
                        
                        // Add summary information
                        activityHtml.Append("<p style='margin-top: 10px; font-size: 9pt;'>");
                        activityHtml.Append($"<strong>Summary:</strong> {totalTickets} tickets sold, ");
                        activityHtml.Append($"total revenue: {totalRevenue:F2} BYN");
                        if (recentSales.Count > displayCount)
                        {
                            activityHtml.Append($" (showing {displayCount} of {recentSales.Count} total sales)");
                        }
                        activityHtml.Append("</p>");
                        
                        cbRecentActivity.ContentHtml = activityHtml.ToString();
                    }
                    else
                    {
                        cbRecentActivity.ContentHtml = "<p>No sales activity recorded in the past 7 days.</p>";
                    }
                }
            }
            catch (ServiceException serviceEx)
            {
                cbRecentActivity.ContentHtml = $"<p><span class='status-error'>Error loading recent activity: {serviceEx.Message}</span></p>";
            }
            catch (Exception ex)
            {
                cbRecentActivity.ContentHtml = $"<p><span class='status-error'>Unexpected error loading activity: {ex.Message}</span></p>";
            }
        }

        /// <summary>
        /// Loads comprehensive maintenance overview including upcoming maintenance
        /// schedules, service history analysis, and cost tracking.
        /// </summary>
        private void LoadMaintenanceOverview()
        {
            try
            {
                StringBuilder maintenanceHtml = new StringBuilder();
                
                using (MaintenanceService maintenanceService = new MaintenanceService())
                {
                    // Get latest maintenance for all buses
                    List<Maintenance> latestMaintenance = maintenanceService.GetLatestMaintenance();
                    
                    if (latestMaintenance != null && latestMaintenance.Count > 0)
                    {
                        maintenanceHtml.Append("<table class='data-table'>");
                        maintenanceHtml.Append("<thead><tr><th>Bus</th><th>Last Service</th><th>Type</th><th>Status</th><th>Cost</th></tr></thead>");
                        maintenanceHtml.Append("<tbody>");
                        
                        int criticalCount = 0;
                        int warningCount = 0;
                        decimal totalCost = 0;
                        
                        foreach (Maintenance maintenance in latestMaintenance)
                        {
                            totalCost += maintenance.MaintenanceCost;
                            
                            // Categorize maintenance status
                            string statusClass = GetRoadworthinessCssClass(maintenance.Roadworthiness);
                            if (maintenance.Roadworthiness == RoadworthinessStatus.NotOperational)
                            {
                                criticalCount++;
                            }
                            else if (maintenance.Roadworthiness == RoadworthinessStatus.NeedsAttention)
                            {
                                warningCount++;
                            }
                            
                            maintenanceHtml.Append("<tr>");
                            maintenanceHtml.Append($"<td>{maintenance.FleetNumber}</td>");
                            maintenanceHtml.Append($"<td>{maintenance.MaintenanceDate:dd.MM.yyyy}</td>");
                            maintenanceHtml.Append($"<td>{maintenance.MaintenanceType}</td>");
                            maintenanceHtml.Append($"<td class='{statusClass}'>{maintenance.Roadworthiness}</td>");
                            maintenanceHtml.Append($"<td>{maintenance.MaintenanceCost:F2} BYN</td>");
                            maintenanceHtml.Append("</tr>");
                        }
                        
                        maintenanceHtml.Append("</tbody></table>");
                        
                        // Add maintenance summary
                        maintenanceHtml.Append("<p style='margin-top: 10px; font-size: 9pt;'>");
                        maintenanceHtml.Append($"<strong>Maintenance Summary:</strong> ");
                        maintenanceHtml.Append($"{latestMaintenance.Count} buses serviced, ");
                        maintenanceHtml.Append($"total cost: {totalCost:F2} BYN, ");
                        maintenanceHtml.Append($"<span class='status-error'>{criticalCount} critical</span>, ");
                        maintenanceHtml.Append($"<span class='status-warning'>{warningCount} warnings</span>");
                        maintenanceHtml.Append("</p>");
                        
                        cbMaintenance.ContentHtml = maintenanceHtml.ToString();
                    }
                    else
                    {
                        cbMaintenance.ContentHtml = "<p>No maintenance records found in the system.</p>";
                    }
                }
            }
            catch (ServiceException serviceEx)
            {
                cbMaintenance.ContentHtml = $"<p><span class='status-error'>Error loading maintenance overview: {serviceEx.Message}</span></p>";
            }
            catch (Exception ex)
            {
                cbMaintenance.ContentHtml = $"<p><span class='status-error'>Unexpected error loading maintenance: {ex.Message}</span></p>";
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Updates navigation links with proper URL resolution and accessibility attributes.
        /// Ensures consistent navigation across the application.
        /// </summary>
        private void UpdateNavigationLinks()
        {
            try
            {
                hlBuses.NavigateUrl = ResolveUrl("~/Fleet/Buses.aspx");
                hlBuses.ToolTip = "Manage bus fleet and vehicle operations";
                
                hlEmployees.NavigateUrl = ResolveUrl("~/Personnel/Employees.aspx");
                hlEmployees.ToolTip = "Manage employee records and personnel";
                
                hlRoutes.NavigateUrl = ResolveUrl("~/Operations/Routes.aspx");
                hlRoutes.ToolTip = "Manage routes and schedules";
                
                hlSales.NavigateUrl = ResolveUrl("~/Sales/Sales.aspx");
                hlSales.ToolTip = "Manage ticket sales and transactions";
                
                hlReports.NavigateUrl = ResolveUrl("~/Reports/Analytics.aspx");
                hlReports.ToolTip = "View reports and analytics";
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error updating navigation links", ex);
            }
        }

        /// <summary>
        /// Sets page metadata including title, description, and other SEO elements.
        /// Implements enterprise-grade metadata management.
        /// </summary>
        private void SetPageMetadata()
        {
            try
            {
                Page.Title = "Dashboard - Autopark Management System";
                
               
            }
            catch (Exception ex)
            {
                // Non-critical error, log but don't fail the page load
                System.Diagnostics.Debug.WriteLine($"Error setting page metadata: {ex.Message}");
            }
        }

        /// <summary>
        /// Formats numbers with thousands separators for improved readability.
        /// </summary>
        /// <param name="number">Number to format</param>
        /// <returns>Formatted number string</returns>
        private string FormatNumberWithCommas(int number)
        {
            return number.ToString("#,##0");
        }

        /// <summary>
        /// Returns CSS class based on sale status for visual indicators.
        /// </summary>
        /// <param name="status">Sale status</param>
        /// <returns>CSS class name</returns>
        private string GetStatusCssClass(string status)
        {
            switch (status)
            {
                case SaleStatus.Completed:
                    return "status-good";
                case SaleStatus.Refunded:
                case SaleStatus.Cancelled:
                    return "status-warning";
                case SaleStatus.Created:
                    return "status-warning";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Returns CSS class based on roadworthiness status for visual indicators.
        /// </summary>
        /// <param name="status">Roadworthiness status</param>
        /// <returns>CSS class name</returns>
        private string GetRoadworthinessCssClass(string status)
        {
            switch (status)
            {
                case RoadworthinessStatus.Operational:
                    return "status-good";
                case RoadworthinessStatus.NeedsAttention:
                    return "status-warning";
                case RoadworthinessStatus.NotOperational:
                    return "status-error";
                default:
                    return "";
            }
        }

        #endregion

        #region Error Handling Methods

        /// <summary>
        /// Handles database-specific errors with appropriate user messaging.
        /// Implements detailed error logging and user-friendly error display.
        /// </summary>
        /// <param name="ex">SQL exception</param>
        private void HandleDatabaseError(System.Data.SqlClient.SqlException ex)
        {
            pnlError.Visible = true;
            
            StringBuilder errorMessage = new StringBuilder();
            errorMessage.Append("<strong>Database Connection Error:</strong> ");
            errorMessage.Append("Unable to connect to the Autopark database. ");
            
            switch (ex.Number)
            {
                case 53:   // Server not found
                    errorMessage.Append("Database server is not available. Please check your connection settings.");
                    break;
                case 18456: // Login failed
                    errorMessage.Append("Authentication failed. Please verify your database credentials.");
                    break;
                case 208:   // Database not found
                    errorMessage.Append("Database 'AutoparkDB' not found. Please run the database setup script.");
                    break;
                default:
                    errorMessage.Append($"Error code: {ex.Number}. {ex.Message}");
                    break;
            }
            
            litError.Text = errorMessage.ToString();
            
            // Log detailed error for debugging
            System.Diagnostics.Debug.WriteLine($"Database Error: {ex}");
        }

        /// <summary>
        /// Handles service layer errors with context-specific messaging.
        /// </summary>
        /// <param name="ex">Service exception</param>
        private void HandleServiceError(ServiceException ex)
        {
            pnlError.Visible = true;
            litError.Text = $"<strong>Service Error:</strong> {ex.Message}";
            
            if (ex.InnerException != null)
            {
                litError.Text += $"<br><small>Details: {ex.InnerException.Message}</small>";
            }
            
            // Log detailed error for debugging
            System.Diagnostics.Debug.WriteLine($"Service Error: {ex}");
        }

        /// <summary>
        /// Handles generic unexpected errors with fallback messaging.
        /// </summary>
        /// <param name="ex">Generic exception</param>
        private void HandleGenericError(Exception ex)
        {
            pnlError.Visible = true;
            litError.Text = $"<strong>Unexpected Error:</strong> An error occurred while loading the dashboard. Please try again or contact system administrator.";
            
            // Log detailed error for debugging
            System.Diagnostics.Debug.WriteLine($"Generic Error: {ex}");
        }

        #endregion
    }
}