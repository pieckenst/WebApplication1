<%@ Page Title="Known Issues" Language="C#" MasterPageFile="~/en/support/Support.master" AutoEventWireup="true" CodeBehind="KnownIssues.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.en.support.KnownIssues" %>
<%@ Register TagPrefix="uc" TagName="ContentBox" Src="~/Controls/ContentBox.ascx" %>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        .cw, .cw table, .cw td, .cw p, .cw div, .cw span, .cw ul, .cw li { font-family: Tahoma, Verdana, Arial, sans-serif; }
        .cw { margin: 0; padding: 0; background-color: #FFFFFF; color: #000000; }
        .cw a { color: #000080; text-decoration: none; }
        .cw a:hover { color: #FF6600; text-decoration: underline; }
        .cw ul { margin-top: 5px; margin-bottom: 5px; padding-left: 18px; }
        .cw li { margin-bottom: 8px; font-size: 11px; }
        .cw .title-main { font-size: 16pt; color: #000080; font-weight: bold; }
        .cw .text-regular { font-size: 10pt; line-height: 1.5; }
        .cw .text-small { font-size: 8pt; }
        .cw .text-gray { color: #666666; }
        .cw .text-black { color: #000000; }
        .cw a.text-black { color: #000000; }
        .cw .box-header-white { font-size: 10pt; color: #FFFFFF; font-weight: bold; }
        .cw .box-header-small { font-size: 8pt; color: #FFFFFF; font-weight: bold; }
    </style>

    <div class="cw">

        <a name="top"></a>

        <table width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td valign="bottom" class="title-main">
                    Known Issues
                </td>
            </tr>
        </table>

        <table width="100%" border="0" cellpadding="0" cellspacing="0" style="margin-top: 5px; margin-bottom: 15px;">
            <tr><td height="2" bgcolor="#000080"></td></tr>
        </table>

        <uc:ContentBox ID="cbIntro" runat="server" HeaderText="Overview" HeaderColor="Blue" ContentColor="White" />

        <table border="0" cellpadding="0" cellspacing="0"><tr><td height="15"></td></tr></table>

        <table width="100%" border="0" cellpadding="4" cellspacing="0" bgcolor="#000080">
            <tr>
                <td class="box-header-white">&nbsp;Windows Update Restored V2</td>
            </tr>
        </table>
        <table width="100%" border="0" cellpadding="10" cellspacing="0" style="border: 1px solid #808080; border-top: none;">
            <tr>
                <td class="text-regular">
                    <ul>
                        <li><b>Download Issues:</b> Some updates may not download due to missing files (CIFs/packages).</li>
                        <li><b>Driver Updates:</b> Currently, driver updates may not show any available updates.</li>
                        <li><b>Multilanguage Support:</b> We are missing many inventories necessary for multi-language support, which might result in a "Cannot Display Page" message when checking for updates.</li>
                    </ul>
                </td>
            </tr>
        </table>

        <table border="0" cellpadding="0" cellspacing="0"><tr><td height="20"></td></tr></table>

        <table width="100%" border="0" cellpadding="4" cellspacing="0" bgcolor="#666666">
            <tr>
                <td class="box-header-white">&nbsp;Windows Update Restored V3.1</td>
            </tr>
        </table>
        <table width="100%" border="0" cellpadding="10" cellspacing="0" style="border: 1px solid #808080; border-top: none;">
            <tr>
                <td bgcolor="#F2F2F2" class="text-regular">
                    <ul>
                        <li><b>ActiveX Installation Conflicts:</b> For Windows 98 First Edition, Second Edition, Windows Millennium Edition (Windows ME), and Windows 2000 systems, installing Windows Update v4 ActiveX Controls will disable the use of Windows Update v3.1.</li>
                        <li><b>Windows 2000 Compatibility:</b> Checking for updates does not work on Windows 2000 with Internet Explorer 6, as we are missing required inventories. This will lead to an error during the update process.</li>
                        <li><b>Multilanguage Support:</b> We are missing many inventories for multi-language support, which might trigger "Cannot Display Page" notifications.</li>
                        <li><b>Driver Updates:</b> Driver updates may not show any updates, and certain updates may not download due to missing files (CIFs/packages).</li>
                    </ul>
                </td>
            </tr>
        </table>

        <table border="0" cellpadding="0" cellspacing="0"><tr><td height="20"></td></tr></table>

        <table width="100%" border="0" cellpadding="4" cellspacing="0" bgcolor="#000080">
            <tr>
                <td class="box-header-white">&nbsp;Windows Update Restored V4</td>
            </tr>
        </table>
        <table width="100%" border="0" cellpadding="10" cellspacing="0" style="border: 1px solid #808080; border-top: none;">
            <tr>
                <td class="text-regular">
                    <ul>
                        <li><b>Driver Updates:</b> At this time, driver updates do not display any available updates across all supported operating systems.</li>
                        <li><b>Multilanguage Support:</b> Checking for updates on Windows 98 or Windows Millennium Edition in languages other than English or French may not display many available updates. We are actively working on a better multilingual support.</li>
                        <li><b>Windows Update Catalog:</b> Not all the Windows Update Catalog has been translated, contributing to limited availability of updates.</li>
                        <li><b>Driver Updates section only displays in the english Windows Update Catalog:</b> The Driver Updates section in the Windows Update Catalog does not display on the website that is in any language other than English.</li>
                        <li><b>Windows Update is stuck on "Checking for the latest version of the Windows Update software...":</b> Windows Update will not go further than this page. To fix this issue, empty Internet Explorer's cache by going to Tools &gt; Internet Options &gt; Delete Files (in the Temporary Internet files section).</li>
                    </ul>
                </td>
            </tr>
        </table>

        <table border="0" cellpadding="0" cellspacing="0"><tr><td height="20"></td></tr></table>

        <table width="100%" border="0" cellpadding="4" cellspacing="0" bgcolor="#666666">
            <tr>
                <td class="box-header-white">&nbsp;Windows Update Restored V5</td>
            </tr>
        </table>
        <table width="100%" border="0" cellpadding="10" cellspacing="0" style="border: 1px solid #808080; border-top: none;">
            <tr>
                <td bgcolor="#F2F2F2" class="text-regular">
                    <ul>
                        <li><b>Website will not work without system modifications:</b> This website requires specific system modifications to function properly. Please refer to our <a href="<%= ResolveUrl("~/en/Prerequisites.aspx") %>"><b><u>Prerequisites</u></b></a> page for instructions and to install the <i>Windows Update Restored Prerequisites Installer</i> to ensure the website operates correctly on your system.</li>
                        <li><b>Windows Update Stuck on "Windows Update is looking for available updates" on Windows 2000:</b> This issue occurs if you are using Internet Explorer 5. Upgrade to Internet Explorer 5.5 or higher.</li>
                        <li><b>Page Availability:</b> Windows Update Restored v5 may still lack some pages; users might encounter 404 error pages when attempting to click on certain links.</li>
                    </ul>
                </td>
            </tr>
        </table>

        <table border="0" cellpadding="0" cellspacing="0"><tr><td height="20"></td></tr></table>

        <table width="100%" border="0" cellpadding="4" cellspacing="0" bgcolor="#000080">
            <tr>
                <td class="box-header-white">&nbsp;Windows Update Restored V6</td>
            </tr>
        </table>
        <table width="100%" border="0" cellpadding="10" cellspacing="0" style="border: 1px solid #808080; border-top: none;">
            <tr>
                <td class="text-regular">
                    <ul>
                        <li><b>Website will not work without system modifications:</b> This website requires specific system modifications to function properly. Please refer to our <a href="<%= ResolveUrl("~/en/Prerequisites.aspx") %>"><b><u>Prerequisites</u></b></a> page for instructions and to install the <i>Windows Update Restored Prerequisites Installer</i> to ensure the website operates correctly on your system.</li>
                        <li><b>Website throws an "Application Error":</b> This happens because we're missing pages for the V6 website (both WU and MU). It's not a proper Windows Update error.</li>
                        <li><b>Page Availability:</b> Windows Update Restored v6 may still lack some pages; users might encounter 404 error pages when attempting to click on certain links.</li>
                    </ul>
                </td>
            </tr>
        </table>

        <table border="0" cellpadding="0" cellspacing="0"><tr><td height="20"></td></tr></table>

        <table width="100%" border="0" cellpadding="4" cellspacing="0" bgcolor="#666666">
            <tr>
                <td class="box-header-white">&nbsp;Microsoft Update Restored V6</td>
            </tr>
        </table>
        <table width="100%" border="0" cellpadding="10" cellspacing="0" style="border: 1px solid #808080; border-top: none;">
            <tr>
                <td bgcolor="#F2F2F2" class="text-regular">
                    <ul>
                        <li><b>Website will not work without system modifications:</b> This website requires specific system modifications to function properly. Please refer to our <a href="<%= ResolveUrl("~/en/Prerequisites.aspx") %>"><b><u>Prerequisites</u></b></a> page for instructions and to install the <i>Windows Update Restored Prerequisites Installer</i> to ensure the website operates correctly on your system.</li>
                        <li><b>Website throws a 404 error on "splash.aspx":</b> This happens because Microsoft Update is missing pages. Use V6 first to install the required updates, and then switch back to Microsoft Update. If you encounter this error, send the 404 URL to Windows Update Restored for inspection.</li>
                        <li><b>Website loops on <i>muoptdefault.aspx</i>:</b> Website will load but show <i>muoptdefault.aspx</i> repeatedly, no matter what the user clicked. This is due to a wrong version of <i>WUAUENG.DLL.</i> A new compatible and patched version is included in the <a href="<%= ResolveUrl("~/en/downloads/WurV5PatcherTool.aspx") %>"><b><u>Prerequisites Installer</u></b></a> (version 1.6 and newer).</li>
                    </ul>
                </td>
            </tr>
        </table>

        <p align="right" style="margin-top: 20px;"><a href="#top" class="text-small text-black">Back to top &#9650;</a></p>

    </div>

</asp:Content>
