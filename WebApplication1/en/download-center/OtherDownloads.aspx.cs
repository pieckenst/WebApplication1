using BRU.WEBFORMS.ASPNET.APP.Controls;

namespace BRU.WEBFORMS.ASPNET.APP.en.download_center
{
    public partial class OtherDownloads : BasePage
    {
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbIntro;

        protected override void LoadPageContent()
        {
            LogPageContentLoad();

            cbIntro.ContentHtml = GetConfiguredContent("cbIntro",
                "<p><b>These are all the available downloads that do not fit in the categories available on the <a href=\"" +
                ResolveUrl("~/en/download-center/Default.aspx") +
                "\">Download Center homepage</a>. Scroll down through the page to find what you're looking for.</b></p>" +
                "<p>The downloads on this page are split into categories. You can read more about them below, or jump directly to the download options at the bottom of the page.</p>");
        }
    }
}
