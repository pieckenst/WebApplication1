using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BRU.WEBFORMS.ASPNET.APP;
using BRU.WEBFORMS.ASPNET.APP.Models;
using BRU.WEBFORMS.ASPNET.APP.Services;

namespace BRU.WEBFORMS.ASPNET.APP.Reports
{
    public partial class MaintenanceReports : SecurePage
    {
        private const int PageSize = 50;
        private bool _exporting;
        private bool _templateControlsResolved;

        protected override string[] RequiredPermissions { get { return new[] { "report.read" }; } }

        protected global::System.Web.UI.WebControls.Label lblError;
        protected global::System.Web.UI.WebControls.TextBox txtDateFrom;
        protected global::System.Web.UI.WebControls.TextBox txtDateTo;
        protected global::System.Web.UI.WebControls.DropDownList ddlBus;
        protected global::System.Web.UI.WebControls.DropDownList ddlRoadworthiness;
        protected global::System.Web.UI.WebControls.Button btnApply;
        protected global::System.Web.UI.WebControls.Button btnExport;
        protected global::System.Web.UI.WebControls.Literal litRecordCount;
        protected global::System.Web.UI.WebControls.Literal litTotalCost;
        protected global::System.Web.UI.WebControls.Literal litNotRoadworthy;
        protected global::System.Web.UI.WebControls.Literal litUpcoming;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbMaintenanceFilters;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbMaintenanceList;
        protected global::System.Web.UI.WebControls.GridView gvMaintenance;
        protected global::System.Web.UI.WebControls.LinkButton btnPreviousPage;
        protected global::System.Web.UI.WebControls.Label lblPageInfo;
        protected global::System.Web.UI.WebControls.LinkButton btnNextPage;

        protected void Page_Load(object sender, EventArgs e)
        {
            EnsureTemplateControlsResolved();
            if (!IsPostBack)
            {
                txtDateFrom.Text = DateTime.Today.AddDays(-365).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                txtDateTo.Text = DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                LoadBuses();
                BindReport();
            }
        }

        private void EnsureTemplateControlsResolved()
        {
            if (_templateControlsResolved)
                return;

            txtDateFrom = cbMaintenanceFilters.FindContentControl<TextBox>("txtDateFrom");
            txtDateTo = cbMaintenanceFilters.FindContentControl<TextBox>("txtDateTo");
            ddlBus = cbMaintenanceFilters.FindContentControl<DropDownList>("ddlBus");
            ddlRoadworthiness = cbMaintenanceFilters.FindContentControl<DropDownList>("ddlRoadworthiness");
            btnApply = cbMaintenanceFilters.FindContentControl<Button>("btnApply");
            btnExport = cbMaintenanceFilters.FindContentControl<Button>("btnExport");
            gvMaintenance = cbMaintenanceList.FindContentControl<GridView>("gvMaintenance");
            btnPreviousPage = cbMaintenanceList.FindContentControl<LinkButton>("btnPreviousPage");
            lblPageInfo = cbMaintenanceList.FindContentControl<Label>("lblPageInfo");
            btnNextPage = cbMaintenanceList.FindContentControl<LinkButton>("btnNextPage");

            if (txtDateFrom == null || txtDateTo == null || ddlBus == null || ddlRoadworthiness == null ||
                btnApply == null || btnExport == null || gvMaintenance == null || btnPreviousPage == null ||
                lblPageInfo == null || btnNextPage == null)
                throw new InvalidOperationException("Maintenance report controls were not created inside their ContentBox templates.");

            _templateControlsResolved = true;
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            gvMaintenance.PageIndex = 0;
            BindReport();
        }

        protected void btnPreviousPage_Click(object sender, EventArgs e)
        {
            if (gvMaintenance.PageIndex > 0)
                gvMaintenance.PageIndex--;
            BindReport();
        }

        protected void btnNextPage_Click(object sender, EventArgs e)
        {
            gvMaintenance.PageIndex++;
            BindReport();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                DateRange range = ReadDateRange();
                int? busId = ReadBusFilter();
                StringBuilder csv = new StringBuilder();
                csv.AppendLine("Maintenance ID,Service date,Bus,Model,Service,State,Cost,Next service,Mechanic");
                using (ReportsService service = new ReportsService())
                {
                    MaintenanceReportData firstPage = service.GetMaintenanceReport(range.From, range.ToExclusive,
                        busId, ddlRoadworthiness.SelectedValue, 0, 10000);
                    if (firstPage.TotalRows > 100000)
                        throw new ServiceException("This export exceeds 100,000 rows. Narrow the date or bus filters.");

                    AppendRows(csv, firstPage);
                    int pageCount = (int)Math.Ceiling((double)firstPage.TotalRows / 10000);
                    for (int page = 1; page < pageCount; page++)
                    {
                        MaintenanceReportData nextPage = service.GetMaintenanceReport(range.From, range.ToExclusive,
                            busId, ddlRoadworthiness.SelectedValue, page, 10000);
                        AppendRows(csv, nextPage);
                    }
                }

                _exporting = true;
                Response.Clear();
                Response.ContentType = "text/csv";
                Response.ContentEncoding = Encoding.UTF8;
                Response.AddHeader("Content-Disposition", "attachment; filename=maintenance-report-" +
                    range.From.ToString("yyyyMMdd", CultureInfo.InvariantCulture) + "-" +
                    range.ToExclusive.AddDays(-1).ToString("yyyyMMdd", CultureInfo.InvariantCulture) + ".csv");
                Response.BinaryWrite(Encoding.UTF8.GetPreamble());
                Response.Write(csv.ToString());
                Response.Flush();
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (ServiceException ex)
            {
                ShowReportError(ex.Message);
            }
            catch (SqlException ex)
            {
                HandleDatabaseError(ex);
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!_exporting)
                base.Render(writer);
        }

        private void LoadBuses()
        {
            using (BusService service = new BusService())
            {
                foreach (Bus bus in service.GetAllBuses())
                    ddlBus.Items.Add(new ListItem(bus.FleetNumber + " - " + bus.Model, bus.BusId.ToString(CultureInfo.InvariantCulture)));
            }
        }

        private void BindReport()
        {
            try
            {
                DateRange range = ReadDateRange();
                MaintenanceReportData report;
                int pageCount;
                using (ReportsService service = new ReportsService())
                {
                    report = service.GetMaintenanceReport(range.From, range.ToExclusive,
                        ReadBusFilter(), ddlRoadworthiness.SelectedValue, gvMaintenance.PageIndex, PageSize);
                    pageCount = Math.Max(1, (int)Math.Ceiling((double)report.TotalRows / PageSize));
                    if (gvMaintenance.PageIndex >= pageCount)
                    {
                        gvMaintenance.PageIndex = pageCount - 1;
                        report = service.GetMaintenanceReport(range.From, range.ToExclusive,
                            ReadBusFilter(), ddlRoadworthiness.SelectedValue, gvMaintenance.PageIndex, PageSize);
                    }
                }

                litRecordCount.Text = report.Summary.RecordCount.ToString("N0", CultureInfo.InvariantCulture);
                litTotalCost.Text = report.Summary.TotalCost.ToString("N2", CultureInfo.InvariantCulture);
                litNotRoadworthy.Text = report.Summary.NotRoadworthyCount.ToString("N0", CultureInfo.InvariantCulture);
                litUpcoming.Text = report.Summary.UpcomingCount.ToString("N0", CultureInfo.InvariantCulture);
                gvMaintenance.DataSource = report.Rows;
                gvMaintenance.DataBind();
                btnPreviousPage.Enabled = gvMaintenance.PageIndex > 0;
                btnNextPage.Enabled = gvMaintenance.PageIndex + 1 < pageCount;
                lblPageInfo.Text = "Page " + (gvMaintenance.PageIndex + 1) + " of " + pageCount +
                    " (" + report.TotalRows.ToString("N0", CultureInfo.InvariantCulture) + " rows)";
                lblError.Visible = false;
            }
            catch (ServiceException ex)
            {
                ShowReportError(ex.Message);
            }
            catch (SqlException ex)
            {
                HandleDatabaseError(ex);
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        private int? ReadBusFilter()
        {
            if (string.IsNullOrEmpty(ddlBus.SelectedValue)) return null;
            int busId;
            if (!int.TryParse(ddlBus.SelectedValue, out busId))
                throw new ServiceException("Select a valid bus filter.");
            return busId;
        }

        private DateRange ReadDateRange()
        {
            DateTime dateFrom;
            DateTime dateTo;
            if (!DateTime.TryParseExact(txtDateFrom.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateFrom) ||
                !DateTime.TryParseExact(txtDateTo.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTo))
                throw new ServiceException("Enter valid start and end dates.");
            if (dateTo < dateFrom)
                throw new ServiceException("The end date cannot be before the start date.");
            return new DateRange { From = dateFrom.Date, ToExclusive = dateTo.Date.AddDays(1) };
        }

        private static void AppendRows(StringBuilder csv, MaintenanceReportData report)
        {
            foreach (MaintenanceReportRow row in report.Rows)
            {
                csv.Append(row.MaintenanceId).Append(',')
                    .Append(Csv(row.MaintenanceDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))).Append(',')
                    .Append(Csv(row.FleetNumber)).Append(',')
                    .Append(Csv(row.BusModel)).Append(',')
                    .Append(Csv(row.MaintenanceType)).Append(',')
                    .Append(Csv(row.Roadworthiness)).Append(',')
                    .Append(row.MaintenanceCost.ToString("0.00", CultureInfo.InvariantCulture)).Append(',')
                    .Append(Csv(row.NextMaintenanceDate.HasValue ? row.NextMaintenanceDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : string.Empty)).Append(',')
                    .Append(Csv(row.EmployeeName)).AppendLine();
            }
        }

        private static string Csv(string value)
        {
            value = value ?? string.Empty;
            if (value.Length > 0 && "=+-@\t\r".IndexOf(value[0]) >= 0)
                value = "'" + value;
            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }

        private void ShowReportError(string message)
        {
            lblError.Text = Server.HtmlEncode(message);
            lblError.Visible = true;
        }

        private sealed class DateRange
        {
            public DateTime From { get; set; }
            public DateTime ToExclusive { get; set; }
        }
    }
}
