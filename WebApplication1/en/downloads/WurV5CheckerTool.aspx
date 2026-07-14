<%@ Page Title="Windows Update Restored v5 Checker Tool" Language="C#" MasterPageFile="~/en/download-center/DownloadCenter.master" AutoEventWireup="true" CodeBehind="WurV5CheckerTool.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.en.downloads.WurV5CheckerTool" %>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        .cw {color: black;
            background-color: white;
            overflow: auto;
            margin: 10px 16px;
            font-family: 'Segoe UI Variable Text', Segoe UI, Tahoma, sans-serif; /* Applied font stack */ }
        .cw h1 {font-size: 20px;
            color: #000080;
            border-bottom: 2px solid #000080;
            padding-bottom: 8px;
            margin-bottom: 10px; }
        .cw h2 {color: #000080;
            font-size: 16px;
            margin: 20px 0 10px;
            border-bottom: 1px solid #000080;
            padding-bottom: 5px; }
        .cw p {font-size: 13px;
            line-height: 1.6;
            margin: 10px 0; }
        .cw ul {margin: 5px 0 10px 20px; /* Adjust margin for lists */ }
        .cw hr {border: none;
            border-top: 1px solid #000080;
            margin: 10px 0; }
        .cw .disclaimer {background-color: #ffcccc;
            border: 1px solid #cc0000;
            color: #cc0000;
            padding: 10px;
            margin: 10px 0;
            font-size: 13px;
            border-radius: 5px;
            box-shadow: 0 0 5px rgba(0, 0, 0, 0.1); }
        .cw a {color: #000080;
            text-decoration: none; }
        .cw a:hover {text-decoration: underline; }
        .cw .download-button {font-size: 10.5pt;
            font-weight: 700;
            color: #000000;
            display: inline-block;
            padding: 5px;
            border: 1px solid #000080;
            background-color: #e6f7ff;
            border-radius: 5px; }
    </style>

    <div class="cw">


    <h1>Windows Update Restored Compatibility Checker</h1>

    <p><b>Published:</b> January 23, 2024</p>

    <p>The Windows Update Restored Compatibility Checker checks your system to 
	determine if your computer is compatible with Windows Update Restored v5. If 
	not, it will inform you of what is needed to resolve the issue.</p>

    <hr>

    <div class="disclaimer">
        <b>Disclaimer</b><br>
        We are not responsible for anything that goes wrong to your system if an event like that were to occur. Please proceed at your own risk.
    </div>
    <h2><br>
	File Information</h2>
    <p><strong>File name:</strong> wurv5checkertool.exe</p>
    <p><strong>File size:</strong> 48 KB</p>
    <p><strong>Developer:</strong> Joshua Maitland</p>
    <p><strong>Software version:</strong> v1.0.1.0 (Release)</p>
    <p><strong>Download server:</strong> <a href="<%= SiteConfig.DownloadBaseUrl %>/" target="_blank"><%= SiteConfig.DownloadBaseUrl %></a></p>
    <p><strong>Digital signature:</strong> Signed on April 17, 2024 by windowsupdaterestored.com</p>
    <p><strong>Additional information:</strong> This tool only monitors if your computer is Windows Update Restored v5-ready; it does not patch your system.<br>

<br>
</p>
<h2>Download File</h2>
	<p><font color="#000080" style="font-size: 9pt" face="Tahoma">
	<a style="color: rgb(51, 102, 255);" href="<%= SiteConfig.DownloadBaseUrl %>/wutools/wurv5checkertool.EXE">
	<img border="0" src="<%= ResolveUrl("~/en/images/arrow.gif") %>" align="middle" width="30" height="25"></a></font><font face="Tahoma" style="font-size: 10.5pt;"><span style="font-weight: 700;"><a style="color: rgb(51, 102, 255); text-decoration:none" href="<%= SiteConfig.DownloadBaseUrl %>/wutools/wurv5checkertool.EXE"><font color="#000000">Download 
	the wurv5checkertool.exe file now!</font></a></span></font><br>
<br>
    </p>
    <h2>System Requirements</h2>
    <p><strong>Operating Systems:</strong></p>
<ul>
	<li><font size="2">Windows XP</font></li>
<li><font size="2">Windows Server 2003</font></li>
	<li><font size="2">Windows 2000 with Service Pack 4</font></li>
<li><font size="2">Windows 2000 with Service Pack 3</font></li>
</ul>
    <p><strong>Languages:</strong> This tool is multi-language.</p>
    <p><strong>Microsoft .NET Framework:</strong> Version 1.0</p>
    <h2>Instructions</h2>

                    <section class="history">

        <section class="languages">
        </section>
        </section>

        <section class="known-issues">
    
    <div style="background-color: #f9f9f9; border: 1px solid #dcdcdc; padding: 10px; margin-top: 20px;">
        <ol style="margin: 10px 0; padding-left: 20px;">
        <li><font size="2">Install <b>.NET Framework 1.0 or higher</b> if not 
		done already. You can download it from the <a href="http://v4.windowsupdaterestored.com/catalog/en/" target="_blank">
		Windows Update Catalog</a>.</font></li>
        <li><font size="2">Run the <b>WURv5CheckerTool.exe</b> file once 
		downloaded.</font></li>
        <li><font size="2">One of the following will happen:
            </font>
            <ul>
                <li><font size="2">If the Status text is <font color="#FF0000">
				Red</font> and says &quot;Not Ready!&quot;, you will need to download the 
				Windows Update Restored v5 Prerequisites Installer.</font></li>
                <li><font size="2">If the Status text is <font color="#008000">
				Green</font> and says &quot;Ready!&quot;, click the &quot;Launch V5!&quot; button to 
				open Windows Update Restored v5 in Internet Explorer!</font></li>
            </ul>
        </li>
        </ol>
    </div>
        </section></font>


    </div>

</asp:Content>
