using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using BRU.WEBFORMS.ASPNET.APP.Models;
using BRU.WEBFORMS.ASPNET.APP.Services;

namespace BRU.WEBFORMS.ASPNET.APP.SystemPages
{
    public partial class Roles : SecurePage
    {
        protected override string[] RequiredRoles { get { return new[] { "administrator" }; } }

        protected global::System.Web.UI.WebControls.Panel pnlError;
        protected global::System.Web.UI.WebControls.Literal litError;
        protected global::System.Web.UI.WebControls.Panel pnlSuccess;
        protected global::System.Web.UI.WebControls.Literal litSuccess;
        protected global::System.Web.UI.WebControls.Literal litTotalRoles;
        protected global::System.Web.UI.WebControls.Literal litActiveRoles;
        protected global::System.Web.UI.WebControls.Literal litInactiveRoles;
        protected global::System.Web.UI.WebControls.TextBox txtSearch;
        protected global::System.Web.UI.WebControls.DropDownList ddlRoleStatus;
        protected global::System.Web.UI.WebControls.Button btnApplyFilters;
        protected global::System.Web.UI.WebControls.Button btnNewRole;
        protected global::System.Web.UI.WebControls.Panel pnlEditor;
        protected global::System.Web.UI.WebControls.Literal litEditorTitle;
        protected global::System.Web.UI.WebControls.HiddenField hfRoleId;
        protected global::System.Web.UI.WebControls.TextBox txtRoleName;
        protected global::System.Web.UI.WebControls.TextBox txtRoleDescription;
        protected global::System.Web.UI.WebControls.CheckBox chkRoleActive;
        protected global::System.Web.UI.WebControls.CheckBoxList cblPermissions;
        protected global::System.Web.UI.WebControls.Panel pnlSystemRoleNote;
        protected global::System.Web.UI.WebControls.Literal litSystemRoleNote;
        protected global::System.Web.UI.WebControls.Button btnSaveRole;
        protected global::System.Web.UI.WebControls.Button btnCancelEdit;
        protected global::System.Web.UI.WebControls.GridView gvRoles;

        private int CurrentActorId { get { return AuthContext.CurrentUserId ?? 0; } }

        protected void Page_Init(object sender, EventArgs e)
        {
            try { BindPermissionChoices(); }
            catch (Exception ex) { ShowError(GetRoleFacingError(ex)); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) BindRoles();
        }

        private void BindPermissionChoices()
        {
            cblPermissions.Items.Clear();
            using (AuthService service = new AuthService())
                foreach (Permission permission in service.GetAllPermissions().OrderBy(p => p.PermissionName))
                {
                    string label = permission.PermissionName;
                    if (!string.IsNullOrWhiteSpace(permission.Description)) label += " — " + permission.Description;
                    cblPermissions.Items.Add(new ListItem(label, permission.PermissionId.ToString()));
                }
        }

        private void BindRoles()
        {
            try
            {
                List<Role> roles;
                using (AuthService service = new AuthService()) roles = service.GetAllRoles() ?? new List<Role>();
                litTotalRoles.Text = roles.Count.ToString();
                litActiveRoles.Text = roles.Count(r => r.IsActive).ToString();
                litInactiveRoles.Text = roles.Count(r => !r.IsActive).ToString();

                IEnumerable<Role> filtered = roles;
                if (ddlRoleStatus.SelectedValue == "active") filtered = filtered.Where(r => r.IsActive);
                else if (ddlRoleStatus.SelectedValue == "inactive") filtered = filtered.Where(r => !r.IsActive);
                string search = (txtSearch.Text ?? string.Empty).Trim();
                if (search.Length > 0) filtered = filtered.Where(r => Contains(r.RoleName, search) || Contains(r.Description, search));
                gvRoles.DataSource = filtered.ToList();
                gvRoles.DataBind();
            }
            catch (Exception ex) { ShowError(GetRoleFacingError(ex)); }
        }

        protected void btnApplyFilters_Click(object sender, EventArgs e) { BindRoles(); }

        protected void btnNewRole_Click(object sender, EventArgs e)
        {
            ClearMessages();
            hfRoleId.Value = "0";
            litEditorTitle.Text = HttpUtility.HtmlEncode(Localization.Get("AdminRoles_New"));
            txtRoleName.Text = string.Empty;
            txtRoleName.ReadOnly = false;
            txtRoleDescription.Text = string.Empty;
            chkRoleActive.Checked = true;
            chkRoleActive.Enabled = true;
            cblPermissions.Enabled = true;
            foreach (ListItem item in cblPermissions.Items) item.Selected = false;
            pnlSystemRoleNote.Visible = false;
            pnlEditor.Visible = true;
        }

        protected void btnCancelEdit_Click(object sender, EventArgs e) { pnlEditor.Visible = false; ClearMessages(); }

        protected void btnSaveRole_Click(object sender, EventArgs e)
        {
            ClearMessages();
            int roleId;
            if (!int.TryParse(hfRoleId.Value, out roleId) || roleId < 0)
            {
                ShowError(Localization.Get("AdminRoles_InvalidRole"));
                return;
            }

            try
            {
                using (AuthService service = new AuthService())
                {
                    bool isNew = roleId == 0;
                    Role existing = isNew ? null : service.GetRoleById(roleId);
                    if (!isNew && existing == null) { ShowError(Localization.Get("AdminRoles_InvalidRole")); return; }

                    bool protectedAdmin = existing != null && string.Equals(existing.RoleName, "administrator", StringComparison.OrdinalIgnoreCase);
                    Role role = new Role
                    {
                        RoleId = roleId,
                        RoleName = protectedAdmin ? existing.RoleName : (txtRoleName.Text ?? string.Empty).Trim(),
                        Description = (txtRoleDescription.Text ?? string.Empty).Trim(),
                        IsActive = protectedAdmin ? existing.IsActive : chkRoleActive.Checked
                    };
                    List<int> permissionIds = protectedAdmin
                        ? service.GetRolePermissions(roleId).Select(p => p.PermissionId).ToList()
                        : cblPermissions.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => Convert.ToInt32(i.Value)).Distinct().ToList();
                    service.SaveRoleAdministration(role, permissionIds, CurrentActorId);
                }

                LogInformation((roleId == 0 ? "Created" : "Updated") + " role '" + (txtRoleName.Text ?? string.Empty).Trim() + "' by administrator '" + AuthContext.CurrentUsername + "'.");
                pnlEditor.Visible = false;
                ShowSuccess(Localization.Get(roleId == 0 ? "AdminRoles_CreatedSuccess" : "AdminRoles_SavedSuccess"));
                BindRoles();
            }
            catch (Exception ex) { ShowError(GetRoleFacingError(ex)); }
        }

        protected void gvRoles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e == null || e.CommandArgument == null) return;
            int roleId;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out roleId) || roleId <= 0)
            {
                ShowError(Localization.Get("AdminRoles_InvalidRole"));
                return;
            }
            try
            {
                if (e.CommandName == "EditRole") { OpenRoleEditor(roleId); return; }
                if (e.CommandName == "DeleteRole")
                {
                    using (AuthService service = new AuthService()) service.DeleteRole(roleId);
                    LogInformation("Deleted role id=" + roleId + " by administrator '" + AuthContext.CurrentUsername + "'.");
                    ShowSuccess(Localization.Get("AdminRoles_DeletedSuccess"));
                    BindRoles();
                }
            }
            catch (Exception ex) { ShowError(GetRoleFacingError(ex)); }
        }

        private void OpenRoleEditor(int roleId)
        {
            using (AuthService service = new AuthService())
            {
                Role role = service.GetRoleById(roleId);
                if (role == null) throw new InvalidOperationException(Localization.Get("AdminRoles_InvalidRole"));
                hfRoleId.Value = role.RoleId.ToString();
                litEditorTitle.Text = HttpUtility.HtmlEncode(Localization.Get("AdminRoles_Edit") + " — " + role.RoleName);
                txtRoleName.Text = role.RoleName;
                txtRoleDescription.Text = role.Description ?? string.Empty;
                chkRoleActive.Checked = role.IsActive;

                bool systemAdmin = string.Equals(role.RoleName, "administrator", StringComparison.OrdinalIgnoreCase);
                txtRoleName.ReadOnly = systemAdmin;
                chkRoleActive.Enabled = !systemAdmin;
                cblPermissions.Enabled = !systemAdmin;
                pnlSystemRoleNote.Visible = systemAdmin;
                litSystemRoleNote.Text = HttpUtility.HtmlEncode(Localization.Get("AdminRoles_SystemRoleNote"));

                HashSet<int> permissionIds = new HashSet<int>(service.GetRolePermissions(roleId).Select(p => p.PermissionId));
                foreach (ListItem item in cblPermissions.Items) item.Selected = permissionIds.Contains(Convert.ToInt32(item.Value));
                pnlEditor.Visible = true;
                ClearMessages();
            }
        }

        protected void gvRoles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            Role role = e.Row.DataItem as Role;
            if (role == null) return;

            bool systemAdmin = string.Equals(role.RoleName, "administrator", StringComparison.OrdinalIgnoreCase);
            Label status = e.Row.FindControl("lblRoleStatus") as Label;
            if (status != null)
            {
                status.Text = Localization.Get(role.IsActive ? "AdminRoles_Active" : "AdminRoles_Inactive");
                status.CssClass = "status-pill " + (role.IsActive ? "status-active" : "status-inactive");
                if (systemAdmin) status.Text += " · " + Localization.Get("AdminRoles_Protected");
            }

            Button delete = e.Row.FindControl("btnDeleteRole") as Button;
            if (delete != null)
            {
                delete.Visible = !systemAdmin && role.UserCount == 0;
                delete.ToolTip = systemAdmin ? Localization.Get("AdminRoles_Protected") :
                    (role.UserCount > 0 ? Localization.Get("AdminRoles_AssignedDeleteHint") : Localization.Get("AdminCommon_Delete"));
                delete.OnClientClick = "return confirm('" + HttpUtility.JavaScriptStringEncode(Localization.Get("AdminRoles_DeleteConfirm")) + "');";
            }
        }

        private string GetRoleFacingError(Exception ex)
        {
            ServiceException se = ex as ServiceException;
            if (se != null && System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.Equals("ru", StringComparison.OrdinalIgnoreCase))
                return se.LocalizedMessage;
            return ex.Message;
        }

        private static bool Contains(string value, string search)
        {
            return !string.IsNullOrEmpty(value) && value.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;
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