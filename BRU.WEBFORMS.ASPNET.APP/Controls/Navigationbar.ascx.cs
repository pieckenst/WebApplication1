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
                // Dashboard Section
                new NavigationItem { Title = "Dashboard", NavigateUrl = "~/Default.aspx", IconUrl = "~/en/images/toc_endnode.gif", ToolTip = "Main dashboard with fleet overview" },
                
                // Subheader: Fleet Management
                new NavigationItem { Title = "Fleet Management", IsSubheader = true },
                
                new NavigationItem { Title = "Bus Fleet", NavigateUrl = "~/Fleet/Buses.aspx", IconUrl = "~/en/images/toc_endnode.gif", ToolTip = "Manage bus fleet and vehicle operations" },
                new NavigationItem { Title = "Maintenance", NavigateUrl = "~/Fleet/Maintenance.aspx", IconUrl = "~/en/images/toc_endnode.gif", ToolTip = "Vehicle maintenance and service records" },
                
                // Subheader: Personnel
                new NavigationItem { Title = "Personnel", IsSubheader = true },
                
                new NavigationItem { Title = "Employees", NavigateUrl = "~/Personnel/Employees.aspx", IconUrl = "~/en/images/toc_endnode.gif", ToolTip = "Employee management and personnel records" },
                new NavigationItem { Title = "Departments", NavigateUrl = "~/Personnel/Departments.aspx", IconUrl = "~/en/images/toc_endnode.gif", ToolTip = "Department and job management" },
                
                // Subheader: Operations
                new NavigationItem { Title = "Operations", IsSubheader = true },
                
                new NavigationItem { Title = "Routes", NavigateUrl = "~/Operations/Routes.aspx", IconUrl = "~/en/images/toc_endnode.gif", ToolTip = "Route management and scheduling" },
                new NavigationItem { Title = "Schedule", NavigateUrl = "~/Operations/Schedule.aspx", IconUrl = "~/en/images/toc_endnode.gif", ToolTip = "Daily schedule and trip planning" },
                new NavigationItem { Title = "Stops", NavigateUrl = "~/Operations/Stops.aspx", IconUrl = "~/en/images/toc_endnode.gif", ToolTip = "Bus stop management" },
                
                // Subheader: Sales & Tickets
                new NavigationItem { Title = "Sales & Tickets", IsSubheader = true },
                
                new NavigationItem { Title = "Ticket Types", NavigateUrl = "~/Sales/Tickets.aspx", IconUrl = "~/en/images/toc_endnode.gif", ToolTip = "Ticket type management" },
                new NavigationItem { Title = "Sales", NavigateUrl = "~/Sales/Sales.aspx", IconUrl = "~/en/images/toc_endnode.gif", ToolTip = "Sales transactions and payments" },
                new NavigationItem { Title = "Payments", NavigateUrl = "~/Sales/Payments.aspx", IconUrl = "~/en/images/toc_endnode.gif", ToolTip = "Payment processing and history" },
                
                // Subheader: Reports
                new NavigationItem { Title = "Reports", IsSubheader = true },
                
                new NavigationItem { Title = "Analytics", NavigateUrl = "~/Reports/Analytics.aspx", IconUrl = "~/en/images/toc_endnode.gif", ToolTip = "Business analytics and performance reports" },
                new NavigationItem { Title = "Sales Reports", NavigateUrl = "~/Reports/SalesReports.aspx", IconUrl = "~/en/images/toc_endnode.gif", ToolTip = "Detailed sales reporting" },
                new NavigationItem { Title = "Maintenance Reports", NavigateUrl = "~/Reports/MaintenanceReports.aspx", IconUrl = "~/en/images/toc_endnode.gif", ToolTip = "Maintenance cost analysis" },
                
                // Subheader: System
                new NavigationItem { Title = "System", IsSubheader = true },
                
                new NavigationItem { Title = "Users", NavigateUrl = "~/System/Users.aspx", IconUrl = "~/en/images/toc_endnode.gif", ToolTip = "User account management" },
                new NavigationItem { Title = "Roles & Permissions", NavigateUrl = "~/System/Roles.aspx", IconUrl = "~/en/images/toc_endnode.gif", ToolTip = "Role and permission configuration" },
                new NavigationItem { Title = "Settings", NavigateUrl = "~/System/Settings.aspx", IconUrl = "~/en/images/toc_endnode.gif", ToolTip = "System configuration" },
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
