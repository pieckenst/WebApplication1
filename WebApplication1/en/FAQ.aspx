<%@ Page Title="Frequently Asked Questions" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FAQ.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.en.FAQ" %>
<%@ Register TagPrefix="uc" TagName="ContentBox" Src="~/Controls/ContentBox.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <style type="text/css">
        .content-page { padding: 10px; font-family: Tahoma, Verdana, Arial, sans-serif; }
        .page-title { font-size: 16pt; color: #000080; font-weight: bold; }
        .page-divider { height: 2px; background-color: #000080; margin: 5px 0 15px 0; }
        .text-regular { font-size: 10pt; line-height: 1.5; }
        .faq-question { font-weight: bold; margin-top: 10px; }
        .faq-answer { margin-top: 5px; margin-bottom: 15px; }
    </style>
    
    <div class="content-page">
        <a name="top"></a>
        <div class="page-title">Frequently Asked Questions</div>
        <div class="page-divider"></div>
        
        <p class="text-regular" style="margin-top: 0; margin-bottom: 0;">
            Looking for answers to your questions? The FAQ is the right place!
        </p>
        
        <uc:ContentBox ID="cbGeneral" runat="server" HeaderText="General Project Questions" HeaderColor="Blue" ContentColor="White" />
        
        <uc:ContentBox ID="cbCompatibility" runat="server" HeaderText="Compatibility & Operating Systems" HeaderColor="Gray" ContentColor="Gray" />
        
        <uc:ContentBox ID="cbUpdates" runat="server" HeaderText="Updates & Contributing" HeaderColor="Blue" ContentColor="White" />
        
        <p align="right" style="margin-top: 20px;"><a href="#top" style="font-size: 8pt; color: #000000;">Back to top ▲</a></p>
    </div>
    
</asp:Content>
