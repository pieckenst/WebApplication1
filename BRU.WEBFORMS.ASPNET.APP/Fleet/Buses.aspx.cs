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
    /// Provides CRUD operations, filtering, searching, sorting,
    /// pagination, and maintenance tracking.
    /// </summary>
    public partial class Buses : SecurePage
    {
        protected override string[] RequiredPermissions { get { return new[] { "bus.read" }; } }
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

        // txtSearch is directly on Buses.aspx, so it remains a normal page control.
        protected global::System.Web.UI.WebControls.TextBox txtSearch;
        protected global::System.Web.UI.WebControls.Button btnSearch;

        // ContentBox controls are directly on the page.
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbFilters;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbBusesList;

        protected global::System.Web.UI.WebControls.Literal litPagination;

        #endregion

        #region Private Fields

        private List<Bus> _currentBusList;

        private const int _pageSize = 20;

        #endregion

        #region State Properties

        /// <summary>
        /// Current page number. Stored in ViewState so it survives postbacks.
        /// </summary>
        private int CurrentPage
        {
            get
            {
                object value = ViewState["Buses_CurrentPage"];

                if (value == null)
                {
                    return 1;
                }

                int page;

                if (int.TryParse(value.ToString(), out page) && page > 0)
                {
                    return page;
                }

                return 1;
            }
            set
            {
                ViewState["Buses_CurrentPage"] = value < 1 ? 1 : value;
            }
        }

        /// <summary>
        /// Current sort expression. Stored in ViewState.
        /// </summary>
        private string CurrentSortExpression
        {
            get
            {
                object value = ViewState["Buses_SortExpression"];

                return value == null
                    ? "fleet_number"
                    : value.ToString();
            }
            set
            {
                ViewState["Buses_SortExpression"] =
                    string.IsNullOrEmpty(value) ? "fleet_number" : value;
            }
        }

        /// <summary>
        /// Current sort direction. Stored in ViewState.
        /// </summary>
        private SortDirection CurrentSortDirection
        {
            get
            {
                object value = ViewState["Buses_SortDirection"];

                if (value == null)
                {
                    return SortDirection.Ascending;
                }

                int direction;

                if (int.TryParse(value.ToString(), out direction))
                {
                    return (SortDirection)direction;
                }

                return SortDirection.Ascending;
            }
            set
            {
                ViewState["Buses_SortDirection"] = (int)value;
            }
        }

        #endregion

        #region ContentBox Control Accessors

        /// <summary>
        /// Gets the status filter located inside cbFilters ContentTemplate.
        /// </summary>
        private DropDownList DdlStatusFilter
        {
            get
            {
                return cbFilters.FindContentControl<DropDownList>("ddlStatusFilter");
            }
        }

        /// <summary>
        /// Gets the manufacturer filter located inside cbFilters ContentTemplate.
        /// </summary>
        private DropDownList DdlManufacturerFilter
        {
            get
            {
                return cbFilters.FindContentControl<DropDownList>("ddlManufacturerFilter");
            }
        }

        /// <summary>
        /// Gets the "from year" textbox located inside cbFilters ContentTemplate.
        /// </summary>
        private TextBox TxtYearFrom
        {
            get
            {
                return cbFilters.FindContentControl<TextBox>("txtYearFrom");
            }
        }

        /// <summary>
        /// Gets the "to year" textbox located inside cbFilters ContentTemplate.
        /// </summary>
        private TextBox TxtYearTo
        {
            get
            {
                return cbFilters.FindContentControl<TextBox>("txtYearTo");
            }
        }

        /// <summary>
        /// Gets the Apply Year Filter button located inside cbFilters ContentTemplate.
        /// </summary>
        private Button BtnApplyYearFilter
        {
            get
            {
                return cbFilters.FindContentControl<Button>("btnApplyYearFilter");
            }
        }

        /// <summary>
        /// Gets the GridView located inside cbBusesList ContentTemplate.
        /// </summary>
        private GridView GvBuses
        {
            get
            {
                return cbBusesList.FindContentControl<GridView>("gvBuses");
            }
        }

        /// <summary>
        /// Validates that all controls required from the ContentBox templates
        /// were successfully created.
        /// </summary>
        private void ValidateTemplateControls()
        {
            if (cbFilters == null)
            {
                throw new InvalidOperationException(
                    "The cbFilters ContentBox control was not created.");
            }

            if (cbBusesList == null)
            {
                throw new InvalidOperationException(
                    "The cbBusesList ContentBox control was not created.");
            }

            if (DdlStatusFilter == null)
            {
                throw new InvalidOperationException(
                    "Control 'ddlStatusFilter' was not found inside cbFilters.");
            }

            if (DdlManufacturerFilter == null)
            {
                throw new InvalidOperationException(
                    "Control 'ddlManufacturerFilter' was not found inside cbFilters.");
            }

            if (TxtYearFrom == null)
            {
                throw new InvalidOperationException(
                    "Control 'txtYearFrom' was not found inside cbFilters.");
            }

            if (TxtYearTo == null)
            {
                throw new InvalidOperationException(
                    "Control 'txtYearTo' was not found inside cbFilters.");
            }

            if (GvBuses == null)
            {
                throw new InvalidOperationException(
                    "Control 'gvBuses' was not found inside cbBusesList.");
            }
        }

        #endregion

        #region Page Lifecycle

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    CurrentPage = 1;
                    CurrentSortExpression = "fleet_number";
                    CurrentSortDirection = SortDirection.Ascending;
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

        #region Initialization

        private void InitializeFilters()
        {
            try
            {
                ValidateTemplateControls();

                if (DdlStatusFilter.Items.Count > 0)
                {
                    DdlStatusFilter.SelectedIndex = 0;
                }

                if (DdlManufacturerFilter.Items.Count > 0)
                {
                    DdlManufacturerFilter.SelectedIndex = 0;
                }

                TxtYearFrom.Text = string.Empty;
                TxtYearTo.Text = string.Empty;

                if (txtSearch != null)
                {
                    txtSearch.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw new ServiceException(
                    "Error initializing filters", ex);
            }
        }

        private void LoadManufacturerOptions()
        {
            try
            {
                ValidateTemplateControls();

                using (BusService busService = new BusService())
                {
                    List<Bus> allBuses = busService.GetAllBuses();

                    DdlManufacturerFilter.Items.Clear();
                    DdlManufacturerFilter.Items.Add(
                        new ListItem("All Manufacturers", ""));

                    if (allBuses == null)
                    {
                        return;
                    }

                    List<string> manufacturers = allBuses
                        .Select(b => b.Manufacturer)
                        .Where(m => !string.IsNullOrEmpty(m))
                        .Distinct()
                        .OrderBy(m => m)
                        .ToList();

                    foreach (string manufacturer in manufacturers)
                    {
                        DdlManufacturerFilter.Items.Add(
                            new ListItem(manufacturer, manufacturer));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ServiceException(
                    "Error loading manufacturer options", ex);
            }
        }

        #endregion

        #region Data Loading

        private void LoadBusData()
        {
            try
            {
                ValidateTemplateControls();

                using (BusService busService = new BusService())
                {
                    List<Bus> allBuses = busService.GetAllBuses();

                    if (allBuses == null)
                    {
                        allBuses = new List<Bus>();
                    }

                    _currentBusList = ApplyFilters(allBuses);
                    _currentBusList = ApplySorting(_currentBusList);

                    BindGridView();
                }
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException(
                    "Error loading bus data", ex);
            }
        }

        private List<Bus> ApplyFilters(List<Bus> buses)
        {
            try
            {
                List<Bus> filteredBuses =
                    new List<Bus>(buses ?? new List<Bus>());

                // Status filter
                if (DdlStatusFilter != null &&
                    !string.IsNullOrEmpty(DdlStatusFilter.SelectedValue))
                {
                    string selectedStatus =
                        DdlStatusFilter.SelectedValue;

                    filteredBuses = filteredBuses
                        .Where(b => b != null &&
                                    b.Status == selectedStatus)
                        .ToList();
                }

                // Manufacturer filter
                if (DdlManufacturerFilter != null &&
                    !string.IsNullOrEmpty(
                        DdlManufacturerFilter.SelectedValue))
                {
                    string selectedManufacturer =
                        DdlManufacturerFilter.SelectedValue;

                    filteredBuses = filteredBuses
                        .Where(b => b != null &&
                                    b.Manufacturer ==
                                    selectedManufacturer)
                        .ToList();
                }

                // Year filter
                if (TxtYearFrom != null &&
                    TxtYearTo != null &&
                    !string.IsNullOrEmpty(TxtYearFrom.Text) &&
                    !string.IsNullOrEmpty(TxtYearTo.Text))
                {
                    int yearFrom;
                    int yearTo;

                    if (int.TryParse(TxtYearFrom.Text, out yearFrom) &&
                        int.TryParse(TxtYearTo.Text, out yearTo))
                    {
                        if (yearFrom > yearTo)
                        {
                            int temp = yearFrom;
                            yearFrom = yearTo;
                            yearTo = temp;
                        }

                        filteredBuses = filteredBuses
                            .Where(b => b != null &&
                                        b.ManufactureYear >= yearFrom &&
                                        b.ManufactureYear <= yearTo)
                            .ToList();
                    }
                }

                // Search filter
                if (txtSearch != null &&
                    !string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    string searchTerm =
                        txtSearch.Text.Trim();

                    filteredBuses = filteredBuses
                        .Where(b =>
                            b != null &&
                            (
                                (!string.IsNullOrEmpty(b.FleetNumber) &&
                                 b.FleetNumber.IndexOf(
                                     searchTerm,
                                     StringComparison.OrdinalIgnoreCase) >= 0)

                                ||

                                (!string.IsNullOrEmpty(b.Model) &&
                                 b.Model.IndexOf(
                                     searchTerm,
                                     StringComparison.OrdinalIgnoreCase) >= 0)

                                ||

                                (!string.IsNullOrEmpty(b.RegistrationNum) &&
                                 b.RegistrationNum.IndexOf(
                                     searchTerm,
                                     StringComparison.OrdinalIgnoreCase) >= 0)
                            ))
                        .ToList();
                }

                return filteredBuses;
            }
            catch (Exception ex)
            {
                throw new ServiceException(
                    "Error applying filters", ex);
            }
        }

        private List<Bus> ApplySorting(List<Bus> buses)
        {
            try
            {
                if (buses == null)
                {
                    return new List<Bus>();
                }

                bool ascending =
                    CurrentSortDirection == SortDirection.Ascending;

                switch ((CurrentSortExpression ?? "")
                    .ToLowerInvariant())
                {
                    case "fleet_number":
                        return ascending
                            ? buses.OrderBy(b => b.FleetNumber).ToList()
                            : buses.OrderByDescending(
                                b => b.FleetNumber).ToList();

                    case "model":
                        return ascending
                            ? buses.OrderBy(b => b.Model).ToList()
                            : buses.OrderByDescending(
                                b => b.Model).ToList();

                    case "manufacturer":
                        return ascending
                            ? buses.OrderBy(
                                b => b.Manufacturer).ToList()
                            : buses.OrderByDescending(
                                b => b.Manufacturer).ToList();

                    case "manufacture_year":
                        return ascending
                            ? buses.OrderBy(
                                b => b.ManufactureYear).ToList()
                            : buses.OrderByDescending(
                                b => b.ManufactureYear).ToList();

                    case "status":
                        return ascending
                            ? buses.OrderBy(b => b.Status).ToList()
                            : buses.OrderByDescending(
                                b => b.Status).ToList();

                    case "mileage_km":
                        return ascending
                            ? buses.OrderBy(b => b.MileageKm).ToList()
                            : buses.OrderByDescending(
                                b => b.MileageKm).ToList();

                    case "bus_id":
                        return ascending
                            ? buses.OrderBy(b => b.BusId).ToList()
                            : buses.OrderByDescending(
                                b => b.BusId).ToList();

                    case "registration_num":
                        return ascending
                            ? buses.OrderBy(
                                b => b.RegistrationNum).ToList()
                            : buses.OrderByDescending(
                                b => b.RegistrationNum).ToList();

                    case "capacity":
                        return ascending
                            ? buses.OrderBy(b => b.Capacity).ToList()
                            : buses.OrderByDescending(
                                b => b.Capacity).ToList();

                    default:
                        return ascending
                            ? buses.OrderBy(
                                b => b.FleetNumber).ToList()
                            : buses.OrderByDescending(
                                b => b.FleetNumber).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new ServiceException(
                    "Error applying sorting", ex);
            }
        }

        private void BindGridView()
        {
            try
            {
                ValidateTemplateControls();

                if (_currentBusList == null)
                {
                    _currentBusList = new List<Bus>();
                }

                int totalItems = _currentBusList.Count;

                int totalPages =
                    totalItems == 0
                        ? 1
                        : (int)Math.Ceiling(
                            (double)totalItems / _pageSize);

                // Keep current page inside valid range.
                if (CurrentPage > totalPages)
                {
                    CurrentPage = totalPages;
                }

                if (CurrentPage < 1)
                {
                    CurrentPage = 1;
                }

                // Let GridView perform the actual page slicing.
                GvBuses.PageSize = _pageSize;
                GvBuses.PageIndex = CurrentPage - 1;
                GvBuses.DataSource = _currentBusList;
                GvBuses.DataBind();

                UpdatePagination(totalPages, totalItems);
            }
            catch (Exception ex)
            {
                throw new ServiceException(
                    "Error binding grid view", ex);
            }
        }

        private void UpdatePagination(int totalPages, int totalItems)
        {
            try
            {
                StringBuilder paginationHtml =
                    new StringBuilder();

                paginationHtml.Append("Showing ").Append(totalItems).Append(" buses");
                if (totalPages > 1)
                    paginationHtml.Append(" | Page ").Append(CurrentPage).Append(" of ").Append(totalPages);

                litPagination.Text =
                    paginationHtml.ToString();
            }
            catch (Exception ex)
            {
                throw new ServiceException(
                    "Error updating pagination", ex);
            }
        }

        private void UpdateStatistics()
        {
            try
            {
                using (BusService busService = new BusService())
                {
                    List<Bus> allBuses =
                        busService.GetAllBuses();

                    if (allBuses == null)
                    {
                        allBuses = new List<Bus>();
                    }

                    litTotalBuses.Text =
                        allBuses.Count.ToString();

                    litOperationalBuses.Text =
                        allBuses.Count(
                            b => b != null &&
                                 b.Status ==
                                 BusStatus.Operational)
                        .ToString();

                    litRepairBuses.Text =
                        allBuses.Count(
                            b => b != null &&
                                 b.Status ==
                                 BusStatus.InRepair)
                        .ToString();

                    List<Bus> busesNeedingAttention =
                        busService.GetBusesNeedingAttention();

                    litAttentionBuses.Text =
                        busesNeedingAttention == null
                            ? "0"
                            : busesNeedingAttention.Count.ToString();
                }
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException(
                    "Error updating statistics", ex);
            }
        }

        #endregion

        #region Event Handlers

        protected void btnAddBus_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                if (!RequireWritePermission("bus.write")) return;
                Response.Redirect(
                    "~/Fleet/BusEdit.aspx?mode=add");
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        protected void btnRefresh_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                CurrentPage = 1;

                LoadBusData();
                UpdateStatistics();

                ShowSuccessMessage(
                    "Data refreshed successfully.");
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        protected void btnExport_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                ShowSuccessMessage(
                    "Export functionality will be implemented.");
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        protected void btnSearch_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                CurrentPage = 1;
                LoadBusData();
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        protected void ddlStatusFilter_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            try
            {
                CurrentPage = 1;
                LoadBusData();
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        protected void ddlManufacturerFilter_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            try
            {
                CurrentPage = 1;
                LoadBusData();
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        protected void btnApplyYearFilter_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                CurrentPage = 1;
                LoadBusData();
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        protected void gvBuses_PageIndexChanging(
            object sender,
            GridViewPageEventArgs e)
        {
            try
            {
                CurrentPage = e.NewPageIndex + 1;

                // Reload the complete filtered/sorted list.
                // GridView will then display CurrentPage.
                LoadBusData();
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        protected void gvBuses_Sorting(
            object sender,
            GridViewSortEventArgs e)
        {
            try
            {
                if (string.Equals(
                    CurrentSortExpression,
                    e.SortExpression,
                    StringComparison.OrdinalIgnoreCase))
                {
                    CurrentSortDirection =
                        CurrentSortDirection ==
                        SortDirection.Ascending
                            ? SortDirection.Descending
                            : SortDirection.Ascending;
                }
                else
                {
                    CurrentSortExpression =
                        e.SortExpression;

                    CurrentSortDirection =
                        SortDirection.Ascending;
                }

                CurrentPage = 1;

                LoadBusData();
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        protected void gvBuses_RowCommand(
            object sender,
            GridViewCommandEventArgs e)
        {
            try
            {
                if (e == null ||
                    e.CommandArgument == null)
                {
                    return;
                }

                int busId =
                    Convert.ToInt32(e.CommandArgument);

                switch (e.CommandName)
                {
                    case "View":

                        Response.Redirect(
                            "~/Fleet/BusDetails.aspx?busId=" +
                            busId);

                        break;

                    case "Edit":

                        if (!RequireWritePermission("bus.write")) break;
                        Response.Redirect(
                            "~/Fleet/BusEdit.aspx?busId=" +
                            busId +
                            "&mode=edit");

                        break;

                    case "Maintenance":

                        Response.Redirect(
                            "~/Fleet/BusMaintenance.aspx?busId=" +
                            busId);

                        break;

                    case "Delete":

                        if (!RequireWritePermission("bus.write")) break;
                        DeleteBus(busId);

                        break;
                }
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        protected void gvBuses_RowDataBound(
            object sender,
            GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType !=
                    DataControlRowType.DataRow)
                {
                    return;
                }

                Bus bus =
                    e.Row.DataItem as Bus;

                if (bus == null)
                {
                    return;
                }

                Literal litStatus =
                    e.Row.FindControl(
                        "litStatus") as Literal;

                if (litStatus != null)
                {
                    litStatus.Text =
                        "<span class='" +
                        GetStatusCssClass(bus.Status) +
                        "'>" +
                        Server.HtmlEncode(bus.Status) +
                        "</span>";
                }

                Literal litMileageCategory =
                    e.Row.FindControl(
                        "litMileageCategory") as Literal;

                if (litMileageCategory != null)
                {
                    litMileageCategory.Text =
                        "<span class='" +
                        GetMileageCssClass(
                            bus.MileageCategory) +
                        "'>" +
                        Server.HtmlEncode(
                            bus.MileageCategory) +
                        "</span>";
                }
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        #endregion

        #region Business Logic

        private void DeleteBus(int busId)
        {
            try
            {
                RequireWritePermission("bus.write");
                using (BusService busService =
                    new BusService())
                {
                    busService.DeleteBus(busId);
                }

                ShowSuccessMessage(
                    "Bus deleted successfully.");

                if (CurrentPage < 1)
                {
                    CurrentPage = 1;
                }

                LoadBusData();
                UpdateStatistics();
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
                    return string.Empty;
            }
        }

        private string GetMileageCssClass(
            string mileageCategory)
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
                    return string.Empty;
            }
        }

        private void ShowSuccessMessage(
            string message)
        {
            pnlSuccess.Visible = true;
            litSuccess.Text = message;

            pnlError.Visible = false;
            litError.Text = string.Empty;
        }

        private void ShowErrorMessage(
            string message)
        {
            pnlError.Visible = true;
            litError.Text = message;

            pnlSuccess.Visible = false;
            litSuccess.Text = string.Empty;
        }

        #endregion

        #region Error Handling

        private void HandleDatabaseError(
            System.Data.SqlClient.SqlException ex)
        {
            pnlError.Visible = true;

            StringBuilder errorMessage =
                new StringBuilder();

            errorMessage.Append(
                "<strong>Database Connection Error:</strong> ");

            errorMessage.Append(
                "Unable to connect to the Autopark database. ");

            switch (ex.Number)
            {
                case 53:

                    errorMessage.Append(
                        "Database server is not available. " +
                        "Please check your connection settings.");

                    break;

                case 18456:

                    errorMessage.Append(
                        "Authentication failed. " +
                        "Please verify your database credentials.");

                    break;

                case 208:

                    errorMessage.Append(
                        "Database 'AutoparkDB' not found. " +
                        "Please run the database setup script.");

                    break;

                default:

                    errorMessage.Append(
                        "Error code: " +
                        ex.Number +
                        ". " +
                        ex.Message);

                    break;
            }

            litError.Text =
                errorMessage.ToString();

            pnlSuccess.Visible = false;
        }

        private void HandleServiceError(
            ServiceException ex)
        {
            pnlError.Visible = true;

            litError.Text =
                "<strong>Service Error:</strong> " +
                Server.HtmlEncode(ex.Message);

            if (ex.InnerException != null)
            {
                litError.Text +=
                    "<br><small>Details: " +
                    Server.HtmlEncode(
                        ex.InnerException.Message) +
                    "</small>";
            }

            pnlSuccess.Visible = false;
        }

        private void HandleGenericError(
            Exception ex)
        {
            pnlError.Visible = true;

            litError.Text =
                "<strong>Unexpected Error:</strong> " +
                "An error occurred while processing your request. " +
                "Please try again or contact system administrator.";

            pnlSuccess.Visible = false;
        }

        #endregion
    }

    /// <summary>
    /// Sort direction enumeration used by the Buses page.
    /// </summary>
    public enum SortDirection
    {
        Ascending,
        Descending
    }
}