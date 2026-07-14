<%@ Page Title="Welcome - Windows XP Styled Website" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP._Default" %>
<%@ Register TagPrefix="uc" TagName="ContentBox" Src="~/Controls/ContentBox.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <style type="text/css">
        .content-page {
            padding: 20px;
            font-family: Tahoma, Verdana, Arial, sans-serif;
        }
        
        .page-title {
            font-size: 16pt;
            color: #000080;
            font-weight: bold;
            margin-bottom: 5px;
        }
        
        .page-subtitle {
            font-size: 10pt;
            color: #666666;
            margin-bottom: 15px;
        }
        
        .page-divider {
            height: 2px;
            background-color: #000080;
            margin-bottom: 20px;
        }
        
        .welcome-text {
            font-size: 10pt;
            line-height: 1.6;
            margin-bottom: 15px;
        }
        
        .feature-list {
            margin: 10px 0;
            padding-left: 20px;
        }
        
        .feature-list li {
            margin-bottom: 8px;
            font-size: 10pt;
        }
    </style>
    
    <div class="content-page">
        <div class="page-title">
            <asp:Literal ID="litTitle" runat="server">Welcome to Our Website!</asp:Literal>
        </div>
        <div class="page-subtitle">
            <asp:Literal ID="litLastUpdated" runat="server" />
        </div>
        <div class="page-divider"></div>
        
        <uc:ContentBox ID="cbWelcome" runat="server" 
            HeaderText="Welcome!" 
            HeaderColor="Blue" 
            ContentColor="White">
        </uc:ContentBox>
        
        <uc:ContentBox ID="cbAbout" runat="server" 
            HeaderText="About This Site" 
            HeaderColor="Gray" 
            ContentColor="Gray">
        </uc:ContentBox>
        
        <uc:ContentBox ID="cbFeatures" runat="server" 
            HeaderText="Key Features" 
            HeaderColor="Blue" 
            ContentColor="White">
        </uc:ContentBox>
        
        <uc:ContentBox ID="cbGetStarted" runat="server" 
            HeaderText="Getting Started" 
            HeaderColor="Gray" 
            ContentColor="White">
        </uc:ContentBox>
    </div>
    
</asp:Content>
