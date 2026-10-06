<%@ Page Title="<%$ Resources:Strings, Auto_System_Settings_1 %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.SystemPages.Settings" %>
<%@ Register TagPrefix="uc" TagName="ContentBox" Src="~/Controls/ContentBox.ascx" %>
<asp:Content ID="MainContent1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .content-page { padding: 10px; font-family: Tahoma, Verdana, Arial, sans-serif; }
        .page-title { font-size: 16pt; color: #000080; font-weight: bold; }
        .page-divider { height: 2px; background-color: #000080; margin: 5px 0 15px 0; }
        .form-row { margin-bottom: 12px; }
        .form-label { display: inline-block; width: 180px; font-weight: bold; font-size: 9pt; }
        .form-control { padding: 4px 8px; border: 1px solid #1447AE; font-family: Tahoma, Arial, sans-serif; font-size: 9pt; width: 320px; }
        .action-button { display: inline-block; padding: 6px 12px; background-color: #1447AE; color: #FFFFFF; text-decoration: none; border: 1px solid #2459C3; border-radius: 3px; margin-right: 8px; margin-bottom: 5px; font-family: Tahoma, Arial, sans-serif; font-size: 9pt; }
        .error-message { background-color: #FFE6E6; border: 1px solid #CC0000; color: #CC0000; padding: 10px; margin-bottom: 15px; font-size: 10pt; }
        .success-message { background-color: #E6FFE6; border: 1px solid #008000; color: #008000; padding: 10px; margin-bottom: 15px; font-size: 10pt; }
    </style>
    <div class="content-page">
        <div class="page-title"><%= Localization.GetHtml("Settings_Heading") %></div>
        <div class="page-divider"></div>
        <asp:Label ID="lblError" runat="server" CssClass="error-message" Visible="false" />
        <asp:Label ID="lblSuccess" runat="server" CssClass="success-message" Visible="false" />
        <uc:ContentBox ID="cbSettings" runat="server" HeaderText="Site Configuration" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <asp:Panel ID="pnlSettings" runat="server" DefaultButton="btnSave">
                    <div class="form-row"><span class="form-label"><%= Localization.GetHtml("Settings_SiteName") %></span><asp:TextBox ID="txtSiteName" runat="server" CssClass="form-control" MaxLength="100" /></div>
                    <div class="form-row"><span class="form-label"><%= Localization.GetHtml("Settings_LanguageLabel") %></span><asp:DropDownList ID="ddlLanguage" runat="server" CssClass="form-control"><asp:ListItem Value="en" Text="<%$ Resources:Strings, Settings_LanguageEnglish %>" /><asp:ListItem Value="ru" Text="<%$ Resources:Strings, Settings_LanguageRussian %>" /></asp:DropDownList></div>
                    <div class="form-row"><span class="form-label"><%= Localization.GetHtml("Settings_DefaultPageTitle") %></span><asp:TextBox ID="txtDefaultPageTitle" runat="server" CssClass="form-control" MaxLength="150" /></div>
                    <div class="form-row"><span class="form-label"><%= Localization.GetHtml("Settings_Copyright") %></span><asp:TextBox ID="txtCopyright" runat="server" CssClass="form-control" MaxLength="200" /></div>
                    <div class="form-row"><span class="form-label"><%= Localization.GetHtml("Settings_LogoAlt") %></span><asp:TextBox ID="txtLogoAlt" runat="server" CssClass="form-control" MaxLength="100" /></div>
                    <div class="form-row"><span class="form-label"><%= Localization.GetHtml("Settings_LogoWidth") %></span><asp:TextBox ID="txtLogoWidth" runat="server" CssClass="form-control" MaxLength="4" /></div>
                    <div class="form-row"><span class="form-label"><%= Localization.GetHtml("Settings_LogoHeight") %></span><asp:TextBox ID="txtLogoHeight" runat="server" CssClass="form-control" MaxLength="4" /></div>
                    <div class="form-row"><asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Strings, Settings_SaveButton %>" CssClass="action-button" OnClick="btnSave_Click" /></div>
                </asp:Panel>
            </ContentTemplate>
        </uc:ContentBox>
    </div>
</asp:Content>
