using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using BRU.WEBFORMS.ASPNET.APP.Models;
using BRU.WEBFORMS.ASPNET.APP.Services;

namespace BRU.WEBFORMS.ASPNET.APP
{
    public partial class Login : SecurePage
    {
        private bool _templateControlsResolved;

        protected global::System.Web.UI.WebControls.Panel pnlError;
        protected global::System.Web.UI.WebControls.Literal litError;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbSignIn;
        protected global::System.Web.UI.WebControls.TextBox txtLogin;
        protected global::System.Web.UI.WebControls.TextBox txtPassword;
        protected global::System.Web.UI.WebControls.CheckBox chkRememberMe;
        protected global::System.Web.UI.WebControls.Button btnSignIn;

        protected override bool AllowAnonymous
        {
            get { return true; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            EnsureTemplateControlsResolved();
            if (!IsPostBack && AuthContext.IsAuthenticated)
                RedirectToLocalUrl(ResolveUrl("~/Default.aspx"));
        }

        private void EnsureTemplateControlsResolved()
        {
            if (_templateControlsResolved)
                return;

            txtLogin = cbSignIn.FindContentControl<TextBox>("txtLogin");
            txtPassword = cbSignIn.FindContentControl<TextBox>("txtPassword");
            chkRememberMe = cbSignIn.FindContentControl<CheckBox>("chkRememberMe");
            btnSignIn = cbSignIn.FindContentControl<Button>("btnSignIn");

            if (txtLogin == null || txtPassword == null || chkRememberMe == null || btnSignIn == null)
                throw new InvalidOperationException("Sign-in controls were not created inside the ContentBox template.");

            _templateControlsResolved = true;
        }

        protected void btnSignIn_Click(object sender, EventArgs e)
        {
            pnlError.Visible = false;
            try
            {
                UserWithRolesAndPermissions authorization;
                using (AuthService service = new AuthService())
                    authorization = service.AuthenticateUser(txtLogin.Text, txtPassword.Text);

                AuthContext.SignIn(
                    authorization.User,
                    authorization.Roles,
                    authorization.Permissions,
                    chkRememberMe.Checked,
                    authorization.User.EmployeeId);

                string returnUrl = Request.QueryString["ReturnUrl"];
                string destination = IsLocalReturnUrl(returnUrl) ? returnUrl : ResolveUrl("~/Default.aspx");
                RedirectToLocalUrl(destination);
            }
            catch (ServiceException ex)
            {
                litError.Text = Server.HtmlEncode(ex.Message);
                pnlError.Visible = true;
            }
            catch (SqlException ex)
            {
                HandleDatabaseError(ex);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Login failed for request {0}: {1}", Request.Url.AbsolutePath, ex);
                litError.Text = "Sign-in is temporarily unavailable. Please try again later.";
                pnlError.Visible = true;
            }
        }

        private bool IsLocalReturnUrl(string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(returnUrl) || !returnUrl.StartsWith("/", StringComparison.Ordinal) ||
                returnUrl.StartsWith("//", StringComparison.Ordinal) || returnUrl.StartsWith("/\\", StringComparison.Ordinal) ||
                returnUrl.IndexOf('\\') >= 0)
                return false;

            Uri destination;
            return Uri.TryCreate(Request.Url, returnUrl, out destination) &&
                string.Equals(destination.Host, Request.Url.Host, StringComparison.OrdinalIgnoreCase) &&
                destination.Port == Request.Url.Port &&
                string.Equals(destination.Scheme, Request.Url.Scheme, StringComparison.OrdinalIgnoreCase);
        }

        private void RedirectToLocalUrl(string destination)
        {
            Response.Redirect(destination, false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
