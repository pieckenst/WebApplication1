using BRU.WEBFORMS.ASPNET.APP.Controls;

namespace BRU.WEBFORMS.ASPNET.APP.en.support
{
    public partial class Default : BasePage
    {
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbIntro;

        protected override void LoadPageContent()
        {
            LogPageContentLoad();

            cbIntro.ContentHtml = GetConfiguredContent("cbIntro",
                "<p>Welcome to the Windows Update Restored Help and Support Center! This place is ideal if you have trouble getting the Windows Update websites to work. The Knowledge Base (KB) is a great way to find solutions to the problems you encounter. The issues are reported by members, if you are getting an issue with Windows Update Restored that is not in the Knowledge Base, <b><a href=\"" +
                ResolveUrl("~/en/About.aspx") +
                "\">contact us</a></b>.</p>" +
                "<p>Start browsing the Help &amp; Support Center by picking a link among the options below:</p>");
        }
    }
}
