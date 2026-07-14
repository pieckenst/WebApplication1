using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BRU.WEBFORMS.ASPNET.APP.Controls
{
    /// <summary>
    /// Navigation bar user control for Windows XP styled website.
    /// Provides a sidebar navigation with expandable sections.
    /// Site name is configurable via Web.config.
    /// </summary>
    public partial class Navigationbar : UserControl
    {
        protected global::System.Web.UI.WebControls.Repeater rptNavigation;
        protected global::System.Web.UI.WebControls.Literal litSiteName;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(litSiteName.Text))
                litSiteName.Text = NavHeader;

            if (!IsPostBack)
            {
                BindNavigation();
            }
        }

        private System.Collections.Generic.List<NavigationItem> _items;
        private string _navHeader;

        /// <summary>
        /// Configures a section-specific navigation bar (header + items),
        /// e.g. for the Download Center or Support sub-sites. Call from the
        /// section master before the control's Load runs.
        /// </summary>
        public void Configure(string header, System.Collections.Generic.List<NavigationItem> items)
        {
            _navHeader = header;
            _items = items;
        }

        /// <summary>
        /// Header text shown at the top of the navigation bar. Defaults to the
        /// site name; section navigation controls override this.
        /// </summary>
        protected virtual string NavHeader
        {
            get { return string.IsNullOrEmpty(_navHeader) ? SiteConfig.SiteName : _navHeader; }
        }

        /// <summary>
        /// Binds navigation items to the repeater.
        /// </summary>
        protected virtual void BindNavigation()
        {
            var navigationItems = _items ?? GetNavigationItems();
            rptNavigation.DataSource = navigationItems;
            rptNavigation.DataBind();
        }

        /// <summary>
        /// Gets the navigation items for the sidebar.
        /// Override this method to customize navigation.
        /// </summary>
        protected virtual System.Collections.Generic.List<NavigationItem> GetNavigationItems()
        {
            return new System.Collections.Generic.List<NavigationItem>
            {
                // Welcome Section
                new NavigationItem { Title = "Welcome!", NavigateUrl = "~/Default.aspx", IconUrl = "~/en/images/toc_endnode.gif", ToolTip = "Homepage" },
                new NavigationItem { Title = "About", NavigateUrl = "~/en/About.aspx", IconUrl = "~/en/images/toc_endnode.gif" },
                new NavigationItem { Title = "FAQ", NavigateUrl = "~/en/FAQ.aspx", IconUrl = "~/en/images/toc_endnode.gif", ToolTip = "Frequently Asked Questions" },
                
                // Subheader: Main Sections
                new NavigationItem { Title = "Information", IsSubheader = true },
                
                new NavigationItem { Title = "Compatibility", NavigateUrl = "~/en/Compatibility.aspx", IconUrl = "~/en/images/toc_endnode.gif" },
                new NavigationItem { Title = "Prerequisites", NavigateUrl = "~/en/Prerequisites.aspx", IconUrl = "~/en/images/toc_endnode.gif" },
                
                // Subheader: Resources
                new NavigationItem { Title = "Resources", IsSubheader = true },
                
                new NavigationItem { Title = "Download Center", NavigateUrl = "~/en/download-center/", IconUrl = "~/en/images/toc_endnode.gif", Target = "_top" },
                new NavigationItem { Title = "Help & Support", NavigateUrl = "~/en/support/", IconUrl = "~/en/images/toc_endnode.gif", Target = "_top" },
                
                // Subheader: Community
                new NavigationItem { Title = "Community", IsSubheader = true },
                
                new NavigationItem { Title = "Contribute", NavigateUrl = "~/en/Contribute.aspx", IconUrl = "~/en/images/toc_endnode.gif", CssClass = "nav-specialblue" },
                new NavigationItem { Title = "Donations", NavigateUrl = "~/en/Donations.aspx", IconUrl = "~/en/images/toc_endnode.gif" },
                
                // Subheader: Legal
                new NavigationItem { Title = "Legal", IsSubheader = true },
                
                new NavigationItem { Title = "Privacy Policy", NavigateUrl = "~/en/Privacy.aspx", IconUrl = "~/en/images/toc_endnode.gif" },
                new NavigationItem { Title = "Legal Information", NavigateUrl = "~/en/Legal.aspx", IconUrl = "~/en/images/toc_endnode.gif" },
            };
        }

        protected void rptNavigation_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var item = e.Item.DataItem as NavigationItem;
                if (item != null)
                {
                    var phSubheader = e.Item.FindControl("phSubheader") as PlaceHolder;
                    var phLink = e.Item.FindControl("phLink") as PlaceHolder;
                    
                    if (phSubheader != null) phSubheader.Visible = item.IsSubheader;
                    if (phLink != null) phLink.Visible = !item.IsSubheader;
                }
            }
        }

        protected string GetLinkCssClass(object cssClass)
        {
            string css = cssClass?.ToString();
            return string.IsNullOrEmpty(css) ? "nav-link" : "nav-link " + css;
        }

        /// <summary>
        /// Gets or sets the site name displayed in the navigation header.
        /// Overrides Web.config setting.
        /// </summary>
        public string SiteName
        {
            get { return litSiteName.Text; }
            set { litSiteName.Text = value; }
        }
    }

    /// <summary>
    /// Represents a navigation item in the sidebar.
    /// </summary>
    public class NavigationItem
    {
        public string Title { get; set; }
        public string NavigateUrl { get; set; }
        public string IconUrl { get; set; }
        public string Target { get; set; }
        public string ToolTip { get; set; }
        public string CssClass { get; set; }
        public bool IsSubheader { get; set; }

        public NavigationItem()
        {
            IconUrl = "~/en/images/toc_endnode.gif";
            Target = "_self";
            IsSubheader = false;
        }
    }
}
