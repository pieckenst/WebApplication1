<%@ Page Title="<%$ Resources:Strings, Auto_Reports_MaintenanceReports_19 %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MaintenanceReports.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Reports.MaintenanceReports" %>
<%@ Register TagPrefix="uc" TagName="ContentBox" Src="~/Controls/ContentBox.ascx" %>
<asp:Content ID="MainContent1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .content-page { padding: 10px; font-family: Tahoma, Verdana, Arial, sans-serif; }
        .page-title { font-size: 16pt; color: #000080; font-weight: bold; }
        .page-divider { height: 2px; background-color: #000080; margin: 5px 0 15px 0; }
        .action-button { display: inline-block; padding: 6px 12px; background-color: #1447AE; color: #FFFFFF; text-decoration: none; border: 1px solid #2459C3; border-radius: 3px; margin-right: 8px; margin-bottom: 5px; font-family: Tahoma, Arial, sans-serif; font-size: 9pt; }
        .filter-section { background-color: #F2F2F2; border: 1px solid #CCCCCC; padding: 10px; margin-bottom: 15px; }
        .filter-row { margin-bottom: 8px; }
        .filter-label { display: inline-block; width: 150px; font-weight: bold; font-size: 9pt; }
        .form-control { padding: 4px 8px; border: 1px solid #1447AE; font-family: Tahoma, Arial, sans-serif; font-size: 9pt; width: 220px; }
        .stats-summary { background-color: #EDF2FB; border: 1px solid #1447AE; padding: 10px; margin-bottom: 15px; font-size: 9pt; }
        .stats-item { display: inline-block; margin-right: 20px; }
        .stats-label { font-weight: bold; color: #1447AE; }
        .data-table { width: 100%; border-collapse: collapse; margin-top: 10px; font-size: 9pt; }
        .data-table th { background-color: #1447AE; color: #FFFFFF; padding: 8px; text-align: left; border: 1px solid #0A2E7A; }
        .data-table td { padding: 8px; border: 1px solid #CCCCCC; background-color: #FFFFFF; }
        .pagination { margin-top: 15px; text-align: right; }
        .error-message { background-color: #FFE6E6; border: 1px solid #CC0000; color: #CC0000; padding: 10px; margin-bottom: 15px; font-size: 10pt; }
        .data-table { display: block; overflow-x: auto; white-space: nowrap; }
        @media (max-width: 760px) { .content-page { padding: 8px; } .page-title { font-size: 14pt; } .filter-label { display: block; width: auto; margin: 0 0 4px; } .form-control, select, input[type='date'] { width: 100%; max-width: 100%; box-sizing: border-box; } .action-bar { display: flex; flex-wrap: wrap; gap: 6px; } }
    </style>
    <div class="content-page">
        <div class="page-title"><%= Localization.GetHtml("Auto_Reports_MaintenanceReports_20") %></div>
        <div class="page-divider"></div>
        <asp:Label ID="lblError" runat="server" CssClass="error-message" Visible="false" />
        <uc:ContentBox ID="cbMaintenanceFilters" runat="server" HeaderText="Filter Options" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <div class="filter-section">
                    <div class="filter-row"><span class="filter-label"><%= Localization.GetHtml("Auto_Reports_MaintenanceReports_21") %></span><asp:TextBox ID="txtDateFrom" runat="server" TextMode="Date" CssClass="form-control" /></div>
                    <div class="filter-row"><span class="filter-label"><%= Localization.GetHtml("Auto_Reports_MaintenanceReports_22") %></span><asp:TextBox ID="txtDateTo" runat="server" TextMode="Date" CssClass="form-control" /></div>
                    <div class="filter-row"><span class="filter-label"><%= Localization.GetHtml("Auto_Reports_MaintenanceReports_23") %></span><asp:DropDownList ID="ddlBus" runat="server" CssClass="form-control"><asp:ListItem Text="<%$ Resources:Strings, Auto_Reports_MaintenanceReports_1 %>" Value="" /></asp:DropDownList></div>
                    <div class="filter-row"><span class="filter-label"><%= Localization.GetHtml("Auto_Reports_MaintenanceReports_24") %></span><asp:DropDownList ID="ddlRoadworthiness" runat="server" CssClass="form-control"><asp:ListItem Text="<%$ Resources:Strings, Auto_Reports_MaintenanceReports_2 %>" Value="" /><asp:ListItem Text="<%$ Resources:Strings, Auto_Reports_MaintenanceReports_3 %>" Value="Исправен" /><asp:ListItem Text="<%$ Resources:Strings, Auto_Reports_MaintenanceReports_4 %>" Value="Требует внимания" /><asp:ListItem Text="<%$ Resources:Strings, Auto_Reports_MaintenanceReports_5 %>" Value="Неисправен" /></asp:DropDownList></div>
                    <asp:Button ID="btnApply" runat="server" Text="<%$ Resources:Strings, Auto_Reports_MaintenanceReports_6 %>" CssClass="action-button" OnClick="btnApply_Click" />
                    <asp:Button ID="btnExport" runat="server" Text="<%$ Resources:Strings, Auto_Reports_MaintenanceReports_7 %>" CssClass="action-button" OnClick="btnExport_Click" CausesValidation="false" />
                </div>
            </ContentTemplate>
        </uc:ContentBox>
        <div class="stats-summary">
            <div class="stats-item"><span class="stats-label"><%= Localization.GetHtml("Auto_Reports_MaintenanceReports_25") %></span> <asp:Literal ID="litRecordCount" runat="server" Text="0" /></div>
            <div class="stats-item"><span class="stats-label"><%= Localization.GetHtml("Auto_Reports_MaintenanceReports_26") %></span> <asp:Literal ID="litTotalCost" runat="server" Text="0.00" /></div>
            <div class="stats-item"><span class="stats-label"><%= Localization.GetHtml("Auto_Reports_MaintenanceReports_27") %></span> <asp:Literal ID="litNotRoadworthy" runat="server" Text="0" /></div>
            <div class="stats-item"><span class="stats-label"><%= Localization.GetHtml("Auto_Reports_MaintenanceReports_28") %></span> <asp:Literal ID="litUpcoming" runat="server" Text="0" /></div>
        </div>
        <uc:ContentBox ID="cbMaintenanceList" runat="server" HeaderText="Maintenance Analysis" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <asp:GridView ID="gvMaintenance" runat="server" AutoGenerateColumns="false" AllowPaging="false" CssClass="data-table" GridLines="Both" EmptyDataText="<%$ Resources:Strings, Auto_Reports_MaintenanceReports_8 %>">
                    <Columns>
                        <asp:BoundField DataField="MaintenanceDate" HeaderText="<%$ Resources:Strings, Auto_Reports_MaintenanceReports_9 %>" DataFormatString="{0:dd.MM.yyyy}" />
                        <asp:BoundField DataField="FleetNumber" HeaderText="<%$ Resources:Strings, Auto_Reports_MaintenanceReports_10 %>" />
                        <asp:BoundField DataField="BusModel" HeaderText="<%$ Resources:Strings, Auto_Reports_MaintenanceReports_11 %>" />
                        <asp:BoundField DataField="MaintenanceType" HeaderText="<%$ Resources:Strings, Auto_Reports_MaintenanceReports_12 %>" />
                        <asp:BoundField DataField="Roadworthiness" HeaderText="<%$ Resources:Strings, Auto_Reports_MaintenanceReports_13 %>" />
                        <asp:BoundField DataField="MaintenanceCost" HeaderText="<%$ Resources:Strings, Auto_Reports_MaintenanceReports_14 %>" DataFormatString="{0:N2}" />
                        <asp:BoundField DataField="NextMaintenanceDate" HeaderText="<%$ Resources:Strings, Auto_Reports_MaintenanceReports_15 %>" DataFormatString="{0:dd.MM.yyyy}" />
                        <asp:BoundField DataField="EmployeeName" HeaderText="<%$ Resources:Strings, Auto_Reports_MaintenanceReports_16 %>" />
                    </Columns>
                </asp:GridView>
                <div class="pagination">
                    <asp:LinkButton ID="btnPreviousPage" runat="server" Text="<%$ Resources:Strings, Auto_Reports_MaintenanceReports_17 %>" OnClick="btnPreviousPage_Click" CausesValidation="false" />
                    <asp:Label ID="lblPageInfo" runat="server" />
                    <asp:LinkButton ID="btnNextPage" runat="server" Text="<%$ Resources:Strings, Auto_Reports_MaintenanceReports_18 %>" OnClick="btnNextPage_Click" CausesValidation="false" />
                </div>
            </ContentTemplate>
        </uc:ContentBox>
    </div>
</asp:Content>
