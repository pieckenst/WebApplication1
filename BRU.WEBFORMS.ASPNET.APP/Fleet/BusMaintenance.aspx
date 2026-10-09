<%@ Page Title="<%$ Resources:Strings, BusMaintenance_Heading %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BusMaintenance.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Fleet.BusMaintenance" %>
<%@ Register TagPrefix="uc" TagName="ContentBox" Src="~/Controls/ContentBox.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .content-page { padding: 18px; font-family: Tahoma, Verdana, Arial, sans-serif; max-width: 1480px; margin: 0 auto; box-sizing: border-box; }
        .content-page > * + * { margin-top: 16px; }
        .page-title { font-size: 16pt; color: #000080; font-weight: bold; }
        .page-divider { height: 2px; background: #000080; margin: 5px 0 15px; }
        .breadcrumb { font-size: 9pt; color: #666; margin-bottom: 6px; }
        .breadcrumb a { color: #1447AE; text-decoration: none; }
        .breadcrumb .separator { color: #999; margin: 0 5px; }
        .action-bar { display: flex; flex-wrap: wrap; align-items: center; gap: 8px; padding: 12px; background: #EDF2FB; border: 1px solid #1447AE; }
        .action-button { display: inline-block; padding: 7px 12px; background: #1447AE; color: #fff; border: 1px solid #2459C3; border-radius: 3px; margin: 0; font-family: Tahoma, Arial, sans-serif; font-size: 9pt; cursor: pointer; text-decoration: none; }
        .action-button:hover { background: #2459C3; }
        .action-button.secondary { background: #666; border-color: #444; }
        .action-button.danger { background: #C00; border-color: #900; }
        .action-button.danger:hover { background: #900; }
        .error-message { background: #FFE6E6; border: 1px solid #C00; color: #C00; padding: 10px; margin-bottom: 15px; }
        .success-message { background: #E6FFE6; border: 1px solid #080; color: #080; padding: 10px; margin-bottom: 15px; }
        .warning-message { background: #FFF3CD; border: 1px solid #CC9A00; color: #856404; padding: 10px; margin-bottom: 15px; }
        .stats-summary { background: #EDF2FB; border: 1px solid #1447AE; padding: 10px; font-size: 9pt; }
        .stats-item { display: inline-block; margin: 4px 18px 4px 0; }
        .stats-label { font-weight: bold; color: #1447AE; }
        .form-row { display: flex; flex-wrap: wrap; align-items: flex-start; gap: 8px 12px; margin-bottom: 14px; }
        .form-row:last-child { margin-bottom: 0; }
        .form-label { display: inline-block; width: 190px; padding-top: 6px; font-weight: bold; font-size: 9pt; }
        .form-control { padding: 5px 8px; border: 1px solid #1447AE; font-family: Tahoma, Arial, sans-serif; font-size: 9pt; width: 320px; max-width: 100%; box-sizing: border-box; }
        .form-control.small { width: 160px; }
        .form-hint { display: block; padding-top: 5px; font-size: 8pt; color: #666; }
        .form-required { color: #C00; }
        .detail-row { margin: 7px 0; font-size: 9pt; }
        .detail-label { display: inline-block; min-width: 190px; color: #666; font-weight: bold; }
        .detail-value { color: #111; }
        .data-table { width: 100%; border-collapse: collapse; margin-top: 10px; font-size: 9pt; }
        .data-table th { background: #1447AE; color: #fff; padding: 8px; text-align: left; border: 1px solid #0A2E7A; }
        .data-table td { background: #fff; padding: 8px; border: 1px solid #CCC; }
        .data-table tr:hover td { background: #EDF2FB; }
        .rw-operational { color: #080; font-weight: bold; }
        .rw-attention { color: #C60; font-weight: bold; }
        .rw-notoperational { color: #C00; font-weight: bold; }
        .cost-high { color: #C00; font-weight: bold; }
        .cost-medium { color: #C60; font-weight: bold; }
        .cost-low { color: #080; }
        .overdue { color: #C00; font-weight: bold; }
        .empty-state { padding: 26px; text-align: center; color: #666; }
        .pagination { margin-top: 12px; text-align: right; }
        .validation-summary { color: #C00; margin: 8px 0; }
        @media (max-width: 760px) {
            .content-page { padding: 8px; }
            .page-title { font-size: 14pt; }
            .form-label { display: block; width: auto; padding-top: 0; }
            .form-control, .form-control.small, select, textarea, input[type='text'] { width: 100%; max-width: 100%; }
            .data-table { display: block; overflow-x: auto; white-space: nowrap; }
            .detail-label { display: block; min-width: 0; margin-bottom: 3px; }
        }
    </style>

    <div class="content-page">
        <a name="top"></a>
        <div class="breadcrumb">
            <a href='<%= ResolveUrl("~/Default.aspx") %>'>Home</a>
            <span class="separator">/</span>
            <a href='<%= ResolveUrl("~/Fleet/Buses.aspx") %>'><%= Localization.GetHtml("Nav_BusFleet") %></a>
            <span class="separator">/</span>
            <a id="lnkBusEdit" runat="server" href="~/Fleet/BusEdit.aspx"><%= Localization.GetHtml("Common_Edit") %></a>
            <span class="separator">/</span>
            <asp:Literal ID="litPageTitle" runat="server" />
        </div>

        <div class="page-title"><asp:Literal ID="litHeading" runat="server" /></div>
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

        <div class="action-bar">
            <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:Strings, Nav_BusFleet %>" CssClass="action-button secondary" OnClick="btnBack_Click" CausesValidation="false" />
            <asp:Button ID="btnEditBus" runat="server" Text="<%$ Resources:Strings, Common_Edit %>" CssClass="action-button" OnClick="btnEditBus_Click" CausesValidation="false" />
            <asp:Button ID="btnAddRecord" runat="server" Text="<%$ Resources:Strings, Maintenance_AddRecord %>" CssClass="action-button" OnClick="btnAddRecord_Click" CausesValidation="false" />
        </div>

        <uc:ContentBox ID="cbBusPicker" runat="server" HeaderText="Select a bus" HeaderColor="Blue" ContentColor="White" Visible="false">
            <ContentTemplate>
                <p>Select the bus whose maintenance history you want to manage.</p>
                <div class="form-row">
                    <span class="form-label">Bus</span>
                    <asp:DropDownList ID="ddlBusPicker" runat="server" CssClass="form-control" />
                    <asp:Button ID="btnOpenSelectedBus" runat="server" Text="Open bus" CssClass="action-button" OnClick="btnOpenSelectedBus_Click" CausesValidation="false" />
                </div>
            </ContentTemplate>
        </uc:ContentBox>

        <uc:ContentBox ID="cbBusOverview" runat="server" HeaderText="<%$ Resources:Strings, BusMaintenance_BusInfo %>" HeaderColor="Gray" ContentColor="Gray" Visible="false">
            <ContentTemplate>
                <div class="detail-row"><span class="detail-label"><%= Localization.GetHtml("Common_FleetNumber") %>:</span> <asp:Literal ID="litFleetNumber" runat="server" /></div>
                <div class="detail-row"><span class="detail-label"><%= Localization.GetHtml("Common_Registration") %>:</span> <asp:Literal ID="litRegistration" runat="server" /></div>
                <div class="detail-row"><span class="detail-label"><%= Localization.GetHtml("Common_Manufacturer") %> / <%= Localization.GetHtml("Common_Model") %>:</span> <asp:Literal ID="litBusModel" runat="server" /></div>
                <div class="detail-row"><span class="detail-label"><%= Localization.GetHtml("Common_Status") %>:</span> <asp:Literal ID="litBusStatus" runat="server" /></div>
                <div class="detail-row"><span class="detail-label"><%= Localization.GetHtml("Common_MileageKm") %>:</span> <asp:Literal ID="litBusMileage" runat="server" /></div>
            </ContentTemplate>
        </uc:ContentBox>

        <div class="stats-summary">
            <div class="stats-item"><span class="stats-label"><%= Localization.GetHtml("BusMaintenance_TotalRecords") %></span> <asp:Literal ID="litTotalRecords" runat="server" Text="0" /></div>
            <div class="stats-item"><span class="stats-label"><%= Localization.GetHtml("BusMaintenance_TotalCost") %></span> <asp:Literal ID="litTotalCost" runat="server" Text="0.00" /></div>
            <div class="stats-item"><span class="stats-label"><%= Localization.GetHtml("BusMaintenance_LastService") %></span> <asp:Literal ID="litLastService" runat="server" Text="—" /></div>
            <div class="stats-item"><span class="stats-label"><%= Localization.GetHtml("BusMaintenance_NextDue") %></span> <asp:Literal ID="litNextDue" runat="server" Text="—" /></div>
        </div>

        <uc:ContentBox ID="cbFilters" runat="server" HeaderText="Filter maintenance history" HeaderColor="Blue" ContentColor="White" Visible="false">
            <ContentTemplate>
                <div class="form-row">
                    <span class="form-label">Search records</span>
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" MaxLength="120" placeholder="Type, issue, service result..." />
                    <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Strings, Common_Search %>" CssClass="action-button" OnClick="btnSearch_Click" CausesValidation="false" />
                    <asp:Button ID="btnClearFilters" runat="server" Text="Clear filters" CssClass="action-button secondary" OnClick="btnClearFilters_Click" CausesValidation="false" />
                </div>
                <div class="form-row">
                    <span class="form-label"><%= Localization.GetHtml("Common_Roadworthiness") %></span>
                    <asp:DropDownList ID="ddlRoadworthinessFilter" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlRoadworthinessFilter_SelectedIndexChanged">
                        <asp:ListItem Text="All statuses" Value="" />
                        <asp:ListItem Text="<%$ Resources:Strings, Common_Operational %>" Value="Исправен" />
                        <asp:ListItem Text="<%$ Resources:Strings, Common_Attention %>" Value="Требует внимания" />
                        <asp:ListItem Text="<%$ Resources:Strings, Common_NotOperational %>" Value="Неисправен" />
                    </asp:DropDownList>
                </div>
            </ContentTemplate>
        </uc:ContentBox>

        <uc:ContentBox ID="cbMaintenanceList" runat="server" HeaderText="<%$ Resources:Strings, BusMaintenance_PreviousRecords %>" HeaderColor="Blue" ContentColor="White" Visible="false">
            <ContentTemplate>
                <asp:GridView ID="gvMaintenance" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="15" AllowSorting="true" CssClass="data-table" GridLines="Both" PagerStyle-CssClass="pagination" OnPageIndexChanging="gvMaintenance_PageIndexChanging" OnSorting="gvMaintenance_Sorting" OnRowCommand="gvMaintenance_RowCommand" OnRowDataBound="gvMaintenance_RowDataBound" EmptyDataText="No maintenance records match the selected filters.">
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
                        <asp:BoundField DataField="DaysFromLastService" HeaderText="<%$ Resources:Strings, Maintenance_DaysAgo %>" SortExpression="days_from_last_service" />
                        <asp:TemplateField HeaderText="<%$ Resources:Strings, Common_Actions %>">
                            <ItemTemplate>
                                <asp:Button ID="btnView" runat="server" Text="<%$ Resources:Strings, Common_View %>" CommandName="ViewRecord" CommandArgument='<%# Eval("MaintenanceId") %>' CssClass="action-button" CausesValidation="false" />
                                <asp:Button ID="btnEdit" runat="server" Text="<%$ Resources:Strings, Common_Edit %>" CommandName="EditRecord" CommandArgument='<%# Eval("MaintenanceId") %>' CssClass="action-button" CausesValidation="false" />
                                <asp:Button ID="btnDelete" runat="server" Text="<%$ Resources:Strings, Common_Delete %>" CommandName="DeleteRecord" CommandArgument='<%# Eval("MaintenanceId") %>' CssClass="action-button danger" CausesValidation="false" OnClientClick="return confirm('Are you sure you want to delete this maintenance record?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div class="pagination"><asp:Literal ID="litPagination" runat="server" /></div>
            </ContentTemplate>
        </uc:ContentBox>

        <uc:ContentBox ID="cbMaintenanceForm" runat="server" HeaderText="<%$ Resources:Strings, BusMaintenance_AddHeading %>" HeaderColor="Blue" ContentColor="White" Visible="false">
            <ContentTemplate>
                <asp:Panel ID="pnlMaintenanceForm" runat="server" DefaultButton="btnSave">
                    <div class="form-row">
                        <span class="form-label">Bus</span>
                        <asp:Label ID="lblFormBus" runat="server" CssClass="detail-value" />
                    </div>
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Common_Date") %> <span class="form-required">*</span></span>
                        <span><asp:TextBox ID="txtMaintenanceDate" runat="server" CssClass="form-control small" MaxLength="10" placeholder="dd.MM.yyyy" />
                            <asp:RequiredFieldValidator ID="rfvMaintenanceDate" runat="server" ControlToValidate="txtMaintenanceDate" ErrorMessage="<%$ Resources:Strings, Common_Required %>" Display="Dynamic" ForeColor="#C00" ValidationGroup="MaintenanceForm" /></span>
                        <span class="form-hint">The date the work was performed (dd.MM.yyyy); future dates are rejected.</span>
                    </div>
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Common_NextService") %></span>
                        <span><asp:TextBox ID="txtNextDate" runat="server" CssClass="form-control small" MaxLength="10" placeholder="dd.MM.yyyy" /></span>
                        <span class="form-hint">Leave blank only when no follow-up date is planned.</span>
                    </div>
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Common_Type") %> <span class="form-required">*</span></span>
                        <span><asp:TextBox ID="txtType" runat="server" CssClass="form-control" MaxLength="100" />
                            <asp:RequiredFieldValidator ID="rfvType" runat="server" ControlToValidate="txtType" ErrorMessage="<%$ Resources:Strings, Common_Required %>" Display="Dynamic" ForeColor="#C00" ValidationGroup="MaintenanceForm" /></span>
                    </div>
                    <div class="form-row">
                        <span class="form-label">Found issue / inspection notes</span>
                        <asp:TextBox ID="txtFoundIssue" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" MaxLength="500" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Work performed / result</span>
                        <asp:TextBox ID="txtServiceResult" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" MaxLength="500" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Responsible mechanic</span>
                        <asp:DropDownList ID="ddlMechanic" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Common_MileageKm") %></span>
                        <span><asp:TextBox ID="txtMileage" runat="server" CssClass="form-control small" MaxLength="10" />
                            <asp:RangeValidator ID="rvMileage" runat="server" ControlToValidate="txtMileage" MinimumValue="0" MaximumValue="9999999" Type="Integer" ErrorMessage="Mileage must be between 0 and 9,999,999." Display="Dynamic" ForeColor="#C00" ValidationGroup="MaintenanceForm" /></span>
                    </div>
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Common_Roadworthiness") %> <span class="form-required">*</span></span>
                        <asp:DropDownList ID="ddlRoadworthiness" runat="server" CssClass="form-control">
                            <asp:ListItem Text="<%$ Resources:Strings, Common_Operational %>" Value="Исправен" />
                            <asp:ListItem Text="<%$ Resources:Strings, Common_Attention %>" Value="Требует внимания" />
                            <asp:ListItem Text="<%$ Resources:Strings, Common_NotOperational %>" Value="Неисправен" />
                        </asp:DropDownList>
                    </div>
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Common_Cost") %> <span class="form-required">*</span></span>
                        <span><asp:TextBox ID="txtCost" runat="server" CssClass="form-control small" MaxLength="15" />
                            <asp:RequiredFieldValidator ID="rfvCost" runat="server" ControlToValidate="txtCost" ErrorMessage="<%$ Resources:Strings, Common_Required %>" Display="Dynamic" ForeColor="#C00" ValidationGroup="MaintenanceForm" /></span>
                        <span class="form-hint">Non-negative amount in the database currency.</span>
                    </div>
                    <div class="form-row">
                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Strings, Common_Save %>" CssClass="action-button" OnClick="btnSave_Click" ValidationGroup="MaintenanceForm" />
                        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Strings, Common_Cancel %>" CssClass="action-button secondary" OnClick="btnCancel_Click" CausesValidation="false" />
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </uc:ContentBox>

        <uc:ContentBox ID="cbMaintenanceDetail" runat="server" HeaderText="<%$ Resources:Strings, Common_View %>" HeaderColor="Gray" ContentColor="Gray" Visible="false">
            <ContentTemplate>
                <asp:Literal ID="litDetail" runat="server" />
                <div style="margin-top: 12px;"><asp:Button ID="btnDetailEdit" runat="server" Text="<%$ Resources:Strings, Common_Edit %>" CssClass="action-button" OnClick="btnDetailEdit_Click" CausesValidation="false" />
                    <asp:Button ID="btnDetailClose" runat="server" Text="<%$ Resources:Strings, Common_Close %>" CssClass="action-button secondary" OnClick="btnDetailClose_Click" CausesValidation="false" /></div>
            </ContentTemplate>
        </uc:ContentBox>

        <p align="right"><a href="#top" style="font-size: 8pt; color: #000;"><%= Localization.GetHtml("Common_BackToTop") %></a></p>
    </div>
</asp:Content>
