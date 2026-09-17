<%@ Page Title="Help Us & Contribute" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contribute.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.en.Contribute" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <style type="text/css">
        .content-page { padding: 20px; font-family: Verdana, Arial, sans-serif; font-size: 8pt; }
        .page-title { font-size: 16pt; color: #00008B; font-weight: bold; margin-bottom: 15px; }
        .section-title { font-size: 11pt; font-weight: bold; text-decoration: underline; color: #00319C; margin-top: 15px; margin-bottom: 10px; }
        .text-regular { font-size: 10pt; line-height: 1.6; }
        .code-path { font-family: Courier New, monospace; background-color: #F0F0F0; padding: 2px 5px; }
    </style>
    
    <div class="content-page">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td valign="top" align="left" width="94">
                    <asp:Image ID="imgLogo" runat="server" ImageUrl="~/en/images/wulgo_only.gif" Width="100" Height="100" AlternateText="Logo" />
                </td>
                <td valign="top" align="left" style="padding-left: 10px;">
                    <asp:Image ID="imgLogoText" runat="server" ImageUrl="~/en/images/wurlgo_text.gif" Width="188" Height="100" AlternateText="Text" />
                    <h2 style="color: #00008B; margin: 10px 0;">Help Us & Contribute</h2>
                </td>
            </tr>
        </table>
        
        <p class="text-regular" style="text-align: justify;">
            Give this project a boost by contributing improvements, bug fixes, or new features! 
            This page will guide you on how you can help make this template even better.
        </p>
        
        <div class="section-title">Ways to Contribute</div>
        <p class="text-regular">
            There are several ways you can contribute to this project:
            <br /><br />
            <b>1. Code Contributions:</b> Submit pull requests with bug fixes or new features.
            <br /><br />
            <b>2. Documentation:</b> Help improve the documentation and examples.
            <br /><br />
            <b>3. Testing:</b> Test the template on different browsers and report issues.
            <br /><br />
            <b>4. Feedback:</b> Share your experience and suggestions for improvements.
        </p>
        
        <div class="section-title">Getting Started with Development</div>
        <p class="text-regular">
            To start developing with this template:
            <br /><br />
            1. Clone or download the project files.
            <br />
            2. Open the solution in Visual Studio.
            <br />
            3. Build and run the project to see the template in action.
            <br />
            4. Start customizing the controls and pages.
        </p>
        
        <div class="section-title">File Structure</div>
        <p class="text-regular">
            Key files and directories:
            <br /><br />
            <span class="code-path">Site.Master</span> - Master page layout
            <br />
            <span class="code-path">/Controls/</span> - User controls (Toolbar, Navigation, Footer, ContentBox)
            <br />
            <span class="code-path">/en/</span> - English content pages
            <br />
            <span class="code-path">Web.config</span> - Application configuration
        </p>
        
        <p class="text-regular" style="margin-top: 20px;">
            <b>Thank you for your interest in contributing!</b>
        </p>
    </div>
    
</asp:Content>
