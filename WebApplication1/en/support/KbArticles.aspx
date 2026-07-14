<%@ Page Title="Knowledge Base Articles" Language="C#" MasterPageFile="~/en/support/Support.master" AutoEventWireup="true" CodeBehind="KbArticles.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.en.support.KbArticles" %>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        .cw, .cw table, .cw td, .cw p, .cw div, .cw span, .cw ul, .cw li {font-family: Tahoma, Verdana, Arial, sans-serif; 
            font-size: 10pt; }
        .cw {margin: 0; 
            padding: 0; 
            background-color: #FFFFFF; 
            color: #000000; }
        .cw a {color: #000080; text-decoration: none; }
        .cw a:hover {color: #FF6600; text-decoration: underline; }
        .cw .title-main {font-size: 16pt; color: #000080; font-weight: bold; }
        .cw .text-regular {font-size: 10pt; line-height: 1.5; }
        .cw .text-small {font-size: 8pt; }
        .cw .text-gray {color: #666666; }
        .cw .text-black {color: #000000; }
        .cw .toc-bar {background-color: #F5F5F5;
            border: 1px solid #DCDCDC;
            padding: 10px 15px;
            margin-bottom: 25px; }
        .cw .toc-title {font-weight: bold;
            color: #000080;
            margin-bottom: 5px; }
        .cw .toc-list {margin: 0;
            padding-left: 15px; }
        .cw .toc-list li {margin-bottom: 4px; }
        .cw .kb-item {margin-bottom: 25px;
            padding-bottom: 20px;
            border-bottom: 1px solid #E0E0E0; }
        .cw .kb-header-row {margin-bottom: 6px; }
        .cw .kb-badge {background-color: #000080;
            color: #FFFFFF;
            font-size: 8pt;
            font-weight: bold;
            padding: 2px 6px;
            margin-right: 8px;
            vertical-align: middle; }
        .cw .kb-link {font-size: 10.5pt;
            font-weight: bold;
            color: #0040A0;
            vertical-align: middle; }
        .cw .kb-link:hover {color: #FF6600; }
        .cw .kb-meta {font-size: 8.5pt;
            color: #666666;
            margin-bottom: 8px;
            padding-left: 2px; }
        .cw .kb-meta strong {color: #333333; }
        .cw .kb-desc {font-size: 10pt;
            line-height: 1.45;
            color: #333333;
            margin: 5px 0 0 2px; }
    </style>

    <div class="cw">


<a name="top"></a>

<table width="100%" border="0" cellpadding="10" cellspacing="0">
    <tr>
        <td valign="top">
            
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="bottom" class="title-main">
                        Knowledge Base
                    </td>
                </tr>
            </table>
            
            <table width="100%" border="0" cellpadding="0" cellspacing="0" style="margin-top: 5px; margin-bottom: 15px;">
                <tr><td height="2" bgcolor="#000080"></td></tr>
            </table>

            <p class="text-regular" style="margin-top: 0; margin-bottom: 15px;">
                This page lists every Support topic we published over time regarding Windows Update Restored.
            </p>

            <div class="toc-bar">
                <div class="toc-title">Quick Index:</div>
                <ul class="toc-list">
                    <li><a href="#kb-wur-1301"><b>KB-WUR-1301</b>: Windows Update V3.1 no longer works after scanning on V4</a></li>
                    <li><a href="#kb-wur-1001"><b>KB-WUR-1001</b>: Handling security certificate warnings in Internet Explorer</a></li>
                    <li><a href="#kb-wur-2002"><b>KB-WUR-2002</b>: Troubleshooting connection issues using a local proxy</a></li>
                    <li><a href="#kb-wur-2001"><b>KB-WUR-2001</b>: Windows Update Restored connection blocked by local ISPs</a></li>
                </ul>
            </div>

            <a name="kb-wur-1301"></a>
            <div class="kb-item">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top" width="55" style="padding-right: 12px;">
                            <img border="0" src="<%= ResolveUrl("~/en/images/question-big.gif") %>" width="48" height="47" alt="Question">
                        </td>
                        <td valign="top">
                            <div class="kb-header-row">
                                <span class="kb-badge">KB-WUR-1301</span>
                                <a class="kb-link" href="<%= ResolveUrl("~/en/support/kb/WurV31NotWorkingAfterUsingWurV4.aspx") %>">Windows Update V3.1 no longer works after scanning for updates on Windows Update V4</a>
                            </div>
                            <div class="kb-meta">
                                <strong>Published:</strong> April 19, 2024 &nbsp;|&nbsp; 
                                <strong>Author:</strong> TecAdam &nbsp;|&nbsp; 
                                <strong>Applies to:</strong> Windows Update Restored V3.1/V4
                            </div>
                            <p class="kb-desc">
                                When you check for updates on Windows Update Restored v4, ActiveX controls for the website are loaded and registry entries are added, preventing you from scanning for updates on previous versions of Windows Update.
                            </p>
                        </td>
                    </tr>
                </table>
            </div>

            <a name="kb-wur-1001"></a>
            <div class="kb-item">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top" width="55" style="padding-right: 12px;">
                            <img border="0" src="<%= ResolveUrl("~/en/images/question-big.gif") %>" width="48" height="47" alt="Question">
                        </td>
                        <td valign="top">
                            <div class="kb-header-row">
                                <span class="kb-badge">KB-WUR-1001</span>
                                <a class="kb-link" href="<%= ResolveUrl("~/en/support/kb/WurCert.aspx") %>">You get certificate warnings in Internet Explorer</a>
                            </div>
                            <div class="kb-meta">
                                <strong>Published:</strong> April 17, 2024 &nbsp;|&nbsp; 
                                <strong>Author:</strong> WULover &nbsp;|&nbsp; 
                                <strong>Applies to:</strong> Windows Update Restored in general
                            </div>
                            <p class="kb-desc">
                                Windows Update Restored is using self-signed certificates to sign patched CABs and installers. This is the fix to remove the certificate warnings in Internet Explorer.
                            </p>
                        </td>
                    </tr>
                </table>
            </div>

            <a name="kb-wur-2002"></a>
            <div class="kb-item">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top" width="55" style="padding-right: 12px;">
                            <img border="0" src="<%= ResolveUrl("~/en/images/question-big.gif") %>" width="48" height="47" alt="Question">
                        </td>
                        <td valign="top">
                            <div class="kb-header-row">
                                <span class="kb-badge">KB-WUR-2002</span>
                                <a class="kb-link" href="<%= ResolveUrl("~/en/support/kb/Proxy.aspx") %>">Troubleshooting Windows Update using a Proxy</a>
                            </div>
                            <div class="kb-meta">
                                <strong>Published:</strong> April 15, 2024 &nbsp;|&nbsp; 
                                <strong>Author:</strong> WULover &nbsp;|&nbsp; 
                                <strong>Applies to:</strong> Windows Update Restored in general
                            </div>
                            <p class="kb-desc">
                                Windows Update is sometimes a pain to troubleshoot. A proxy tool can be of great help to find out what's wrong/missing!
                            </p>
                        </td>
                    </tr>
                </table>
            </div>

            <a name="kb-wur-2001"></a>
            <div class="kb-item" style="border-bottom: none; margin-bottom: 10px; padding-bottom: 0;">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top" width="55" style="padding-right: 12px;">
                            <img border="0" src="<%= ResolveUrl("~/en/images/question-big.gif") %>" width="48" height="47" alt="Question">
                        </td>
                        <td valign="top">
                            <div class="kb-header-row">
                                <span class="kb-badge">KB-WUR-2001</span>
                                <a class="kb-link" href="<%= ResolveUrl("~/en/support/kb/WurBlockedByIsp.aspx") %>">Windows Update Restored is blocked by your ISP</a>
                            </div>
                            <div class="kb-meta">
                                <strong>Published:</strong> April 15, 2024 &nbsp;|&nbsp; 
                                <strong>Author:</strong> WULover &nbsp;|&nbsp; 
                                <strong>Applies to:</strong> Windows Update Restored in general
                            </div>
                            <p class="kb-desc">
                                Because our website is still using old Web standards, your ISP might not like it. We're providing you a temporary fix to avoid these errors.
                            </p>
                        </td>
                    </tr>
                </table>
            </div>

            <p align="right" style="margin-top: 20px;"><a href="#top" class="text-small text-black">Back to top &#9650;</a></p>

        </td>
    </tr>
</table>


    </div>

</asp:Content>
