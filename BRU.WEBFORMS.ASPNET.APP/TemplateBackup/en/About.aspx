<%@ Page Title="About - Windows XP Styled Website" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.en.About" %>
<%@ Register TagPrefix="uc" TagName="ContentBox" Src="~/Controls/ContentBox.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <style type="text/css">
        .content-page {
            padding: 10px;
            font-family: Tahoma, Verdana, Arial, sans-serif;
        }
        .page-title { font-size: 16pt; color: #000080; font-weight: bold; }
        .page-divider { height: 2px; background-color: #000080; margin: 5px 0 15px 0; }
        .text-regular { font-size: 10pt; line-height: 1.5; }
        .text-small { font-size: 8pt; color: #666666; }
    </style>
    
    <div class="content-page">
        <a name="top"></a>
        <div class="page-title">About This Project</div>
        <div class="page-divider"></div>
        
        <p class="text-regular" style="margin-top: 0; margin-bottom: 0;">
            Want to learn more about this project? If so, you are at the right place!
            <br><br>
            <span class="text-small"><b><asp:Literal ID="litLastUpdated" runat="server" /></b></span>
        </p>
        
        <uc:ContentBox ID="cbAbout" runat="server" HeaderText="What is this project about?" HeaderColor="Blue" ContentColor="White" />
        
        <uc:ContentBox ID="cbHowItWorks" runat="server" HeaderText="How does it work?" HeaderColor="Gray" ContentColor="Gray" />
        
        <uc:ContentBox ID="cbHistory" runat="server" HeaderText="The history of the project" HeaderColor="Blue" ContentColor="White" />
        
        <uc:ContentBox ID="cbContact" runat="server" HeaderText="Get in touch with us!" HeaderColor="Gray" ContentColor="Gray" />
        
        <uc:ContentBox ID="cbLegal" runat="server" HeaderText="Privacy Policy and Legal Information" HeaderColor="Blue" ContentColor="White" />
        
        <p align="right" style="margin-top: 20px;"><a href="#top" style="font-size: 8pt; color: #000000;">Back to top ▲</a></p>
    </div>
    
</asp:Content>
