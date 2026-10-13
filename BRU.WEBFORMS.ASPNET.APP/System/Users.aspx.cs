using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using BRU.WEBFORMS.ASPNET.APP.Models;
using BRU.WEBFORMS.ASPNET.APP.Services;

namespace BRU.WEBFORMS.ASPNET.APP.SystemPages
{
    public partial class Users : SecurePage
    {
        protected override string[] RequiredRoles { get { return new[] { "administrator" }; } }

        protected global::System.Web.UI.WebControls.Panel pnlError;
        protected global::System.Web.UI.WebControls.Literal litError;
        protected global::System.Web.UI.WebControls.Panel pnlSuccess;
        protected global::System.Web.UI.WebControls.Literal litSuccess;
        protected global::System.Web.UI.WebControls.Literal litTotalUsers;
        protected global::System.Web.UI.WebControls.Literal litActiveUsers;
        protected global::System.Web.UI.WebControls.Literal litInactiveUsers;
        protected global::System.Web.UI.WebControls.Literal litAdminUsers;
        protected global::System.Web.UI.WebControls.TextBox txtSearch;
        protected global::System.Web.UI.WebControls.DropDownList ddlStatusFilter;
        protected global::System.Web.UI.WebControls.Button btnApplyFilters;
        protected global::System.Web.UI.WebControls.Button btnNewUser;
        protected global::System.Web.UI.WebControls.Panel pnlEditor;
        protected global::System.Web.UI.WebControls.Literal litEditorTitle;
        protected global::System.Web.UI.WebControls.HiddenField hfUserId;
        protected global::System.Web.UI.WebControls.TextBox txtLogin;
        protected global::System.Web.UI.WebControls.DropDownList ddlEmployee;
        protected global::System.Web.UI.WebControls.TextBox txtEmail;
        protected global::System.Web.UI.WebControls.TextBox txtPhone;
        protected global::System.Web.UI.WebControls.TextBox txtPassword;
        protected global::System.Web.UI.WebControls.TextBox txtConfirmPassword;
        protected global::System.Web.UI.WebControls.Literal litPasswordOptional;
        protected global::System.Web.UI.WebControls.CheckBox chkUserActive;
        protected global::System.Web.UI.WebControls.CheckBoxList cblUserRoles;
        protected global::System.Web.UI.WebControls.Panel pnlInactiveRoleNote;
        protected global::System.Web.UI.WebControls.Literal litInactiveRoleNote;
        protected global::System.Web.UI.WebControls.Button btnSaveUser;
        protected global::System.Web.UI.WebControls.Button btnCancelEdit;
        protected global::System.Web.UI.WebControls.GridView gvUsers;

        private int CurrentActorId { get { return AuthContext.CurrentUserId ?? 0; } }

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                BindRoleChoices();
                BindEmployeeChoices();
            }
            catch (Exception ex) { ShowError(GetUserFacingError(ex)); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) BindUsers();
        }

        private void BindRoleChoices()
        {
            cblUserRoles.Items.Clear();
            using (AuthService service = new AuthService())
                foreach (Role role in service.GetAllRoles().Where(r => r.IsActive).OrderBy(r => r.RoleName))
                {
                    string label = role.RoleName;
                    if (!string.IsNullOrWhiteSpace(role.Description)) label += " — " + role.Description;
                    cblUserRoles.Items.Add(new ListItem(label, role.RoleId.ToString()));
                }
        }

        private void BindEmployeeChoices()
        {
            string previousValue = ddlEmployee.SelectedValue;
            ddlEmployee.Items.Clear();
            ddlEmployee.Items.Add(new ListItem(Localization.Get("AdminUsers_NoEmployee"), string.Empty));
            using (EmployeeService service = new EmployeeService())
                foreach (Employee employee in service.GetAllEmployees().OrderBy(e => e.EmployeeName))
                {
                    string name = string.IsNullOrWhiteSpace(employee.EmployeeName) ? (employee.Surname + " " + employee.Name).Trim() : employee.EmployeeName;
                    string label = name;
                    if (!string.IsNullOrWhiteSpace(employee.JobTitle)) label += " — " + employee.JobTitle;
                    if (!string.IsNullOrWhiteSpace(employee.Status)) label += " (" + employee.Status + ")";
                    ddlEmployee.Items.Add(new ListItem(label, employee.EmployeeId.ToString()));
                }
            if (!string.IsNullOrEmpty(previousValue) && ddlEmployee.Items.FindByValue(previousValue) != null)
                ddlEmployee.SelectedValue = previousValue;
        }

        private void BindUsers()
        {
            try
            {
                List<User> users;
                using (AuthService service = new AuthService()) users = service.GetAllUsers() ?? new List<User>();
                litTotalUsers.Text = users.Count.ToString();
                litActiveUsers.Text = users.Count(u => u.IsActive).ToString();
                litInactiveUsers.Text = users.Count(u => !u.IsActive).ToString();
                litAdminUsers.Text = users.Count(u => u.IsActive &&
                    SplitNames(u.ActiveRoleNames).Any(n => string.Equals(n, "administrator", StringComparison.OrdinalIgnoreCase))).ToString();

                IEnumerable<User> filtered = users;
                if (ddlStatusFilter.SelectedValue == "active") filtered = filtered.Where(u => u.IsActive);
                else if (ddlStatusFilter.SelectedValue == "inactive") filtered = filtered.Where(u => !u.IsActive);

                string search = (txtSearch.Text ?? string.Empty).Trim();
                if (search.Length > 0)
                    filtered = filtered.Where(u =>
                        Contains(u.Login, search) || Contains(u.EmployeeName, search) ||
                        Contains(u.Email, search) || Contains(u.PhoneNumber, search) ||
                        Contains(u.ActiveRoleNames, search) || Contains(u.InactiveRoleNames, search));

                gvUsers.DataSource = filtered.ToList();
                gvUsers.DataBind();
            }
            catch (Exception ex) { ShowError(GetUserFacingError(ex)); }
        }

        protected void btnApplyFilters_Click(object sender, EventArgs e)
        {
            gvUsers.PageIndex = 0;
            BindUsers();
        }

        protected void btnNewUser_Click(object sender, EventArgs e)
        {
            ClearMessages();
            hfUserId.Value = "0";
            litEditorTitle.Text = HttpUtility.HtmlEncode(Localization.Get("AdminUsers_New"));
            litPasswordOptional.Text = HttpUtility.HtmlEncode(Localization.Get("AdminUsers_PasswordRequired"));
            txtLogin.ReadOnly = false;
            txtLogin.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;
            ddlEmployee.SelectedIndex = 0;
            chkUserActive.Checked = true;
            chkUserActive.Enabled = true;
            foreach (ListItem item in cblUserRoles.Items) item.Selected = false;
            pnlInactiveRoleNote.Visible = false;
            pnlEditor.Visible = true;
        }

        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            pnlEditor.Visible = false;
            ClearMessages();
        }

        protected void btnSaveUser_Click(object sender, EventArgs e)
        {
            ClearMessages();
            int userId;
            if (!int.TryParse(hfUserId.Value, out userId) || userId < 0)
            {
                ShowError(Localization.Get("AdminUsers_InvalidUser"));
                return;
            }

            string password = txtPassword.Text ?? string.Empty;
            string confirmation = txtConfirmPassword.Text ?? string.Empty;
            bool isNew = userId == 0;
            if (isNew && password.Length == 0) { ShowError(Localization.Get("AdminUsers_PasswordRequired")); return; }
            if ((password.Length > 0 || confirmation.Length > 0) && !string.Equals(password, confirmation, StringComparison.Ordinal))
            {
                ShowError(Localization.Get("AdminUsers_PasswordMismatch"));
                return;
            }

            try
            {
                using (AuthService service = new AuthService())
                {
                    User user = isNew ? new User() : service.GetUserById(userId);
                    if (user == null) { ShowError(Localization.Get("AdminUsers_InvalidUser")); return; }
                    if (isNew) user.Login = (txtLogin.Text ?? string.Empty).Trim();
                    user.Email = (txtEmail.Text ?? string.Empty).Trim();
                    user.PhoneNumber = (txtPhone.Text ?? string.Empty).Trim();
                    int employeeId;
                    user.EmployeeId = int.TryParse(ddlEmployee.SelectedValue, out employeeId) && employeeId > 0 ? (int?)employeeId : null;
                    user.IsActive = chkUserActive.Checked;

                    List<int> roleIds = cblUserRoles.Items.Cast<ListItem>().Where(i => i.Selected)
                        .Select(i => Convert.ToInt32(i.Value)).Distinct().ToList();
                    if (!isNew)
                    {
                        foreach (UserRole assignment in service.GetUserRoleAssignments(userId))
                            if (!assignment.IsRoleActive && !roleIds.Contains(assignment.RoleId)) roleIds.Add(assignment.RoleId);
                    }

                    service.SaveUserAdministration(user, password.Length == 0 ? null : password, roleIds, AuthContext.CurrentUsername, CurrentActorId);
                    LogInformation((isNew ? "Created" : "Updated") + " application user '" + user.Login + "' by administrator '" + AuthContext.CurrentUsername + "'.");
                }

                pnlEditor.Visible = false;
                ShowSuccess(Localization.Get(isNew ? "AdminUsers_CreatedSuccess" : (password.Length > 0 ? "AdminUsers_PasswordSaved" : "AdminUsers_SavedSuccess")));
                BindUsers();
            }
            catch (Exception ex) { ShowError(GetUserFacingError(ex)); }
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e == null || e.CommandArgument == null) return;
            int userId;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out userId) || userId <= 0)
            {
                ShowError(Localization.Get("AdminUsers_InvalidUser"));
                return;
            }

            try
            {
                if (e.CommandName == "EditUser") { OpenUserEditor(userId); return; }
                if (e.CommandName == "ToggleActive")
                {
                    if (userId == CurrentActorId) { ShowError(Localization.Get("AdminUsers_CannotDisableSelf")); return; }
                    using (AuthService service = new AuthService())
                    {
                        User user = service.GetUserById(userId);
                        if (user == null) { ShowError(Localization.Get("AdminUsers_InvalidUser")); return; }
                        user.IsActive = !user.IsActive;
                        List<int> roleIds = service.GetUserRoleAssignments(userId).Select(a => a.RoleId).Distinct().ToList();
                        service.SaveUserAdministration(user, null, roleIds, AuthContext.CurrentUsername, CurrentActorId);
                        LogInformation((user.IsActive ? "Activated" : "Deactivated") + " application user '" + user.Login + "' by administrator '" + AuthContext.CurrentUsername + "'.");
                        ShowSuccess(Localization.Get(user.IsActive ? "AdminUsers_ActivatedSuccess" : "AdminUsers_DeactivatedSuccess"));
                    }
                    BindUsers();
                }
            }
            catch (Exception ex) { ShowError(GetUserFacingError(ex)); }
        }

        private void OpenUserEditor(int userId)
        {
            using (AuthService service = new AuthService())
            {
                User user = service.GetUserById(userId);
                if (user == null) throw new InvalidOperationException(Localization.Get("AdminUsers_InvalidUser"));
                hfUserId.Value = user.UserId.ToString();
                litEditorTitle.Text = HttpUtility.HtmlEncode(Localization.Get("AdminUsers_Edit") + " — " + user.Login);
                litPasswordOptional.Text = HttpUtility.HtmlEncode(Localization.Get("AdminUsers_PasswordOptional"));
                txtLogin.Text = user.Login;
                txtLogin.ReadOnly = true;
                txtEmail.Text = user.Email ?? string.Empty;
                txtPhone.Text = user.PhoneNumber ?? string.Empty;
                txtPassword.Text = string.Empty;
                txtConfirmPassword.Text = string.Empty;
                chkUserActive.Checked = user.IsActive;
                chkUserActive.Enabled = user.UserId != CurrentActorId;
                string employeeValue = user.EmployeeId.HasValue ? user.EmployeeId.Value.ToString() : string.Empty;
                if (ddlEmployee.Items.FindByValue(employeeValue) != null) ddlEmployee.SelectedValue = employeeValue;

                List<UserRole> assignments = service.GetUserRoleAssignments(userId);
                HashSet<int> activeIds = new HashSet<int>(assignments.Where(a => a.IsRoleActive).Select(a => a.RoleId));
                foreach (ListItem item in cblUserRoles.Items) item.Selected = activeIds.Contains(Convert.ToInt32(item.Value));
                List<string> inactiveNames = assignments.Where(a => !a.IsRoleActive).Select(a => a.RoleName).Distinct().OrderBy(n => n).ToList();
                pnlInactiveRoleNote.Visible = inactiveNames.Count > 0;
                litInactiveRoleNote.Text = inactiveNames.Count > 0
                    ? HttpUtility.HtmlEncode(Localization.Get("AdminUsers_InactiveRolesPreserved") + ": " + string.Join(", ", inactiveNames))
                    : string.Empty;
                pnlEditor.Visible = true;
                ClearMessages();
            }
        }

        protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            User user = e.Row.DataItem as User;
            if (user == null) return;

            Label status = e.Row.FindControl("lblRowStatus") as Label;
            if (status != null)
            {
                status.Text = Localization.Get(user.IsActive ? "AdminUsers_Active" : "AdminUsers_Inactive");
                status.CssClass = "status-pill " + (user.IsActive ? "status-active" : "status-inactive");
            }

            Literal roles = e.Row.FindControl("litRowRoles") as Literal;
            if (roles != null) roles.Text = FormatRoleChips(user);

            Button toggle = e.Row.FindControl("btnToggleActive") as Button;
            if (toggle != null)
            {
                toggle.Text = Localization.Get(user.IsActive ? "AdminUsers_Deactivate" : "AdminUsers_Activate");
                toggle.Enabled = user.UserId != CurrentActorId;
                string key = user.IsActive ? "AdminUsers_DeactivateConfirm" : "AdminUsers_ActivateConfirm";
                toggle.OnClientClick = "return confirm('" + HttpUtility.JavaScriptStringEncode(Localization.Get(key)) + "');";
                if (user.UserId == CurrentActorId) toggle.ToolTip = Localization.Get("AdminUsers_CannotDisableSelf");
            }
        }

        protected void gvUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsers.PageIndex = e.NewPageIndex;
            BindUsers();
        }

        private string FormatRoleChips(User user)
        {
            StringBuilder html = new StringBuilder();
            List<string> active = SplitNames(user.ActiveRoleNames);
            List<string> inactive = SplitNames(user.InactiveRoleNames);
            foreach (string role in active)
                html.Append("<span class='role-chip'>").Append(HttpUtility.HtmlEncode(role)).Append("</span>");
            foreach (string role in inactive)
                html.Append("<span class='role-chip inactive'>").Append(HttpUtility.HtmlEncode(role))
                    .Append(" — ").Append(HttpUtility.HtmlEncode(Localization.Get("AdminRoles_Inactive"))).Append("</span>");
            if (active.Count == 0 && inactive.Count == 0)
                html.Append("<span class='muted'>").Append(HttpUtility.HtmlEncode(Localization.Get("AdminUsers_NoRoles"))).Append("</span>");
            return html.ToString();
        }

        private static List<string> SplitNames(string names)
        {
            return string.IsNullOrWhiteSpace(names) ? new List<string>() :
                names.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(n => n.Trim()).Where(n => n.Length > 0).ToList();
        }

        private static bool Contains(string value, string search)
        {
            return !string.IsNullOrEmpty(value) && value.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private string GetUserFacingError(Exception ex)
        {
            ServiceException se = ex as ServiceException;
            if (se != null && System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.Equals("ru", StringComparison.OrdinalIgnoreCase))
                return se.LocalizedMessage;
            return ex.Message;
        }

        private void ShowError(string message)
        {
            litError.Text = HttpUtility.HtmlEncode(message);
            pnlError.Visible = true;
            pnlSuccess.Visible = false;
        }

        private void ShowSuccess(string message)
        {
            litSuccess.Text = HttpUtility.HtmlEncode(message);
            pnlSuccess.Visible = true;
            pnlError.Visible = false;
        }

        private void ClearMessages() { pnlError.Visible = false; pnlSuccess.Visible = false; }
    }
}