using System.Web.UI;
using BRU.WEBFORMS.ASPNET.APP;

namespace BRU.WEBFORMS.ASPNET.APP.en
{
    public partial class Donations : BasePage
    {
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbSupport;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbHowToHelp;

        protected override void LoadPageContent()
        {
            LogPageContentLoad();

            cbSupport.ContentHtml = GetConfiguredContent("cbSupport", @"This template is provided free of charge. If you find it useful for your projects, 
                consider making a donation to support continued development and maintenance.
                <br /><br />
                Your support helps cover hosting costs and allows us to dedicate more time to improving the template.");

            cbHowToHelp.ContentHtml = GetConfiguredContent("cbHowToHelp", @"Not able to donate? There are other ways you can help:
                <ul>
                    <li><b>Spread the word</b> - Share this template with others who might find it useful</li>
                    <li><b>Report bugs</b> - Help us improve by reporting any issues you encounter</li>
                    <li><b>Contribute code</b> - Submit improvements and new features</li>
                    <li><b>Provide feedback</b> - Let us know how we can make this template better</li>
                </ul>");
        }
    }
}
