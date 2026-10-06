using System;
using System.Collections.Generic;
using System.Globalization;
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
    public partial class Schedule : SecurePage
    {
        protected override string[] RequiredPermissions { get { return new[] { "route.read" }; } }
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
        protected global::System.Web.UI.WebControls.Button btnUpdateStatuses;
        protected global::System.Web.UI.WebControls.DropDownList ddlRouteFilter;
        protected global::System.Web.UI.WebControls.TextBox txtDateFrom;
        protected global::System.Web.UI.WebControls.TextBox txtDateTo;
        protected global::System.Web.UI.WebControls.Button btnApplyDateRange;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbScheduleList;
        protected global::System.Web.UI.WebControls.GridView gvSchedule;
        protected global::System.Web.UI.WebControls.Literal litPagination;
        protected global::System.Web.UI.WebControls.Panel pnlScheduleEditor;
        protected global::System.Web.UI.WebControls.HiddenField hidScheduleId;
        protected global::System.Web.UI.WebControls.DropDownList ddlScheduleRoute;
        protected global::System.Web.UI.WebControls.DropDownList ddlScheduleBus;
        protected global::System.Web.UI.WebControls.DropDownList ddlScheduleDriver;
        protected global::System.Web.UI.WebControls.TextBox txtServiceDate;
        protected global::System.Web.UI.WebControls.TextBox txtDeparture;
        protected global::System.Web.UI.WebControls.TextBox txtArrival;
        protected global::System.Web.UI.WebControls.Button btnNewSchedule;
        protected global::System.Web.UI.WebControls.Button btnSaveSchedule;
        protected global::System.Web.UI.WebControls.Button btnCancelScheduleEdit;
        protected global::System.Web.UI.WebControls.TextBox txtTemplateDate;
        protected global::System.Web.UI.WebControls.TextBox txtRecurringFrom;
        protected global::System.Web.UI.WebControls.TextBox txtRecurringTo;
        protected global::System.Web.UI.WebControls.CheckBoxList cblRecurringDays;
        protected global::System.Web.UI.WebControls.Button btnGenerateRecurring;
        protected global::System.Web.UI.WebControls.Button btnValidateSchedule;

        #endregion

        #region Private Fields

        private List<RouteSchedule> _currentScheduleList;
        private const int _pageSize = 20;
        private bool _templateControlsResolved;

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
                EnsureTemplateControlsResolved();
                pnlScheduleEditor.Visible = CanEditSchedules;
                btnUpdateStatuses.Visible = CanEditSchedules;

                if (!IsPostBack)
                {
                    using (ScheduleAutomationService automation = new ScheduleAutomationService())
                    {
                        ScheduleStatusUpdateResult statusResult = automation.UpdateScheduleStatuses();
                        LogInformation("Automatic trip status update result: success=" + statusResult.Success +
                            ", processed=" + statusResult.TotalProcessed + ", transitions=" +
                            statusResult.TotalUpdated + ", error=" + (statusResult.ErrorMessage ?? "none"));
                        if (!statusResult.Success)
                            ShowError("Automatic trip status update failed: " + statusResult.ErrorMessage);
                    }

                    LoadRouteFilter();
                    LoadScheduleEditorResources();
                    ClearScheduleEditor();
                    txtRecurringFrom.Text = DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    txtRecurringTo.Text = DateTime.Today.AddDays(30).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    // Load all schedules by default instead of just today
                    DateFrom = new DateTime(2020, 1, 1);
                    DateTo = new DateTime(2031, 1, 1);
                    LoadScheduleByDateRange();
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

            EnsureContentBoxCreated(cbScheduleList, "cbScheduleList");

            // Resolve schedule grid control
            gvSchedule = FindRequiredTemplateControl<GridView>(cbScheduleList, "gvSchedule");

            _templateControlsResolved = true;
        }

        private static void EnsureContentBoxCreated(Controls.ContentBox contentBox, string controlId)
        {
            if (contentBox == null)
                throw new InvalidOperationException(
                    "Required ContentBox '" + controlId + "' was not created. Check Schedule.aspx markup and the ContentBox registration.");
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

        private bool CanEditSchedules
        {
            get { return AuthContext.HasPermission("route.write"); }
        }

        private void LoadScheduleEditorResources()
        {
            ddlScheduleRoute.Items.Clear();
            ddlScheduleRoute.Items.Add(new ListItem("-- Select route --", ""));
            using (RouteService service = new RouteService())
            {
                foreach (Route route in service.GetAllActiveRoutes())
                    ddlScheduleRoute.Items.Add(new ListItem(route.RouteNum + " - " + route.RouteName, route.RouteId.ToString()));
            }

            ddlScheduleBus.Items.Clear();
            ddlScheduleBus.Items.Add(new ListItem("-- Select bus --", ""));
            using (BusService service = new BusService())
            {
                foreach (Bus bus in service.GetAllBuses())
                    ddlScheduleBus.Items.Add(new ListItem(bus.FleetNumber + " (" + bus.Model + ", " + bus.Status + ")", bus.BusId.ToString()));
            }

            ddlScheduleDriver.Items.Clear();
            ddlScheduleDriver.Items.Add(new ListItem("-- Select driver --", ""));
            using (EmployeeService service = new EmployeeService())
            {
                foreach (Employee driver in service.GetDrivers())
                    ddlScheduleDriver.Items.Add(new ListItem(driver.EmployeeName + " (" + driver.Status + ")", driver.EmployeeId.ToString()));
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
            List<RouteSchedule> todaySchedules;
            using (RouteService service = new RouteService())
            {
                todaySchedules = service.GetTodaySchedule();
            }

            litTodayTrips.Text = todaySchedules.Count.ToString();
            litPlanned.Text = CountByStatus(todaySchedules, ScheduleStatus.Planned).ToString();
            litInProgress.Text = CountByStatus(todaySchedules, ScheduleStatus.InProgress).ToString();
            litCompleted.Text = CountByStatus(todaySchedules, ScheduleStatus.Completed).ToString();
            litCancelled.Text = CountByStatus(todaySchedules, ScheduleStatus.Cancelled).ToString();
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

            gvSchedule.PageSize = _pageSize;
            gvSchedule.PageIndex = CurrentPage - 1;
            gvSchedule.DataSource = _currentScheduleList;
            gvSchedule.DataBind();
            RenderPagination(totalPages, totalCount);
        }

        private void RenderPagination(int totalPages, int totalCount)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<span class='text-small'>Total: ").Append(totalCount).Append(" trips");
            if (totalPages > 1)
                sb.Append(" | Page ").Append(CurrentPage).Append(" of ").Append(totalPages);
            sb.Append("</span>");
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

                LinkButton editButton = e.Row.FindControl("btnEditSchedule") as LinkButton;
                if (editButton != null)
                    editButton.Visible = CanEditSchedules && schedule.ScheduleStatus == ScheduleStatus.Planned;

                foreach (TableCell cell in e.Row.Cells)
                {
                    if (cell.Text != null && cell.Text == "&nbsp;")
                        cell.Text = "<span class='text-small'>—</span>";
                }
            }
        }

        protected void gvSchedule_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "EditSchedule") return;
            try
            {
                EnsureScheduleWriteAccess();
                int scheduleId;
                if (!int.TryParse(Convert.ToString(e.CommandArgument), out scheduleId))
                    throw new ServiceException("Invalid schedule selection.");

                using (ScheduleAutomationService service = new ScheduleAutomationService())
                    PopulateScheduleEditor(service.GetScheduleById(scheduleId));
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
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
                txtDateTo.Text = DateTime.Today.ToString("dd.MM.yyyy");
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
                string dateFromText = txtDateFrom.Text.Trim();
                string dateToText = txtDateTo.Text.Trim();

                // If both dates are empty, show ALL schedules
                if (string.IsNullOrEmpty(dateFromText) && string.IsNullOrEmpty(dateToText))
                {
                    // Load all schedules (use a wide date range)
                    DateFrom = new DateTime(2020, 1, 1);
                    DateTo = new DateTime(2031, 1, 1);
                    CurrentPage = 1;
                    LoadScheduleByDateRange();
                    LoadStats();
                    ShowSuccess("Showing all schedules in the database.");
                    return;
                }

                // If only one date is provided, require both
                if (string.IsNullOrEmpty(dateFromText) || string.IsNullOrEmpty(dateToText))
                {
                    ShowError("Please provide both Date From and Date To, or leave both empty to show all schedules.");
                    return;
                }

                DateTime dateFrom;
                DateTime dateTo;

                if (!DateTime.TryParseExact(dateFromText, "dd.MM.yyyy",
                    null, System.Globalization.DateTimeStyles.None, out dateFrom))
                {
                    ShowError("Invalid 'Date From' format. Use dd.MM.yyyy");
                    return;
                }

                if (!DateTime.TryParseExact(dateToText, "dd.MM.yyyy",
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

        protected void btnNewSchedule_Click(object sender, EventArgs e)
        {
            try
            {
                EnsureScheduleWriteAccess();
                ClearScheduleEditor();
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        protected void btnCancelScheduleEdit_Click(object sender, EventArgs e)
        {
            ClearScheduleEditor();
        }

        protected void btnSaveSchedule_Click(object sender, EventArgs e)
        {
            try
            {
                EnsureScheduleWriteAccess();
                RouteSchedule schedule = ReadScheduleEditor();
                using (ScheduleAutomationService service = new ScheduleAutomationService())
                    schedule.ScheduleId = service.SaveSchedule(schedule);

                DateFrom = schedule.ServiceDate.Date;
                DateTo = schedule.ServiceDate.Date.AddDays(1);
                txtDateFrom.Text = schedule.ServiceDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
                txtDateTo.Text = txtDateFrom.Text;
                FilterRouteId = string.Empty;
                ddlRouteFilter.SelectedIndex = 0;
                CurrentPage = 1;
                LoadScheduleByDateRange();
                LoadStats();
                ClearScheduleEditor();
                ShowSuccess("Schedule saved. Seat availability is calculated from current non-refunded sales.");
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        protected void btnUpdateStatuses_Click(object sender, EventArgs e)
        {
            try
            {
                EnsureScheduleWriteAccess();
                ScheduleStatusUpdateResult result;
                using (ScheduleAutomationService service = new ScheduleAutomationService())
                    result = service.UpdateScheduleStatuses();
                LogInformation("Manual trip status reconciliation result: success=" + result.Success +
                    ", processed=" + result.TotalProcessed + ", transitions=" + result.TotalUpdated +
                    ", error=" + (result.ErrorMessage ?? "none"));
                if (!result.Success)
                    throw new ServiceException(result.ErrorMessage);

                LoadScheduleByDateRange();
                LoadStats();
                ShowSuccess("Trip statuses reconciled. " + result.TotalUpdated + " transition(s) applied.");
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        protected void btnGenerateRecurring_Click(object sender, EventArgs e)
        {
            try
            {
                EnsureScheduleWriteAccess();
                DateTime templateDate = ParseIsoDate(txtTemplateDate.Text, "Template date");
                DateTime startDate = ParseIsoDate(txtRecurringFrom.Text, "Generate from");
                DateTime endDate = ParseIsoDate(txtRecurringTo.Text, "Generate through");
                if (endDate.Date < startDate.Date)
                    throw new ServiceException("Generate through must be on or after Generate from.");
                if (endDate.Date.Subtract(startDate.Date).TotalDays >= 90)
                    throw new ServiceException("The selected date range cannot exceed 90 days.");

                List<DayOfWeek> days = new List<DayOfWeek>();
                foreach (ListItem item in cblRecurringDays.Items)
                {
                    if (item.Selected)
                        days.Add((DayOfWeek)Enum.Parse(typeof(DayOfWeek), item.Value));
                }
                if (days.Count == 0)
                    throw new ServiceException("Select at least one day of the week.");
                bool hasSelectedDay = false;
                for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                    if (days.Contains(date.DayOfWeek)) hasSelectedDay = true;
                if (!hasSelectedDay)
                    throw new ServiceException("The selected weekdays do not occur in the requested date range.");

                string selectedWeekdays = string.Join(",", days);
                string generationRequest = "range=" + startDate.ToString("yyyy-MM-dd") + ".." +
                    endDate.ToString("yyyy-MM-dd") + ", template=" + templateDate.ToString("yyyy-MM-dd") +
                    ", weekdays=" + selectedWeekdays;
                if (startDate.Date < DateTime.Today)
                    LogWarning("HISTORICAL BACKFILL requested; past trips will be stored as Completed; " + generationRequest);
                else
                    LogInformation("Recurring generation requested; " + generationRequest);

                BatchScheduleGenerationResult result;
                using (ScheduleAutomationService service = new ScheduleAutomationService())
                    result = service.GenerateBatchSchedules(startDate, endDate, templateDate, days);

                if (!result.Success)
                    throw new ServiceException("Generation completed with " + result.TotalDaysFailed +
                        " failed date(s). " + result.TotalSchedulesCreated + " trip(s) were created. " + result.ErrorMessage);

                DateFrom = startDate.Date;
                DateTo = endDate.Date.AddDays(1);
                txtDateFrom.Text = startDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
                txtDateTo.Text = endDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
                CurrentPage = 1;
                LoadScheduleByDateRange();
                LoadStats();
                string historyNotice = startDate.Date < DateTime.Today
                    ? " Past dates were processed as historical backfill; created past trips use status Completed."
                    : string.Empty;
                ShowSuccess("Recurring generation finished: " + result.TotalSchedulesCreated + " created across " +
                    result.TotalDaysProcessed + " selected day(s); " + result.TotalDaysFailed + " date(s) failed." +
                    historyNotice);
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        protected void btnValidateSchedule_Click(object sender, EventArgs e)
        {
            try
            {
                EnsureScheduleWriteAccess();
                LogInformation("Validate Schedule clicked; date range=" + DateFrom.ToString("yyyy-MM-dd") +
                    ".." + DateTo.ToString("yyyy-MM-dd") + " (end exclusive)");
                ScheduleValidationResult result;
                using (ScheduleAutomationService service = new ScheduleAutomationService())
                    result = service.ValidateScheduleIntegrity(DateFrom, DateTo);
                LogInformation("Validate Schedule finished; success=" + result.Success +
                    ", checked=" + result.TotalSchedulesValidated + ", issues=" +
                    result.TotalIssuesFound + ", error=" + (result.ErrorMessage ?? "none"));
                if (!result.Success)
                    throw new ServiceException(result.ErrorMessage);

                if (result.TotalIssuesFound == 0)
                {
                    ShowSuccess("Validated " + result.TotalSchedulesValidated + " schedule(s); no integrity issues found.");
                    return;
                }

                StringBuilder issues = new StringBuilder();
                int issueCount = Math.Min(5, result.Issues.Count);
                for (int i = 0; i < issueCount; i++)
                    issues.Append("Schedule ").Append(result.Issues[i].ScheduleId).Append(": ")
                        .Append(result.Issues[i].Issue).Append(" ");
                ShowError("Found " + result.TotalIssuesFound + " issue(s). " + issues.ToString());
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        private void EnsureScheduleWriteAccess()
        {
            if (!CanEditSchedules)
                throw new UnauthorizedAccessException("You do not have permission to change schedules.");
        }

        private RouteSchedule ReadScheduleEditor()
        {
            int scheduleId;
            int routeId;
            int busId;
            int driverId;
            DateTime serviceDate;
            TimeSpan departure;
            TimeSpan arrival;
            if (!int.TryParse(hidScheduleId.Value, out scheduleId) ||
                !int.TryParse(ddlScheduleRoute.SelectedValue, out routeId) ||
                !int.TryParse(ddlScheduleBus.SelectedValue, out busId) ||
                !int.TryParse(ddlScheduleDriver.SelectedValue, out driverId))
                throw new ServiceException("Select a route, bus, and driver.");
            serviceDate = ParseIsoDate(txtServiceDate.Text, "Service date");
            if (!TimeSpan.TryParseExact(txtDeparture.Text, @"hh\:mm", CultureInfo.InvariantCulture, out departure) ||
                !TimeSpan.TryParseExact(txtArrival.Text, @"hh\:mm", CultureInfo.InvariantCulture, out arrival))
                throw new ServiceException("Enter departure and arrival times in HH:mm format.");

            return new RouteSchedule
            {
                ScheduleId = scheduleId,
                RouteId = routeId,
                BusId = busId,
                DriverId = driverId,
                ServiceDate = serviceDate,
                DepartureTime = departure,
                ArrivalTime = arrival
            };
        }

        private static DateTime ParseIsoDate(string value, string fieldName)
        {
            DateTime date;
            if (!DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out date))
                throw new ServiceException(fieldName + " is required and must be a valid date.");
            return date.Date;
        }

        private void PopulateScheduleEditor(RouteSchedule schedule)
        {
            hidScheduleId.Value = schedule.ScheduleId.ToString(CultureInfo.InvariantCulture);
            string routeId = schedule.RouteId.ToString(CultureInfo.InvariantCulture);
            string busId = schedule.BusId.ToString(CultureInfo.InvariantCulture);
            string driverId = schedule.DriverId.ToString(CultureInfo.InvariantCulture);
            if (ddlScheduleRoute.Items.FindByValue(routeId) == null)
                ddlScheduleRoute.Items.Add(new ListItem(schedule.RouteNum + " - " + schedule.RouteName + " (inactive)", routeId));
            if (ddlScheduleBus.Items.FindByValue(busId) == null)
                ddlScheduleBus.Items.Add(new ListItem(schedule.FleetNumber + " (unavailable)", busId));
            if (ddlScheduleDriver.Items.FindByValue(driverId) == null)
                ddlScheduleDriver.Items.Add(new ListItem(schedule.DriverName + " (unavailable)", driverId));
            ddlScheduleRoute.SelectedValue = routeId;
            ddlScheduleBus.SelectedValue = busId;
            ddlScheduleDriver.SelectedValue = driverId;
            txtServiceDate.Text = schedule.ServiceDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            txtDeparture.Text = schedule.DepartureTime.ToString(@"hh\:mm", CultureInfo.InvariantCulture);
            txtArrival.Text = schedule.ArrivalTime.ToString(@"hh\:mm", CultureInfo.InvariantCulture);
        }

        private void ClearScheduleEditor()
        {
            hidScheduleId.Value = "0";
            if (ddlScheduleRoute.Items.Count > 0) ddlScheduleRoute.SelectedIndex = 0;
            if (ddlScheduleBus.Items.Count > 0) ddlScheduleBus.SelectedIndex = 0;
            if (ddlScheduleDriver.Items.Count > 0) ddlScheduleDriver.SelectedIndex = 0;
            txtServiceDate.Text = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            txtDeparture.Text = string.Empty;
            txtArrival.Text = string.Empty;
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
            ShowError(ex is ServiceException || ex is UnauthorizedAccessException
                ? ex.Message
                : "Unexpected error: " + ex.Message);
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
