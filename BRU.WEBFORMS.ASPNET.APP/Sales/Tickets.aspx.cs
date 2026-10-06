using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BRU.WEBFORMS.ASPNET.APP.Services;
using BRU.WEBFORMS.ASPNET.APP.Models;
using System.Data.SqlClient;

namespace BRU.WEBFORMS.ASPNET.APP.Sales
{
    /// <summary>
    /// Ticket Type Management page for Autopark Management System.
    /// Provides CRUD operations for ticket types, price range filtering,
    /// type filtering, search, sorting, pagination, stock-level tracking,
    /// and comprehensive statistics.
    /// </summary>
    public partial class Tickets : SecurePage
    {
        protected override string[] RequiredPermissions { get { return new[] { "ticket.read" }; } }
        #region Control Declarations

        protected global::System.Web.UI.WebControls.Panel pnlError;
        protected global::System.Web.UI.WebControls.Literal litError;
        protected global::System.Web.UI.WebControls.Panel pnlSuccess;
        protected global::System.Web.UI.WebControls.Literal litSuccess;
        protected global::System.Web.UI.WebControls.Literal litTotalTickets;
        protected global::System.Web.UI.WebControls.Literal litActiveTickets;
        protected global::System.Web.UI.WebControls.Literal litLowStock;
        protected global::System.Web.UI.WebControls.Literal litOutOfStock;
        protected global::System.Web.UI.WebControls.Literal litAvgPrice;
        protected global::System.Web.UI.WebControls.Button btnAddTicket;
        protected global::System.Web.UI.WebControls.Button btnRefresh;
        protected global::System.Web.UI.WebControls.TextBox txtSearch;
        protected global::System.Web.UI.WebControls.Button btnSearch;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbFilters;
        protected global::System.Web.UI.WebControls.DropDownList ddlTypeFilter;
        protected global::System.Web.UI.WebControls.TextBox txtPriceFrom;
        protected global::System.Web.UI.WebControls.TextBox txtPriceTo;
        protected global::System.Web.UI.WebControls.Button btnApplyPriceFilter;
        protected global::System.Web.UI.WebControls.DropDownList ddlStatusFilter;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbTicketsList;
        protected global::System.Web.UI.WebControls.GridView gvTickets;
        protected global::System.Web.UI.WebControls.Literal litPagination;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbTicketForm;
        protected global::System.Web.UI.WebControls.Panel pnlTicketForm;
        protected global::System.Web.UI.WebControls.TextBox txtTicketName;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvTicketName;
        protected global::System.Web.UI.WebControls.TextBox txtTicketType;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvTicketType;
        protected global::System.Web.UI.WebControls.TextBox txtZone;
        protected global::System.Web.UI.WebControls.TextBox txtPrice;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvPrice;
        protected global::System.Web.UI.WebControls.TextBox txtValidDays;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvValidDays;
        protected global::System.Web.UI.WebControls.TextBox txtAvailableCount;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvAvailableCount;
        protected global::System.Web.UI.WebControls.TextBox txtExpiryDate;
        protected global::System.Web.UI.WebControls.CheckBox chkActive;
        protected global::System.Web.UI.WebControls.Button btnSave;
        protected global::System.Web.UI.WebControls.Button btnCancel;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbTicketDetail;
        protected global::System.Web.UI.WebControls.Literal litDetail;
        protected global::System.Web.UI.WebControls.Button btnDetailClose;

        #endregion

        #region Private Fields

        private List<Ticket> _currentTicketList;
        private const int _pageSize = 20;
        private const int _lowStockThreshold = 50;
        private bool _templateControlsResolved;

        #endregion

        #region State Properties

        private int CurrentPage
        {
            get
            {
                object val = ViewState["Tickets_CurrentPage"];
                if (val == null) return 1;
                int page;
                if (int.TryParse(val.ToString(), out page) && page > 0) return page;
                return 1;
            }
            set { ViewState["Tickets_CurrentPage"] = value < 1 ? 1 : value; }
        }

        private string CurrentSortExpression
        {
            get
            {
                object val = ViewState["Tickets_SortExpr"];
                return val == null ? "ticket_name" : val.ToString();
            }
            set { ViewState["Tickets_SortExpr"] = value; }
        }

        private string CurrentSortDirection
        {
            get
            {
                object val = ViewState["Tickets_SortDir"];
                return val == null ? "ASC" : val.ToString();
            }
            set { ViewState["Tickets_SortDir"] = value; }
        }

        private string SearchKeyword
        {
            get
            {
                object val = ViewState["Tickets_SearchKeyword"];
                return val == null ? string.Empty : val.ToString();
            }
            set { ViewState["Tickets_SearchKeyword"] = value; }
        }

        private string FilterType
        {
            get
            {
                object val = ViewState["Tickets_FilterType"];
                return val == null ? string.Empty : val.ToString();
            }
            set { ViewState["Tickets_FilterType"] = value; }
        }

        private decimal? FilterPriceFrom
        {
            get
            {
                object val = ViewState["Tickets_PriceFrom"];
                if (val == null) return null;
                decimal d;
                if (decimal.TryParse(val.ToString(), out d)) return d;
                return null;
            }
            set { ViewState["Tickets_PriceFrom"] = value; }
        }

        private decimal? FilterPriceTo
        {
            get
            {
                object val = ViewState["Tickets_PriceTo"];
                if (val == null) return null;
                decimal d;
                if (decimal.TryParse(val.ToString(), out d)) return d;
                return null;
            }
            set { ViewState["Tickets_PriceTo"] = value; }
        }

        private string FilterStatus
        {
            get
            {
                object val = ViewState["Tickets_FilterStatus"];
                return val == null ? string.Empty : val.ToString();
            }
            set { ViewState["Tickets_FilterStatus"] = value; }
        }

        private int EditingTicketId
        {
            get
            {
                object val = ViewState["Tickets_EditingId"];
                if (val == null) return 0;
                int id;
                if (int.TryParse(val.ToString(), out id)) return id;
                return 0;
            }
            set { ViewState["Tickets_EditingId"] = value; }
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
                    LoadTypeFilter();
                    LoadTickets();
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
            EnsureContentBoxCreated(cbTicketsList, "cbTicketsList");
            EnsureContentBoxCreated(cbTicketForm, "cbTicketForm");
            EnsureContentBoxCreated(cbTicketDetail, "cbTicketDetail");

            // Resolve filter controls
            ddlTypeFilter = FindRequiredTemplateControl<DropDownList>(cbFilters, "ddlTypeFilter");
            txtPriceFrom = FindRequiredTemplateControl<TextBox>(cbFilters, "txtPriceFrom");
            txtPriceTo = FindRequiredTemplateControl<TextBox>(cbFilters, "txtPriceTo");
            btnApplyPriceFilter = FindRequiredTemplateControl<Button>(cbFilters, "btnApplyPriceFilter");
            ddlStatusFilter = FindRequiredTemplateControl<DropDownList>(cbFilters, "ddlStatusFilter");

            // Resolve list controls
            gvTickets = FindRequiredTemplateControl<GridView>(cbTicketsList, "gvTickets");

            // Resolve form controls
            pnlTicketForm = FindRequiredTemplateControl<Panel>(cbTicketForm, "pnlTicketForm");
            txtTicketName = FindRequiredTemplateControl<TextBox>(cbTicketForm, "txtTicketName");
            rfvTicketName = FindRequiredTemplateControl<RequiredFieldValidator>(cbTicketForm, "rfvTicketName");
            txtTicketType = FindRequiredTemplateControl<TextBox>(cbTicketForm, "txtTicketType");
            rfvTicketType = FindRequiredTemplateControl<RequiredFieldValidator>(cbTicketForm, "rfvTicketType");
            txtZone = FindRequiredTemplateControl<TextBox>(cbTicketForm, "txtZone");
            txtPrice = FindRequiredTemplateControl<TextBox>(cbTicketForm, "txtPrice");
            rfvPrice = FindRequiredTemplateControl<RequiredFieldValidator>(cbTicketForm, "rfvPrice");
            txtValidDays = FindRequiredTemplateControl<TextBox>(cbTicketForm, "txtValidDays");
            rfvValidDays = FindRequiredTemplateControl<RequiredFieldValidator>(cbTicketForm, "rfvValidDays");
            txtAvailableCount = FindRequiredTemplateControl<TextBox>(cbTicketForm, "txtAvailableCount");
            rfvAvailableCount = FindRequiredTemplateControl<RequiredFieldValidator>(cbTicketForm, "rfvAvailableCount");
            txtExpiryDate = FindRequiredTemplateControl<TextBox>(cbTicketForm, "txtExpiryDate");
            chkActive = FindRequiredTemplateControl<CheckBox>(cbTicketForm, "chkActive");
            btnSave = FindRequiredTemplateControl<Button>(cbTicketForm, "btnSave");
            btnCancel = FindRequiredTemplateControl<Button>(cbTicketForm, "btnCancel");

            // Resolve detail controls
            litDetail = FindRequiredTemplateControl<Literal>(cbTicketDetail, "litDetail");
            btnDetailClose = FindRequiredTemplateControl<Button>(cbTicketDetail, "btnDetailClose");

            _templateControlsResolved = true;
        }

        private static void EnsureContentBoxCreated(Controls.ContentBox contentBox, string controlId)
        {
            if (contentBox == null)
                throw new InvalidOperationException(
                    "Required ContentBox '" + controlId + "' was not created. Check Tickets.aspx markup and the ContentBox registration.");
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

        private void LoadTypeFilter()
        {
            ddlTypeFilter.Items.Clear();
            ddlTypeFilter.Items.Add(new ListItem("All Types", ""));

            using (TicketService service = new TicketService())
            {
                List<Ticket> tickets = service.GetAllTickets();
                if (tickets != null)
                {
                    Dictionary<string, string> types = new Dictionary<string, string>();
                    foreach (Ticket t in tickets)
                    {
                        if (!string.IsNullOrEmpty(t.TicketType) && !types.ContainsKey(t.TicketType))
                            types[t.TicketType] = t.TicketType;
                    }
                    foreach (string type in types.Keys)
                        ddlTypeFilter.Items.Add(new ListItem(type, type));
                }
            }
        }

        private void LoadTickets()
        {
            using (TicketService service = new TicketService())
            {
                if (FilterPriceFrom.HasValue && FilterPriceTo.HasValue)
                    _currentTicketList = service.GetTicketsByPriceRange(FilterPriceFrom.Value, FilterPriceTo.Value);
                else
                    _currentTicketList = service.GetAllTickets();
            }

            ApplyFiltersAndSort();
            BindGrid();
        }

        private void LoadStats()
        {
            List<Ticket> tickets;
            using (TicketService service = new TicketService())
            {
                tickets = service.GetAllTickets();
            }

            if (tickets == null) tickets = new List<Ticket>();

            litTotalTickets.Text = tickets.Count.ToString();

            int activeCount = 0, lowStockCount = 0, outOfStockCount = 0;
            decimal totalPrice = 0;

            foreach (Ticket t in tickets)
            {
                if (t.IsActive) activeCount++;
                if (t.AvailableCount == 0) outOfStockCount++;
                else if (t.AvailableCount <= _lowStockThreshold) lowStockCount++;
                totalPrice += t.Price;
            }

            litActiveTickets.Text = activeCount.ToString();
            litLowStock.Text = lowStockCount.ToString();
            litOutOfStock.Text = outOfStockCount.ToString();
            litAvgPrice.Text = tickets.Count > 0
                ? (totalPrice / tickets.Count).ToString("F2")
                : "0.00";
        }

        private void ApplyFiltersAndSort()
        {
            if (_currentTicketList == null) _currentTicketList = new List<Ticket>();

            // Apply search keyword filter
            if (!string.IsNullOrEmpty(SearchKeyword))
            {
                string keyword = SearchKeyword.ToLowerInvariant();
                List<Ticket> filtered = new List<Ticket>();
                foreach (Ticket t in _currentTicketList)
                {
                    if ((t.TicketName != null && t.TicketName.ToLowerInvariant().Contains(keyword)) ||
                        (t.TicketType != null && t.TicketType.ToLowerInvariant().Contains(keyword)))
                        filtered.Add(t);
                }
                _currentTicketList = filtered;
            }

            // Apply type filter
            if (!string.IsNullOrEmpty(FilterType))
            {
                List<Ticket> filtered = new List<Ticket>();
                foreach (Ticket t in _currentTicketList)
                {
                    if (t.TicketType == FilterType) filtered.Add(t);
                }
                _currentTicketList = filtered;
            }

            // Apply status filter
            if (!string.IsNullOrEmpty(FilterStatus))
            {
                bool wantActive = FilterStatus == "1";
                List<Ticket> filtered = new List<Ticket>();
                foreach (Ticket t in _currentTicketList)
                {
                    if (t.IsActive == wantActive) filtered.Add(t);
                }
                _currentTicketList = filtered;
            }

            ApplySorting();
        }

        private void ApplySorting()
        {
            if (_currentTicketList == null || _currentTicketList.Count <= 1) return;

            string sortExpr = CurrentSortExpression;
            bool ascending = CurrentSortDirection == "ASC";

            Comparison<Ticket> comparison = null;
            switch (sortExpr)
            {
                case "ticket_id":
                    comparison = delegate(Ticket a, Ticket b) { return a.TicketId.CompareTo(b.TicketId); };
                    break;
                case "ticket_name":
                    comparison = delegate(Ticket a, Ticket b) { return string.Compare(a.TicketName ?? "", b.TicketName ?? "", StringComparison.Ordinal); };
                    break;
                case "ticket_type":
                    comparison = delegate(Ticket a, Ticket b) { return string.Compare(a.TicketType ?? "", b.TicketType ?? "", StringComparison.Ordinal); };
                    break;
                case "price":
                    comparison = delegate(Ticket a, Ticket b) { return a.Price.CompareTo(b.Price); };
                    break;
                case "valid_days":
                    comparison = delegate(Ticket a, Ticket b) { return a.ValidDays.CompareTo(b.ValidDays); };
                    break;
                case "available_count":
                    comparison = delegate(Ticket a, Ticket b) { return a.AvailableCount.CompareTo(b.AvailableCount); };
                    break;
                case "issue_date":
                    comparison = delegate(Ticket a, Ticket b) { return a.IssueDate.CompareTo(b.IssueDate); };
                    break;
                case "expiry_date":
                    comparison = delegate(Ticket a, Ticket b)
                    {
                        DateTime da = a.ExpiryDate ?? DateTime.MaxValue;
                        DateTime db = b.ExpiryDate ?? DateTime.MaxValue;
                        return da.CompareTo(db);
                    };
                    break;
                case "is_active":
                    comparison = delegate(Ticket a, Ticket b) { return a.IsActive.CompareTo(b.IsActive); };
                    break;
                default:
                    comparison = delegate(Ticket a, Ticket b) { return string.Compare(a.TicketName ?? "", b.TicketName ?? "", StringComparison.Ordinal); };
                    break;
            }

            _currentTicketList.Sort(comparison);
            if (!ascending) _currentTicketList.Reverse();
        }

        private void BindGrid()
        {
            int totalCount = _currentTicketList != null ? _currentTicketList.Count : 0;
            int totalPages = (int)Math.Ceiling((double)totalCount / _pageSize);
            if (totalPages == 0) totalPages = 1;
            if (CurrentPage > totalPages) CurrentPage = totalPages;

            gvTickets.PageSize = _pageSize;
            gvTickets.PageIndex = CurrentPage - 1;
            gvTickets.DataSource = _currentTicketList;
            gvTickets.DataBind();
            RenderPagination(totalPages, totalCount);
        }

        private void RenderPagination(int totalPages, int totalCount)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<span class='text-small'>Total: ").Append(totalCount).Append(" tickets");
            if (totalPages > 1)
                sb.Append(" | Page ").Append(CurrentPage).Append(" of ").Append(totalPages);
            sb.Append("</span>");
            litPagination.Text = sb.ToString();
        }

        #endregion

        #region Grid Events

        protected void gvTickets_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CurrentPage = e.NewPageIndex + 1;
            LoadTickets();
        }

        protected void gvTickets_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression == CurrentSortExpression)
                CurrentSortDirection = CurrentSortDirection == "ASC" ? "DESC" : "ASC";
            else
            {
                CurrentSortExpression = e.SortExpression;
                CurrentSortDirection = "ASC";
            }
            CurrentPage = 1;
            LoadTickets();
        }

        protected void gvTickets_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandArgument == null) return;
            int ticketId;
            if (!int.TryParse(e.CommandArgument.ToString(), out ticketId)) return;

            switch (e.CommandName)
            {
                case "ViewTicket":
                    ShowTicketDetail(ticketId);
                    break;
                case "EditTicket":
                    ShowEditForm(ticketId);
                    break;
                case "DeleteTicket":
                    DeleteTicket(ticketId);
                    break;
            }
        }

        protected void gvTickets_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button deleteButton = e.Row.FindControl("btnDelete") as Button;
                if (deleteButton != null)
                {
                    deleteButton.OnClientClick = "return confirm(" +
                        HttpUtility.JavaScriptStringEncode(Localization.Get("Common_DeleteTicketConfirm"), true) + ");";
                }

                Ticket ticket = (Ticket)e.Row.DataItem;

                // Price coloring
                Literal litPrice = (Literal)e.Row.FindControl("litPrice");
                if (litPrice != null)
                {
                    string cssClass;
                    if (ticket.Price >= 100) cssClass = "price-high";
                    else if (ticket.Price >= 30) cssClass = "price-medium";
                    else cssClass = "price-low";
                    litPrice.Text = "<span class='" + cssClass + "'>" + ticket.Price.ToString("F2") + "</span>";
                }

                // Stock coloring
                Literal litStock = (Literal)e.Row.FindControl("litStock");
                if (litStock != null)
                {
                    string cssClass;
                    if (ticket.AvailableCount == 0) cssClass = "stock-out";
                    else if (ticket.AvailableCount <= _lowStockThreshold) cssClass = "stock-low";
                    else if (ticket.AvailableCount <= 200) cssClass = "stock-medium";
                    else cssClass = "stock-high";
                    litStock.Text = "<span class='" + cssClass + "'>" + ticket.AvailableCount.ToString() + "</span>";
                }

                // Status coloring
                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                if (litStatus != null)
                {
                    string cssClass = ticket.IsActive ? "status-active" : "status-inactive";
                    string statusText = ticket.IsActive ? "Active" : "Inactive";
                    litStatus.Text = "<span class='" + cssClass + "'>" + statusText + "</span>";
                }

                // Zone fallback
                foreach (TableCell cell in e.Row.Cells)
                {
                    if (cell.Text != null && cell.Text == "&nbsp;")
                        cell.Text = "<span class='text-small'>—</span>";
                }
            }
        }

        #endregion

        #region Action Button Events

        protected void btnAddTicket_Click(object sender, EventArgs e)
        {
            EditingTicketId = 0;
            ClearForm();
            cbTicketForm.HeaderText = "Add New Ticket Type";
            cbTicketForm.Visible = true;
            cbTicketDetail.Visible = false;
            pnlError.Visible = false;
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            SearchKeyword = string.Empty;
            FilterType = string.Empty;
            FilterStatus = string.Empty;
            FilterPriceFrom = null;
            FilterPriceTo = null;
            CurrentSortExpression = "ticket_name";
            CurrentSortDirection = "ASC";
            txtSearch.Text = string.Empty;
            txtPriceFrom.Text = string.Empty;
            txtPriceTo.Text = string.Empty;
            ddlTypeFilter.SelectedIndex = 0;
            ddlStatusFilter.SelectedIndex = 0;
            cbTicketForm.Visible = false;
            cbTicketDetail.Visible = false;
            LoadTickets();
            LoadStats();
            ShowSuccess("Ticket list refreshed successfully.");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchKeyword = txtSearch.Text.Trim();
            CurrentPage = 1;
            LoadTickets();
        }

        protected void ddlTypeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterType = ddlTypeFilter.SelectedValue;
            CurrentPage = 1;
            LoadTickets();
        }

        protected void ddlStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterStatus = ddlStatusFilter.SelectedValue;
            CurrentPage = 1;
            LoadTickets();
        }

        protected void btnApplyPriceFilter_Click(object sender, EventArgs e)
        {
            try
            {
                decimal from, to;
                if (!string.IsNullOrWhiteSpace(txtPriceFrom.Text) &&
                    decimal.TryParse(txtPriceFrom.Text.Trim(), out from))
                    FilterPriceFrom = from;
                else
                    FilterPriceFrom = null;

                if (!string.IsNullOrWhiteSpace(txtPriceTo.Text) &&
                    decimal.TryParse(txtPriceTo.Text.Trim(), out to))
                    FilterPriceTo = to;
                else
                    FilterPriceTo = null;

                if (FilterPriceFrom.HasValue && FilterPriceTo.HasValue &&
                    FilterPriceFrom > FilterPriceTo)
                {
                    ShowError("Price From cannot be greater than Price To.");
                    return;
                }

                CurrentPage = 1;
                LoadTickets();
                ShowSuccess("Price filter applied.");
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Ticket ticket = new Ticket();
                ticket.TicketId = EditingTicketId;
                ticket.TicketName = txtTicketName.Text.Trim();
                ticket.TicketType = txtTicketType.Text.Trim();
                ticket.Zone = string.IsNullOrWhiteSpace(txtZone.Text) ? null : txtZone.Text.Trim();

                decimal price;
                if (!decimal.TryParse(txtPrice.Text.Trim(), out price))
                {
                    ShowError("Invalid price format.");
                    return;
                }
                ticket.Price = price;

                short validDays;
                if (!short.TryParse(txtValidDays.Text.Trim(), out validDays))
                {
                    ShowError("Invalid valid days format.");
                    return;
                }
                ticket.ValidDays = validDays;

                int availCount;
                if (!int.TryParse(txtAvailableCount.Text.Trim(), out availCount))
                {
                    ShowError("Invalid available count format.");
                    return;
                }
                ticket.AvailableCount = availCount;

                if (!string.IsNullOrWhiteSpace(txtExpiryDate.Text))
                {
                    DateTime expiry;
                    if (!DateTime.TryParseExact(txtExpiryDate.Text.Trim(), "dd.MM.yyyy",
                        null, System.Globalization.DateTimeStyles.None, out expiry))
                    {
                        ShowError("Invalid expiry date format. Use dd.MM.yyyy");
                        return;
                    }
                    ticket.ExpiryDate = expiry;
                }
                else
                {
                    ticket.ExpiryDate = null;
                }

                ticket.IsActive = chkActive.Checked;

                using (TicketService service = new TicketService())
                {
                    if (EditingTicketId > 0)
                    {
                        service.UpdateTicket(ticket);
                        ShowSuccess("Ticket type updated successfully.");
                    }
                    else
                    {
                        service.CreateTicket(ticket);
                        ShowSuccess("Ticket type created successfully.");
                    }
                }

                cbTicketForm.Visible = false;
                LoadTickets();
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            cbTicketForm.Visible = false;
            ClearForm();
            pnlError.Visible = false;
        }

        protected void btnDetailClose_Click(object sender, EventArgs e)
        {
            cbTicketDetail.Visible = false;
        }

        #endregion

        #region Form Helpers

        private void ClearForm()
        {
            txtTicketName.Text = string.Empty;
            txtTicketType.Text = string.Empty;
            txtZone.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtValidDays.Text = string.Empty;
            txtAvailableCount.Text = string.Empty;
            txtExpiryDate.Text = string.Empty;
            chkActive.Checked = true;
        }

        private void ShowEditForm(int ticketId)
        {
            try
            {
                using (TicketService service = new TicketService())
                {
                    Ticket ticket = service.GetTicketById(ticketId);
                    txtTicketName.Text = ticket.TicketName;
                    txtTicketType.Text = ticket.TicketType;
                    txtZone.Text = ticket.Zone ?? string.Empty;
                    txtPrice.Text = ticket.Price.ToString("F2");
                    txtValidDays.Text = ticket.ValidDays.ToString();
                    txtAvailableCount.Text = ticket.AvailableCount.ToString();
                    txtExpiryDate.Text = ticket.ExpiryDate.HasValue
                        ? ticket.ExpiryDate.Value.ToString("dd.MM.yyyy")
                        : string.Empty;
                    chkActive.Checked = ticket.IsActive;
                }

                EditingTicketId = ticketId;
                cbTicketForm.HeaderText = "Edit Ticket Type";
                cbTicketForm.Visible = true;
                cbTicketDetail.Visible = false;
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

        private void ShowTicketDetail(int ticketId)
        {
            try
            {
                using (TicketService service = new TicketService())
                {
                    Ticket ticket = service.GetTicketById(ticketId);

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<div class='detail-section'>");
                    sb.Append("<div class='detail-row'><span class='detail-label'>ID:</span><span class='detail-value'>").Append(ticket.TicketId).Append("</span></div>");
                    sb.Append("<div class='detail-row'><span class='detail-label'>Name:</span><span class='detail-value'>").Append(Server.HtmlEncode(ticket.TicketName)).Append("</span></div>");
                    sb.Append("<div class='detail-row'><span class='detail-label'>Type:</span><span class='detail-value'>").Append(Server.HtmlEncode(ticket.TicketType)).Append("</span></div>");
                    sb.Append("<div class='detail-row'><span class='detail-label'>Zone:</span><span class='detail-value'>").Append(Server.HtmlEncode(ticket.Zone ?? "—")).Append("</span></div>");
                    sb.Append("<div class='detail-row'><span class='detail-label'>Price:</span><span class='detail-value'>").Append(ticket.Price.ToString("F2")).Append("</span></div>");
                    sb.Append("<div class='detail-row'><span class='detail-label'>Valid Days:</span><span class='detail-value'>").Append(ticket.ValidDays).Append("</span></div>");
                    sb.Append("<div class='detail-row'><span class='detail-label'>Available Count:</span><span class='detail-value'>").Append(ticket.AvailableCount).Append("</span></div>");
                    sb.Append("<div class='detail-row'><span class='detail-label'>Issue Date:</span><span class='detail-value'>").Append(ticket.IssueDate.ToString("dd.MM.yyyy")).Append("</span></div>");
                    sb.Append("<div class='detail-row'><span class='detail-label'>Expiry Date:</span><span class='detail-value'>").Append(ticket.ExpiryDate.HasValue ? ticket.ExpiryDate.Value.ToString("dd.MM.yyyy") : "—").Append("</span></div>");
                    sb.Append("<div class='detail-row'><span class='detail-label'>Status:</span><span class='detail-value'>").Append(ticket.IsActive ? "Active" : "Inactive").Append("</span></div>");
                    sb.Append("</div>");

                    litDetail.Text = sb.ToString();
                }

                cbTicketDetail.Visible = true;
                cbTicketForm.Visible = false;
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

        private void DeleteTicket(int ticketId)
        {
            try
            {
                using (TicketService service = new TicketService())
                {
                    service.DeleteTicket(ticketId);
                }
                ShowSuccess("Ticket type deleted successfully.");
                LoadTickets();
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
            System.Diagnostics.Debug.WriteLine("Tickets SQL Error: " + sqlEx.ToString());
        }

        private void HandleServiceError(ServiceException svcEx)
        {
            string msg = svcEx.Message;
            if (svcEx.InnerException != null) msg += " (" + svcEx.InnerException.Message + ")";
            ShowError(msg);
            System.Diagnostics.Debug.WriteLine("Tickets Service Error: " + svcEx.ToString());
        }

        private void HandleGenericError(Exception ex)
        {
            ShowError("Unexpected error: " + ex.Message);
            System.Diagnostics.Debug.WriteLine("Tickets Generic Error: " + ex.ToString());
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
