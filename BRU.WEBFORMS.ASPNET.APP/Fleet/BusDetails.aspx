<%@ Page Title="<%$ Resources:Strings, Common_View %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BusDetails.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Fleet.BusDetails" %>
<%@ Register TagPrefix="uc" TagName="ContentBox" Src="~/Controls/ContentBox.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .content-page { padding: 18px; max-width: 1400px; margin: 0 auto; font-family: Tahoma, Verdana, Arial, sans-serif; box-sizing: border-box; }
        .page-title { font-size: 16pt; font-weight: bold; color: #000080; }
        .page-divider { height: 2px; background: #000080; margin: 6px 0 16px; }
        .breadcrumb { font-size: 9pt; color: #666; margin-bottom: 8px; }
        .breadcrumb a { color: #1447AE; text-decoration: none; }
        .breadcrumb .separator { margin: 0 5px; color: #999; }
        .action-bar { display: flex; flex-wrap: wrap; gap: 8px; padding: 12px; border: 1px solid #1447AE; background: #EDF2FB; }
        .action-button { display: inline-block; padding: 7px 12px; border: 1px solid #2459C3; background: #1447AE; color: #fff; border-radius: 3px; font: 9pt Tahoma, Arial, sans-serif; cursor: pointer; text-decoration: none; }
        .action-button:hover { background: #2459C3; }
        .action-button.secondary { background: #666; border-color: #444; }
        .error-message { background: #FFE6E6; color: #C00; border: 1px solid #C00; padding: 10px; margin-bottom: 15px; }
        .stats-summary { background: #EDF2FB; border: 1px solid #1447AE; padding: 10px; font-size: 9pt; }
        .stats-item { display: inline-block; margin: 4px 22px 4px 0; }
        .stats-label { font-weight: bold; color: #1447AE; }
        .detail-row { margin: 8px 0; font-size: 9pt; }
        .detail-label { display: inline-block; width: 200px; color: #666; font-weight: bold; }
        .detail-value { color: #111; }
        .data-table { width: 100%; border-collapse: collapse; font-size: 9pt; margin-top: 10px; }
        .data-table th { background: #1447AE; color: #fff; border: 1px solid #0A2E7A; padding: 8px; text-align: left; }
        .data-table td { background: #fff; border: 1px solid #CCC; padding: 8px; }
        .data-table tr:hover td { background: #EDF2FB; }
        .status-operational { color: #080; font-weight: bold; }
        .status-repair { color: #C60; font-weight: bold; }
        .status-retired { color: #C00; font-weight: bold; }
        .status-reserve { color: #666; font-weight: bold; }
        .rw-operational { color: #080; font-weight: bold; }
        .rw-attention { color: #C60; font-weight: bold; }
        .rw-notoperational { color: #C00; font-weight: bold; }
        .overdue { color: #C00; font-weight: bold; }
        .empty-state { text-align: center; padding: 24px; color: #666; }
        @media (max-width: 760px) {
            .content-page { padding: 8px; }
            .page-title { font-size: 14pt; }
            .detail-label { display: block; width: auto; margin-bottom: 3px; }
            .data-table { display: block; overflow-x: auto; white-space: nowrap; }
        }
    </style>

    <div class="content-page">
        <div class="breadcrumb">
            <a href='<%= ResolveUrl("~/Default.aspx") %>'>Home</a><span class="separator">/</span>
            <a href='<%= ResolveUrl("~/Fleet/Buses.aspx") %>'><%= Localization.GetHtml("Nav_BusFleet") %></a><span class="separator">/</span>
            <asp:Literal ID="litBreadcrumb" runat="server" />
        </div>

        <div class="page-title"><asp:Literal ID="litPageTitle" runat="server" /></div>
        <div class="page-divider"></div>

        <asp:Panel ID="pnlError" runat="server" CssClass="error-message" Visible="false">
            <asp:Literal ID="litError" runat="server" />
        </asp:Panel>

        <div class="action-bar">
            <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:Strings, Nav_BusFleet %>" CssClass="action-button secondary" OnClick="btnBack_Click" CausesValidation="false" />
            <asp:Button ID="btnEditBus" runat="server" Text="<%$ Resources:Strings, Common_Edit %>" CssClass="action-button" OnClick="btnEditBus_Click" CausesValidation="false" />
            <asp:Button ID="btnMaintenance" runat="server" Text="<%$ Resources:Strings, Nav_Maintenance %>" CssClass="action-button" OnClick="btnMaintenance_Click" CausesValidation="false" />
        </div>

        <uc:ContentBox ID="cbBusDetails" runat="server" HeaderText="Bus details" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <div class="detail-row"><span class="detail-label"><%= Localization.GetHtml("Common_ID") %>:</span> <asp:Literal ID="litBusId" runat="server" /></div>
                <div class="detail-row"><span class="detail-label"><%= Localization.GetHtml("Common_FleetNumber") %>:</span> <asp:Literal ID="litFleetNumber" runat="server" /></div>
                <div class="detail-row"><span class="detail-label"><%= Localization.GetHtml("Common_Registration") %>:</span> <asp:Literal ID="litRegistration" runat="server" /></div>
                <div class="detail-row"><span class="detail-label"><%= Localization.GetHtml("Common_Manufacturer") %>:</span> <asp:Literal ID="litManufacturer" runat="server" /></div>
                <div class="detail-row"><span class="detail-label"><%= Localization.GetHtml("Common_Model") %>:</span> <asp:Literal ID="litModel" runat="server" /></div>
                <div class="detail-row"><span class="detail-label"><%= Localization.GetHtml("Common_Year") %>:</span> <asp:Literal ID="litYear" runat="server" /></div>
                <div class="detail-row"><span class="detail-label"><%= Localization.GetHtml("Common_Capacity") %>:</span> <asp:Literal ID="litCapacity" runat="server" /></div>
                <div class="detail-row"><span class="detail-label"><%= Localization.GetHtml("Common_Status") %>:</span> <asp:Literal ID="litStatus" runat="server" /></div>
                <div class="detail-row"><span class="detail-label"><%= Localization.GetHtml("Common_MileageKm") %>:</span> <asp:Literal ID="litMileage" runat="server" /></div>
                <div class="detail-row"><span class="detail-label"><%= Localization.GetHtml("Buses_MileageCategory") %>:</span> <asp:Literal ID="litMileageCategory" runat="server" /></div>
            </ContentTemplate>
        </uc:ContentBox>

        <div class="stats-summary">
            <div class="stats-item"><span class="stats-label"><%= Localization.GetHtml("BusEdit_MaintenanceCount") %></span> <asp:Literal ID="litMaintenanceCount" runat="server" Text="0" /></div>
            <div class="stats-item"><span class="stats-label"><%= Localization.GetHtml("BusEdit_TotalMaintenanceCost") %></span> <asp:Literal ID="litTotalMaintenanceCost" runat="server" Text="0.00" /></div>
            <div class="stats-item"><span class="stats-label"><%= Localization.GetHtml("BusMaintenance_LastService") %></span> <asp:Literal ID="litLastService" runat="server" Text="—" /></div>
            <div class="stats-item"><span class="stats-label"><%= Localization.GetHtml("BusMaintenance_NextDue") %></span> <asp:Literal ID="litNextDue" runat="server" Text="—" /></div>
        </div>

        <uc:ContentBox ID="cbMaintenanceHistory" runat="server" HeaderText="<%$ Resources:Strings, BusEdit_MaintenanceHistory %>" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <asp:GridView ID="gvMaintenanceHistory" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" AllowSorting="true" CssClass="data-table" GridLines="Both" PagerStyle-CssClass="pagination" OnPageIndexChanging="gvMaintenanceHistory_PageIndexChanging" OnSorting="gvMaintenanceHistory_Sorting" OnRowCommand="gvMaintenanceHistory_RowCommand" OnRowDataBound="gvMaintenanceHistory_RowDataBound" EmptyDataText="No maintenance records found for this bus.">
                    <Columns>
                        <asp:BoundField DataField="MaintenanceId" HeaderText="<%$ Resources:Strings, Common_ID %>" SortExpression="maintenance_id" ReadOnly="true" />
                        <asp:BoundField DataField="MaintenanceDate" HeaderText="<%$ Resources:Strings, Common_Date %>" SortExpression="maintenance_date" DataFormatString="{0:dd.MM.yyyy}" />
                        <asp:BoundField DataField="MaintenanceType" HeaderText="<%$ Resources:Strings, Common_Type %>" SortExpression="maintenance_type" />
                        <asp:BoundField DataField="MileageKm" HeaderText="<%$ Resources:Strings, Common_MileageKm %>" SortExpression="mileage_km" />
                        <asp:TemplateField HeaderText="<%$ Resources:Strings, Common_Roadworthiness %>" SortExpression="roadworthiness">
                            <ItemTemplate><asp:Literal ID="litRw" runat="server" /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Strings, Common_Cost %>" SortExpression="maintenance_cost">
                            <ItemTemplate><asp:Literal ID="litCost" runat="server" /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Strings, Common_NextService %>" SortExpression="next_maintenance_date">
                            <ItemTemplate><asp:Literal ID="litNextDate" runat="server" /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Strings, Common_Actions %>">
                            <ItemTemplate>
                                <asp:Button ID="btnViewRecord" runat="server" Text="<%$ Resources:Strings, Common_View %>" CommandName="ViewRecord" CommandArgument='<%# Eval("MaintenanceId") %>' CssClass="action-button" CausesValidation="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div style="margin-top: 12px;"><asp:Button ID="btnOpenMaintenance" runat="server" Text="<%$ Resources:Strings, Maintenance_AddRecord %>" CssClass="action-button" OnClick="btnMaintenance_Click" CausesValidation="false" /></div>
            </ContentTemplate>
        </uc:ContentBox>
    </div>
</asp:Content>
