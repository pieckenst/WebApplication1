<%@ Page Title="Users - Autopark Management System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.SystemPages.Users" %>
<%@ Register TagPrefix="uc" TagName="ContentBox" Src="~/Controls/ContentBox.ascx" %>
<asp:Content ID="MainContent1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .content-page { padding: 10px; font-family: Tahoma, Verdana, Arial, sans-serif; }
        .page-title { font-size: 16pt; color: #000080; font-weight: bold; }
        .page-divider { height: 2px; background-color: #000080; margin: 5px 0 15px 0; }
        .data-table { width: 100%; border-collapse: collapse; margin-top: 10px; font-size: 9pt; }
        .data-table th { background-color: #1447AE; color: #FFFFFF; padding: 8px; text-align: left; border: 1px solid #0A2E7A; }
        .data-table td { padding: 8px; border: 1px solid #CCCCCC; background-color: #FFFFFF; }
        .error-message { background-color: #FFE6E6; border: 1px solid #CC0000; color: #CC0000; padding: 10px; margin-bottom: 15px; font-size: 10pt; }
        @media (max-width: 760px) { .content-page { padding: 8px; } .data-table { min-width: 560px; } }
    </style>
    <div class="content-page">
        <div class="page-title"><%= Localization.GetHtml("Users_Heading") %></div>
        <div class="page-divider"></div>
        <asp:Label ID="lblError" runat="server" CssClass="error-message" Visible="false" />
        <uc:ContentBox ID="cbUsersList" runat="server" HeaderText="Application Users" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="false" CssClass="data-table" GridLines="Both" EmptyDataText="No users found.">
                    <Columns>
                        <asp:BoundField DataField="Login" HeaderText="Login" />
                        <asp:BoundField DataField="EmployeeName" HeaderText="Employee" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:BoundField DataField="PhoneNumber" HeaderText="Phone" />
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <%# ((BRU.WEBFORMS.ASPNET.APP.Models.User)Container.DataItem).StatusDescription %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </uc:ContentBox>
    </div>
</asp:Content>
