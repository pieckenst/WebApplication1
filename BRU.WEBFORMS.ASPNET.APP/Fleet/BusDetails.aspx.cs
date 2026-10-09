using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BRU.WEBFORMS.ASPNET.APP.Controls;
using BRU.WEBFORMS.ASPNET.APP.Models;
using MaintenanceRecord = BRU.WEBFORMS.ASPNET.APP.Models.Maintenance;
using BRU.WEBFORMS.ASPNET.APP.Services;

namespace BRU.WEBFORMS.ASPNET.APP.Fleet
{
    /// <summary>
    /// Read-only bus profile. This endpoint is kept separate from the editor so
    /// that the GridView's View action never opens a form with write operations.
    /// </summary>
    public partial class BusDetails : SecurePage
    {
        protected override string[] RequiredPermissions
        {
            get { return new[] { "bus.read" }; }
        }

        protected global::System.Web.UI.WebControls.Literal litBreadcrumb;
        protected global::System.Web.UI.WebControls.Literal litPageTitle;
        protected global::System.Web.UI.WebControls.Panel pnlError;
        protected global::System.Web.UI.WebControls.Literal litError;
        protected global::System.Web.UI.WebControls.Button btnBack;
        protected global::System.Web.UI.WebControls.Button btnEditBus;
        protected global::System.Web.UI.WebControls.Button btnMaintenance;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbBusDetails;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbMaintenanceHistory;
        protected global::System.Web.UI.WebControls.Literal litMaintenanceCount;
        protected global::System.Web.UI.WebControls.Literal litTotalMaintenanceCost;
        protected global::System.Web.UI.WebControls.Literal litLastService;
        protected global::System.Web.UI.WebControls.Literal litNextDue;

        protected global::System.Web.UI.WebControls.Literal litBusId;
        protected global::System.Web.UI.WebControls.Literal litFleetNumber;
        protected global::System.Web.UI.WebControls.Literal litRegistration;
        protected global::System.Web.UI.WebControls.Literal litManufacturer;
        protected global::System.Web.UI.WebControls.Literal litModel;
        protected global::System.Web.UI.WebControls.Literal litYear;
        protected global::System.Web.UI.WebControls.Literal litCapacity;
        protected global::System.Web.UI.WebControls.Literal litStatus;
        protected global::System.Web.UI.WebControls.Literal litMileage;
        protected global::System.Web.UI.WebControls.Literal litMileageCategory;
        protected global::System.Web.UI.WebControls.GridView gvMaintenanceHistory;

        private bool _templateControlsResolved;

        private int BusId
        {
            get
            {
                int value;
                return int.TryParse(Request.QueryString["busId"], out value) && value > 0 ? value : 0;
            }
        }

        private string CurrentSortExpression
        {
            get { return Convert.ToString(ViewState["BusDetails_Sort"] ?? "maintenance_date", CultureInfo.InvariantCulture); }
            set { ViewState["BusDetails_Sort"] = value; }
        }

        private string CurrentSortDirection
        {
            get { return Convert.ToString(ViewState["BusDetails_SortDirection"] ?? "DESC", CultureInfo.InvariantCulture); }
            set { ViewState["BusDetails_SortDirection"] = value == "ASC" ? "ASC" : "DESC"; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureTemplateControlsResolved();
                if (!IsPostBack)
                    LoadBusDetails();
            }
            catch (ServiceException ex)
            {
                LogException(ex, "Bus details service error");
                ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                LogException(ex, "Bus details page error");
                ShowError("Bus details could not be loaded. Check the application log for details.");
            }
        }

        private void EnsureTemplateControlsResolved()
        {
            if (_templateControlsResolved)
                return;

            EnsureBox(cbBusDetails, "cbBusDetails");
            EnsureBox(cbMaintenanceHistory, "cbMaintenanceHistory");

            litBusId = FindRequired<Literal>(cbBusDetails, "litBusId");
            litFleetNumber = FindRequired<Literal>(cbBusDetails, "litFleetNumber");
            litRegistration = FindRequired<Literal>(cbBusDetails, "litRegistration");
            litManufacturer = FindRequired<Literal>(cbBusDetails, "litManufacturer");
            litModel = FindRequired<Literal>(cbBusDetails, "litModel");
            litYear = FindRequired<Literal>(cbBusDetails, "litYear");
            litCapacity = FindRequired<Literal>(cbBusDetails, "litCapacity");
            litStatus = FindRequired<Literal>(cbBusDetails, "litStatus");
            litMileage = FindRequired<Literal>(cbBusDetails, "litMileage");
            litMileageCategory = FindRequired<Literal>(cbBusDetails, "litMileageCategory");
            gvMaintenanceHistory = FindRequired<GridView>(cbMaintenanceHistory, "gvMaintenanceHistory");

            _templateControlsResolved = true;
        }

        private static void EnsureBox(ContentBox box, string id)
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

        private void LoadBusDetails()
        {
            if (BusId <= 0)
            {
                ShowError(Localization.Get("BusEdit_InvalidId"));
                cbBusDetails.Visible = false;
                cbMaintenanceHistory.Visible = false;
                btnEditBus.Visible = false;
                btnMaintenance.Visible = false;
                return;
            }

            Bus bus;
            using (BusService service = new BusService())
                bus = service.GetBusById(BusId);

            string fleetNumber = bus.FleetNumber ?? string.Empty;
            litPageTitle.Text = HttpUtility.HtmlEncode(fleetNumber + " — " + Localization.Get("Common_View"));
            litBreadcrumb.Text = HttpUtility.HtmlEncode(fleetNumber);
            cbBusDetails.HeaderText = Localization.Get("BusEdit_BusSummary") + " — " + fleetNumber;

            litBusId.Text = bus.BusId.ToString(CultureInfo.InvariantCulture);
            litFleetNumber.Text = HttpUtility.HtmlEncode(bus.FleetNumber);
            litRegistration.Text = HttpUtility.HtmlEncode(bus.RegistrationNum);
            litManufacturer.Text = HttpUtility.HtmlEncode(bus.Manufacturer);
            litModel.Text = HttpUtility.HtmlEncode(bus.Model);
            litYear.Text = bus.ManufactureYear.ToString(CultureInfo.InvariantCulture);
            litCapacity.Text = bus.Capacity.ToString(CultureInfo.InvariantCulture);
            litStatus.Text = StatusMarkup(bus.Status);
            litMileage.Text = bus.MileageKm.ToString("N0", Localization.Culture) + " km";
            litMileageCategory.Text = HttpUtility.HtmlEncode(GetMileageCategory(bus));

            btnEditBus.Visible = HasWritePermission("bus.write");
            btnMaintenance.Visible = true;
            LoadMaintenanceStatistics();
            BindMaintenanceHistory();
        }

        private void LoadMaintenanceStatistics()
        {
            List<MaintenanceRecord> rows;
            using (MaintenanceService service = new MaintenanceService())
                rows = service.GetMaintenanceByBus(BusId);
            if (rows == null)
                rows = new List<MaintenanceRecord>();

            litMaintenanceCount.Text = rows.Count.ToString(CultureInfo.InvariantCulture);
            litTotalMaintenanceCost.Text = rows.Sum(x => x.MaintenanceCost).ToString("N2", Localization.Culture);

            MaintenanceRecord last = rows.OrderByDescending(x => x.MaintenanceDate).FirstOrDefault();
            litLastService.Text = last == null ? "—" : last.MaintenanceDate.ToString("dd.MM.yyyy", Localization.Culture);

            MaintenanceRecord due = rows.Where(x => x.NextMaintenanceDate.HasValue).OrderBy(x => x.NextMaintenanceDate.Value).FirstOrDefault();
            if (due == null)
                litNextDue.Text = "—";
            else
            {
                string date = due.NextMaintenanceDate.Value.ToString("dd.MM.yyyy", Localization.Culture);
                litNextDue.Text = due.NextMaintenanceDate.Value.Date < DateTime.Today
                    ? "<span class='overdue'>" + HttpUtility.HtmlEncode(date + " — " + Localization.Get("BusMaintenance_Overdue")) + "</span>"
                    : HttpUtility.HtmlEncode(date);
            }
        }

        private void BindMaintenanceHistory()
        {
            List<MaintenanceRecord> rows;
            using (MaintenanceService service = new MaintenanceService())
                rows = service.GetMaintenanceByBus(BusId);
            if (rows == null)
                rows = new List<MaintenanceRecord>();

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
            if (string.Equals(CurrentSortExpression, e.SortExpression, StringComparison.OrdinalIgnoreCase))
                CurrentSortDirection = CurrentSortDirection == "ASC" ? "DESC" : "ASC";
            else
            {
                CurrentSortExpression = e.SortExpression;
                CurrentSortDirection = "ASC";
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

            Literal rw = e.Row.FindControl("litRw") as Literal;
            if (rw != null)
            {
                string css = record.Roadworthiness == RoadworthinessStatus.NotOperational ? "rw-notoperational" :
                    record.Roadworthiness == RoadworthinessStatus.NeedsAttention ? "rw-attention" : "rw-operational";
                rw.Text = "<span class='" + css + "'>" + HttpUtility.HtmlEncode(record.Roadworthiness ?? string.Empty) + "</span>";
            }

            Literal cost = e.Row.FindControl("litCost") as Literal;
            if (cost != null)
                cost.Text = HttpUtility.HtmlEncode(record.MaintenanceCost.ToString("N2", Localization.Culture));

            Literal next = e.Row.FindControl("litNextDate") as Literal;
            if (next != null)
            {
                if (!record.NextMaintenanceDate.HasValue)
                    next.Text = "—";
                else
                {
                    string date = record.NextMaintenanceDate.Value.ToString("dd.MM.yyyy", Localization.Culture);
                    next.Text = record.NextMaintenanceDate.Value.Date < DateTime.Today
                        ? "<span class='overdue'>" + HttpUtility.HtmlEncode(date) + "</span>"
                        : HttpUtility.HtmlEncode(date);
                }
            }
        }

        protected void gvMaintenanceHistory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e == null || e.CommandName != "ViewRecord" || e.CommandArgument == null)
                return;

            int id;
            if (!int.TryParse(Convert.ToString(e.CommandArgument, CultureInfo.InvariantCulture), out id) || id <= 0)
                return;

            RedirectTo("~/Fleet/BusMaintenance.aspx?busId=" + BusId.ToString(CultureInfo.InvariantCulture) +
                "&maintenanceId=" + id.ToString(CultureInfo.InvariantCulture) + "&mode=view");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            RedirectTo("~/Fleet/Buses.aspx");
        }

        protected void btnEditBus_Click(object sender, EventArgs e)
        {
            if (!RequireWritePermission("bus.write"))
                return;
            if (BusId > 0)
                RedirectTo("~/Fleet/BusEdit.aspx?busId=" + BusId.ToString(CultureInfo.InvariantCulture) + "&mode=edit");
        }

        protected void btnMaintenance_Click(object sender, EventArgs e)
        {
            if (BusId > 0)
                RedirectTo("~/Fleet/BusMaintenance.aspx?busId=" + BusId.ToString(CultureInfo.InvariantCulture));
        }

        private void ShowError(string message)
        {
            pnlError.Visible = true;
            litError.Text = HttpUtility.HtmlEncode(message ?? string.Empty);
        }

        private void RedirectTo(string url)
        {
            Response.Redirect(ResolveUrl(url), false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private static string StatusMarkup(string status)
        {
            string css = status == BusStatus.Operational ? "status-operational" :
                status == BusStatus.InRepair ? "status-repair" :
                status == BusStatus.Retired ? "status-retired" : "status-reserve";
            return "<span class='" + css + "'>" + HttpUtility.HtmlEncode(status ?? string.Empty) + "</span>";
        }

        private static string GetMileageCategory(Bus bus)
        {
            if (!string.IsNullOrWhiteSpace(bus.MileageCategory))
                return bus.MileageCategory;
            if (bus.MileageKm >= 150000)
                return MileageCategory.High;
            if (bus.MileageKm >= 50000)
                return MileageCategory.Medium;
            return MileageCategory.Low;
        }
    }
}
