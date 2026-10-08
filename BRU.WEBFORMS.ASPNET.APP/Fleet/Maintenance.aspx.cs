using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BRU.WEBFORMS.ASPNET.APP.Controls;
using BRU.WEBFORMS.ASPNET.APP.Services;
using BRU.WEBFORMS.ASPNET.APP.Models;
using System.Data.SqlClient;
// Alias to avoid collision with the BRU.WEBFORMS.ASPNET.APP.Fleet.Maintenance page class
using MaintenanceRecord = BRU.WEBFORMS.ASPNET.APP.Models.Maintenance;

namespace BRU.WEBFORMS.ASPNET.APP.Fleet
{
    /// <summary>
    /// Maintenance Records page for Autopark Management System.
    /// Provides CRUD operations for maintenance records, roadworthiness
    /// tracking, cost analysis, overdue maintenance alerts, bus/mechanic
    /// filtering, sorting, pagination, and comprehensive statistics.
    /// </summary>
    public partial class Maintenance : SecurePage
    {
        protected override string[] RequiredPermissions { get { return new[] { "bus.read" }; } }
        #region Control Declarations

        protected global::System.Web.UI.WebControls.Panel pnlError;
        protected global::System.Web.UI.WebControls.Literal litError;
        protected global::System.Web.UI.WebControls.Panel pnlSuccess;
        protected global::System.Web.UI.WebControls.Literal litSuccess;
        protected global::System.Web.UI.WebControls.Panel pnlOverdueAlert;
        protected global::System.Web.UI.WebControls.Literal litOverdueAlert;
        protected global::System.Web.UI.WebControls.Literal litTotalRecords;
        protected global::System.Web.UI.WebControls.Literal litOperational;
        protected global::System.Web.UI.WebControls.Literal litNeedsAttention;
        protected global::System.Web.UI.WebControls.Literal litNotOperational;
        protected global::System.Web.UI.WebControls.Literal litTotalCost;
        protected global::System.Web.UI.WebControls.Button btnAddRecord;
        protected global::System.Web.UI.WebControls.Button btnRefresh;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbFilters;
        protected global::System.Web.UI.WebControls.DropDownList ddlRwFilter;
        protected global::System.Web.UI.WebControls.DropDownList ddlBusFilter;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbMaintenanceList;
        protected global::System.Web.UI.WebControls.GridView gvMaintenance;
        protected global::System.Web.UI.WebControls.Literal litPagination;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbMaintenanceForm;
        protected global::System.Web.UI.WebControls.Panel pnlMaintenanceForm;
        protected global::System.Web.UI.WebControls.DropDownList ddlBus;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvBus;
        protected global::System.Web.UI.WebControls.DropDownList ddlMechanic;
        protected global::System.Web.UI.WebControls.TextBox txtMaintenanceDate;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvDate;
        protected global::System.Web.UI.WebControls.TextBox txtNextDate;
        protected global::System.Web.UI.WebControls.TextBox txtType;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvType;
        protected global::System.Web.UI.WebControls.TextBox txtFoundIssue;
        protected global::System.Web.UI.WebControls.TextBox txtServiceResult;
        protected global::System.Web.UI.WebControls.TextBox txtMileage;
        protected global::System.Web.UI.WebControls.DropDownList ddlRoadworthiness;
        protected global::System.Web.UI.WebControls.TextBox txtCost;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvCost;
        protected global::System.Web.UI.WebControls.Button btnSave;
        protected global::System.Web.UI.WebControls.Button btnCancel;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbMaintenanceDetail;
        protected global::System.Web.UI.WebControls.Literal litDetail;
        protected global::System.Web.UI.WebControls.Button btnDetailClose;

        #endregion

        #region Private Fields

        private List<MaintenanceRecord> _currentMaintenanceList;
        private bool _templateControlsResolved;
        private const int _pageSize = 20;

        #endregion

        #region State Properties

        private int CurrentPage
        {
            get
            {
                object val = ViewState["Maint_CurrentPage"];
                if (val == null) return 1;
                int page;
                if (int.TryParse(val.ToString(), out page) && page > 0) return page;
                return 1;
            }
            set { ViewState["Maint_CurrentPage"] = value < 1 ? 1 : value; }
        }

        private string CurrentSortExpression
        {
            get
            {
                object val = ViewState["Maint_SortExpr"];
                return val == null ? "maintenance_date" : val.ToString();
            }
            set { ViewState["Maint_SortExpr"] = value; }
        }

        private string CurrentSortDirection
        {
            get
            {
                object val = ViewState["Maint_SortDir"];
                return val == null ? "DESC" : val.ToString();
            }
            set { ViewState["Maint_SortDir"] = value; }
        }

        private string FilterRoadworthiness
        {
            get
            {
                object val = ViewState["Maint_FilterRw"];
                return val == null ? string.Empty : val.ToString();
            }
            set { ViewState["Maint_FilterRw"] = value; }
        }

        private int? FilterBusId
        {
            get
            {
                object val = ViewState["Maint_FilterBusId"];
                if (val == null) return null;
                int id;
                if (int.TryParse(val.ToString(), out id)) return id;
                return null;
            }
            set { ViewState["Maint_FilterBusId"] = value; }
        }

        private int EditingMaintenanceId
        {
            get
            {
                object val = ViewState["Maint_EditingId"];
                if (val == null) return 0;
                int id;
                if (int.TryParse(val.ToString(), out id)) return id;
                return 0;
            }
            set { ViewState["Maint_EditingId"] = value; }
        }

        #endregion

        #region Page Lifecycle

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureTemplateControlsResolved();

                if (!IsPostBack)
                {
                    LoadBusFilters();
                    LoadMaintenance();
                    LoadStats();
                    LoadOverdueAlert();
                }
            }
            catch (SqlException sqlEx)
            {
                HandleDatabaseError(sqlEx);
            }
            catch (ServiceException svcEx)
            {
                HandleServiceError(svcEx);
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        #endregion

        #region Template Control Resolution

        private void EnsureTemplateControlsResolved()
        {
            if (_templateControlsResolved)
            {
                return;
            }

            EnsureContentBoxCreated(cbFilters, "cbFilters");
            EnsureContentBoxCreated(cbMaintenanceList, "cbMaintenanceList");
            EnsureContentBoxCreated(cbMaintenanceForm, "cbMaintenanceForm");
            EnsureContentBoxCreated(cbMaintenanceDetail, "cbMaintenanceDetail");

            ddlRwFilter = FindRequiredTemplateControl<DropDownList>(cbFilters, "ddlRwFilter");
            ddlBusFilter = FindRequiredTemplateControl<DropDownList>(cbFilters, "ddlBusFilter");
            gvMaintenance = FindRequiredTemplateControl<GridView>(cbMaintenanceList, "gvMaintenance");
            pnlMaintenanceForm = FindRequiredTemplateControl<Panel>(cbMaintenanceForm, "pnlMaintenanceForm");
            ddlBus = FindRequiredTemplateControl<DropDownList>(cbMaintenanceForm, "ddlBus");
            rfvBus = FindRequiredTemplateControl<RequiredFieldValidator>(cbMaintenanceForm, "rfvBus");
            ddlMechanic = FindRequiredTemplateControl<DropDownList>(cbMaintenanceForm, "ddlMechanic");
            txtMaintenanceDate = FindRequiredTemplateControl<TextBox>(cbMaintenanceForm, "txtMaintenanceDate");
            rfvDate = FindRequiredTemplateControl<RequiredFieldValidator>(cbMaintenanceForm, "rfvDate");
            txtNextDate = FindRequiredTemplateControl<TextBox>(cbMaintenanceForm, "txtNextDate");
            txtType = FindRequiredTemplateControl<TextBox>(cbMaintenanceForm, "txtType");
            rfvType = FindRequiredTemplateControl<RequiredFieldValidator>(cbMaintenanceForm, "rfvType");
            txtFoundIssue = FindRequiredTemplateControl<TextBox>(cbMaintenanceForm, "txtFoundIssue");
            txtServiceResult = FindRequiredTemplateControl<TextBox>(cbMaintenanceForm, "txtServiceResult");
            txtMileage = FindRequiredTemplateControl<TextBox>(cbMaintenanceForm, "txtMileage");
            ddlRoadworthiness = FindRequiredTemplateControl<DropDownList>(cbMaintenanceForm, "ddlRoadworthiness");
            txtCost = FindRequiredTemplateControl<TextBox>(cbMaintenanceForm, "txtCost");
            rfvCost = FindRequiredTemplateControl<RequiredFieldValidator>(cbMaintenanceForm, "rfvCost");
            btnSave = FindRequiredTemplateControl<Button>(cbMaintenanceForm, "btnSave");
            btnCancel = FindRequiredTemplateControl<Button>(cbMaintenanceForm, "btnCancel");
            litDetail = FindRequiredTemplateControl<Literal>(cbMaintenanceDetail, "litDetail");
            btnDetailClose = FindRequiredTemplateControl<Button>(cbMaintenanceDetail, "btnDetailClose");

            _templateControlsResolved = true;
        }

        private void EnsureContentBoxCreated(ContentBox contentBox, string controlId)
        {
            if (contentBox == null)
            {
                throw new InvalidOperationException("Required ContentBox '" + controlId + "' was not created. Check Maintenance.aspx markup and the ContentBox registration.");
            }
        }

        private T FindRequiredTemplateControl<T>(ContentBox contentBox, string controlId) where T : Control
        {
            T control = contentBox.FindContentControl<T>(controlId);
            if (control == null)
            {
                throw new InvalidOperationException("Required control '" + controlId + "' was not found inside ContentBox '" + contentBox.ID + "'. Check that the control is inside the ContentTemplate and has runat=\"server\".");
            }
            return control;
        }

        #endregion

        #region Data Loading

        private void LoadBusFilters()
        {
            using (BusService busService = new BusService())
            {
                List<Bus> buses = busService.GetAllBuses();

                ddlBusFilter.Items.Clear();
                ddlBusFilter.Items.Add(new ListItem("All Buses", ""));
                ddlBus.Items.Clear();
                ddlBus.Items.Add(new ListItem("-- Select Bus --", ""));

                if (buses != null)
                {
                    foreach (Bus bus in buses)
                    {
                        string text = bus.FleetNumber + " (" + bus.Model + ")";
                        string value = bus.BusId.ToString();
                        ddlBusFilter.Items.Add(new ListItem(text, value));
                        ddlBus.Items.Add(new ListItem(text, value));
                    }
                }
            }

            // Load mechanics
            ddlMechanic.Items.Clear();
            ddlMechanic.Items.Add(new ListItem("-- None --", ""));
            using (EmployeeService empService = new EmployeeService())
            {
                List<Employee> mechanics = empService.GetMechanics();
                if (mechanics != null)
                {
                    foreach (Employee m in mechanics)
                    {
                        ddlMechanic.Items.Add(new ListItem(m.EmployeeName, m.EmployeeId.ToString()));
                    }
                }
            }
        }

        private void LoadMaintenance()
        {
            using (MaintenanceService service = new MaintenanceService())
            {
                if (FilterBusId.HasValue)
                    _currentMaintenanceList = service.GetMaintenanceByBus(FilterBusId.Value);
                else
                    _currentMaintenanceList = service.GetAllMaintenance();
            }

            ApplyFiltersAndSort();
            BindGrid();
        }

        private void LoadStats()
        {
            List<MaintenanceRecord> records;
            using (MaintenanceService service = new MaintenanceService())
            {
                records = service.GetAllMaintenance();
            }

            if (records == null) records = new List<MaintenanceRecord>();

            int operational = 0, attention = 0, notOperational = 0;
            decimal totalCost = 0;

            foreach (MaintenanceRecord m in records)
            {
                if (m.Roadworthiness == RoadworthinessStatus.Operational) operational++;
                else if (m.Roadworthiness == RoadworthinessStatus.NeedsAttention) attention++;
                else if (m.Roadworthiness == RoadworthinessStatus.NotOperational) notOperational++;
                totalCost += m.MaintenanceCost;
            }

            litTotalRecords.Text = records.Count.ToString();
            litOperational.Text = operational.ToString();
            litNeedsAttention.Text = attention.ToString();
            litNotOperational.Text = notOperational.ToString();
            litTotalCost.Text = totalCost.ToString("F2");
        }

        private void LoadOverdueAlert()
        {
            try
            {
                using (MaintenanceService service = new MaintenanceService())
                {
                    List<int> overdueBusIds = service.GetBusesOverdueForMaintenance();
                    if (overdueBusIds != null && overdueBusIds.Count > 0)
                    {
                        pnlOverdueAlert.Visible = true;
                        litOverdueAlert.Text = "<strong>Warning:</strong> " + overdueBusIds.Count +
                            " bus(es) are overdue for maintenance. Please schedule service immediately.";
                    }
                    else
                    {
                        pnlOverdueAlert.Visible = false;
                    }
                }
            }
            catch (Exception)
            {
                // Overdue alert is non-critical; don't break page on failure
                pnlOverdueAlert.Visible = false;
            }
        }

        private void ApplyFiltersAndSort()
        {
            if (_currentMaintenanceList == null) _currentMaintenanceList = new List<MaintenanceRecord>();

            // Apply roadworthiness filter
            if (!string.IsNullOrEmpty(FilterRoadworthiness))
            {
                List<MaintenanceRecord> filtered = new List<MaintenanceRecord>();
                foreach (MaintenanceRecord m in _currentMaintenanceList)
                {
                    if (m.Roadworthiness == FilterRoadworthiness) filtered.Add(m);
                }
                _currentMaintenanceList = filtered;
            }

            ApplySorting();
        }

        private void ApplySorting()
        {
            if (_currentMaintenanceList == null || _currentMaintenanceList.Count <= 1) return;

            string sortExpr = CurrentSortExpression;
            bool ascending = CurrentSortDirection == "ASC";

            Comparison<MaintenanceRecord> comparison = null;
            switch (sortExpr)
            {
                case "maintenance_id":
                    comparison = delegate(MaintenanceRecord a, MaintenanceRecord b) { return a.MaintenanceId.CompareTo(b.MaintenanceId); };
                    break;
                case "fleet_number":
                    comparison = delegate(MaintenanceRecord a, MaintenanceRecord b) { return string.Compare(a.FleetNumber ?? "", b.FleetNumber ?? "", StringComparison.Ordinal); };
                    break;
                case "maintenance_date":
                    comparison = delegate(MaintenanceRecord a, MaintenanceRecord b) { return a.MaintenanceDate.CompareTo(b.MaintenanceDate); };
                    break;
                case "next_maintenance_date":
                    comparison = delegate(MaintenanceRecord a, MaintenanceRecord b)
                    {
                        DateTime da = a.NextMaintenanceDate ?? DateTime.MaxValue;
                        DateTime db = b.NextMaintenanceDate ?? DateTime.MaxValue;
                        return da.CompareTo(db);
                    };
                    break;
                case "maintenance_type":
                    comparison = delegate(MaintenanceRecord a, MaintenanceRecord b) { return string.Compare(a.MaintenanceType ?? "", b.MaintenanceType ?? "", StringComparison.Ordinal); };
                    break;
                case "mileage_km":
                    comparison = delegate(MaintenanceRecord a, MaintenanceRecord b)
                    {
                        int va = a.MileageKm ?? 0;
                        int vb = b.MileageKm ?? 0;
                        return va.CompareTo(vb);
                    };
                    break;
                case "roadworthiness":
                    comparison = delegate(MaintenanceRecord a, MaintenanceRecord b) { return string.Compare(a.Roadworthiness ?? "", b.Roadworthiness ?? "", StringComparison.Ordinal); };
                    break;
                case "maintenance_cost":
                    comparison = delegate(MaintenanceRecord a, MaintenanceRecord b) { return a.MaintenanceCost.CompareTo(b.MaintenanceCost); };
                    break;
                case "days_from_last_service":
                    comparison = delegate(MaintenanceRecord a, MaintenanceRecord b) { return a.DaysFromLastService.CompareTo(b.DaysFromLastService); };
                    break;
                default:
                    comparison = delegate(MaintenanceRecord a, MaintenanceRecord b) { return a.MaintenanceDate.CompareTo(b.MaintenanceDate); };
                    break;
            }

            _currentMaintenanceList.Sort(comparison);
            if (!ascending) _currentMaintenanceList.Reverse();
        }

        private void BindGrid()
        {
            int totalCount = _currentMaintenanceList != null ? _currentMaintenanceList.Count : 0;
            int totalPages = (int)Math.Ceiling((double)totalCount / _pageSize);
            if (totalPages == 0) totalPages = 1;
            if (CurrentPage > totalPages) CurrentPage = totalPages;

            gvMaintenance.PageSize = _pageSize;
            gvMaintenance.PageIndex = CurrentPage - 1;
            gvMaintenance.DataSource = _currentMaintenanceList;
            gvMaintenance.DataBind();
            RenderPagination(totalPages, totalCount);
        }

        private void RenderPagination(int totalPages, int totalCount)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<span class='text-small'>Total: ").Append(totalCount).Append(" records");
            if (totalPages > 1)
                sb.Append(" | Page ").Append(CurrentPage).Append(" of ").Append(totalPages);
            sb.Append("</span>");
            litPagination.Text = sb.ToString();
        }

        #endregion

        #region Grid Events

        protected void gvMaintenance_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CurrentPage = e.NewPageIndex + 1;
            LoadMaintenance();
        }

        protected void gvMaintenance_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression == CurrentSortExpression)
                CurrentSortDirection = CurrentSortDirection == "ASC" ? "DESC" : "ASC";
            else
            {
                CurrentSortExpression = e.SortExpression;
                CurrentSortDirection = "ASC";
            }
            CurrentPage = 1;
            LoadMaintenance();
        }

        protected void gvMaintenance_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandArgument == null) return;
            int maintenanceId;
            if (!int.TryParse(e.CommandArgument.ToString(), out maintenanceId)) return;

            switch (e.CommandName)
            {
                case "ViewRecord":
                    ShowMaintenanceDetail(maintenanceId);
                    break;
                case "EditRecord":
                    if (!RequireWritePermission("bus.write")) return;
                    ShowEditForm(maintenanceId);
                    break;
                case "DeleteRecord":
                    if (!RequireWritePermission("bus.write")) return;
                    DeleteMaintenance(maintenanceId);
                    break;
            }
        }

        protected void gvMaintenance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                MaintenanceRecord record = (MaintenanceRecord)e.Row.DataItem;

                // Roadworthiness coloring
                Literal litRw = (Literal)e.Row.FindControl("litRw");
                if (litRw != null)
                {
                    string cssClass;
                    switch (record.Roadworthiness)
                    {
                        case RoadworthinessStatus.Operational: cssClass = "rw-operational"; break;
                        case RoadworthinessStatus.NeedsAttention: cssClass = "rw-attention"; break;
                        case RoadworthinessStatus.NotOperational: cssClass = "rw-notoperational"; break;
                        default: cssClass = "rw-operational"; break;
                    }
                    litRw.Text = "<span class='" + cssClass + "'>" + record.Roadworthiness + "</span>";
                }

                // Cost coloring
                Literal litCost = (Literal)e.Row.FindControl("litCost");
                if (litCost != null)
                {
                    string cssClass;
                    if (record.MaintenanceCost >= 10000) cssClass = "cost-high";
                    else if (record.MaintenanceCost >= 2000) cssClass = "cost-medium";
                    else cssClass = "cost-low";
                    litCost.Text = "<span class='" + cssClass + "'>" + record.MaintenanceCost.ToString("F2") + "</span>";
                }

                // Next maintenance date overdue check
                if (record.NextMaintenanceDate.HasValue && record.NextMaintenanceDate.Value < DateTime.Today)
                {
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        // Find the "Next Service" column (index 7)
                        if (cell.Text != null && cell.Text.Contains(record.NextMaintenanceDate.Value.ToString("dd.MM.yyyy")))
                        {
                            cell.Text = "<span class='overdue'>" + record.NextMaintenanceDate.Value.ToString("dd.MM.yyyy") + "</span>";
                        }
                    }
                }

                // Null fallbacks
                foreach (TableCell cell in e.Row.Cells)
                {
                    if (cell.Text != null && cell.Text == "&nbsp;")
                        cell.Text = "<span class='text-small'>—</span>";
                }
            }
        }

        #endregion

        #region Action Button Events

        protected void btnAddRecord_Click(object sender, EventArgs e)
        {
            if (!RequireWritePermission("bus.write")) return;
            LoadBusFilters();
            EditingMaintenanceId = 0;
            ClearForm();
            cbMaintenanceForm.HeaderText = "Add New Maintenance Record";
            cbMaintenanceForm.Visible = true;
            cbMaintenanceDetail.Visible = false;
            pnlError.Visible = false;
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            FilterRoadworthiness = string.Empty;
            FilterBusId = null;
            CurrentSortExpression = "maintenance_date";
            CurrentSortDirection = "DESC";
            ddlRwFilter.SelectedIndex = 0;
            ddlBusFilter.SelectedIndex = 0;
            cbMaintenanceForm.Visible = false;
            cbMaintenanceDetail.Visible = false;
            LoadMaintenance();
            LoadStats();
            LoadOverdueAlert();
            ShowSuccess("Maintenance list refreshed successfully.");
        }

        protected void ddlRwFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterRoadworthiness = ddlRwFilter.SelectedValue;
            CurrentPage = 1;
            LoadMaintenance();
        }

        protected void ddlBusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlBusFilter.SelectedValue))
            {
                int busId;
                if (int.TryParse(ddlBusFilter.SelectedValue, out busId))
                    FilterBusId = busId;
                else
                    FilterBusId = null;
            }
            else
            {
                FilterBusId = null;
            }
            CurrentPage = 1;
            LoadMaintenance();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!RequireWritePermission("bus.write")) return;
                MaintenanceRecord record = new MaintenanceRecord();
                record.MaintenanceId = EditingMaintenanceId;

                int busId;
                if (!int.TryParse(ddlBus.SelectedValue, out busId) || busId <= 0)
                {
                    ShowError("Please select a valid bus.");
                    return;
                }
                record.BusId = busId;

                if (!string.IsNullOrEmpty(ddlMechanic.SelectedValue))
                {
                    int empId;
                    if (int.TryParse(ddlMechanic.SelectedValue, out empId))
                        record.EmployeeId = empId;
                }

                DateTime maintDate;
                if (!DateTime.TryParseExact(txtMaintenanceDate.Text.Trim(), "dd.MM.yyyy",
                    null, System.Globalization.DateTimeStyles.None, out maintDate))
                {
                    ShowError("Invalid maintenance date format. Use dd.MM.yyyy");
                    return;
                }
                record.MaintenanceDate = maintDate;

                if (!string.IsNullOrWhiteSpace(txtNextDate.Text))
                {
                    DateTime nextDate;
                    if (!DateTime.TryParseExact(txtNextDate.Text.Trim(), "dd.MM.yyyy",
                        null, System.Globalization.DateTimeStyles.None, out nextDate))
                    {
                        ShowError("Invalid next service date format. Use dd.MM.yyyy");
                        return;
                    }
                    record.NextMaintenanceDate = nextDate;
                }
                else
                {
                    record.NextMaintenanceDate = null;
                }

                record.MaintenanceType = txtType.Text.Trim();

                record.FoundIssue = string.IsNullOrWhiteSpace(txtFoundIssue.Text)
                    ? null : txtFoundIssue.Text.Trim();

                record.ServiceResult = string.IsNullOrWhiteSpace(txtServiceResult.Text)
                    ? null : txtServiceResult.Text.Trim();

                if (!string.IsNullOrWhiteSpace(txtMileage.Text))
                {
                    int mileage;
                    if (!int.TryParse(txtMileage.Text.Trim(), out mileage) || mileage < 0)
                    {
                        ShowError("Invalid mileage value.");
                        return;
                    }
                    record.MileageKm = mileage;
                }
                else
                {
                    record.MileageKm = null;
                }

                record.Roadworthiness = ddlRoadworthiness.SelectedValue;

                decimal cost;
                if (!decimal.TryParse(txtCost.Text.Trim(), out cost) || cost < 0)
                {
                    ShowError("Invalid cost value.");
                    return;
                }
                record.MaintenanceCost = cost;

                using (MaintenanceService service = new MaintenanceService())
                {
                    if (EditingMaintenanceId > 0)
                    {
                        service.UpdateMaintenance(record);
                        ShowSuccess("Maintenance record updated successfully.");
                    }
                    else
                    {
                        service.CreateMaintenance(record);
                        ShowSuccess("Maintenance record created successfully.");
                    }
                }

                cbMaintenanceForm.Visible = false;
                LoadMaintenance();
                LoadStats();
                LoadOverdueAlert();
            }
            catch (ServiceException svcEx)
            {
                HandleServiceError(svcEx);
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            cbMaintenanceForm.Visible = false;
            ClearForm();
            pnlError.Visible = false;
        }

        protected void btnDetailClose_Click(object sender, EventArgs e)
        {
            cbMaintenanceDetail.Visible = false;
        }

        #endregion

        #region Form Helpers

        private void ClearForm()
        {
            if (ddlBus.Items.Count > 0) ddlBus.SelectedIndex = 0;
            if (ddlMechanic.Items.Count > 0) ddlMechanic.SelectedIndex = 0;
            txtMaintenanceDate.Text = DateTime.Today.ToString("dd.MM.yyyy");
            txtNextDate.Text = string.Empty;
            txtType.Text = string.Empty;
            txtFoundIssue.Text = string.Empty;
            txtServiceResult.Text = string.Empty;
            txtMileage.Text = string.Empty;
            if (ddlRoadworthiness.Items.Count > 0) ddlRoadworthiness.SelectedIndex = 0;
            txtCost.Text = "0.00";
        }

        private void ShowEditForm(int maintenanceId)
        {
            try
            {
                MaintenanceRecord record = null;
                using (MaintenanceService service = new MaintenanceService())
                {
                    List<MaintenanceRecord> allRecords = service.GetAllMaintenance();
                    if (allRecords != null)
                    {
                        foreach (MaintenanceRecord m in allRecords)
                        {
                            if (m.MaintenanceId == maintenanceId) { record = m; break; }
                        }
                    }
                }

                if (record == null)
                {
                    ShowError("Maintenance record not found.");
                    return;
                }

                LoadBusFilters();

                // Select the bus in dropdown
                ListItem busItem = ddlBus.Items.FindByValue(record.BusId.ToString());
                if (busItem != null) ddlBus.SelectedValue = record.BusId.ToString();

                if (record.EmployeeId.HasValue)
                {
                    ListItem mechItem = ddlMechanic.Items.FindByValue(record.EmployeeId.Value.ToString());
                    if (mechItem != null) ddlMechanic.SelectedValue = record.EmployeeId.Value.ToString();
                }

                txtMaintenanceDate.Text = record.MaintenanceDate.ToString("dd.MM.yyyy");
                txtNextDate.Text = record.NextMaintenanceDate.HasValue
                    ? record.NextMaintenanceDate.Value.ToString("dd.MM.yyyy")
                    : string.Empty;
                txtType.Text = record.MaintenanceType ?? string.Empty;
                txtFoundIssue.Text = record.FoundIssue ?? string.Empty;
                txtServiceResult.Text = record.ServiceResult ?? string.Empty;
                txtMileage.Text = record.MileageKm.HasValue ? record.MileageKm.Value.ToString() : string.Empty;

                ListItem rwItem = ddlRoadworthiness.Items.FindByValue(record.Roadworthiness);
                if (rwItem != null) ddlRoadworthiness.SelectedValue = record.Roadworthiness;

                txtCost.Text = record.MaintenanceCost.ToString("F2");

                EditingMaintenanceId = maintenanceId;
                cbMaintenanceForm.HeaderText = "Edit Maintenance Record";
                cbMaintenanceForm.Visible = true;
                cbMaintenanceDetail.Visible = false;
                pnlError.Visible = false;
            }
            catch (ServiceException svcEx)
            {
                HandleServiceError(svcEx);
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        private void ShowMaintenanceDetail(int maintenanceId)
        {
            try
            {
                MaintenanceRecord record = null;
                using (MaintenanceService service = new MaintenanceService())
                {
                    List<MaintenanceRecord> allRecords = service.GetAllMaintenance();
                    if (allRecords != null)
                    {
                        foreach (MaintenanceRecord m in allRecords)
                        {
                            if (m.MaintenanceId == maintenanceId) { record = m; break; }
                        }
                    }
                }

                if (record == null)
                {
                    ShowError("Maintenance record not found.");
                    return;
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("<div class='detail-section'>");
                sb.Append("<div class='detail-row'><span class='detail-label'>ID:</span><span class='detail-value'>").Append(record.MaintenanceId).Append("</span></div>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Bus Fleet #:</span><span class='detail-value'>").Append(Server.HtmlEncode(record.FleetNumber ?? "—")).Append("</span></div>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Bus ID:</span><span class='detail-value'>").Append(record.BusId).Append("</span></div>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Date:</span><span class='detail-value'>").Append(record.MaintenanceDate.ToString("dd.MM.yyyy")).Append("</span></div>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Next Service:</span><span class='detail-value'>").Append(record.NextMaintenanceDate.HasValue ? record.NextMaintenanceDate.Value.ToString("dd.MM.yyyy") : "—").Append("</span></div>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Type:</span><span class='detail-value'>").Append(Server.HtmlEncode(record.MaintenanceType ?? "—")).Append("</span></div>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Found Issue:</span><span class='detail-value'>").Append(Server.HtmlEncode(record.FoundIssue ?? "—")).Append("</span></div>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Service Result:</span><span class='detail-value'>").Append(Server.HtmlEncode(record.ServiceResult ?? "—")).Append("</span></div>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Mileage (km):</span><span class='detail-value'>").Append(record.MileageKm.HasValue ? record.MileageKm.Value.ToString() : "—").Append("</span></div>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Roadworthiness:</span><span class='detail-value'>").Append(Server.HtmlEncode(record.Roadworthiness ?? "—")).Append("</span></div>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Cost:</span><span class='detail-value'>").Append(record.MaintenanceCost.ToString("F2")).Append("</span></div>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Days From Service:</span><span class='detail-value'>").Append(record.DaysFromLastService).Append("</span></div>");
                sb.Append("</div>");

                litDetail.Text = sb.ToString();
                cbMaintenanceDetail.Visible = true;
                cbMaintenanceForm.Visible = false;
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        private void DeleteMaintenance(int maintenanceId)
        {
            try
            {
                if (!RequireWritePermission("bus.write")) return;
                using (MaintenanceService service = new MaintenanceService())
                {
                    service.DeleteMaintenance(maintenanceId);
                }
                ShowSuccess("Maintenance record deleted successfully.");
                LoadMaintenance();
                LoadStats();
                LoadOverdueAlert();
            }
            catch (ServiceException svcEx)
            {
                HandleServiceError(svcEx);
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        #endregion

        #region Error Handling

        private void HandleDatabaseError(SqlException sqlEx)
        {
            string errorMsg;
            switch (sqlEx.Number)
            {
                case 53: errorMsg = "Database server is not available. Please check your connection."; break;
                case 18456: errorMsg = "Database login failed. Please check credentials."; break;
                case 208: errorMsg = "Database table not found. Please ensure the schema is deployed."; break;
                case 547: errorMsg = "Cannot delete record: it is referenced by other records."; break;
                default: errorMsg = "Database error (Code " + sqlEx.Number + "): " + sqlEx.Message; break;
            }
            ShowError(errorMsg);
            System.Diagnostics.Debug.WriteLine("Maintenance SQL Error: " + sqlEx.ToString());
        }

        private void HandleServiceError(ServiceException svcEx)
        {
            string msg = svcEx.Message;
            if (svcEx.InnerException != null) msg += " (" + svcEx.InnerException.Message + ")";
            ShowError(msg);
            System.Diagnostics.Debug.WriteLine("Maintenance Service Error: " + svcEx.ToString());
        }

        private void HandleGenericError(Exception ex)
        {
            ShowError("Unexpected error: " + ex.Message);
            System.Diagnostics.Debug.WriteLine("Maintenance Generic Error: " + ex.ToString());
        }

        #endregion

        #region UI Helpers

        private void ShowError(string message)
        {
            pnlError.Visible = true;
            pnlSuccess.Visible = false;
            litError.Text = Server.HtmlEncode(message);
        }

        private void ShowSuccess(string message)
        {
            pnlSuccess.Visible = true;
            pnlError.Visible = false;
            litSuccess.Text = Server.HtmlEncode(message);
        }

        #endregion
    }
}
