using System;
using System.Web.UI.HtmlControls;
using BRU.WEBFORMS.ASPNET.APP;

namespace BRU.WEBFORMS.ASPNET.APP.en.account
{
    /// <summary>
    /// Account registration form template. UI only: the handlers below are
    /// stubs marking where to plug in real account creation and OAuth sign-up.
    /// </summary>
    public partial class Register : BasePage
    {
        protected override void LoadPageContent()
        {
            LogPageContentLoad();
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            // TODO: validate the inputs (email format, password strength,
            // matching confirmation, accepted terms) and create the account.
            ShowStatus("Template only: connect btnRegister_Click to your account-creation back end.");
        }

        protected void OAuth_Click(object sender, EventArgs e)
        {
            var button = sender as HtmlButton;
            string provider = button != null ? button.Attributes["data-provider"] : null;
            // TODO: start the external identity provider sign-up/OAuth flow.
            ShowStatus("Template only: start the OAuth sign-up flow for provider '" + (provider ?? "unknown") + "' here.");
        }

        private void ShowStatus(string message)
        {
            litStatus.Text = Server.HtmlEncode(message);
            pnlStatus.Visible = true;
        }
    }
}
