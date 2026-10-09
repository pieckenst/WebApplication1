<%@ Page Title="<%$ Resources:Strings, Auto_Fleet_Buses_1 %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BusEdit.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Fleet.BusEdit" %>
<%@ Register TagPrefix="uc" TagName="ContentBox" Src="~/Controls/ContentBox.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        .content-page { padding: 18px; font-family: Tahoma, Verdana, Arial, sans-serif; max-width: 1200px; margin: 0 auto; }
        .content-page > * + * { margin-top: 16px; }
        .page-title { font-size: 16pt; color: #000080; font-weight: bold; }
        .page-divider { height: 2px; background-color: #000080; margin: 5px 0 15px 0; }
        .text-regular { font-size: 10pt; line-height: 1.5; }
        .text-small { font-size: 8pt; color: #666666; }
        .action-bar { background-color: #EDF2FB; border: 1px solid #1447AE; padding: 12px; margin-bottom: 18px; display: flex; flex-wrap: wrap; align-items: center; gap: 8px; }
        .action-button { display: inline-block; padding: 7px 12px; background-color: #1447AE; color: #FFFFFF; text-decoration: none; border: 1px solid #2459C3; border-radius: 3px; margin: 0; font-family: Tahoma, Arial, sans-serif; font-size: 9pt; min-height: 30px; box-sizing: border-box; cursor: pointer; }
        .action-button:hover { background-color: #2459C3; }
        .action-button.danger { background-color: #CC0000; border-color: #990000; }
        .action-button.danger:hover { background-color: #990000; }
        .action-button.secondary { background-color: #666666; border-color: #444444; }
        .action-button.secondary:hover { background-color: #444444; }
        .breadcrumb { font-size: 9pt; color: #666666; margin-bottom: 6px; }
        .breadcrumb a { color: #1447AE; text-decoration: none; }
        .breadcrumb a:hover { text-decoration: underline; }
        .breadcrumb .separator { margin: 0 4px; color: #999999; }
        .form-section { margin-bottom: 20px; }
        .form-row { margin-bottom: 16px; display: flex; flex-wrap: wrap; align-items: flex-start; gap: 8px 12px; }
        .form-row:last-child { margin-bottom: 0; }
        .form-label { display: inline-block; width: 180px; padding-top: 6px; font-weight: bold; font-size: 9pt; }
        .form-control { padding: 4px 8px; border: 1px solid #1447AE; font-family: Tahoma, Arial, sans-serif; font-size: 9pt; width: 300px; box-sizing: border-box; }
        .form-control.small { width: 120px; }
        .form-control.medium { width: 200px; }
        .form-required { color: #CC0000; }
        .form-hint { font-size: 8pt; color: #666666; padding-top: 6px; }
        .form-error { font-size: 8pt; color: #CC0000; padding-top: 4px; }
        .error-message { background-color: #FFE6E6; border: 1px solid #CC0000; color: #CC0000; padding: 10px; margin-bottom: 15px; font-size: 10pt; }
        .success-message { background-color: #E6FFE6; border: 1px solid #008000; color: #008000; padding: 10px; margin-bottom: 15px; font-size: 10pt; }
        .warning-message { background-color: #FFF3CD; border: 1px solid #FFCC00; color: #856404; padding: 10px; margin-bottom: 15px; font-size: 10pt; }
        .data-table { width: 100%; border-collapse: collapse; margin-top: 10px; font-size: 9pt; display: block; overflow-x: auto; white-space: nowrap; }
        .data-table th { background-color: #1447AE; color: #FFFFFF; padding: 8px; text-align: left; border: 1px solid #0A2E7A; }
        .data-table td { padding: 8px; border: 1px solid #CCCCCC; background-color: #FFFFFF; }
        .data-table tr:hover td { background-color: #EDF2FB; }
        .status-operational { color: #008000; font-weight: bold; }
        .status-repair { color: #FF6600; font-weight: bold; }
        .status-retired { color: #CC0000; font-weight: bold; }
        .status-reserve { color: #666666; font-weight: bold; }
        .rw-operational { color: #008000; font-weight: bold; }
        .rw-attention { color: #FF6600; font-weight: bold; }
        .rw-notoperational { color: #CC0000; font-weight: bold; }
        .overdue { color: #CC0000; font-weight: bold; }
        .stats-summary { background-color: #EDF2FB; border: 1px solid #1447AE; padding: 10px; margin-bottom: 15px; font-size: 9pt; }
        .stats-item { display: inline-block; margin-right: 20px; }
        .stats-label { font-weight: bold; color: #1447AE; }
        .detail-section { margin-bottom: 15px; }
        .detail-row { margin-bottom: 6px; font-size: 9pt; }
        .detail-label { display: inline-block; width: 180px; font-weight: bold; color: #666666; }
        .detail-value { color: #000000; }
        .empty-state { text-align: center; padding: 30px; color: #666666; font-size: 10pt; }
        .field-validation-group { margin-top: 4px; }
        .checkbox-row { display: flex; align-items: center; gap: 8px; }
        .checkbox-row input[type="checkbox"] { margin: 0; }
        .checkbox-label { font-size: 9pt; font-weight: normal; }
        @media (max-width: 760px) {
            .content-page { padding: 8px; }
            .page-title { font-size: 14pt; }
            .form-label { display: block; width: auto; margin: 0 0 4px; }
            .form-control, .form-control.small, .form-control.medium, select, input[type='text'], textarea { width: 100%; max-width: 100%; box-sizing: border-box; }
            .action-bar { display: flex; flex-wrap: wrap; gap: 6px; }
            .action-button { margin: 0; }
        }
    </style>

    <div class="content-page">
        <a name="top"></a>

        <div class="breadcrumb">
            <a href='<%= ResolveUrl("~/Default.aspx") %>'>Home</a>
            <span class="separator">/</span>
            <a href='<%= ResolveUrl("~/Fleet/Buses.aspx") %>'><%= Localization.GetHtml("Nav_BusFleet") %></a>
            <span class="separator">/</span>
            <span><asp:Literal ID="litBreadcrumb" runat="server" /></span>
        </div>

        <div class="page-title"><asp:Literal ID="litPageTitle" runat="server" /></div>
        <div class="page-divider"></div>

        <asp:Panel ID="pnlError" runat="server" CssClass="error-message" Visible="false">
            <asp:Literal ID="litError" runat="server" />
        </asp:Panel>

        <asp:Panel ID="pnlSuccess" runat="server" CssClass="success-message" Visible="false">
            <asp:Literal ID="litSuccess" runat="server" />
        </asp:Panel>

        <asp:Panel ID="pnlWarning" runat="server" CssClass="warning-message" Visible="false">
            <asp:Literal ID="litWarning" runat="server" />
        </asp:Panel>

        <!-- Action Bar -->
        <div class="action-bar">
            <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:Strings, Nav_BusFleet %>" CssClass="action-button secondary" OnClick="btnBack_Click" CausesValidation="false" />
            <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Strings, Common_Save %>" CssClass="action-button" OnClick="btnSave_Click" ValidationGroup="BusEditForm" />
            <asp:Button ID="btnSaveAndNew" runat="server" Text="<%$ Resources:Strings, Common_Save %>" CssClass="action-button" OnClick="btnSaveAndNew_Click" ValidationGroup="BusEditForm" Visible="false" />
            <asp:Button ID="btnDelete" runat="server" Text="<%$ Resources:Strings, Common_Delete %>" CssClass="action-button danger" OnClick="btnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this bus? This action cannot be undone.');" Visible="false" CausesValidation="false" />
        </div>

        <!-- Bus Edit Form -->
        <uc:ContentBox ID="cbBusForm" runat="server" HeaderText="<%$ Resources:Strings, Buses_Heading %>" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <asp:Panel ID="pnlBusForm" runat="server" DefaultButton="btnSave">

                    <!-- Bus ID (read-only, edit mode only) -->
                    <div class="form-row" id="rowBusId" runat="server" visible="false">
                        <span class="form-label"><%= Localization.GetHtml("Common_ID") %></span>
                        <asp:Label ID="lblBusId" runat="server" CssClass="detail-value" />
                    </div>

                    <!-- Fleet Number -->
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Common_FleetNumber") %> <span class="form-required">*</span></span>
                        <span>
                            <asp:TextBox ID="txtFleetNumber" runat="server" CssClass="form-control medium" MaxLength="20" />
                            <asp:RequiredFieldValidator ID="rfvFleetNumber" runat="server" ControlToValidate="txtFleetNumber" ErrorMessage="<%$ Resources:Strings, Common_Required %>" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="BusEditForm" />
                        </span>
                        <span class="form-hint">Max 20 characters</span>
                    </div>

                    <!-- Registration Number -->
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Common_Registration") %> <span class="form-required">*</span></span>
                        <span>
                            <asp:TextBox ID="txtRegistrationNum" runat="server" CssClass="form-control medium" MaxLength="20" />
                            <asp:RequiredFieldValidator ID="rfvRegistrationNum" runat="server" ControlToValidate="txtRegistrationNum" ErrorMessage="<%$ Resources:Strings, Common_Required %>" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="BusEditForm" />
                        </span>
                        <span class="form-hint">Max 20 characters</span>
                    </div>

                    <!-- Manufacturer -->
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Common_Manufacturer") %> <span class="form-required">*</span></span>
                        <span>
                            <asp:TextBox ID="txtManufacturer" runat="server" CssClass="form-control" MaxLength="80" />
                            <asp:RequiredFieldValidator ID="rfvManufacturer" runat="server" ControlToValidate="txtManufacturer" ErrorMessage="<%$ Resources:Strings, Common_Required %>" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="BusEditForm" />
                        </span>
                        <span class="form-hint">Max 80 characters</span>
                    </div>

                    <!-- Model -->
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Common_Model") %> <span class="form-required">*</span></span>
                        <span>
                            <asp:TextBox ID="txtModel" runat="server" CssClass="form-control" MaxLength="80" />
                            <asp:RequiredFieldValidator ID="rfvModel" runat="server" ControlToValidate="txtModel" ErrorMessage="<%$ Resources:Strings, Common_Required %>" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="BusEditForm" />
                        </span>
                        <span class="form-hint">Max 80 characters</span>
                    </div>

                    <!-- Manufacture Year -->
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Common_Year") %> <span class="form-required">*</span></span>
                        <span>
                            <asp:TextBox ID="txtManufactureYear" runat="server" CssClass="form-control small" MaxLength="4" />
                            <asp:RequiredFieldValidator ID="rfvManufactureYear" runat="server" ControlToValidate="txtManufactureYear" ErrorMessage="<%$ Resources:Strings, Common_Required %>" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="BusEditForm" />
                            <asp:RangeValidator ID="rvManufactureYear" runat="server" ControlToValidate="txtManufactureYear" MinimumValue="1980" MaximumValue="2100" Type="Integer" ErrorMessage="Year must be 1980-2100" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="BusEditForm" />
                        </span>
                        <span class="form-hint">Between 1980 and 2100</span>
                    </div>

                    <!-- Capacity -->
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Common_Capacity") %> <span class="form-required">*</span></span>
                        <span>
                            <asp:TextBox ID="txtCapacity" runat="server" CssClass="form-control small" MaxLength="5" />
                            <asp:RequiredFieldValidator ID="rfvCapacity" runat="server" ControlToValidate="txtCapacity" ErrorMessage="<%$ Resources:Strings, Common_Required %>" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="BusEditForm" />
                            <asp:RangeValidator ID="rvCapacity" runat="server" ControlToValidate="txtCapacity" MinimumValue="1" MaximumValue="99999" Type="Integer" ErrorMessage="Must be 1-99999" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="BusEditForm" />
                        </span>
                        <span class="form-hint">Number of seats</span>
                    </div>

                    <!-- Status -->
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Common_Status") %> <span class="form-required">*</span></span>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control medium">
                            <asp:ListItem Text="<%$ Resources:Strings, Common_Operational %>" Value="Исправен" />
                            <asp:ListItem Text="<%$ Resources:Strings, Common_InRepair %>" Value="На ремонте" />
                            <asp:ListItem Text="<%$ Resources:Strings, Common_Retired %>" Value="Списан" />
                            <asp:ListItem Text="<%$ Resources:Strings, Common_Reserve %>" Value="Резерв" />
                        </asp:DropDownList>
                        <span class="form-hint">Cannot change retired bus directly to operational</span>
                    </div>

                    <!-- Mileage -->
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Common_MileageKm") %></span>
                        <span>
                            <asp:TextBox ID="txtMileageKm" runat="server" CssClass="form-control small" MaxLength="10" Text="0" />
                            <asp:RangeValidator ID="rvMileageKm" runat="server" ControlToValidate="txtMileageKm" MinimumValue="0" MaximumValue="9999999" Type="Integer" ErrorMessage="Must be 0-9999999" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="BusEditForm" />
                        </span>
                        <span class="form-hint">Kilometers</span>
                    </div>

                    <!-- Current Mileage Category (read-only, computed) -->
                    <div class="form-row" id="rowMileageCategory" runat="server" visible="false">
                        <span class="form-label"><%= Localization.GetHtml("Buses_MileageCategory") %></span>
                        <asp:Label ID="lblMileageCategory" runat="server" CssClass="detail-value" />
                    </div>

                </asp:Panel>
            </ContentTemplate>
        </uc:ContentBox>

        <!-- Bus Summary (edit mode only) -->
        <uc:ContentBox ID="cbBusSummary" runat="server" HeaderText="Bus Summary" HeaderColor="Gray" ContentColor="Gray" Visible="false">
            <ContentTemplate>
                <div class="stats-summary">
                    <div class="stats-item">
                        <span class="stats-label"><%= Localization.GetHtml("Common_Status") %>:</span>
                        <asp:Literal ID="litSummaryStatus" runat="server" />
                    </div>
                    <div class="stats-item">
                        <span class="stats-label"><%= Localization.GetHtml("Common_MileageKm") %>:</span>
                        <asp:Literal ID="litSummaryMileage" runat="server" />
                    </div>
                    <div class="stats-item">
                        <span class="stats-label"><%= Localization.GetHtml("Buses_MileageCategory") %>:</span>
                        <asp:Literal ID="litSummaryMileageCategory" runat="server" />
                    </div>
                    <div class="stats-item">
                        <span class="stats-label"><%= Localization.GetHtml("Nav_Maintenance") %>:</span>
                        <asp:Literal ID="litSummaryMaintenanceCount" runat="server" />
                    </div>
                    <div class="stats-item">
                        <span class="stats-label">Total Maintenance Cost:</span>
                        <asp:Literal ID="litSummaryTotalCost" runat="server" />
                    </div>
                </div>
            </ContentTemplate>
        </uc:ContentBox>

        <!-- Maintenance History (edit mode only) -->
        <uc:ContentBox ID="cbMaintenanceHistory" runat="server" HeaderText="<%$ Resources:Strings, Nav_Maintenance %>" HeaderColor="Blue" ContentColor="White" Visible="false">
            <ContentTemplate>
                <asp:Literal ID="litBusIdHidden" runat="server" Visible="false" />
                <asp:GridView
                    ID="gvMaintenanceHistory"
                    runat="server"
                    AutoGenerateColumns="false"
                    AllowPaging="true"
                    PageSize="10"
                    AllowSorting="true"
                    CssClass="data-table"
                    GridLines="Both"
                    PagerStyle-CssClass="pagination"
                    OnPageIndexChanging="gvMaintenanceHistory_PageIndexChanging"
                    OnSorting="gvMaintenanceHistory_Sorting"
                    OnRowDataBound="gvMaintenanceHistory_RowDataBound"
                    EmptyDataText="No maintenance records found for this bus.">

                    <Columns>
                        <asp:BoundField DataField="MaintenanceId" HeaderText="<%$ Resources:Strings, Common_ID %>" SortExpression="maintenance_id" ReadOnly="true" />
                        <asp:BoundField DataField="MaintenanceDate" HeaderText="<%$ Resources:Strings, Common_Date %>" SortExpression="maintenance_date" DataFormatString="{0:dd.MM.yyyy}" />
                        <asp:BoundField DataField="MaintenanceType" HeaderText="<%$ Resources:Strings, Common_Type %>" SortExpression="maintenance_type" />
                        <asp:BoundField DataField="MileageKm" HeaderText="<%$ Resources:Strings, Common_MileageKm %>" SortExpression="mileage_km" />
                        <asp:TemplateField HeaderText="<%$ Resources:Strings, Common_Roadworthiness %>" SortExpression="roadworthiness">
                            <ItemTemplate>
                                <asp:Literal ID="litHistoryRw" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Strings, Common_Cost %>" SortExpression="maintenance_cost">
                            <ItemTemplate>
                                <asp:Literal ID="litHistoryCost" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="NextMaintenanceDate" HeaderText="<%$ Resources:Strings, Common_NextService %>" SortExpression="next_maintenance_date" DataFormatString="{0:dd.MM.yyyy}" />
                        <asp:BoundField DataField="DaysFromLastService" HeaderText="<%$ Resources:Strings, Maintenance_DaysAgo %>" SortExpression="days_from_last_service" />
                        <asp:TemplateField HeaderText="<%$ Resources:Strings, Common_Actions %>">
                            <ItemTemplate>
                                <asp:Button ID="btnViewRecord" runat="server" Text="<%$ Resources:Strings, Common_View %>" CommandName="ViewRecord" CommandArgument='<%# Eval("MaintenanceId") %>' CssClass="action-button" OnClick="btnViewRecord_Click" />
                                <asp:Button ID="btnEditRecord" runat="server" Text="<%$ Resources:Strings, Common_Edit %>" CommandName="EditRecord" CommandArgument='<%# Eval("MaintenanceId") %>' CssClass="action-button" OnClick="btnEditRecord_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>

                <div style="margin-top: 12px;">
                    <asp:Button ID="btnAddMaintenance" runat="server" Text="<%$ Resources:Strings, Maintenance_AddRecord %>" CssClass="action-button" OnClick="btnAddMaintenance_Click" CausesValidation="false" />
                </div>
            </ContentTemplate>
        </uc:ContentBox>

        <p align="right" style="margin-top: 20px;"><a href="#top" style="font-size: 8pt; color: #000000;"><%= Localization.GetHtml("Common_BackToTop") %></a></p>
    </div>

</asp:Content>
