using BRU.WEBFORMS.ASPNET.APP.Controls;

namespace BRU.WEBFORMS.ASPNET.APP.en.support.kb
{
    public partial class WurBlockedByIsp : BasePage
    {
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbArticle;

        protected override void LoadPageContent()
        {
            LogPageContentLoad();

            cbArticle.ContentHtml = GetConfiguredContent("cbArticle",
                @"<p style=""font-size: 10pt; margin: 10px 0;""><b>Article Published: 04/15/2024</b></p>
                <p><font size=""2""><b>Summary</b><br>
                Because our website is still using old Web standards, your ISP might not like it. Here, we're providing you with a temporary fix for your retro machine.<br>
                <br>
                <b>Notes</b><br>
                This fix uses one of our member's proxy servers and captures all your Internet traffic (with your client machine). <u><b>It can be used for Windows Update Restored only.</b></u><br>
                This is a temporary fix for your Windows system.<br>
                <br>
                <b>Instructions</b><br>
                Follow the instructions below to help you temporarily fix your ISP blocking access to Windows Update Restored:</font></p>
                <ol>
                    <li><font size=""2"">In Internet Explorer, go to Tools, Internet Options, Connections tab, LAN Settings, check ""Use a proxy server"", click Advanced.</font></li>
                    <li><font size=""2"">In the dialog box, under ""HTTP"", type serverkul.ddns.net, and for the port, enter 707.</font></li>
                    <li><font size=""2"">Go to the Advanced tab, and check ""Enable HTTP 1.1 through proxy connection"" in the list. Click OK to save your changes.</font></li>
                    <li><font size=""2"">There you go! You should now be able to access Windows Update Restored.</font></li>
                </ol>
                <hr style=""height: 1px; border: 0 none; margin: 10px 0; background: #000080"">
                <p style=""font-size: 13px;"">If you have any more questions or need further assistance, feel free to join our <a target=""_blank"" href=""https://discord.gg/Aqp4GgnWAg"" style=""color: #000080; text-decoration: none;"">Discord server</a> to get help from our members.</p>");
        }
    }
}
