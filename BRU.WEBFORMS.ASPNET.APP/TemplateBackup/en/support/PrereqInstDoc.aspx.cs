using BRU.WEBFORMS.ASPNET.APP.Controls;

namespace BRU.WEBFORMS.ASPNET.APP.en.support
{
    public partial class PrereqInstDoc : BasePage
    {
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbIntro;

        protected override void LoadPageContent()
        {
            LogPageContentLoad();

            cbIntro.ContentHtml = GetConfiguredContent("cbIntro",
                @"Thank you for using the Windows Update Restored Prerequisites Installer! This page is the short online documentation available for the Prerequisites Installer.<br>
                <br>
                Take a time to explore what we have here:");
        }
    }
}
