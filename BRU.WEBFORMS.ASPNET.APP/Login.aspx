<%@ Page Title="<%$ Resources:Strings, Auto_Login_1 %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Login" %>
<%@ Register TagPrefix="uc" TagName="ContentBox" Src="~/Controls/ContentBox.ascx" %>
<asp:Content ID="HeadContent1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .content-page { padding: 10px; font-family: Tahoma, Verdana, Arial, sans-serif; }
        .page-title { font-size: 16pt; color: #000080; font-weight: bold; }
        .page-divider { height: 2px; background-color: #000080; margin: 5px 0 15px 0; }
        .form-row { margin-bottom: 12px; }
        .form-label { display: inline-block; width: 150px; font-weight: bold; font-size: 9pt; }
        .form-control { padding: 4px 8px; border: 1px solid #1447AE; font-family: Tahoma, Arial, sans-serif; font-size: 9pt; width: 300px; }
        .action-button { display: inline-block; padding: 6px 12px; background-color: #1447AE; color: #FFFFFF; text-decoration: none; border: 1px solid #2459C3; border-radius: 3px; margin-right: 8px; margin-bottom: 5px; font-family: Tahoma, Arial, sans-serif; font-size: 9pt; }
        .error-message { background-color: #FFE6E6; border: 1px solid #CC0000; color: #CC0000; padding: 10px; margin-bottom: 15px; font-size: 10pt; }
    </style>
</asp:Content>
<asp:Content ID="MainContent1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-page">
        <div class="page-title"><%= Localization.GetHtml("Login_Heading") %></div>
        <div class="page-divider"></div>
        <asp:Panel ID="pnlError" runat="server" CssClass="error-message" Visible="false" role="alert">
            <asp:Literal ID="litError" runat="server" />
        </asp:Panel>
        <uc:ContentBox ID="cbSignIn" runat="server" HeaderText="<%$ Resources:Strings, Login_AccountHeader %>" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
            <div class="form-row"><span class="form-label"><%= Localization.GetHtml("Login_LoginLabel") %></span><asp:TextBox ID="txtLogin" runat="server" CssClass="form-control" MaxLength="80" autocomplete="username" /></div>
            <div class="form-row"><span class="form-label"><%= Localization.GetHtml("Login_PasswordLabel") %></span><asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" MaxLength="100" autocomplete="current-password" /></div>
            <div class="form-row"><span class="form-label"></span><asp:CheckBox ID="chkRememberMe" runat="server" Text="<%$ Resources:Strings, Login_Remember %>" /></div>
            <div class="form-row"><span class="form-label"></span><asp:Button ID="btnSignIn" runat="server" Text="<%$ Resources:Strings, Login_Button %>" CssClass="action-button" OnClick="btnSignIn_Click" /></div>
            </ContentTemplate>
        </uc:ContentBox>
    </div>
</asp:Content>
