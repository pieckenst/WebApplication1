using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BRU.WEBFORMS.ASPNET.APP.Controls;
using BRU.WEBFORMS.ASPNET.APP.Models;
using BRU.WEBFORMS.ASPNET.APP.Services;

namespace BRU.WEBFORMS.ASPNET.APP.Fleet
{
    /// <summary>
    /// Per-bus maintenance workspace. Every read and mutation is scoped to the
    /// selected bus; maintenance IDs received from the browser are revalidated
    /// against that bus before they can be viewed, edited, or deleted.
    /// </summary>
    public partial class BusMaintenance : SecurePage
    {
        protected override string[] RequiredPermissions
        {
            get { return new[] { "bus.read" }; }
        }

        #region Page-level controls

        protected global::System.Web.UI.WebControls.Literal litPageTitle;
        protected global::System.Web.UI.WebControls.Literal litHeading;
        protected global::System.Web.UI.WebControls.Panel pnlError;
        protected global::System.Web.UI.WebControls.Literal litError;
        protected global::System.Web.UI.WebControls.Panel pnlSuccess;
        protected global::System.Web.UI.WebControls.Literal litSuccess;
        protected global::System.Web.UI.WebControls.Panel pnlWarning;
        protected global::System.Web.UI.WebControls.Literal litWarning;
        protected global::System.Web.UI.HtmlControls.HtmlAnchor lnkBusEdit;
        protected global::System.Web.UI.WebControls.Button btnBack;
        protected global::System.Web.UI.WebControls.Button btnEditBus;
        protected global::System.Web.UI.WebControls.Button btnAddRecord;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbBusPicker;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbBusOverview;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbFilters;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbMaintenanceList;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbMaintenanceForm;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbMaintenanceDetail;

        protected global::System.Web.UI.WebControls.Literal litTotalRecords;
        protected global::System.Web.UI.WebControls.Literal litTotalCost;
        protected global::System.Web.UI.WebControls.Literal litLastService;
        protected global::System.Web.UI.WebControls.Literal litNextDue;

        #endregion

        #region Template controls

        protected global::System.Web.UI.WebControls.DropDownList ddlBusPicker;
        protected global::System.Web.UI.WebControls.Button btnOpenSelectedBus;
        protected global::System.Web.UI.WebControls.Literal litFleetNumber;
        protected global::System.Web.UI.WebControls.Literal litRegistration;
        protected global::System.Web.UI.WebControls.Literal litBusModel;
        protected global::System.Web.UI.WebControls.Literal litBusStatus;
        protected global::System.Web.UI.WebControls.Literal litBusMileage;
        protected global::System.Web.UI.WebControls.TextBox txtSearch;
        protected global::System.Web.UI.WebControls.Button btnSearch;
        protected global::System.Web.UI.WebControls.Button btnClearFilters;
        protected global::System.Web.UI.WebControls.DropDownList ddlRoadworthinessFilter;
        protected global::System.Web.UI.WebControls.GridView gvMaintenance;
        protected global::System.Web.UI.WebControls.Literal litPagination;

        protected global::System.Web.UI.WebControls.Panel pnlMaintenanceForm;
        protected global::System.Web.UI.WebControls.Label lblFormBus;
        protected global::System.Web.UI.WebControls.TextBox txtMaintenanceDate;
        protected global::System.Web.UI.WebControls.TextBox txtNextDate;
        protected global::System.Web.UI.WebControls.TextBox txtType;
        protected global::System.Web.UI.WebControls.TextBox txtFoundIssue;
        protected global::System.Web.UI.WebControls.TextBox txtServiceResult;
        protected global::System.Web.UI.WebControls.DropDownList ddlMechanic;
        protected global::System.Web.UI.WebControls.TextBox txtMileage;
        protected global::System.Web.UI.WebControls.DropDownList ddlRoadworthiness;
        protected global::System.Web.UI.WebControls.TextBox txtCost;
        protected global::System.Web.UI.WebControls.Button btnSave;
        protected global::System.Web.UI.WebControls.Button btnCancel;

        protected global::System.Web.UI.WebControls.Literal litDetail;
        protected global::System.Web.UI.WebControls.Button btnDetailEdit;
        protected global::System.Web.UI.WebControls.Button btnDetailClose;

        #endregion

        #region State

        private bool _templateControlsResolved;

        private int BusId
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["busId"], out id) && id > 0 ? id : 0;
            }
        }

        private int EditingMaintenanceId
        {
            get
            {
                object value = ViewState["BusMaintenance_EditingId"];
                return value == null ? 0 : Convert.ToInt32(value, CultureInfo.InvariantCulture);
            }
            set { ViewState["BusMaintenance_EditingId"] = value; }
        }

        private int SelectedMaintenanceId
        {
            get
            {
                object value = ViewState["BusMaintenance_SelectedId"];
                return value == null ? 0 : Convert.ToInt32(value, CultureInfo.InvariantCulture);
            }
            set { ViewState["BusMaintenance_SelectedId"] = value; }
        }

        private string CurrentSortExpression
        {
            get { return Convert.ToString(ViewState["BusMaintenance_Sort"] ?? "maintenance_date", CultureInfo.InvariantCulture); }
            set { ViewState["BusMaintenance_Sort"] = value; }
        }

        private string CurrentSortDirection
        {
            get { return Convert.ToString(ViewState["BusMaintenance_SortDirection"] ?? "DESC", CultureInfo.InvariantCulture); }
            set { ViewState["BusMaintenance_SortDirection"] = value == "ASC" ? "ASC" : "DESC"; }
        }

        #endregion

        #region Lifecycle and control resolution

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureTemplateControlsResolved();

                if (!IsPostBack)
                    InitializePage();
            }
            catch (SqlException ex)
            {
                LogException(ex, "Bus maintenance database error");
                ShowError("A database error prevented the maintenance workspace from loading. Check the application log for details.");
            }
            catch (ServiceException ex)
            {
                LogException(ex, "Bus maintenance service error");
                ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                LogException(ex, "Bus maintenance initialization error");
                ShowError("The maintenance workspace could not be initialized. Check the application log for details.");
            }
        }

        private void EnsureTemplateControlsResolved()
        {
            if (_templateControlsResolved)
                return;

            EnsureContentBox(cbBusPicker, "cbBusPicker");
            EnsureContentBox(cbBusOverview, "cbBusOverview");
            EnsureContentBox(cbFilters, "cbFilters");
            EnsureContentBox(cbMaintenanceList, "cbMaintenanceList");
            EnsureContentBox(cbMaintenanceForm, "cbMaintenanceForm");
            EnsureContentBox(cbMaintenanceDetail, "cbMaintenanceDetail");

            ddlBusPicker = FindRequired<DropDownList>(cbBusPicker, "ddlBusPicker");
            btnOpenSelectedBus = FindRequired<Button>(cbBusPicker, "btnOpenSelectedBus");

            litFleetNumber = FindRequired<Literal>(cbBusOverview, "litFleetNumber");
            litRegistration = FindRequired<Literal>(cbBusOverview, "litRegistration");
            litBusModel = FindRequired<Literal>(cbBusOverview, "litBusModel");
            litBusStatus = FindRequired<Literal>(cbBusOverview, "litBusStatus");
            litBusMileage = FindRequired<Literal>(cbBusOverview, "litBusMileage");

            txtSearch = FindRequired<TextBox>(cbFilters, "txtSearch");
            btnSearch = FindRequired<Button>(cbFilters, "btnSearch");
            btnClearFilters = FindRequired<Button>(cbFilters, "btnClearFilters");
            ddlRoadworthinessFilter = FindRequired<DropDownList>(cbFilters, "ddlRoadworthinessFilter");

            gvMaintenance = FindRequired<GridView>(cbMaintenanceList, "gvMaintenance");
            litPagination = FindRequired<Literal>(cbMaintenanceList, "litPagination");

            pnlMaintenanceForm = FindRequired<Panel>(cbMaintenanceForm, "pnlMaintenanceForm");
            lblFormBus = FindRequired<Label>(cbMaintenanceForm, "lblFormBus");
            txtMaintenanceDate = FindRequired<TextBox>(cbMaintenanceForm, "txtMaintenanceDate");
            txtNextDate = FindRequired<TextBox>(cbMaintenanceForm, "txtNextDate");
            txtType = FindRequired<TextBox>(cbMaintenanceForm, "txtType");
            txtFoundIssue = FindRequired<TextBox>(cbMaintenanceForm, "txtFoundIssue");
            txtServiceResult = FindRequired<TextBox>(cbMaintenanceForm, "txtServiceResult");
            ddlMechanic = FindRequired<DropDownList>(cbMaintenanceForm, "ddlMechanic");
            txtMileage = FindRequired<TextBox>(cbMaintenanceForm, "txtMileage");
            ddlRoadworthiness = FindRequired<DropDownList>(cbMaintenanceForm, "ddlRoadworthiness");
            txtCost = FindRequired<TextBox>(cbMaintenanceForm, "txtCost");
            btnSave = FindRequired<Button>(cbMaintenanceForm, "btnSave");
            btnCancel = FindRequired<Button>(cbMaintenanceForm, "btnCancel");

            litDetail = FindRequired<Literal>(cbMaintenanceDetail, "litDetail");
            btnDetailEdit = FindRequired<Button>(cbMaintenanceDetail, "btnDetailEdit");
            btnDetailClose = FindRequired<Button>(cbMaintenanceDetail, "btnDetailClose");

            _templateControlsResolved = true;
        }

        private static void EnsureContentBox(ContentBox box, string id)
        {
            if (box == null)
                throw new InvalidOperationException("Required ContentBox '" + id + "' was not created.");
        }

        private static T FindRequired<T>(ContentBox box, string id) where T : Control
        {
            T control = box.FindContentControl<T>(id);
            if (control == null)
                throw new InvalidOperationException("Required control '" + id + "' was not found inside ContentBox '" + box.ID + "'.");
            return control;
        }

        private void InitializePage()
        {
            if (BusId <= 0)
            {
                ShowBusPicker();
                return;
            }

            Bus bus;
            using (BusService service = new BusService())
                bus = service.GetBusById(BusId);

            cbBusPicker.Visible = false;
            cbBusOverview.Visible = true;
            cbFilters.Visible = true;
            cbMaintenanceList.Visible = true;
            btnEditBus.Visible = true;
            btnAddRecord.Visible = true;
            lnkBusEdit.HRef = ResolveUrl("~/Fleet/BusEdit.aspx?busId=" + BusId.ToString(CultureInfo.InvariantCulture) + "&mode=edit");

            litPageTitle.Text = HttpUtility.HtmlEncode(bus.FleetNumber);
            litHeading.Text = HttpUtility.HtmlEncode(Localization.Get("BusMaintenance_Heading") + " — " + bus.FleetNumber);
            cbBusOverview.HeaderText = Localization.Get("BusMaintenance_BusInfo") + " — " + bus.FleetNumber;

            litFleetNumber.Text = HttpUtility.HtmlEncode(bus.FleetNumber);
            litRegistration.Text = HttpUtility.HtmlEncode(bus.RegistrationNum);
            litBusModel.Text = HttpUtility.HtmlEncode((bus.Manufacturer ?? string.Empty) + " " + (bus.Model ?? string.Empty) + " (" + bus.ManufactureYear.ToString(CultureInfo.InvariantCulture) + ")");
            litBusStatus.Text = HttpUtility.HtmlEncode(bus.Status);
            litBusMileage.Text = bus.MileageKm.ToString("N0", Localization.Culture) + " km";

            LoadMechanics();
            LoadStatistics();
            BindGrid();

            if (Request.QueryString["saved"] == "1")
                ShowSuccess(Localization.Get("BusMaintenance_Saved"));
            else if (Request.QueryString["deleted"] == "1")
                ShowSuccess(Localization.Get("BusMaintenance_Deleted"));

            int maintenanceId;
            if (int.TryParse(Request.QueryString["maintenanceId"], out maintenanceId) && maintenanceId > 0)
            {
                string mode = (Request.QueryString["mode"] ?? string.Empty).Trim().ToLowerInvariant();
                if (mode == "edit")
                {
                    if (HasWritePermission("bus.write"))
                        ShowEditForm(maintenanceId);
                    else
                        ShowWarning("You can view maintenance history, but you do not have permission to edit records.");
                }
                else if (mode == "view")
                {
                    ShowMaintenanceDetail(maintenanceId);
                }
            }
            else if (string.Equals(Request.QueryString["mode"], "add", StringComparison.OrdinalIgnoreCase))
            {
                if (HasWritePermission("bus.write"))
                    OpenNewMaintenanceForm();
                else
                    ShowWarning("You can view maintenance history, but you do not have permission to add records.");
            }
        }

        private void ShowBusPicker()
        {
            cbBusPicker.Visible = true;
            cbBusOverview.Visible = false;
            cbFilters.Visible = false;
            cbMaintenanceList.Visible = false;
            cbMaintenanceForm.Visible = false;
            cbMaintenanceDetail.Visible = false;
            btnEditBus.Visible = false;
            btnAddRecord.Visible = false;
            litHeading.Text = HttpUtility.HtmlEncode(Localization.Get("BusMaintenance_Heading"));
            litPageTitle.Text = HttpUtility.HtmlEncode(Localization.Get("BusMaintenance_Heading"));

            using (BusService service = new BusService())
            {
                ddlBusPicker.Items.Clear();
                ddlBusPicker.Items.Add(new ListItem("-- Select bus --", ""));
                foreach (Bus bus in service.GetAllBuses().OrderBy(x => x.FleetNumber))
                    ddlBusPicker.Items.Add(new ListItem(bus.FleetNumber + " — " + bus.Model, bus.BusId.ToString(CultureInfo.InvariantCulture)));
            }

            if (!string.IsNullOrWhiteSpace(Request.QueryString["busId"]))
                ShowError(Localization.Get("BusMaintenance_InvalidId"));
        }

        private void LoadMechanics()
        {
            string selected = ddlMechanic.SelectedValue;
            ddlMechanic.Items.Clear();
            ddlMechanic.Items.Add(new ListItem("-- None assigned --", ""));

            using (EmployeeService service = new EmployeeService())
            {
                foreach (Employee mechanic in service.GetMechanics().OrderBy(x => x.EmployeeName))
                    ddlMechanic.Items.Add(new ListItem(mechanic.EmployeeName, mechanic.EmployeeId.ToString(CultureInfo.InvariantCulture)));
            }

            if (selected.Length > 0 && ddlMechanic.Items.FindByValue(selected) != null)
                ddlMechanic.SelectedValue = selected;
        }

        #endregion

        #region Filtered history and statistics

        private List<Maintenance> GetFilteredRows()
        {
            List<Maintenance> rows;
            using (MaintenanceService service = new MaintenanceService())
                rows = service.GetMaintenanceByBus(BusId);

            if (rows == null)
                rows = new List<Maintenance>();

            string search = (txtSearch.Text ?? string.Empty).Trim();
            if (search.Length > 0)
            {
                rows = rows.Where(x =>
                    ContainsIgnoreCase(x.MaintenanceType, search) ||
                    ContainsIgnoreCase(x.FoundIssue, search) ||
                    ContainsIgnoreCase(x.ServiceResult, search) ||
                    x.MaintenanceId.ToString(CultureInfo.InvariantCulture).Contains(search)).ToList();
            }

            string status = ddlRoadworthinessFilter.SelectedValue;
            if (!string.IsNullOrWhiteSpace(status))
                rows = rows.Where(x => string.Equals(x.Roadworthiness, status, StringComparison.OrdinalIgnoreCase)).ToList();

            bool ascending = CurrentSortDirection == "ASC";
            switch (CurrentSortExpression)
            {
                case "maintenance_id":
                    rows = ascending ? rows.OrderBy(x => x.MaintenanceId).ToList() : rows.OrderByDescending(x => x.MaintenanceId).ToList();
                    break;
                case "maintenance_type":
                    rows = ascending ? rows.OrderBy(x => x.MaintenanceType).ToList() : rows.OrderByDescending(x => x.MaintenanceType).ToList();
                    break;
                case "mileage_km":
                    rows = ascending ? rows.OrderBy(x => x.MileageKm).ToList() : rows.OrderByDescending(x => x.MileageKm).ToList();
                    break;
                case "roadworthiness":
                    rows = ascending ? rows.OrderBy(x => x.Roadworthiness).ToList() : rows.OrderByDescending(x => x.Roadworthiness).ToList();
                    break;
                case "maintenance_cost":
                    rows = ascending ? rows.OrderBy(x => x.MaintenanceCost).ToList() : rows.OrderByDescending(x => x.MaintenanceCost).ToList();
                    break;
                case "next_maintenance_date":
                    rows = ascending ? rows.OrderBy(x => x.NextMaintenanceDate).ToList() : rows.OrderByDescending(x => x.NextMaintenanceDate).ToList();
                    break;
                case "days_from_last_service":
                    rows = ascending ? rows.OrderBy(x => x.DaysFromLastService).ToList() : rows.OrderByDescending(x => x.DaysFromLastService).ToList();
                    break;
                default:
                    rows = ascending ? rows.OrderBy(x => x.MaintenanceDate).ToList() : rows.OrderByDescending(x => x.MaintenanceDate).ToList();
                    break;
            }

            return rows;
        }

        private void BindGrid()
        {
            if (BusId <= 0 || gvMaintenance == null)
                return;

            List<Maintenance> rows = GetFilteredRows();
            int totalPages = Math.Max(1, (int)Math.Ceiling(rows.Count / (double)gvMaintenance.PageSize));
            if (gvMaintenance.PageIndex >= totalPages)
                gvMaintenance.PageIndex = totalPages - 1;

            gvMaintenance.DataSource = rows;
            gvMaintenance.DataBind();

            litPagination.Text = "<span class='text-small'>Total: " +
                rows.Count.ToString(CultureInfo.InvariantCulture) + " records</span>";
        }

        private void LoadStatistics()
        {
            List<Maintenance> allRows;
            using (MaintenanceService service = new MaintenanceService())
                allRows = service.GetMaintenanceByBus(BusId);
            if (allRows == null)
                allRows = new List<Maintenance>();

            litTotalRecords.Text = allRows.Count.ToString(CultureInfo.InvariantCulture);
            litTotalCost.Text = allRows.Sum(x => x.MaintenanceCost).ToString("N2", Localization.Culture);

            Maintenance latest = allRows.OrderByDescending(x => x.MaintenanceDate).FirstOrDefault();
            litLastService.Text = latest == null ? "—" : latest.MaintenanceDate.ToString("dd.MM.yyyy", Localization.Culture);

            Maintenance due = allRows.Where(x => x.NextMaintenanceDate.HasValue)
                .OrderBy(x => x.NextMaintenanceDate.Value).FirstOrDefault();
            if (due == null)
            {
                litNextDue.Text = "—";
            }
            else
            {
                string date = due.NextMaintenanceDate.Value.ToString("dd.MM.yyyy", Localization.Culture);
                if (due.NextMaintenanceDate.Value.Date < DateTime.Today)
                {
                    litNextDue.Text = "<span class='overdue'>" + HttpUtility.HtmlEncode(date + " — " + Localization.Get("BusMaintenance_Overdue")) + "</span>";
                    ShowWarning("The next scheduled maintenance date has passed. Review the service plan before returning this bus to operational use.");
                }
                else
                {
                    litNextDue.Text = HttpUtility.HtmlEncode(date);
                }
            }
        }

        private static bool ContainsIgnoreCase(string value, string search)
        {
            return !string.IsNullOrEmpty(value) && value.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvMaintenance.PageIndex = 0;
            BindGrid();
        }

        protected void btnClearFilters_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            ddlRoadworthinessFilter.SelectedIndex = 0;
            gvMaintenance.PageIndex = 0;
            BindGrid();
        }

        protected void ddlRoadworthinessFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvMaintenance.PageIndex = 0;
            BindGrid();
        }

        protected void gvMaintenance_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            e.Cancel = true;
            gvMaintenance.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvMaintenance_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (string.Equals(CurrentSortExpression, e.SortExpression, StringComparison.OrdinalIgnoreCase))
                CurrentSortDirection = CurrentSortDirection == "ASC" ? "DESC" : "ASC";
            else
            {
                CurrentSortExpression = e.SortExpression;
                CurrentSortDirection = "ASC";
            }

            gvMaintenance.PageIndex = 0;
            BindGrid();
        }

        protected void gvMaintenance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            Maintenance record = e.Row.DataItem as Maintenance;
            if (record == null)
                return;

            Literal rw = e.Row.FindControl("litRw") as Literal;
            if (rw != null)
            {
                string css = record.Roadworthiness == RoadworthinessStatus.NotOperational ? "rw-notoperational" :
                    record.Roadworthiness == RoadworthinessStatus.NeedsAttention ? "rw-attention" : "rw-operational";
                rw.Text = "<span class='" + css + "'>" + HttpUtility.HtmlEncode(record.Roadworthiness ?? string.Empty) + "</span>";
            }

            Literal cost = e.Row.FindControl("litCost") as Literal;
            if (cost != null)
            {
                string css = record.MaintenanceCost >= 10000m ? "cost-high" :
                    record.MaintenanceCost >= 2000m ? "cost-medium" : "cost-low";
                cost.Text = "<span class='" + css + "'>" + record.MaintenanceCost.ToString("N2", Localization.Culture) + "</span>";
            }

            Literal next = e.Row.FindControl("litNextDate") as Literal;
            if (next != null)
            {
                if (!record.NextMaintenanceDate.HasValue)
                    next.Text = "—";
                else
                {
                    string date = record.NextMaintenanceDate.Value.ToString("dd.MM.yyyy", Localization.Culture);
                    bool overdue = record.NextMaintenanceDate.Value.Date < DateTime.Today;
                    next.Text = overdue
                        ? "<span class='overdue'>" + HttpUtility.HtmlEncode(date + " — " + Localization.Get("BusMaintenance_Overdue")) + "</span>"
                        : HttpUtility.HtmlEncode(date);
                }
            }
        }

        #endregion

        #region Record commands

        protected void gvMaintenance_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e == null || e.CommandArgument == null)
                return;

            int maintenanceId;
            if (!int.TryParse(Convert.ToString(e.CommandArgument, CultureInfo.InvariantCulture), out maintenanceId) || maintenanceId <= 0)
                return;

            switch (e.CommandName)
            {
                case "ViewRecord":
                    ShowMaintenanceDetail(maintenanceId);
                    break;
                case "EditRecord":
                    if (!RequireWritePermission("bus.write"))
                        return;
                    ShowEditForm(maintenanceId);
                    break;
                case "DeleteRecord":
                    DeleteMaintenance(maintenanceId);
                    break;
                default:
                    // Ignore built-in GridView commands and unrelated postbacks.
                    break;
            }
        }

        private Maintenance FindRecordForCurrentBus(int maintenanceId)
        {
            using (MaintenanceService service = new MaintenanceService())
            {
                List<Maintenance> rows = service.GetMaintenanceByBus(BusId);
                return rows == null ? null : rows.FirstOrDefault(x => x.MaintenanceId == maintenanceId);
            }
        }

        private void ShowMaintenanceDetail(int maintenanceId)
        {
            try
            {
                Maintenance record = FindRecordForCurrentBus(maintenanceId);
                if (record == null)
                {
                    ShowError(Localization.Get("BusMaintenance_NotFound"));
                    return;
                }

                SelectedMaintenanceId = record.MaintenanceId;
                StringBuilder html = new StringBuilder();
                html.Append("<div class='detail-row'><span class='detail-label'>Record ID:</span> ").Append(record.MaintenanceId).Append("</div>");
                html.Append("<div class='detail-row'><span class='detail-label'>Date:</span> ").Append(HttpUtility.HtmlEncode(record.MaintenanceDate.ToString("dd.MM.yyyy", Localization.Culture))).Append("</div>");
                html.Append("<div class='detail-row'><span class='detail-label'>Type:</span> ").Append(HttpUtility.HtmlEncode(record.MaintenanceType ?? string.Empty)).Append("</div>");
                html.Append("<div class='detail-row'><span class='detail-label'>Found issue:</span> ").Append(HttpUtility.HtmlEncode(string.IsNullOrWhiteSpace(record.FoundIssue) ? "—" : record.FoundIssue)).Append("</div>");
                html.Append("<div class='detail-row'><span class='detail-label'>Service result:</span> ").Append(HttpUtility.HtmlEncode(string.IsNullOrWhiteSpace(record.ServiceResult) ? "—" : record.ServiceResult)).Append("</div>");
                html.Append("<div class='detail-row'><span class='detail-label'>Mileage:</span> ").Append(record.MileageKm.HasValue ? record.MileageKm.Value.ToString("N0", Localization.Culture) + " km" : "—").Append("</div>");
                html.Append("<div class='detail-row'><span class='detail-label'>Roadworthiness:</span> ").Append(HttpUtility.HtmlEncode(record.Roadworthiness ?? string.Empty)).Append("</div>");
                html.Append("<div class='detail-row'><span class='detail-label'>Next service:</span> ").Append(record.NextMaintenanceDate.HasValue ? HttpUtility.HtmlEncode(record.NextMaintenanceDate.Value.ToString("dd.MM.yyyy", Localization.Culture)) : "—").Append("</div>");
                html.Append("<div class='detail-row'><span class='detail-label'>Cost:</span> ").Append(record.MaintenanceCost.ToString("N2", Localization.Culture)).Append("</div>");

                litDetail.Text = html.ToString();
                cbMaintenanceDetail.HeaderText = Localization.Get("BusMaintenance_PreviousRecords") + " — #" + record.MaintenanceId.ToString(CultureInfo.InvariantCulture);
                cbMaintenanceDetail.Visible = true;
                cbMaintenanceForm.Visible = false;
                ScrollToControl(btnDetailClose, "scrollToMaintenanceDetail");
            }
            catch (Exception ex)
            {
                LogException(ex, "Reading maintenance detail failed");
                ShowError("The maintenance record could not be loaded.");
            }
        }

        protected void btnDetailEdit_Click(object sender, EventArgs e)
        {
            if (!RequireWritePermission("bus.write"))
                return;
            if (SelectedMaintenanceId > 0)
                ShowEditForm(SelectedMaintenanceId);
        }

        private void ShowEditForm(int maintenanceId)
        {
            try
            {
                Maintenance record = FindRecordForCurrentBus(maintenanceId);
                if (record == null)
                {
                    ShowError(Localization.Get("BusMaintenance_NotFound"));
                    return;
                }

                EditingMaintenanceId = record.MaintenanceId;
                LoadMechanics();
                lblFormBus.Text = HttpUtility.HtmlEncode(GetBusDisplayText());
                txtMaintenanceDate.Text = record.MaintenanceDate.ToString("dd.MM.yyyy", Localization.Culture);
                txtNextDate.Text = record.NextMaintenanceDate.HasValue ? record.NextMaintenanceDate.Value.ToString("dd.MM.yyyy", Localization.Culture) : string.Empty;
                txtType.Text = record.MaintenanceType ?? string.Empty;
                txtFoundIssue.Text = record.FoundIssue ?? string.Empty;
                txtServiceResult.Text = record.ServiceResult ?? string.Empty;
                txtMileage.Text = record.MileageKm.HasValue ? record.MileageKm.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                txtCost.Text = record.MaintenanceCost.ToString("0.00", Localization.Culture);
                if (ddlMechanic.Items.FindByValue(record.EmployeeId.HasValue ? record.EmployeeId.Value.ToString(CultureInfo.InvariantCulture) : string.Empty) != null)
                    ddlMechanic.SelectedValue = record.EmployeeId.HasValue ? record.EmployeeId.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                if (ddlRoadworthiness.Items.FindByValue(record.Roadworthiness) != null)
                    ddlRoadworthiness.SelectedValue = record.Roadworthiness;

                cbMaintenanceForm.HeaderText = Localization.Get("BusMaintenance_EditHeading");
                cbMaintenanceForm.Visible = true;
                cbMaintenanceDetail.Visible = false;
                ShowWarning(string.Empty);
                ScrollToControl(btnSave, "scrollToMaintenanceForm");
            }
            catch (Exception ex)
            {
                LogException(ex, "Opening maintenance editor failed");
                ShowError("The maintenance record could not be opened for editing.");
            }
        }

        protected void btnAddRecord_Click(object sender, EventArgs e)
        {
            if (BusId <= 0)
            {
                ShowBusPicker();
                return;
            }
            if (!RequireWritePermission("bus.write"))
                return;

            OpenNewMaintenanceForm();
        }

        private void OpenNewMaintenanceForm()
        {
            EditingMaintenanceId = 0;
            SelectedMaintenanceId = 0;
            ClearForm();
            lblFormBus.Text = HttpUtility.HtmlEncode(GetBusDisplayText());
            cbMaintenanceForm.HeaderText = Localization.Get("BusMaintenance_AddHeading");
            cbMaintenanceForm.Visible = true;
            cbMaintenanceDetail.Visible = false;
            ScrollToControl(btnSave, "scrollToMaintenanceForm");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate("MaintenanceForm");
            if (!Page.IsValid)
                return;
            if (!RequireWritePermission("bus.write"))
                return;

            try
            {
                if (BusId <= 0)
                {
                    ShowError(Localization.Get("BusMaintenance_InvalidId"));
                    return;
                }

                Bus bus;
                using (BusService busService = new BusService())
                    bus = busService.GetBusById(BusId);

                Maintenance record = new Maintenance
                {
                    MaintenanceId = EditingMaintenanceId,
                    BusId = BusId,
                    MaintenanceType = (txtType.Text ?? string.Empty).Trim(),
                    FoundIssue = string.IsNullOrWhiteSpace(txtFoundIssue.Text) ? null : txtFoundIssue.Text.Trim(),
                    ServiceResult = string.IsNullOrWhiteSpace(txtServiceResult.Text) ? null : txtServiceResult.Text.Trim(),
                    Roadworthiness = ddlRoadworthiness.SelectedValue
                };

                DateTime maintenanceDate;
                if (!DateTime.TryParseExact(txtMaintenanceDate.Text.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out maintenanceDate))
                {
                    ShowError("Maintenance date must use dd.MM.yyyy format.");
                    return;
                }
                record.MaintenanceDate = maintenanceDate;

                if (!string.IsNullOrWhiteSpace(txtNextDate.Text))
                {
                    DateTime nextDate;
                    if (!DateTime.TryParseExact(txtNextDate.Text.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out nextDate))
                    {
                        ShowError("Next maintenance date must use dd.MM.yyyy format.");
                        return;
                    }
                    record.NextMaintenanceDate = nextDate;
                }

                if (!string.IsNullOrWhiteSpace(txtMileage.Text))
                {
                    int mileage;
                    if (!int.TryParse(txtMileage.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out mileage) || mileage < 0 || mileage > 9999999)
                    {
                        ShowError("Mileage must be a whole number between 0 and 9,999,999 km.");
                        return;
                    }
                    record.MileageKm = mileage;
                }

                if (ddlMechanic.SelectedValue.Length > 0)
                {
                    int employeeId;
                    if (!int.TryParse(ddlMechanic.SelectedValue, out employeeId) || employeeId <= 0)
                    {
                        ShowError("Select a valid mechanic or leave the field unassigned.");
                        return;
                    }
                    record.EmployeeId = employeeId;
                }

                decimal cost;
                if (!decimal.TryParse(txtCost.Text.Trim(), NumberStyles.Number, Localization.Culture, out cost) &&
                    !decimal.TryParse(txtCost.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out cost))
                {
                    ShowError("Enter a valid maintenance cost.");
                    return;
                }
                if (cost < 0)
                {
                    ShowError("Maintenance cost cannot be negative.");
                    return;
                }
                record.MaintenanceCost = cost;

                if (EditingMaintenanceId > 0 && FindRecordForCurrentBus(EditingMaintenanceId) == null)
                {
                    ShowError(Localization.Get("BusMaintenance_NotFound"));
                    return;
                }

                using (MaintenanceService service = new MaintenanceService())
                {
                    if (EditingMaintenanceId > 0)
                    {
                        if (!service.UpdateMaintenance(record))
                        {
                            ShowError(Localization.Get("BusMaintenance_NotFound"));
                            return;
                        }
                    }
                    else
                    {
                        record.MaintenanceId = service.CreateMaintenance(record);
                    }
                }

                LogInformation((EditingMaintenanceId > 0 ? "Maintenance record updated" : "Maintenance record created") +
                    ": busId=" + BusId + ", maintenanceId=" + record.MaintenanceId +
                    ", roadworthiness=" + record.Roadworthiness);

                if (record.Roadworthiness == RoadworthinessStatus.NotOperational && bus.Status != BusStatus.Retired && bus.Status != BusStatus.InRepair)
                    ShowWarning("The maintenance record marks this bus as not roadworthy. The fleet status has not been changed automatically; review the bus status in the bus editor.");

                RedirectTo("~/Fleet/BusMaintenance.aspx?busId=" + BusId.ToString(CultureInfo.InvariantCulture) + "&saved=1");
            }
            catch (SqlException ex)
            {
                LogException(ex, "Saving maintenance record failed");
                ShowError(GetDatabaseErrorMessage(ex));
            }
            catch (ServiceException ex)
            {
                LogException(ex, "Saving maintenance record failed validation");
                ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                LogException(ex, "Saving maintenance record failed");
                ShowError("The maintenance record could not be saved. Check the values and application log.");
            }
        }

        private void DeleteMaintenance(int maintenanceId)
        {
            if (!RequireWritePermission("bus.write"))
                return;

            try
            {
                Maintenance existing = FindRecordForCurrentBus(maintenanceId);
                if (existing == null)
                {
                    ShowError(Localization.Get("BusMaintenance_NotFound"));
                    return;
                }

                using (MaintenanceService service = new MaintenanceService())
                {
                    if (!service.DeleteMaintenance(maintenanceId))
                    {
                        ShowError(Localization.Get("BusMaintenance_NotFound"));
                        return;
                    }
                }

                LogInformation("Maintenance record deleted: busId=" + BusId + ", maintenanceId=" + maintenanceId);
                RedirectTo("~/Fleet/BusMaintenance.aspx?busId=" + BusId.ToString(CultureInfo.InvariantCulture) + "&deleted=1");
            }
            catch (SqlException ex)
            {
                LogException(ex, "Deleting maintenance record failed");
                ShowError("The maintenance record could not be deleted because of a database constraint.");
            }
            catch (ServiceException ex)
            {
                LogException(ex, "Deleting maintenance record failed");
                ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                LogException(ex, "Deleting maintenance record failed");
                ShowError("The maintenance record could not be deleted. Check the application log.");
            }
        }

        private void ClearForm()
        {
            EditingMaintenanceId = 0;
            SelectedMaintenanceId = 0;
            LoadMechanics();
            txtMaintenanceDate.Text = DateTime.Today.ToString("dd.MM.yyyy", Localization.Culture);
            txtNextDate.Text = DateTime.Today.AddDays(90).ToString("dd.MM.yyyy", Localization.Culture);
            txtType.Text = string.Empty;
            txtFoundIssue.Text = string.Empty;
            txtServiceResult.Text = string.Empty;
            using (BusService service = new BusService())
            {
                Bus bus = service.GetBusById(BusId);
                txtMileage.Text = bus.MileageKm.ToString(CultureInfo.InvariantCulture);
                lblFormBus.Text = HttpUtility.HtmlEncode(GetBusDisplayText(bus));
            }
            txtCost.Text = "0.00";
            if (ddlMechanic.Items.Count > 0)
                ddlMechanic.SelectedIndex = 0;
            if (ddlRoadworthiness.Items.FindByValue(RoadworthinessStatus.Operational) != null)
                ddlRoadworthiness.SelectedValue = RoadworthinessStatus.Operational;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            cbMaintenanceForm.Visible = false;
            EditingMaintenanceId = 0;
            ShowMessagesFromQueryString();
        }

        protected void btnDetailClose_Click(object sender, EventArgs e)
        {
            cbMaintenanceDetail.Visible = false;
            SelectedMaintenanceId = 0;
        }

        protected void btnOpenSelectedBus_Click(object sender, EventArgs e)
        {
            int id;
            if (!int.TryParse(ddlBusPicker.SelectedValue, out id) || id <= 0)
            {
                ShowError("Select a bus first.");
                return;
            }
            RedirectTo("~/Fleet/BusMaintenance.aspx?busId=" + id.ToString(CultureInfo.InvariantCulture));
        }

        protected void btnEditBus_Click(object sender, EventArgs e)
        {
            if (BusId > 0)
                RedirectTo("~/Fleet/BusEdit.aspx?busId=" + BusId.ToString(CultureInfo.InvariantCulture) + "&mode=edit");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (BusId > 0)
                RedirectTo("~/Fleet/Buses.aspx");
            else
                RedirectTo("~/Fleet/Maintenance.aspx");
        }

        private string GetBusDisplayText()
        {
            using (BusService service = new BusService())
                return GetBusDisplayText(service.GetBusById(BusId));
        }

        private static string GetBusDisplayText(Bus bus)
        {
            return bus == null ? string.Empty : bus.FleetNumber + " — " + bus.Manufacturer + " " + bus.Model;
        }

        private void ShowMessagesFromQueryString()
        {
            pnlError.Visible = false;
            pnlWarning.Visible = false;
            if (Request.QueryString["saved"] == "1")
                ShowSuccess(Localization.Get("BusMaintenance_Saved"));
            else if (Request.QueryString["deleted"] == "1")
                ShowSuccess(Localization.Get("BusMaintenance_Deleted"));
        }

        #endregion

        #region Feedback and helpers

        private void ShowError(string message)
        {
            pnlError.Visible = true;
            pnlSuccess.Visible = false;
            litError.Text = HttpUtility.HtmlEncode(message ?? string.Empty);
        }

        private void ShowSuccess(string message)
        {
            pnlSuccess.Visible = true;
            pnlError.Visible = false;
            litSuccess.Text = HttpUtility.HtmlEncode(message ?? string.Empty);
        }

        private void ShowWarning(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                pnlWarning.Visible = false;
                litWarning.Text = string.Empty;
                return;
            }

            pnlWarning.Visible = true;
            litWarning.Text = HttpUtility.HtmlEncode(message);
        }

        private void ScrollToControl(Control control, string key)
        {
            if (control == null)
                return;

            string id = HttpUtility.JavaScriptStringEncode(control.ClientID, true);
            string script = "window.setTimeout(function(){var el=document.getElementById(" + id +
                ");if(el){el.scrollIntoView({block:'center',behavior:'auto'});}},0);";
            ScriptManager.RegisterStartupScript(this, GetType(), key, script, true);
        }

        private void RedirectTo(string url)
        {
            Response.Redirect(ResolveUrl(url), false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private static string GetDatabaseErrorMessage(SqlException ex)
        {
            if (ex.Number == 547)
                return "The maintenance operation violates a database relationship. Check the selected bus and mechanic, and retain maintenance history if the bus is still referenced.";
            if (ex.Number == 2601 || ex.Number == 2627)
                return "A unique value conflicts with an existing record.";
            return "A database error prevented the maintenance operation. Check the application log for details.";
        }

        #endregion
    }
}
