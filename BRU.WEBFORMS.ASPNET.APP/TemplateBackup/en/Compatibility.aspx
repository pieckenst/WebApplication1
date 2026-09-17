<%@ Page Title="Compatibility Guide" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Compatibility.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.en.Compatibility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <style type="text/css">
        .content-page { padding: 10px; font-family: Tahoma, Verdana, Arial, sans-serif; }
        .page-title { font-size: 16pt; color: #000080; font-weight: bold; }
        .page-divider { height: 2px; background-color: #000080; margin: 5px 0 15px 0; }
        .text-regular { font-size: 10pt; line-height: 1.5; }
        .text-small { font-size: 8pt; }
        .compat-table { width: 100%; border-collapse: collapse; border: 1px solid #CCCCCC; margin-top: 10px; }
        .compat-table th { background-color: #E0E0E0; padding: 5px; text-align: center; font-size: 8pt; }
        .compat-table td { border: 1px solid #CCCCCC; padding: 5px; text-align: center; font-size: 8pt; }
        .status-yes { background-color: #8BFF8B; }
        .status-partial { background-color: #FFFF99; }
        .status-no { background-color: #FF8C8C; }
        .legend-item { display: inline-block; margin-right: 15px; }
        .legend-box { display: inline-block; width: 12px; height: 12px; border: 1px solid #000; margin-right: 5px; vertical-align: middle; }
    </style>
    
    <div class="content-page">
        <a name="top"></a>
        <div class="page-title">Compatibility Guide</div>
        <div class="page-divider"></div>
        
        <p class="text-regular" style="margin-top: 0;">
            This guide provides a comprehensive overview of browser and feature compatibility for this template.
        </p>
        
        <div style="border: 1px solid #808080; border-top: none; padding: 10px; background-color: #FDFDFD;">
            
            <!-- Legend -->
            <div style="margin-bottom: 10px;">
                <span class="legend-item"><span class="legend-box status-yes"></span><span class="text-small">Supported</span></span>
                <span class="legend-item"><span class="legend-box status-partial"></span><span class="text-small">Partial</span></span>
                <span class="legend-item"><span class="legend-box status-no"></span><span class="text-small">Not Supported</span></span>
            </div>
            
            <!-- Browser Compatibility Table -->
            <table class="compat-table">
                <tr>
                    <th><b>Feature</b></th>
                    <th><b>Chrome</b></th>
                    <th><b>Firefox</b></th>
                    <th><b>Edge</b></th>
                    <th><b>Safari</b></th>
                    <th><b>IE 11</b></th>
                </tr>
                <tr>
                    <td style="text-align: left;"><b>Frame Layout</b></td>
                    <td class="status-yes">Yes</td>
                    <td class="status-yes">Yes</td>
                    <td class="status-yes">Yes</td>
                    <td class="status-yes">Yes</td>
                    <td class="status-partial">Partial</td>
                </tr>
                <tr>
                    <td style="text-align: left;"><b>Navigation</b></td>
                    <td class="status-yes">Yes</td>
                    <td class="status-yes">Yes</td>
                    <td class="status-yes">Yes</td>
                    <td class="status-yes">Yes</td>
                    <td class="status-yes">Yes</td>
                </tr>
                <tr>
                    <td style="text-align: left;"><b>Responsive Design</b></td>
                    <td class="status-yes">Yes</td>
                    <td class="status-yes">Yes</td>
                    <td class="status-yes">Yes</td>
                    <td class="status-yes">Yes</td>
                    <td class="status-no">No</td>
                </tr>
                <tr>
                    <td style="text-align: left;"><b>CSS Styling</b></td>
                    <td class="status-yes">Yes</td>
                    <td class="status-yes">Yes</td>
                    <td class="status-yes">Yes</td>
                    <td class="status-yes">Yes</td>
                    <td class="status-partial">Partial</td>
                </tr>
                <tr>
                    <td style="text-align: left;"><b>PostBacks</b></td>
                    <td class="status-yes">Yes</td>
                    <td class="status-yes">Yes</td>
                    <td class="status-yes">Yes</td>
                    <td class="status-yes">Yes</td>
                    <td class="status-yes">Yes</td>
                </tr>
            </table>
            
            <div class="text-small" style="margin-top: 8px; color: #666666;">
                <i>Note: This template is optimized for modern browsers. Internet Explorer 11 has limited support.</i>
            </div>
        </div>
        
        <p align="right" style="margin-top: 15px;"><a href="#top" style="font-size: 8pt; color: #000000;">Back to top ▲</a></p>
    </div>
    
</asp:Content>
