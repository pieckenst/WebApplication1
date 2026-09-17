using BRU.WEBFORMS.ASPNET.APP.Controls;

namespace BRU.WEBFORMS.ASPNET.APP.en.downloads
{
    public partial class SpCenter : BasePage
    {
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbIntro;

        protected override void LoadPageContent()
        {
            LogPageContentLoad();

            cbIntro.ContentHtml = GetConfiguredContent("cbIntro",
                @"This page consists of Service Pack downloads for Windows.<br>
                <i>Please note that all the downloads we offer here were taken from download.microsoft.com or the Wayback Machine, and are only hosted by Windows Update Restored. We did not modify any file listed here.</i><br>
                <br>
                <b>Caption:<br> </b>&nbsp;-&nbsp;<span style=""color: red;"">Not available:</span> The download is not available because we haven't found the files, or the file was never officially released by Microsoft. You can help us finding these files so we can add the download to the table.<br>
                &nbsp;- SP1 / SP2 / SP3: Click the version you want to download next to its language to download it.&nbsp;<i>For example, clicking SP3 next to French in the Microsoft Windows 2000 table will download the following: Microsoft Windows 2000 SP3 French.</i><br>
                <br>
                <b>Select a&nbsp;</b><b>Service Pack&nbsp;version in the list:</b><br>
                &nbsp;-&nbsp;<a href=""#WIN95"">Microsoft Windows 95 (SP1)</a><br>
                &nbsp;-&nbsp;<a href=""#WINNT40"">Microsoft Windows NT 4.0 (SP1 | SP2 | SP3 | SP4 | SP5 | SP6a)</a><br>
                &nbsp;-&nbsp;<a href=""#WIN98"">Microsoft Windows 98 (RTM | Y2K Update 1 | Y2K Update 2)</a><br>
                &nbsp;-&nbsp;<a href=""#WIN2K"">Microsoft Windows 2000 (SP1 | SP2 | SP3 | SP4 | UR1)</a><br>
                &nbsp;-&nbsp;<a href=""#WINXP"">Microsoft Windows XP x86 (SP1 | SP2 | SP3)</a><br>
                &nbsp;- <a href=""#WINXP64"">Microsoft Windows XP AMD64 &amp; IA-64 (SP2)</a><br>");
        }
    }
}
