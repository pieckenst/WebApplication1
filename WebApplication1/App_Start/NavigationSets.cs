using System.Collections.Generic;
using BRU.WEBFORMS.ASPNET.APP.Controls;

namespace BRU.WEBFORMS.ASPNET.APP
{
    /// <summary>
    /// Navigation item sets for the site's sub-sections (Download Center,
    /// Help &amp; Support Center). All internal links use app-relative "~/"
    /// paths so they resolve at runtime regardless of where the app is hosted;
    /// external service/community links come from SiteConfig (Web.config) so
    /// they are not hardcoded here, and open in a new window.
    /// </summary>
    public static class NavigationSets
    {
        public const string DownloadCenterHeader = "Download Center";
        public const string SupportHeader = "Help & Support Center";

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
                new NavigationItem { Title = "Related Projects", NavigateUrl = "~/en/related.htm.tmp.html", ToolTip = "Windows Update Restored is also available through other projects. Go check them out!" },
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
                new NavigationItem { Title = "Known Issues", NavigateUrl = "~/en/knownissues.htm.tmp.html" },
                new NavigationItem { Title = "Knowledge Base", NavigateUrl = "~/en/support/KbArticles.aspx" },

                new NavigationItem { Title = "Our Community", IsSubheader = true },
                new NavigationItem { Title = "MSFN Forum", NavigateUrl = SiteConfig.MsfnForumUrl, Target = "_blank" },
                new NavigationItem { Title = "Discord Server", NavigateUrl = SiteConfig.DiscordUrl, Target = "_blank" },

                new NavigationItem { Title = "Contribute", IsSubheader = true },
                new NavigationItem { Title = "Help Us and Contribute", NavigateUrl = "~/en/Contribute.aspx", CssClass = "nav-specialblue" },
                new NavigationItem { Title = "Donations", NavigateUrl = "~/en/Donations.aspx" },

                new NavigationItem { Title = "See Also", IsSubheader = true },
                new NavigationItem { Title = "Frequently Asked Questions", NavigateUrl = "~/en/FAQ.aspx", ToolTip = "Read how the project works, what we used to restore the websites and other answers to common questions related to the project." },
                new NavigationItem { Title = "Related Projects", NavigateUrl = "~/en/related.htm.tmp.html", ToolTip = "Windows Update Restored is also available through other projects. Go check them out!" },
                new NavigationItem { Title = "Download Center", NavigateUrl = "~/en/download-center/Default.aspx", Target = "_top" },

                new NavigationItem { Title = "Windows Update Restored", IsSubheader = true },
                new NavigationItem { Title = "Site Homepage", NavigateUrl = "~/Default.aspx", Target = "_top" },
                new NavigationItem { Title = "About the Project", NavigateUrl = "~/en/About.aspx" },
            };
        }
    }
}
