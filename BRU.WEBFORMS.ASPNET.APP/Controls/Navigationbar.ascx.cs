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
            rptNavigation.DataSource = FilterNavigationItems(navigationItems);
            rptNavigation.DataBind();
        }

        private static System.Collections.Generic.List<NavigationItem> FilterNavigationItems(System.Collections.Generic.List<NavigationItem> items)
        {
            System.Collections.Generic.List<NavigationItem> filtered = new System.Collections.Generic.List<NavigationItem>();
            for (int index = 0; index < items.Count; index++)
            {
                NavigationItem item = items[index];
                if (!item.IsSubheader)
                {
                    if (HasNavigationAccess(item.NavigateUrl)) filtered.Add(item);
                    continue;
                }

                bool hasVisibleItem = false;
                for (int next = index + 1; next < items.Count && !items[next].IsSubheader; next++)
                    hasVisibleItem |= HasNavigationAccess(items[next].NavigateUrl);
                if (hasVisibleItem) filtered.Add(item);
            }
            return filtered;
        }

        private static bool HasNavigationAccess(string navigateUrl)
        {
            if (!AuthContext.IsAuthenticated)
                return false;
            if (string.IsNullOrEmpty(navigateUrl) || !navigateUrl.StartsWith("~/", StringComparison.Ordinal))
                return true;

            string path = navigateUrl.Substring(2).ToLowerInvariant();
            if (path == "default.aspx") return true;
            if (path == "fleet/buses.aspx") return AuthContext.HasPermission("bus.read");
            if (path == "fleet/maintenance.aspx") return AuthContext.HasPermission("bus.read");
            if (path == "personnel/employees.aspx") return AuthContext.HasPermission("employee.read");
            if (path == "personnel/departments.aspx") return AuthContext.HasPermission("employee.read");
            if (path == "operations/routes.aspx") return AuthContext.HasPermission("route.read");
            if (path == "operations/schedule.aspx") return AuthContext.HasPermission("route.read");
            if (path == "operations/stops.aspx") return AuthContext.HasPermission("route.read");
            if (path == "sales/tickets.aspx") return AuthContext.HasPermission("ticket.read");
            if (path == "sales/sales.aspx") return AuthContext.HasPermission("sale.read");
            if (path == "sales/payments.aspx") return AuthContext.HasPermission("payment.read");
            if (path.StartsWith("reports/", StringComparison.Ordinal)) return AuthContext.HasPermission("report.read");
            if (path.StartsWith("system/", StringComparison.Ordinal)) return AuthContext.IsInRole("administrator");
            return true;
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
                new NavigationItem { Title = Localization.Get("Nav_Dashboard"), NavigateUrl = "~/Default.aspx", IconUrl = "~/images/autopark_icon.svg", ToolTip = "Main dashboard with fleet overview" },
                
                // Subheader: Fleet Management
                new NavigationItem { Title = Localization.Get("Nav_FleetManagement"), IsSubheader = true },
                
                new NavigationItem { Title = Localization.Get("Nav_BusFleet"), NavigateUrl = "~/Fleet/Buses.aspx", IconUrl = "~/images/autopark_icon.svg", ToolTip = "Manage bus fleet and vehicle operations" },
                new NavigationItem { Title = Localization.Get("Nav_Maintenance"), NavigateUrl = "~/Fleet/Maintenance.aspx", IconUrl = "~/images/autopark_icon.svg", ToolTip = "Vehicle maintenance and service records" },
                
                // Subheader: Personnel
                new NavigationItem { Title = Localization.Get("Nav_Personnel"), IsSubheader = true },
                
                new NavigationItem { Title = Localization.Get("Nav_Employees"), NavigateUrl = "~/Personnel/Employees.aspx", IconUrl = "~/images/autopark_icon.svg", ToolTip = "Employee management and personnel records" },
                new NavigationItem { Title = Localization.Get("Nav_Departments"), NavigateUrl = "~/Personnel/Departments.aspx", IconUrl = "~/images/autopark_icon.svg", ToolTip = "Department and job management" },
                
                // Subheader: Operations
                new NavigationItem { Title = Localization.Get("Nav_Operations"), IsSubheader = true },
                
                new NavigationItem { Title = Localization.Get("Nav_Routes"), NavigateUrl = "~/Operations/Routes.aspx", IconUrl = "~/images/autopark_icon.svg", ToolTip = "Route management and scheduling" },
                new NavigationItem { Title = Localization.Get("Nav_Schedule"), NavigateUrl = "~/Operations/Schedule.aspx", IconUrl = "~/images/autopark_icon.svg", ToolTip = "Daily schedule and trip planning" },
                new NavigationItem { Title = Localization.Get("Nav_Stops"), NavigateUrl = "~/Operations/Stops.aspx", IconUrl = "~/images/autopark_icon.svg", ToolTip = "Bus stop management" },
                
                // Subheader: Sales & Tickets
                new NavigationItem { Title = Localization.Get("Nav_SalesTickets"), IsSubheader = true },
                
                new NavigationItem { Title = Localization.Get("Nav_TicketTypes"), NavigateUrl = "~/Sales/Tickets.aspx", IconUrl = "~/images/autopark_icon.svg", ToolTip = "Ticket type management" },
                new NavigationItem { Title = Localization.Get("Nav_Sales"), NavigateUrl = "~/Sales/Sales.aspx", IconUrl = "~/images/autopark_icon.svg", ToolTip = "Sales transactions and payments" },
                new NavigationItem { Title = Localization.Get("Nav_Payments"), NavigateUrl = "~/Sales/Payments.aspx", IconUrl = "~/images/autopark_icon.svg", ToolTip = "Payment processing and history" },
                
                // Subheader: Reports
                new NavigationItem { Title = Localization.Get("Nav_Reports"), IsSubheader = true },
                
                new NavigationItem { Title = Localization.Get("Nav_Analytics"), NavigateUrl = "~/Reports/Analytics.aspx", IconUrl = "~/images/autopark_icon.svg", ToolTip = "Business analytics and performance reports" },
                new NavigationItem { Title = Localization.Get("Nav_SalesReports"), NavigateUrl = "~/Reports/SalesReports.aspx", IconUrl = "~/images/autopark_icon.svg", ToolTip = "Detailed sales reporting" },
                new NavigationItem { Title = Localization.Get("Nav_MaintenanceReports"), NavigateUrl = "~/Reports/MaintenanceReports.aspx", IconUrl = "~/images/autopark_icon.svg", ToolTip = "Maintenance cost analysis" },
                
                // Subheader: System
                new NavigationItem { Title = Localization.Get("Nav_System"), IsSubheader = true },
                
                new NavigationItem { Title = Localization.Get("Nav_Users"), NavigateUrl = "~/System/Users.aspx", IconUrl = "~/images/autopark_icon.svg", ToolTip = "User account management" },
                new NavigationItem { Title = Localization.Get("Nav_Roles"), NavigateUrl = "~/System/Roles.aspx", IconUrl = "~/images/autopark_icon.svg", ToolTip = "Role and permission configuration" },
                new NavigationItem { Title = Localization.Get("Nav_Settings"), NavigateUrl = "~/System/Settings.aspx", IconUrl = "~/images/autopark_icon.svg", ToolTip = "System configuration" },
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
            IconUrl = "~/images/autopark_icon.svg";
            Target = "_self";
            IsSubheader = false;
        }
    }
}
