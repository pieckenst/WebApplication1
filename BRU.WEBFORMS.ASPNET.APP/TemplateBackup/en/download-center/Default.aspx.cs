using BRU.WEBFORMS.ASPNET.APP.Controls;

namespace BRU.WEBFORMS.ASPNET.APP.en.download_center
{
    public partial class Default : BasePage
    {
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbIntro;

        protected override void LoadPageContent()
        {
            LogPageContentLoad();

            cbIntro.ContentHtml = GetConfiguredContent("cbIntro", @"<p><b>All the downloads provided here are coming from our servers. They do not contain any sort of virus or malware and are verified before being uploaded to the server and added to the website.</b></p>
                <p>Sometimes, when using Windows Update, you need specific files or set of files. Regarding the Windows Update Restored website, those files are usually very hard to find. We're hosting them and make them publicly available for everyone to keep them safe and always online.</p>
                <p>To browse the Download Center, pick an option among the ones below:</p>");
        }
    }
}
