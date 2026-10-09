using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BRU.WEBFORMS.ASPNET.APP.Controls;
using BRU.WEBFORMS.ASPNET.APP.Models;
using MaintenanceRecord = BRU.WEBFORMS.ASPNET.APP.Models.Maintenance;
using BRU.WEBFORMS.ASPNET.APP.Services;

namespace BRU.WEBFORMS.ASPNET.APP.Fleet
{
    /// <summary>
    /// Create and edit a bus, enforce write permissions, show fleet state and
    /// maintenance totals, and expose the selected bus's maintenance history.
    /// The form deliberately keeps domain validation in the service layer as
    /// well as validating input at the Web Forms boundary.
    /// </summary>
    public partial class BusEdit : SecurePage
    {
        protected override string[] RequiredPermissions
        {
            get { return new[] { "bus.read" }; }
        }

        #region Page-level controls

        protected global::System.Web.UI.WebControls.Literal litBreadcrumb;
        protected global::System.Web.UI.WebControls.Literal litPageTitle;
        protected global::System.Web.UI.WebControls.Panel pnlError;
        protected global::System.Web.UI.WebControls.Literal litError;
        protected global::System.Web.UI.WebControls.Panel pnlSuccess;
        protected global::System.Web.UI.WebControls.Literal litSuccess;
        protected global::System.Web.UI.WebControls.Panel pnlWarning;
        protected global::System.Web.UI.WebControls.Literal litWarning;
        protected global::System.Web.UI.WebControls.Button btnBack;
        protected global::System.Web.UI.WebControls.Button btnSave;
        protected global::System.Web.UI.WebControls.Button btnSaveAndNew;
        protected global::System.Web.UI.WebControls.Button btnDelete;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbBusForm;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbBusSummary;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbMaintenanceHistory;

        #endregion

        #region Template controls

        protected global::System.Web.UI.WebControls.Panel pnlBusForm;
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl rowBusId;
        protected global::System.Web.UI.WebControls.Label lblBusId;
        protected global::System.Web.UI.WebControls.TextBox txtFleetNumber;
        protected global::System.Web.UI.WebControls.TextBox txtRegistrationNum;
        protected global::System.Web.UI.WebControls.TextBox txtManufacturer;
        protected global::System.Web.UI.WebControls.TextBox txtModel;
        protected global::System.Web.UI.WebControls.TextBox txtManufactureYear;
        protected global::System.Web.UI.WebControls.TextBox txtCapacity;
        protected global::System.Web.UI.WebControls.DropDownList ddlStatus;
        protected global::System.Web.UI.WebControls.TextBox txtMileageKm;
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl rowMileageCategory;
        protected global::System.Web.UI.WebControls.Label lblMileageCategory;
        protected global::System.Web.UI.WebControls.Literal litSummaryStatus;
        protected global::System.Web.UI.WebControls.Literal litSummaryMileage;
        protected global::System.Web.UI.WebControls.Literal litSummaryMileageCategory;
        protected global::System.Web.UI.WebControls.Literal litSummaryMaintenanceCount;
        protected global::System.Web.UI.WebControls.Literal litSummaryTotalCost;
        protected global::System.Web.UI.WebControls.GridView gvMaintenanceHistory;
        protected global::System.Web.UI.WebControls.Button btnAddMaintenance;

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

        private bool IsEditMode
        {
            get { return BusId > 0; }
        }

        private int HistoryPage
        {
            get
            {
                object value = ViewState["BusEdit_HistoryPage"];
                return value == null ? 0 : Math.Max(0, Convert.ToInt32(value, CultureInfo.InvariantCulture));
            }
            set { ViewState["BusEdit_HistoryPage"] = Math.Max(0, value); }
        }

        private string HistorySortExpression
        {
            get { return Convert.ToString(ViewState["BusEdit_HistorySort"] ?? "maintenance_date", CultureInfo.InvariantCulture); }
            set { ViewState["BusEdit_HistorySort"] = value; }
        }

        private string HistorySortDirection
        {
            get { return Convert.ToString(ViewState["BusEdit_HistorySortDirection"] ?? "DESC", CultureInfo.InvariantCulture); }
            set { ViewState["BusEdit_HistorySortDirection"] = value == "ASC" ? "ASC" : "DESC"; }
        }

        #endregion

        #region Page lifecycle

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureTemplateControlsResolved();

                if (!IsPostBack)
                {
                    InitializePage();
                }
            }
            catch (SqlException ex)
            {
                LogException(ex, "Bus editor database error");
                ShowError(GetDatabaseErrorMessage(ex));
            }
            catch (ServiceException ex)
            {
                LogException(ex, "Bus editor service error");
                ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                LogException(ex, "Bus editor initialization error");
                ShowError("The bus editor could not be initialized. Check the application log for details.");
            }
        }

        private void InitializePage()
        {
            btnDelete.Visible = IsEditMode;
            btnSaveAndNew.Visible = !IsEditMode;
            rowBusId.Visible = IsEditMode;
            rowMileageCategory.Visible = IsEditMode;

            if (!IsEditMode)
            {
                litPageTitle.Text = HttpUtility.HtmlEncode(Localization.Get("BusEdit_AddHeading"));
                litBreadcrumb.Text = HttpUtility.HtmlEncode(Localization.Get("BusEdit_AddHeading"));
                cbBusForm.HeaderText = Localization.Get("BusEdit_AddHeading");
                cbBusSummary.Visible = false;
                cbMaintenanceHistory.Visible = false;
                ClearForm();
                return;
            }

            Bus bus;
            using (BusService service = new BusService())
            {
                bus = service.GetBusById(BusId);
            }

            PopulateForm(bus);
            litPageTitle.Text = HttpUtility.HtmlEncode(Localization.Get("BusEdit_EditHeading") + " — " + bus.FleetNumber);
            litBreadcrumb.Text = HttpUtility.HtmlEncode(bus.FleetNumber);
            cbBusForm.HeaderText = Localization.Get("BusEdit_EditHeading");
            cbBusSummary.Visible = true;
            cbMaintenanceHistory.Visible = true;
            LoadBusSummary(bus);
            BindMaintenanceHistory();

            if (Request.QueryString["saved"] == "1")
                ShowSuccess(Localization.Get("BusEdit_Saved"));
            if (Request.QueryString["deleted"] == "1")
                ShowSuccess(Localization.Get("BusEdit_Deleted"));
        }

        private void EnsureTemplateControlsResolved()
        {
            if (_templateControlsResolved)
                return;

            EnsureContentBox(cbBusForm, "cbBusForm");
            EnsureContentBox(cbBusSummary, "cbBusSummary");
            EnsureContentBox(cbMaintenanceHistory, "cbMaintenanceHistory");

            pnlBusForm = FindRequired<Panel>(cbBusForm, "pnlBusForm");
            rowBusId = FindRequired<HtmlGenericControl>(cbBusForm, "rowBusId");
            lblBusId = FindRequired<Label>(cbBusForm, "lblBusId");
            txtFleetNumber = FindRequired<TextBox>(cbBusForm, "txtFleetNumber");
            txtRegistrationNum = FindRequired<TextBox>(cbBusForm, "txtRegistrationNum");
            txtManufacturer = FindRequired<TextBox>(cbBusForm, "txtManufacturer");
            txtModel = FindRequired<TextBox>(cbBusForm, "txtModel");
            txtManufactureYear = FindRequired<TextBox>(cbBusForm, "txtManufactureYear");
            txtCapacity = FindRequired<TextBox>(cbBusForm, "txtCapacity");
            ddlStatus = FindRequired<DropDownList>(cbBusForm, "ddlStatus");
            txtMileageKm = FindRequired<TextBox>(cbBusForm, "txtMileageKm");
            rowMileageCategory = FindRequired<HtmlGenericControl>(cbBusForm, "rowMileageCategory");
            lblMileageCategory = FindRequired<Label>(cbBusForm, "lblMileageCategory");

            litSummaryStatus = FindRequired<Literal>(cbBusSummary, "litSummaryStatus");
            litSummaryMileage = FindRequired<Literal>(cbBusSummary, "litSummaryMileage");
            litSummaryMileageCategory = FindRequired<Literal>(cbBusSummary, "litSummaryMileageCategory");
            litSummaryMaintenanceCount = FindRequired<Literal>(cbBusSummary, "litSummaryMaintenanceCount");
            litSummaryTotalCost = FindRequired<Literal>(cbBusSummary, "litSummaryTotalCost");

            gvMaintenanceHistory = FindRequired<GridView>(cbMaintenanceHistory, "gvMaintenanceHistory");
            btnAddMaintenance = FindRequired<Button>(cbMaintenanceHistory, "btnAddMaintenance");

            _templateControlsResolved = true;
        }

        private static void EnsureContentBox(ContentBox box, string id)
        {
            if (box == null)
                throw new InvalidOperationException("Required ContentBox '" + id + "' was not created.");
        }

        private static T FindRequired<T>(ContentBox box, string id) where T : Control
        {
            T found = box.FindContentControl<T>(id);
            if (found == null)
                throw new InvalidOperationException("Required control '" + id + "' was not found inside ContentBox '" + box.ID + "'.");
            return found;
        }

        #endregion

        #region Bus form

        private void ClearForm()
        {
            lblBusId.Text = string.Empty;
            txtFleetNumber.Text = string.Empty;
            txtRegistrationNum.Text = string.Empty;
            txtManufacturer.Text = string.Empty;
            txtModel.Text = string.Empty;
            txtManufactureYear.Text = DateTime.Today.Year.ToString(CultureInfo.InvariantCulture);
            txtCapacity.Text = "40";
            if (ddlStatus.Items.FindByValue(BusStatus.Operational) != null)
                ddlStatus.SelectedValue = BusStatus.Operational;
            txtMileageKm.Text = "0";
            lblMileageCategory.Text = HttpUtility.HtmlEncode(MileageCategory.Low);
        }

        private void PopulateForm(Bus bus)
        {
            if (bus == null)
                throw new ServiceException("Bus not found.");

            lblBusId.Text = bus.BusId.ToString(CultureInfo.InvariantCulture);
            txtFleetNumber.Text = bus.FleetNumber ?? string.Empty;
            txtRegistrationNum.Text = bus.RegistrationNum ?? string.Empty;
            txtManufacturer.Text = bus.Manufacturer ?? string.Empty;
            txtModel.Text = bus.Model ?? string.Empty;
            txtManufactureYear.Text = bus.ManufactureYear.ToString(CultureInfo.InvariantCulture);
            txtCapacity.Text = bus.Capacity.ToString(CultureInfo.InvariantCulture);
            if (ddlStatus.Items.FindByValue(bus.Status) != null)
                ddlStatus.SelectedValue = bus.Status;
            txtMileageKm.Text = bus.MileageKm.ToString(CultureInfo.InvariantCulture);
            lblMileageCategory.Text = HttpUtility.HtmlEncode(GetMileageCategory(bus));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveBus(false);
        }

        protected void btnSaveAndNew_Click(object sender, EventArgs e)
        {
            SaveBus(true);
        }

        private void SaveBus(bool saveAndNew)
        {
            Page.Validate("BusEditForm");
            if (!Page.IsValid)
                return;

            if (!RequireWritePermission("bus.write"))
                return;

            try
            {
                int year;
                int capacity;
                int mileage;

                if (!int.TryParse(txtManufactureYear.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out year) ||
                    year < 1980 || year > 2100)
                {
                    ShowError("Manufacture year must be an integer between 1980 and 2100.");
                    return;
                }

                if (!int.TryParse(txtCapacity.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out capacity) ||
                    capacity < 1 || capacity > short.MaxValue)
                {
                    ShowError("Capacity must be a whole number between 1 and " + short.MaxValue + ".");
                    return;
                }

                if (!int.TryParse(txtMileageKm.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out mileage) ||
                    mileage < 0 || mileage > 9999999)
                {
                    ShowError("Mileage must be a whole number between 0 and 9,999,999 km.");
                    return;
                }

                Bus bus = new Bus
                {
                    BusId = BusId,
                    FleetNumber = txtFleetNumber.Text.Trim(),
                    RegistrationNum = txtRegistrationNum.Text.Trim(),
                    Manufacturer = txtManufacturer.Text.Trim(),
                    Model = txtModel.Text.Trim(),
                    ManufactureYear = checked((short)year),
                    Capacity = checked((short)capacity),
                    Status = ddlStatus.SelectedValue,
                    MileageKm = mileage
                };

                using (BusService service = new BusService())
                {
                    if (IsEditMode)
                    {
                        Bus existing = service.GetBusById(BusId);
                        if (existing.Status == BusStatus.Retired && bus.Status == BusStatus.Operational)
                        {
                            ShowWarning(Localization.Get("BusEdit_StatusChangeWarning"));
                            return;
                        }

                        if (!service.UpdateBus(bus))
                        {
                            ShowError("The bus was not updated. It may have been removed by another operation.");
                            return;
                        }

                        LogInformation("Bus updated: busId=" + bus.BusId + ", fleetNumber=" + bus.FleetNumber);
                    }
                    else
                    {
                        bus.BusId = service.CreateBus(bus);
                        LogInformation("Bus created: busId=" + bus.BusId + ", fleetNumber=" + bus.FleetNumber);
                    }
                }

                if (saveAndNew && !IsEditMode)
                {
                    ClearForm();
                    ShowSuccess(Localization.Get("BusEdit_Saved"));
                    return;
                }

                RedirectTo("~/Fleet/BusEdit.aspx?busId=" + bus.BusId.ToString(CultureInfo.InvariantCulture) +
                    "&mode=edit&saved=1");
            }
            catch (SqlException ex)
            {
                LogException(ex, "Saving bus failed");
                ShowError(GetDatabaseErrorMessage(ex));
            }
            catch (ServiceException ex)
            {
                LogException(ex, "Saving bus failed validation");
                ShowError(ex.Message);
            }
            catch (OverflowException ex)
            {
                LogException(ex, "Bus numeric field overflow");
                ShowError("One of the numeric values is outside the supported range.");
            }
            catch (Exception ex)
            {
                LogException(ex, "Saving bus failed");
                ShowError("The bus could not be saved. Check the values and application log.");
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!RequireWritePermission("bus.write"))
                return;
            if (!IsEditMode)
                return;

            try
            {
                using (BusService service = new BusService())
                {
                    if (!service.DeleteBus(BusId))
                    {
                        ShowError("The bus was not deleted. It may already have been removed.");
                        return;
                    }
                }

                LogInformation("Bus deleted: busId=" + BusId);
                RedirectTo("~/Fleet/Buses.aspx?deleted=1");
            }
            catch (SqlException ex)
            {
                LogException(ex, "Deleting bus failed");
                ShowError("The bus could not be deleted. It may still be referenced by schedules or maintenance history. Retire the bus and retain its operational history instead of deleting it.");
            }
            catch (ServiceException ex)
            {
                LogException(ex, "Deleting bus failed business rules");
                ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                LogException(ex, "Deleting bus failed");
                ShowError("The bus could not be deleted. Check the application log for details.");
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            RedirectTo("~/Fleet/Buses.aspx");
        }

        private void LoadBusSummary(Bus bus)
        {
            List<MaintenanceRecord> history;
            using (MaintenanceService service = new MaintenanceService())
            {
                history = service.GetMaintenanceByBus(bus.BusId);
            }

            if (history == null)
                history = new List<MaintenanceRecord>();

            litSummaryStatus.Text = HttpUtility.HtmlEncode(bus.Status ?? string.Empty);
            litSummaryMileage.Text = bus.MileageKm.ToString("N0", Localization.Culture);
            litSummaryMileageCategory.Text = HttpUtility.HtmlEncode(GetMileageCategory(bus));
            litSummaryMaintenanceCount.Text = history.Count.ToString(CultureInfo.InvariantCulture);
            litSummaryTotalCost.Text = history.Sum(x => x.MaintenanceCost).ToString("N2", Localization.Culture);
        }

        private static string GetMileageCategory(Bus bus)
        {
            if (bus != null && !string.IsNullOrWhiteSpace(bus.MileageCategory))
                return bus.MileageCategory;

            int mileage = bus == null ? 0 : bus.MileageKm;
            if (mileage >= 150000)
                return MileageCategory.High;
            if (mileage >= 50000)
                return MileageCategory.Medium;
            return MileageCategory.Low;
        }

        #endregion

        #region Maintenance history

        private void BindMaintenanceHistory()
        {
            if (!IsEditMode || gvMaintenanceHistory == null)
                return;

            List<MaintenanceRecord> rows;
            using (MaintenanceService service = new MaintenanceService())
            {
                rows = service.GetMaintenanceByBus(BusId);
            }
            if (rows == null)
                rows = new List<MaintenanceRecord>();

            bool ascending = HistorySortDirection == "ASC";
            switch (HistorySortExpression)
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

            int pageCount = Math.Max(1, (int)Math.Ceiling(rows.Count / (double)gvMaintenanceHistory.PageSize));
            if (gvMaintenanceHistory.PageIndex >= pageCount)
                gvMaintenanceHistory.PageIndex = pageCount - 1;

            gvMaintenanceHistory.DataSource = rows;
            gvMaintenanceHistory.DataBind();
        }

        protected void gvMaintenanceHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            e.Cancel = true;
            gvMaintenanceHistory.PageIndex = e.NewPageIndex;
            BindMaintenanceHistory();
        }

        protected void gvMaintenanceHistory_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (string.Equals(HistorySortExpression, e.SortExpression, StringComparison.OrdinalIgnoreCase))
                HistorySortDirection = HistorySortDirection == "ASC" ? "DESC" : "ASC";
            else
            {
                HistorySortExpression = e.SortExpression;
                HistorySortDirection = "ASC";
            }

            gvMaintenanceHistory.PageIndex = 0;
            BindMaintenanceHistory();
        }

        protected void gvMaintenanceHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            MaintenanceRecord record = e.Row.DataItem as MaintenanceRecord;
            if (record == null)
                return;

            Literal roadworthiness = e.Row.FindControl("litHistoryRw") as Literal;
            if (roadworthiness != null)
            {
                string css = record.Roadworthiness == RoadworthinessStatus.NotOperational ? "rw-notoperational" :
                    record.Roadworthiness == RoadworthinessStatus.NeedsAttention ? "rw-attention" : "rw-operational";
                roadworthiness.Text = "<span class='" + css + "'>" + HttpUtility.HtmlEncode(record.Roadworthiness ?? string.Empty) + "</span>";
            }

            Literal cost = e.Row.FindControl("litHistoryCost") as Literal;
            if (cost != null)
                cost.Text = "<span>" + record.MaintenanceCost.ToString("N2", Localization.Culture) + "</span>";
        }

        protected void btnViewRecord_Click(object sender, EventArgs e)
        {
            int maintenanceId = ReadMaintenanceIdFromButton(sender);
            if (maintenanceId > 0)
                RedirectTo("~/Fleet/BusMaintenance.aspx?busId=" + BusId.ToString(CultureInfo.InvariantCulture) +
                    "&maintenanceId=" + maintenanceId.ToString(CultureInfo.InvariantCulture) + "&mode=view");
        }

        protected void btnEditRecord_Click(object sender, EventArgs e)
        {
            if (!RequireWritePermission("bus.write"))
                return;

            int maintenanceId = ReadMaintenanceIdFromButton(sender);
            if (maintenanceId > 0)
                RedirectTo("~/Fleet/BusMaintenance.aspx?busId=" + BusId.ToString(CultureInfo.InvariantCulture) +
                    "&maintenanceId=" + maintenanceId.ToString(CultureInfo.InvariantCulture) + "&mode=edit");
        }

        private static int ReadMaintenanceIdFromButton(object sender)
        {
            Button button = sender as Button;
            int id;
            return button != null && int.TryParse(button.CommandArgument, out id) && id > 0 ? id : 0;
        }

        protected void btnAddMaintenance_Click(object sender, EventArgs e)
        {
            if (!IsEditMode)
                return;
            if (!RequireWritePermission("bus.write"))
                return;

            RedirectTo("~/Fleet/BusMaintenance.aspx?busId=" + BusId.ToString(CultureInfo.InvariantCulture) + "&mode=add");
        }

        #endregion

        #region Feedback and navigation helpers

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
            pnlWarning.Visible = true;
            litWarning.Text = HttpUtility.HtmlEncode(message ?? string.Empty);
        }

        private void RedirectTo(string url)
        {
            Response.Redirect(ResolveUrl(url), false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private static string GetDatabaseErrorMessage(SqlException ex)
        {
            if (ex.Number == 2601 || ex.Number == 2627)
                return "The fleet number or registration number is already assigned to another bus.";
            if (ex.Number == 547)
                return "This operation conflicts with related records. Keep the bus and its maintenance/schedule history, or resolve those references before deleting it.";
            if (ex.Number == 51001)
                return "A retired bus cannot be changed directly to operational status. Move it to reserve or in-repair first.";
            return "A database error prevented this operation. Check the application log for the SQL error details.";
        }

        #endregion
    }
}
