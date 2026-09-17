using BRU.WEBFORMS.ASPNET.APP.Controls;

namespace BRU.WEBFORMS.ASPNET.APP.en.downloads
{
    public partial class IeDownloads : BasePage
    {
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbIntro;

        protected override void LoadPageContent()
        {
            LogPageContentLoad();

            cbIntro.ContentHtml = GetConfiguredContent("cbIntro",
                @"This page lists downloads of Microsoft Internet Explorer that you can use with Windows Update.<br>
                Only Web Installers are listed here, which download the required files from the Windows Update Restored servers.");
        }
    }
}
