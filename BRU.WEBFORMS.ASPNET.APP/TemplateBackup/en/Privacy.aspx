<%@ Page Title="Privacy Policy" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Privacy.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.en.Privacy" %>
<%@ Register TagPrefix="uc" TagName="ContentBox" Src="~/Controls/ContentBox.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <style type="text/css">
        .content-page { padding: 10px; font-family: Tahoma, Verdana, Arial, sans-serif; }
        .page-title { font-size: 16pt; color: #000080; font-weight: bold; }
        .page-divider { height: 2px; background-color: #000080; margin: 5px 0 15px 0; }
        .text-regular { font-size: 10pt; line-height: 1.5; }
        .text-small { font-size: 8pt; color: #666666; }
    </style>
    
    <div class="content-page">
        <a name="top"></a>
        <div class="page-title">Privacy Policy</div>
        <div class="page-divider"></div>
        
        <p class="text-regular" style="margin-top: 0; margin-bottom: 0;">
            This privacy policy explains how we collect, use, and protect your information.
            <br><br>
            <span class="text-small"><b>Last Updated: <asp:Literal ID="litLastUpdated" runat="server" /></b></span>
        </p>
        
        <uc:ContentBox ID="cbInfoCollect" runat="server" HeaderText="Information We Collect" HeaderColor="Blue" ContentColor="White" />
        
        <uc:ContentBox ID="cbInfoUse" runat="server" HeaderText="How We Use Your Information" HeaderColor="Gray" ContentColor="Gray" />
        
        <uc:ContentBox ID="cbDataSecurity" runat="server" HeaderText="Data Security" HeaderColor="Blue" ContentColor="White" />
        
        <uc:ContentBox ID="cbContact" runat="server" HeaderText="Contact Us" HeaderColor="Gray" ContentColor="White" />
        
        <p align="right" style="margin-top: 20px;"><a href="#top" style="font-size: 8pt; color: #000000;">Back to top ▲</a></p>
    </div>
    
</asp:Content>
