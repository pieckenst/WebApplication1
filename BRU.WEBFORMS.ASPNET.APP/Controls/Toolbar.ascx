<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Toolbar.ascx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Controls.Toolbar" %>

<style>
    a.toolbar-link { text-decoration: none; }
    .toolbar-body { margin: 0; padding: 0; }
    .toolbar-body img { display: block; }
    .auth-status { position: absolute; top: 10px; right: 16px; color: #FFFFFF; font: 12px Tahoma, Arial, sans-serif; }
    .auth-status a { color: #FFFFFF; margin-left: 12px; }
</style>

<div class="toolbar-body">
    <asp:HyperLink ID="hlHome" runat="server" CssClass="toolbar-link">
        <asp:Image ID="imgLogo" runat="server" />
    </asp:HyperLink>
    <div class="auth-status">
        <asp:Literal ID="litCurrentUser" runat="server" />
        <asp:HyperLink ID="hlSignIn" runat="server" NavigateUrl="~/Login.aspx" Text="Sign in" />
        <asp:HyperLink ID="hlSignOut" runat="server" NavigateUrl="~/Logout.aspx" Text="Sign out" />
    </div>
</div>
