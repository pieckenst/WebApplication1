using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BRU.WEBFORMS.ASPNET.APP.Services;
using BRU.WEBFORMS.ASPNET.APP.Models;

namespace BRU.WEBFORMS.ASPNET.APP.Fleet
{
    /// <summary>
    /// Bus Fleet Management page for Autopark Management System.
    /// Provides comprehensive bus fleet operations including CRUD operations,
    /// filtering, searching, and maintenance tracking.
    /// Implements enterprise-grade error handling and data validation.
    /// </summary>
    public partial class Buses : System.Web.UI.Page
    {
        #region Control Declarations
        
        protected global::System.Web.UI.WebControls.Panel pnlError;
        protected global::System.Web.UI.WebControls.Literal litError;
        protected global::System.Web.UI.WebControls.Panel pnlSuccess;
        protected global::System.Web.UI.WebControls.Literal litSuccess;
        protected global::System.Web.UI.WebControls.Literal litTotalBuses;
        protected global::System.Web.UI.WebControls.Literal litOperationalBuses;
        protected global::System.Web.UI.WebControls.Literal litRepairBuses;
        protected global::System.Web.UI.WebControls.Literal litAttentionBuses;
        protected global::System.Web.UI.WebControls.Button btnAddBus;
        protected global::System.Web.UI.WebControls.Button btnRefresh;
        protected global::System.Web.UI.WebControls.Button btnExport;
        protected global::System.Web.UI.WebControls.TextBox txtSearch;
        protected global::System.Web.UI.WebControls.Button btnSearch;
        protected global::System.Web.UI.WebControls.DropDownList ddlStatusFilter;
        protected global::System.Web.UI.WebControls.DropDownList ddlManufacturerFilter;
        protected global::System.Web.UI.WebControls.TextBox txtYearFrom;
        protected global::System.Web.UI.WebControls.TextBox txtYearTo;
        protected global::System.Web.UI.WebControls.Button btnApplyYearFilter;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbFilters;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbBusesList;
        protected global::System.Web.UI.WebControls.GridView gvBuses;
        protected global::System.Web.UI.WebControls.Literal litPagination;
        
        #endregion

        #region Private Fields
        
        private BusService _busService;
        private List<Bus> _currentBusList;
        private string _currentSortExpression = "fleet_number";
        private SortDirection _currentSortDirection = SortDirection.Ascending;
        private int _currentPage = 1;
        private const int _pageSize = 20;
        
        #endregion

        #region Page Lifecycle Methods

        /// <summary>
        /// Page load event handler with comprehensive initialization.
        /// Implements idempotent loading pattern with proper state management.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    InitializeServiceLayer();
                    InitializeFilters();
                    LoadManufacturerOptions();
                    LoadBusData();
                    UpdateStatistics();
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

        #region Initialization Methods

        /// <summary>
        /// Initializes the service layer for data operations.
        /// </summary>
        private void InitializeServiceLayer()
        {
            try
            {
                _busService = new BusService();
                _currentBusList = new List<Bus>();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error initializing service layer", ex);
            }
        }

        /// <summary>
        /// Initializes filter controls with default values.
        /// </summary>
        private void InitializeFilters()
        {
            try
            {
                ddlStatusFilter.SelectedIndex = 0;
                ddlManufacturerFilter.SelectedIndex = 0;
                txtYearFrom.Text = string.Empty;
                txtYearTo.Text = string.Empty;
                txtSearch.Text = string.Empty;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error initializing filters", ex);
            }
        }

        /// <summary>
        /// Loads manufacturer options for filtering.
        /// </summary>
        private void LoadManufacturerOptions()
        {
            try
            {
                using (BusService busService = new BusService())
                {
                    List<Bus> allBuses = busService.GetAllBuses();
                    
                    if (allBuses != null && allBuses.Count > 0)
                    {
                        // Get unique manufacturers
                        var manufacturers = allBuses
                            .Select(b => b.Manufacturer)
                            .Distinct()
                            .OrderBy(m => m)
                            .ToList();
                        
                        ddlManufacturerFilter.Items.Clear();
                        ddlManufacturerFilter.Items.Add(new ListItem("All Manufacturers", ""));
                        
                        foreach (string manufacturer in manufacturers)
                        {
                            ddlManufacturerFilter.Items.Add(new ListItem(manufacturer, manufacturer));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error loading manufacturer options", ex);
            }
        }

        #endregion

        #region Data Loading Methods

        /// <summary>
        /// Loads bus data with comprehensive filtering and sorting.
        /// </summary>
        private void LoadBusData()
        {
            try
            {
                using (BusService busService = new BusService())
                {
                    // Get all buses initially
                    List<Bus> allBuses = busService.GetAllBuses();
                    
                    // Apply filters
                    _currentBusList = ApplyFilters(allBuses);
                    
                    // Apply sorting
                    _currentBusList = ApplySorting(_currentBusList);
                    
                    // Bind to grid view
                    BindGridView();
                }
            }
            catch (ServiceException serviceEx)
            {
                HandleServiceError(serviceEx);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error loading bus data", ex);
            }
        }

        /// <summary>
        /// Applies current filter settings to the bus list.
        /// </summary>
        private List<Bus> ApplyFilters(List<Bus> buses)
        {
            List<Bus> filteredBuses = new List<Bus>(buses);
            
            try
            {
                // Status filter
                if (!string.IsNullOrEmpty(ddlStatusFilter.SelectedValue))
                {
                    string selectedStatus = ddlStatusFilter.SelectedValue;
                    filteredBuses = filteredBuses.Where(b => b.Status == selectedStatus).ToList();
                }
                
                // Manufacturer filter
                if (!string.IsNullOrEmpty(ddlManufacturerFilter.SelectedValue))
                {
                    string selectedManufacturer = ddlManufacturerFilter.SelectedValue;
                    filteredBuses = filteredBuses.Where(b => b.Manufacturer == selectedManufacturer).ToList();
                }
                
                // Year range filter
                if (!string.IsNullOrEmpty(txtYearFrom.Text) && !string.IsNullOrEmpty(txtYearTo.Text))
                {
                    int yearFrom, yearTo;
                    if (int.TryParse(txtYearFrom.Text, out yearFrom) && int.TryParse(txtYearTo.Text, out yearTo))
                    {
                        filteredBuses = filteredBuses.Where(b => b.ManufactureYear >= yearFrom && b.ManufactureYear <= yearTo).ToList();
                    }
                }
                
                // Search filter
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    string searchTerm = txtSearch.Text.ToLower();
                    filteredBuses = filteredBuses.Where(b => 
                        b.FleetNumber.ToLower().Contains(searchTerm) ||
                        b.Model.ToLower().Contains(searchTerm) ||
                        b.RegistrationNum.ToLower().Contains(searchTerm)
                    ).ToList();
                }
                
                return filteredBuses;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error applying filters", ex);
            }
        }

        /// <summary>
        /// Applies current sorting to the bus list.
        /// </summary>
        private List<Bus> ApplySorting(List<Bus> buses)
        {
            try
            {
                switch (_currentSortExpression.ToLower())
                {
                    case "fleet_number":
                        return _currentSortDirection == SortDirection.Ascending 
                            ? buses.OrderBy(b => b.FleetNumber).ToList()
                            : buses.OrderByDescending(b => b.FleetNumber).ToList();
                    case "model":
                        return _currentSortDirection == SortDirection.Ascending 
                            ? buses.OrderBy(b => b.Model).ToList()
                            : buses.OrderByDescending(b => b.Model).ToList();
                    case "manufacturer":
                        return _currentSortDirection == SortDirection.Ascending 
                            ? buses.OrderBy(b => b.Manufacturer).ToList()
                            : buses.OrderByDescending(b => b.Manufacturer).ToList();
                    case "manufacture_year":
                        return _currentSortDirection == SortDirection.Ascending 
                            ? buses.OrderBy(b => b.ManufactureYear).ToList()
                            : buses.OrderByDescending(b => b.ManufactureYear).ToList();
                    case "status":
                        return _currentSortDirection == SortDirection.Ascending 
                            ? buses.OrderBy(b => b.Status).ToList()
                            : buses.OrderByDescending(b => b.Status).ToList();
                    case "mileage_km":
                        return _currentSortDirection == SortDirection.Ascending 
                            ? buses.OrderBy(b => b.MileageKm).ToList()
                            : buses.OrderByDescending(b => b.MileageKm).ToList();
                    default:
                        return buses.OrderBy(b => b.FleetNumber).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error applying sorting", ex);
            }
        }

        /// <summary>
        /// Binds filtered and sorted data to the grid view.
        /// </summary>
        private void BindGridView()
        {
            try
            {
                // Calculate pagination
                int totalItems = _currentBusList.Count;
                int totalPages = (int)Math.Ceiling((double)totalItems / _pageSize);
                
                // Ensure current page is valid
                if (_currentPage > totalPages) _currentPage = totalPages;
                if (_currentPage < 1) _currentPage = 1;
                
                // Get page data
                var pageData = _currentBusList
                    .Skip((_currentPage - 1) * _pageSize)
                    .Take(_pageSize)
                    .ToList();
                
                // Bind to grid view
                gvBuses.DataSource = pageData;
                gvBuses.VirtualItemCount = totalItems;
                gvBuses.DataBind();
                
                // Update pagination display
                UpdatePagination(totalPages, totalItems);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error binding grid view", ex);
            }
        }

        /// <summary>
        /// Updates pagination controls and display.
        /// </summary>
        private void UpdatePagination(int totalPages, int totalItems)
        {
            try
            {
                StringBuilder paginationHtml = new StringBuilder();
                
                if (totalPages > 1)
                {
                    paginationHtml.Append("Page ");
                    
                    for (int i = 1; i <= totalPages; i++)
                    {
                        if (i == _currentPage)
                        {
                            paginationHtml.Append($"<span class='current'>{i}</span>");
                        }
                        else
                        {
                            paginationHtml.Append($"<a href='?page={i}'>{i}</a>");
                        }
                    }
                    
                    paginationHtml.Append($" of {totalPages} ({totalItems} total buses)");
                }
                else
                {
                    paginationHtml.Append($"Showing {totalItems} buses");
                }
                
                litPagination.Text = paginationHtml.ToString();
            }
            catch (Exception ex)
                {
                    throw new ServiceException("Error updating pagination", ex);
                }
        }

        /// <summary>
        /// Updates statistics display with current fleet information.
        /// </summary>
        private void UpdateStatistics()
        {
            try
            {
                using (BusService busService = new BusService())
                {
                    List<Bus> allBuses = busService.GetAllBuses();
                    
                    if (allBuses != null)
                    {
                        litTotalBuses.Text = allBuses.Count.ToString();
                        litOperationalBuses.Text = allBuses.Count(b => b.Status == BusStatus.Operational).ToString();
                        litRepairBuses.Text = allBuses.Count(b => b.Status == BusStatus.InRepair).ToString();
                        
                        List<Bus> busesNeedingAttention = busService.GetBusesNeedingAttention();
                        litAttentionBuses.Text = busesNeedingAttention.Count.ToString();
                    }
                }
            }
            catch (ServiceException serviceEx)
            {
                HandleServiceError(serviceEx);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error updating statistics", ex);
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles add new bus button click.
        /// </summary>
        protected void btnAddBus_Click(object sender, EventArgs e)
        {
            try
            {
                // Redirect to add bus page (to be implemented)
                Response.Redirect("~/Fleet/BusEdit.aspx?mode=add");
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        /// <summary>
        /// Handles refresh button click.
        /// </summary>
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                LoadBusData();
                UpdateStatistics();
                ShowSuccessMessage("Data refreshed successfully.");
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        ///        /// <summary>
        /// Handles export to Excel button click.
        /// </summary>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                // Export current filtered data to Excel (to be implemented)
                ShowSuccessMessage("Export functionality will be implemented.");
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        /// <summary>
        /// Handles search button click.
        /// </summary>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                LoadBusData();
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        /// <summary>
        /// Handles status filter selection change.
        /// </summary>
        protected void ddlStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _currentPage = 1;
                LoadBusData();
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        /// <summary>
        /// Handles manufacturer filter selection change.
        /// </summary>
        protected void ddlManufacturerFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _currentPage = 1;
                LoadBusData();
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        /// <summary>
        /// Handles year range filter application.
        /// </summary>
        protected void btnApplyYearFilter_Click(object sender, EventArgs e)
        {
            try
            {
                _currentPage = 1;
                LoadBusData();
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        /// <summary>
        /// Handles grid view page index changing.
        /// </summary>
        protected void gvBuses_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                _currentPage = e.NewPageIndex + 1;
                BindGridView();
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        /// <summary>
        /// Handles grid view sorting.
        /// </summary>
        protected void gvBuses_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (_currentSortExpression == e.SortExpression)
                {
                    _currentSortDirection = _currentSortDirection == SortDirection.Ascending 
                        ? SortDirection.Descending 
                        : SortDirection.Ascending;
                }
                else
                {
                    _currentSortExpression = e.SortExpression;
                    _currentSortDirection = SortDirection.Ascending;
                }
                
                LoadBusData();
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        /// <summary>
        /// Handles grid view row commands (view, edit, delete, maintenance).
        /// </summary>
        protected void gvBuses_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int busId = Convert.ToInt32(e.CommandArgument);
                
                switch (e.CommandName)
                {
                    case "View":
                        // Redirect to view page (to be implemented)
                        Response.Redirect($"~/Fleet/BusDetails.aspx?busId={busId}");
                        break;
                        
                    case "Edit":
                        // Redirect to edit page (to be implemented)
                        Response.Redirect($"~/Fleet/BusEdit.aspx?busId={busId}&mode=edit");
                        break;
                        
                    case "Maintenance":
                        // Redirect to maintenance page (to be implemented)
                        Response.Redirect($"~/Fleet/BusMaintenance.aspx?busId={busId}");
                        break;
                        
                    case "Delete":
                        DeleteBus(busId);
                        break;
                }
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        /// <summary>
        /// Handles grid view row data binding for custom styling.
        /// </summary>
        protected void gvBuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Bus bus = e.Row.DataItem as Bus;
                    if (bus != null)
                    {
                        // Style status cell
                        Literal litStatus = e.Row.FindControl("litStatus") as Literal;
                        if (litStatus != null)
                        {
                            litStatus.Text = $"<span class='{GetStatusCssClass(bus.Status)}'>{bus.Status}</span>";
                        }
                        
                        // Style mileage category cell
                        Literal litMileageCategory = e.Row.FindControl("litMileageCategory") as Literal;
                        if (litMileageCategory != null)
                        {
                            litMileageCategory.Text = $"<span class='{GetMileageCssClass(bus.MileageCategory)}'>{bus.MileageCategory}</span>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        #endregion

        #region Business Logic Methods

        /// <summary>
        /// Deletes a bus with validation and business rule enforcement.
        /// </summary>
        private void DeleteBus(int busId)
        {
            try
            {
                using (BusService busService = new BusService())
                {
                    busService.DeleteBus(busId);
                    ShowSuccessMessage("Bus deleted successfully.");
                    LoadBusData();
                    UpdateStatistics();
                }
            }
            catch (ServiceException serviceEx)
            {
                ShowErrorMessage(serviceEx.Message);
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Returns CSS class based on bus status for visual indicators.
        /// </summary>
        private string GetStatusCssClass(string status)
        {
            switch (status)
            {
                case BusStatus.Operational:
                    return "status-operational";
                case BusStatus.InRepair:
                    return "status-repair";
                case BusStatus.Retired:
                    return "status-retired";
                case BusStatus.Reserve:
                    return "status-reserve";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Returns CSS class based on mileage category for visual indicators.
        /// </summary>
        private string GetMileageCssClass(string mileageCategory)
        {
            switch (mileageCategory)
            {
                case MileageCategory.High:
                    return "mileage-high";
                case MileageCategory.Medium:
                    return "mileage-medium";
                case MileageCategory.Low:
                    return "mileage-low";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Shows success message to user.
        /// </summary>
        private void ShowSuccessMessage(string message)
        {
            pnlSuccess.Visible = true;
            litSuccess.Text = message;
            pnlError.Visible = false;
        }

        /// <summary>
        /// Shows error message to user.
        /// </summary>
        private void ShowErrorMessage(string message)
        {
            pnlError.Visible = true;
            litError.Text = message;
            pnlSuccess.Visible = false;
        }

        #endregion

        #region Error Handling Methods

        /// <summary>
        /// Handles database-specific errors with appropriate user messaging.
        /// </summary>
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
        }

        /// <summary>
        /// Handles service layer errors with context-specific messaging.
        /// </summary>
        private void HandleServiceError(ServiceException ex)
        {
            pnlError.Visible = true;
            litError.Text = $"<strong>Service Error:</strong> {ex.Message}";
            
            if (ex.InnerException != null)
            {
                litError.Text += $"<br><small>Details: {ex.InnerException.Message}</small>";
            }
        }

        /// <summary>
        /// Handles generic unexpected errors with fallback messaging.
        /// </summary>
        private void HandleGenericError(Exception ex)
        {
            pnlError.Visible = true;
            litError.Text = $"<strong>Unexpected Error:</strong> An error occurred while processing your request. Please try again or contact system administrator.";
        }

        #endregion

        #region Cleanup

        /// <summary>
        /// Dispose pattern implementation for service layer cleanup.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_busService != null)
                {
                    _busService.Dispose();
                    _busService = null;
                }
            }
            base.Dispose(disposing);
        }

        #endregion
    }

    /// <summary>
    /// Sort direction enumeration for grid view sorting.
    /// </summary>
    public enum SortDirection
    {
        Ascending,
        Descending
    }
}