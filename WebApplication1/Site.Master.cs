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
        protected void Page_Load(object sender, EventArgs e)
        {
            // Configuration can be set programmatically
            if (!IsPostBack)
            {
                // Initialize any master page level functionality
            }
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
