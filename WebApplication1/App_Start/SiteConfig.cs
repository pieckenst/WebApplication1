using System.Configuration;

namespace BRU.WEBFORMS.ASPNET.APP
{
    /// <summary>
    /// Site configuration helper class that reads settings from Web.config.
    /// All site-wide settings can be configured in the appSettings section.
    /// </summary>
    public static class SiteConfig
    {
        /// <summary>
        /// Gets the site name displayed in navigation and titles.
        /// </summary>
        public static string SiteName
        {
            get { return GetSetting("SiteName", "Windows XP Styled Website"); }
        }

        /// <summary>
        /// Gets the home page URL (virtual path).
        /// </summary>
        public static string HomeUrl
        {
            get { return GetSetting("SiteHomeUrl", "~/Default.aspx"); }
        }

        /// <summary>
        /// Gets the logo image URL (virtual path).
        /// </summary>
        public static string LogoUrl
        {
            get { return GetSetting("SiteLogoUrl", "~/en/images/mstoolbar_icp.gif"); }
        }

        /// <summary>
        /// Gets the logo image width in pixels.
        /// </summary>
        public static int LogoWidth
        {
            get
            {
                string val = GetSetting("SiteLogoWidth", "438");
                int width;
                return int.TryParse(val, out width) ? width : 438;
            }
        }

        /// <summary>
        /// Gets the logo image height in pixels.
        /// </summary>
        public static int LogoHeight
        {
            get
            {
                string val = GetSetting("SiteLogoHeight", "81");
                int height;
                return int.TryParse(val, out height) ? height : 81;
            }
        }

        /// <summary>
        /// Gets the logo alt text.
        /// </summary>
        public static string LogoAltText
        {
            get { return GetSetting("SiteLogoAlt", "Home"); }
        }

        /// <summary>
        /// Gets the copyright text for the footer.
        /// </summary>
        public static string CopyrightText
        {
            get { return GetSetting("CopyrightText", "© 2026 - My Website"); }
        }

        /// <summary>
        /// Gets the default page title.
        /// </summary>
        public static string DefaultPageTitle
        {
            get { return GetSetting("DefaultPageTitle", "Welcome - Windows XP Styled Website"); }
        }

        /// <summary>
        /// Gets a page-specific title override from Web.config.
        /// Key format: Page.{pageKey}.Title
        /// </summary>
        public static string GetPageTitle(string pageKey)
        {
            return GetSetting(BuildPageKey(pageKey, null, "Title"), null);
        }

        /// <summary>
        /// Gets a page control property override from Web.config.
        /// Key format: Page.{pageKey}.{controlId}.{propertyName}
        /// </summary>
        public static string GetPageSetting(string pageKey, string controlId, string propertyName, string defaultValue = null)
        {
            return GetSetting(BuildPageKey(pageKey, controlId, propertyName), defaultValue);
        }

        /// <summary>
        /// Gets ContentHtml for a ContentBox on a page, if configured in Web.config.
        /// </summary>
        public static string GetContentHtml(string pageKey, string controlId)
        {
            return GetPageSetting(pageKey, controlId, "ContentHtml");
        }

        /// <summary>
        /// Gets HeaderText for a ContentBox on a page, if configured in Web.config.
        /// </summary>
        public static string GetHeaderText(string pageKey, string controlId)
        {
            return GetPageSetting(pageKey, controlId, "HeaderText");
        }

        /// <summary>
        /// Gets text for a Literal control on a page, if configured in Web.config.
        /// </summary>
        public static string GetLiteralText(string pageKey, string controlId)
        {
            return GetPageSetting(pageKey, controlId, "Text");
        }

        private static string BuildPageKey(string pageKey, string controlId, string propertyName)
        {
            if (string.IsNullOrEmpty(controlId))
                return "Page." + pageKey + "." + propertyName;

            return "Page." + pageKey + "." + controlId + "." + propertyName;
        }

        /// <summary>
        /// Gets a setting value from Web.config appSettings.
        /// </summary>
        private static string GetSetting(string key, string defaultValue)
        {
            string value = ConfigurationManager.AppSettings[key];
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }
    }
}
