using BRU.WEBFORMS.ASPNET.APP.Controls;

namespace BRU.WEBFORMS.ASPNET.APP.en.downloads
{
    public partial class WurInvChecker : BasePage
    {
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbIntro;

        protected override void LoadPageContent()
        {
            LogPageContentLoad();

            cbIntro.ContentHtml = GetConfiguredContent("cbIntro",
                @"The Windows Update Restored Inventory Checker Tool (made using Visual Basic 5.0) checks your system for original Windows Update V2/V3.1, V4 and Office Update inventories, and backs them up when needed. Download this tool now and help us save the original Windows Update experience! This tool is small enough for you to transfer it on your target computer(s) using a floppy disk!<br>
                <br>
                <i>Getting a VB5FR.DLL error? Look at the Known Issues section at the bottom of this page!</i>");
        }
    }
}
