<%@ Page Title="Home - Windows Update Restored" Language="C#" MasterPageFile="~/en/support/Support.master" AutoEventWireup="true" CodeBehind="PrereqInstDoc.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.en.support.PrereqInstDoc" %>
<%@ Register TagPrefix="uc" TagName="ContentBox" Src="~/Controls/ContentBox.ascx" %>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        .cw <!--
 .smallblack {font-family: verdana, arial, sans-serif; font-size: 8pt; }
        .cw .darkblue {color: #00319C; }
        .cw A:link {color: #00319C; }
        .cw .BodyText {COLOR: black; FONT-FAMILY: Verdana, Arial; FONT-SIZE: 8pt; }
    </style>

    <div class="cw">

 <FONT ID="f1" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <TABLE CELLPADDING="0" CELLSPACING="0" BORDER="0" WIDTH="100%">
 <TR>
 <TD ID="td1" VALIGN="top" ALIGN="left" WIDTH="94">
 <IMG ID="ix1" HEIGHT="100" ALT="" SRC='<%= ResolveUrl("~/en/images/wulgo_only.gif") %>' WIDTH="100" BORDER="0">
 </TD>
 <TD ID="td2" VALIGN="top" ALIGN="left">
 <IMG ID="ix2" HEIGHT="100" ALT="" SRC='<%= ResolveUrl("~/en/images/wurlgo_text.gif") %>' WIDTH="182" BORDER="0">
 <BR>
 <FONT ID="f2" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f3" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <H2>
 <font color="#00008B"><br>
	Prerequisites Installer - Online Documentation</font><FONT COLOR="darkblue">
 <BR>
 <ID ID="x1a"></ID>
 </FONT>
 </H2>
 <uc:ContentBox ID="cbIntro" runat="server" HeaderText="Prerequisites Installer - Online Documentation" HeaderColor="Blue" ContentColor="White" />
 -&gt;
 <a href="<%= ResolveUrl("~/en/downloads/WurV5PatcherTool.aspx") %>"><span style="color: #00319C">
	Prerequisites Installer download page...</span></a><br>
  <br>
  <strong>Switches (command line arguments)<br>
	<font color="#FF0000">These switches only work in version 1.6 of the 
	Prerequisites Installer and newer! Previous versions did not support command 
	line switches, except &quot;/extract&quot;.</font></strong><br>
  <br>
	<u>/extract</u> : Extracts all the contents of the Prerequisites Installer 
	into the letter:\WUR_EXTRACTED (where &quot;letter&quot; is the drive containing 
	Windows, e.g. &quot;C&quot;).<p><u>/auto</u> : Proceeds to an automated install, with 
	options defined in the next command lines switches (all the switches below 
	are mandatory for the automated install):</p>
	<p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - /site : Defines which Windows Update 
	website to use by default in the Start Menu shortcut. Possible sites: V5, 
	V6. Examples: <i>/site:V5</i> or <i>/site:V6<br>
	<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </i>- /mu : Sets if Microsoft Update will be 
	enabled or not during the installation. Possible options: enabled, disabled. 
	Examples: <i>/mu:enabled</i> or <i>/mu:disabled<br>
	<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </i>- /provider : Sets the Update Provider to use 
	when checking for updates. Possible options: LU, WUR.<i> </i>Examples: <i>/provider:LU</i> 
	or <i>/provider:WUR<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </i><b>The Custom Update 
	Provider is not supported in command line use.<br>
	</b><i><br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </i>- /endmsg : Displays a message saying that 
	the installation at the end has finished. This switch being missing will 
	result in a simple program closing at the end of the installation.</p>
	<p>
 <FONT ID="f208" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f209" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f210" FACE="Verdana, Arial, Helvetica" SIZE="2">
 	<i>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </i>- /hidden : Hides the automatic 
	install window.</FONT></FONT></FONT></p>
	<p>
 <FONT ID="f211" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f212" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f213" FACE="Verdana, Arial, Helvetica" SIZE="2">
 	<i>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </i>- /restart : Automatically restarts 
	the computer once the installation is done.</FONT></FONT></FONT></p>
	<p>
	<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Example full command: <i>wurv5patchertool.exe 
	/auto /site:V6 /mu:enabled /provider:LU /endmsg</i></p>
	<p><u>Please note :</u> As the automated install is meant to be used during 
	Windows deployments, the installer will not automatically restart at the end 
	of the installation. You need to use the &quot;/restart&quot; switch.</p>
	<p>
 <FONT ID="f214" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f215" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f216" FACE="Verdana, Arial, Helvetica" SIZE="2">
 	<u>/regmu</u> : Reinstalls the Microsoft Update Restored Web Control onto 
	your computer (Prerequisites Installer v1.7 and newer). <br>
	</FONT></FONT>
 </FONT>
 <FONT ID="f217" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f218" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f219" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f220" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f221" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f222" FACE="Verdana, Arial, Helvetica" SIZE="2">
 	<i>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </i>- /wuaueng:yes : Reinstalls 
	WUAUENG.DLL with the Microsoft Update Restored Web Control. This requires a 
	restart.</FONT></FONT></FONT></FONT></FONT></FONT></p>
	<p>
 <FONT ID="f223" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f224" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f225" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f226" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f227" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f228" FACE="Verdana, Arial, Helvetica" SIZE="2">
 	<u>/wurctlreg</u> : Reinstalls the Windows Update Restored Server Web 
	Control onto your computer (Prerequisites Installer v1.7.1 and newer). Does 
	not require a restart.</FONT></FONT></FONT></FONT></FONT></FONT></p>
	<p>&nbsp;</p>
	<p><i>Some of these tools are also available in the user interface (UI), in 
	About -&gt; Manual Tools.</i></p>
	<p><i>
	<br>
	</i><strong>Installer Contents (Executables)</strong><br>The installer 
	contains multiple EXE files, which are modified updates made to properly 
	patch your system. They're all identified by a file code, visible at the end 
	of each update's EULA.<br>
	Here's the table linking their file code to what they are:<br>
 <FONT ID="f37" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f38" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f39" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f40" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f41" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f42" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f43" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f44" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f45" FACE="Verdana, Arial, Helvetica" SIZE="2">
 	- <b>WURKB_PREREQ_132_5_2K</b>: WUPDMGR.EXE, 32-bit (x86), Windows 2000 only 
	(Windows Update V5) - Filename: <i>WUPDMGRV5.EXE</i><br>
	- <b>WURKB_PREREQ_132_6_2K</b>: </FONT></FONT>
 </FONT></FONT></FONT>
 </FONT>
 <FONT ID="f16" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f17" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f18" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f19" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f20" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f21" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f22" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f23" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f24" FACE="Verdana, Arial, Helvetica" SIZE="2">
 	WUPDMGR.EXE, 32-bit (x86), Windows 2000 only (Windows Update V6) </FONT></FONT>
 </FONT></FONT></FONT>
 </FONT></FONT></FONT>
 </FONT></FONT></FONT>
 </FONT>
 <FONT ID="f67" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f68" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f69" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f70" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f71" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f72" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f73" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f74" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f75" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f76" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f77" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f78" FACE="Verdana, Arial, Helvetica" SIZE="2">
 	- Filename: <i>WUPDMGRV6.EXE</i></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT><FONT ID="f37" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f38" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f39" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f16" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f17" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f18" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f19" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f20" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f21" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f22" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f23" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f24" FACE="Verdana, Arial, Helvetica" SIZE="2"><br>
	- <b>WURKB_PREREQ_132_XP</b>: WUPDMGR.EXE, 32-bit (x86), Windows XP and 
	Server 2003 (no fixed Windows Update version) </FONT></FONT>
 </FONT></FONT></FONT>
 </FONT></FONT></FONT>
 </FONT></FONT></FONT>
 </FONT>
 <FONT ID="f79" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f80" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f81" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f82" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f83" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f84" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f85" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f86" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f87" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f88" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f89" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f90" FACE="Verdana, Arial, Helvetica" SIZE="2">
 	- Filename: <i>WUPDMGRXP32.EXE</i></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT><FONT ID="f37" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f38" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f39" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f16" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f17" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f18" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f19" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f20" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f21" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f22" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f23" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f24" FACE="Verdana, Arial, Helvetica" SIZE="2"><br>
	- <b>WURKB_PREREQ_164_XP</b>: </FONT></FONT>
 </FONT></FONT></FONT>
 </FONT></FONT></FONT>
 </FONT>
 <FONT ID="f46" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f47" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f48" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f49" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f50" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f51" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f52" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f53" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f54" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f55" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f56" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f57" FACE="Verdana, Arial, Helvetica" SIZE="2">
 	WUPDMGR.EXE, 64-bit (x86 AMD64), Windows XP and Server 2003 AMD64 (no fixed 
	Windows Update version) </FONT></FONT>
 </FONT></FONT></FONT>
 </FONT></FONT></FONT>
 </FONT></FONT></FONT>
 </FONT></FONT></FONT>
 </FONT>
 <FONT ID="f91" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f92" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f93" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f94" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f95" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f96" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f97" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f98" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f99" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f100" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f101" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f102" FACE="Verdana, Arial, Helvetica" SIZE="2">
 	- Filename: <i>WUPDMGRXP64.EXE</i></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT><br>
	- <b>WURKB_PREREQ_232_2K</b>: WUAUENG.DLL, 32-bit (x86), Windows 2000 only (requires 
	a reboot at the end of the installation)<FONT ID="f103" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f104" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f105" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f106" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f107" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f108" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f109" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f110" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f111" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f112" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f113" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f114" FACE="Verdana, Arial, Helvetica" SIZE="2"> 
	- Filename: <i>WUAUENG2K.EXE</i></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT><br>
	- <b>WURKB_PREREQ_232_XP</b>: WUAUENG.DLL, 32-bit (x86), Windows XP and 
	Server 2003
 <FONT ID="f7" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f8" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f9" FACE="Verdana, Arial, Helvetica" SIZE="2">
 	(requires a reboot at the end of the installation)</FONT></FONT></FONT><FONT ID="f115" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f116" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f117" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f118" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f119" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f120" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f121" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f122" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f123" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f124" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f125" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f126" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f127" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f128" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f129" FACE="Verdana, Arial, Helvetica" SIZE="2"> 
	- Filename: <i>WUAUENG32.EXE</i></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT><FONT ID="f7" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f8" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f9" FACE="Verdana, Arial, Helvetica" SIZE="2"><br>
	- <b>WURKB_PREREQ_264_XP</b>: WUAUENG.DLL, 64-bit (x86 AMD64), Windows XP 
	and Server 2003 AMD64 </FONT></FONT>
 </FONT>
 <FONT ID="f10" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f11" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f12" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f13" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f14" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f15" FACE="Verdana, Arial, Helvetica" SIZE="2">
 	(requires a reboot at the end of the installation)</FONT></FONT></FONT></FONT></FONT></FONT><FONT ID="f130" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f131" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f132" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f133" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f134" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f135" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f136" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f137" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f138" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f139" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f140" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f141" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f142" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f143" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f144" FACE="Verdana, Arial, Helvetica" SIZE="2"> 
	- Filename: <i>WUAUENG64.EXE</i></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT><FONT ID="f25" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f26" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f27" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f28" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f29" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f30" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f31" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f32" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f33" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f34" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f35" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f36" FACE="Verdana, Arial, Helvetica" SIZE="2"><br>
	- <b>WURKB_PREREQ_332_2K</b>: WUWEB.DLL, 32-bit (x86), Windows 2000 only</FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT><FONT ID="f145" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f146" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f147" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f148" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f149" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f150" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f151" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f152" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f153" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f154" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f155" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f156" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f157" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f158" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f159" FACE="Verdana, Arial, Helvetica" SIZE="2"> 
	- Filename: <i>WUWEB2K.EXE</i></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT><FONT ID="f25" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f26" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f27" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f28" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f29" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f30" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f31" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f32" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f33" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f34" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f35" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f36" FACE="Verdana, Arial, Helvetica" SIZE="2"><br>
	- <b>WURKB_PREREQ_332_XP</b>: WUWEB.DLL, 32-bit (x86), Windows XP and Server 
	2003</FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT><FONT ID="f160" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f161" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f162" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f163" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f164" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f165" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f166" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f167" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f168" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f169" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f170" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f171" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f172" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f173" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f174" FACE="Verdana, Arial, Helvetica" SIZE="2"> 
	- Filename: <i>WUWEB32.EXE</i></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT><FONT ID="f25" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f26" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f27" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f28" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f29" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f30" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f31" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f32" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f33" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f34" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f35" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f36" FACE="Verdana, Arial, Helvetica" SIZE="2"><br>
	- <b>WURKB_PREREQ_364_XP</b>: WUWEB.DLL, 64-bit (x86 AMD64), Windows XP and 
	Server 2003 AMD64 (</FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT><FONT ID="f58" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f59" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f60" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f61" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f62" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f63" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f64" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f65" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f66" FACE="Verdana, Arial, Helvetica" SIZE="2">requires 
	a reboot at the end of the installation)</FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT><FONT ID="f175" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f176" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f177" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f178" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f179" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f180" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f181" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f182" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f183" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f184" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f185" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f186" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f187" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f188" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f189" FACE="Verdana, Arial, Helvetica" SIZE="2"> 
	- Filename: <i>WUWEB64.EXE<br>
	<br>
	</i></FONT></FONT>
 </FONT></FONT></FONT>
 </FONT></FONT></FONT>
 </FONT></FONT></FONT>
 </FONT></FONT></FONT>
 </FONT>
 <FONT ID="f190" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f191" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f192" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f193" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f194" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f195" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f196" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f197" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f198" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f199" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f200" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f201" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f202" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f203" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f204" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f205" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f206" FACE="Verdana, Arial, Helvetica" SIZE="2">
	<FONT ID="f207" FACE="Verdana, Arial, Helvetica" SIZE="2">
 	<i>WUA32.exe</i>: Windows Update Agent 3.0 32-bit (x86)</FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT><FONT ID="f175" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f176" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f177" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f178" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f179" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f180" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f181" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f182" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f183" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f184" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f185" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f186" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f187" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f188" FACE="Verdana, Arial, Helvetica" SIZE="2"><FONT ID="f189" FACE="Verdana, Arial, Helvetica" SIZE="2"><i><br>
	WUA64.exe</i>: Windows Update Agent 3.0 64-bit (x86 AMD64)<br>
	<br>
	<strong>Logging<br></strong>
	The Prerequisites Installer creates a log at the root of the system drive 
	(in most cases, C:\). This log lists every action the installer performed, 
	in order to help bug tracking. The log filename is <i>WURPREREQINST.LOG</i>.<br>
	</FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT></FONT><br><strong>Supported Operating Systems</strong><br>The Prerequisites 
	Installer supports the following operating systems: </p>
  <ul>
    <li>- Microsoft Windows 2000 (Professional, Server, Advanced Server);</li>
	<li>- Microsoft Windows XP (Home, Professional, AMD64);</li>
	<li>- Microsoft Windows Server 2003 (Standard, Enterprise, Datacenter, 
	AMD64);</li>
	<li>- Microsoft Windows Home Server;</li>
	<li>- Microsoft Windows NT 5.0/5.1 based operating systems.</li>
    </ul>
	<p>
 <FONT ID="f4" FACE="Verdana, Arial, Helvetica" SIZE="2">
 <FONT ID="f5" FACE="Verdana, Arial, Helvetica" SIZE="2">
    <FONT ID="f6" FACE="Verdana, Arial, Helvetica" SIZE="2">
    <strong style="color: darkred;">Warning!</strong><br>
    This patch modifies some system files (<i>WUPDMGR.EXE, WUWEB.DLL</i>, <i>
	MUWEB.DLL </i>and<i> WUAUENG.DLL</i>). Please back up any important data (or 
	make a snapshot in case you're using a virtual machine) before installing it 
	on your computer. We do not believe anything should go wrong, but make sure 
	everything is safe before running it.<br>
    The Windows Update Restored Prerequisites Installer cannot be uninstalled (any 
	uninstallation will have to be done manually, or using <i>Add/Remove 
	Programs</i> for the DLLs and <i>WUPDMGR.EXE</i>).<br>
    </FONT></FONT>
 </FONT>
    <br>
    <strong>How does the Prerequisites Installer work?</strong><br>
	The Prerequisites Installer works by only extracting the necessary resources 
	to a temp directory (<i>%temp%\WURPatcherTemp</i>) and then installing/copying 
	each file in its corresponding directory (mainly System32, System32\DLLcache 
	and SysWOW64). It patches the entire operating system, as well as the 
	Windows Update shortcut in the Start Menu.<p>
    We've made sure to use strong and efficient methods to prevent your 
	operating system from being corrupted while patching it, but please report 
	any bug you find to Windows Update Restored, so we can fix them for the next 
	release.<p>
    <u><b>With Legacy Update<br>
    </b></u>Installing Legacy Update even though you've already used the 
	Prerequisites Installer on the same computer shouldn't cause problems (the 
	Prerequisites Installer even offers to the user the ability to use Legacy 
	Update as Update Provider). Bare in mind that, if you chose to install 
	Legacy Update and decide to uninstall it while still having this patch 
	installed with Legacy Update selected as Update Provider, you might get 
	errors when trying to run Windows Update Restored V5/V6, as the service will 
	run on Legacy Update in that case.<p>
    <u><b>Windows 2000 Users<br>
    </b></u>The Prerequisites Installer supports all editions of Windows 2000 
	running Service Pack 3 or Service Pack 4. <br>
    Using version 1.5, Windows 2000 users may recieve one or more Windows File 
	Protection warnings telling them that system files have been modified, and 
	need to be replaced using the Windows 2000 CD-ROM. <b>In order for the 
	installation to take effect, please always click &quot;Cancel&quot; and &quot;Yes&quot; at each 
	prompt you see, <u>otherwise the patch won't work</u>. </b>Newer versions 
	are not affected by this problem.<p>
    <u><b>Windows XP x64 and Windows Server 2003 x64 users<br>
    </b></u>On Windows XP x64 and Windows Server 2003 x64 systems, the 
	Prerequisites Installer (version 1.5 and newer) will always require a restart at the 
	end of the installation.</FONT></FONT></TD>
 </TR>
 <TR>
 <TD ID="td1" VALIGN="top" ALIGN="left" WIDTH="94">
 &nbsp;</TD>
 <TD ID="td2" VALIGN="top" ALIGN="left">
 &nbsp;</TD>
 </TR>
 </TABLE>
 </FONT>
 <BR>
 <CENTER>
 &nbsp;&nbsp;&nbsp; <i>This page is based on the very first homepage of Windows 
	Update Restored!</i><BR>
 </CENTER>
 
    </div>

</asp:Content>
