<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContentBox.ascx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox" %>

<style type="text/css">
    .content-box {
        width: 100%;
        margin-bottom: 20px;
    }
    
    .content-box-header {
        padding: 4px 8px;
        font-family: Tahoma, Verdana, Arial, sans-serif;
        font-size: 10pt;
        color: #FFFFFF;
        font-weight: bold;
    }
    
    .content-box-header-blue {
        background-color: #000080;
    }
    
    .content-box-header-gray {
        background-color: #666666;
    }
    
    .content-box-header-red {
        background-color: #CC0000;
    }
    
    .content-box-content {
        padding: 10px;
        font-family: Tahoma, Verdana, Arial, sans-serif;
        font-size: 10pt;
        border: 1px solid #808080;
        border-top: none;
    }
    
    .content-box-content-white {
        background-color: #FFFFFF;
    }
    
    .content-box-content-gray {
        background-color: #F2F2F2;
    }
    
    .content-box-content-yellow {
        background-color: #FFE6E6;
    }
</style>

<div class="content-box">
    <asp:Panel ID="pnlHeader" runat="server" CssClass="content-box-header">
        <asp:Literal ID="litHeader" runat="server"></asp:Literal>
    </asp:Panel>
    <asp:Panel ID="pnlContent" runat="server" CssClass="content-box-content">
        <asp:Literal ID="litContent" runat="server"></asp:Literal>
    </asp:Panel>
</div>
