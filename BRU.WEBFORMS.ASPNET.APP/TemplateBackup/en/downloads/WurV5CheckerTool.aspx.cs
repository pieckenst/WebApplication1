using BRU.WEBFORMS.ASPNET.APP.Controls;

namespace BRU.WEBFORMS.ASPNET.APP.en.downloads
{
    public partial class WurV5CheckerTool : BasePage
    {
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbIntro;

        protected override void LoadPageContent()
        {
            LogPageContentLoad();

            cbIntro.ContentHtml = GetConfiguredContent("cbIntro",
                "The Windows Update Restored Compatibility Checker checks your system to determine if your computer is compatible with Windows Update Restored v5. If not, it will inform you of what is needed to resolve the issue.");
        }
    }
}
