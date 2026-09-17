using BRU.WEBFORMS.ASPNET.APP.Controls;

namespace BRU.WEBFORMS.ASPNET.APP.en.support
{
    public partial class KnownIssues : BasePage
    {
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbIntro;

        protected override void LoadPageContent()
        {
            LogPageContentLoad();

            cbIntro.ContentHtml = GetConfiguredContent("cbIntro",
                @"While we try to provide the best possible experience, it is important to clarify that Windows Update Restored is not perfect, and users may encounter certain known issues. Below are outlined ongoing challenges associated with each version, along with specific caveats:
                <br><br>
                <span class=""text-black text-small""><b>Last Updated: 05/14/2026</b></span>");
        }
    }
}
