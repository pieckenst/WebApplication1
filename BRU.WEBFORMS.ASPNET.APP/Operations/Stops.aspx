<%@ Page Title="<%$ Resources:Strings, Auto_Operations_Stops_18 %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Stops.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Operations.Stops" %>
<%@ Register TagPrefix="uc" TagName="ContentBox" Src="~/Controls/ContentBox.ascx" %>
<asp:Content ID="MainContent1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .content-page { padding: 18px; font-family: Tahoma, Verdana, Arial, sans-serif; max-width: 1480px; margin: 0 auto; box-sizing: border-box; }
        .content-page > * + * { margin-top: 16px; }
        .content-page input, .content-page select, .content-page textarea { box-sizing: border-box; max-width: 100%; }
        .filter-row { display: flex; flex-wrap: wrap; align-items: center; gap: 8px 12px; margin-bottom: 12px; }
        .filter-row:last-child { margin-bottom: 0; }
        .action-bar { display: flex; flex-wrap: wrap; align-items: center; gap: 8px; padding: 12px; margin-bottom: 18px; }
        .data-table { display: block; overflow-x: auto; white-space: nowrap; }
        @media (max-width: 760px) { .content-page { padding: 10px; } .filter-label { width: auto; min-width: 120px; } .data-table { font-size: 8pt; } }
        .page-title { font-size: 16pt; color: #000080; font-weight: bold; }
        .page-divider { height: 2px; background-color: #000080; margin: 5px 0 15px 0; }
        .action-bar { background-color: #EDF2FB; border: 1px solid #1447AE; padding: 10px; margin-bottom: 15px; }
        .action-button { display: inline-block; padding: 6px 12px; background-color: #1447AE; color: #FFFFFF; text-decoration: none; border: 1px solid #2459C3; border-radius: 3px; margin-right: 8px; margin-bottom: 5px; font-family: Tahoma, Arial, sans-serif; font-size: 9pt; }
        .form-row { margin-bottom: 12px; }
        .form-label { display: inline-block; width: 150px; font-weight: bold; font-size: 9pt; }
        .form-control { padding: 4px 8px; border: 1px solid #1447AE; font-family: Tahoma, Arial, sans-serif; font-size: 9pt; width: 300px; }
        .data-table { width: 100%; border-collapse: collapse; margin-top: 10px; font-size: 9pt; }
        .data-table th { background-color: #1447AE; color: #FFFFFF; padding: 8px; text-align: left; border: 1px solid #0A2E7A; }
        .data-table td { padding: 8px; border: 1px solid #CCCCCC; background-color: #FFFFFF; }
        .error-message { background-color: #FFE6E6; border: 1px solid #CC0000; color: #CC0000; padding: 10px; margin-bottom: 15px; font-size: 10pt; }
        .success-message { background-color: #E6FFE6; border: 1px solid #008000; color: #008000; padding: 10px; margin-bottom: 15px; font-size: 10pt; }
        .pagination { margin-top: 15px; text-align: right; }
        .data-table { display: block; overflow-x: auto; white-space: nowrap; }
        @media (max-width: 760px) {
            .content-page { padding: 8px; }
            .page-title { font-size: 14pt; }
            .action-bar { display: flex; flex-wrap: wrap; gap: 6px; }
            .action-button { margin: 0; }
            .form-row, .filter-row { display: flex; flex-wrap: wrap; gap: 6px; }
            .form-label, .filter-label { display: block; width: 100%; margin: 0 !important; }
            .form-control, select, input[type='text'] { width: 100%; max-width: 100%; box-sizing: border-box; }
        }
    </style>
    <div class="content-page">
        <div class="page-title"><%= Localization.GetHtml("Auto_Operations_Stops_19") %></div>
        <div class="page-divider"></div>
        <asp:Label ID="lblError" runat="server" CssClass="error-message" Visible="false" />
        <asp:Label ID="lblSuccess" runat="server" CssClass="success-message" Visible="false" />
        <div class="action-bar">
            <asp:Button ID="btnNew" runat="server" Text="<%$ Resources:Strings, Auto_Operations_Stops_1 %>" CssClass="action-button" OnClick="btnNew_Click" CausesValidation="false" />
            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" MaxLength="120" placeholder="<%$ Resources:Strings, Auto_Operations_Stops_2 %>" />
            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control"><asp:ListItem Text="<%$ Resources:Strings, Auto_Operations_Stops_3 %>" Value="" /><asp:ListItem Text="<%$ Resources:Strings, Auto_Operations_Stops_4 %>" Value="1" /><asp:ListItem Text="<%$ Resources:Strings, Auto_Operations_Stops_5 %>" Value="0" /></asp:DropDownList>
            <asp:Button ID="btnApply" runat="server" Text="<%$ Resources:Strings, Auto_Operations_Stops_6 %>" CssClass="action-button" OnClick="btnApply_Click" />
        </div>
        <uc:ContentBox ID="cbStopEditor" runat="server" HeaderText="Add / Edit Stop" HeaderColor="Blue" ContentColor="White" Visible="false">
            <ContentTemplate>
                <asp:Panel ID="pnlEditor" runat="server" DefaultButton="btnSave" Visible="false">
                    <asp:HiddenField ID="hidStopId" runat="server" Value="0" />
                    <div class="form-row"><span class="form-label"><%= Localization.GetHtml("Auto_Operations_Stops_20") %></span><asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="120" /></div>
                    <div class="form-row"><span class="form-label"><%= Localization.GetHtml("Auto_Operations_Stops_21") %></span><asp:TextBox ID="txtLocation" runat="server" CssClass="form-control" MaxLength="200" /></div>
                    <div class="form-row"><span class="form-label"><%= Localization.GetHtml("Auto_Operations_Stops_22") %></span><asp:TextBox ID="txtLatitude" runat="server" CssClass="form-control" MaxLength="16" /></div>
                    <div class="form-row"><span class="form-label"><%= Localization.GetHtml("Auto_Operations_Stops_23") %></span><asp:TextBox ID="txtLongitude" runat="server" CssClass="form-control" MaxLength="16" /></div>
                    <div class="form-row"><asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Strings, Auto_Operations_Stops_7 %>" CssClass="action-button" OnClick="btnSave_Click" /><asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Strings, Auto_Operations_Stops_8 %>" CssClass="action-button" OnClick="btnCancel_Click" CausesValidation="false" /></div>
                </asp:Panel>
            </ContentTemplate>
        </uc:ContentBox>
        <uc:ContentBox ID="cbStopsList" runat="server" HeaderText="Stops" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <asp:GridView ID="gvStops" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="25" CssClass="data-table" GridLines="Both" PagerStyle-CssClass="pagination" OnPageIndexChanging="gvStops_PageIndexChanging" OnRowCommand="gvStops_RowCommand" OnRowDataBound="gvStops_RowDataBound" EmptyDataText="<%$ Resources:Strings, Auto_Operations_Stops_9 %>">
                    <Columns>
                        <asp:BoundField DataField="StopName" HeaderText="<%$ Resources:Strings, Auto_Operations_Stops_10 %>" />
                        <asp:BoundField DataField="Location" HeaderText="<%$ Resources:Strings, Auto_Operations_Stops_11 %>" />
                        <asp:BoundField DataField="Coordinates" HeaderText="<%$ Resources:Strings, Auto_Operations_Stops_12 %>" />
                        <asp:BoundField DataField="RouteCount" HeaderText="<%$ Resources:Strings, Auto_Operations_Stops_13 %>" />
                        <asp:BoundField DataField="IsActive" HeaderText="<%$ Resources:Strings, Auto_Operations_Stops_14 %>" />
                        <asp:TemplateField HeaderText="<%$ Resources:Strings, Auto_Operations_Stops_15 %>"><ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" Text="<%$ Resources:Strings, Auto_Operations_Stops_16 %>"<%= Localization.GetHtml("Auto_Operations_Stops_24") %><%# Eval("StopId") %>' CausesValidation="false" />
                            <asp:LinkButton ID="btnToggle" runat="server" Text="<%$ Resources:Strings, Auto_Operations_Stops_17 %>"<%= Localization.GetHtml("Auto_Operations_Stops_25") %><%# Eval("StopId") %>' CausesValidation="false" />
                        </ItemTemplate></asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </uc:ContentBox>
    </div>
</asp:Content>
