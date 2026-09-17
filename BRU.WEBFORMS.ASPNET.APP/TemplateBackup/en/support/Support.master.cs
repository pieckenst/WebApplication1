using System;
using System.Web.UI;
using BRU.WEBFORMS.ASPNET.APP.Controls;

namespace BRU.WEBFORMS.ASPNET.APP.en.support
{
    /// <summary>
    /// Nested master for the Help &amp; Support Center sub-site. Reuses the site
    /// frame layout (Site.Master) but swaps in the Support logo and its
    /// section-specific navigation.
    /// </summary>
    public partial class SupportMaster : MasterPage
    {
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.Navigationbar Navigationbar1;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Navigationbar1.Configure(NavigationSets.SupportHeader, NavigationSets.Support());
        }
    }
}
