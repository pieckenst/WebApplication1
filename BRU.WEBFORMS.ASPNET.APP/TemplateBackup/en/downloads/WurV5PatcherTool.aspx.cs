using BRU.WEBFORMS.ASPNET.APP.Controls;

namespace BRU.WEBFORMS.ASPNET.APP.en.downloads
{
    public partial class WurV5PatcherTool : BasePage
    {
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbIntro;

        protected override void LoadPageContent()
        {
            LogPageContentLoad();

            cbIntro.ContentHtml = GetConfiguredContent("cbIntro",
                @"The Prerequisites Installer automatically configures and updates your computer so that Windows Update Restored V5, V6, and Microsoft Update work correctly.
                <br><br>
                <span class=""text-black text-small""><b>Published: 03/24/2024</b></span>");
        }
    }
}
