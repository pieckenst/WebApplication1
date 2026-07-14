<%@ Page Title="Help &amp; Support Center - Windows Update Restored" Language="C#" MasterPageFile="~/en/support/Support.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.en.support.Default" %>
<%@ Register TagPrefix="uc" TagName="ContentBox" Src="~/Controls/ContentBox.ascx" %>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        .cw {color: black;
            background-color: white;
            margin: 0;
            padding: 0;
            font-family: Segoe UI Variable Text, Segoe UI, Tahoma, sans-serif; /* Global font family */
            line-height: 1.6;
            padding: 20px; /* Add padding to the body for better spacing from edges */ }
        .cw header {margin: 0 0 20px 0; /* Margin below header */ }
        .cw h1 {font-size: 20px;
            color: #000080; /* Title color */
            border-bottom: 2px solid #000080; /* Title underline color */
            padding-bottom: 10px;
            margin: 0; /* No extra margin for consistency */ }
        .cw p {font-size: 13px;
            margin: 10px 0; }
        .cw a {color: #000080; /* Default link color */
            text-decoration: none; }
        .cw a:hover {text-decoration: underline; }
        .cw .tutorial-link {font-weight: bold;
            font-size: 14px;
            color: #000080; /* Ensure link color matches */
            display: block;
            margin-bottom: 10px; }
        .cw .tutorial-link img {vertical-align: middle;
            border: 0;
            margin-right: 5px; }
        .cw blockquote {margin: 10px 0;
            padding: 10px;
            border-left: 4px solid #CCCCCC;
            background-color: #F9F9F9; }
        .cw footer {text-align: center;
            margin-top: 20px;
            border-top: 1px solid #000080;
            padding-top: 10px; }
        .cw .discord-link {color: #0000FF; }
        .cw .discord-link:hover {text-decoration: underline; }
        .cw .content-table {width: 100%;
            border-collapse: collapse;
            margin-bottom: 20px; /* Space below the table */ }
        .cw .content-table td {padding: 10px;
            vertical-align: top; }
        .cw .news-section {margin-top: 20px;
            border: 1px solid #5E7DD8;
            padding: 10px;
            background-color: #FFFFFF; }
        .cw .latest-news-title {font-size: 16px;
            font-weight: bold;
            color: #000080;
            margin: 0;
            padding: 10px 0; }
        .cw .news-title {font-size: 10pt;
            font-weight: bold;
            color: black;
            margin-bottom: 10px;
            display: flex;
            align-items: center; }
        .cw .news-title img {margin-right: 10px; }
        .cw .news-item {font-size: 9pt;
            color: #000000;
            margin-bottom: 15px; }
        .cw .news-item p {margin: 0;
            padding: 0; }
        .cw .news-item a {color: #000080; /* Ensure link color matches */
            text-decoration: none;
            font-weight: bold; }
        .cw .news-item a:hover {text-decoration: underline; }
        .cw hr {border: 0;
            height: 1px;
            background: #000080; /* Set the horizontal rule color */
            margin: 10px 0; }
    </style>

    <div class="cw">


    <header>
        <h1><font size="5">Help &amp; Support Center</font></h1>
    </header>

    <main>
        <uc:ContentBox ID="cbIntro" runat="server" HeaderText="Welcome" HeaderColor="Blue" ContentColor="White" />
        <hr>

        <table class="content-table">
            <tbody>
            <tr>
                <td>
                    <a class="tutorial-link" href="<%= ResolveUrl("~/en/support/KnownIssues.aspx") %>">
                        <img src="<%= ResolveUrl("~/en/images/arrow.gif") %>" width="30" height="25" alt="Arrow">Known Issues with Windows Update Restored
                    </a>
                    <blockquote>
                        <p>Wondering what current issues exist within the Windows Update Restored website? You can find out by visiting our Known Issues page!
						<br>
						<b>This page consists of issues within Windows Update Restored 
						V2, V3.1, V4, and V5.</b></p>
                    </blockquote>
                </td>
            </tr>
            <tr>
                <td>
                    <a class="tutorial-link" title="Contact us to get help for a bug not listed in the Knowledge Base." href="<%= ResolveUrl("~/en/About.aspx") %>">
                        <img src="<%= ResolveUrl("~/en/images/arrow.gif") %>" width="30" height="25" alt="Arrow">Assistance on a specific bug
                    </a><blockquote>
                        <p>Need help regarding a specific bug or issue you're 
						encoutering when using Windows Update Restored? Feel 
						free to contact us! Learn more on the <b>
						<a href="<%= ResolveUrl("~/en/About.aspx") %>">About the Project</a></b> page, 
						which contains the email adresses of the owner and 
						server provider. You can also contact us by joining our
						<b>
						<a target="_blank" href="http://legacyupdate.net/discord">
						Discord Server</a></b>, or sending a message on our <b>
						<a target="_blank" href="https://msfn.org/board/topic/183394-microsoft-windows-update-v3v4v5/">
						MSFN topic</a></b>.</p>
                    </blockquote>
                </td>
            </tr>
            <tr>
                <td>
                    <a class="tutorial-link" title="Some versions of the Windows Update website rquire system modifications to work properly." href="<%= ResolveUrl("~/en/prerequisites.htm.tmp.html") %>">
                        <img src="<%= ResolveUrl("~/en/images/arrow.gif") %>" width="30" height="25" alt="Arrow">Prerequisites to run Windows Update Restored
                    </a>
                    <blockquote>
                        <p>Is Windows Update Restored not working for you out of the box? 
						Head over to our <b><a href="<%= ResolveUrl("~/en/prerequisites.htm.tmp.html") %>">Prerequisites</a></b> page and that should help you!
						<br>
						<b>Please note that this applies to Windows Update Restored only.</b></p>
                    </blockquote>
                </td>
            </tr>
            <tr>
                <td>
                    <a class="tutorial-link" title="The Knowledge Base contains every support article we publish." href="<%= ResolveUrl("~/en/support/KbArticles.aspx") %>">
                        <img src="<%= ResolveUrl("~/en/images/arrow.gif") %>" width="30" height="25" alt="Arrow">Knowledge Base
                    </a>
                    <blockquote>
                        <p>If the above topics do not correspond to what you're looking for, the 
						<b><a href="<%= ResolveUrl("~/en/support/KbArticles.aspx") %>">Knowledge Base</a></b> might contain something helpful.</p>
                    </blockquote>
                </td>
            </tr>
        </tbody>
        </table>

        <hr>

        <div class="latest-news-title">
            Help &amp; Support Center News</div>
        <div class="news-section">
            <div class="news-item">
                <div class="news-title">
                    <img src="<%= ResolveUrl("~/en/images/warning_icon.gif") %>" width="16" height="16" alt="Warning Triangle"> 
                    New! The Windows Update Restored Knowledge Base is here!</div>
                <p>
				<font face="Tahoma" style="color: rgb(0, 0, 0); font-family: Tahoma; font-size: 9pt; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: 400; letter-spacing: normal; orphans: 2; text-indent: 0px; text-transform: none; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; white-space: normal; background-color: rgb(255, 255, 255); text-decoration-thickness: initial; text-decoration-style: initial; text-decoration-color: initial; text-align: start;">
				The Windows Update Restored website now has a&nbsp;<a href="<%= ResolveUrl("~/en/support/KbArticles.aspx") %>">Knowledge 
				Base</a>, listing every Support topics we published. If we see 
				that one or more issues are frequent and we're able to fix them, 
				we will add Support topics in the Knowledge Base regarding those 
				issues.</font></p>
            </div>
            <div class="news-item">
                <div class="news-title">
                    <img src="<%= ResolveUrl("~/en/images/warning_icon.gif") %>" width="16" height="16" alt="Warning Triangle">
					<b style="color: rgb(0, 0, 0); font-family: Tahoma; font-size: medium; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; letter-spacing: normal; orphans: 2; text-align: -webkit-left; text-indent: 0px; text-transform: none; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; white-space: normal; text-decoration-thickness: initial; text-decoration-style: initial; text-decoration-color: initial;">
					<font style="font-size: 10pt;">Newly Revamped Help &amp; Support 
					Center</font></b><font style="color: rgb(0, 0, 0); font-family: Tahoma; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: 400; letter-spacing: normal; orphans: 2; text-align: -webkit-left; text-indent: 0px; text-transform: none; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; white-space: normal; text-decoration-thickness: initial; text-decoration-style: initial; text-decoration-color: initial; font-size: 10pt;"><b>!</b></font></div>
                <p>
				<span style="color: rgb(0, 0, 0); font-family: Tahoma; font-size: 12px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: 400; letter-spacing: normal; orphans: 2; text-align: -webkit-left; text-indent: 0px; text-transform: none; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; white-space: normal; background-color: rgb(255, 255, 255); text-decoration-thickness: initial; text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">
				The Help &amp; Support Center is now revamped for a much more easier 
				and enjoyable user experience! We have new pages as well as 
				up-to-date info so you will always be able to find a way to fix 
				your problems while using Windows Update Restored!</span></p>
            </div>
            <div class="news-item">
                <div class="news-title">
                    <img src="<%= ResolveUrl("~/en/images/warning_icon.gif") %>" width="16" height="16" alt="Warning Triangle">
                    Windows Update Restored v5 Troubleshooting Tools are here!</div>
                <p>Ever since the launch of Windows Update Restored v5, users have had plenty of problems when trying to get certain things working with the website on their designated Windows systems. But those problems are one more step closer to being gone with the requirement of needing a proxy to use Windows Update Restored 
				V5 gone! Thanks to WinFX, you no longer need a proxy to use Windows Update Restored v5. Instead, you can connect to 
				<b>v5.windowsupdaterestored</b>.com as a WSUS server instead of Microsoft's. Lastly, Joshua Maitland has developed the Windows Update Restored v5 Checker Tool that will tell you 
				<b>exactly what needs to be patched on your Windows operating system</b> so you can fix them and start using Windows Update Restored! 
				<b>To check out how to apply the Windows Update Restored v5 WSUS patch to you system, visit the 
				</b><a href="<%= ResolveUrl("~/en/prerequisites.htm.tmp.html") %>">P</a><b><a href="<%= ResolveUrl("~/en/prerequisites.htm.tmp.html") %>">rerequisites</a> page.</b>
				<b><a href="<%= ResolveUrl("~/en/downloads/WurV5CheckerTool.aspx") %>">Learn more about the 
				Windows Update Restored V5 Checker Tool.</a></b></p>
            </div>
        </div>
    </main>


    </div>

</asp:Content>
