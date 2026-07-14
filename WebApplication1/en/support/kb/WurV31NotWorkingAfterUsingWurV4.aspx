<%@ Page Title="Windows Update Restored v3.1 no longer works after scanning for updates on Windows Update Restored v4" Language="C#" MasterPageFile="~/en/support/Support.master" AutoEventWireup="true" CodeBehind="WurV31NotWorkingAfterUsingWurV4.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.en.support.kb.WurV31NotWorkingAfterUsingWurV4" %>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        
    </style>

    <div class="cw">


    <header>
        <h1 style="font-size: 16pt; color: #000080; border-bottom: 2px solid #000080; padding-bottom: 7px; margin: 0">
            Windows Update Restored v3.1 no longer works after scanning for updates on Windows Update Restored v4</h1>

        <p style="font-size: 10pt; margin: 10px 0;">
            <b>Article Published: 04/19/2024</b>
        </p>
        <p><font size="2"><b>Summary</b><br>
		When you scan for updates on Windows Update Restored v4, ActiveX 
		controls for the website are loaded and registry entries are added to 
		your system, preventing you to scan for updates with Windows Update 
		Restored v3.1.<br>
		<br>
		<b>Notes</b><br>
		This is not a permanent fix, and you will run into this issue every 
		single time you check for updates on Windows Update Restored v4.<br>
		<br>
		<b>Instructions</b><br>
		Follow these instructions to get back the ability to check for updates 
		on the Windows Update Restored v3.1 website:</font></p>
<ol>
	<li><font size="2">Open <b>&quot;regedit.exe&quot;</b></font></li>
	<li><font size="2">Go to <b>HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet 
	Explorer\ActiveX Compatibility</b></font></li>
	<li><font size="2">Delete <b>
	{CEBC955E-58AF-11D2-A30A-00A0C903492B}</b></font></li>
	<li><font size="2">Open <b>Windows Update Restored v3.1</b> to scan for 
	updates.</font></li>
</ol>
    
    <hr style="height: 1px; border: 0 none; margin: 10px 0; background: #000080">
        
    <main>
        <p style="font-size: 13px;">If you have any more questions or need further assistance, feel free to join our Discord server by 
        <a href="https://discord.gg/Aqp4GgnWAg" target="_blank" style="color: #000080; text-decoration: none">clicking here</a> and you can get help from our Discord members.</p>
    </main>
    </header>


    </div>

</asp:Content>
