using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using BRU.WEBFORMS.ASPNET.APP;
using BRU.WEBFORMS.ASPNET.APP.Models;
using BRU.WEBFORMS.ASPNET.APP.Services;

namespace BRU.WEBFORMS.ASPNET.APP.Personnel
{
    public partial class Departments : SecurePage
    {
        private List<Department> _departments;

        protected override string[] RequiredPermissions { get { return new[] { "employee.read" }; } }

        protected global::System.Web.UI.WebControls.Label lblError;
        protected global::System.Web.UI.WebControls.Label lblSuccess;
        protected global::System.Web.UI.WebControls.TextBox txtSearch;
        protected global::System.Web.UI.WebControls.DropDownList ddlStatus;
        protected global::System.Web.UI.WebControls.Button btnApply;
        protected global::System.Web.UI.WebControls.Button btnNew;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbDepartmentEditor;
        protected global::System.Web.UI.WebControls.Panel pnlEditor;
        protected global::System.Web.UI.WebControls.HiddenField hidDepartmentId;
        protected global::System.Web.UI.WebControls.TextBox txtName;
        protected global::System.Web.UI.WebControls.TextBox txtCode;
        protected global::System.Web.UI.WebControls.TextBox txtDescription;
        protected global::System.Web.UI.WebControls.Button btnSave;
        protected global::System.Web.UI.WebControls.Button btnCancel;
        protected global::System.Web.UI.WebControls.GridView gvDepartments;

        protected void Page_Load(object sender, EventArgs e)
        {
            bool canManage = AuthContext.IsInRole("administrator");
            btnNew.Visible = canManage;
            cbDepartmentEditor.Visible = canManage && pnlEditor.Visible;
            if (!IsPostBack)
                BindDepartments();
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            gvDepartments.PageIndex = 0;
            BindDepartments();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                RequireRole("administrator");
                ClearEditor();
                pnlEditor.Visible = true;
                cbDepartmentEditor.Visible = true;
            }
            catch (UnauthorizedAccessException ex)
            {
                ShowError(ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                RequireRole("administrator");
                int departmentId;
                if (!int.TryParse(hidDepartmentId.Value, out departmentId))
                    throw new ServiceException("Invalid department selection.");

                Department department = new Department
                {
                    DepartmentId = departmentId,
                    DepartmentName = txtName.Text,
                    DepartmentCode = txtCode.Text,
                    Description = txtDescription.Text
                };
                using (EmployeeService service = new EmployeeService())
                {
                    if (departmentId == 0)
                        service.CreateDepartment(department);
                    else
                        service.UpdateDepartment(department);
                }

                ClearEditor();
                pnlEditor.Visible = false;
                cbDepartmentEditor.Visible = false;
                gvDepartments.PageIndex = 0;
                BindDepartments();
                ShowSuccess("Department saved.");
            }
            catch (ServiceException ex)
            {
                ShowError(ex.Message);
            }
            catch (SqlException ex)
            {
                HandleDatabaseError(ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearEditor();
            pnlEditor.Visible = false;
            cbDepartmentEditor.Visible = false;
        }

        protected void gvDepartments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDepartments.PageIndex = e.NewPageIndex;
            BindDepartments();
        }

        protected void gvDepartments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int departmentId;
                if (!int.TryParse(Convert.ToString(e.CommandArgument), out departmentId))
                    throw new ServiceException("Invalid department selection.");
                Department department = GetDepartments().FirstOrDefault(item => item.DepartmentId == departmentId);
                if (department == null)
                    throw new ServiceException("Department not found.");

                if (e.CommandName == "EditDepartment")
                {
                    RequireRole("administrator");
                    hidDepartmentId.Value = department.DepartmentId.ToString();
                    txtName.Text = department.DepartmentName;
                    txtCode.Text = department.DepartmentCode;
                    txtDescription.Text = department.Description;
                    pnlEditor.Visible = true;
                    cbDepartmentEditor.Visible = true;
                }
                else if (e.CommandName == "ToggleDepartment")
                {
                    RequireRole("administrator");
                    using (EmployeeService service = new EmployeeService())
                        service.SetDepartmentActive(department.DepartmentId, !department.IsActive);
                    BindDepartments();
                    ShowSuccess(department.IsActive ? "Department deactivated." : "Department activated.");
                }
            }
            catch (ServiceException ex)
            {
                ShowError(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                ShowError(ex.Message);
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

        protected void gvDepartments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            Department department = (Department)e.Row.DataItem;
            LinkButton edit = e.Row.FindControl("btnEdit") as LinkButton;
            LinkButton toggle = e.Row.FindControl("btnToggle") as LinkButton;
            bool canManage = AuthContext.IsInRole("administrator");
            if (edit != null) edit.Visible = canManage;
            if (toggle != null)
            {
                toggle.Visible = canManage;
                toggle.Text = department.IsActive ? "Deactivate" : "Activate";
            }
        }

        private List<Department> GetDepartments()
        {
            if (_departments == null)
            {
                using (EmployeeService service = new EmployeeService())
                    _departments = service.GetDepartmentsForManagement();
            }
            return _departments;
        }

        private void BindDepartments()
        {
            try
            {
                IEnumerable<Department> departments = GetDepartments();
                string search = (txtSearch.Text ?? string.Empty).Trim();
                if (search.Length > 0)
                    departments = departments.Where(item => item.DepartmentName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        item.DepartmentCode.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);
                if (ddlStatus.SelectedValue == "1") departments = departments.Where(item => item.IsActive);
                else if (ddlStatus.SelectedValue == "0") departments = departments.Where(item => !item.IsActive);
                gvDepartments.DataSource = departments.ToList();
                gvDepartments.DataBind();
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

        private void ClearEditor()
        {
            hidDepartmentId.Value = "0";
            txtName.Text = string.Empty;
            txtCode.Text = string.Empty;
            txtDescription.Text = string.Empty;
        }

        private void ShowError(string message)
        {
            lblError.Text = Server.HtmlEncode(message);
            lblError.Visible = true;
            lblSuccess.Visible = false;
        }

        private void ShowSuccess(string message)
        {
            lblSuccess.Text = Server.HtmlEncode(message);
            lblSuccess.Visible = true;
            lblError.Visible = false;
        }
    }
}
