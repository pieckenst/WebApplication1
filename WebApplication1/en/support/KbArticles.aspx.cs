using BRU.WEBFORMS.ASPNET.APP.Controls;

namespace BRU.WEBFORMS.ASPNET.APP.en.support
{
    public partial class KbArticles : BasePage
    {
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbIntro;

        protected override void LoadPageContent()
        {
            LogPageContentLoad();

            cbIntro.ContentHtml = GetConfiguredContent("cbIntro",
                "This page lists every Support topic we published over time regarding Windows Update Restored.");
        }
    }
}
