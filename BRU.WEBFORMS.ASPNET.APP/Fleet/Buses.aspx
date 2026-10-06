<%@ Page Title="<%$ Resources:Strings, Auto_Fleet_Buses_1 %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Buses.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Fleet.Buses" %>
<%@ Register TagPrefix="uc" TagName="ContentBox" Src="~/Controls/ContentBox.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <style type="text/css">
        .content-page {
            padding: 10px;
            font-family: Tahoma, Verdana, Arial, sans-serif;
        }
        .page-title { 
            font-size: 16pt; 
            color: #000080; 
            font-weight: bold; 
        }
        .page-divider { 
            height: 2px; 
            background-color: #000080; 
            margin: 5px 0 15px 0; 
        }
        .text-regular { 
            font-size: 10pt; 
            line-height: 1.5; 
        }
        .text-small { 
            font-size: 8pt; 
            color: #666666; 
        }
        .action-bar {
            background-color: #EDF2FB;
            border: 1px solid #1447AE;
            padding: 10px;
            margin-bottom: 15px;
        }
        .action-button {
            display: inline-block;
            padding: 6px 12px;
            background-color: #1447AE;
            color: #FFFFFF;
            text-decoration: none;
            border: 1px solid #2459C3;
            border-radius: 3px;
            margin-right: 8px;
            margin-bottom: 5px;
            font-family: Tahoma, Arial, sans-serif;
            font-size: 9pt;
        }
        .action-button:hover {
            background-color: #2459C3;
        }
        .search-box {
            display: inline-block;
            margin-left: 20px;
        }
        .search-input {
            padding: 4px 8px;
            border: 1px solid #1447AE;
            font-family: Tahoma, Arial, sans-serif;
            font-size: 9pt;
        }
        .filter-section {
            background-color: #F2F2F2;
            border: 1px solid #CCCCCC;
            padding: 10px;
            margin-bottom: 15px;
        }
        .filter-row {
            margin-bottom: 8px;
        }
        .filter-label {
            display: inline-block;
            width: 120px;
            font-weight: bold;
            font-size: 9pt;
        }
        .filter-control {
            display: inline-block;
        }
        .data-table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 10px;
            font-size: 9pt;
        }
        .data-table th {
            background-color: #1447AE;
            color: #FFFFFF;
            padding: 8px;
            text-align: left;
            border: 1px solid #0A2E7A;
        }
        .data-table td {
            padding: 8px;
            border: 1px solid #CCCCCC;
            background-color: #FFFFFF;
        }
        .data-table tr:hover td {
            background-color: #EDF2FB;
        }
        .status-operational { color: #008000; font-weight: bold; }
        .status-repair { color: #FF6600; font-weight: bold; }
        .status-retired { color: #CC0000; font-weight: bold; }
        .status-reserve { color: #666666; font-weight: bold; }
        .mileage-high { color: #FF6600; }
        .mileage-medium { color: #008000; }
        .mileage-low { color: #0066CC; }
        .pagination {
            margin-top: 15px;
            text-align: right;
        }
        .pagination a {
            padding: 4px 8px;
            margin: 0 2px;
            background-color: #EDF2FB;
            border: 1px solid #1447AE;
            text-decoration: none;
            font-size: 9pt;
        }
        .pagination a:hover {
            background-color: #1447AE;
            color: #FFFFFF;
        }
        .pagination .current {
            padding: 4px 8px;
            margin: 0 2px;
            background-color: #1447AE;
            color: #FFFFFF;
            font-weight: bold;
        }
        .modal {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0,0,0,0.5);
            z-index: 1000;
        }
        .modal-content {
            background-color: #FFFFFF;
            margin: 10% auto;
            padding: 20px;
            width: 600px;
            border: 2px solid #1447AE;
            border-radius: 5px;
        }
        .modal-header {
            background-color: #1447AE;
            color: #FFFFFF;
            padding: 10px;
            font-weight: bold;
            margin: -20px -20px 20px -20px;
            border-radius: 3px 3px 0 0;
        }
        .modal-body {
            padding: 15px 0;
        }
        .modal-footer {
            padding: 15px 0 0 0;
            text-align: right;
        }
        .form-row {
            margin-bottom: 12px;
        }
        .form-label {
            display: inline-block;
            width: 150px;
            font-weight: bold;
            font-size: 9pt;
        }
        .form-control {
            padding: 4px 8px;
            border: 1px solid #1447AE;
            font-family: Tahoma, Arial, sans-serif;
            font-size: 9pt;
            width: 300px;
        }
        .form-required {
            color: #CC0000;
        }
        .error-message {
            background-color: #FFE6E6;
            border: 1px solid #CC0000;
            color: #CC0000;
            padding: 10px;
            margin-bottom: 15px;
            font-size: 10pt;
        }
        .success-message {
            background-color: #E6FFE6;
            border: 1px solid #008000;
            color: #008000;
            padding: 10px;
            margin-bottom: 15px;
            font-size: 10pt;
        }
        .stats-summary {
            background-color: #EDF2FB;
            border: 1px solid #1447AE;
            padding: 10px;
            margin-bottom: 15px;
            font-size: 9pt;
        }
        .stats-item {
            display: inline-block;
            margin-right: 20px;
        }
        .stats-label {
            font-weight: bold;
            color: #1447AE;
        }
    </style>
    
    <div class="content-page">
        <a name="top"></a>
        <div class="page-title"><%= Localization.GetHtml("Buses_Heading") %></div>
        <div class="page-divider"></div>
        
        <asp:Panel ID="pnlError" runat="server" CssClass="error-message" Visible="false">
            <asp:Literal ID="litError" runat="server" />
        </asp:Panel>
        
        <asp:Panel ID="pnlSuccess" runat="server" CssClass="success-message" Visible="false">
            <asp:Literal ID="litSuccess" runat="server" />
        </asp:Panel>
        
        <!-- Statistics Summary -->
        <div class="stats-summary">
            <div class="stats-item">
                <span class="stats-label"><%= Localization.GetHtml("Buses_Total") %></span>
                <asp:Literal ID="litTotalBuses" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label"><%= Localization.GetHtml("Buses_Operational") %></span>
                <asp:Literal ID="litOperationalBuses" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label"><%= Localization.GetHtml("Buses_InRepair") %></span>
                <asp:Literal ID="litRepairBuses" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label"><%= Localization.GetHtml("Buses_NeedAttention") %></span>
                <asp:Literal ID="litAttentionBuses" runat="server" Text="0" />
            </div>
        </div>
        
        <!-- Action Bar -->
        <div class="action-bar">
            <asp:Button ID="btnAddBus" runat="server" Text="<%$ Resources:Strings, Buses_Add %>" CssClass="action-button" OnClick="btnAddBus_Click" />
            <asp:Button ID="btnRefresh" runat="server" Text="<%$ Resources:Strings, Common_Refresh %>" CssClass="action-button" OnClick="btnRefresh_Click" />
            <asp:Button ID="btnExport" runat="server" Text="<%$ Resources:Strings, Common_ExportExcel %>" CssClass="action-button" OnClick="btnExport_Click" />
            
            <div class="search-box">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="search-input" Placeholder="<%$ Resources:Strings, Buses_SearchPlaceholder %>" />
                <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Strings, Common_Search %>" CssClass="action-button" OnClick="btnSearch_Click" />
            </div>
        </div>
        
        <!-- Filter Section -->
      <!-- Filter Section -->
<uc:ContentBox
    ID="cbFilters"
    runat="server"
    HeaderText="<%$ Resources:Strings, Buses_FilterOptions %>"
    HeaderColor="Blue"
    ContentColor="White">

    <ContentTemplate>

        <div class="filter-section">

            <!-- Status -->
            <div class="filter-row">
                <span class="filter-label"><%= Localization.GetHtml("Buses_StatusFilter") %></span>

                <span class="filter-control">
                    <asp:DropDownList
                        ID="ddlStatusFilter"
                        runat="server"
                        CssClass="form-control"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlStatusFilter_SelectedIndexChanged">

                        <asp:ListItem
                            Text="<%$ Resources:Strings, Common_AllStatuses %>"
                            Value="" />

                        <asp:ListItem
                            Text="<%$ Resources:Strings, Common_Operational %>"
                            Value="Исправен" />

                        <asp:ListItem
                            Text="<%$ Resources:Strings, Common_InRepair %>"
                            Value="На ремонте" />

                        <asp:ListItem
                            Text="<%$ Resources:Strings, Common_Retired %>"
                            Value="Списан" />

                        <asp:ListItem
                            Text="<%$ Resources:Strings, Common_Reserve %>"
                            Value="Резерв" />

                    </asp:DropDownList>
                </span>
            </div>

            <!-- Manufacturer -->
            <div class="filter-row">
                <span class="filter-label"><%= Localization.GetHtml("Buses_ManufacturerFilter") %></span>

                <span class="filter-control">
                    <asp:DropDownList
                        ID="ddlManufacturerFilter"
                        runat="server"
                        CssClass="form-control"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlManufacturerFilter_SelectedIndexChanged">

                        <asp:ListItem
                            Text="<%$ Resources:Strings, Buses_AllManufacturers %>"
                            Value="" />

                    </asp:DropDownList>
                </span>
            </div>

            <!-- Year Range -->
            <div class="filter-row">
                <span class="filter-label"><%= Localization.GetHtml("Buses_YearRange") %></span>

                <span class="filter-control">

                    <asp:TextBox
                        ID="txtYearFrom"
                        runat="server"
                        CssClass="form-control"
                        Width="80"
                        Placeholder="<%$ Resources:Strings, Common_From %>" />

                    <asp:TextBox
                        ID="txtYearTo"
                        runat="server"
                        CssClass="form-control"
                        Width="80"
                        Placeholder="<%$ Resources:Strings, Common_To %>" />

                    <asp:Button
                        ID="btnApplyYearFilter"
                        runat="server"
                        Text="<%$ Resources:Strings, Common_Apply %>"
                        CssClass="action-button"
                        OnClick="btnApplyYearFilter_Click" />

                </span>
            </div>

        </div>

    </ContentTemplate>

</uc:ContentBox>
       
       <!-- Buses Grid -->
<uc:ContentBox
    ID="cbBusesList"
    runat="server"
    HeaderText="<%$ Resources:Strings, Nav_BusFleet %>"
    HeaderColor="Blue"
    ContentColor="White">

    <ContentTemplate>

        <asp:GridView
            ID="gvBuses"
            runat="server"
            AutoGenerateColumns="false"
            AllowPaging="true"
            PageSize="20"
            AllowSorting="true"
            CssClass="data-table"
            GridLines="Both"
            PagerStyle-CssClass="pagination"
            OnPageIndexChanging="gvBuses_PageIndexChanging"
            OnSorting="gvBuses_Sorting"
            OnRowCommand="gvBuses_RowCommand"
            OnRowDataBound="gvBuses_RowDataBound">

           <Columns>

    <asp:BoundField
        DataField="BusId"
        HeaderText="<%$ Resources:Strings, Common_ID %>"
        SortExpression="bus_id"
        ReadOnly="true" />

    <asp:BoundField
        DataField="FleetNumber"
        HeaderText="<%$ Resources:Strings, Common_FleetNumber %>"
        SortExpression="fleet_number" />

    <asp:BoundField
        DataField="RegistrationNum"
        HeaderText="<%$ Resources:Strings, Common_Registration %>"
        SortExpression="registration_num" />

    <asp:BoundField
        DataField="Model"
        HeaderText="<%$ Resources:Strings, Common_Model %>"
        SortExpression="model" />

    <asp:BoundField
        DataField="Manufacturer"
        HeaderText="<%$ Resources:Strings, Common_Manufacturer %>"
        SortExpression="manufacturer" />

    <asp:BoundField
        DataField="ManufactureYear"
        HeaderText="<%$ Resources:Strings, Common_Year %>"
        SortExpression="manufacture_year" />

    <asp:BoundField
        DataField="Capacity"
        HeaderText="<%$ Resources:Strings, Common_Capacity %>"
        SortExpression="capacity" />

    <asp:TemplateField
        HeaderText="<%$ Resources:Strings, Common_Status %>"
        SortExpression="status">

        <ItemTemplate>
            <asp:Literal
                ID="litStatus"
                runat="server" />
        </ItemTemplate>

    </asp:TemplateField>

    <asp:BoundField
        DataField="MileageKm"
        HeaderText="<%$ Resources:Strings, Common_MileageKm %>"
        SortExpression="mileage_km"
        DataFormatString="{0:N0}" />

    <asp:TemplateField
        HeaderText="<%$ Resources:Strings, Buses_MileageCategory %>">

        <ItemTemplate>
            <asp:Literal
                ID="litMileageCategory"
                runat="server" />
        </ItemTemplate>

    </asp:TemplateField>

    <asp:TemplateField HeaderText="<%$ Resources:Strings, Common_Actions %>">

        <ItemTemplate>

            <asp:Button
                ID="btnView"
                runat="server"
                Text="<%$ Resources:Strings, Common_View %>"
                CommandName="View"
                CommandArgument='<%# Eval("BusId") %>'
                CssClass="action-button" />

            <asp:Button
                ID="btnEdit"
                runat="server"
                Text="<%$ Resources:Strings, Common_Edit %>"
                CommandName="Edit"
                CommandArgument='<%# Eval("BusId") %>'
                CssClass="action-button" />

            <asp:Button
                ID="btnMaintenance"
                runat="server"
                Text="<%$ Resources:Strings, Nav_Maintenance %>"
                CommandName="Maintenance"
                CommandArgument='<%# Eval("BusId") %>'
                CssClass="action-button" />

            <asp:Button
                ID="btnDelete"
                runat="server"
                Text="<%$ Resources:Strings, Common_Delete %>"
                CommandName="Delete"
                CommandArgument='<%# Eval("BusId") %>'
                CssClass="action-button"
                OnClientClick="return confirm('<%$ Resources:Strings, Buses_DeleteConfirm %>');" />

        </ItemTemplate>

    </asp:TemplateField>

</Columns>

        </asp:GridView>

    </ContentTemplate>

</uc:ContentBox>
        
        <!-- Pagination -->
        <div class="pagination">
            <asp:Literal ID="litPagination" runat="server" />
        </div>
        
        <p align="right" style="margin-top: 20px;"><a href="#top" style="font-size: 8pt; color: #000000;"><%= Localization.GetHtml("Common_BackToTop") %></a></p>
    </div>
    
</asp:Content>