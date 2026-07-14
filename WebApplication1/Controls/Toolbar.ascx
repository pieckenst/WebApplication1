<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Toolbar.ascx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Controls.Toolbar" %>
<%@ Import Namespace="BRU.WEBFORMS.ASPNET.APP" %>

<style>
    a.toolbar-link { text-decoration: none; }
    .toolbar-body { margin: 0; padding: 0; }
    .toolbar-body img { display: block; }
</style>

<div class="toolbar-body">
    <asp:HyperLink ID="hlHome" runat="server" NavigateUrl="<%= ResolveUrl(SiteConfig.HomeUrl) %>" CssClass="toolbar-link">
        <asp:Image ID="imgLogo" runat="server"
            ImageUrl="<%= ResolveUrl(SiteConfig.LogoUrl) %>"
            Width="<%= SiteConfig.LogoWidth %>"
            Height="<%= SiteConfig.LogoHeight %>"
            AlternateText="<%= SiteConfig.LogoAltText %>" />
    </asp:HyperLink>
</div>
