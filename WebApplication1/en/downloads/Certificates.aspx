<%@ Page Title="Windows Update Restored Certificates" Language="C#" MasterPageFile="~/en/download-center/DownloadCenter.master" AutoEventWireup="true" CodeBehind="Certificates.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.en.downloads.Certificates" %>
<%@ Register TagPrefix="uc" TagName="ContentBox" Src="~/Controls/ContentBox.ascx" %>

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
            margin: 10px 0;
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
        .cw .download-link {font-size: 13px; /* Match the font size */
            font-weight: bold; /* Set font weight to bold */
            color: #000000;
            display: inline-block;
            padding: 5px;
            border: 2px solid #000080;
            background-color: #e6f7ff;
            border-radius: 4px;
            text-align: center;
            width: 250px; /* Make a consistent button width */ }
        .cw .instructions, .cw .known-issues {font-size: 13px; /* Maintain consistency */ }
    </style>

    <div class="cw">


    <h1>Windows Update Restored Certificates</h1>

    <p><b>Published:</b> April 17, 2024</p>
    <uc:ContentBox ID="cbIntro" runat="server" HeaderText="About the Certificates" HeaderColor="Blue" ContentColor="White" />

    <hr>

    <div class="disclaimer">
        <b>Disclaimer</b><br>
        We are not responsible for anything that goes wrong to your system if an event like that were to occur. Please proceed at your own risk.
    </div>

    <h2><br>
	File Info</h2>
    <p><strong>File name:</strong> wurcert.reg</p>
    <p><strong>File size:</strong> 8KB</p>
    <p><strong>Software version:</strong> No version available</p>
    <p><strong>Developer:</strong> WinFX</p>
    <p><strong>Download server:</strong> <a href="<%= SiteConfig.DownloadBaseUrl %>/" target="_blank"><%= SiteConfig.DownloadBaseUrl %></a></p>
    <p><strong>Digital signature:</strong> No signature available</p>
    <p><strong>Additional information:</strong> This certificate will automatically install in the right Certificates Store when being imported into the registry.</p>

    <h2><br>
		Download File</h2>
	<p><font color="#000080" style="font-size: 9pt">
	<a href="<%= SiteConfig.DownloadBaseUrl %>/wutools/wurcert_ansi.reg">
	<font color="#000000">
	<img border="0" src="<%= ResolveUrl("~/en/images/arrow.gif") %>" align="middle" width="30" height="25"></font></a></font><a href="<%= SiteConfig.DownloadBaseUrl %>/wutools/wurcert_ansi.reg"><span style="font-weight: 700"><font color="#000000" style="font-size: 10.5pt">Download 
	the wurcert.reg (ANSI)</font></span><span style="font-weight: 700;"><font color="#000000"> 
	file now!</font></font></span></a></p>
<p><font color="#000080" style="font-size: 9pt">
	<a href="<%= SiteConfig.DownloadBaseUrl %>/wutools/wurcert_unicode.reg">
	<font color="#000000">
	<img border="0" src="<%= ResolveUrl("~/en/images/arrow.gif") %>" align="middle" width="30" height="25"></font></a></font><a href="<%= SiteConfig.DownloadBaseUrl %>/wutools/wurcert_unicode.reg"><span style="font-weight: 700"><font color="#000000" style="font-size: 10.5pt">Download 
	the wurcert.reg</font></span><span style="font-weight: 700;"><font color="#000000"> 
(UNICODE) 
	file now!</font></font></span></a></p>

    <h2><br>
	System Requirements</h2>


        <section class="languages">
            <p style="font-size: 13px; line-height: 1.6; margin: 10px 0"><u><b>
			Unicode:</b></u></p>
	<b>

            <ul style="margin-left: 16px; margin-right: 0px; margin-top: 5px; margin-bottom: 5px">
				<li style="font-size: 13px; line-height: 1.6; margin: 10px 0">
				<p style="font-size: 13px; line-height: 1.6; margin: 10px 0">
				Windows XP</li>
				<li style="font-size: 13px; line-height: 1.6; margin: 10px 0">
				<p style="font-size: 13px; line-height: 1.6; margin: 10px 0">
				Windows Server 2003</li>
				<li style="font-size: 13px; line-height: 1.6; margin: 10px 0">
				<p style="font-size: 13px; line-height: 1.6; margin: 10px 0">
				Windows 2000 with Service Pack 4<li style="font-size: 13px; line-height: 1.6; margin: 10px 0">
				<p style="font-size: 13px; line-height: 1.6; margin: 10px 0">Windows 2000 with Service Pack 3</li>
</ul>
	<p style="font-size: 13px; line-height: 1.6; margin: 10px 0">&nbsp;</p>
	<p style="font-size: 13px; line-height: 1.6; margin: 10px 0"><u>ANSI:</u></p>
	<ul style="margin-left: 16px; margin-right: 0px; margin-top: 5px; margin-bottom: 5px">
				<li style="font-size: 13px; line-height: 1.6; margin: 10px 0">
				<p style="font-size: 13px; line-height: 1.6; margin: 10px 0">Windows Millennium Edition (Windows Me)</li>
				<li style="font-size: 13px; line-height: 1.6; margin: 10px 0">
				<p style="font-size: 13px; line-height: 1.6; margin: 10px 0">Windows 98 Second Edition</li>
				<li style="font-size: 13px; line-height: 1.6; margin: 10px 0">
				<p style="font-size: 13px; line-height: 1.6; margin: 10px 0">Windows 98</li>
				<li style="font-size: 13px; line-height: 1.6; margin: 10px 0">
				<p style="font-size: 13px; line-height: 1.6; margin: 10px 0">Windows NT 4.0 Workstation</li>
				<li style="font-size: 13px; line-height: 1.6; margin: 10px 0">
				<p style="font-size: 13px; line-height: 1.6; margin: 10px 0">Windows 95<br>&nbsp;</li>
</ul>
<p style="font-size: 13px; line-height: 1.6; margin: 10px 0">Note:</b>
				These 
certificates support multi-language systems.</font></p>
        </section>

    <h2><br>
		Instructions</h2>
<font face="Tahoma" style="font-size: 10.5pt" color="#000000">
                    <section class="history">

        <section class="languages">
        </section>
        </section>

        <section class="known-issues">
    
    <div style="background-color: #f9f9f9; border: 1px solid #dcdcdc; padding: 10px; margin-top: 20px;">
        <ol style="margin: 10px 0; padding-left: 20px;">
            <li style="margin-bottom: 8px;"><span style="font-size: 9pt">Double-click on the file to import it into the Windows Registry.</span></li>
            </font>
<font face="Tahoma" style="font-size: 9pt; font-weight:700" color="#000000">
            <li style="margin-bottom: 8px;">Restart your computer once 
			installed.</li>
        </ol>
    </div>
        </font>
        </section>
<h2><br>
Known Issues</h2>
    <p class="known-issues">No known issues were found with this download.</p>


    </div>

</asp:Content>
