using System.Collections.Generic;
using BRU.WEBFORMS.ASPNET.APP.Controls;

namespace BRU.WEBFORMS.ASPNET.APP
{
    /// <summary>
    /// Navigation item sets for the site's sub-sections (Download Center,
    /// Help &amp; Support Center, Autopark Management). All internal links use app-relative "~/"
    /// paths so they resolve at runtime regardless of where the app is hosted;
    /// external service/community links come from SiteConfig (Web.config) so
    /// they are not hardcoded here, and open in a new window.
    /// </summary>
    public static class NavigationSets
    {
        public const string DownloadCenterHeader = "Download Center";
        public const string SupportHeader = "Help & Support Center";
        public const string AutoparkHeader = "Autopark Management";

        public static List<NavigationItem> DownloadCenter()
        {
            return new List<NavigationItem>
            {
                new NavigationItem { Title = "Download Center Home", NavigateUrl = "~/en/download-center/Default.aspx", ToolTip = "Windows Update Restored Homepage" },

                new NavigationItem { Title = "Popular Downloads", IsSubheader = true },
                new NavigationItem { Title = "Prerequisites Installer", NavigateUrl = "~/en/downloads/WurV5PatcherTool.aspx" },
                new NavigationItem { Title = "Inventory Checker", NavigateUrl = "~/en/downloads/WurInvChecker.aspx" },
                new NavigationItem { Title = "Internet Explorer Installers", NavigateUrl = "~/en/downloads/IeDownloads.aspx" },
                new NavigationItem { Title = "Windows Service Packs", NavigateUrl = "~/en/downloads/SpCenter.aspx" },
                new NavigationItem { Title = "Other Downloads", NavigateUrl = "~/en/download-center/OtherDownloads.aspx" },

                new NavigationItem { Title = "Available Resources", IsSubheader = true },
                new NavigationItem { Title = "Public Database", NavigateUrl = SiteConfig.PublicDatabaseUrl, Target = "_blank" },
                new NavigationItem { Title = "Legacy Update", NavigateUrl = SiteConfig.LegacyUpdateUrl, Target = "_blank" },

                new NavigationItem { Title = "Our Community", IsSubheader = true },
                new NavigationItem { Title = "MSFN Forum", NavigateUrl = SiteConfig.MsfnForumUrl, Target = "_blank" },
                new NavigationItem { Title = "Discord Server", NavigateUrl = SiteConfig.DiscordUrl, Target = "_blank" },

                new NavigationItem { Title = "Contribute", IsSubheader = true },
                new NavigationItem { Title = "Help Us and Contribute", NavigateUrl = "~/en/Contribute.aspx", CssClass = "nav-specialblue" },
                new NavigationItem { Title = "Donations", NavigateUrl = "~/en/Donations.aspx" },

                new NavigationItem { Title = "See Also", IsSubheader = true },
                new NavigationItem { Title = "Frequently Asked Questions", NavigateUrl = "~/en/FAQ.aspx", ToolTip = "Read how the project works, what we used to restore the websites and other answers to common questions related to the project." },
                new NavigationItem { Title = "Related Projects", NavigateUrl = "~/en/Related.aspx", ToolTip = "Windows Update Restored is also available through other projects. Go check them out!" },
                new NavigationItem { Title = "Help & Support Center", NavigateUrl = "~/en/support/Default.aspx", Target = "_top" },

                new NavigationItem { Title = "Windows Update Restored", IsSubheader = true },
                new NavigationItem { Title = "Site Homepage", NavigateUrl = "~/Default.aspx", Target = "_top" },
                new NavigationItem { Title = "About the Project", NavigateUrl = "~/en/About.aspx" },
            };
        }

        public static List<NavigationItem> Support()
        {
            return new List<NavigationItem>
            {
                new NavigationItem { Title = "Help & Support Center Home", NavigateUrl = "~/en/support/Default.aspx", ToolTip = "Windows Update Restored Homepage" },

                new NavigationItem { Title = "Available Resources", IsSubheader = true },
                new NavigationItem { Title = "Known Issues", NavigateUrl = "~/en/support/KnownIssues.aspx" },
                new NavigationItem { Title = "Knowledge Base", NavigateUrl = "~/en/support/KbArticles.aspx" },

                new NavigationItem { Title = "Our Community", IsSubheader = true },
                new NavigationItem { Title = "MSFN Forum", NavigateUrl = SiteConfig.MsfnForumUrl, Target = "_blank" },
                new NavigationItem { Title = "Discord Server", NavigateUrl = SiteConfig.DiscordUrl, Target = "_blank" },

                new NavigationItem { Title = "Contribute", IsSubheader = true },
                new NavigationItem { Title = "Help Us and Contribute", NavigateUrl = "~/en/Contribute.aspx", CssClass = "nav-specialblue" },
                new NavigationItem { Title = "Donations", NavigateUrl = "~/en/Donations.aspx" },

                new NavigationItem { Title = "See Also", IsSubheader = true },
                new NavigationItem { Title = "Frequently Asked Questions", NavigateUrl = "~/en/FAQ.aspx", ToolTip = "Read how the project works, what we used to restore the websites and other answers to common questions related to the project." },
                new NavigationItem { Title = "Related Projects", NavigateUrl = "~/en/Related.aspx", ToolTip = "Windows Update Restored is also available through other projects. Go check them out!" },
                new NavigationItem { Title = "Download Center", NavigateUrl = "~/en/download-center/Default.aspx", Target = "_top" },

                new NavigationItem { Title = "Windows Update Restored", IsSubheader = true },
                new NavigationItem { Title = "Site Homepage", NavigateUrl = "~/Default.aspx", Target = "_top" },
                new NavigationItem { Title = "About the Project", NavigateUrl = "~/en/About.aspx" },
            };
        }

        public static List<NavigationItem> Autopark()
        {
            return new List<NavigationItem>
            {
                new NavigationItem { Title = "Dashboard", NavigateUrl = "~/Default.aspx", ToolTip = "Main dashboard with fleet overview" },

                new NavigationItem { Title = "Fleet Management", IsSubheader = true },
                new NavigationItem { Title = "Bus Fleet", NavigateUrl = "~/Fleet/Buses.aspx", ToolTip = "Manage bus fleet and vehicle operations" },
                new NavigationItem { Title = "Maintenance", NavigateUrl = "~/Fleet/Maintenance.aspx", ToolTip = "Vehicle maintenance and service records" },

                new NavigationItem { Title = "Personnel", IsSubheader = true },
                new NavigationItem { Title = "Employees", NavigateUrl = "~/Personnel/Employees.aspx", ToolTip = "Employee management and personnel records" },
                new NavigationItem { Title = "Departments", NavigateUrl = "~/Personnel/Departments.aspx", ToolTip = "Department and job management" },

                new NavigationItem { Title = "Operations", IsSubheader = true },
                new NavigationItem { Title = "Routes", NavigateUrl = "~/Operations/Routes.aspx", ToolTip = "Route management and scheduling" },
                new NavigationItem { Title = "Schedule", NavigateUrl = "~/Operations/Schedule.aspx", ToolTip = "Daily schedule and trip planning" },
                new NavigationItem { Title = "Stops", NavigateUrl = "~/Operations/Stops.aspx", ToolTip = "Bus stop management" },

                new NavigationItem { Title = "Sales & Tickets", IsSubheader = true },
                new NavigationItem { Title = "Ticket Types", NavigateUrl = "~/Sales/Tickets.aspx", ToolTip = "Ticket type management" },
                new NavigationItem { Title = "Sales", NavigateUrl = "~/Sales/Sales.aspx", ToolTip = "Sales transactions and payments" },
                new NavigationItem { Title = "Payments", NavigateUrl = "~/Sales/Payments.aspx", ToolTip = "Payment processing and history" },

                new NavigationItem { Title = "Reports", IsSubheader = true },
                new NavigationItem { Title = "Analytics", NavigateUrl = "~/Reports/Analytics.aspx", ToolTip = "Business analytics and performance reports" },
                new NavigationItem { Title = "Sales Reports", NavigateUrl = "~/Reports/SalesReports.aspx", ToolTip = "Detailed sales reporting" },
                new NavigationItem { Title = "Maintenance Reports", NavigateUrl = "~/Reports/MaintenanceReports.aspx", ToolTip = "Maintenance cost analysis" },

                new NavigationItem { Title = "System", IsSubheader = true },
                new NavigationItem { Title = "Users", NavigateUrl = "~/System/Users.aspx", ToolTip = "User account management" },
                new NavigationItem { Title = "Roles & Permissions", NavigateUrl = "~/System/Roles.aspx", ToolTip = "Role and permission configuration" },
                new NavigationItem { Title = "Settings", NavigateUrl = "~/System/Settings.aspx", ToolTip = "System configuration" },
            };
        }
    }
}
