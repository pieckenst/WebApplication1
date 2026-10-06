using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BRU.WEBFORMS.ASPNET.APP.Services;
using BRU.WEBFORMS.ASPNET.APP.Models;
using System.Data.SqlClient;

namespace BRU.WEBFORMS.ASPNET.APP.Operations
{
    /// <summary>
    /// Route Management page for Autopark Management System.
    /// Provides CRUD operations for routes, route stops viewing,
    /// search, sorting, pagination, and comprehensive error handling.
    /// </summary>
    public partial class Routes : SecurePage
    {
        protected override string[] RequiredPermissions { get { return new[] { "route.read" }; } }
        #region Control Declarations

        protected global::System.Web.UI.WebControls.Panel pnlError;
        protected global::System.Web.UI.WebControls.Literal litError;
        protected global::System.Web.UI.WebControls.Panel pnlSuccess;
        protected global::System.Web.UI.WebControls.Literal litSuccess;
        protected global::System.Web.UI.WebControls.Literal litTotalRoutes;
        protected global::System.Web.UI.WebControls.Literal litActiveRoutes;
        protected global::System.Web.UI.WebControls.Literal litTotalStops;
        protected global::System.Web.UI.WebControls.Button btnAddRoute;
        protected global::System.Web.UI.WebControls.Button btnRefresh;
        protected global::System.Web.UI.WebControls.TextBox txtSearch;
        protected global::System.Web.UI.WebControls.Button btnSearch;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbRoutesList;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbRouteForm;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbRouteStops;
        protected global::System.Web.UI.WebControls.GridView gvRoutes;
        protected global::System.Web.UI.WebControls.Literal litPagination;

        protected global::System.Web.UI.WebControls.TextBox txtRouteNum;
        protected global::System.Web.UI.WebControls.TextBox txtRouteName;
        protected global::System.Web.UI.WebControls.TextBox txtStartStop;
        protected global::System.Web.UI.WebControls.TextBox txtEndStop;
        protected global::System.Web.UI.WebControls.CheckBox chkActive;
        protected global::System.Web.UI.WebControls.Button btnSaveRoute;
        protected global::System.Web.UI.WebControls.Button btnCancelRoute;

        protected global::System.Web.UI.WebControls.Literal litRouteStops;
        protected global::System.Web.UI.WebControls.Button btnCloseStops;

        #endregion

        #region Private Fields

        private List<Route> _currentRouteList;
        private const int _pageSize = 15;
        private bool _templateControlsResolved;

        #endregion

        #region State Properties

        private int CurrentPage
        {
            get
            {
                object val = ViewState["Routes_CurrentPage"];
                if (val == null) return 1;
                int page;
                if (int.TryParse(val.ToString(), out page) && page > 0) return page;
                return 1;
            }
            set { ViewState["Routes_CurrentPage"] = value < 1 ? 1 : value; }
        }

        private string CurrentSortExpression
        {
            get
            {
                object val = ViewState["Routes_SortExpr"];
                return val == null ? "route_num" : val.ToString();
            }
            set { ViewState["Routes_SortExpr"] = value; }
        }

        private string CurrentSortDirection
        {
            get
            {
                object val = ViewState["Routes_SortDir"];
                return val == null ? "ASC" : val.ToString();
            }
            set { ViewState["Routes_SortDir"] = value; }
        }

        private int EditingRouteId
        {
            get
            {
                object val = ViewState["Routes_EditingId"];
                if (val == null) return 0;
                int id;
                if (int.TryParse(val.ToString(), out id)) return id;
                return 0;
            }
            set { ViewState["Routes_EditingId"] = value; }
        }

        private string SearchTerm
        {
            get
            {
                object val = ViewState["Routes_SearchTerm"];
                return val == null ? string.Empty : val.ToString();
            }
            set { ViewState["Routes_SearchTerm"] = value; }
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
                    LoadRouteData();
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

            EnsureContentBoxCreated(cbRoutesList, "cbRoutesList");
            EnsureContentBoxCreated(cbRouteForm, "cbRouteForm");
            EnsureContentBoxCreated(cbRouteStops, "cbRouteStops");

            // Resolve list controls
            gvRoutes = FindRequiredTemplateControl<GridView>(cbRoutesList, "gvRoutes");

            // Resolve form controls
            txtRouteNum = FindRequiredTemplateControl<TextBox>(cbRouteForm, "txtRouteNum");
            txtRouteName = FindRequiredTemplateControl<TextBox>(cbRouteForm, "txtRouteName");
            txtStartStop = FindRequiredTemplateControl<TextBox>(cbRouteForm, "txtStartStop");
            txtEndStop = FindRequiredTemplateControl<TextBox>(cbRouteForm, "txtEndStop");
            chkActive = FindRequiredTemplateControl<CheckBox>(cbRouteForm, "chkActive");
            btnSaveRoute = FindRequiredTemplateControl<Button>(cbRouteForm, "btnSaveRoute");
            btnCancelRoute = FindRequiredTemplateControl<Button>(cbRouteForm, "btnCancelRoute");

            // Resolve stops controls
            litRouteStops = FindRequiredTemplateControl<Literal>(cbRouteStops, "litRouteStops");
            btnCloseStops = FindRequiredTemplateControl<Button>(cbRouteStops, "btnCloseStops");

            _templateControlsResolved = true;
        }

        private static void EnsureContentBoxCreated(Controls.ContentBox contentBox, string controlId)
        {
            if (contentBox == null)
                throw new InvalidOperationException(
                    "Required ContentBox '" + controlId + "' was not created. Check Routes.aspx markup and the ContentBox registration.");
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

        private void LoadRouteData()
        {
            using (RouteService service = new RouteService())
            {
                _currentRouteList = service.GetAllActiveRoutes();
            }

            ApplyFiltersAndSort();
            BindGrid();
        }

        private void LoadStats()
        {
            using (RouteService service = new RouteService())
            {
                _currentRouteList = service.GetAllActiveRoutes();
                List<Stop> stops = service.GetAllStops();

                litTotalRoutes.Text = _currentRouteList.Count.ToString();
                litActiveRoutes.Text = _currentRouteList.Count.ToString();
                litTotalStops.Text = stops.Count.ToString();
            }
        }

        private void ApplyFiltersAndSort()
        {
            if (_currentRouteList == null) _currentRouteList = new List<Route>();

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                string term = SearchTerm.ToLowerInvariant();
                List<Route> filtered = new List<Route>();
                foreach (Route r in _currentRouteList)
                {
                    string num = (r.RouteNum ?? string.Empty).ToLowerInvariant();
                    string name = (r.RouteName ?? string.Empty).ToLowerInvariant();
                    string start = (r.StartStop ?? string.Empty).ToLowerInvariant();
                    string end = (r.EndStop ?? string.Empty).ToLowerInvariant();
                    if (num.Contains(term) || name.Contains(term) || start.Contains(term) || end.Contains(term))
                        filtered.Add(r);
                }
                _currentRouteList = filtered;
            }

            ApplySorting();
        }

        private void ApplySorting()
        {
            if (_currentRouteList == null || _currentRouteList.Count <= 1) return;

            string sortExpr = CurrentSortExpression;
            bool ascending = CurrentSortDirection == "ASC";

            Comparison<Route> comparison = null;
            switch (sortExpr)
            {
                case "route_id":
                    comparison = delegate(Route a, Route b) { return a.RouteId.CompareTo(b.RouteId); };
                    break;
                case "route_num":
                    comparison = delegate(Route a, Route b) { return string.Compare(a.RouteNum ?? "", b.RouteNum ?? "", StringComparison.Ordinal); };
                    break;
                case "route_name":
                    comparison = delegate(Route a, Route b) { return string.Compare(a.RouteName ?? "", b.RouteName ?? "", StringComparison.Ordinal); };
                    break;
                case "start_stop":
                    comparison = delegate(Route a, Route b) { return string.Compare(a.StartStop ?? "", b.StartStop ?? "", StringComparison.Ordinal); };
                    break;
                case "end_stop":
                    comparison = delegate(Route a, Route b) { return string.Compare(a.EndStop ?? "", b.EndStop ?? "", StringComparison.Ordinal); };
                    break;
                default:
                    comparison = delegate(Route a, Route b) { return string.Compare(a.RouteNum ?? "", b.RouteNum ?? "", StringComparison.Ordinal); };
                    break;
            }

            _currentRouteList.Sort(comparison);
            if (!ascending) _currentRouteList.Reverse();
        }

        private void BindGrid()
        {
            int totalCount = _currentRouteList != null ? _currentRouteList.Count : 0;
            int totalPages = (int)Math.Ceiling((double)totalCount / _pageSize);
            if (totalPages == 0) totalPages = 1;
            if (CurrentPage > totalPages) CurrentPage = totalPages;

            gvRoutes.PageSize = _pageSize;
            gvRoutes.PageIndex = CurrentPage - 1;
            gvRoutes.DataSource = _currentRouteList;
            gvRoutes.DataBind();
            RenderPagination(totalPages, totalCount);
        }

        private void RenderPagination(int totalPages, int totalCount)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<span class='text-small'>Total: ").Append(totalCount).Append(" routes");
            if (totalPages > 1)
                sb.Append(" | Page ").Append(CurrentPage).Append(" of ").Append(totalPages);
            sb.Append("</span>");
            litPagination.Text = sb.ToString();
        }

        #endregion

        #region Grid Events

        protected void gvRoutes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CurrentPage = e.NewPageIndex + 1;
            LoadRouteData();
        }

        protected void gvRoutes_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression == CurrentSortExpression)
                CurrentSortDirection = CurrentSortDirection == "ASC" ? "DESC" : "ASC";
            else
            {
                CurrentSortExpression = e.SortExpression;
                CurrentSortDirection = "ASC";
            }
            CurrentPage = 1;
            LoadRouteData();
        }

        protected void gvRoutes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewStops" || e.CommandName == "EditRoute" || e.CommandName == "DeleteRoute")
            {
                int routeId;
                if (int.TryParse(e.CommandArgument.ToString(), out routeId))
                {
                    switch (e.CommandName)
                    {
                        case "ViewStops":
                            ShowRouteStops(routeId);
                            break;
                        case "EditRoute":
                            ShowEditForm(routeId);
                            break;
                        case "DeleteRoute":
                            DeleteRouteRecord(routeId);
                            break;
                    }
                }
            }
        }

        protected void gvRoutes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button deleteButton = e.Row.FindControl("btnDelete") as Button;
                if (deleteButton != null)
                {
                    deleteButton.OnClientClick = "return confirm(" +
                        HttpUtility.JavaScriptStringEncode(Localization.Get("Common_DeleteRouteConfirm"), true) + ");";
                }

                Route route = (Route)e.Row.DataItem;
                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                if (litStatus != null)
                {
                    if (route.IsActive)
                        litStatus.Text = "<span class='status-active'>Active</span>";
                    else
                        litStatus.Text = "<span class='status-inactive'>Inactive</span>";
                }
            }
        }

        #endregion

        #region Route Stops Detail

        private void ShowRouteStops(int routeId)
        {
            try
            {
                using (RouteService service = new RouteService())
                {
                    Route route = service.GetRouteDetails(routeId);
                    List<RouteStop> stops = service.GetRouteStops(routeId);

                    StringBuilder sb = new StringBuilder();

                    if (route != null)
                    {
                        sb.Append("<div class='detail-section'>");
                        sb.Append("<div class='detail-row'><span class='detail-label'>Route:</span><span class='route-num'>").Append(Server.HtmlEncode(route.RouteNum)).Append("</span></div>");
                        sb.Append("<div class='detail-row'><span class='detail-label'>Name:</span><span class='detail-value'>").Append(Server.HtmlEncode(route.RouteName)).Append("</span></div>");
                        sb.Append("<div class='detail-row'><span class='detail-label'>From:</span><span class='detail-value'>").Append(Server.HtmlEncode(route.StartStop)).Append("</span></div>");
                        sb.Append("<div class='detail-row'><span class='detail-label'>To:</span><span class='detail-value'>").Append(Server.HtmlEncode(route.EndStop)).Append("</span></div>");
                        sb.Append("<div class='detail-row'><span class='detail-label'>Total Stops:</span><span class='detail-value'>").Append(route.StopCount).Append("</span></div>");
                        sb.Append("</div>");
                    }

                    if (stops != null && stops.Count > 0)
                    {
                        sb.Append("<div class='stop-list'>");
                        sb.Append("<table class='data-table'>");
                        sb.Append("<tr><th>#</th><th>Stop Name</th><th>Location</th><th>Distance (km)</th><th>Offset (min)</th></tr>");

                        foreach (RouteStop stop in stops)
                        {
                            sb.Append("<tr>");
                            sb.Append("<td style='text-align:center;font-weight:bold;color:#1447AE;'>").Append(stop.StopSequence).Append("</td>");
                            sb.Append("<td>").Append(Server.HtmlEncode(stop.StopName ?? "")).Append("</td>");
                            sb.Append("<td>").Append(Server.HtmlEncode(stop.StopLocation ?? "—")).Append("</td>");
                            sb.Append("<td>").Append(stop.DistanceKm.HasValue ? stop.DistanceKm.Value.ToString("0.00") : "—").Append("</td>");
                            sb.Append("<td>").Append(stop.ArrivalOffsetMin.HasValue ? stop.ArrivalOffsetMin.Value.ToString() : "—").Append("</td>");
                            sb.Append("</tr>");
                        }

                        sb.Append("</table>");
                        sb.Append("</div>");
                    }
                    else
                    {
                        sb.Append("<p class='text-regular'>No stops defined for this route.</p>");
                    }

                    litRouteStops.Text = sb.ToString();
                    cbRouteStops.Visible = true;
                    cbRouteForm.Visible = false;
                    HideMessages();
                }
            }
            catch (Exception ex)
            {
                ShowError("Error loading route stops: " + ex.Message);
            }
        }

        protected void btnCloseStops_Click(object sender, EventArgs e)
        {
            cbRouteStops.Visible = false;
        }

        #endregion

        #region Add/Edit Form

        protected void btnAddRoute_Click(object sender, EventArgs e)
        {
            EditingRouteId = 0;
            ClearForm();
            cbRouteForm.HeaderText = "Add New Route";
            cbRouteForm.Visible = true;
            cbRouteStops.Visible = false;
            HideMessages();
        }

        private void ShowEditForm(int routeId)
        {
            try
            {
                using (RouteService service = new RouteService())
                {
                    Route route = service.GetRouteById(routeId);
                    if (route != null)
                    {
                        EditingRouteId = route.RouteId;
                        txtRouteNum.Text = route.RouteNum;
                        txtRouteName.Text = route.RouteName;
                        txtStartStop.Text = route.StartStop;
                        txtEndStop.Text = route.EndStop;
                        chkActive.Checked = route.IsActive;
                        cbRouteForm.HeaderText = "Edit Route";
                        cbRouteForm.Visible = true;
                        cbRouteStops.Visible = false;
                        HideMessages();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Error loading route for edit: " + ex.Message);
            }
        }

        private void ClearForm()
        {
            txtRouteNum.Text = string.Empty;
            txtRouteName.Text = string.Empty;
            txtStartStop.Text = string.Empty;
            txtEndStop.Text = string.Empty;
            chkActive.Checked = true;
        }

        protected void btnSaveRoute_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                Route route = new Route();
                route.RouteId = EditingRouteId;
                route.RouteNum = txtRouteNum.Text.Trim();
                route.RouteName = txtRouteName.Text.Trim();
                route.StartStop = txtStartStop.Text.Trim();
                route.EndStop = txtEndStop.Text.Trim();
                route.IsActive = chkActive.Checked;

                using (RouteService service = new RouteService())
                {
                    if (EditingRouteId == 0)
                    {
                        int newId = service.CreateRoute(route);
                        ShowSuccess("Route created successfully. ID: " + newId);
                    }
                    else
                    {
                        bool success = service.UpdateRoute(route);
                        if (success)
                            ShowSuccess("Route updated successfully.");
                        else
                            ShowError("Failed to update route.");
                    }
                }

                cbRouteForm.Visible = false;
                LoadRouteData();
                LoadStats();
            }
            catch (ServiceException svcEx)
            {
                ShowError(svcEx.Message + (svcEx.InnerException != null ? " (" + svcEx.InnerException.Message + ")" : ""));
            }
            catch (Exception ex)
            {
                ShowError("Error saving route: " + ex.Message);
            }
        }

        protected void btnCancelRoute_Click(object sender, EventArgs e)
        {
            cbRouteForm.Visible = false;
            ClearForm();
            HideMessages();
        }

        #endregion

        #region Delete

        private void DeleteRouteRecord(int routeId)
        {
            try
            {
                using (RouteService service = new RouteService())
                {
                    bool success = service.DeleteRoute(routeId);
                    if (success)
                    {
                        ShowSuccess("Route deleted successfully.");
                        LoadRouteData();
                        LoadStats();
                    }
                    else
                    {
                        ShowError("Failed to delete route.");
                    }
                }
            }
            catch (ServiceException svcEx)
            {
                ShowError(svcEx.Message);
            }
            catch (Exception ex)
            {
                ShowError("Error deleting route: " + ex.Message);
            }
        }

        #endregion

        #region Other Actions

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            SearchTerm = string.Empty;
            CurrentSortExpression = "route_num";
            CurrentSortDirection = "ASC";
            txtSearch.Text = string.Empty;
            LoadRouteData();
            LoadStats();
            cbRouteForm.Visible = false;
            cbRouteStops.Visible = false;
            ShowSuccess("Data refreshed successfully.");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchTerm = txtSearch.Text.Trim();
            CurrentPage = 1;
            LoadRouteData();
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
            System.Diagnostics.Debug.WriteLine("Routes SQL Error: " + sqlEx.ToString());
        }

        private void HandleServiceError(ServiceException svcEx)
        {
            string msg = svcEx.Message;
            if (svcEx.InnerException != null) msg += " (" + svcEx.InnerException.Message + ")";
            ShowError(msg);
            System.Diagnostics.Debug.WriteLine("Routes Service Error: " + svcEx.ToString());
        }

        private void HandleGenericError(Exception ex)
        {
            ShowError("Unexpected error: " + ex.Message);
            System.Diagnostics.Debug.WriteLine("Routes Generic Error: " + ex.ToString());
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

        private void HideMessages()
        {
            pnlError.Visible = false;
            pnlSuccess.Visible = false;
        }

        #endregion
    }
}
