using System;
using System.Web.UI.HtmlControls;
using BRU.WEBFORMS.ASPNET.APP;

namespace BRU.WEBFORMS.ASPNET.APP.en.account
{
    /// <summary>
    /// Sign-in form template. UI only: the handlers below are stubs that show
    /// where to plug in a real authentication back end (password sign-in,
    /// passwordless one-time codes, two-factor verification and OAuth). No
    /// credentials are validated or stored here.
    /// </summary>
    public partial class Login : BasePage
    {
        protected override void LoadPageContent()
        {
            LogPageContentLoad();
        }

        protected void btnSignIn_Click(object sender, EventArgs e)
        {
            // TODO: validate txtEmail.Text / txtPassword.Text against your
            // membership provider and sign the user in.
            ShowStatus("Template only: connect btnSignIn_Click to your authentication back end.");
        }

        protected void btnPasswordless_Click(object sender, EventArgs e)
        {
            // TODO: send a one-time login code / magic link to txtEmail.Text.
            ShowStatus("Template only: wire up passwordless one-time-code delivery here.");
        }

        protected void btnVerifyMfa_Click(object sender, EventArgs e)
        {
            // TODO: verify txtMfaCode.Text against the user's TOTP / SMS code.
            ShowStatus("Template only: verify the two-factor code against the user's secret here.");
        }

        protected void OAuth_Click(object sender, EventArgs e)
        {
            var button = sender as HtmlButton;
            string provider = button != null ? button.Attributes["data-provider"] : null;
            // TODO: redirect to the given external identity provider's authorize
            // endpoint and handle the OAuth/OpenID Connect callback.
            ShowStatus("Template only: start the OAuth flow for provider '" + (provider ?? "unknown") + "' here.");
        }

        private void ShowStatus(string message)
        {
            litStatus.Text = Server.HtmlEncode(message);
            pnlStatus.Visible = true;
        }
    }
}
