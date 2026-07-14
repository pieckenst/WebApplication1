using System.Web.UI;

namespace BRU.WEBFORMS.ASPNET.APP
{
    /// <summary>
    /// Default home page for Windows XP styled website template.
    /// </summary>
    public partial class _Default : BasePage
    {
        protected global::System.Web.UI.WebControls.Literal litLastUpdated;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbWelcome;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbAbout;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbFeatures;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbGetStarted;

        protected override void LoadPageContent()
        {
            LogPageContentLoad();

            cbWelcome.ContentHtml = GetConfiguredContent("cbWelcome", @"Welcome to this Windows XP styled website template! 
                This template provides a nostalgic look reminiscent of classic Windows XP/2000 era websites. 
                The layout uses a frame-based design with a top toolbar, navigation sidebar, and main content area.
                <br /><br />
                Feel free to customize the navigation, content, and styling to match your needs.");

            cbAbout.ContentHtml = GetConfiguredContent("cbAbout", @"This template is designed to provide an authentic retro computing aesthetic. 
                Key features include:
                <ul>
                    <li>Windows XP/2000 era styling and colors</li>
                    <li>Frame-based layout with toolbar and sidebar navigation</li>
                    <li>Reusable content box controls</li>
                    <li>Generic and customizable structure</li>
                    <li>ASP.NET Web Forms architecture</li>
                </ul>");

            cbFeatures.ContentHtml = GetConfiguredContent("cbFeatures", @"<b>Template Features:</b>
                <ul class='feature-list'>
                    <li><b>Master Page Layout</b> - Consistent frame-based structure across all pages</li>
                    <li><b>Navigation Control</b> - Configurable sidebar navigation with sections</li>
                    <li><b>Toolbar Control</b> - Logo and branding via Web.config</li>
                    <li><b>Content Box Control</b> - Reusable styled content containers</li>
                    <li><b>Footer Control</b> - Copyright text via Web.config</li>
                    <li><b>Responsive Design</b> - Works on various screen sizes</li>
                </ul>");

            cbGetStarted.ContentHtml = GetConfiguredContent("cbGetStarted", @"<b>To customize this template:</b>
                <ul>
                    <li>Edit <code>Web.config</code> appSettings for logo, site name, footer, and page content</li>
                    <li>Set <code>ContentHtml</code> directly on ContentBox controls in .aspx markup</li>
                    <li>Override any page box via <code>Page.{PageKey}.{ControlId}.ContentHtml</code> in Web.config</li>
                    <li>Modify <code>Navigationbar.ascx.cs</code> to customize navigation items</li>
                    <li>Add new content pages inheriting from <code>BasePage</code></li>
                </ul>
                <br />
                <i>This template is ready for use as a starting point for Windows XP styled websites!</i>");
        }
    }
}
