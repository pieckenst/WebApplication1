<%@ Page Title="Related Projects" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Related.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.en.Related" %>
<%@ Register TagPrefix="uc" TagName="ContentBox" Src="~/Controls/ContentBox.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

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
                    Related Projects
                </td>
            </tr>
        </table>

        <table width="100%" border="0" cellpadding="0" cellspacing="0" style="margin-top: 5px; margin-bottom: 15px;">
            <tr><td height="2" bgcolor="#000080"></td></tr>
        </table>

        <uc:ContentBox ID="cbIntro" runat="server" HeaderText="Overview" HeaderColor="Blue" ContentColor="White" />

        <table border="0" cellpadding="0" cellspacing="0"><tr><td height="15"></td></tr></table>

        <table width="100%" border="0" cellpadding="4" cellspacing="0" bgcolor="#000080">
            <tr><td class="box-header-white">&nbsp;Legacy Update</td></tr>
        </table>
        <table width="100%" border="0" cellpadding="10" cellspacing="0" style="border: 1px solid #808080; border-top: none;">
            <tr>
                <td class="text-regular">
                    Legacy Update is our sister project, and it's the best place to update Windows Vista and newer versions of Windows!
                    <br><br>
                    <div align="left">
                        <a target="_blank" title="Get back online, activate, and install updates on your legacy Windows PC." href="http://legacyupdate.net">
                            <img src="<%= ResolveUrl("~/en/images/relatedprojects/banner_legacyupdate.gif") %>" alt="Get back online, activate, and install updates on your legacy Windows PC" border="0" style="max-width: 100%; height: auto;">
                        </a>
                    </div>
                </td>
            </tr>
        </table>

        <table border="0" cellpadding="0" cellspacing="0"><tr><td height="20"></td></tr></table>

        <table width="100%" border="0" cellpadding="4" cellspacing="0" bgcolor="#806000">
            <tr><td class="box-header-white">&nbsp;TheOldNet Webring</td></tr>
        </table>
        <table width="100%" border="0" cellpadding="10" cellspacing="0" style="border: 1px solid #808080; border-top: none;">
            <tr>
                <td class="text-regular" bgcolor="#F2F2F2">
                    We are member of TheOldNet webring!
                    <br><br>
                    <div align="left">
                        <a id="theoldnet-webring-href" data-website-id="fa90ee4ad8daa3229247b0f2573aef61" target="_blank" href="http://webring.theoldnet.com/widget/fa90ee4ad8daa3229247b0f2573aef61/navigate">
                            <img src="http://webring.theoldnet.com/widget/fa90ee4ad8daa3229247b0f2573aef61/image" alt="Windows Update Restored" border="0" style="max-width: 100%; height: auto;">
                        </a>
                    </div>
                    <br>
                    Proud member of <a target="_blank" href="http://webring.theoldnet.com/">TheOldNet</a> webring! Check some other cool websites!
                    <br>
                    [<a target="_blank" href="http://webring.theoldnet.com/member/fa90ee4ad8daa3229247b0f2573aef61/previous/navigate">Previous site</a>] - [<a target="_blank" href="http://webring.theoldnet.com/member/fa90ee4ad8daa3229247b0f2573aef61/random/navigate">Random site</a>] - [<a target="_blank" href="http://webring.theoldnet.com/member/fa90ee4ad8daa3229247b0f2573aef61/next/navigate">Next site</a>]
                    <script type="text/javascript" src="http://webring.theoldnet.com/widget/widget.js"></script>
                </td>
            </tr>
        </table>

        <table border="0" cellpadding="0" cellspacing="0"><tr><td height="20"></td></tr></table>

        <table width="100%" border="0" cellpadding="4" cellspacing="0" bgcolor="#000000">
            <tr><td class="box-header-white">&nbsp;Protoweb</td></tr>
        </table>
        <table width="100%" border="0" cellpadding="10" cellspacing="0" style="border: 1px solid #808080; border-top: none;">
            <tr>
                <td class="text-regular">
                    Windows Update Restored is also available on Protoweb, giving you the original Web experience of the 90's, with downloads, updates and more!
                    <br><br>
                    <div align="left">
                        <a target="_blank" title="Bringing Back The Information Super Highway!" href="http://protoweb.org">
                            <img src="<%= ResolveUrl("~/en/images/relatedprojects/protoweb.gif") %>" alt="Bringing Back The Information Super Highway!" border="0" width="253" height="57">
                        </a>
                    </div>
                </td>
            </tr>
        </table>

        <table border="0" cellpadding="0" cellspacing="0"><tr><td height="20"></td></tr></table>

        <table width="100%" border="0" cellpadding="4" cellspacing="0" bgcolor="#806000">
            <tr><td class="box-header-white">&nbsp;Vornext's Golden Browser</td></tr>
        </table>
        <table width="100%" border="0" cellpadding="10" cellspacing="0" style="border: 1px solid #808080; border-top: none;">
            <tr>
                <td class="text-regular" bgcolor="#F2F2F2">
                    Vornext's Golden Browser is a modified version of Internet Explorer, designed to run on low end hardware.
                    <br><br>
                    <div align="left">
                        <a target="_blank" title="Get a decent working browser for low end hardware!" href="http://goldenbrowser.eu.org/">
                            <img src="<%= ResolveUrl("~/en/images/relatedprojects/gbbanner_wur.gif") %>" alt="Golden Browser works faster to save you time." border="0" style="max-width: 100%; height: auto;">
                        </a>
                    </div>
                </td>
            </tr>
        </table>

        <table border="0" cellpadding="0" cellspacing="0"><tr><td height="20"></td></tr></table>

        <table width="100%" border="0" cellpadding="4" cellspacing="0" bgcolor="#000080">
            <tr><td class="box-header-white">&nbsp;retro.directory</td></tr>
        </table>
        <table width="100%" border="0" cellpadding="10" cellspacing="0" style="border: 1px solid #808080; border-top: none;">
            <tr>
                <td class="text-regular">
                    Windows Update Restored is listed in the <b>Retro Directory</b>!
                    <br><br>
                    <div align="left">
                        <a target="_blank" href="https://retro.directory">
                            <img src="http://retro.directory/images/w191x98.png" width="191" height="98" alt="We're listed in the retro.directory, take a look">
                        </a>
                    </div>
                </td>
            </tr>
        </table>

        <table border="0" cellpadding="0" cellspacing="0"><tr><td height="20"></td></tr></table>

        <table width="100%" border="0" cellpadding="4" cellspacing="0" bgcolor="#006633">
            <tr><td class="box-header-white">&nbsp;2600 Network</td></tr>
        </table>
        <table width="100%" border="0" cellpadding="10" cellspacing="0" style="border: 1px solid #808080; border-top: none;">
            <tr>
                <td class="text-regular" bgcolor="#F2F2F2">
                    Windows Update Restored is available using <b>2600network</b>!
                    <br><br>
                    <div align="left">
                        <a target="_blank" href="http://retro.2600.network">
                            <img src="http://www.windowsupdaterestored.com/en/images/relatedprojects/network2600_banner.gif" alt="We're available through 2600network using dial-up!" border="0" height="60" style="height: 60px; width: auto; max-width: 100%;">
                        </a>
                    </div>
                </td>
            </tr>
        </table>

        <table border="0" cellpadding="0" cellspacing="0"><tr><td height="20"></td></tr></table>

        <table width="100%" border="0" cellpadding="4" cellspacing="0" bgcolor="#000080">
            <tr><td class="box-header-white">&nbsp;Compu-Global-Hyper-Mega-Net (CGHMN)</td></tr>
        </table>
        <table width="100%" border="0" cellpadding="10" cellspacing="0" style="border: 1px solid #808080; border-top: none;">
            <tr>
                <td class="text-regular">
                    You can access this website through the use of <b>CGHMN</b>!
                    <br><br>
                    <div align="left">
                        <a target="_blank" href="http://cghmn.org">
                            <img src="http://www.windowsupdaterestored.com/en/images/relatedprojects/cghmn_logo.gif" alt="We're available through CGHMN!" border="0" height="100" style="height: 100px; width: auto; max-width: 100%;">
                        </a>
                    </div>
                </td>
            </tr>
        </table>

        <p align="right" style="margin-top: 20px;"><a href="#top" class="text-small text-black">Back to top &#9650;</a></p>

    </div>

</asp:Content>
