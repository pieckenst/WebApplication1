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
    /// Sales Transactions page for Autopark Management System.
    /// Provides sales record viewing, date range filtering, channel/status
    /// filtering, new sale creation, detail view, sorting, pagination,
    /// and comprehensive revenue statistics.
    /// </summary>
    public partial class SalesPage : System.Web.UI.Page
    {
        #region Control Declarations

        protected global::System.Web.UI.WebControls.Panel pnlError;
        protected global::System.Web.UI.WebControls.Literal litError;
        protected global::System.Web.UI.WebControls.Panel pnlSuccess;
        protected global::System.Web.UI.WebControls.Literal litSuccess;
        protected global::System.Web.UI.WebControls.Literal litTotalSales;
        protected global::System.Web.UI.WebControls.Literal litCompleted;
        protected global::System.Web.UI.WebControls.Literal litTotalTickets;
        protected global::System.Web.UI.WebControls.Literal litTotalRevenue;
        protected global::System.Web.UI.WebControls.Literal litAvgSale;
        protected global::System.Web.UI.WebControls.Button btnNewSale;
        protected global::System.Web.UI.WebControls.Button btnRefresh;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbFilters;
        protected global::System.Web.UI.WebControls.TextBox txtDateFrom;
        protected global::System.Web.UI.WebControls.TextBox txtDateTo;
        protected global::System.Web.UI.WebControls.Button btnApplyDateRange;
        protected global::System.Web.UI.WebControls.DropDownList ddlChannelFilter;
        protected global::System.Web.UI.WebControls.DropDownList ddlStatusFilter;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbSalesList;
        protected global::System.Web.UI.WebControls.GridView gvSales;
        protected global::System.Web.UI.WebControls.Literal litPagination;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbSaleForm;
        protected global::System.Web.UI.WebControls.Panel pnlSaleForm;
        protected global::System.Web.UI.WebControls.DropDownList ddlTicket;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvTicket;
        protected global::System.Web.UI.WebControls.TextBox txtQuantity;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvQuantity;
        protected global::System.Web.UI.WebControls.TextBox txtSalePrice;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvSalePrice;
        protected global::System.Web.UI.WebControls.DropDownList ddlChannel;
        protected global::System.Web.UI.WebControls.DropDownList ddlCashier;
        protected global::System.Web.UI.WebControls.Button btnSaveSale;
        protected global::System.Web.UI.WebControls.Button btnCancelSale;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbSaleDetail;
        protected global::System.Web.UI.WebControls.Literal litSaleDetail;
        protected global::System.Web.UI.WebControls.Button btnDetailClose;

        #endregion

        #region Private Fields

        private List<Sale> _currentSaleList;
        private const int _pageSize = 20;
        private bool _templateControlsResolved;

        #endregion

        #region State Properties

        private int CurrentPage
        {
            get
            {
                object val = ViewState["Sales_CurrentPage"];
                if (val == null) return 1;
                int page;
                if (int.TryParse(val.ToString(), out page) && page > 0) return page;
                return 1;
            }
            set { ViewState["Sales_CurrentPage"] = value < 1 ? 1 : value; }
        }

        private string CurrentSortExpression
        {
            get
            {
                object val = ViewState["Sales_SortExpr"];
                return val == null ? "sale_date" : val.ToString();
            }
            set { ViewState["Sales_SortExpr"] = value; }
        }

        private string CurrentSortDirection
        {
            get
            {
                object val = ViewState["Sales_SortDir"];
                return val == null ? "DESC" : val.ToString();
            }
            set { ViewState["Sales_SortDir"] = value; }
        }

        private DateTime? FilterDateFrom
        {
            get
            {
                object val = ViewState["Sales_DateFrom"];
                if (val == null) return null;
                DateTime dt;
                if (DateTime.TryParse(val.ToString(), out dt)) return dt;
                return null;
            }
            set { ViewState["Sales_DateFrom"] = value; }
        }

        private DateTime? FilterDateTo
        {
            get
            {
                object val = ViewState["Sales_DateTo"];
                if (val == null) return null;
                DateTime dt;
                if (DateTime.TryParse(val.ToString(), out dt)) return dt;
                return null;
            }
            set { ViewState["Sales_DateTo"] = value; }
        }

        private string FilterChannel
        {
            get
            {
                object val = ViewState["Sales_FilterChannel"];
                return val == null ? string.Empty : val.ToString();
            }
            set { ViewState["Sales_FilterChannel"] = value; }
        }

        private string FilterStatus
        {
            get
            {
                object val = ViewState["Sales_FilterStatus"];
                return val == null ? string.Empty : val.ToString();
            }
            set { ViewState["Sales_FilterStatus"] = value; }
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
                    LoadSales();
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
            EnsureContentBoxCreated(cbSalesList, "cbSalesList");
            EnsureContentBoxCreated(cbSaleForm, "cbSaleForm");
            EnsureContentBoxCreated(cbSaleDetail, "cbSaleDetail");

            // Resolve filter controls
            txtDateFrom = FindRequiredTemplateControl<TextBox>(cbFilters, "txtDateFrom");
            txtDateTo = FindRequiredTemplateControl<TextBox>(cbFilters, "txtDateTo");
            btnApplyDateRange = FindRequiredTemplateControl<Button>(cbFilters, "btnApplyDateRange");
            ddlChannelFilter = FindRequiredTemplateControl<DropDownList>(cbFilters, "ddlChannelFilter");
            ddlStatusFilter = FindRequiredTemplateControl<DropDownList>(cbFilters, "ddlStatusFilter");

            // Resolve list controls
            gvSales = FindRequiredTemplateControl<GridView>(cbSalesList, "gvSales");

            // Resolve form controls
            pnlSaleForm = FindRequiredTemplateControl<Panel>(cbSaleForm, "pnlSaleForm");
            ddlTicket = FindRequiredTemplateControl<DropDownList>(cbSaleForm, "ddlTicket");
            rfvTicket = FindRequiredTemplateControl<RequiredFieldValidator>(cbSaleForm, "rfvTicket");
            txtQuantity = FindRequiredTemplateControl<TextBox>(cbSaleForm, "txtQuantity");
            rfvQuantity = FindRequiredTemplateControl<RequiredFieldValidator>(cbSaleForm, "rfvQuantity");
            txtSalePrice = FindRequiredTemplateControl<TextBox>(cbSaleForm, "txtSalePrice");
            rfvSalePrice = FindRequiredTemplateControl<RequiredFieldValidator>(cbSaleForm, "rfvSalePrice");
            ddlChannel = FindRequiredTemplateControl<DropDownList>(cbSaleForm, "ddlChannel");
            ddlCashier = FindRequiredTemplateControl<DropDownList>(cbSaleForm, "ddlCashier");
            btnSaveSale = FindRequiredTemplateControl<Button>(cbSaleForm, "btnSaveSale");
            btnCancelSale = FindRequiredTemplateControl<Button>(cbSaleForm, "btnCancelSale");

            // Resolve detail controls
            litSaleDetail = FindRequiredTemplateControl<Literal>(cbSaleDetail, "litSaleDetail");
            btnDetailClose = FindRequiredTemplateControl<Button>(cbSaleDetail, "btnDetailClose");

            _templateControlsResolved = true;
        }

        private static void EnsureContentBoxCreated(Controls.ContentBox contentBox, string controlId)
        {
            if (contentBox == null)
                throw new InvalidOperationException(
                    "Required ContentBox '" + controlId + "' was not created. Check Sales.aspx markup and the ContentBox registration.");
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

        private void LoadSales()
        {
            using (TicketService service = new TicketService())
            {
                if (FilterDateFrom.HasValue && FilterDateTo.HasValue)
                    _currentSaleList = service.GetSalesByDateRange(FilterDateFrom.Value, FilterDateTo.Value);
                else if (!string.IsNullOrEmpty(FilterChannel))
                    _currentSaleList = service.GetSalesByChannel(FilterChannel);
                else
                    _currentSaleList = service.GetAllSales();
            }

            ApplyFiltersAndSort();
            BindGrid();
        }

        private void LoadStats()
        {
            List<Sale> sales;
            using (TicketService service = new TicketService())
            {
                sales = service.GetAllSales();
            }

            if (sales == null) sales = new List<Sale>();

            litTotalSales.Text = sales.Count.ToString();

            int completedCount = 0, totalTickets = 0;
            decimal totalRevenue = 0;

            foreach (Sale s in sales)
            {
                if (s.SaleStatus == SaleStatus.Completed)
                {
                    completedCount++;
                    totalRevenue += s.SaleTotal;
                    totalTickets += s.TicketQuantity;
                }
            }

            litCompleted.Text = completedCount.ToString();
            litTotalTickets.Text = totalTickets.ToString();
            litTotalRevenue.Text = totalRevenue.ToString("F2");
            litAvgSale.Text = completedCount > 0
                ? (totalRevenue / completedCount).ToString("F2")
                : "0.00";
        }

        private void ApplyFiltersAndSort()
        {
            if (_currentSaleList == null) _currentSaleList = new List<Sale>();

            // Apply channel filter if not already filtered at DB level
            if (!string.IsNullOrEmpty(FilterChannel) &&
                (!FilterDateFrom.HasValue || !FilterDateTo.HasValue))
            {
                List<Sale> filtered = new List<Sale>();
                foreach (Sale s in _currentSaleList)
                {
                    if (s.SaleChannel == FilterChannel) filtered.Add(s);
                }
                _currentSaleList = filtered;
            }

            // Apply status filter
            if (!string.IsNullOrEmpty(FilterStatus))
            {
                List<Sale> filtered = new List<Sale>();
                foreach (Sale s in _currentSaleList)
                {
                    if (s.SaleStatus == FilterStatus) filtered.Add(s);
                }
                _currentSaleList = filtered;
            }

            ApplySorting();
        }

        private void ApplySorting()
        {
            if (_currentSaleList == null || _currentSaleList.Count <= 1) return;

            string sortExpr = CurrentSortExpression;
            bool ascending = CurrentSortDirection == "ASC";

            Comparison<Sale> comparison = null;
            switch (sortExpr)
            {
                case "sale_id":
                    comparison = delegate(Sale a, Sale b) { return a.SaleId.CompareTo(b.SaleId); };
                    break;
                case "sale_date":
                    comparison = delegate(Sale a, Sale b) { return a.SaleDate.CompareTo(b.SaleDate); };
                    break;
                case "ticket_name":
                    comparison = delegate(Sale a, Sale b) { return string.Compare(a.TicketName ?? "", b.TicketName ?? "", StringComparison.Ordinal); };
                    break;
                case "ticket_type":
                    comparison = delegate(Sale a, Sale b) { return string.Compare(a.TicketType ?? "", b.TicketType ?? "", StringComparison.Ordinal); };
                    break;
                case "ticket_quantity":
                    comparison = delegate(Sale a, Sale b) { return a.TicketQuantity.CompareTo(b.TicketQuantity); };
                    break;
                case "sale_price":
                    comparison = delegate(Sale a, Sale b) { return a.SalePrice.CompareTo(b.SalePrice); };
                    break;
                case "sale_total":
                    comparison = delegate(Sale a, Sale b) { return a.SaleTotal.CompareTo(b.SaleTotal); };
                    break;
                case "sale_channel":
                    comparison = delegate(Sale a, Sale b) { return string.Compare(a.SaleChannel ?? "", b.SaleChannel ?? "", StringComparison.Ordinal); };
                    break;
                case "sale_status":
                    comparison = delegate(Sale a, Sale b) { return string.Compare(a.SaleStatus ?? "", b.SaleStatus ?? "", StringComparison.Ordinal); };
                    break;
                case "payment_status":
                    comparison = delegate(Sale a, Sale b) { return string.Compare(a.PaymentStatus ?? "", b.PaymentStatus ?? "", StringComparison.Ordinal); };
                    break;
                case "cashier_name":
                    comparison = delegate(Sale a, Sale b) { return string.Compare(a.CashierName ?? "", b.CashierName ?? "", StringComparison.Ordinal); };
                    break;
                default:
                    comparison = delegate(Sale a, Sale b) { return a.SaleDate.CompareTo(b.SaleDate); };
                    break;
            }

            _currentSaleList.Sort(comparison);
            if (!ascending) _currentSaleList.Reverse();
        }

        private void BindGrid()
        {
            int totalCount = _currentSaleList != null ? _currentSaleList.Count : 0;
            int totalPages = (int)Math.Ceiling((double)totalCount / _pageSize);
            if (totalPages == 0) totalPages = 1;
            if (CurrentPage > totalPages) CurrentPage = totalPages;

            int skip = (CurrentPage - 1) * _pageSize;
            int take = Math.Min(_pageSize, totalCount - skip);
            if (take < 0) take = 0;

            List<Sale> pageData = new List<Sale>();
            for (int i = skip; i < skip + take && i < totalCount; i++)
                pageData.Add(_currentSaleList[i]);

            gvSales.DataSource = pageData;
            gvSales.DataBind();
            RenderPagination(totalPages, totalCount);
        }

        private void RenderPagination(int totalPages, int totalCount)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<span class='text-small'>Total: ").Append(totalCount).Append(" sales | </span>");

            if (totalPages <= 1)
            {
                sb.Append("<span class='current'>1</span>");
            }
            else
            {
                if (CurrentPage > 1)
                    sb.Append("<a href=\"#\" onclick=\"__doPostBack('gvSales','Page$").Append(CurrentPage - 1).Append("');return false;\">&#9664; Prev</a>");

                int startPage = Math.Max(1, CurrentPage - 3);
                int endPage = Math.Min(totalPages, CurrentPage + 3);

                for (int i = startPage; i <= endPage; i++)
                {
                    if (i == CurrentPage)
                        sb.Append("<span class='current'>").Append(i).Append("</span>");
                    else
                        sb.Append("<a href=\"#\" onclick=\"__doPostBack('gvSales','Page$").Append(i).Append("');return false;\">").Append(i).Append("</a>");
                }

                if (CurrentPage < totalPages)
                    sb.Append("<a href=\"#\" onclick=\"__doPostBack('gvSales','Page$").Append(CurrentPage + 1).Append("');return false;\">Next &#9654;</a>");
            }

            litPagination.Text = sb.ToString();
        }

        private void LoadFormDropdowns()
        {
            // Load tickets
            ddlTicket.Items.Clear();
            ddlTicket.Items.Add(new ListItem("-- Select Ticket --", ""));
            using (TicketService service = new TicketService())
            {
                List<Ticket> tickets = service.GetAllTickets();
                if (tickets != null)
                {
                    foreach (Ticket t in tickets)
                    {
                        ddlTicket.Items.Add(new ListItem(
                            t.TicketName + " (" + t.Price.ToString("F2") + ")",
                            t.TicketId.ToString()));
                    }
                }
            }

            // Load cashiers
            ddlCashier.Items.Clear();
            ddlCashier.Items.Add(new ListItem("-- None --", ""));
            using (EmployeeService service = new EmployeeService())
            {
                List<Employee> cashiers = service.GetCashiers();
                if (cashiers != null)
                {
                    foreach (Employee c in cashiers)
                    {
                        ddlCashier.Items.Add(new ListItem(
                            c.EmployeeName,
                            c.EmployeeId.ToString()));
                    }
                }
            }
        }

        #endregion

        #region Grid Events

        protected void gvSales_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CurrentPage = e.NewPageIndex + 1;
            LoadSales();
        }

        protected void gvSales_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression == CurrentSortExpression)
                CurrentSortDirection = CurrentSortDirection == "ASC" ? "DESC" : "ASC";
            else
            {
                CurrentSortExpression = e.SortExpression;
                CurrentSortDirection = "ASC";
            }
            CurrentPage = 1;
            LoadSales();
        }

        protected void gvSales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandArgument == null) return;
            long saleId;
            if (!long.TryParse(e.CommandArgument.ToString(), out saleId)) return;

            switch (e.CommandName)
            {
                case "ViewSale":
                    ShowSaleDetail(saleId);
                    break;
            }
        }

        protected void gvSales_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Sale sale = (Sale)e.Row.DataItem;

                // Sale status coloring
                Literal litSaleStatus = (Literal)e.Row.FindControl("litSaleStatus");
                if (litSaleStatus != null)
                {
                    string cssClass;
                    switch (sale.SaleStatus)
                    {
                        case SaleStatus.Created: cssClass = "status-created"; break;
                        case SaleStatus.Completed: cssClass = "status-completed"; break;
                        case SaleStatus.Cancelled: cssClass = "status-cancelled"; break;
                        case SaleStatus.Refunded: cssClass = "status-refunded"; break;
                        default: cssClass = "status-created"; break;
                    }
                    litSaleStatus.Text = "<span class='" + cssClass + "'>" + sale.SaleStatus + "</span>";
                }

                // Payment status coloring
                Literal litPayStatus = (Literal)e.Row.FindControl("litPayStatus");
                if (litPayStatus != null)
                {
                    string cssClass;
                    switch (sale.PaymentStatus)
                    {
                        case PaymentStatus.Pending: cssClass = "pay-pending"; break;
                        case PaymentStatus.Paid: cssClass = "pay-paid"; break;
                        case PaymentStatus.Error: cssClass = "pay-error"; break;
                        case PaymentStatus.Refunded: cssClass = "pay-refunded"; break;
                        default: cssClass = "pay-pending"; break;
                    }
                    litPayStatus.Text = "<span class='" + cssClass + "'>" + sale.PaymentStatus + "</span>";
                }

                // Cashier fallback
                foreach (TableCell cell in e.Row.Cells)
                {
                    if (cell.Text != null && cell.Text == "&nbsp;")
                        cell.Text = "<span class='text-small'>—</span>";
                }
            }
        }

        #endregion

        #region Action Button Events

        protected void btnNewSale_Click(object sender, EventArgs e)
        {
            LoadFormDropdowns();
            cbSaleForm.Visible = true;
            cbSaleDetail.Visible = false;
            pnlError.Visible = false;
            txtQuantity.Text = "1";
            txtSalePrice.Text = string.Empty;
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            FilterChannel = string.Empty;
            FilterStatus = string.Empty;
            FilterDateFrom = null;
            FilterDateTo = null;
            CurrentSortExpression = "sale_date";
            CurrentSortDirection = "DESC";
            txtDateFrom.Text = string.Empty;
            txtDateTo.Text = string.Empty;
            ddlChannelFilter.SelectedIndex = 0;
            ddlStatusFilter.SelectedIndex = 0;
            cbSaleForm.Visible = false;
            cbSaleDetail.Visible = false;
            LoadSales();
            LoadStats();
            ShowSuccess("Sales list refreshed successfully.");
        }

        protected void btnApplyDateRange_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dateFrom, dateTo;

                if (!DateTime.TryParseExact(txtDateFrom.Text.Trim(), "dd.MM.yyyy",
                    null, System.Globalization.DateTimeStyles.None, out dateFrom))
                {
                    ShowError("Invalid 'Date From' format. Use dd.MM.yyyy");
                    return;
                }

                if (!DateTime.TryParseExact(txtDateTo.Text.Trim(), "dd.MM.yyyy",
                    null, System.Globalization.DateTimeStyles.None, out dateTo))
                {
                    ShowError("Invalid 'Date To' format. Use dd.MM.yyyy");
                    return;
                }

                if (dateFrom > dateTo)
                {
                    ShowError("Date From cannot be after Date To.");
                    return;
                }

                FilterDateFrom = dateFrom;
                FilterDateTo = dateTo.AddDays(1).AddSeconds(-1);
                CurrentPage = 1;
                LoadSales();
                ShowSuccess("Sales loaded for " + dateFrom.ToString("dd.MM.yyyy") + " - " + dateTo.ToString("dd.MM.yyyy"));
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        protected void ddlChannelFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterChannel = ddlChannelFilter.SelectedValue;
            CurrentPage = 1;
            LoadSales();
        }

        protected void ddlStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterStatus = ddlStatusFilter.SelectedValue;
            CurrentPage = 1;
            LoadSales();
        }

        protected void btnSaveSale_Click(object sender, EventArgs e)
        {
            try
            {
                Sale sale = new Sale();

                int ticketId;
                if (!int.TryParse(ddlTicket.SelectedValue, out ticketId) || ticketId <= 0)
                {
                    ShowError("Please select a valid ticket.");
                    return;
                }
                sale.TicketId = ticketId;

                int quantity;
                if (!int.TryParse(txtQuantity.Text.Trim(), out quantity) || quantity <= 0)
                {
                    ShowError("Quantity must be a positive integer.");
                    return;
                }
                sale.TicketQuantity = quantity;

                decimal price;
                if (!decimal.TryParse(txtSalePrice.Text.Trim(), out price) || price < 0)
                {
                    ShowError("Invalid sale price.");
                    return;
                }
                sale.SalePrice = price;

                sale.SaleChannel = ddlChannel.SelectedValue;
                sale.SaleStatus = SaleStatus.Created;
                sale.PaymentStatus = PaymentStatus.Pending;

                if (!string.IsNullOrEmpty(ddlCashier.SelectedValue))
                {
                    int cashierId;
                    if (int.TryParse(ddlCashier.SelectedValue, out cashierId))
                        sale.CashierId = cashierId;
                }

                using (TicketService service = new TicketService())
                {
                    long saleId = service.CreateSale(sale);
                    ShowSuccess("Sale created successfully. Sale ID: " + saleId);
                }

                cbSaleForm.Visible = false;
                LoadSales();
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

        protected void btnCancelSale_Click(object sender, EventArgs e)
        {
            cbSaleForm.Visible = false;
            pnlError.Visible = false;
        }

        protected void btnDetailClose_Click(object sender, EventArgs e)
        {
            cbSaleDetail.Visible = false;
        }

        #endregion

        #region Detail View

        private void ShowSaleDetail(long saleId)
        {
            try
            {
                Sale sale = null;
                using (TicketService service = new TicketService())
                {
                    List<Sale> allSales = service.GetAllSales();
                    if (allSales != null)
                    {
                        foreach (Sale s in allSales)
                        {
                            if (s.SaleId == saleId) { sale = s; break; }
                        }
                    }
                }

                if (sale == null)
                {
                    ShowError("Sale not found.");
                    return;
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("<div class='detail-section'>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Sale ID:</span><span class='detail-value'>").Append(sale.SaleId).Append("</span></div>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Date:</span><span class='detail-value'>").Append(sale.SaleDate.ToString("dd.MM.yyyy HH:mm")).Append("</span></div>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Ticket:</span><span class='detail-value'>").Append(Server.HtmlEncode(sale.TicketName ?? "—")).Append("</span></div>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Type:</span><span class='detail-value'>").Append(Server.HtmlEncode(sale.TicketType ?? "—")).Append("</span></div>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Quantity:</span><span class='detail-value'>").Append(sale.TicketQuantity).Append("</span></div>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Unit Price:</span><span class='detail-value'>").Append(sale.SalePrice.ToString("F2")).Append("</span></div>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Total:</span><span class='detail-value'>").Append(sale.SaleTotal.ToString("F2")).Append("</span></div>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Channel:</span><span class='detail-value'>").Append(Server.HtmlEncode(sale.SaleChannel ?? "—")).Append("</span></div>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Sale Status:</span><span class='detail-value'>").Append(Server.HtmlEncode(sale.SaleStatus ?? "—")).Append("</span></div>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Payment Status:</span><span class='detail-value'>").Append(Server.HtmlEncode(sale.PaymentStatus ?? "—")).Append("</span></div>");
                sb.Append("<div class='detail-row'><span class='detail-label'>Cashier:</span><span class='detail-value'>").Append(Server.HtmlEncode(sale.CashierName ?? "—")).Append("</span></div>");
                sb.Append("</div>");

                litSaleDetail.Text = sb.ToString();
                cbSaleDetail.Visible = true;
                cbSaleForm.Visible = false;
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
            System.Diagnostics.Debug.WriteLine("Sales SQL Error: " + sqlEx.ToString());
        }

        private void HandleServiceError(ServiceException svcEx)
        {
            string msg = svcEx.Message;
            if (svcEx.InnerException != null) msg += " (" + svcEx.InnerException.Message + ")";
            ShowError(msg);
            System.Diagnostics.Debug.WriteLine("Sales Service Error: " + svcEx.ToString());
        }

        private void HandleGenericError(Exception ex)
        {
            ShowError("Unexpected error: " + ex.Message);
            System.Diagnostics.Debug.WriteLine("Sales Generic Error: " + ex.ToString());
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
