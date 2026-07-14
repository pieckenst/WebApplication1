<%@ Page Title="Service Pack Center" Language="C#" MasterPageFile="~/en/download-center/DownloadCenter.master" AutoEventWireup="true" CodeBehind="SpCenter.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.en.downloads.SpCenter" %>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        .cw {font-family: Arial;
			font-size: small; }
        .cw table {font-size: small; }
        .cw a {color: #00319C; }
    </style>

    <div class="cw">

	<table CELLPADDING="0" CELLSPACING="0" BORDER="0" WIDTH="100%">
		<tr>
			<td ID="td1" VALIGN="top" ALIGN="left" WIDTH="94"><IMG ID="ix3" ALT=""
					SRC='<%= ResolveUrl("~/en/images/wulgo_only.gif") %>' WIDTH="100" BORDER="0"> </td>
			<td ID="td2" VALIGN="top" ALIGN="left"><IMG ID="ix2" ALT=""
					SRC='<%= ResolveUrl("~/en/images/wurlgo_text.gif") %>' WIDTH="188" BORDER="0"> <br>
				<h2 style="color: darkblue; font-size: 18pt;"> Windows Service Packs Center <br>
					<ID ID="x1a"></ID>
				</h2>
				<p align="justify">
					<id id="x2"> This page consists of Service Pack downloads for Windows.</id><br> <i>Please note that all the
						downloads we offer here were taken from download.microsoft.com or 
					the Wayback Machine, and are only hosted by
						Windows Update Restored. We did not modify any file 
					listed here.</i><br> <br> <b>Caption:<br> </b>&nbsp;-&nbsp;<span
						style="color: red;">Not available:</span> The download is not available because we haven't found the files, 
					or the file was never officially released by Microsoft.
					You can help us finding these files so we can add the download to the 
					table.<br> &nbsp;- SP1 / SP2 / SP3:
					Click the version you want to download next to its language to download it.&nbsp;<i>For example, clicking SP3
						next to French in the Microsoft Windows 2000 table will download the following: Microsoft Windows 2000 SP3
						French.</i><i><br> <br> </i><b>Select a&nbsp;</b><b>Service Pack&nbsp;version in the list:</b><br>
					&nbsp;-&nbsp;<a href="#WIN95">Microsoft Windows 95 (SP1)</a><br>
					&nbsp;-&nbsp;<a href="#WINNT40">Microsoft Windows NT 4.0 (SP1 | SP2 | SP3 |
						SP4 | SP5 | SP6a)</a><br>
					&nbsp;-&nbsp;<a href="#WIN98">Microsoft Windows 98 (RTM | Y2K Update 1 | Y2K
						Update 2)</a><br>
					&nbsp;-&nbsp;<a href="#WIN2K">Microsoft Windows 2000 (SP1 | SP2 | SP3 |
						SP4 | UR1)</a><br>
					&nbsp;-&nbsp;<a href="#WINXP">Microsoft Windows XP x86 (SP1 | SP2
						| SP3)</a><br>
&nbsp;- <a href="#WINXP64">Microsoft Windows XP AMD64 &amp; IA-64 (SP2)</a><br>
					<br>
					&nbsp;
				</p>
				<hr>
				<p> <u>
						<id id="WIN95"><strong>Microsoft Windows 95&nbsp;(Service Pack 1):</strong></id>
					</u><br>
					<id id="x3"><i>These files are hosted on the Windows Update Restored servers.</i></id>
				</p>
				<table border="1" width="350px" style="border-collapse: collapse;">
					<tr>
						<td width="15%">
							<p align="center"><u><b>Language</b></u>
						</td>
						<td>
							<p align="center"><u><b>Service Pack</b></u><b><u> Version</u></b>
						</td>
					</tr>
					<tr>
						<td width="15%">Arabic</td>
						<td width="12%" align="center">
							<span style="color: red;">Not available</span>
						</td>
					</tr>
					<tr>
						<td width="15%">Chinese (Taiwan)</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win95/sp1/win40sp1_tw.exe">SP1</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Chinese (Simplified)</td>
						<td width="12%" align="center">
							<span style="color: red;">Not available</span>
						</td>
					</tr>
					<tr>
						<td width="15%">Czech</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win95/sp1/win40sp1_cs.exe">SP1</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Danish</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win95/sp1/win40sp1_da.exe">SP1</a>
						</td>
					</tr>
					<tr>
						<td width="15%">German</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win95/sp1/win40sp1_de.exe">SP1</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Greek</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win95/sp1/win40sp1_el.exe">SP1</a>
						</td>
					</tr>
					<tr>
						<td width="15%">English</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win95/sp1/win40sp1_en.exe">SP1</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Spanish</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win95/sp1/win40sp1_es.exe">SP1</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Finnish</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win95/sp1/win40sp1_fi.exe">SP1</a>
						</td>
					</tr>
					<tr>
						<td width="15%">French</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win95/sp1/win40sp1_fr.exe">SP1</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Hebrew</td>
						<td width="12%" align="center">
							<span style="color: red;">Not available</span>
						</td>
					</tr>
					<tr>
						<td width="15%" height="23">Hungarian</td>
						<td width="12%" align="center" height="23">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win95/sp1/win40sp1_hu.exe">SP1</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Italian</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win95/sp1/win40sp1_it.exe">SP1</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Japanese</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win95/sp1/win40sp1_ja.exe">SP1</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Korean</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win95/sp1/win40sp1_ko.exe">SP1</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Dutch</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win95/sp1/win40sp1_nl.exe">SP1</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Norwegian</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win95/sp1/win40sp1_no.exe">SP1</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Polish</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win95/sp1/win40sp1_pl.exe">SP1</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Portuguese (Brazilian)</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win95/sp1/win40sp1_br.exe">SP1</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Portuguese</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win95/sp1/win40sp1_pt.exe">SP1</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Russian</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win95/sp1/win40sp1_ru.exe">SP1</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Slovenian</td>
						<td width="12%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win95/sp1/win40sp1_sl.exe">SP1</a></td>
					</tr>
					<tr>
						<td width="15%">Swedish</td>
						<td width="12%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win95/sp1/win40sp1_sv.exe">SP1</a>
						</td>
					</tr>
					<tr>
						<td width="15%"><span style="background-color: #FFFFFF"> Thai</span></td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win95/sp1/win40sp1_th.exe">SP1</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Turkish</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win95/sp1/win40sp1_tr.exe">SP1</a>
						</td>
					</tr>
				</table>
				<p> <u>
						<id id="WINNT40"><strong>Microsoft Windows NT 4.0&nbsp;(SP1 | SP2 | SP3 | SP4 | SP5 | SP6a):</strong></id>
					</u><br>
					<id id="x11">The Service Packs listed are 40-bit unless mentioned otherwise.</id><id id="x19"><i> 
				Italic</i> text indicates 128-bit.</id><id id="x11"><br>
				</id><b>Note:</b> Windows NT
					4.0 <u>T</u>erminal <u>S</u>erver (TS) Edition utilizes Service Packs specifically designed for itself.<id id="x11">
						<br>
					</id>
					<id id="x11"><i>These files are hosted on the Windows Update Restored servers.</i></id>
				</p>
				<table border="1" width="533" style="border-collapse: collapse;">
					<tr>
						<td width="18%">
							<p align="center"><u><b>Language</b></u>
						</td>
						<td colspan="6">
							<p align="center"><u><b>Service Pack</b></u><b><u> Version</u></b>
						</td>
					</tr>
					<tr>
						<td width="18%">Arabic</td>
						<td width="11%" align="center">SP1</td>
						<td width="9%" align="center">SP2</td>
						<td width="10%" align="center">SP3</td>
						<td width="12%" align="center">SP4</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp5/winnt40sp5-ar.EXE">SP5</a>
						</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp6a/sp6i386-ar.exe">SP6a</a>
						</td>
					</tr>
					<tr>
						<td width="18%">Chinese (Taiwan)</td>
						<td width="11%" align="center">SP1</td>
						<td width="9%" align="center">SP2</td>
						<td width="10%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp3/winnt40sp3-zhtw.exe">SP3</a>
						</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp4/winnt40sp4-ZHTW.exe">SP4</a>
						</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp5/winnt40sp5-zhtw.EXE">SP5</a>
						</td>
						<td width="18%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/cabpool/servicepacks/winnt40/sp6a/sp6i386-tw.exe">SP6a</a></td>
					</tr>
					<tr>
						<td width="18%">Chinese (Simplified)</td>
						<td width="11%" align="center">SP1</td>
						<td width="9%" align="center">SP2</td>
						<td width="10%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp3/winnt40sp3-zhcn.exe">SP3</a>
						</td>
						<td width="12%" align="center">SP4</td>
						<td width="12%" align="center">SP5</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp6a/SP6I386-cn.EXE">SP6a</a>
						</td>
					</tr>
					<tr>
						<td width="18%">Czech</td>
						<td width="11%" align="center">SP1</td>
						<td width="9%" align="center">SP2</td>
						<td width="10%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp3/winnt40sp3-cs.exe">SP3</a>
						</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp4/winnt40sp4-CS.exe">SP4</a>
						</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp5/winnt40sp5-cs.EXE">SP5</a>
						</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp6a/sp6i386-cs.exe">SP6a</a>
						</td>
					</tr>
					<tr>
						<td width="18%">Danish</td>
						<td width="11%" align="center">SP1</td>
						<td width="9%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp2/winnt40sp2-da.EXE">SP2</a>
						</td>
						<td width="10%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp3/winnt40sp3-da.exe">SP3</a>
						</td>
						<td width="12%" align="center">SP4</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp5/winnt40sp5-da.EXE">SP5</a>
						</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp6a/sp6i386-da.exe">SP6a</a>
						</td>
					</tr>
					<tr>
						<td width="18%">German</td>
						<td width="11%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp1/SP1_400I-DE.exe">SP1</a>
						</td>
						<td width="9%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp2/winnt40sp2-de.exe">SP2</a>
						</td>
						<td width="10%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp3/winnt40sp3-de.exe">SP3</a>
						</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp4/winnt40sp4-de.EXE">SP4</a>
						</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp5/winnt40sp5-de.EXE">SP5</a>
						</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp6a/sp6i386-de.exe">SP6a</a>
						</td>
					</tr>
					<tr>
						<td width="18%">Greek</td>
						<td width="11%" align="center">SP1</td>
						<td width="9%" align="center">SP2</td>
						<td width="10%" align="center">SP3</td>
						<td width="12%" align="center">SP4</td>
						<td width="12%" align="center">SP5</td>
						<td width="18%" align="center">SP6a</td>
					</tr>
					<tr>
						<td width="18%">English</td>
						<td width="11%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp1/SP1_400I.EXE">SP1</a>
						</td>
						<td width="9%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp2/winnt40sp2-en.exe">SP2</a>
						</td>
						<td width="10%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp3/winnt40sp3-en.exe">SP3</a>
						</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp4/winnt40sp4-en.EXE">SP4</a><br>
							<br>
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40ts/sp4/wtsi386.exe">
							SP4 (TS)</a></td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp5/winnt40sp5-en.EXE">SP5</a><br>
							<br>
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40ts/sp5/wtsi386.exe">
							SP5 (TS)</a>
						</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp6a/sp6ai386-en.exe">SP6a</a><br>
							<br>
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40ts/sp6/wtsi386.exe">
							SP6 (TS)</a>
						</td>
					</tr>
					<tr>
						<td width="18%">Spanish</td>
						<td width="11%" align="center">SP1</td>
						<td width="9%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp2/winnt40sp2-es.exe">SP2</a>
						</td>
						<td width="10%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp3/winnt40sp3-es.exe">SP3</a>
						</td>
						<td width="12%" align="center">SP4</td>
						<td width="12%" align="center">SP5</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp6a/SP6ai386-es.exe">SP6a</a>
						</td>
					</tr>
					<tr>
						<td width="18%">Finnish</td>
						<td width="11%" align="center">SP1</td>
						<td width="9%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp2/winnt40sp2-fi.exe">SP2</a>
						</td>
						<td width="10%" align="center">SP3</td>
						<td width="12%" align="center">SP4</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp5/winnt40sp5-fi.EXE">SP5</a>
						</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp6a/sp6i386-fi.exe">SP6a</a>
						</td>
					</tr>
					<tr>
						<td width="18%">French</td>
						<td width="11%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp1/SP1_400I-FR.EXE">SP1</a>
						</td>
						<td width="9%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp2/winnt40sp2-fr.EXE">SP2</a>
						</td>
						<td width="10%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp3/winnt40sp3-fr.exe">SP3</a>
						</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp4/winnt40sp4-FR.exe">SP4</a>
						</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp5/winnt40sp5-fr.exe">SP5</a>
						</td>
						<td width="18%" align="center">
							<i>
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp6a/sp6ai386-fr.exe">SP6a</a></i>
						</td>
					</tr>
					<tr>
						<td width="18%">Hebrew</td>
						<td width="11%" align="center">SP1</td>
						<td width="9%" align="center">SP2</td>
						<td width="10%" align="center">SP3</td>
						<td width="12%" align="center">SP4</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp5/winnt40sp5-he.EXE">SP5</a>
						</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp6a/sp6i386-he.exe">SP6a</a>
						</td>
					</tr>
					<tr>
						<td width="18%">Hungarian</td>
						<td width="11%" align="center">SP1</td>
						<td width="9%" align="center">SP2</td>
						<td width="10%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp3/winnt40sp3-hu.exe">SP3</a>
						</td>
						<td width="12%" align="center">SP4</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp5/winnt40sp5-hu.EXE">SP5</a>
						</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp6a/sp6i386-hu.exe">SP6a</a>
						</td>
					</tr>
					<tr>
						<td width="18%">Italian</td>
						<td width="11%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp1/SP1_400I-it.EXE">SP1
						</td>
						<td width="9%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp2/winnt40sp2-it.EXE">SP2</a>
						</td>
						<td width="10%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp3/winnt40sp3-it.EXE">SP3</a>
						</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp4/winnt40sp4-it.EXE">SP4</a>
						</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp5/winnt40sp5-it.EXE">SP5</a>
						</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp6a/SP6ai386-it.exe">SP6a</a>
						</td>
					</tr>
					<tr>
						<td width="18%">Japanese</td>
						<td width="11%" align="center">SP1</td>
						<td width="9%" align="center">SP2</td>
						<td width="10%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp3/winnt40sp3-JA.exe">SP3</a>
						</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp4/winnt40sp4-ja.EXE">SP4</a>
						</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp5/winnt40sp5-ja.EXE">SP5</a>
						</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp6a/SP6I386-JA.EXE">SP6a</a>
						</td>
					</tr>
					<tr>
						<td width="18%">Korean</td>
						<td width="11%" align="center">SP1</td>
						<td width="9%" align="center">SP2</td>
						<td width="10%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp3/winnt40sp3-ko.exe">SP3</a>
						</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp4/winnt40sp4-ko.EXE">SP4</a>
						</td>
						<td width="12%" align="center">SP5</td>
						<td width="18%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/cabpool/servicepacks/winnt40/sp6a/sp6i386-ko.exe">SP6a</a></td>
					</tr>
					<tr>
						<td width="18%">Dutch</td>
						<td width="11%" align="center">SP1</td>
						<td width="9%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp2/winnt40sp2-nl.EXE">SP2</a>
						</td>
						<td width="10%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp3/winnt40sp3-nl.exe">SP3</a>
						</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp4/winnt40sp4-nl.EXE">SP4</a>
						</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp5/winnt40sp5-nl.EXE">SP5</a>
						</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp6a/sp6i386-nl.exe">SP6a</a>
						</td>
					</tr>
					<tr>
						<td width="18%">Norwegian</td>
						<td width="11%" align="center">SP1</td>
						<td width="9%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp2/winnt40sp2-no.EXE">SP2</a>
						</td>
						<td width="10%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp3/winnt40sp3-no.exe">SP3</a>
						</td>
						<td width="12%" align="center">SP4</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp5/winnt40sp5-no.EXE">SP5</a>
						</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp6a/sp6i386-no.exe">SP6a</a>
						</td>
					</tr>
					<tr>
						<td width="18%">Polish</td>
						<td width="11%" align="center">SP1</td>
						<td width="9%" align="center">SP2</td>
						<td width="10%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp3/winnt40sp3-pl.exe">SP3</a>
						</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp4/winnt40sp4-pl.EXE">SP4</a>
						</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp5/winnt40sp5-pl.EXE">SP5</a>
						</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp6a/SP6I386-PL.EXE">SP6a</a>
						</td>
					</tr>
					<tr>
						<td width="18%">Portuguese (Brazilian)</td>
						<td width="11%" align="center">SP1</td>
						<td width="9%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp2/winnt40sp2-ptbr.EXE">SP2</a>
						</td>
						<td width="10%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp3/winnt40sp3-ptbr.exe">SP3</a>
						</td>
						<td width="12%" align="center">SP4</td>
						<td width="12%" align="center">SP5</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp6a/sp6i386-ptbr.exe">SP6a</a>
						</td>
					</tr>
					<tr>
						<td width="18%">Portuguese</td>
						<td width="11%" align="center">SP1</td>
						<td width="9%" align="center">SP2</td>
						<td width="10%" align="center">SP3</td>
						<td width="12%" align="center">SP4</td>
						<td width="12%" align="center">SP5</td>
						<td width="18%" align="center">SP6a</td>
					</tr>
					<tr>
						<td width="18%">Russian</td>
						<td width="11%" align="center">SP1</td>
						<td width="9%" align="center">SP2</td>
						<td width="10%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp3/winnt40sp3-ru.exe">SP3</a>
						</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp4/winnt40sp4-ru.EXE">SP4</a>
						</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp5/winnt40sp5-ru.EXE">SP5</a>
						</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp6a/sp6ai386-ru.exe">SP6a</a>
						</td>
					</tr>
					<tr>
						<td width="18%">Swedish</td>
						<td width="11%" align="center">SP1</td>
						<td width="9%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp2/winnt40sp2-sv.EXE">SP2</a>
						</td>
						<td width="10%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp3/winnt40sp3-sv.exe">SP3</a>
						</td>
						<td width="12%" align="center">SP4</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp5/winnt40sp5-sv.EXE">SP5</a>
						</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp6a/sp6i386-sv.exe">SP6a</a>
						</td>
					</tr>
					<tr>
						<td width="18%">Thai</td>
						<td width="11%" align="center">SP1</td>
						<td width="9%" align="center">SP2</td>
						<td width="10%" align="center">SP3</td>
						<td width="12%" align="center">SP4</td>
						<td width="12%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winnt40/sp5/winnt40sp5-th.EXE">SP5</a>
						</td>
						<td width="18%" align="center">SP6a</td>
					</tr>
					<tr>
						<td width="18%">Turkish</td>
						<td width="11%" align="center">SP1</td>
						<td width="9%" align="center">SP2</td>
						<td width="10%" align="center">SP3</td>
						<td width="12%" align="center">SP4</td>
						<td width="12%" align="center">SP5</td>
						<td width="18%" align="center">SP6a</td>
					</tr>
				</table>
				<p> <u>
						<id id="WIN98"><strong>Microsoft Windows 98&nbsp;(Service Pack 1):</strong></id>
					</u><br>
					<id id="x12"><i>These files are hosted on the Windows Update Restored servers.</i></id>
				</p>
				<table border="1" width="441" style="border-collapse: collapse;">
					<tr>
						<td width="15%">
							<p align="center"><u><b>Language</b></u>
						</td>
						<td colspan="3">
							<p align="center"><u><b>Service Pack</b></u><b><u> Version</u></b>
						</td>
					</tr>
					<tr>
						<td width="15%">Arabic</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/ar/wucsp.exe">SP1</a>
						</td>
						<td width="28%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/ar/y2k.exe">Y2K
								Update 1</a>
						</td>
						<td width="25%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/ar/Y2KW98_2.EXE">Y2K
								Update 2</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Chinese (Simplified)</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/cn/wucsp.exe">SP1</a>
						</td>
						<td width="28%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/cn/y2k.exe">Y2K
								Update 1</a>
						</td>
						<td width="25%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/cn/Y2KW98_2.EXE">Y2K
								Update 2</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Chinese (Taiwan)</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/tw/wucsp.exe">SP1</a>
						</td>
						<td width="28%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/tw/y2k.exe">Y2K
								Update 1</a>
						</td>
						<td width="25%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/tw/Y2KW98_2.EXE">Y2K
								Update 2</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Czech</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/cs/wucsp.exe">SP1</a>
						</td>
						<td width="28%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/cs/y2k.exe">Y2K
								Update 1</a>
						</td>
						<td width="25%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/cs/Y2KW98_2.EXE">Y2K
								Update 2</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Danish</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/da/wucsp.exe">SP1</a>
						</td>
						<td width="28%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/nl/Y2K.EXE">Y2K
								Update 1</a>
						</td>
						<td width="25%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/nl/Y2KW98_2.EXE">Y2K
								Update 2</a>
						</td>
					</tr>
					<tr>
						<td width="15%">German</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/de/wucsp.exe">SP1</a>
						</td>
						<td width="28%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/de/y2k.exe">Y2K
								Update 1</a>
						</td>
						<td width="25%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/de/Y2KW98_2.EXE">Y2K
								Update 2</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Greek</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/el/wucsp.exe">SP1</a>
						</td>
						<td width="28%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/el/y2k.exe">Y2K
								Update 1</a>
						</td>
						<td width="25%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/el/Y2KW98_2.EXE">Y2K
								Update 2</a>
						</td>
					</tr>
					<tr>
						<td width="15%">English</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/en/wucsp.exe">SP1</a>
						</td>
						<td width="28%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/en/y2k.exe">Y2K
								Update 1</a>
						</td>
						<td width="25%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/en/Y2KW98_2.EXE">Y2K
								Update 2</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Spanish</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/es/wucsp.exe">SP1</a>
						</td>
						<td width="28%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/es/y2k.exe">Y2K
								Update 1</a>
						</td>
						<td width="25%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/fi/Y2KW98_2.EXE">Y2K
								Update 2</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Finnish</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/fi/wucsp.exe">SP1</a>
						</td>
						<td width="28%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/fi/y2k.exe">Y2K
								Update 1</a>
						</td>
						<td width="25%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/fi/Y2KW98_2.EXE">Y2K
								Update 2</a>
						</td>
					</tr>
					<tr>
						<td width="15%">French</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/fr/wucsp.exe">SP1</a>
						</td>
						<td width="28%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/fr/y2k.exe">Y2K
								Update 1</a>
						</td>
						<td width="25%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/fr/Y2KW98_2.EXE">Y2K
								Update 2</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Hebrew</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/he/wucsp.exe">SP1</a>
						</td>
						<td width="28%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/he/y2k.exe">Y2K
								Update 1</a>
						</td>
						<td width="25%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/he/Y2KW98_2.EXE">Y2K
								Update 2</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Hungarian</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/hu/wucsp.exe">SP1</a>
						</td>
						<td width="28%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/hu/y2k.exe">Y2K
								Update 1</a>
						</td>
						<td width="25%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/hu/Y2KW98_2.EXE">Y2K
								Update 2</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Italian</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/it/wucsp.exe">SP1</a>
						</td>
						<td width="28%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/it/Y2K.EXE">Y2K
								Update 1</a>
						</td>
						<td width="25%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/it/Y2KW98_2.EXE">Y2K
								Update 2</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Japanese</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/ja/wucsp.exe">SP1</a>
						</td>
						<td width="28%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/ja/Y2K.EXE">Y2K
								Update 1</a>
						</td>
						<td width="25%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/ja/Y2KW98_2.EXE">Y2K
								Update 2</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Korean</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/ko/wucsp.exe">SP1</a>
						</td>
						<td width="28%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/ko/Y2K.EXE">Y2K
								Update 1</a>
						</td>
						<td width="25%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/ko/Y2KW98_2.EXE">Y2K
								Update 2</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Dutch</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/nl/wucsp.exe">SP1</a>
						</td>
						<td width="28%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/nl/Y2K.EXE">Y2K
								Update 1</a>
						</td>
						<td width="25%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/nl/Y2KW98_2.EXE">Y2K
								Update 2</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Norwegian</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/no/wucsp.exe">SP1</a>
						</td>
						<td width="28%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/no/Y2K.EXE">Y2K
								Update 1</a>
						</td>
						<td width="25%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/no/Y2KW98_2.EXE">Y2K
								Update 2</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Polish</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/pl/wucsp.exe">SP1</a>
						</td>
						<td width="28%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/pl/Y2K.EXE">Y2K
								Update 1</a>
						</td>
						<td width="25%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/pl/Y2KW98_2.EXE">Y2K
								Update 2</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Portuguese (Brazilian)</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/pt-br/wucsp.exe">SP1</a>
						</td>
						<td width="28%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/pt-br/Y2K.EXE">Y2K
								Update 1</a>
						</td>
						<td width="25%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/pt-br/Y2KW98_2.EXE">Y2K
								Update 2</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Portuguese</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/pt/WUCSP.EXE">SP1</a>
						</td>
						<td width="28%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/pt/Y2K.EXE">Y2K
								Update 1</a>
						</td>
						<td width="25%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/pt/Y2KW98_2.EXE">Y2K
								Update 2</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Russian</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/ru/wucsp.exe">SP1</a>
						</td>
						<td width="28%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/ru/Y2K.EXE">Y2K
								Update 1</a>
						</td>
						<td width="25%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/ru/Y2KW98_2.EXE">Y2K
								Update 2</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Swedish</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/sv/wucsp.exe">SP1</a>
						</td>
						<td width="28%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/sv/Y2K.EXE">Y2K
								Update 1</a>
						</td>
						<td width="25%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/sv/Y2KW98_2.EXE">Y2K
								Update 2</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Thai</td>
						<td width="18%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/th/wucsp.exe">SP1</a></td>
						<td width="28%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/th/y2k.exe">Y2K Update 1</a></td>
						<td width="25%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/th/Y2KW98_2.exe">Y2K Update 2</a></td>
					</tr>
					<tr>
						<td width="15%">Turkish</td>
						<td width="18%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/tr/wucsp.exe">SP1</a>
						</td>
						<td width="28%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/tr/Y2K.EXE">Y2K
								Update 1</a></td>
						<td width="25%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win98/sp1/tr/Y2KW98_2.EXE">Y2K
								Update 2</a>
						</td>
					</tr>
				</table>
				<p> <u>
						<id id="WIN2K"><strong>Microsoft Windows 2000&nbsp;(SP1 | SP2 | SP3 | SP4 
				| UR1):</strong></id>
					</u><br>
					<id id="x13"><i>These files are hosted on the Windows Update Restored servers.</i></id>
				<i><br>
				</i>&quot;UR1&quot; stands for <u>U</u>pdate <u>R</u>ollup <u>1</u>&quot;.<br>
					<id id="x15"> <span
							style="color: red; font-style: italic;">Red text indicates that the file was never officially released by
							Microsoft.</span></id>
				</p>
				<table border="1" width="441" style="border-collapse: collapse;">
					<tr>
						<td width="15%">
							<p align="center"><u><b>Language</b></u>
						</td>
						<td colspan="4">
							<p align="center"><u><b>Service Pack</b></u><b><u> Version</u></b>
						</td>
						<td width="25%">
							<p align="center"><u><b>Update Rollup 1</b></u></p>
						</td>
					</tr>
					<tr>
						<td width="15%">Arabic</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp1/w2ksp1_ar.exe">SP1</a></td>
						<td width="9%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp2/w2ksp2_ar.exe">SP2</a></td>
						<td width="10%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp3/w2ksp3_ar.exe">SP3</a></td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp4/w2ksp4_ar.exe">
						SP4</a>
						</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v1/windows2000-kb891861-x86-ara_4fff6b16b06d1caba65dbe822e2a48b48b5f2b03.exe">Ver 1</a> | 
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v2/windows2000-kb891861-v2-x86-ara_73c7fa3d013476d3a2b8ff8569e3bc82f1ad1582.exe">Ver 2</a></td>
					</tr>
					<tr>
						<td width="15%">Chinese (Hong Kong)</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp1/w2ksp1_zhhk.exe">SP1</a></td>
						<td width="9%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp2/w2ksp2_zhhk.exe">SP2</a></td>
						<td width="10%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp3/w2ksp3_zhhk.EXE">SP3</a></td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp4/w2ksp4_zhhk.exe">
						SP4</a>
						</td>
						<td width="11%" align="center">
						<font color="#FF0000">Not available</font></td>
					</tr>
					<tr>
						<td width="15%">Chinese (Taiwan)</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp1/w2ksp1_zhtw.exe">SP1</a></td>
						<td width="9%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp2/w2ksp2_zhtw.exe">SP2</a></td>
						<td width="10%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp3/w2ksp3_zhtw.exe">SP3</a></td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp4/w2ksp4_zhtw.exe">
						SP4</a>
						</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v1/windows2000-kb891861-x86-cht_f4b5784c8d88177a862b37532b2cf3364d067475.exe">Ver 1</a> | 
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v2/windows2000-kb891861-v2-x86-cht_4b7c5abb4e313f5cbd5f139ea7094145e2a80b5a.exe">Ver 2</a></td>
					</tr>
					<tr>
						<td width="15%">Chinese (Simplified)</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp1/w2ksp1_zhcn.exe">SP1</a></td>
						<td width="9%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp2/w2ksp2_zhcn.exe">SP2</a></td>
						<td width="10%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp3/w2ksp3_zhcn.exe">SP3</a></td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp4/w2ksp4_zhcn.exe">
						SP4</a>
						</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v1/windows2000-kb891861-x86-chs_a63ca7ccb48a0f5f976cc1254ffc69ec4327d4bf.exe">Ver 1</a> |
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v2/windows2000-kb891861-v2-x86-chs_fe99e12ef1944cc43ad2891797ac9a7698e194d9.exe">Ver 2</a></td>
					</tr>
					<tr>
						<td width="15%">Czech</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp1/w2ksp1_cs.exe">SP1</a></td>
						<td width="9%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp2/w2ksp2_cs.exe">SP2</a></td>
						<td width="10%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp3/w2ksp3_cs.exe">SP3</a></td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp4/w2ksp4_cs.exe">
						SP4</a>
						</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v1/windows2000-kb891861-x86-csy_d96c47793b43ab0dbfdb36722acfbd7a2a7d1caf.exe">Ver 1</a> |
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v2/windows2000-kb891861-v2-x86-csy_a18b67cb73d57b03d9ae276097e33ae870acc77d.exe">Ver 2</a></td>
					</tr>
					<tr>
						<td width="15%">Danish</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp1/w2ksp1_da.exe">SP1</a></td>
						<td width="9%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp2/w2ksp2_da.exe">SP2</a></td>
						<td width="10%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp3/w2ksp3_da.exe">SP3</a></td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp4/w2ksp4_da.exe">
						SP4</a>
						</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v1/windows2000-kb891861-x86-dan_00d607e26fad577be4ee0c032f7aaa59e81b185e.exe">Ver 1</a> |
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v2/windows2000-kb891861-v2-x86-dan_0b5521ef897e7859fefd1ceace978ba06a67caf6.exe">Ver 2</a></td>
					</tr>
					<tr>
						<td width="15%">German</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp1/w2ksp1_de.exe">SP1</a></td>
						<td width="9%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp2/w2ksp2_de.exe">SP2</a></td>
						<td width="10%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp3/w2ksp3_de.exe">SP3</a></td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp4/w2ksp4_de.exe">
						SP4</a>
						</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v1/windows2000-kb891861-x86-deu_58091de522376c5676c6f607bc14e6169a2958c5.exe">Ver 1</a> |
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v2/windows2000-kb891861-v2-x86-deu_e2c13be516127d3b1de17a60b9fadfb11862b86b.exe">Ver 2</a></td>
					</tr>
					<tr>
						<td width="15%">Greek</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp1/w2ksp1_el.exe">SP1</a></td>
						<td width="9%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp2/w2ksp2_el.exe">SP2</a></td>
						<td width="10%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp3/w2ksp3_el.exe">SP3</a></td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp4/w2ksp4_el.exe">
						SP4</a>
						</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v1/windows2000-kb891861-x86-ell_8ac0302e899782a4b65e9eb6acafd2a11ea3aa94.exe">Ver 1</a> |
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v2/windows2000-kb891861-v2-x86-ell_e04a73c2761a323c0b266f766c5cc3e6f9389b76.exe">Ver 2</a></td>
					</tr>
					<tr>
						<td width="15%">English</td>
						<td width="11%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp1/w2ksp1_en.exe">SP1</a>
						</td>
						<td width="9%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp2/w2ksp2_en.exe">SP2</a>
						</td>
						<td width="10%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp3/w2ksp3_en.exe">SP3</a>
						</td>
						<td width="11%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp4/w2ksp4_en.exe">
							SP4</a>
						</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v1/windows2000-kb891861-x86-enu_44dd2ee1e4ef53a46b86d79421e6b61cd5aa0554.exe">Ver 1</a> |
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v2/windows2000-kb891861-v2-x86-enu_f118bd276f4211929719961a2e929b620c1a2234.exe">Ver 2</a></td>
					</tr>
					<tr>
						<td width="15%">Spanish</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp1/w2ksp1_es.exe">SP1</a></td>
						<td width="9%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp2/w2ksp2_es.exe">SP2</a></td>
						<td width="10%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp3/w2ksp3_es.exe">SP3</a></td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp4/w2ksp4_es.exe">
						SP4</a>
						</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v1/windows2000-kb891861-x86-esn_fbd11ab584b6521227596cf93efb46b86124afb4.exe">Ver 1</a> |
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v2/windows2000-kb891861-v2-x86-esn_b9f1a26cc909adca730ed20344cf712480c9877e.exe">Ver 2</a></td>
					</tr>
					<tr>
						<td width="15%">Finnish</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp1/w2ksp1_fi.exe">SP1</a></td>
						<td width="9%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp2/w2ksp2_fi.exe">SP2</a></td>
						<td width="10%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp3/w2ksp3_fi.exe">SP3</a></td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp4/w2ksp4_fi.exe">
						SP4</a>
						</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v1/windows2000-kb891861-x86-fin_e902dc0aa91c03009b7e8d27424e2062ebea19f3.exe">Ver 1</a> |
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v2/windows2000-kb891861-v2-x86-fin_318d6b6a1ac04e9c9c033d53d7fcd47748e7f75f.exe">Ver 2</a></td>
					</tr>
					<tr>
						<td width="15%">French</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp1/w2ksp1_fr.exe">SP1</a></td>
						<td width="9%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp2/w2ksp2_fr.exe">SP2</a></td>
						<td width="10%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp3/w2ksp3_fr.exe">SP3</a></td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp4/w2ksp4_fr.exe">
						SP4</a>
						</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v1/windows2000-kb891861-x86-fra_c47e349aa99989354a8de6c1b1c875ac9725aff0.exe">Ver 1</a> |
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v2/windows2000-kb891861-v2-x86-fra_8d036d92d4895bf23b0e9b714937b5b211b1d592.exe">Ver 2</a></td>
					</tr>
					<tr>
						<td width="15%">Hebrew</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp1/w2ksp1_he.exe">SP1</a></td>
						<td width="9%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp2/w2ksp2_he.exe">SP2</a></td>
						<td width="10%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp3/w2ksp3_he.EXE">SP3</a></td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp4/w2ksp4_he.exe">
						SP4</a>
						</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v1/windows2000-kb891861-x86-heb_627c8364fe4525945e134a0decf1f5f8eae350a9.exe">Ver 1</a> |
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v2/windows2000-kb891861-v2-x86-heb_3f08f4037db77733e5de5e1cb89e5bedebb4d249.exe">Ver 2</a></td>
					</tr>
					<tr>
						<td width="15%">Hungarian</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp1/w2ksp1_hu.exe">SP1</a></td>
						<td width="9%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp2/w2ksp2_hu.exe">SP2</a></td>
						<td width="10%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp3/w2ksp3_hu.exe">SP3</a></td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp4/w2ksp4_hu.exe">
						SP4</a>
						</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v1/windows2000-kb891861-x86-hun_2969deb557923dd563b46b673c85a5504ca6a7e7.exe">Ver 1</a> |
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v2/windows2000-kb891861-v2-x86-hun_d94efec685c682825592e04130ea1b0bf3c4e532.exe">Ver 2</a></td>
					</tr>
					<tr>
						<td width="15%">Italian</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp1/w2ksp1_it.exe">SP1</a></td>
						<td width="9%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp2/w2ksp2_it.exe">SP2</a></td>
						<td width="10%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp3/w2ksp3_it.exe">SP3</a></td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp4/w2ksp4_it.exe">
						SP4</a>
						</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v1/windows2000-kb891861-x86-ita_659db3a91c3524703e789587b3c4db50ce5a469f.exe">Ver 1</a> |
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v2/windows2000-kb891861-v2-x86-ita_aaf013cd7a046d6069c0250e8e7f0bf8582c3e31.exe">Ver 2</a></td>
					</tr>
					<tr>
						<td width="15%">Japanese</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp1/w2ksp1_ja.exe">SP1</a></td>
						<td width="9%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp2/w2ksp2_ja.exe">SP2</a></td>
						<td width="10%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp3/w2ksp3_ja.exe">SP3</a></td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp4/w2ksp4_ja.exe">
						SP4</a>
						</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v1/windows2000-kb891861-x86-jpn_c871d8c223fea626a5d2d310d2cb4e03ccfc5624.exe">Ver 1</a> |
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v2/windows2000-kb891861-v2-x86-jpn_d5fa54ab3547a38fae6260e28227a6fb0ed4a827.exe">Ver 2</a></td>
					</tr>
					<tr>
						<td width="15%">Japanese<br><i>(NEC PC-98)</i></td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp1/pc98/w2ksp1_ja_pc98.exe">SP1</a></td>
						<td width="9%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp2/pc98/w2ksp2_ja_pc98.exe">SP2</a></td>
						<td width="10%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp3/pc98/w2ksp3_ja_pc98.exe">SP3</a></td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp4/pc98/w2ksp4_ja_pc98.exe">
						SP4</a>
						</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v1/windows2000-kb891861-nec98-jpn_4fe381dbe7d7e587986715c7bb938d6d43b33e67.exe">Ver 1</a> | 
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v1/windows2000-kb891861-v2-nec98-jpn_ca1372faeb77c66ded958bd3dc8e3344e1cd4260.exe">Ver 2</a></td>
					</tr>
					<tr>
						<td width="15%">Korean</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp1/w2ksp1_ko.exe">SP1</a></td>
						<td width="9%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp2/w2ksp2_ko.exe">SP2</a></td>
						<td width="10%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp3/w2ksp3_ko.exe">SP3</a></td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp4/w2ksp4_ko.exe">
						SP4</a>
						</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v1/windows2000-kb891861-x86-kor_9b5a67a0f84eb34780c635ec8fe13ebfbff2f2fd.exe">Ver 1</a> | 
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v2/windows2000-kb891861-v2-x86-kor_10db79be10624535fb3926781db5a02efb2b5503.exe">Ver 2</a></td>
					</tr>
					<tr>
						<td width="15%">Dutch</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp1/w2ksp1_nl.exe">SP1</a></td>
						<td width="9%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp2/w2ksp2_nl.exe">SP2</a></td>
						<td width="10%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp3/w2ksp3_nl.exe">SP3</a></td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp4/w2ksp4_nl.exe">
						SP4</a>
						</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v1/windows2000-kb891861-x86-nld_29674e57b1b7ac13ccb28e8aae32b3c159e904d8.exe">Ver 1</a> | 
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v2/windows2000-kb891861-v2-x86-nld_33cbe9c7dedc85d22f6fc281aefef77b9d26ce6f.exe">Ver 2</a></td>
					</tr>
					<tr>
						<td width="15%" height="23">Norwegian</td>
						<td width="11%" align="center" height="23">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp1/w2ksp1_no.exe">SP1</a></td>
						<td width="9%" align="center" height="23">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp2/w2ksp2_no.exe">SP2</a></td>
						<td width="10%" align="center" height="23">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp3/w2ksp3_no.exe">SP3</a></td>
						<td width="11%" align="center" height="23">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp4/w2ksp4_no.exe">
						SP4</a>
						</td>
						<td width="11%" align="center" height="23">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v1/windows2000-kb891861-x86-nor_83bd92e2fce55b6d63331915ea8a81990cf34192.exe">Ver 1</a> | 
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v2/windows2000-kb891861-v2-x86-nor_6dce467272a1cb3375bc74bec901a581d55661e6.exe">Ver 2</a></td>
					</tr>
					<tr>
						<td width="15%">Polish</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp1/w2ksp1_pl.exe">SP1</a></td>
						<td width="9%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp2/w2ksp2_pl.exe">SP2</a></td>
						<td width="10%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp3/w2ksp3_pl.exe">SP3</a></td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp4/w2ksp4_pl.exe">
						SP4</a>
						</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v1/windows2000-kb891861-x86-plk_8650e9643c3d4364a0a4370baee2d7e82f2523b7.exe">Ver 1</a> | 
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v2/windows2000-kb891861-v2-x86-plk_1cded665d65ea7614b7b33989efb81a4b91cf674.exe">Ver 2</a></td>
					</tr>
					<tr>
						<td width="15%">Portuguese (Brazilian)</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp1/w2ksp1_ptbr.exe">SP1</a></td>
						<td width="9%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp2/w2ksp2_ptbr.exe">SP2</a></td>
						<td width="10%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp3/w2ksp3_ptbr.exe">SP3</a></td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp4/w2ksp4_ptbr.exe">
						SP4</a>
						</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v1/windows2000-kb891861-x86-ptb_48ffb8eaf5861abfd9411464cee9f21a1a4584c8.exe">Ver 1</a> | 
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v2/windows2000-kb891861-v2-x86-ptb_d1a5c2bf1c9393f64748bab275339d09f767a1d9.exe">Ver 2</a></td>
					</tr>
					<tr>
						<td width="15%">Portuguese</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp1/w2ksp1_pt.exe">SP1</a></td>
						<td width="9%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp2/w2ksp2_pt.exe">SP2</a></td>
						<td width="10%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp3/w2ksp3_pt.exe">SP3</a></td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp4/w2ksp4_pt.exe">
						SP4</a>
						</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v1/windows2000-kb891861-x86-ptg_29bfc53c50a1d6dbb494d31682eed5b4300b692b.exe">Ver 1</a> | 
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v2/windows2000-kb891861-v2-x86-ptg_ad4fd8ab12ee20236a44cf3e550ee55a3e8019ec.exe">Ver 2</a></td>
					</tr>
					<tr>
						<td width="15%">Russian</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp1/w2ksp1_ru.exe">SP1</a></td>
						<td width="9%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp2/w2ksp2_ru.exe">SP2</a></td>
						<td width="10%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp3/w2ksp3_ru.exe">SP3</a></td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp4/w2ksp4_ru.exe">
						SP4</a>
						</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v1/windows2000-kb891861-x86-rus_05b6afe89151e40cbdac6c9093c563655a0ba6da.exe">Ver 1</a> | 
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v2/windows2000-kb891861-v2-x86-rus_2d2b365faf8fc71be99122fa89b8eea97ae40abd.exe">Ver 2</a></td>
					</tr>
					<tr>
						<td width="15%">Swedish</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp1/w2ksp1_sv.exe">SP1</a></td>
						<td width="9%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp2/w2ksp2_sv.exe">SP2</a></td>
						<td width="10%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp3/w2ksp3_sv.exe">SP3</a></td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp4/w2ksp4_sv.exe">
						SP4</a>
						</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v1/windows2000-kb891861-x86-sve_68a4a87a28106ccefb5cd60a4eb48875f253e289.exe">Ver 1</a> | 
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v2/windows2000-kb891861-v2-x86-sve_e694a39661f9a6531c1b9367ed6fc9458ae536fb.exe">Ver 2</a></td>
					</tr>
					<tr>
						<td width="15%">Turkish</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp1/w2ksp1_tr.exe">SP1</a></td>
						<td width="9%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp2/w2ksp2_tr.exe">SP2</a></td>
						<td width="10%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp3/w2ksp3_tr.exe">SP3</a></td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/sp4/w2ksp4_tr.exe">
						SP4</a>
						</td>
						<td width="11%" align="center">
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v1/windows2000-kb891861-x86-trk_86c7684649890345aaa2cfef0507bee59ced13b9.exe">Ver 1</a> | 
						<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/win2k/ur1/v2/windows2000-kb891861-v2-x86-trk_31ee7902955f771a63b8bd6165fa41119624ac23.exe">Ver 2</a></td>
					</tr>
				</table>
				<p> <u>
						<id id="WINXP"><strong>Microsoft Windows XP 32-Bit&nbsp;(SP1 | SP2 | SP3):</strong></id>
					</u><br>
					<id id="x20"><i>These files are hosted on the Windows Update Restored servers.</i></id>
				<br>
				<font color="#FF0000">Russian is currently missing.</font></p>
				<table border="1" width="442" style="border-collapse: collapse;">
					<tr>
						<td width="25%" align="center">
							<u><b>Language</b></u>
						</td>
						<td width="68%" align="center" colspan="3">
							<u><b>Service Pack Version</b></u>
						</td>
					</tr>
					<tr>
						<td width="15%">Arabic</td>
						<td width="25%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/ar/xpsp1a_e31fa91a89a658f3d586aa1c9992b01af3e11fe0.exe">SP1</a>
						</td>
						<td width="25%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/ar/xpsp2_a001b2b27b0f748ab89bd28fa45359f28f95ffb6.exe">
							SP2</a>
						</td>
						<td width="25%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/ar/windowsxp-kb936929-sp3-x86-ara_4e82a87cea3b9643fd57b39393a88061614cdae6.exe">SP3</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Chinese (Hong Kong)</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/hk/xpsp1a_c7c76e11bb0c1bbabefe5d4576851f3.exe">
							SP1</a>
						</td>
						<td width="24%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/hk/xpsp2_546c11a0a5a41e90ae5f021f234d9abad5a0a25a.exe">SP2</a>
						</td>
						<td width="30%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/hk/windowsxp-kb936929-sp3-x86-chh_2663fb610f29e65a10be176740ea6757ca1f22d5.exe">
							SP3</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Chinese (Taiwan)</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/tw/xpsp1a_98472ed83897ec104c8496fcd661e6f289ef5090.exe">
							SP1</a>
						</td>
						<td width="24%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/tw/xpsp2_66d8ba2cbf90737010a45e2b5969c9a779ab1d35.exe">SP2</a>
						</td>
						<td width="30%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/tw/windowsxp-kb936929-sp3-x86-cht_8a0625e10b8c6cb88d9d1952f1a189fbd761b744.exe">
							SP3</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Chinese (Simplified)</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/cn/xpsp1a_8bd1bde300df00e7bd6831ac37cb87e437e2b729.exe">
							SP1</a>
						</td>
						<td width="24%" align="center">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/cn/xpsp2_554ee954bae0f65e88b706cf9b9fd733c02fa397.exe">SP2</a>
						</td>
						<td width="30%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/cn/windowsxp-kb936929-sp3-x86-chs_d7067e86abd4257454200d0c398d71c4ce6cd33e.exe">
							SP3</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Czech</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/cs/xpsp1a_57f0caa259089b1477a162f220d08b1c4bc86010.exe">
							SP1</a>
						</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/cs/xpsp2_767f81c9d0154bfa21d19a49e0b27ede9f93190a.exe">
							SP2</a>
						</td>
						<td width="30%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/cs/windowsxp-kb936929-sp3-x86-csy_7af606916b887dba9dd38ae282505ce2c2b81b08.exe">
							SP3</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Danish</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/da/xpsp1a_619cc6cf6a98f75d18f4d6eddb662ebfe4a44223.exe">
							SP1</a>
						</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/da/xpsp2_e60a2b567fda2ab2cf4629700cf65cd3f4d46b0d.exe">
							SP2</a>
						</td>
						<td width="30%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/da/windowsxp-kb936929-sp3-x86-dan_37e03a7d43ad65bc4b71f3947c76bd2fc0fe44d6.exe">
							SP3</a>
						</td>
					</tr>
					<tr>
						<td width="15%" height="23">German</td>
						<td width="24%" align="center" height="23">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/de/xpsp1a_cb7d182645ea1019741d9d94732c29251294acdc.exe">
							SP1</a>
						</td>
						<td width="24%" align="center" height="23">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/de/xpsp2_ac8d3101744ff56f74a4de941dc04a7e567c8ba7.exe">
							SP2</a>
						</td>
						<td width="30%" align="center" height="23">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/de/windowsxp-kb936929-sp3-x86-deu_f2dcd2211384a78df215c696a7fd1a7949dc794b.exe">
							SP3</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Greek</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/el/xpsp1a_47666b37340d6224eeb40f67ae16da3e457e64e0.exe">
							SP1</a>
						</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/el/xpsp2_fd86a331f91bc16185e88e6706db859cb27264d1.exe">
							SP2</a>
						</td>
						<td width="30%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/el/windowsxp-kb936929-sp3-x86-ell_cec2ed1c3097e44e62dcb1f2a473a64a58e031fe.exe">
							SP3</a>
						</td>
					</tr>
					<tr>
						<td width="15%">English</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/en/xpsp1a_98472ed83897ec104c8496fcd661e6f289ef5090.exe">
							SP1</a>
						</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/en/xpsp2_33a8fef60d48ae1f2c4feea27111af5ceca3c4f6.exe">
							SP2</a>
						</td>
						<td width="30%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/en/windowsxp-kb936929-sp3-x86-enu_c81472f7eeea2eca421e116cd4c03e2300ebfde4.exe">
							SP3</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Spanish</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/es/xpsp1a_e675d08b12d20ca372ba6d3b502074000981870f.exe">
							SP1</a>
						</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/es/xpsp2_8e7e8de62676d994a7412df190fcdce848f25cf0.exe">
							SP2</a>
						</td>
						<td width="30%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/es/windowsxp-kb936929-sp3-x86-esn_e305becfc6fd5a8199368ceffc496397247ac60f.exe">
							SP3</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Finnish</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/fi/xpsp1a_8a254d89daeb22f4f4ccb9b0861e83cb5ea66196.exe">
							SP1</a>
						</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/fi/xpsp2_2feacc1ad5a703d508e9aee4c2fca1e0b9ca9962.exe">
							SP2</a>
						</td>
						<td width="30%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/fi/windowsxp-kb936929-sp3-x86-fin_5654c021a03bfb10543a2c673bdfc42a853e347a.exe">
							SP3</a>
						</td>
					</tr>
					<tr>
						<td width="15%">French</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/fr/xpsp1a_fe557a68aed45003151f647189767aeb2bed53a9.exe">
							SP1</a>
						</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/fr/xpsp2_da94a031147ebd6f8d02eadb7972801843a533bb.exe">
							SP2</a>
						</td>
						<td width="30%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/fr/windowsxp-kb936929-sp3-x86-fra_414b61bbc86e09579d8447baa23eb1b867f9ca93.exe">
							SP3</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Hebrew</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/he/xpsp1a_499ee7440dba140909fe4d3a86fffd8ec4a1d473.exe">
							SP1</a>
						</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/he/xpsp2_76096ed9f2218ac7a14472cbf435f380ffc1c2f6.exe">
							SP2</a>
						</td>
						<td width="30%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/he/windowsxp-kb936929-sp3-x86-heb_eb8fc9ff0890279346661dde065c14b5c696e423.exe">
							SP3</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Hungarian</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/hu/xpsp1a_9f24000eb4b69aaa975edeb578b4cd22f971318b.exe">
							SP1</a>
						</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/hu/xpsp2_bd03a3d2a28de2de9e58da3e5dec888ffae25aca.exe">
							SP2</a>
						</td>
						<td width="30%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/hu/windowsxp-kb936929-sp3-x86-hun_77b70fe421baeebb115c2f378b8a1fc274db9867.exe">
							SP3</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Italian</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/it/xpsp1a_4f5aa5833bee00c3309fc03d6eafcc6e6f4e2eea.exe">
							SP1</a>
						</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/it/xpsp2_1334fdd285666bfd821eef8590914f188b1bcc0c.exe">
							SP2</a>
						</td>
						<td width="30%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/it/windowsxp-kb936929-sp3-x86-ita_2162c1d419d1e462a7dc34294528b2daf593302c.exe">
							SP3</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Japanese</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/ja/xpsp1a_a03d9d2ad30a478ca4c2a51c1de26eaca021e4d1.exe">
							SP1</a>
						</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/ja/xpsp2_d36c0a7046f2dbe29dfff33b6dbb6bb4574bbd7d.exe">
							SP2</a>
						</td>
						<td width="30%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/ja/windowsxp-kb936929-sp3-x86-jpn_e0fc34cfa52d270b3c79a68af8fa358244f7419e.exe">
							SP3</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Korean</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/ko/xpsp1a_e79e224ef297b77c0a23a9440f919c8d64e6554a.exe">
							SP1</a>
						</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/ko/xpsp2_4a6b32f007fe94a2a8b4542fc6778582ef4245e3.exe">
							SP2</a>
						</td>
						<td width="30%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/ko/windowsxp-kb936929-sp3-x86-kor_8ca7e862bfc2742ad9c4c58df0b0cd5ad4b700ae.exe">
							SP3</a>
						</td>
					</tr>
					<tr>
						<td width="15%" height="23">Dutch</td>
						<td width="24%" align="center" height="23">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/nl/xpsp1a_f1a0183ddc01376d27f374b69fa9364647b336b2.exe">
							SP1</a>
						</td>
						<td width="24%" align="center" height="23">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/nl/xpsp2_64551a535981fbcd9c2bac3f91df30c6b3610725.exe">
							SP2</a>
						</td>
						<td width="30%" align="center" height="23">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/nl/windowsxp-kb936929-sp3-x86-nld_b2576b4d1972583a3b776fdf963b914341d34058.exe">
							SP3</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Norwegian</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/no/xpsp1a_487a5bbfcf9164a0cda2bbe31f9d4aed6c5455a2.exe">
							SP1</a>
						</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/no/xpsp2_ace2994463597f9cb7cb059e1dca2a87c12bf278.exe">
							SP2</a>
						</td>
						<td width="30%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/no/windowsxp-kb936929-sp3-x86-nor_67c9167275a2c3e3884a38c2ad7387556ad4713a.exe">
							SP3</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Polish</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/pl/xpsp1a_d2a89787c9ac8ed660684686930513663bc723ba.exe">
							SP1</a>
						</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/pl/xpsp2_8aefc12abade033f2f093786646206fbb70cbb5d.exe">
							SP2</a>
						</td>
						<td width="30%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/pl/windowsxp-kb936929-sp3-x86-plk_7cbe718131e9c71b322f1149e86bedba351ba11c.exe">
							SP3</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Portuguese (Brazilian)</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/ptbr/xpsp1a_87ec9b54ce8b93f7319b2158cbb0fa1de47d48df.exe">
							SP1</a>
						</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/ptbr/xpsp2_bcbc1bbb2c493dd0a942d13695b18da0400188f8.exe">
							SP2</a>
						</td>
						<td width="30%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/ptbr/windowsxp-kb936929-sp3-x86-ptb_02900ef11f5a982a93de5f927997ce92d5a81a86.exe">
							SP3</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Portuguese</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/pt/xpsp1a_625bc5ebcc1306c2632c1b1a60a6f8f30f230cb9.exe">
							SP1</a>
						</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/pt/xpsp2_db1f7c486f0eda249d77ae91c858a162cefb769b.exe">
							SP2</a>
						</td>
						<td width="30%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/pt/windowsxp-kb936929-sp3-x86-ptg_bc5189c05e93cd0e5742712d84f0b5f5ffcbb7e4.exe">
							SP3</a>
						</td>
					</tr>
					<tr>
						<td width="15%" height="23">Russian</td>
						<td width="24%" align="center" height="23">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/ru/xpsp1a_b5151f669462b3154faf93d48e10beae1b0339aa.exe">SP1</a>
						</td>
						<td width="24%" align="center" height="23">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/ru/xpsp2_72946fbe955201f2387430963d4372cda7cac392.exe">SP2</a>
						</td>
						<td width="30%" align="center" height="23">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/ru/windowsxp-kb936929-sp3-x86-rus_850cda9f57033a17d046a56d422547ea80dcaf61.exe">SP3</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Swedish</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/sv/xpsp1a_8f11213e01d65c883a95b8871b6134e179e09941.exe">
							SP1</a>
						</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/sv/xpsp2_3ff578d759e77df91effefe34a42030b8ada1c24.exe">
							SP2</a>
						</td>
						<td width="30%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/sv/windowsxp-kb936929-sp3-x86-sve_13c5ecca22e12224934a1faa1190ee34db3995ae.exe">
							SP3</a>
						</td>
					</tr>
					<tr>
						<td width="15%">Turkish</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/tr/xpsp1a_a2bc3da0f0a3d366c78d4c51ea6aabbb2dd0d545.exe">
							SP1</a>
						</td>
						<td width="24%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/tr/xpsp2_68cfeb8fda746b900f9c3ce313d7348c812fd30d.exe">
							SP2</a>
						</td>
						<td width="30%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/32/tr/windowsxp-kb936929-sp3-x86-trk_5aaf60501636af08c97ef1c18f1315f4ed6fbcdf.exe">
							SP3</a>
						</td>
					</tr>
				</table>
				<p> <u>
						<id id="WINXP64"><strong>Microsoft Windows XP AMD64 &amp; 
				IA-64&nbsp;(SP2):</strong></id>
					</u><br>
					<id id="x18"><i>These files are hosted on the Windows Update Restored servers.</i></id>
				<id id="x16">
				<span
							style="color: red; font-style: italic;"><br>
				</span>Other languages will have to be installed using an MUI 
				pack, over the English version.<span
							style="color: red; font-style: italic;"><br>
				Red text indicates that the file was never officially released by
							Microsoft.</span></id>
				</p>
				<table border="1" width="442" style="border-collapse: collapse;">
					<tr>
						<td width="24%" align="center">
							<u><b>Language</b></u>
						</td>
						<td align="center" colspan="2">
							<u><b>Service Pack 2</b></u>
						</td>
					</tr>
					<tr>
						<td width="24%">German</td>
						<td width="36%" align="center">
							<span style="color: #FF0000">Not available</span>
						</td>
						<td width="36%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/64/ia64/windowsserver2003-kb914961-sp2-ia64-deu_f6857c1bb8fc03798541a78cdc1f5bb98b456333.exe">
							IA-64</a>
						</td>
					</tr>
					<tr>
						<td width="24%" height="24">English</td>
						<td width="36%" align="center" height="24">
							<a
								href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/64/amd64/windowsserver2003.windowsxp-kb914961-sp2-x64-enu_7f8e909c52d23ac8b5dbfd73f1f12d3ee0fe794c.exe">AMD64
							</a>
						</td>
						<td width="36%" align="center" height="24">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/64/ia64/windowsserver2003-kb914961-sp2-ia64-enu_8856af0aa0f198a8aad2de2c1ca68240d1d49bf3.exe">
							IA-64</a>
						</td>
					</tr>
					<tr>
						<td width="24%">French</td>
						<td width="36%" align="center">
							<span style="color: #FF0000">Not available</span>
						</td>
						<td width="36%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/64/ia64/windowsserver2003-kb914961-sp2-ia64-fra_0289b81fe7ed5c1c36f87232f87b25cdb21d331d.exe">
							IA-64</a>
						</td>
					</tr>
					<tr>
						<td width="24%">Japanese</td>
						<td width="36%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/64/amd64/windowsserver2003.windowsxp-kb914961-sp2-x64-jpn_c725a8c4e03803b12a9ac8820e964dbc377a80dc.exe">AMD64</a></td>
						<td width="36%" align="center">
							<a href="<%= SiteConfig.DownloadBaseUrl %>/updates/CabPool/servicepacks/winxp/64/ia64/windowsserver2003-kb914961-sp2-ia64-jpn_3216b1978fd418181ecbf77eef67398edb97a106.exe">
							IA-64</a>
						</td>
					</tr>
					</table>
				<p>&nbsp;</p>
				<p> 
				<ID ID="x10"><br> <i>Some of the Microsoft Windows NT 4.0 Service Packs 
				listed here were provided
							by @sjarb (<a target="_blank" href="http://sjarb.fr/">sjarb.fr</a>). 
				The rest were found by @Shwarc-1500 and other members.</i></ID></td>
		</tr>
		<tr>
			<td ID="td1" VALIGN="top" ALIGN="left" WIDTH="94">&nbsp;</td>
			<td ID="td2" VALIGN="top" ALIGN="left">&nbsp;</td>
		</tr>
	</table> <br>

    </div>

</asp:Content>
