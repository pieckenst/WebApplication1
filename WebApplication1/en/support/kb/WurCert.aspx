<%@ Page Title="You get certificate warnings in Internet Explorer" Language="C#" MasterPageFile="~/en/support/Support.master" AutoEventWireup="true" CodeBehind="WurCert.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.en.support.kb.WurCert" %>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        
    </style>

    <div class="cw">


    <header>
        <h1 style="font-size: 16pt; color: #000080; border-bottom: 2px solid #000080; padding-bottom: 7px; margin: 0;">
            You get certificate warnings in Internet Explorer</h1>

        <p style="font-size: 10pt; margin: 10px 0;">
            <b>Article Published: 04/17/2024</b>
        </p>
        <p><font size="2"><b>Summary</b><br>
        Windows Update Restored is using self-signed certificates to sign patched CABs and installers. This is the fix to remove the 
        certificate warnings in Internet Explorer. <i>Also make sure that your 
		computer has the right time and date. This is crucial for the fix to 
		work.</i><br>
        <br>
        <b>Notes</b><br>
        This is a permanent fix that will help you get rid of the Internet Explorer certificate warnings about Windows Update Restored.<br>
        <br>
        <b>Instructions</b><br>
        Follow the instructions below to help you remove the certificate warnings in Internet Explorer when you use Windows Update Restored:</font></p>
        <ol>
            <li><font size="2">Download the Windows Update Restored certificate (registry file) 
			<a style="color: #000080; text-decoration: none;" href="<%= ResolveUrl("~/en/downloads/Certificates.aspx") %>">from the Download Center</a>.</font></li>
            <li><font size="2">Import it into the Windows Registry by double-clicking on the file.</font></li>
            <li><font size="2">Restart your computer.</font></li>
            <li><font size="2">There you go! You should now be able to check "Always accept ActiveX controls from Windows Update Restored" and no longer be bothered with those warnings.</font></li>
        </ol>
        
        <hr style="height: 1px; border: 0 none; margin: 10px 0; background: #000080">
            
        <main>
            <p style="font-size: 13px;">If you have any more questions or need further assistance, feel free to join our Discord server by 
            <a href="https://discord.gg/Aqp4GgnWAg" target="_blank" style="color: #000080; text-decoration: none;">clicking here</a> and you can get help from our Discord members.</p>
        </main>
    </header>


    </div>

</asp:Content>
