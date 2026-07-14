<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Navigationbar.ascx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Controls.Navigationbar" %>
<%@ Import Namespace="BRU.WEBFORMS.ASPNET.APP" %>

<style type="text/css">
    .nav-body {
        margin: 0px;
        padding: 0px;
        background-color: #8CACDF;
        font-family: Tahoma, Arial, Helvetica, sans-serif;
        font-size: 8pt;
    }
    
    .nav-table {
        font-family: Tahoma, Arial, Helvetica, sans-serif;
        font-size: 8pt;
    }
    
    .nav-mainborder {
        border: 1px solid #1447AE;
        background-color: #EDF2FB;
    }
    
    .nav-header {
        background-color: #1447AE;
        color: #FFFFFF;
        font-weight: bold;
        padding-left: 11px;
        height: 25px;
    }
    
    .nav-content {
        padding: 11px;
        background-color: #EDF2FB;
        line-height: 18px;
    }
    
    .nav-subheader {
        margin-top: 20px;
        margin-bottom: 6px;
        color: #2459C3;
        font-weight: bold;
    }
    
    .nav-link {
        text-decoration: none;
        color: #0000FF;
        line-height: 18px;
    }
    
    .nav-link:hover {
        text-decoration: underline;
        color: #3366FF;
    }
    
    .nav-specialred {
        color: #8B0000;
        font-weight: bold;
    }
    
    .nav-specialblue {
        color: #1447AE;
        font-weight: bold;
        font-style: italic;
    }
    
    .nav-linkimg {
        border: 0px;
        width: 11px;
        height: 11px;
        vertical-align: middle;
    }
    
    .nav-linkrow {
        padding-top: 2px;
        padding-bottom: 2px;
    }
</style>

<div class="nav-body" style="height: 100%;">
    <table cellspacing="11" cellpadding="0" border="0" width="100%" height="100%">
    <tr>
    <td valign="top" width="100%" height="100%">
    
    <table class="nav-mainborder" cellspacing="0" cellpadding="0" border="0" width="100%" height="100%">
    <tr>
    <td class="nav-header" valign="middle" height="25">
        <asp:Literal ID="litSiteName" runat="server" Text="<%= SiteConfig.SiteName %>" />
    </td>
    </tr>
    
    <tr>
    <td class="nav-content" valign="top">
    
        <!-- Main Navigation Section -->
        <asp:Repeater ID="rptNavigation" runat="server" OnItemDataBound="rptNavigation_ItemDataBound">
            <ItemTemplate>
                <asp:PlaceHolder ID="phSubheader" runat="server" Visible='<%# Eval("IsSubheader") %>'>
                    <div class="nav-subheader"><%# Eval("Title") %></div>
                </asp:PlaceHolder>
                
                <asp:PlaceHolder ID="phLink" runat="server" Visible='<%# !((bool)Eval("IsSubheader")) %>'>
                    <div class="nav-linkrow">
                        <img src='<%# ResolveUrl((string)Eval("IconUrl")) %>' class="nav-linkimg" alt="">
                        <asp:HyperLink ID="hlNavItem" runat="server" 
                            NavigateUrl='<%# Eval("NavigateUrl") %>'
                            CssClass='<%# GetLinkCssClass(Eval("CssClass")) %>'
                            Target='<%# Eval("Target") %>'
                            ToolTip='<%# Eval("ToolTip") %>'>
                            <%# Eval("Title") %>
                        </asp:HyperLink>
                    </div>
                </asp:PlaceHolder>
            </ItemTemplate>
        </asp:Repeater>
    
    </td>
    </tr>
    </table>
    
    </td>
    </tr>
    </table>
</div>
