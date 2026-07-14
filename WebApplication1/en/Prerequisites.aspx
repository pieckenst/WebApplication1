<%@ Page Title="Prerequisites" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Prerequisites.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.en.Prerequisites" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <style type="text/css">
        .content-page { padding: 10px; font-family: Tahoma, Verdana, Arial, sans-serif; }
        .page-title { font-size: 16pt; color: #000080; font-weight: bold; }
        .page-divider { height: 2px; background-color: #000080; margin: 5px 0 15px 0; }
        .text-regular { font-size: 10pt; line-height: 1.5; }
        .text-small { font-size: 8pt; }
        .resource-box { border: 1px solid #808080; border-top: none; padding: 12px; margin-bottom: 20px; }
        .resource-box-blue { background-color: #F9F9F9; }
        .resource-box-yellow { background-color: #FFF9CC; }
        .resource-row { display: table; width: 100%; }
        .resource-icon { display: table-cell; width: 35px; vertical-align: top; }
        .resource-content { display: table-cell; vertical-align: top; }
        .tutorial-link { font-weight: bold; font-size: 14px; color: #000080; text-decoration: none; }
        .tutorial-link:hover { color: #FF6600; text-decoration: underline; }
        .section-header { background-color: #000080; color: #FFFFFF; font-weight: bold; padding: 4px 8px; font-size: 10pt; }
        .section-header-gray { background-color: #666666; }
    </style>
    
    <div class="content-page">
        <a name="top"></a>
        <div class="page-title">Prerequisites</div>
        <div class="page-divider"></div>
        
        <p class="text-regular" style="margin-top: 0;">
            Not all features work out of the box. Below you will find the necessary documentation and guides 
            to properly configure your website template.
        </p>
        
        <div class="section-header">&nbsp;Template Resources</div>
        <div class="resource-box resource-box-blue">
            <div class="resource-row">
                <div class="resource-icon">
                    <img src="<%= ResolveUrl("~/en/images/arrow.gif") %>" width="30" height="25" border="0" alt="">
                </div>
                <div class="resource-content">
                    <asp:HyperLink ID="hlSetup" runat="server" NavigateUrl="~/en/ReadThisFirst.aspx" CssClass="tutorial-link">
                        Getting Started Guide
                    </asp:HyperLink>
                    <p style="margin-top: 5px; margin-bottom: 15px; font-size: 10pt; color: #333333;">
                        Need help setting up this template? This guide helps you configure your project, 
                        customize navigation, and add your own content pages.
                    </p>
                </div>
            </div>
            <div class="resource-row">
                <div class="resource-icon">
                    <img src="<%= ResolveUrl("~/en/images/arrow.gif") %>" width="30" height="25" border="0" alt="">
                </div>
                <div class="resource-content">
                    <asp:HyperLink ID="hlFAQ" runat="server" NavigateUrl="~/en/FAQ.aspx" CssClass="tutorial-link">
                        Frequently Asked Questions
                    </asp:HyperLink>
                    <p style="margin-top: 5px; margin-bottom: 0; font-size: 10pt; color: #333333;">
                        Find answers to common questions about using and customizing this template.
                    </p>
                </div>
            </div>
        </div>
        
        <div class="section-header section-header-gray">&nbsp;Other Resources</div>
        <div class="resource-box resource-box-yellow">
            <div class="resource-row">
                <div class="resource-icon">
                    <img src="<%= ResolveUrl("~/en/images/arrow.gif") %>" width="30" height="25" border="0" alt="">
                </div>
                <div class="resource-content">
                    <asp:HyperLink ID="hlExternal" runat="server" NavigateUrl="https://learn.microsoft.com/en-us/aspnet/web-forms/" CssClass="tutorial-link" Target="_blank">
                        ASP.NET Web Forms Documentation (Microsoft Learn)
                    </asp:HyperLink>
                    <p style="margin-top: 5px; margin-bottom: 0; font-size: 10pt; color: #000000;">
                        Looking to learn more about ASP.NET Web Forms? Microsoft's official documentation 
                        provides comprehensive guides and API references.
                    </p>
                </div>
            </div>
        </div>
        
        <p align="right" style="margin-top: 20px;"><a href="#top" style="font-size: 8pt; color: #000000;">Back to top ▲</a></p>
    </div>
    
</asp:Content>
