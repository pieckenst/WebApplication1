using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BRU.WEBFORMS.ASPNET.APP.Services;
using BRU.WEBFORMS.ASPNET.APP.Models;
using System.Data.SqlClient;

namespace BRU.WEBFORMS.ASPNET.APP.Personnel
{
    /// <summary>
    /// Employee Management page for Autopark Management System.
    /// Provides comprehensive CRUD operations, filtering by status/job/department,
    /// search, sorting, pagination, and employee detail views.
    /// Implements enterprise-grade validation and error handling.
    /// </summary>
    public partial class Employees : SecurePage
    {
        protected override string[] RequiredPermissions { get { return new[] { "employee.read" }; } }
        #region Control Declarations

        protected global::System.Web.UI.WebControls.Panel pnlError;
        protected global::System.Web.UI.WebControls.Literal litError;
        protected global::System.Web.UI.WebControls.Panel pnlSuccess;
        protected global::System.Web.UI.WebControls.Literal litSuccess;
        protected global::System.Web.UI.WebControls.Literal litTotalEmployees;
        protected global::System.Web.UI.WebControls.Literal litActiveEmployees;
        protected global::System.Web.UI.WebControls.Literal litVacationEmployees;
        protected global::System.Web.UI.WebControls.Literal litSickEmployees;
        protected global::System.Web.UI.WebControls.Literal litDismissedEmployees;
        protected global::System.Web.UI.WebControls.Button btnAddEmployee;
        protected global::System.Web.UI.WebControls.Button btnRefresh;
        protected global::System.Web.UI.WebControls.TextBox txtSearch;
        protected global::System.Web.UI.WebControls.Button btnSearch;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbFilters;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbEmployeesList;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbEmployeeForm;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbEmployeeDetail;
        protected global::System.Web.UI.WebControls.GridView gvEmployees;
        protected global::System.Web.UI.WebControls.Literal litPagination;

        // Filter controls (inside cbFilters template)
        protected global::System.Web.UI.WebControls.DropDownList ddlStatusFilter;
        protected global::System.Web.UI.WebControls.DropDownList ddlJobFilter;
        protected global::System.Web.UI.WebControls.DropDownList ddlDepartmentFilter;

        // Form controls (inside cbEmployeeForm template)
        protected global::System.Web.UI.WebControls.TextBox txtSurname;
        protected global::System.Web.UI.WebControls.TextBox txtName;
        protected global::System.Web.UI.WebControls.TextBox txtPatronym;
        protected global::System.Web.UI.WebControls.TextBox txtEmployedDate;
        protected global::System.Web.UI.WebControls.DropDownList ddlJob;
        protected global::System.Web.UI.WebControls.DropDownList ddlDepartment;
        protected global::System.Web.UI.WebControls.TextBox txtPhone;
        protected global::System.Web.UI.WebControls.TextBox txtEmail;
        protected global::System.Web.UI.WebControls.DropDownList ddlStatus;
        protected global::System.Web.UI.WebControls.Button btnSave;
        protected global::System.Web.UI.WebControls.Button btnCancel;

        // Detail controls (inside cbEmployeeDetail template)
        protected global::System.Web.UI.WebControls.Literal litDetailSurname;
        protected global::System.Web.UI.WebControls.Literal litDetailName;
        protected global::System.Web.UI.WebControls.Literal litDetailPatronym;
        protected global::System.Web.UI.WebControls.Literal litDetailEmployedDate;
        protected global::System.Web.UI.WebControls.Literal litDetailJob;
        protected global::System.Web.UI.WebControls.Literal litDetailDepartment;
        protected global::System.Web.UI.WebControls.Literal litDetailPhone;
        protected global::System.Web.UI.WebControls.Literal litDetailEmail;
        protected global::System.Web.UI.WebControls.Literal litDetailStatus;
        protected global::System.Web.UI.WebControls.Literal litDetailServiceYears;
        protected global::System.Web.UI.WebControls.Button btnDetailClose;

        #endregion

        #region Private Fields

        private List<Employee> _currentEmployeeList;
        private List<Job> _allJobs;
        private List<Department> _allDepartments;

        private const int _pageSize = 20;
        private bool _templateControlsResolved;

        #endregion

        #region State Properties

        private int CurrentPage
        {
            get
            {
                object val = ViewState["Employees_CurrentPage"];
                if (val == null) return 1;
                int page;
                if (int.TryParse(val.ToString(), out page) && page > 0) return page;
                return 1;
            }
            set { ViewState["Employees_CurrentPage"] = value < 1 ? 1 : value; }
        }

        private string CurrentSortExpression
        {
            get
            {
                object val = ViewState["Employees_SortExpr"];
                return val == null ? "employee_name" : val.ToString();
            }
            set { ViewState["Employees_SortExpr"] = value; }
        }

        private string CurrentSortDirection
        {
            get
            {
                object val = ViewState["Employees_SortDir"];
                return val == null ? "ASC" : val.ToString();
            }
            set { ViewState["Employees_SortDir"] = value; }
        }

        private int EditingEmployeeId
        {
            get
            {
                object val = ViewState["Employees_EditingId"];
                if (val == null) return 0;
                int id;
                if (int.TryParse(val.ToString(), out id)) return id;
                return 0;
            }
            set { ViewState["Employees_EditingId"] = value; }
        }

        private string SearchTerm
        {
            get
            {
                object val = ViewState["Employees_SearchTerm"];
                return val == null ? string.Empty : val.ToString();
            }
            set { ViewState["Employees_SearchTerm"] = value; }
        }

        private string FilterStatus
        {
            get
            {
                object val = ViewState["Employees_FilterStatus"];
                return val == null ? string.Empty : val.ToString();
            }
            set { ViewState["Employees_FilterStatus"] = value; }
        }

        private string FilterJobId
        {
            get
            {
                object val = ViewState["Employees_FilterJobId"];
                return val == null ? string.Empty : val.ToString();
            }
            set { ViewState["Employees_FilterJobId"] = value; }
        }

        private string FilterDepartmentId
        {
            get
            {
                object val = ViewState["Employees_FilterDeptId"];
                return val == null ? string.Empty : val.ToString();
            }
            set { ViewState["Employees_FilterDeptId"] = value; }
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
                    LoadFilterData();
                    LoadEmployeeData();
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
            EnsureContentBoxCreated(cbEmployeesList, "cbEmployeesList");
            EnsureContentBoxCreated(cbEmployeeForm, "cbEmployeeForm");
            EnsureContentBoxCreated(cbEmployeeDetail, "cbEmployeeDetail");

            // Resolve filter controls
            ddlStatusFilter = FindRequiredTemplateControl<DropDownList>(cbFilters, "ddlStatusFilter");
            ddlJobFilter = FindRequiredTemplateControl<DropDownList>(cbFilters, "ddlJobFilter");
            ddlDepartmentFilter = FindRequiredTemplateControl<DropDownList>(cbFilters, "ddlDepartmentFilter");

            // Resolve grid control
            gvEmployees = FindRequiredTemplateControl<GridView>(cbEmployeesList, "gvEmployees");

            // Resolve form controls
            txtSurname = FindRequiredTemplateControl<TextBox>(cbEmployeeForm, "txtSurname");
            txtName = FindRequiredTemplateControl<TextBox>(cbEmployeeForm, "txtName");
            txtPatronym = FindRequiredTemplateControl<TextBox>(cbEmployeeForm, "txtPatronym");
            txtEmployedDate = FindRequiredTemplateControl<TextBox>(cbEmployeeForm, "txtEmployedDate");
            ddlJob = FindRequiredTemplateControl<DropDownList>(cbEmployeeForm, "ddlJob");
            ddlDepartment = FindRequiredTemplateControl<DropDownList>(cbEmployeeForm, "ddlDepartment");
            txtPhone = FindRequiredTemplateControl<TextBox>(cbEmployeeForm, "txtPhone");
            txtEmail = FindRequiredTemplateControl<TextBox>(cbEmployeeForm, "txtEmail");
            ddlStatus = FindRequiredTemplateControl<DropDownList>(cbEmployeeForm, "ddlStatus");
            btnSave = FindRequiredTemplateControl<Button>(cbEmployeeForm, "btnSave");
            btnCancel = FindRequiredTemplateControl<Button>(cbEmployeeForm, "btnCancel");

            // Resolve detail controls
            litDetailSurname = FindRequiredTemplateControl<Literal>(cbEmployeeDetail, "litDetailSurname");
            litDetailName = FindRequiredTemplateControl<Literal>(cbEmployeeDetail, "litDetailName");
            litDetailPatronym = FindRequiredTemplateControl<Literal>(cbEmployeeDetail, "litDetailPatronym");
            litDetailEmployedDate = FindRequiredTemplateControl<Literal>(cbEmployeeDetail, "litDetailEmployedDate");
            litDetailJob = FindRequiredTemplateControl<Literal>(cbEmployeeDetail, "litDetailJob");
            litDetailDepartment = FindRequiredTemplateControl<Literal>(cbEmployeeDetail, "litDetailDepartment");
            litDetailPhone = FindRequiredTemplateControl<Literal>(cbEmployeeDetail, "litDetailPhone");
            litDetailEmail = FindRequiredTemplateControl<Literal>(cbEmployeeDetail, "litDetailEmail");
            litDetailStatus = FindRequiredTemplateControl<Literal>(cbEmployeeDetail, "litDetailStatus");
            litDetailServiceYears = FindRequiredTemplateControl<Literal>(cbEmployeeDetail, "litDetailServiceYears");
            btnDetailClose = FindRequiredTemplateControl<Button>(cbEmployeeDetail, "btnDetailClose");

            _templateControlsResolved = true;
        }

        private static void EnsureContentBoxCreated(Controls.ContentBox contentBox, string controlId)
        {
            if (contentBox == null)
                throw new InvalidOperationException(
                    "Required ContentBox '" + controlId + "' was not created. Check Employees.aspx markup and the ContentBox registration.");
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

        private void LoadFilterData()
        {
            using (EmployeeService service = new EmployeeService())
            {
                _allJobs = service.GetAllJobs();
                _allDepartments = service.GetAllDepartments();
            }

            PopulateJobDropdowns();
            PopulateDepartmentDropdowns();
        }

        private void PopulateJobDropdowns()
        {
            ddlJobFilter.Items.Clear();
            ddlJobFilter.Items.Add(new ListItem("All Jobs", ""));
            if (_allJobs != null)
            {
                foreach (Job job in _allJobs)
                {
                    ddlJobFilter.Items.Add(new ListItem(job.JobTitle, job.JobId.ToString()));
                }
            }

            ddlJob.Items.Clear();
            ddlJob.Items.Add(new ListItem("-- Select Job --", ""));
            if (_allJobs != null)
            {
                foreach (Job job in _allJobs)
                {
                    ddlJob.Items.Add(new ListItem(job.JobTitle, job.JobId.ToString()));
                }
            }
        }

        private void PopulateDepartmentDropdowns()
        {
            ddlDepartmentFilter.Items.Clear();
            ddlDepartmentFilter.Items.Add(new ListItem("All Departments", ""));
            if (_allDepartments != null)
            {
                foreach (Department dept in _allDepartments)
                {
                    ddlDepartmentFilter.Items.Add(new ListItem(dept.DepartmentName, dept.DepartmentId.ToString()));
                }
            }

            ddlDepartment.Items.Clear();
            ddlDepartment.Items.Add(new ListItem("-- No Department --", ""));
            if (_allDepartments != null)
            {
                foreach (Department dept in _allDepartments)
                {
                    ddlDepartment.Items.Add(new ListItem(dept.DepartmentName, dept.DepartmentId.ToString()));
                }
            }
        }

        private void LoadEmployeeData()
        {
            using (EmployeeService service = new EmployeeService())
            {
                _currentEmployeeList = service.GetAllEmployees();
            }

            ApplyFiltersAndSort();
            BindGrid();
        }

        private void LoadStats()
        {
            if (_currentEmployeeList == null || _currentEmployeeList.Count == 0)
            {
                using (EmployeeService service = new EmployeeService())
                {
                    _currentEmployeeList = service.GetAllEmployees();
                }
            }

            litTotalEmployees.Text = _currentEmployeeList.Count.ToString();
            litActiveEmployees.Text = CountByStatus(_currentEmployeeList, EmployeeStatus.Working).ToString();
            litVacationEmployees.Text = CountByStatus(_currentEmployeeList, EmployeeStatus.OnVacation).ToString();
            litSickEmployees.Text = CountByStatus(_currentEmployeeList, EmployeeStatus.OnSickLeave).ToString();
            litDismissedEmployees.Text = CountByStatus(_currentEmployeeList, EmployeeStatus.Dismissed).ToString();
        }

        private int CountByStatus(List<Employee> employees, string status)
        {
            int count = 0;
            foreach (Employee emp in employees)
            {
                if (emp.Status == status) count++;
            }
            return count;
        }

        private void ApplyFiltersAndSort()
        {
            if (_currentEmployeeList == null) _currentEmployeeList = new List<Employee>();

            // Apply search filter
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                string term = SearchTerm.ToLowerInvariant();
                List<Employee> filtered = new List<Employee>();
                foreach (Employee emp in _currentEmployeeList)
                {
                    string empName = (emp.EmployeeName ?? string.Empty).ToLowerInvariant();
                    string phone = (emp.Phone ?? string.Empty).ToLowerInvariant();
                    string email = (emp.Email ?? string.Empty).ToLowerInvariant();
                    if (empName.Contains(term) || phone.Contains(term) || email.Contains(term))
                    {
                        filtered.Add(emp);
                    }
                }
                _currentEmployeeList = filtered;
            }

            // Apply status filter
            if (!string.IsNullOrEmpty(FilterStatus))
            {
                List<Employee> filtered = new List<Employee>();
                foreach (Employee emp in _currentEmployeeList)
                {
                    if (emp.Status == FilterStatus) filtered.Add(emp);
                }
                _currentEmployeeList = filtered;
            }

            // Apply job filter
            if (!string.IsNullOrEmpty(FilterJobId))
            {
                int jobId;
                if (int.TryParse(FilterJobId, out jobId))
                {
                    List<Employee> filtered = new List<Employee>();
                    foreach (Employee emp in _currentEmployeeList)
                    {
                        if (emp.JobId == jobId) filtered.Add(emp);
                    }
                    _currentEmployeeList = filtered;
                }
            }

            // Apply department filter
            if (!string.IsNullOrEmpty(FilterDepartmentId))
            {
                int deptId;
                if (int.TryParse(FilterDepartmentId, out deptId))
                {
                    List<Employee> filtered = new List<Employee>();
                    foreach (Employee emp in _currentEmployeeList)
                    {
                        if (emp.DepartmentId.HasValue && emp.DepartmentId.Value == deptId)
                            filtered.Add(emp);
                    }
                    _currentEmployeeList = filtered;
                }
            }

            // Apply sorting
            ApplySorting();
        }

        private void ApplySorting()
        {
            if (_currentEmployeeList == null || _currentEmployeeList.Count <= 1) return;

            string sortExpr = CurrentSortExpression;
            bool ascending = CurrentSortDirection == "ASC";
            Comparison<Employee> comparison = null;

            switch (sortExpr)
            {
                case "employee_id":
                    comparison = delegate(Employee a, Employee b) { return a.EmployeeId.CompareTo(b.EmployeeId); };
                    break;
                case "employee_name":
                    comparison = delegate(Employee a, Employee b) { return string.Compare(a.EmployeeName ?? "", b.EmployeeName ?? "", StringComparison.Ordinal); };
                    break;
                case "job_title":
                    comparison = delegate(Employee a, Employee b) { return string.Compare(a.JobTitle ?? "", b.JobTitle ?? "", StringComparison.Ordinal); };
                    break;
                case "department_name":
                    comparison = delegate(Employee a, Employee b) { return string.Compare(a.DepartmentName ?? "", b.DepartmentName ?? "", StringComparison.Ordinal); };
                    break;
                case "service_years":
                    comparison = delegate(Employee a, Employee b) { return a.ServiceYears.CompareTo(b.ServiceYears); };
                    break;
                case "status":
                    comparison = delegate(Employee a, Employee b) { return string.Compare(a.Status ?? "", b.Status ?? "", StringComparison.Ordinal); };
                    break;
                default:
                    comparison = delegate(Employee a, Employee b) { return string.Compare(a.EmployeeName ?? "", b.EmployeeName ?? "", StringComparison.Ordinal); };
                    break;
            }

            _currentEmployeeList.Sort(comparison);
            if (!ascending) _currentEmployeeList.Reverse();
        }

        private void BindGrid()
        {
            int totalCount = _currentEmployeeList != null ? _currentEmployeeList.Count : 0;
            int totalPages = (int)Math.Ceiling((double)totalCount / _pageSize);
            if (totalPages == 0) totalPages = 1;

            if (CurrentPage > totalPages) CurrentPage = totalPages;

            gvEmployees.PageSize = _pageSize;
            gvEmployees.PageIndex = CurrentPage - 1;
            gvEmployees.DataSource = _currentEmployeeList;
            gvEmployees.DataBind();

            RenderPagination(totalPages, totalCount);
        }

        private void RenderPagination(int totalPages, int totalCount)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<span class='text-small'>Total: ").Append(totalCount).Append(" records");
            if (totalPages > 1)
                sb.Append(" | Page ").Append(CurrentPage).Append(" of ").Append(totalPages);
            sb.Append("</span>");
            litPagination.Text = sb.ToString();
        }

        #endregion

        #region Grid Events

        protected void gvEmployees_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CurrentPage = e.NewPageIndex + 1;
            LoadEmployeeData();
        }

        protected void gvEmployees_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression == CurrentSortExpression)
            {
                CurrentSortDirection = CurrentSortDirection == "ASC" ? "DESC" : "ASC";
            }
            else
            {
                CurrentSortExpression = e.SortExpression;
                CurrentSortDirection = "ASC";
            }
            CurrentPage = 1;
            LoadEmployeeData();
        }

        protected void gvEmployees_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "View" || e.CommandName == "EditEmp" || e.CommandName == "DeleteEmp")
            {
                int employeeId;
                if (int.TryParse(e.CommandArgument.ToString(), out employeeId))
                {
                    switch (e.CommandName)
                    {
                        case "View":
                            ShowEmployeeDetail(employeeId);
                            break;
                        case "EditEmp":
                            if (!RequireWritePermission("administrator")) return;
                            ShowEditForm(employeeId);
                            break;
                        case "DeleteEmp":
                            if (!RequireWritePermission("administrator")) return;
                            DeleteEmployeeRecord(employeeId);
                            break;
                    }
                }
            }
        }

        protected void gvEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button deleteButton = e.Row.FindControl("btnDelete") as Button;
                if (deleteButton != null)
                {
                    deleteButton.OnClientClick = "return confirm(" +
                        HttpUtility.JavaScriptStringEncode(Localization.Get("Common_DeleteEmployeeConfirm"), true) + ");";
                }

                Employee emp = (Employee)e.Row.DataItem;
                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                if (litStatus != null)
                {
                    string cssClass;
                    switch (emp.Status)
                    {
                        case EmployeeStatus.Working:
                            cssClass = "status-working";
                            break;
                        case EmployeeStatus.OnVacation:
                            cssClass = "status-vacation";
                            break;
                        case EmployeeStatus.OnSickLeave:
                            cssClass = "status-sickleave";
                            break;
                        case EmployeeStatus.Dismissed:
                            cssClass = "status-dismissed";
                            break;
                        default:
                            cssClass = "status-working";
                            break;
                    }
                    litStatus.Text = "<span class='" + cssClass + "'>" + emp.Status + "</span>";
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

        protected void btnAddEmployee_Click(object sender, EventArgs e)
        {
            RequireRole("administrator");
            EditingEmployeeId = 0;
            ClearForm();
            cbEmployeeForm.HeaderText = "Add New Employee";
            cbEmployeeForm.Visible = true;
            cbEmployeeDetail.Visible = false;
            HideMessages();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            SearchTerm = string.Empty;
            FilterStatus = string.Empty;
            FilterJobId = string.Empty;
            FilterDepartmentId = string.Empty;
            CurrentSortExpression = "employee_name";
            CurrentSortDirection = "ASC";
            txtSearch.Text = string.Empty;

            ddlStatusFilter.SelectedIndex = 0;
            ddlJobFilter.SelectedIndex = 0;
            ddlDepartmentFilter.SelectedIndex = 0;

            LoadFilterData();
            LoadEmployeeData();
            LoadStats();
            cbEmployeeForm.Visible = false;
            cbEmployeeDetail.Visible = false;

            ShowSuccess("Data refreshed successfully.");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchTerm = txtSearch.Text.Trim();
            CurrentPage = 1;
            LoadEmployeeData();
        }

        #endregion

        #region Filter Events

        protected void ddlStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterStatus = ddlStatusFilter.SelectedValue;
            CurrentPage = 1;
            LoadEmployeeData();
        }

        protected void ddlJobFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterJobId = ddlJobFilter.SelectedValue;
            CurrentPage = 1;
            LoadEmployeeData();
        }

        protected void ddlDepartmentFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterDepartmentId = ddlDepartmentFilter.SelectedValue;
            CurrentPage = 1;
            LoadEmployeeData();
        }

        #endregion

        #region Detail View

        private void ShowEmployeeDetail(int employeeId)
        {
            try
            {
                using (EmployeeService service = new EmployeeService())
                {
                    Employee emp = service.GetEmployeeById(employeeId);
                    if (emp != null)
                    {
                        StringBuilder sb = new StringBuilder();

                        sb.Append("<div class='detail-section'>");
                        sb.Append("<div class='detail-row'><span class='detail-label'>Employee ID:</span><span class='detail-value'>").Append(emp.EmployeeId).Append("</span></div>");
                        sb.Append("<div class='detail-row'><span class='detail-label'>Full Name:</span><span class='detail-value'>").Append(Server.HtmlEncode(emp.EmployeeName ?? "")).Append("</span></div>");
                        sb.Append("<div class='detail-row'><span class='detail-label'>Surname:</span><span class='detail-value'>").Append(Server.HtmlEncode(emp.Surname ?? "")).Append("</span></div>");
                        sb.Append("<div class='detail-row'><span class='detail-label'>Name:</span><span class='detail-value'>").Append(Server.HtmlEncode(emp.Name ?? "")).Append("</span></div>");
                        sb.Append("<div class='detail-row'><span class='detail-label'>Patronymic:</span><span class='detail-value'>").Append(Server.HtmlEncode(emp.Patronym ?? "—")).Append("</span></div>");
                        sb.Append("<div class='detail-row'><span class='detail-label'>Employed Date:</span><span class='detail-value'>").Append(emp.EmployedDate.ToString("dd.MM.yyyy")).Append("</span></div>");
                        sb.Append("<div class='detail-row'><span class='detail-label'>Job Title:</span><span class='detail-value'>").Append(Server.HtmlEncode(emp.JobTitle ?? "—")).Append("</span></div>");
                        sb.Append("<div class='detail-row'><span class='detail-label'>Department:</span><span class='detail-value'>").Append(Server.HtmlEncode(emp.DepartmentName ?? "—")).Append("</span></div>");
                        sb.Append("<div class='detail-row'><span class='detail-label'>Phone:</span><span class='detail-value'>").Append(Server.HtmlEncode(emp.Phone ?? "—")).Append("</span></div>");
                        sb.Append("<div class='detail-row'><span class='detail-label'>Email:</span><span class='detail-value'>").Append(Server.HtmlEncode(emp.Email ?? "—")).Append("</span></div>");
                        sb.Append("<div class='detail-row'><span class='detail-label'>Status:</span><span class='detail-value'>").Append(Server.HtmlEncode(emp.Status ?? "")).Append("</span></div>");
                        sb.Append("<div class='detail-row'><span class='detail-label'>Service Years:</span><span class='detail-value'>").Append(emp.ServiceYears).Append("</span></div>");
                        sb.Append("</div>");

                        litDetailSurname.Text = sb.ToString();
                        cbEmployeeDetail.Visible = true;
                        cbEmployeeForm.Visible = false;
                        HideMessages();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Error loading employee details: " + ex.Message);
            }
        }

        protected void btnDetailClose_Click(object sender, EventArgs e)
        {
            cbEmployeeDetail.Visible = false;
        }

        #endregion

        #region Edit Form

        private void ShowEditForm(int employeeId)
        {
            try
            {
                using (EmployeeService service = new EmployeeService())
                {
                    Employee emp = service.GetEmployeeById(employeeId);
                    if (emp != null)
                    {
                        EditingEmployeeId = emp.EmployeeId;
                        txtSurname.Text = emp.Surname;
                        txtName.Text = emp.Name;
                        txtPatronym.Text = emp.Patronym ?? "";
                        txtEmployedDate.Text = emp.EmployedDate.ToString("dd.MM.yyyy");
                        txtPhone.Text = emp.Phone ?? "";
                        txtEmail.Text = emp.Email ?? "";

                        if (ddlJob.Items.FindByValue(emp.JobId.ToString()) != null)
                            ddlJob.SelectedValue = emp.JobId.ToString();

                        if (emp.DepartmentId.HasValue && ddlDepartment.Items.FindByValue(emp.DepartmentId.Value.ToString()) != null)
                            ddlDepartment.SelectedValue = emp.DepartmentId.Value.ToString();
                        else
                            ddlDepartment.SelectedIndex = 0;

                        if (ddlStatus.Items.FindByValue(emp.Status) != null)
                            ddlStatus.SelectedValue = emp.Status;

                        cbEmployeeForm.HeaderText = "Edit Employee";
                        cbEmployeeForm.Visible = true;
                        cbEmployeeDetail.Visible = false;
                        HideMessages();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Error loading employee for edit: " + ex.Message);
            }
        }

        private void ClearForm()
        {
            txtSurname.Text = string.Empty;
            txtName.Text = string.Empty;
            txtPatronym.Text = string.Empty;
            txtEmployedDate.Text = DateTime.Today.ToString("dd.MM.yyyy");
            txtPhone.Text = string.Empty;
            txtEmail.Text = string.Empty;
            ddlJob.SelectedIndex = 0;
            ddlDepartment.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            RequireRole("administrator");

            try
            {
                Employee emp = new Employee();
                emp.EmployeeId = EditingEmployeeId;
                emp.Surname = txtSurname.Text.Trim();
                emp.Name = txtName.Text.Trim();
                emp.Patronym = string.IsNullOrEmpty(txtPatronym.Text.Trim()) ? null : txtPatronym.Text.Trim();

                DateTime employedDate;
                if (DateTime.TryParseExact(txtEmployedDate.Text.Trim(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out employedDate))
                {
                    emp.EmployedDate = employedDate;
                }
                else
                {
                    ShowError("Invalid date format. Use dd.MM.yyyy");
                    return;
                }

                int jobId;
                if (int.TryParse(ddlJob.SelectedValue, out jobId))
                {
                    emp.JobId = jobId;
                }

                if (!string.IsNullOrEmpty(ddlDepartment.SelectedValue))
                {
                    int deptId;
                    if (int.TryParse(ddlDepartment.SelectedValue, out deptId))
                    {
                        emp.DepartmentId = deptId;
                    }
                }

                emp.Phone = string.IsNullOrEmpty(txtPhone.Text.Trim()) ? null : txtPhone.Text.Trim();
                emp.Email = string.IsNullOrEmpty(txtEmail.Text.Trim()) ? null : txtEmail.Text.Trim();
                emp.Status = ddlStatus.SelectedValue;

                using (EmployeeService service = new EmployeeService())
                {
                    if (EditingEmployeeId == 0)
                    {
                        int newId = service.CreateEmployee(emp);
                        ShowSuccess("Employee created successfully. ID: " + newId);
                    }
                    else
                    {
                        bool success = service.UpdateEmployee(emp);
                        if (success)
                            ShowSuccess("Employee updated successfully.");
                        else
                            ShowError("Failed to update employee.");
                    }
                }

                cbEmployeeForm.Visible = false;
                CurrentPage = 1;
                LoadEmployeeData();
                LoadStats();
            }
            catch (ServiceException svcEx)
            {
                ShowError(svcEx.Message + (svcEx.InnerException != null ? " (" + svcEx.InnerException.Message + ")" : ""));
            }
            catch (Exception ex)
            {
                ShowError("Error saving employee: " + ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            cbEmployeeForm.Visible = false;
            ClearForm();
            HideMessages();
        }

        #endregion

        #region Delete

        private void DeleteEmployeeRecord(int employeeId)
        {
            RequireRole("administrator");
            try
            {
                using (EmployeeService service = new EmployeeService())
                {
                    bool success = service.DeleteEmployee(employeeId);
                    if (success)
                    {
                        ShowSuccess("Employee deleted successfully.");
                        LoadEmployeeData();
                        LoadStats();
                    }
                    else
                    {
                        ShowError("Failed to delete employee.");
                    }
                }
            }
            catch (ServiceException svcEx)
            {
                ShowError(svcEx.Message);
            }
            catch (Exception ex)
            {
                ShowError("Error deleting employee: " + ex.Message);
            }
        }

        #endregion

        #region Error Handling

        private void HandleDatabaseError(SqlException sqlEx)
        {
            string errorMsg;
            switch (sqlEx.Number)
            {
                case 53:
                    errorMsg = "Database server is not available. Please check your connection.";
                    break;
                case 18456:
                    errorMsg = "Database login failed. Please check credentials.";
                    break;
                case 208:
                    errorMsg = "Database table not found. Please ensure the database schema is deployed.";
                    break;
                default:
                    errorMsg = "Database error (Code " + sqlEx.Number + "): " + sqlEx.Message;
                    break;
            }
            ShowError(errorMsg);
            System.Diagnostics.Debug.WriteLine("Employees SQL Error: " + sqlEx.ToString());
        }

        private void HandleServiceError(ServiceException svcEx)
        {
            string msg = svcEx.Message;
            if (svcEx.InnerException != null)
            {
                msg += " (" + svcEx.InnerException.Message + ")";
            }
            ShowError(msg);
            System.Diagnostics.Debug.WriteLine("Employees Service Error: " + svcEx.ToString());
        }

        private void HandleGenericError(Exception ex)
        {
            ShowError("Unexpected error: " + ex.Message);
            System.Diagnostics.Debug.WriteLine("Employees Generic Error: " + ex.ToString());
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
