<%@ Page Title="Windows Update Restored Download Center" Language="C#" MasterPageFile="~/en/download-center/DownloadCenter.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.en.download_center.Default" %>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        .dc-page { color: #000000; font-family: Tahoma, Verdana, Arial, sans-serif; font-size: 13px; }
        .dc-page a { color: #000080; text-decoration: none; }
        .dc-page a:hover { color: #FF6600; text-decoration: underline; }
        .dc-page .title-main { font-size: 20px; color: #000080; font-weight: bold; }
        .dc-page .section-header { font-size: 14px; color: #FFFFFF; font-weight: bold; }
        .dc-page .nav-link-title { font-size: 14px; color: #000080; font-weight: bold; }
        .dc-page .text-small { font-size: 11px; }
        .dc-page p { line-height: 1.4; margin: 10px 0; font-size: 13px; }
        .dc-page .news-block { font-size: 13px; line-height: 1.4; }
    </style>

    <div class="dc-page">
    <a name="top"></a>

    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top" style="padding: 10px;">

                <div class="title-main">Download Center</div>

                <table width="100%" border="0" cellpadding="0" cellspacing="0" style="margin-top: 5px; margin-bottom: 12px;">
                    <tr><td height="2" bgcolor="#000080"></td></tr>
                </table>

                <p><b>All the downloads provided here are coming from our servers. They do not contain any sort of virus or malware and are verified before being uploaded to the server and added to the website.</b></p>

                <p>Sometimes, when using Windows Update, you need specific files or set of files. Regarding the Windows Update Restored website, those files are usually very hard to find. We're hosting them and make them publicly available for everyone to keep them safe and always online.</p>

                <p>To browse the Download Center, pick an option among the ones below:</p>

                <table border="0" cellpadding="0" cellspacing="0"><tr><td height="12"></td></tr></table>

                <table width="100%" border="0" cellpadding="4" cellspacing="0" bgcolor="#000080">
                    <tr>
                        <td class="section-header">&nbsp;Available Repositories</td>
                    </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="0"><tr><td height="12"></td></tr></table>

                <table width="100%" border="0" cellpadding="5" cellspacing="0">
                    <tr>
                        <td align="center" valign="top" width="33%">
                            <a target="_blank" href="<%= SiteConfig.PublicDatabaseUrl %>"><img src='<%= ResolveUrl("~/en/images/windows_xp_flag.gif") %>' alt="WUR Updates" border="0" width="80"></a><br><br>
                            <a target="_blank" href="<%= SiteConfig.PublicDatabaseUrl %>" class="nav-link-title">Updates Database</a><br>
                            <div style="line-height: 1.3; margin-top: 8px; font-size: 12px;">Access every single update we host. All files are free, virus-free, and signed by Microsoft.</div>
                        </td>

                        <td align="center" valign="top" width="33%">
                            <a target="_blank" href="<%= SiteConfig.LegacyUpdateUrl %>"><img src='<%= ResolveUrl("~/en/images/windows_app.gif") %>' alt="Legacy Update" border="0" width="80"></a><br><br>
                            <a target="_blank" href="<%= SiteConfig.LegacyUpdateUrl %>" class="nav-link-title">Legacy Update Download Center</a><br>
                            <div style="line-height: 1.3; margin-top: 8px; font-size: 12px;">A powerful archive of the old Microsoft Download Center. Great for finding specific software!</div>
                        </td>

                        <td align="center" valign="top" width="34%">
                            <a href='<%= ResolveUrl("~/en/download-center/OtherDownloads.aspx") %>'><img src='<%= ResolveUrl("~/en/images/windows_tool.gif") %>' alt="Other Downloads" border="0" width="80"></a><br><br>
                            <a href='<%= ResolveUrl("~/en/download-center/OtherDownloads.aspx") %>' class="nav-link-title">Other Downloads</a><br>
                            <div style="line-height: 1.3; margin-top: 8px; font-size: 12px;">Browse IE versions, Service Packs, and other essential tools for your retro systems.</div>
                        </td>
                    </tr>
                </table>

                <table border="0" cellpadding="0" cellspacing="0"><tr><td height="8"></td></tr></table>

                <p align="right" style="margin-top: 5px;"><a href="#top" class="text-small">Back to top &#9650;</a></p>

                <table border="0" cellpadding="0" cellspacing="0"><tr><td height="12"></td></tr></table>

                <a name="news"></a>
                <table width="100%" border="0" cellpadding="4" cellspacing="0" bgcolor="#000080">
                    <tr>
                        <td class="section-header">&nbsp;Download Center News!</td>
                    </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="0"><tr><td height="12"></td></tr></table>

                <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#5E7DD8">
                    <tr>
                        <td bgcolor="#FFFFFF">
                            <table width="100%" border="0" cellpadding="12" cellspacing="0"><tr><td>
                                <div class="news-block">
                                    <b><img src='<%= ResolveUrl("~/en/images/warning_icon.gif") %>' width="16" height="16" align="absmiddle" style="margin-right: 5px;"> The newly revamped Download Center is here!</b>
                                    <table border="0" cellpadding="0" cellspacing="0"><tr><td height="4"></td></tr></table>
                                    We have released the new and improved layout for the Download Center! Our repositories and links are now easier to browse, providing a much more cohesive user experience while maintaining absolute compatibility with older versions of Internet Explorer. Check out the options above to explore our available repositories!

                                    <table border="0" cellpadding="0" cellspacing="0"><tr><td height="18"></td></tr></table>

                                    <b><img src='<%= ResolveUrl("~/en/images/warning_icon.gif") %>' width="16" height="16" align="absmiddle" style="margin-right: 5px;"> The Prerequisites Installer v1.7.1 is now out!</b>
                                    <table border="0" cellpadding="0" cellspacing="0"><tr><td height="4"></td></tr></table>
                                    We've released the latest version of the Prerequisites Installer, which is version 1.7.1. This new releases brings bug fixes, enhancements and new command line switches to use in automated deployments. It also fixes problems with Microsoft Update Restored. <a href='<%= ResolveUrl("~/en/downloads/WurV5PatcherTool.aspx") %>'>Get it from its Download Center page!</a>

                                    <table border="0" cellpadding="0" cellspacing="0"><tr><td height="18"></td></tr></table>

                                    <b><img src='<%= ResolveUrl("~/en/images/warning_icon.gif") %>' width="16" height="16" align="absmiddle" style="margin-right: 5px;"> The Inventory Checker is now fully English and has issues fixed!</b>
                                    <table border="0" cellpadding="0" cellspacing="0"><tr><td height="4"></td></tr></table>
                                    We have released version 1.0.4 of the Inventory Checker! This new version removes all legacy French strings and migrates the codebase entirely to VB6, completely removing the need for the `VB5FR.DLL` runtime library that was causing issues on classic setups. The software operates identically to previous releases and contains no new command line arguments. <a href='<%= ResolveUrl("~/en/downloads/WurInvChecker.aspx") %>'><b>View the tool page here!</b></a>
                                </div>
                            </td></tr></table>
                        </td>
                    </tr>
                </table>

                <p align="right" style="margin-top: 5px;"><a href="#top" class="text-small">Back to top &#9650;</a></p>

                <table border="0" cellpadding="0" cellspacing="0"><tr><td height="15"></td></tr></table>

            </td>
        </tr>
    </table>
    </div>

</asp:Content>
