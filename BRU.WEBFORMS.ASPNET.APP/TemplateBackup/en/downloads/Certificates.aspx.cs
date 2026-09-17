using BRU.WEBFORMS.ASPNET.APP.Controls;

namespace BRU.WEBFORMS.ASPNET.APP.en.downloads
{
    public partial class Certificates : BasePage
    {
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbIntro;

        protected override void LoadPageContent()
        {
            LogPageContentLoad();

            cbIntro.ContentHtml = GetConfiguredContent("cbIntro",
                "Windows Update Restored uses self-signed certificates to sign patched CABs and installers. This fix removes the certificate warnings in Internet Explorer.");
        }
    }
}
