using System;
using BRU.WEBFORMS.ASPNET.APP;

namespace BRU.WEBFORMS.ASPNET.APP.en.account
{
    /// <summary>
    /// Password-reset request form template. UI only: the handler below is a
    /// stub marking where to send a real password-reset link.
    /// </summary>
    public partial class ForgotPassword : BasePage
    {
        protected global::System.Web.UI.WebControls.Panel pnlStatus;
        protected global::System.Web.UI.WebControls.Literal litStatus;

        protected override void LoadPageContent()
        {
            LogPageContentLoad();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            // TODO: look up the account for txtEmail.Text and email a
            // time-limited password-reset link. Always show the same message
            // regardless of whether the address exists, to avoid disclosing
            // which emails are registered.
            litStatus.Text = "If an account exists for that email, a reset link has been sent.";
            pnlStatus.Visible = true;
        }
    }
}
