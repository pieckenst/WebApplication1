<%@ Page Title="Donations" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Donations.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.en.Donations" %>
<%@ Register TagPrefix="uc" TagName="ContentBox" Src="~/Controls/ContentBox.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <style type="text/css">
        .content-page { padding: 10px; font-family: Tahoma, Verdana, Arial, sans-serif; }
        .page-title { font-size: 16pt; color: #000080; font-weight: bold; }
        .page-divider { height: 2px; background-color: #000080; margin: 5px 0 15px 0; }
        .text-regular { font-size: 10pt; line-height: 1.5; }
    </style>
    
    <div class="content-page">
        <a name="top"></a>
        <div class="page-title">Donations</div>
        <div class="page-divider"></div>
        
        <p class="text-regular" style="margin-top: 0;">
            If you find this template useful, consider supporting its continued development.
        </p>
        
        <uc:ContentBox ID="cbSupport" runat="server" HeaderText="Support This Project" HeaderColor="Blue" ContentColor="White" />
        
        <uc:ContentBox ID="cbHowToHelp" runat="server" HeaderText="Other Ways to Help" HeaderColor="Gray" ContentColor="Gray" />
        
        <p align="right" style="margin-top: 20px;"><a href="#top" style="font-size: 8pt; color: #000000;">Back to top ▲</a></p>
    </div>
    
</asp:Content>
