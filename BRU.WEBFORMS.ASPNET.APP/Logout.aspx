<%@ Page Title="<%$ Resources:Strings, Auto_Logout_1 %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Logout" %>
<asp:Content ID="MainContent1" ContentPlaceHolderID="MainContent" runat="server">
    <p><%= Localization.GetHtml("Logout_Message") %></p>
</asp:Content>
