using System;
using System.Web.UI;

namespace BRU.WEBFORMS.ASPNET.APP
{
    /// <summary>
    /// Master page for Windows XP styled website template.
    /// Provides frame-based layout with toolbar, navigation sidebar, and content area.
    /// </summary>
    public partial class SiteMaster : MasterPage
    {
        protected global::System.Web.UI.WebControls.Literal litPageTitle;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            // Reflect the page's title (set via the Page Title attribute or
            // BasePage) in the browser title bar, falling back to SiteConfig.
            litPageTitle.Text = !string.IsNullOrEmpty(Page.Title)
                ? Page.Title
                : SiteConfig.DefaultPageTitle;
        }

        /// <summary>
        /// Gets or sets the site title displayed in the browser title bar.
        /// </summary>
        public string SiteTitle
        {
            get { return Page.Title; }
            set { Page.Title = value; }
        }
    }
}
