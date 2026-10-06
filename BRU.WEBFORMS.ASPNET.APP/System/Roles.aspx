<%@ Page Title="<%$ Resources:Strings, Auto_System_Roles_7 %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Roles.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.SystemPages.Roles" %>
<%@ Register TagPrefix="uc" TagName="ContentBox" Src="~/Controls/ContentBox.ascx" %>
<asp:Content ID="MainContent1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .content-page { padding: 18px; font-family: Tahoma, Verdana, Arial, sans-serif; max-width: 1480px; margin: 0 auto; box-sizing: border-box; }
        .content-page > * + * { margin-top: 16px; }
        .content-page input, .content-page select, .content-page textarea { box-sizing: border-box; max-width: 100%; }
        .action-bar { display: flex; flex-wrap: wrap; align-items: center; gap: 8px; padding: 12px; margin-bottom: 18px; }
        .data-table { display: block; overflow-x: auto; white-space: nowrap; }
        @media (max-width: 760px) { .content-page { padding: 10px; } .data-table { font-size: 8pt; } }
        .page-title { font-size: 16pt; color: #000080; font-weight: bold; }
        .page-divider { height: 2px; background-color: #000080; margin: 5px 0 15px 0; }
        .data-table { width: 100%; border-collapse: collapse; margin-top: 10px; font-size: 9pt; }
        .data-table th { background-color: #1447AE; color: #FFFFFF; padding: 8px; text-align: left; border: 1px solid #0A2E7A; }
        .data-table td { padding: 8px; border: 1px solid #CCCCCC; background-color: #FFFFFF; }
        .error-message { background-color: #FFE6E6; border: 1px solid #CC0000; color: #CC0000; padding: 10px; margin-bottom: 15px; font-size: 10pt; }
        .data-table { display: block; overflow-x: auto; white-space: nowrap; }
        @media (max-width: 760px) {
            .content-page { padding: 8px; }
            .page-title { font-size: 14pt; }
            .data-table { font-size: 8pt; }
        }
    </style>
    <div class="content-page">
        <div class="page-title"><%= Localization.GetHtml("Auto_System_Roles_8") %></div>
        <div class="page-divider"></div>
        <asp:Label ID="lblError" runat="server" CssClass="error-message" Visible="false" />
        <uc:ContentBox ID="cbRolesList" runat="server" HeaderText="Security Roles" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <asp:GridView ID="gvRoles" runat="server" AutoGenerateColumns="false" CssClass="data-table" GridLines="Both" EmptyDataText="<%$ Resources:Strings, Auto_System_Roles_1 %>">
                    <Columns>
                        <asp:BoundField DataField="RoleName" HeaderText="<%$ Resources:Strings, Auto_System_Roles_2 %>" />
                        <asp:BoundField DataField="Description" HeaderText="<%$ Resources:Strings, Auto_System_Roles_3 %>" />
                        <asp:BoundField DataField="UserCount" HeaderText="<%$ Resources:Strings, Auto_System_Roles_4 %>" />
                        <asp:BoundField DataField="PermissionCount" HeaderText="<%$ Resources:Strings, Auto_System_Roles_5 %>" />
                        <asp:TemplateField HeaderText="<%$ Resources:Strings, Auto_System_Roles_6 %>">
                            <ItemTemplate>
                                <%# ((BRU.WEBFORMS.ASPNET.APP.Models.Role)Container.DataItem).IsActive ? "Active" : "Inactive" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </uc:ContentBox>
    </div>
</asp:Content>
