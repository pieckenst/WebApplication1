using BRU.WEBFORMS.ASPNET.APP.Controls;

namespace BRU.WEBFORMS.ASPNET.APP.en.downloads
{
    public partial class SpCenter_Legacy : BasePage
    {
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbIntro;

        protected override void LoadPageContent()
        {
            LogPageContentLoad();

            cbIntro.ContentHtml = GetConfiguredContent("cbIntro",
                @"This page consists of Service Pack downloads for Windows.<br>
                <i>Please note that all the downloads we offer here were taken from download.microsoft.com or the Wayback Machine, and are only hosted by Windows Update Restored. We did not modify any file listed here.</i><br>
                <br>
                <b>Caption:<br>
                </b>&nbsp;-&nbsp;Not available: The download is not available because we haven't found the files, or the file was never officially released by Microsoft. You can help us finding these files so we can add the download to the table.<br>
                &nbsp;- SP1 / SP2 / SP3: Click the version you want to download next to its language to download it.&nbsp;<i>For example, clicking SP3 next to French in the Microsoft Windows 2000 table will download the following: Microsoft Windows 2000 SP3 French.</i>");
        }
    }
}
