<%@ Page Title="Legal Information" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Legal.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.en.Legal" %>
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
        <div class="page-title">Legal Information</div>
        <div class="page-divider"></div>
        
        <p class="text-regular" style="margin-top: 0; margin-bottom: 0;">
            Important legal information about this website template.
            <br><br>
            <span class="text-small"><b>Last Updated: <asp:Literal ID="litLastUpdated" runat="server" /></b></span>
        </p>
        
        <uc:ContentBox ID="cbDisclaimer" runat="server" HeaderText="Disclaimer" HeaderColor="Red" ContentColor="Yellow" />
        
        <uc:ContentBox ID="cbLicense" runat="server" HeaderText="License" HeaderColor="Blue" ContentColor="White" />
        
        <uc:ContentBox ID="cbTrademarks" runat="server" HeaderText="Trademarks" HeaderColor="Gray" ContentColor="Gray" />
        
        <uc:ContentBox ID="cbLiability" runat="server" HeaderText="Limitation of Liability" HeaderColor="Blue" ContentColor="White" />
        
        <p align="right" style="margin-top: 20px;"><a href="#top" style="font-size: 8pt; color: #000000;">Back to top ▲</a></p>
    </div>
    
</asp:Content>
