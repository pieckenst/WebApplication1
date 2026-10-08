using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BRU.WEBFORMS.ASPNET.APP.Services;
using BRU.WEBFORMS.ASPNET.APP.Models;
using System.Data.SqlClient;

namespace BRU.WEBFORMS.ASPNET.APP.Sales
{
    /// <summary>
    /// Payment Processing page for Autopark Management System.
    /// Provides payment record viewing, status filtering, new payment
    /// creation, detail view, sorting, pagination, and comprehensive
    /// payment statistics.
    /// </summary>
    public partial class Payments : SecurePage
    {
        protected override string[] RequiredPermissions { get { return new[] { "payment.read" }; } }
        #region Control Declarations

        protected global::System.Web.UI.WebControls.Panel pnlError;
        protected global::System.Web.UI.WebControls.Literal litError;
        protected global::System.Web.UI.WebControls.Panel pnlSuccess;
        protected global::System.Web.UI.WebControls.Literal litSuccess;
        protected global::System.Web.UI.WebControls.Literal litTotalPayments;
        protected global::System.Web.UI.WebControls.Literal litPaid;
        protected global::System.Web.UI.WebControls.Literal litPending;
        protected global::System.Web.UI.WebControls.Literal litErrors;
        protected global::System.Web.UI.WebControls.Literal litTotalAmount;
        protected global::System.Web.UI.WebControls.Button btnNewPayment;
        protected global::System.Web.UI.WebControls.Button btnRefresh;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbFilters;
        protected global::System.Web.UI.WebControls.DropDownList ddlStatusFilter;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbPaymentsList;
        protected global::System.Web.UI.WebControls.GridView gvPayments;
        protected global::System.Web.UI.WebControls.Literal litPagination;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbPaymentForm;
        protected global::System.Web.UI.WebControls.Panel pnlPaymentForm;
        protected global::System.Web.UI.WebControls.TextBox txtSaleId;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvSaleId;
        protected global::System.Web.UI.WebControls.TextBox txtAmount;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvAmount;
        protected global::System.Web.UI.WebControls.DropDownList ddlMethod;
        protected global::System.Web.UI.WebControls.TextBox txtTransactionId;
        protected global::System.Web.UI.WebControls.Button btnSavePayment;
        protected global::System.Web.UI.WebControls.Button btnCancelPayment;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbPaymentDetail;
        protected global::System.Web.UI.WebControls.Literal litPaymentDetail;
        protected global::System.Web.UI.WebControls.Button btnDetailClose;

        #endregion

        #region Private Fields

        private List<Payment> _currentPaymentList;
        private const int _pageSize = 20;
        private bool _templateControlsResolved;

        #endregion

        #region State Properties

        private int CurrentPage
        {
            get
            {
                object val = ViewState["Payments_CurrentPage"];
                if (val == null) return 1;
                int page;
                if (int.TryParse(val.ToString(), out page) && page > 0) return page;
                return 1;
            }
            set { ViewState["Payments_CurrentPage"] = value < 1 ? 1 : value; }
        }

        private string CurrentSortExpression
        {
            get
            {
                object val = ViewState["Payments_SortExpr"];
                return val == null ? "payment_date" : val.ToString();
            }
            set { ViewState["Payments_SortExpr"] = value; }
        }

        private string CurrentSortDirection
        {
            get
            {
                object val = ViewState["Payments_SortDir"];
                return val == null ? "DESC" : val.ToString();
            }
            set { ViewState["Payments_SortDir"] = value; }
        }

        private string FilterStatus
        {
            get
            {
                object val = ViewState["Payments_FilterStatus"];
                return val == null ? string.Empty : val.ToString();
            }
            set { ViewState["Payments_FilterStatus"] = value; }
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
                    LoadPayments();
                    LoadStats();
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
            if (_templateControlsResolved) return;

            EnsureContentBoxCreated(cbFilters, "cbFilters");
            EnsureContentBoxCreated(cbPaymentsList, "cbPaymentsList");
            EnsureContentBoxCreated(cbPaymentForm, "cbPaymentForm");
            EnsureContentBoxCreated(cbPaymentDetail, "cbPaymentDetail");

            // Resolve filter controls
            ddlStatusFilter = FindRequiredTemplateControl<DropDownList>(cbFilters, "ddlStatusFilter");

            // Resolve list controls
            gvPayments = FindRequiredTemplateControl<GridView>(cbPaymentsList, "gvPayments");

            // Resolve form controls
            pnlPaymentForm = FindRequiredTemplateControl<Panel>(cbPaymentForm, "pnlPaymentForm");
            txtSaleId = FindRequiredTemplateControl<TextBox>(cbPaymentForm, "txtSaleId");
            rfvSaleId = FindRequiredTemplateControl<RequiredFieldValidator>(cbPaymentForm, "rfvSaleId");
            txtAmount = FindRequiredTemplateControl<TextBox>(cbPaymentForm, "txtAmount");
            rfvAmount = FindRequiredTemplateControl<RequiredFieldValidator>(cbPaymentForm, "rfvAmount");
            ddlMethod = FindRequiredTemplateControl<DropDownList>(cbPaymentForm, "ddlMethod");
            txtTransactionId = FindRequiredTemplateControl<TextBox>(cbPaymentForm, "txtTransactionId");
            btnSavePayment = FindRequiredTemplateControl<Button>(cbPaymentForm, "btnSavePayment");
            btnCancelPayment = FindRequiredTemplateControl<Button>(cbPaymentForm, "btnCancelPayment");

            // Resolve detail controls
            litPaymentDetail = FindRequiredTemplateControl<Literal>(cbPaymentDetail, "litPaymentDetail");
            btnDetailClose = FindRequiredTemplateControl<Button>(cbPaymentDetail, "btnDetailClose");

            _templateControlsResolved = true;
        }

        private static void EnsureContentBoxCreated(Controls.ContentBox contentBox, string controlId)
        {
            if (contentBox == null)
                throw new InvalidOperationException(
                    "Required ContentBox '" + controlId + "' was not created. Check Payments.aspx markup and the ContentBox registration.");
        }

        private static T FindRequiredTemplateControl<T>(Controls.ContentBox contentBox, string controlId) where T : Control
        {
            T control = contentBox.FindContentControl<T>(controlId);
            if (control == null)
                throw new InvalidOperationException(
                    "Required control '" + controlId + "' was not found inside ContentBox '" +
                    contentBox.ID + "'. Check that the control is inside the ContentTemplate and has runat=\"server\".");
            return control;
        }

        #endregion

        #region Data Loading

        private void LoadPayments()
        {
            using (TicketService service = new TicketService())
            {
                _currentPaymentList = service.GetAllPayments();
            }

            ApplyFiltersAndSort();
            BindGrid();
        }

        private void LoadStats()
        {
            List<Payment> payments;
            using (TicketService service = new TicketService())
            {
                payments = service.GetAllPayments();
            }

            if (payments == null) payments = new List<Payment>();

            int paidCount = 0, pendingCount = 0, errorCount = 0;
            decimal totalAmount = 0;

            foreach (Payment p in payments)
            {
                if (p.PaymentStatus == PaymentStatus.Paid)
                {
                    paidCount++;
                    totalAmount += p.Amount;
                }
                else if (p.PaymentStatus == PaymentStatus.Pending)
                    pendingCount++;
                else if (p.PaymentStatus == PaymentStatus.Error)
                    errorCount++;
            }

            litTotalPayments.Text = payments.Count.ToString();
            litPaid.Text = paidCount.ToString();
            litPending.Text = pendingCount.ToString();
            litErrors.Text = errorCount.ToString();
            litTotalAmount.Text = totalAmount.ToString("F2");
        }

        private void ApplyFiltersAndSort()
        {
            if (_currentPaymentList == null) _currentPaymentList = new List<Payment>();

            // Apply status filter
            if (!string.IsNullOrEmpty(FilterStatus))
            {
                List<Payment> filtered = new List<Payment>();
                foreach (Payment p in _currentPaymentList)
                {
                    if (p.PaymentStatus == FilterStatus) filtered.Add(p);
                }
                _currentPaymentList = filtered;
            }

            ApplySorting();
        }

        private void ApplySorting()
        {
            if (_currentPaymentList == null || _currentPaymentList.Count <= 1) return;

            string sortExpr = CurrentSortExpression;
            bool ascending = CurrentSortDirection == "ASC";

            Comparison<Payment> comparison = null;
            switch (sortExpr)
            {
                case "payment_id":
                    comparison = delegate(Payment a, Payment b) { return a.PaymentId.CompareTo(b.PaymentId); };
                    break;
                case "sale_id":
                    comparison = delegate(Payment a, Payment b) { return a.SaleId.CompareTo(b.SaleId); };
                    break;
                case "payment_date":
                    comparison = delegate(Payment a, Payment b) { return a.PaymentDate.CompareTo(b.PaymentDate); };
                    break;
                case "amount":
                    comparison = delegate(Payment a, Payment b) { return a.Amount.CompareTo(b.Amount); };
                    break;
                case "payment_method":
                    comparison = delegate(Payment a, Payment b) { return string.Compare(a.PaymentMethod ?? "", b.PaymentMethod ?? "", StringComparison.Ordinal); };
                    break;
                case "payment_status":
                    comparison = delegate(Payment a, Payment b) { return string.Compare(a.PaymentStatus ?? "", b.PaymentStatus ?? "", StringComparison.Ordinal); };
                    break;
                case "transaction_id":
                    comparison = delegate(Payment a, Payment b) { return string.Compare(a.TransactionId ?? "", b.TransactionId ?? "", StringComparison.Ordinal); };
                    break;
                case "control_status":
                    comparison = delegate(Payment a, Payment b) { return string.Compare(a.ControlStatus ?? "", b.ControlStatus ?? "", StringComparison.Ordinal); };
                    break;
                default:
                    comparison = delegate(Payment a, Payment b) { return a.PaymentDate.CompareTo(b.PaymentDate); };
                    break;
            }

            _currentPaymentList.Sort(comparison);
            if (!ascending) _currentPaymentList.Reverse();
        }

        private void BindGrid()
        {
            int totalCount = _currentPaymentList != null ? _currentPaymentList.Count : 0;
            int totalPages = (int)Math.Ceiling((double)totalCount / _pageSize);
            if (totalPages == 0) totalPages = 1;
            if (CurrentPage > totalPages) CurrentPage = totalPages;

            gvPayments.PageSize = _pageSize;
            gvPayments.PageIndex = CurrentPage - 1;
            gvPayments.DataSource = _currentPaymentList;
            gvPayments.DataBind();
            RenderPagination(totalPages, totalCount);
        }

        private void RenderPagination(int totalPages, int totalCount)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<span class='text-small'>Total: ").Append(totalCount).Append(" payments");
            if (totalPages > 1)
                sb.Append(" | Page ").Append(CurrentPage).Append(" of ").Append(totalPages);
            sb.Append("</span>");
            litPagination.Text = sb.ToString();
        }

        #endregion

        #region Grid Events

        protected void gvPayments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CurrentPage = e.NewPageIndex + 1;
            LoadPayments();
        }

        protected void gvPayments_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression == CurrentSortExpression)
                CurrentSortDirection = CurrentSortDirection == "ASC" ? "DESC" : "ASC";
            else
            {
                CurrentSortExpression = e.SortExpression;
                CurrentSortDirection = "ASC";
            }
            CurrentPage = 1;
            LoadPayments();
        }

        protected void gvPayments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandArgument == null) return;
            long paymentId;
            if (!long.TryParse(e.CommandArgument.ToString(), out paymentId)) return;

            switch (e.CommandName)
            {
                case "ViewPayment":
                    ShowPaymentDetail(paymentId);
                    break;
            }
        }

        protected void gvPayments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Payment payment = (Payment)e.Row.DataItem;

                // Payment status coloring
                Literal litPayStatus = (Literal)e.Row.FindControl("litPayStatus");
                if (litPayStatus != null)
                {
                    string cssClass;
                    switch (payment.PaymentStatus)
                    {
                        case PaymentStatus.Pending: cssClass = "pay-pending"; break;
                        case PaymentStatus.Paid: cssClass = "pay-paid"; break;
                        case PaymentStatus.Error: cssClass = "pay-error"; break;
                        case PaymentStatus.Refunded: cssClass = "pay-refunded"; break;
                        default: cssClass = "pay-pending"; break;
                    }
                    litPayStatus.Text = "<span class='" + cssClass + "'>" + payment.PaymentStatus + "</span>";
                }

                // Control status coloring
                Literal litCtrlStatus = (Literal)e.Row.FindControl("litCtrlStatus");
                if (litCtrlStatus != null)
                {
                    string ctrl = payment.ControlStatus ?? "";
                    string cssClass;
                    if (ctrl.IndexOf("ок", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        ctrl.IndexOf("OK", StringComparison.OrdinalIgnoreCase) >= 0)
                        cssClass = "ctrl-ok";
                    else if (ctrl.IndexOf("ожид", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        ctrl.IndexOf("pending", StringComparison.OrdinalIgnoreCase) >= 0)
                        cssClass = "ctrl-pending";
                    else
                        cssClass = "ctrl-error";

                    litCtrlStatus.Text = "<span class='" + cssClass + "'>" + Server.HtmlEncode(ctrl) + "</span>";
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

        protected void btnNewPayment_Click(object sender, EventArgs e)
        {
            cbPaymentForm.Visible = true;
            cbPaymentDetail.Visible = false;
            pnlError.Visible = false;
            txtSaleId.Text = string.Empty;
            txtAmount.Text = string.Empty;
            txtTransactionId.Text = string.Empty;
            ddlMethod.SelectedIndex = 0;
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            FilterStatus = string.Empty;
            CurrentSortExpression = "payment_date";
            CurrentSortDirection = "DESC";
            ddlStatusFilter.SelectedIndex = 0;
            cbPaymentForm.Visible = false;
            cbPaymentDetail.Visible = false;
            LoadPayments();
            LoadStats();
            ShowSuccess("Payment list refreshed successfully.");
        }

        protected void ddlStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterStatus = ddlStatusFilter.SelectedValue;
            CurrentPage = 1;
            LoadPayments();
        }

        protected void btnSavePayment_Click(object sender, EventArgs e)
        {
            try
            {
                RequireWritePermission("payment.write");
                Payment payment = new Payment();

                long saleId;
                if (!long.TryParse(txtSaleId.Text.Trim(), out saleId) || saleId <= 0)
                {
                    ShowError("Invalid Sale ID.");
                    return;
                }
                payment.SaleId = saleId;

                decimal amount;
                if (!decimal.TryParse(txtAmount.Text.Trim(), out amount) || amount < 0)
                {
                    ShowError("Invalid amount.");
                    return;
                }
                payment.Amount = amount;

                payment.PaymentMethod = ddlMethod.SelectedValue;
                payment.PaymentStatus = PaymentStatus.Paid;
                payment.TransactionId = string.IsNullOrWhiteSpace(txtTransactionId.Text)
                    ? null : txtTransactionId.Text.Trim();

                using (TicketService service = new TicketService())
                {
                    long paymentId = service.CreatePayment(payment);
                    ShowSuccess("Payment created successfully. Payment ID: " + paymentId);
                }

                cbPaymentForm.Visible = false;
                LoadPayments();
                LoadStats();
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

        protected void btnCancelPayment_Click(object sender, EventArgs e)
        {
            cbPaymentForm.Visible = false;
            pnlError.Visible = false;
        }

        protected void btnDetailClose_Click(object sender, EventArgs e)
        {
            cbPaymentDetail.Visible = false;
        }

        #endregion

        #region Detail View

        private void ShowPaymentDetail(long paymentId)
        {
            try
            {
                Payment payment = null;
                using (TicketService service = new TicketService())
                {
                    List<Payment> allPayments = service.GetAllPayments();
                    if (allPayments != null)
                    {
                        foreach (Payment p in allPayments)
                        {
                            if (p.PaymentId == paymentId) { payment = p; break; }
                        }
                    }
                }

                if (payment == null)
                {
                    ShowError("Payment not found.");
                    return;
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("<div class='detail-section'>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Payment ID:</span><span class='detail-value'>").Append(payment.PaymentId).Append("</span></div>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Sale ID:</span><span class='detail-value'>").Append(payment.SaleId).Append("</span></div>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Date:</span><span class='detail-value'>").Append(payment.PaymentDate.ToString("dd.MM.yyyy HH:mm")).Append("</span></div>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Amount:</span><span class='detail-value'>").Append(payment.Amount.ToString("F2")).Append("</span></div>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Method:</span><span class='detail-value'>").Append(Server.HtmlEncode(payment.PaymentMethod ?? "—")).Append("</span></div>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Status:</span><span class='detail-value'>").Append(Server.HtmlEncode(payment.PaymentStatus ?? "—")).Append("</span></div>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Transaction ID:</span><span class='detail-value'>").Append(Server.HtmlEncode(payment.TransactionId ?? "—")).Append("</span></div>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Control Status:</span><span class='detail-value'>").Append(Server.HtmlEncode(payment.ControlStatus ?? "—")).Append("</span></div>");
                sb.Append("</div>");

                litPaymentDetail.Text = sb.ToString();
                cbPaymentDetail.Visible = true;
                cbPaymentForm.Visible = false;
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
                default: errorMsg = "Database error (Code " + sqlEx.Number + "): " + sqlEx.Message; break;
            }
            ShowError(errorMsg);
            System.Diagnostics.Debug.WriteLine("Payments SQL Error: " + sqlEx.ToString());
        }

        private void HandleServiceError(ServiceException svcEx)
        {
            string msg = svcEx.Message;
            if (svcEx.InnerException != null) msg += " (" + svcEx.InnerException.Message + ")";
            ShowError(msg);
            System.Diagnostics.Debug.WriteLine("Payments Service Error: " + svcEx.ToString());
        }

        private void HandleGenericError(Exception ex)
        {
            ShowError("Unexpected error: " + ex.Message);
            System.Diagnostics.Debug.WriteLine("Payments Generic Error: " + ex.ToString());
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
