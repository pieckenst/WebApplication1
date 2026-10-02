using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BRU.WEBFORMS.ASPNET.APP.Services;
using BRU.WEBFORMS.ASPNET.APP.Models;
using System.Data.SqlClient;

namespace BRU.WEBFORMS.ASPNET.APP.Operations
{
    /// <summary>
    /// Schedule Management page for Autopark Management System.
    /// Provides daily trip schedule viewing, date range filtering,
    /// route filtering, sorting, and pagination with comprehensive
    /// statistics tracking.
    /// </summary>
    public partial class Schedule : System.Web.UI.Page
    {
        #region Control Declarations

        protected global::System.Web.UI.WebControls.Panel pnlError;
        protected global::System.Web.UI.WebControls.Literal litError;
        protected global::System.Web.UI.WebControls.Panel pnlSuccess;
        protected global::System.Web.UI.WebControls.Literal litSuccess;
        protected global::System.Web.UI.WebControls.Literal litTodayTrips;
        protected global::System.Web.UI.WebControls.Literal litPlanned;
        protected global::System.Web.UI.WebControls.Literal litInProgress;
        protected global::System.Web.UI.WebControls.Literal litCompleted;
        protected global::System.Web.UI.WebControls.Literal litCancelled;
        protected global::System.Web.UI.WebControls.Button btnToday;
        protected global::System.Web.UI.WebControls.Button btnRefresh;
        protected global::System.Web.UI.WebControls.DropDownList ddlRouteFilter;
        protected global::System.Web.UI.WebControls.TextBox txtDateFrom;
        protected global::System.Web.UI.WebControls.TextBox txtDateTo;
        protected global::System.Web.UI.WebControls.Button btnApplyDateRange;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbScheduleList;
        protected global::System.Web.UI.WebControls.GridView gvSchedule;
        protected global::System.Web.UI.WebControls.Literal litPagination;

        #endregion

        #region Private Fields

        private List<RouteSchedule> _currentScheduleList;
        private const int _pageSize = 20;

        #endregion

        #region State Properties

        private int CurrentPage
        {
            get
            {
                object val = ViewState["Schedule_CurrentPage"];
                if (val == null) return 1;
                int page;
                if (int.TryParse(val.ToString(), out page) && page > 0) return page;
                return 1;
            }
            set { ViewState["Schedule_CurrentPage"] = value < 1 ? 1 : value; }
        }

        private string CurrentSortExpression
        {
            get
            {
                object val = ViewState["Schedule_SortExpr"];
                return val == null ? "departure_time" : val.ToString();
            }
            set { ViewState["Schedule_SortExpr"] = value; }
        }

        private string CurrentSortDirection
        {
            get
            {
                object val = ViewState["Schedule_SortDir"];
                return val == null ? "ASC" : val.ToString();
            }
            set { ViewState["Schedule_SortDir"] = value; }
        }

        private DateTime DateFrom
        {
            get
            {
                object val = ViewState["Schedule_DateFrom"];
                if (val == null) return DateTime.Today;
                DateTime dt;
                if (DateTime.TryParse(val.ToString(), out dt)) return dt;
                return DateTime.Today;
            }
            set { ViewState["Schedule_DateFrom"] = value; }
        }

        private DateTime DateTo
        {
            get
            {
                object val = ViewState["Schedule_DateTo"];
                if (val == null) return DateTime.Today.AddDays(1);
                DateTime dt;
                if (DateTime.TryParse(val.ToString(), out dt)) return dt;
                return DateTime.Today.AddDays(1);
            }
            set { ViewState["Schedule_DateTo"] = value; }
        }

        private string FilterRouteId
        {
            get
            {
                object val = ViewState["Schedule_FilterRouteId"];
                return val == null ? string.Empty : val.ToString();
            }
            set { ViewState["Schedule_FilterRouteId"] = value; }
        }

        #endregion

        #region Page Lifecycle

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LoadRouteFilter();
                    LoadTodaySchedule();
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

        #region Data Loading

        private void LoadRouteFilter()
        {
            using (RouteService service = new RouteService())
            {
                List<Route> routes = service.GetAllActiveRoutes();

                ddlRouteFilter.Items.Clear();
                ddlRouteFilter.Items.Add(new ListItem("All Routes", ""));

                if (routes != null)
                {
                    foreach (Route route in routes)
                    {
                        ddlRouteFilter.Items.Add(
                            new ListItem(route.RouteNum + " - " + route.RouteName,
                                route.RouteId.ToString()));
                    }
                }
            }
        }

        private void LoadTodaySchedule()
        {
            using (RouteService service = new RouteService())
            {
                _currentScheduleList = service.GetTodaySchedule();
            }

            ApplyFiltersAndSort();
            BindGrid();
        }

        private void LoadScheduleByDateRange()
        {
            using (RouteService service = new RouteService())
            {
                _currentScheduleList = service.GetScheduleRange(DateFrom, DateTo);
            }

            ApplyFiltersAndSort();
            BindGrid();
        }

        private void LoadStats()
        {
            if (_currentScheduleList == null || _currentScheduleList.Count == 0)
            {
                using (RouteService service = new RouteService())
                {
                    _currentScheduleList = service.GetTodaySchedule();
                }
            }

            litTodayTrips.Text = _currentScheduleList.Count.ToString();
            litPlanned.Text = CountByStatus(_currentScheduleList, ScheduleStatus.Planned).ToString();
            litInProgress.Text = CountByStatus(_currentScheduleList, ScheduleStatus.InProgress).ToString();
            litCompleted.Text = CountByStatus(_currentScheduleList, ScheduleStatus.Completed).ToString();
            litCancelled.Text = CountByStatus(_currentScheduleList, ScheduleStatus.Cancelled).ToString();
        }

        private int CountByStatus(List<RouteSchedule> schedules, string status)
        {
            int count = 0;
            foreach (RouteSchedule s in schedules)
            {
                if (s.ScheduleStatus == status) count++;
            }
            return count;
        }

        private void ApplyFiltersAndSort()
        {
            if (_currentScheduleList == null) _currentScheduleList = new List<RouteSchedule>();

            // Apply route filter
            if (!string.IsNullOrEmpty(FilterRouteId))
            {
                int routeId;
                if (int.TryParse(FilterRouteId, out routeId))
                {
                    List<RouteSchedule> filtered = new List<RouteSchedule>();
                    foreach (RouteSchedule s in _currentScheduleList)
                    {
                        if (s.RouteId == routeId) filtered.Add(s);
                    }
                    _currentScheduleList = filtered;
                }
            }

            ApplySorting();
        }

        private void ApplySorting()
        {
            if (_currentScheduleList == null || _currentScheduleList.Count <= 1) return;

            string sortExpr = CurrentSortExpression;
            bool ascending = CurrentSortDirection == "ASC";

            Comparison<RouteSchedule> comparison = null;
            switch (sortExpr)
            {
                case "schedule_id":
                    comparison = delegate(RouteSchedule a, RouteSchedule b) { return a.ScheduleId.CompareTo(b.ScheduleId); };
                    break;
                case "route_num":
                    comparison = delegate(RouteSchedule a, RouteSchedule b) { return string.Compare(a.RouteNum ?? "", b.RouteNum ?? "", StringComparison.Ordinal); };
                    break;
                case "route_name":
                    comparison = delegate(RouteSchedule a, RouteSchedule b) { return string.Compare(a.RouteName ?? "", b.RouteName ?? "", StringComparison.Ordinal); };
                    break;
                case "service_date":
                    comparison = delegate(RouteSchedule a, RouteSchedule b) { return a.ServiceDate.CompareTo(b.ServiceDate); };
                    break;
                case "departure_time":
                    comparison = delegate(RouteSchedule a, RouteSchedule b) { return a.DepartureTime.CompareTo(b.DepartureTime); };
                    break;
                case "arrival_time":
                    comparison = delegate(RouteSchedule a, RouteSchedule b) { return a.ArrivalTime.CompareTo(b.ArrivalTime); };
                    break;
                case "trip_minutes":
                    comparison = delegate(RouteSchedule a, RouteSchedule b) { return a.TripMinutes.CompareTo(b.TripMinutes); };
                    break;
                case "fleet_number":
                    comparison = delegate(RouteSchedule a, RouteSchedule b) { return string.Compare(a.FleetNumber ?? "", b.FleetNumber ?? "", StringComparison.Ordinal); };
                    break;
                case "driver_name":
                    comparison = delegate(RouteSchedule a, RouteSchedule b) { return string.Compare(a.DriverName ?? "", b.DriverName ?? "", StringComparison.Ordinal); };
                    break;
                case "schedule_status":
                    comparison = delegate(RouteSchedule a, RouteSchedule b) { return string.Compare(a.ScheduleStatus ?? "", b.ScheduleStatus ?? "", StringComparison.Ordinal); };
                    break;
                default:
                    comparison = delegate(RouteSchedule a, RouteSchedule b) { return a.DepartureTime.CompareTo(b.DepartureTime); };
                    break;
            }

            _currentScheduleList.Sort(comparison);
            if (!ascending) _currentScheduleList.Reverse();
        }

        private void BindGrid()
        {
            int totalCount = _currentScheduleList != null ? _currentScheduleList.Count : 0;
            int totalPages = (int)Math.Ceiling((double)totalCount / _pageSize);
            if (totalPages == 0) totalPages = 1;
            if (CurrentPage > totalPages) CurrentPage = totalPages;

            int skip = (CurrentPage - 1) * _pageSize;
            int take = Math.Min(_pageSize, totalCount - skip);
            if (take < 0) take = 0;

            List<RouteSchedule> pageData = new List<RouteSchedule>();
            for (int i = skip; i < skip + take && i < totalCount; i++)
            {
                pageData.Add(_currentScheduleList[i]);
            }

            gvSchedule.DataSource = pageData;
            gvSchedule.DataBind();
            RenderPagination(totalPages, totalCount);
        }

        private void RenderPagination(int totalPages, int totalCount)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<span class='text-small'>Total: ").Append(totalCount).Append(" trips | </span>");

            if (totalPages <= 1)
            {
                sb.Append("<span class='current'>1</span>");
            }
            else
            {
                if (CurrentPage > 1)
                    sb.Append("<a href=\"#\" onclick=\"__doPostBack('gvSchedule','Page$").Append(CurrentPage - 1).Append("');return false;\">&#9664; Prev</a>");

                int startPage = Math.Max(1, CurrentPage - 3);
                int endPage = Math.Min(totalPages, CurrentPage + 3);

                for (int i = startPage; i <= endPage; i++)
                {
                    if (i == CurrentPage)
                        sb.Append("<span class='current'>").Append(i).Append("</span>");
                    else
                        sb.Append("<a href=\"#\" onclick=\"__doPostBack('gvSchedule','Page$").Append(i).Append("');return false;\">").Append(i).Append("</a>");
                }

                if (CurrentPage < totalPages)
                    sb.Append("<a href=\"#\" onclick=\"__doPostBack('gvSchedule','Page$").Append(CurrentPage + 1).Append("');return false;\">Next &#9654;</a>");
            }

            litPagination.Text = sb.ToString();
        }

        #endregion

        #region Grid Events

        protected void gvSchedule_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CurrentPage = e.NewPageIndex + 1;
            LoadScheduleByDateRange();
        }

        protected void gvSchedule_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression == CurrentSortExpression)
                CurrentSortDirection = CurrentSortDirection == "ASC" ? "DESC" : "ASC";
            else
            {
                CurrentSortExpression = e.SortExpression;
                CurrentSortDirection = "ASC";
            }
            CurrentPage = 1;
            LoadScheduleByDateRange();
        }

        protected void gvSchedule_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                RouteSchedule schedule = (RouteSchedule)e.Row.DataItem;
                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                if (litStatus != null)
                {
                    string cssClass;
                    switch (schedule.ScheduleStatus)
                    {
                        case ScheduleStatus.Planned:
                            cssClass = "status-planned";
                            break;
                        case ScheduleStatus.InProgress:
                            cssClass = "status-inprogress";
                            break;
                        case ScheduleStatus.Completed:
                            cssClass = "status-completed";
                            break;
                        case ScheduleStatus.Cancelled:
                            cssClass = "status-cancelled";
                            break;
                        default:
                            cssClass = "status-planned";
                            break;
                    }
                    litStatus.Text = "<span class='" + cssClass + "'>" + schedule.ScheduleStatus + "</span>";
                }

                foreach (TableCell cell in e.Row.Cells)
                {
                    if (cell.Text != null && cell.Text == "&nbsp;")
                        cell.Text = "<span class='text-small'>—</span>";
                }
            }
        }

        #endregion

        #region Action Button Events

        protected void btnToday_Click(object sender, EventArgs e)
        {
            try
            {
                DateFrom = DateTime.Today;
                DateTo = DateTime.Today.AddDays(1);
                CurrentPage = 1;
                txtDateFrom.Text = DateTime.Today.ToString("dd.MM.yyyy");
                txtDateTo.Text = DateTime.Today.AddDays(1).ToString("dd.MM.yyyy");
                LoadTodaySchedule();
                LoadStats();
                ShowSuccess("Showing today's schedule.");
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            FilterRouteId = string.Empty;
            CurrentSortExpression = "departure_time";
            CurrentSortDirection = "ASC";
            ddlRouteFilter.SelectedIndex = 0;
            txtDateFrom.Text = string.Empty;
            txtDateTo.Text = string.Empty;
            DateFrom = DateTime.Today;
            DateTo = DateTime.Today.AddDays(1);
            LoadTodaySchedule();
            LoadStats();
            ShowSuccess("Schedule refreshed successfully.");
        }

        protected void ddlRouteFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterRouteId = ddlRouteFilter.SelectedValue;
            CurrentPage = 1;
            LoadScheduleByDateRange();
        }

        protected void btnApplyDateRange_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dateFrom;
                DateTime dateTo;

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

                DateFrom = dateFrom;
                DateTo = dateTo.AddDays(1);
                CurrentPage = 1;
                LoadScheduleByDateRange();
                LoadStats();
                ShowSuccess("Schedule loaded for " + dateFrom.ToString("dd.MM.yyyy") + " - " + dateTo.ToString("dd.MM.yyyy"));
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
            System.Diagnostics.Debug.WriteLine("Schedule SQL Error: " + sqlEx.ToString());
        }

        private void HandleServiceError(ServiceException svcEx)
        {
            string msg = svcEx.Message;
            if (svcEx.InnerException != null) msg += " (" + svcEx.InnerException.Message + ")";
            ShowError(msg);
            System.Diagnostics.Debug.WriteLine("Schedule Service Error: " + svcEx.ToString());
        }

        private void HandleGenericError(Exception ex)
        {
            ShowError("Unexpected error: " + ex.Message);
            System.Diagnostics.Debug.WriteLine("Schedule Generic Error: " + ex.ToString());
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
