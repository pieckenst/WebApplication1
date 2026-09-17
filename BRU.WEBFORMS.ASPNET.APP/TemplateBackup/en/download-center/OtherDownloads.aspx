<%@ Page Title="Other Downloads - Windows Update Restored Download Center" Language="C#" MasterPageFile="~/en/download-center/DownloadCenter.master" AutoEventWireup="true" CodeBehind="OtherDownloads.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.en.download_center.OtherDownloads" %>
<%@ Register TagPrefix="uc" TagName="ContentBox" Src="~/Controls/ContentBox.ascx" %>

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
    </style>

    <div class="dc-page">
    <a name="top"></a>

    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top" style="padding: 10px;">

                <div class="title-main">Other Downloads</div>

                <table width="100%" border="0" cellpadding="0" cellspacing="0" style="margin-top: 5px; margin-bottom: 12px;">
                    <tr><td height="2" bgcolor="#000080"></td></tr>
                </table>

                <uc:ContentBox ID="cbIntro" runat="server" HeaderText="About Other Downloads" HeaderColor="Blue" ContentColor="White" />

                <table width="100%" border="0" cellpadding="4" cellspacing="0" bgcolor="#000080">
                    <tr>
                        <td class="section-header">&nbsp;Download Categories</td>
                    </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="0"><tr><td height="12"></td></tr></table>

                <table width="100%" border="0" cellpadding="5" cellspacing="0">
                    <tr>
                        <td align="center" valign="top" width="33%">
                            <a title="This is the place to go if you're experiencing issues getting Windows Update Restored to work on your Windows operating system." href='<%= ResolveUrl("~/en/download-center/WurApps.aspx") %>'><img src='<%= ResolveUrl("~/en/images/automatic_updates.gif") %>' alt="WUR Tools" border="0" width="80"></a><br><br>
                            <a title="This is the place to go if you're experiencing issues getting Windows Update Restored to work on your Windows operating system." href='<%= ResolveUrl("~/en/download-center/WurApps.aspx") %>' class="nav-link-title">Windows Update Restored Tools</a><br>
                            <div style="line-height: 1.3; margin-top: 8px; font-size: 12px;">Is Windows Update Restored not working for you? Check out our dedicated tools for various OS versions.</div>
                        </td>

                        <td align="center" valign="top" width="33%">
                            <a href='<%= ResolveUrl("~/en/downloads/IeDownloads.aspx") %>' title="Download Internet Explorer 3.0 to 6.0 SP1, available in multilanguage."><img src='<%= ResolveUrl("~/en/images/internet_explorer.gif") %>' alt="Internet Explorer" border="0" width="80"></a><br><br>
                            <a href='<%= ResolveUrl("~/en/downloads/IeDownloads.aspx") %>' title="Download Internet Explorer 3.0 to 6.0 SP1, available in multilanguage." class="nav-link-title">Internet Explorer Downloads</a><br>
                            <div style="line-height: 1.3; margin-top: 8px; font-size: 12px;">Looking for specific versions of Internet Explorer? Access multi-language installers from 3.0 to 6.0 SP1.</div>
                        </td>

                        <td align="center" valign="top" width="34%">
                            <a title="Need to download a Service Pack for your version of Windows? This is the right place to visit!" href='<%= ResolveUrl("~/en/downloads/SpCenter.aspx") %>'><img src='<%= ResolveUrl("~/en/images/service_pack.gif") %>' alt="Service Packs" border="0" width="80"></a><br><br>
                            <a title="Need to download a Service Pack for your version of Windows? This is the right place to visit!" href='<%= ResolveUrl("~/en/downloads/SpCenter.aspx") %>' class="nav-link-title">Service Pack Center</a><br>
                            <div style="line-height: 1.3; margin-top: 8px; font-size: 12px;">The best way to start patching. Full packs for Windows 95 through Windows XP are available here.</div>
                        </td>
                    </tr>
                </table>

                <table border="0" cellpadding="0" cellspacing="0"><tr><td height="8"></td></tr></table>

                <p align="right" style="margin-top: 20px;"><a href="#top" class="text-small">Back to top &#9650;</a></p>

                <table border="0" cellpadding="0" cellspacing="0"><tr><td height="15"></td></tr></table>

            </td>
        </tr>
    </table>
    </div>

</asp:Content>
