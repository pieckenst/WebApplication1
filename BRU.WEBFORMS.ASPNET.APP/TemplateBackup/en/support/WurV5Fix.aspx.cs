using BRU.WEBFORMS.ASPNET.APP.Controls;

namespace BRU.WEBFORMS.ASPNET.APP.en.support
{
    public partial class WurV5Fix : BasePage
    {
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbArticle;

        protected override void LoadPageContent()
        {
            LogPageContentLoad();

            cbArticle.ContentHtml = GetConfiguredContent("cbArticle",
                @"<p style=""font-size: 10pt; margin: 10px 0""><b>Article Published: 05/12/2024</b><br>
                <b><i>Updated: 10/30/2025</i></b></p>
                <p style=""font-size: 13px; line-height: 1.6; margin: 10px 0;"">Windows Update Restored V5 and V6 do not work out of the box, and require system modifications. Originally, you would have to manually apply patches that modify your Windows system in order for the Windows Update Restored V5 and V6 websites to function. Since then, times have changed, and we have released an automated tool called the Windows Update Restored Prerequisites Installer. This tool completely automates the process of installing the required prerequisites to run Windows Update Restored V5 and V6 on your Windows system. This tool also works with Microsoft Update Restored.</p>
                <hr style=""border: none; border-top: 1px solid #000080; margin: 10px 0;"">
                <div style=""background-color: #ffcccc; border: 1px solid #cc0000; color: #cc0000; padding: 10px; margin-bottom: 20px; font-size: 13px; border-radius: 5px; box-shadow: 0 0 5px rgba(0, 0, 0, 0.1);""><b>Disclaimer</b><br>
                <span>We are not responsible for anything that goes wrong to your system if an event like that were to occur. Please proceed at your own risk.</span></div>
                <p style=""margin: 10px 0;""><b><span style=""font-size: 10pt"">Notes:</span></b><span style=""font-size: 10pt""><br>
                This only applies to <b>Windows 2000 SP3/SP4, Windows XP x86 &amp; x64, and Windows Server 2003 x86 &amp; x64.</b></span></p>
                <p style=""margin: 10px 0;""><span style=""font-size: 10pt"">To download the Windows Update Restored Prerequisites Installer, <a style=""color: #000080; text-decoration: none;"" href=""" + ResolveUrl("~/en/downloads/WurV5PatcherTool.aspx") + @""">visit its download page</a>.</span></p>
                <hr>
                <div style=""background-color: #f9f9f9; border: 1px solid #dcdcdc; padding: 10px; margin-top: 20px;""><h2 style=""font-size: 11pt; color: #000080; margin-top: 0; margin-bottom: 10px;"">Instructions</h2>
                <p style=""margin: 0;""><span style=""font-size: 10pt"">Follow these steps to restore access to the Windows Update Restored V5 and V6 websites:</span></p>
                <ol style=""margin: 10px 0; padding-left: 20px;""><li style=""margin-bottom: 8px;""><span style=""font-size: 10pt"">Follow the on-screen instructions to install this patch.</span></li>
                <li style=""margin-bottom: 8px;""><span style=""font-size: 10pt"">Click <b>""Cancel""</b> at any <b>""Windows File Protection""</b> popup windows that might appear during installation.</span></li></ol></div>");
        }
    }
}
