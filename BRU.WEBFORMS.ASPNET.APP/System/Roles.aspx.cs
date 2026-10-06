using System;
using System.Web.UI.WebControls;
using BRU.WEBFORMS.ASPNET.APP.Services;

namespace BRU.WEBFORMS.ASPNET.APP.SystemPages
{
    public partial class Roles : SecurePage
    {
        private bool _templateControlsResolved;

        protected override string[] RequiredRoles { get { return new[] { "administrator" }; } }

        protected global::System.Web.UI.WebControls.Label lblError;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbRolesList;
        protected global::System.Web.UI.WebControls.GridView gvRoles;

        protected void Page_Load(object sender, EventArgs e)
        {
            EnsureTemplateControlsResolved();
            if (!IsPostBack)
                BindRoles();
        }

        private void EnsureTemplateControlsResolved()
        {
            if (_templateControlsResolved)
                return;

            gvRoles = cbRolesList.FindContentControl<GridView>("gvRoles");
            if (gvRoles == null)
                throw new InvalidOperationException("Role grid was not created inside the ContentBox template.");

            _templateControlsResolved = true;
        }

        private void BindRoles()
        {
            try
            {
                using (AuthService service = new AuthService())
                {
                    gvRoles.DataSource = service.GetAllRoles();
                    gvRoles.DataBind();
                }
                lblError.Visible = false;
            }
            catch (ServiceException ex)
            {
                ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        private void ShowError(string message)
        {
            lblError.Text = Server.HtmlEncode(message);
            lblError.Visible = true;
        }
    }
}
