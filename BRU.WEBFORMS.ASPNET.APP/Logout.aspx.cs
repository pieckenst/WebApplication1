using System;
using System.Web.UI;

namespace BRU.WEBFORMS.ASPNET.APP
{
    public partial class Logout : SecurePage
    {
        protected override bool AllowAnonymous
        {
            get { return true; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthContext.SignOut();
            Response.Redirect(ResolveUrl("~/Login.aspx"), false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
