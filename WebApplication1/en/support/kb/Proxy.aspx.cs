using BRU.WEBFORMS.ASPNET.APP.Controls;

namespace BRU.WEBFORMS.ASPNET.APP.en.support.kb
{
    public partial class Proxy : BasePage
    {
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbArticle;

        protected override void LoadPageContent()
        {
            LogPageContentLoad();

            cbArticle.ContentHtml = GetConfiguredContent("cbArticle",
                @"<p style=""font-size: 10pt; margin: 10px 0;""><b>Article Published: 04/15/2024</b></p>
                <p><font size=""2""><b>Summary</b><br>
                Troubleshooting Windows Update can easily become a pain, especially for the v2/v3.1 websites. Using a proxy tool greatly helps, as it gives you what's missing in a second! This tutorial will help you setting up a proxy tool called <b>Fiddler Classic</b> to troubleshoot Windows Update.<br>
                <br>
                <b>Notes</b><br>
                You will need a computer running Windows XP or higher (We recommend that you use Windows 7 or higher) to install Fiddler Classic.<br>
                Fiddler Classic is a free software to download.<br>
                <br>
                <b>Instructions</b><br>
                Follow the instructions below to help you set up Fiddler Classic to troubleshoot Windows Update:</font></p>
                <ol>
                    <li><font size=""2"">Download <b>Fiddler Classic</b> from <a target=""_blank"" href=""https://www.telerik.com/fiddler/fiddler-classic"" style=""color: #000080; text-decoration: none;"">its official website</a> (don't worry, it's free).</font></li>
                    <li><font size=""2"">Install it on a modern machine, running Windows 7/8/10 or 11.</font></li>
                    <li><font size=""2"">Open <b>Fiddler Classic</b>. Go to Tools, Options, Connections tab and check ""Allow remote computers to connect"". Set the port to 8888 and save your changes.</font></li>
                    <li><font size=""2"">On the client machine (running Windows Update), open Internet Explorer.</font></li>
                    <li><font size=""2"">Go to Tools, Internet Options, Connections tab, LAN Settings, check ""Use a proxy server"", click Advanced.</font></li>
                    <li><font size=""2"">In the dialog box, under ""HTTP"", enter the IP address of the machine running Fiddler, and the port (which is 8888 by default). Click OK, OK, <b>but do not close the Internet Options</b>.</font></li>
                    <li><font size=""2"">Go to the Advanced tab, and check ""Enable HTTP 1.1 through proxy connection"" in the list. Click OK to save your changes.</font></li>
                    <li><font size=""2"">There you go! You should now be able to proxy what your client does, and find what's missing for Windows Update!</font></li>
                </ol>
                <hr style=""height: 1px; border: 0 none; margin: 10px 0; background: #000080"">
                <p style=""font-size: 13px;"">If you have any more questions or need further assistance, feel free to join our <a target=""_blank"" href=""https://discord.gg/Aqp4GgnWAg"" style=""color: #000080; text-decoration: none;"">Discord server</a> to get help from our members.</p>");
        }
    }
}
