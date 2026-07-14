<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Toolbar.ascx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Controls.Toolbar" %>

<style>
    a.toolbar-link { text-decoration: none; }
    .toolbar-body { margin: 0; padding: 0; }
    .toolbar-body img { display: block; }
</style>

<div class="toolbar-body">
    <asp:HyperLink ID="hlHome" runat="server" CssClass="toolbar-link">
        <asp:Image ID="imgLogo" runat="server" />
    </asp:HyperLink>
</div>
