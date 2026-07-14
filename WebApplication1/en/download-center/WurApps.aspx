<%@ Page Title="Windows Update Restored Tools" Language="C#" MasterPageFile="~/en/download-center/DownloadCenter.master" AutoEventWireup="true" CodeBehind="WurApps.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.en.download_center.WurApps" %>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        .dc-tools { color: #000000; font-family: Tahoma, Verdana, Arial, sans-serif; line-height: 1.6; padding: 20px; }
        .dc-tools a { color: #000080; text-decoration: none; }
        .dc-tools a:hover { text-decoration: underline; }
        .dc-tools h1 { font-size: 20px; color: #000080; border-bottom: 2px solid #000080; padding-bottom: 10px; margin: 0 0 20px 0; }
        .dc-tools p { font-size: 13px; margin: 10px 0; }
        .dc-tools hr { border: 0; height: 1px; background: #000080; margin: 10px 0; }
        .dc-tools .tool-table { width: 100%; border-collapse: collapse; }
        .dc-tools .tool-table td { padding: 10px; vertical-align: top; }
        .dc-tools .tool-icon { width: 1px; padding: 1px 8pt 0 0; vertical-align: top; }
        .dc-tools .tool-name { color: #003C91; font-size: 13pt; margin: 0 0 2pt 0; }
        .dc-tools .tool-name a { display: block; }
        .dc-tools .tool-meta { margin: 0 0 2pt 0; padding: 0; list-style: none; display: flex; flex-wrap: wrap; font-size: 9pt; }
        .dc-tools .tool-meta li { display: inline; margin-right: 1em; }
        .dc-tools .tool-meta strong { font-weight: 600; }
        .dc-tools .tool-desc { margin: 0 0 16pt 0; font-size: 10pt; }
    </style>

    <div class="dc-tools">
        <a name="top"></a>
        <h1>Windows Update Restored Tools</h1>

        <p>This page lists all the Windows Update Restored tools we have created. These tools help prevent errors on the Windows Update websites while they are in use.</p>

        <hr>

        <table class="tool-table">
            <tr>
                <td class="tool-icon"><img border="0" src='<%= ResolveUrl("~/en/images/automatic_updates.gif") %>' width="48" height="48" alt=""></td>
                <td>
                    <h3 class="tool-name"><a title="Download the Windows Update Restored Prerequisites Installer to automatically install all the required prerequisites for Windows Update Restored v5 to work on your computer." href='<%= ResolveUrl("~/en/downloads/WurV5PatcherTool.aspx") %>'>Windows Update Restored Prerequisites Installer</a></h3>
                    <ul class="tool-meta">
                        <li><strong>Published:</strong>&nbsp;March 21, 2024</li>
                        <li><strong>Developer:</strong>&nbsp;WULover</li>
                        <li><strong>Version:</strong>&nbsp;v1.6 (Release)</li>
                        <li><strong>Category:</strong>&nbsp;Troubleshooter</li>
                        <li><strong>Language:</strong>&nbsp;English</li>
                    </ul>
                    <p class="tool-desc">The Windows Update Restored Prerequisites Installer is the only available tool to make Windows Update Restored V5 and V6 work on your computer. It will automatically install all the necessary prerequisites for the website to function properly on any supported systems.</p>
                </td>
            </tr>
            <tr>
                <td class="tool-icon"><img border="0" src='<%= ResolveUrl("~/en/images/automatic_updates.gif") %>' width="48" height="48" alt=""></td>
                <td>
                    <h3 class="tool-name"><a title="This tool will check your computer for useful Windows Update inventories, in case you have any." href='<%= ResolveUrl("~/en/downloads/WurInvChecker.aspx") %>'>Windows Update Restored Inventory Checker</a></h3>
                    <ul class="tool-meta">
                        <li><strong>Published:</strong>&nbsp;February 13, 2025</li>
                        <li><strong>Developer:</strong>&nbsp;WULover</li>
                        <li><strong>Version:</strong>&nbsp;1.0.4 (Release)</li>
                        <li><strong>Category:</strong>&nbsp;Checker</li>
                        <li><strong>Language:</strong>&nbsp;English</li>
                    </ul>
                    <p class="tool-desc">This tool will check your computer for useful Windows Update V2/V3.1, V4 and Office Update inventories, in case you have any. You will have an option to backup those inventories, before maybe accidentaly erasing them.</p>
                </td>
            </tr>
            <tr>
                <td class="tool-icon"><img border="0" src='<%= ResolveUrl("~/en/images/automatic_updates.gif") %>' width="48" height="48" alt=""></td>
                <td>
                    <h3 class="tool-name"><a title="The Windows Update Restored Compatibility Checker checks your system to determine if your computer is compatible with Windows Update Restored v5. If not, it will tell you what needs to be fixed." href='<%= ResolveUrl("~/en/downloads/WurV5CheckerTool.aspx") %>'>Windows Update Restored Compatibility Checker</a></h3>
                    <ul class="tool-meta">
                        <li><strong>Published:</strong>&nbsp;January 23, 2024</li>
                        <li><strong>Developer:</strong>&nbsp;Joshua Maitland</li>
                        <li><strong>Version:</strong>&nbsp;1.0.1.0</li>
                        <li><strong>Category:</strong>&nbsp;Checker</li>
                        <li><strong>Language:</strong>&nbsp;English</li>
                    </ul>
                    <p class="tool-desc">This tool checks your system to determine if your computer is compatible with Windows Update Restored v5. If not, it will inform you of what is needed to resolve the issue.</p>
                </td>
            </tr>
            <tr>
                <td class="tool-icon"><img border="0" src='<%= ResolveUrl("~/en/images/automatic_updates.gif") %>' width="48" height="48" alt=""></td>
                <td>
                    <h3 class="tool-name"><a title="Windows Update Restored uses self-signed certificates for patched CABs and installers. This fix removes certificate warnings in Internet Explorer." href='<%= ResolveUrl("~/en/downloads/Certificates.aspx") %>'>Windows Update Restored Certificates</a></h3>
                    <ul class="tool-meta">
                        <li><strong>Published:</strong>&nbsp;April 17, 2024</li>
                        <li><strong>Developer:</strong>&nbsp;WinFX</li>
                        <li><strong>Version:</strong>&nbsp;No version available</li>
                        <li><strong>Category:</strong>&nbsp;Utilities</li>
                        <li><strong>Language:</strong>&nbsp;Multilanguage</li>
                    </ul>
                    <p class="tool-desc">Windows Update Restored uses self-signed certificates to sign patched CABs and installers. This fix removes the certificate warnings in Internet Explorer.</p>
                </td>
            </tr>
        </table>
        <hr>
    </div>

</asp:Content>
