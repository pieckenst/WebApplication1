<%@ Page Title="Windows Update Restored Prerequisites Installer" Language="C#" MasterPageFile="~/en/download-center/DownloadCenter.master" AutoEventWireup="true" CodeBehind="WurV5PatcherTool.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.en.downloads.WurV5PatcherTool" %>
<%@ Register TagPrefix="uc" TagName="ContentBox" Src="~/Controls/ContentBox.ascx" %>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        .cw, .cw table, .cw td, .cw p, .cw div, .cw span, .cw ul, .cw li, .cw ol {font-family: Tahoma, Verdana, Arial, sans-serif; 
            font-size: 10pt; /* Global reset prevents default browser text inflation */ }
        .cw {margin: 0; 
            padding: 0; 
            background-color: #FFFFFF; 
            color: #000000; }
        .cw a {color: #000080; text-decoration: none; }
        .cw a:hover {color: #FF6600; text-decoration: underline; }
        .cw ul {margin-top: 5px; margin-bottom: 5px; padding-left: 18px; }
        .cw li {margin-bottom: 8px; font-size: 10pt; }
        .cw ol {margin-top: 5px; margin-bottom: 5px; padding-left: 20px; }
        .cw ol li {margin-bottom: 6px; font-size: 10pt; }
        .cw .title-main {font-size: 16pt; color: #000080; font-weight: bold; }
        .cw .text-regular {font-size: 10pt; line-height: 1.5; }
        .cw .text-small {font-size: 8pt; }
        .cw .text-gray {color: #666666; }
        .cw .text-black {color: #000000; }
        .cw a.text-black {color: #000000; }
        .cw .box-header-white {font-size: 10pt; color: #FFFFFF; font-weight: bold; }
        .cw .box-header-small {font-size: 8pt; color: #FFFFFF; font-weight: bold; }
    </style>

    <div class="cw">


<a name="top"></a>

<table width="100%" border="0" cellpadding="10" cellspacing="0">
    <tr>
        <td valign="top">
            
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="bottom" class="title-main">
                        Windows Update Restored Prerequisites Installer
                    </td>
                </tr>
            </table>
            
            <table width="100%" border="0" cellpadding="0" cellspacing="0" style="margin-top: 5px; margin-bottom: 15px;">
                <tr><td height="2" bgcolor="#000080"></td></tr>
            </table>

            <uc:ContentBox ID="cbIntro" runat="server" HeaderText="About the Prerequisites Installer" HeaderColor="Blue" ContentColor="White" />

            <table border="0" cellpadding="0" cellspacing="0"><tr><td height="15"></td></tr></table>

            <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#CC9999">
                <tr>
                    <td bgcolor="#FFE6E6">
                        <table width="100%" border="0" cellpadding="10" cellspacing="0">
                            <tr>
                                <td class="text-regular">
                                    <font color="#660000"><b>Disclaimer:</b> We are not responsible for anything that goes wrong to your system if an event like that were to occur. Please proceed at your own risk.</font>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <table border="0" cellpadding="0" cellspacing="0"><tr><td height="20"></td></tr></table>

            <table width="100%" border="0" cellpadding="4" cellspacing="0" bgcolor="#000080">
                <tr>
                    <td class="box-header-white">&nbsp;File Information</td>
                </tr>
            </table>
            <table width="100%" border="0" cellpadding="10" cellspacing="0" style="border: 1px solid #808080; border-top: none;">
                <tr>
                    <td class="text-regular">
                        <p style="margin-top: 0; margin-bottom: 8px;"><strong>File name:</strong> wurv5patchertool.exe</p>
                        <p style="margin-top: 0; margin-bottom: 8px;"><strong>File size:</strong> 23.9 MB</p>
                        <p style="margin-top: 0; margin-bottom: 8px;"><strong>Software version:</strong> v1.7 (Release)</p>
                        <p style="margin-top: 0; margin-bottom: 8px;"><strong>Developer:</strong> WULover (helped by WinFX, Joshua Maitland, TheOneGoofAli, Trisii, Johnmacro, ward201185 and Retro Circutry)</p>
                        <p style="margin-top: 0; margin-bottom: 8px;"><strong>Download server:</strong> <a href="<%= SiteConfig.DownloadBaseUrl %>/" target="_blank"><b><u><%= SiteConfig.DownloadBaseUrl %></u></b></a></p>
                        <p style="margin-top: 0; margin-bottom: 8px;"><strong>Digital signature:</strong> Signed on May 13th, 2026 by windowsupdaterestored.com</p>
                        <p style="margin-top: 0; margin-bottom: 0;"><strong>Additional information:</strong> Windows Update Restored server provided by TheOneGoofAli (serverkul.ddns.net), and Legacy Update server provided by kirb (legacyupdate.net). This patch only applies for Windows Update Restored V5 and V6 (Microsoft Update included).<br>Windows Update Restored cannot be held responsible if the custom Update Provider you chose caused problems to your system.</p>
                    </td>
                </tr>
            </table>

            <table border="0" cellpadding="0" cellspacing="0"><tr><td height="20"></td></tr></table>

            <table width="100%" border="0" cellpadding="4" cellspacing="0" bgcolor="#666666">
                <tr>
                    <td class="box-header-white">&nbsp;Downloads</td>
                </tr>
            </table>
            <table width="100%" border="0" cellpadding="10" cellspacing="0" style="border: 1px solid #808080; border-top: none;">
                <tr>
                    <td bgcolor="#F2F2F2" class="text-regular">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td valign="middle" width="35">
                                    <a href="<%= SiteConfig.DownloadBaseUrl %>/wutools/wurv5patchertool.EXE"><img border="0" src="<%= ResolveUrl("~/en/images/arrow.gif") %>" width="30" height="25" alt="Download"></a>
                                </td>
                                <td valign="middle">
                                    <span style="font-weight: 700;"><a href="<%= SiteConfig.DownloadBaseUrl %>/wutools/wurv5patchertool.EXE"><b><u>Download now (Stable) - Recommended</u></b></a></span>
                                </td>
                            </tr>
                            <tr><td height="10" colspan="2"></td></tr>
                            <tr>
                                <td valign="middle" width="35">
                                    <a href="<%= SiteConfig.DownloadBaseUrl %>/wutools/wurv5patchertool_legacy.EXE"><img border="0" src="<%= ResolveUrl("~/en/images/arrow.gif") %>" width="30" height="25" alt="Download"></a>
                                </td>
                                <td valign="middle">
                                    <span style="font-weight: 700;"><a href="<%= SiteConfig.DownloadBaseUrl %>/wutools/wurv5patchertool_legacy.EXE"><b><u>Download now (Legacy - Batch)</u></b></a></span>
                                </td>
                            </tr>
                        </table>
                        <p style="margin-top: 10px; margin-bottom: 0;" class="text-small">
                            <i>The legacy version of this patch has known issues when running on x64 systems. We strongly recommend that you use the Stable release. This version does not support Windows Update Restored V6.</i>
                            <br>
                            <font color="#CC0000"><b>This version is no longer maintained</b></font> by Windows Update Restored, and is offered here for archiving purposes only.
                        </p>
                    </td>
                </tr>
            </table>

            <table border="0" cellpadding="0" cellspacing="0"><tr><td height="20"></td></tr></table>

            <table width="100%" border="0" cellpadding="4" cellspacing="0" bgcolor="#000080">
                <tr>
                    <td class="box-header-white">&nbsp;System Requirements</td>
                </tr>
            </table>
            <table width="100%" border="0" cellpadding="10" cellspacing="0" style="border: 1px solid #808080; border-top: none;">
                <tr>
                    <td class="text-regular">
                        Compatible with the following operating systems (including multi-language and x64 system editions, excluding IA-64):
                        <ul style="margin-top: 10px;">
                            <li><b>Windows XP</b></li>
                            <li><b>Windows Server 2003</b></li>
                            <li><b>Windows 2000 with Service Pack 4</b></li>
                            <li><b>Windows 2000 with Service Pack 3</b></li>
                        </ul>
                    </td>
                </tr>
            </table>

            <table border="0" cellpadding="0" cellspacing="0"><tr><td height="20"></td></tr></table>

            <table width="100%" border="0" cellpadding="4" cellspacing="0" bgcolor="#666666">
                <tr>
                    <td class="box-header-white">&nbsp;Instructions</td>
                </tr>
            </table>
            <table width="100%" border="0" cellpadding="10" cellspacing="0" style="border: 1px solid #808080; border-top: none;">
                <tr>
                    <td bgcolor="#F2F2F2" class="text-regular">
                        Follow these steps to install the Windows Update Restored Prerequisites Installer:
                        <br><br>
                        <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#DCDCDC">
                            <tr>
                                <td bgcolor="#FFFFFF">
                                    <table width="100%" border="0" cellpadding="10" cellspacing="0">
                                        <tr>
                                            <td class="text-regular">
                                                <ol style="margin-top: 0; margin-bottom: 0;">
                                                    <li>Follow the <b>on-screen instructions</b> to install this patch.</li>
                                                    <li>Click "<b>Cancel</b>" at any "Windows File Protection" popup windows that might appear during installation.</li>
                                                </ol>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <table border="0" cellpadding="0" cellspacing="0"><tr><td height="20"></td></tr></table>

            <table width="100%" border="0" cellpadding="4" cellspacing="0" bgcolor="#000080">
                <tr>
                    <td class="box-header-white">&nbsp;Known Issues</td>
                </tr>
            </table>
            <table width="100%" border="0" cellpadding="10" cellspacing="0" style="border: 1px solid #808080; border-top: none;">
                <tr>
                    <td class="text-regular">
                        <p style="margin-top: 0; margin-bottom: 5px;"><font color="#800000"><strong>Stable Version:</strong></font></p>
                        <ul>
                            <li><strong>Error with MSCOMTCL.OCX not being present:</strong><br>If you see an error stating that "MSCOMTCL.OCX" is missing on Windows 2000/XP/Server 2003, download and install the <strong>Visual Basic 6.0 runtime</strong> <a href="<%= SiteConfig.DownloadBaseUrl %>/wutools/VB6Runtime.exe"><b><u>from here</u></b></a> to resolve this. The patcher automatically installs this file onto your system.</li>
                            <li><b>Microsoft Update errors out on Windows 2000:</b><br>Microsoft Update requires <b>Internet Explorer 5.5</b> at minimum to work. <b>Any previous versions will cause the website to fail.</b></li>
                            <li><b>Windows Update/Microsoft Update errors out before displaying Express/Custom, and Internet Explorer asks to install an ActiveX control signed by "windowsupdaterestored.com":</b><br>This happens because your computer does not have the Windows Update Restored Server Web Control (WUR_ACTIVEX.DLL), which is required to use the website. To fix it, download the latest stable version of the patcher and follow the instructions below:<br>- Click the bottom left "About" link<br>- In the "About" dialog box, click the "Manual Tools" button<br>- Click on the "Reinstall WUR_ACTIVEX.DLL" button<br>Done! The control has been reinstalled and registered onto your computer.</li>
                        </ul>

                        <table border="0" cellpadding="0" cellspacing="0"><tr><td height="15"></td></tr></table>

                        <p style="margin-top: 0; margin-bottom: 5px;"><font color="#800000"><strong>Legacy Version:</strong></font></p>
                        <ul>
                            <li><strong>Site loading issues on Windows XP/Server 2003 x64:</strong><br>If the site gets stuck at "Checking for the latest version," clear your Internet Temporary Files:
                                <ol style="margin-top: 5px; margin-bottom: 5px;">
                                    <li>Go to <strong>Tools &gt; Internet Options.</strong></li>
                                    <li>Click on <strong>"Delete files."</strong></li>
                                    <li>Check <strong>"Delete all offline content"</strong> and click <strong>OK.</strong></li>
                                </ol>
                                You can also hit Ctrl + F5 in Internet Explorer.
                            </li>
                            <li><strong>Windows Update stuck on "Windows Update is looking for available updates" on Windows 2000:</strong><br>This occurs if you're using <strong>Internet Explorer 5.</strong> Upgrade to <strong>IE 5.5 or 6.0.</strong></li>
                            <li><strong>Error after checking for updates:</strong><br>If you get an error after clicking "Express" or "Custom," ensure <strong>WUWEB.DLL</strong> has been correctly replaced. Run the patcher again if necessary.</li>
                            <li><strong>Website load error on Windows XP/Server 2003 AMD64:</strong><br>Use the <strong>Stable release</strong> to avoid x64-related issues.</li>
                        </ul>
                    </td>
                </tr>
            </table>

            <p align="right" style="margin-top: 20px;"><a href="#top" class="text-small text-black">Back to top &#9650;</a></p>

        </td>
    </tr>
</table>


    </div>

</asp:Content>
