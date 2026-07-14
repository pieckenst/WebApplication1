using System.Web.UI;
using BRU.WEBFORMS.ASPNET.APP;

namespace BRU.WEBFORMS.ASPNET.APP.en
{
    public partial class ReadThisFirst : BasePage
    {
        protected global::System.Web.UI.WebControls.Literal litLastUpdated;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbTOC;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbDisclaimer;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbWelcome;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbFeatures;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbStarted;

        protected override void LoadPageContent()
        {
            cbTOC.ContentHtml = GetConfiguredContent("cbTOC", @"
                <ul class='toc-list'>
                    <li><b><a href='#disclaimer'>Disclaimer and Important Notices</a></b></li>
                    <li><b><a href='#welcome'>Welcome!</a></b></li>
                    <li><b><a href='#features'>Key Features</a></b></li>
                    <li><b><a href='#started'>Getting Started</a></b></li>
                </ul>");

            cbDisclaimer.ContentHtml = GetConfiguredContent("cbDisclaimer", @"
                <font color='#660000'>
                    <b>This template is provided ""as is"", without warranty of any kind. 
                    While we've tested it thoroughly, please ensure you have backups of any important data. 
                    This template is for educational and personal use.</b>
                </font>");

            cbWelcome.ContentHtml = GetConfiguredContent("cbWelcome", @"
                Thank you for using this Windows XP styled website template! 
                This template is designed to help developers create retro-styled websites with an authentic 
                Windows XP/2000 era look and feel.
                <br /><br />
                The template uses ASP.NET Web Forms with master pages, user controls, and CSS to recreate 
                the classic frame-based layout that was common in early 2000s websites.");

            cbFeatures.ContentHtml = GetConfiguredContent("cbFeatures", @"
                <b>Template Features:</b>
                <ul>
                    <li><b>Master Page Layout</b> - Consistent frame-based structure</li>
                    <li><b>Navigation Control</b> - Configurable sidebar navigation</li>
                    <li><b>Content Boxes</b> - Reusable styled content containers</li>
                    <li><b>Retro Styling</b> - Authentic Windows XP aesthetic</li>
                    <li><b>Responsive Design</b> - Works on modern devices</li>
                    <li><b>Easy Customization</b> - Web.config and markup overrides</li>
                </ul>");

            cbStarted.ContentHtml = GetConfiguredContent("cbStarted", @"
                To get started with this template:
                <br /><br />
                <b>1.</b> Open the project in Visual Studio
                <br />
                <b>2.</b> Build and run the project
                <br />
                <b>3.</b> Customize logo and site name in <code>Web.config</code>
                <br />
                <b>4.</b> Add your content pages using the master page and <code>BasePage</code>
                <br />
                <b>5.</b> Modify the CSS to match your branding
                <br /><br />
                <i>Refer to the FAQ page for more detailed instructions!</i>");
        }
    }
}
