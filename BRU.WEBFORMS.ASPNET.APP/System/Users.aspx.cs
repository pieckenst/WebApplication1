using System;
using System.Web.UI.WebControls;
using BRU.WEBFORMS.ASPNET.APP.Services;

namespace BRU.WEBFORMS.ASPNET.APP.SystemPages
{
    public partial class Users : SecurePage
    {
        private bool _templateControlsResolved;

        protected override string[] RequiredRoles { get { return new[] { "administrator" }; } }

        protected global::System.Web.UI.WebControls.Label lblError;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbUsersList;
        protected global::System.Web.UI.WebControls.GridView gvUsers;

        protected void Page_Load(object sender, EventArgs e)
        {
            EnsureTemplateControlsResolved();
            if (!IsPostBack)
                BindUsers();
        }

        private void EnsureTemplateControlsResolved()
        {
            if (_templateControlsResolved)
                return;

            gvUsers = cbUsersList.FindContentControl<GridView>("gvUsers");
            if (gvUsers == null)
                throw new InvalidOperationException("User grid was not created inside the ContentBox template.");

            _templateControlsResolved = true;
        }

        private void BindUsers()
        {
            try
            {
                using (AuthService service = new AuthService())
                {
                    gvUsers.DataSource = service.GetAllUsers();
                    gvUsers.DataBind();
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
