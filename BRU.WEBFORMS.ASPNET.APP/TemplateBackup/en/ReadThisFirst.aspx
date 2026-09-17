<%@ Page Title="READ THIS FIRST" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReadThisFirst.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.en.ReadThisFirst" %>
<%@ Register TagPrefix="uc" TagName="ContentBox" Src="~/Controls/ContentBox.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <style type="text/css">
        .content-page { padding: 10px; font-family: Tahoma, Verdana, Arial, sans-serif; }
        .page-title { font-size: 16pt; color: #800000; font-weight: bold; }
        .page-divider { height: 2px; background-color: #800000; margin: 5px 0 15px 0; }
        .text-regular { font-size: 10pt; line-height: 1.5; }
        .text-small { font-size: 8pt; color: #666666; }
        .toc-list { margin: 0; padding-left: 20px; }
        .toc-list li { margin-bottom: 8px; font-size: 10pt; }
    </style>
    
    <div class="content-page">
        <a name="top"></a>
        <div class="page-title">READ THIS FIRST</div>
        <div class="page-divider"></div>
        
        <p class="text-regular" style="margin-top: 0; margin-bottom: 0;">
            Here's what you need to know before using this template.
            <br><br>
            <span class="text-small"><b><asp:Literal ID="litLastUpdated" runat="server" /></b></span>
        </p>
        
        <uc:ContentBox ID="cbTOC" runat="server" HeaderText="Table of Contents" HeaderColor="Gray" ContentColor="Gray" />
        
        <uc:ContentBox ID="cbDisclaimer" runat="server" HeaderText="Disclaimer and Important Notices" HeaderColor="Red" ContentColor="Yellow" />
        
        <uc:ContentBox ID="cbWelcome" runat="server" HeaderText="Welcome!" HeaderColor="Blue" ContentColor="White" />
        
        <uc:ContentBox ID="cbFeatures" runat="server" HeaderText="Key Features" HeaderColor="Gray" ContentColor="Gray" />
        
        <uc:ContentBox ID="cbStarted" runat="server" HeaderText="Getting Started" HeaderColor="Blue" ContentColor="White" />
        
        <p align="right" style="margin-top: 20px;"><a href="#top" style="font-size: 8pt; color: #000000;">Back to top ▲</a></p>
    </div>
    
</asp:Content>
