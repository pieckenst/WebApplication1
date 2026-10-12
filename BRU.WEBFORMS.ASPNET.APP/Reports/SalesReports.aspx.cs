using System;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BRU.WEBFORMS.ASPNET.APP;
using BRU.WEBFORMS.ASPNET.APP.Models;
using BRU.WEBFORMS.ASPNET.APP.Services;

namespace BRU.WEBFORMS.ASPNET.APP.Reports
{
    public partial class SalesReports : SecurePage
    {
        private const int PageSize = 50;
        private bool _exporting;
        private bool _templateControlsResolved;

        protected override string[] RequiredPermissions { get { return new[] { "report.read" }; } }

        protected global::System.Web.UI.WebControls.Label lblError;
        protected global::System.Web.UI.WebControls.TextBox txtDateFrom;
        protected global::System.Web.UI.WebControls.TextBox txtDateTo;
        protected global::System.Web.UI.WebControls.DropDownList ddlChannel;
        protected global::System.Web.UI.WebControls.DropDownList ddlStatus;
        protected global::System.Web.UI.WebControls.Button btnApply;
        protected global::System.Web.UI.WebControls.Button btnExport;
        protected global::System.Web.UI.WebControls.Literal litSaleCount;
        protected global::System.Web.UI.WebControls.Literal litTicketCount;
        protected global::System.Web.UI.WebControls.Literal litGross;
        protected global::System.Web.UI.WebControls.Literal litRefunds;
        protected global::System.Web.UI.WebControls.Literal litNet;
        protected global::System.Web.UI.WebControls.GridView gvSales;
        protected global::System.Web.UI.WebControls.LinkButton btnPreviousPage;
        protected global::System.Web.UI.WebControls.Label lblPageInfo;
        protected global::System.Web.UI.WebControls.LinkButton btnNextPage;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbSalesFilters;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbSalesList;

        protected void Page_Load(object sender, EventArgs e)
        {
            EnsureTemplateControlsResolved();
            if (!IsPostBack)
            {
                txtDateFrom.Text = DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                txtDateTo.Text = DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                BindReport();
            }
        }

        private void EnsureTemplateControlsResolved()
        {
            if (_templateControlsResolved) return;
            txtDateFrom = cbSalesFilters.FindContentControl<TextBox>("txtDateFrom");
            txtDateTo = cbSalesFilters.FindContentControl<TextBox>("txtDateTo");
            ddlChannel = cbSalesFilters.FindContentControl<DropDownList>("ddlChannel");
            ddlStatus = cbSalesFilters.FindContentControl<DropDownList>("ddlStatus");
            btnApply = cbSalesFilters.FindContentControl<Button>("btnApply");
            btnExport = cbSalesFilters.FindContentControl<Button>("btnExport");
            gvSales = cbSalesList.FindContentControl<GridView>("gvSales");
            btnPreviousPage = cbSalesList.FindContentControl<LinkButton>("btnPreviousPage");
            lblPageInfo = cbSalesList.FindContentControl<Label>("lblPageInfo");
            btnNextPage = cbSalesList.FindContentControl<LinkButton>("btnNextPage");
            if (txtDateFrom == null || txtDateTo == null || ddlChannel == null || ddlStatus == null ||
                btnApply == null || btnExport == null || gvSales == null || btnPreviousPage == null ||
                lblPageInfo == null || btnNextPage == null)
                throw new InvalidOperationException("Sales report controls were not created inside their ContentBox templates.");
            _templateControlsResolved = true;
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            gvSales.PageIndex = 0;
            BindReport();
        }

        protected void btnPreviousPage_Click(object sender, EventArgs e)
        {
            if (gvSales.PageIndex > 0)
                gvSales.PageIndex--;
            BindReport();
        }

        protected void btnNextPage_Click(object sender, EventArgs e)
        {
            gvSales.PageIndex++;
            BindReport();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                DateRange range = ReadDateRange();
                StringBuilder csv = new StringBuilder();
                csv.AppendLine("Sale ID,Sale time,Route,Ticket,Quantity,Unit price,Total,Channel,Sale status,Payment status");
                using (ReportsService service = new ReportsService())
                {
                    SalesReportData firstPage = service.GetSalesReport(range.From, range.ToExclusive,
                        GetSaleChannelFilterValue(ddlChannel.SelectedValue), GetSaleStatusFilterValue(ddlStatus.SelectedValue), 0, 10000);
                    if (firstPage.TotalRows > 100000)
                        throw new ServiceException("This export exceeds 100,000 rows. Narrow the date or status filters.");
                    AppendSalesRows(csv, firstPage);
                    int pageCount = (int)Math.Ceiling((double)firstPage.TotalRows / 10000);
                    for (int page = 1; page < pageCount; page++)
                    {
                        SalesReportData nextPage = service.GetSalesReport(range.From, range.ToExclusive,
                            GetSaleChannelFilterValue(ddlChannel.SelectedValue), GetSaleStatusFilterValue(ddlStatus.SelectedValue), page, 10000);
                        AppendSalesRows(csv, nextPage);
                    }
                }

                _exporting = true;
                Response.Clear();
                Response.ContentType = "text/csv";
                Response.ContentEncoding = Encoding.UTF8;
                Response.AddHeader("Content-Disposition", "attachment; filename=sales-report-" +
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

        private void BindReport()
        {
            try
            {
                DateRange range = ReadDateRange();
                SalesReportData report;
                int pageCount;
                using (ReportsService service = new ReportsService())
                {
                    report = service.GetSalesReport(range.From, range.ToExclusive,
                        GetSaleChannelFilterValue(ddlChannel.SelectedValue), GetSaleStatusFilterValue(ddlStatus.SelectedValue), gvSales.PageIndex, PageSize);
                    pageCount = Math.Max(1, (int)Math.Ceiling((double)report.TotalRows / PageSize));
                    if (gvSales.PageIndex >= pageCount)
                    {
                        gvSales.PageIndex = pageCount - 1;
                        report = service.GetSalesReport(range.From, range.ToExclusive,
                            GetSaleChannelFilterValue(ddlChannel.SelectedValue), GetSaleStatusFilterValue(ddlStatus.SelectedValue), gvSales.PageIndex, PageSize);
                    }
                }

                litSaleCount.Text = report.Summary.SaleCount.ToString("N0", CultureInfo.InvariantCulture);
                litTicketCount.Text = report.Summary.TicketCount.ToString("N0", CultureInfo.InvariantCulture);
                litGross.Text = report.Summary.GrossAmount.ToString("N2", CultureInfo.InvariantCulture);
                litRefunds.Text = report.Summary.RefundedAmount.ToString("N2", CultureInfo.InvariantCulture);
                litNet.Text = report.Summary.NetAmount.ToString("N2", CultureInfo.InvariantCulture);
                gvSales.DataSource = report.Rows;
                gvSales.DataBind();
                btnPreviousPage.Enabled = gvSales.PageIndex > 0;
                btnNextPage.Enabled = gvSales.PageIndex + 1 < pageCount;
                lblPageInfo.Text = "Page " + (gvSales.PageIndex + 1) + " of " + pageCount +
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

        private static string GetSaleChannelFilterValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            switch (value.Trim())
            {
                case "cash-desk": return SaleChannel.CashDesk;
                case "conductor": return SaleChannel.Conductor;
                case "validator": return SaleChannel.Validator;
                case "qr": return SaleChannel.QR;
                case "online": return SaleChannel.Online;
                case SaleChannel.CashDesk: return SaleChannel.CashDesk;
                case SaleChannel.Conductor: return SaleChannel.Conductor;
                case SaleChannel.Validator: return SaleChannel.Validator;
                case SaleChannel.QR: return SaleChannel.QR;
                case SaleChannel.Online: return SaleChannel.Online;
                default: return null;
            }
        }

        private static string GetSaleStatusFilterValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            switch (value.Trim())
            {
                case "completed": return SaleStatus.Completed;
                case "cancelled": return SaleStatus.Cancelled;
                case "refunded": return SaleStatus.Refunded;
                case SaleStatus.Completed: return SaleStatus.Completed;
                case SaleStatus.Cancelled: return SaleStatus.Cancelled;
                case SaleStatus.Refunded: return SaleStatus.Refunded;
                default: return null;
            }
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

        private static void AppendSalesRows(StringBuilder csv, SalesReportData report)
        {
            foreach (SalesReportRow row in report.Rows)
            {
                csv.Append(row.SaleId).Append(',')
                    .Append(Csv(row.SaleDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))).Append(',')
                    .Append(Csv(row.RouteNumber)).Append(',')
                    .Append(Csv(row.TicketName)).Append(',')
                    .Append(row.TicketQuantity).Append(',')
                    .Append(row.SalePrice.ToString("0.00", CultureInfo.InvariantCulture)).Append(',')
                    .Append(row.TotalAmount.ToString("0.00", CultureInfo.InvariantCulture)).Append(',')
                    .Append(Csv(row.SaleChannel)).Append(',')
                    .Append(Csv(row.SaleStatus)).Append(',')
                    .Append(Csv(row.PaymentStatus)).AppendLine();
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
