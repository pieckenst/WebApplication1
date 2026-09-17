<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Footer.ascx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Controls.Footer" %>

<style type="text/css">
    .footer-container {
        background-color: #6487DC;
        height: 24px;
        display: flex;
        align-items: center;
        padding: 0 10px;
        font-family: Tahoma, Arial, sans-serif;
        font-size: 8pt;
        color: #FFFFFF;
    }
    
    .footer-container a {
        color: #FFFFFF;
        text-decoration: none;
    }
    
    .footer-container a:hover {
        text-decoration: underline;
    }
</style>

<div class="footer-container">
    <asp:Literal ID="litCopyright" runat="server" />
</div>
