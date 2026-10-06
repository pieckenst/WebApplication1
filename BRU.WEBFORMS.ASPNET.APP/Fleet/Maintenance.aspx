<%@ Page Title="<%$ Resources:Strings, Auto_Fleet_Maintenance_3 %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Maintenance.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Fleet.Maintenance" %>
<%@ Register TagPrefix="uc" TagName="ContentBox" Src="~/Controls/ContentBox.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        .content-page { padding: 18px; font-family: Tahoma, Verdana, Arial, sans-serif; max-width: 1480px; margin: 0 auto; }
        .page-title { font-size: 16pt; color: #000080; font-weight: bold; margin-bottom: 6px; }
        .page-divider { height: 2px; background-color: #000080; margin: 0 0 20px 0; }
        .text-regular { font-size: 10pt; line-height: 1.5; }
        .content-page > * + * { margin-top: 16px; }
        .content-page .page-divider + * { margin-top: 0; }
        .content-page .action-bar, .content-page .stats-summary { margin-top: 16px; }
        .text-small { font-size: 8pt; color: #666666; }
        .action-bar { background-color: #EDF2FB; border: 1px solid #1447AE; padding: 12px; margin-bottom: 18px; display: flex; flex-wrap: wrap; align-items: center; gap: 8px; }
        .action-button { display: inline-block; padding: 7px 12px; background-color: #1447AE; color: #FFFFFF; text-decoration: none; border: 1px solid #2459C3; border-radius: 3px; margin: 0; font-family: Tahoma, Arial, sans-serif; font-size: 9pt; min-height: 30px; box-sizing: border-box; }
        .action-button:hover { background-color: #2459C3; }
        .filter-section { background-color: #F2F2F2; border: 1px solid #CCCCCC; padding: 14px; margin-bottom: 18px; }
        .filter-row { margin-bottom: 12px; display: flex; flex-wrap: wrap; align-items: center; gap: 8px 12px; }
        .filter-row:last-child { margin-bottom: 0; }
        .filter-label { display: inline-block; width: 120px; font-weight: bold; font-size: 9pt; }
        .data-table { width: 100%; border-collapse: collapse; margin-top: 10px; font-size: 9pt; }
        .data-table th { background-color: #1447AE; color: #FFFFFF; padding: 8px; text-align: left; border: 1px solid #0A2E7A; }
        .data-table td { padding: 8px; border: 1px solid #CCCCCC; background-color: #FFFFFF; }
        .data-table tr:hover td { background-color: #EDF2FB; }
        .rw-operational { color: #008000; font-weight: bold; }
        .rw-attention { color: #FF6600; font-weight: bold; }
        .rw-notoperational { color: #CC0000; font-weight: bold; }
        .cost-high { color: #CC0000; font-weight: bold; }
        .cost-medium { color: #FF6600; font-weight: bold; }
        .cost-low { color: #008000; font-weight: bold; }
        .overdue { color: #CC0000; font-weight: bold; }
        .pagination { margin-top: 15px; text-align: right; }
        .pagination a { padding: 4px 8px; margin: 0 2px; background-color: #EDF2FB; border: 1px solid #1447AE; text-decoration: none; font-size: 9pt; }
        .pagination a:hover { background-color: #1447AE; color: #FFFFFF; }
        .pagination .current { padding: 4px 8px; margin: 0 2px; background-color: #1447AE; color: #FFFFFF; font-weight: bold; }
        .form-row { margin-bottom: 16px; display: flex; flex-wrap: wrap; align-items: flex-start; gap: 8px 12px; }
        .form-row:last-child { margin-bottom: 0; }
        .form-label { display: inline-block; width: 180px; padding-top: 6px; font-weight: bold; font-size: 9pt; }
        .form-control { padding: 4px 8px; border: 1px solid #1447AE; font-family: Tahoma, Arial, sans-serif; font-size: 9pt; width: 300px; }
        .form-required { color: #CC0000; }
        .error-message { background-color: #FFE6E6; border: 1px solid #CC0000; color: #CC0000; padding: 10px; margin-bottom: 15px; font-size: 10pt; }
        .success-message { background-color: #E6FFE6; border: 1px solid #008000; color: #008000; padding: 10px; margin-bottom: 15px; font-size: 10pt; }
        .stats-summary { background-color: #EDF2FB; border: 1px solid #1447AE; padding: 10px; margin-bottom: 15px; font-size: 9pt; }
        .stats-item { display: inline-block; margin-right: 20px; }
        .stats-label { font-weight: bold; color: #1447AE; }
        .detail-section { margin-bottom: 15px; }
        .detail-row { margin-bottom: 6px; font-size: 9pt; }
        .detail-label { display: inline-block; width: 180px; font-weight: bold; color: #666666; }
        .detail-value { color: #000000; }
        .alert-box { background-color: #FFF3CD; border: 1px solid #FFCC00; color: #856404; padding: 10px; margin-bottom: 15px; font-size: 10pt; }
        .data-table { display: block; overflow-x: auto; white-space: nowrap; }
        @media (max-width: 760px) { .content-page { padding: 8px; } .page-title { font-size: 14pt; } .form-label, .filter-label { display: block; width: auto; margin: 0 0 4px; } .form-control, select, input[type='text'], input[type='date'] { width: 100%; max-width: 100%; box-sizing: border-box; } .action-bar { display: flex; flex-wrap: wrap; gap: 6px; } .action-button { margin: 0; } }
    </style>

    <div class="content-page">
        <a name="top"></a>
        <div class="page-title"><%= Localization.GetHtml("Maintenance_Heading") %></div>
        <div class="page-divider"></div>

        <asp:Panel ID="pnlError" runat="server" CssClass="error-message" Visible="false">
            <asp:Literal ID="litError" runat="server" />
        </asp:Panel>

        <asp:Panel ID="pnlSuccess" runat="server" CssClass="success-message" Visible="false">
            <asp:Literal ID="litSuccess" runat="server" />
        </asp:Panel>

        <asp:Panel ID="pnlOverdueAlert" runat="server" CssClass="alert-box" Visible="false">
            <asp:Literal ID="litOverdueAlert" runat="server" />
        </asp:Panel>

        <div class="stats-summary">
            <div class="stats-item">
                <span class="stats-label"><%= Localization.GetHtml("Maintenance_TotalRecords") %></span>
                <asp:Literal ID="litTotalRecords" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label"><%= Localization.GetHtml("Maintenance_Operational") %></span>
                <asp:Literal ID="litOperational" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label"><%= Localization.GetHtml("Maintenance_NeedsAttention") %></span>
                <asp:Literal ID="litNeedsAttention" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label"><%= Localization.GetHtml("Maintenance_NotOperational") %></span>
                <asp:Literal ID="litNotOperational" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label"><%= Localization.GetHtml("Maintenance_TotalCost") %></span>
                <asp:Literal ID="litTotalCost" runat="server" Text="0.00" />
            </div>
        </div>

        <div class="action-bar">
            <asp:Button ID="btnAddRecord" runat="server" Text="<%$ Resources:Strings, Maintenance_AddRecord %>" CssClass="action-button" OnClick="btnAddRecord_Click" />
            <asp:Button ID="btnRefresh" runat="server" Text="<%$ Resources:Strings, Common_Refresh %>" CssClass="action-button" OnClick="btnRefresh_Click" />
        </div>

        <uc:ContentBox ID="cbFilters" runat="server" HeaderText="<%$ Resources:Strings, Maintenance_FilterRecords %>" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <div class="filter-section">
                    <div class="filter-row">
                        <span class="filter-label"><%= Localization.GetHtml("Maintenance_RoadworthinessFilter") %></span>
                        <asp:DropDownList ID="ddlRwFilter" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlRwFilter_SelectedIndexChanged">
                            <asp:ListItem Text="<%$ Resources:Strings, Common_All %>" Value="" />
                            <asp:ListItem Text="<%$ Resources:Strings, Common_Operational %>" Value="Исправен" />
                            <asp:ListItem Text="<%$ Resources:Strings, Common_Attention %>" Value="Требует внимания" />
                            <asp:ListItem Text="<%$ Resources:Strings, Common_NotOperational %>" Value="Неис��равен" />
                        </asp:DropDownList>
                    </div>
                    <div class="filter-row">
                        <span class="filter-label"><%= Localization.GetHtml("Maintenance_BusFilter") %></span>
                        <asp:DropDownList ID="ddlBusFilter" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlBusFilter_SelectedIndexChanged">
                            <asp:ListItem Text="<%$ Resources:Strings, Maintenance_AllBuses %>" Value="" />
                        </asp:DropDownList>
                    </div>
                </div>
            </ContentTemplate>
        </uc:ContentBox>

        <uc:ContentBox ID="cbMaintenanceList" runat="server" HeaderText="<%$ Resources:Strings, Maintenance_ListHeader %>" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <asp:GridView ID="gvMaintenance" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="20" AllowSorting="true" CssClass="data-table" GridLines="Both" PagerStyle-CssClass="pagination" OnPageIndexChanging="gvMaintenance_PageIndexChanging" OnSorting="gvMaintenance_Sorting" OnRowCommand="gvMaintenance_RowCommand" OnRowDataBound="gvMaintenance_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="MaintenanceId" HeaderText="<%$ Resources:Strings, Common_ID %>" SortExpression="maintenance_id" ReadOnly="true" />
                        <asp:BoundField DataField="FleetNumber" HeaderText="<%$ Resources:Strings, Common_Bus %>" SortExpression="fleet_number" />
                        <asp:BoundField DataField="MaintenanceDate" HeaderText="<%$ Resources:Strings, Maintenance_Date %>" SortExpression="maintenance_date" DataFormatString="{0:dd.MM.yyyy}" />
                        <asp:BoundField DataField="MaintenanceType" HeaderText="<%$ Resources:Strings, Common_Type %>" SortExpression="maintenance_type" />
                        <asp:BoundField DataField="MileageKm" HeaderText="<%$ Resources:Strings, Common_Mileage %>" SortExpression="mileage_km" />
                        <asp:TemplateField HeaderText="<%$ Resources:Strings, Common_Roadworthiness %>" SortExpression="roadworthiness">
                            <ItemTemplate>
                                <asp:Literal ID="litRw" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Strings, Common_Cost %>" SortExpression="maintenance_cost">
                            <ItemTemplate>
                                <asp:Literal ID="litCost" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="NextMaintenanceDate" HeaderText="<%$ Resources:Strings, Common_NextService %>" SortExpression="next_maintenance_date" DataFormatString="{0:dd.MM.yyyy}" />
                        <asp:BoundField DataField="DaysFromLastService" HeaderText="<%$ Resources:Strings, Maintenance_DaysAgo %>" SortExpression="days_from_last_service" />
                        <asp:TemplateField HeaderText="<%$ Resources:Strings, Common_Actions %>">
                            <ItemTemplate>
                                <asp:Button ID="btnView" runat="server" Text="<%$ Resources:Strings, Common_View %>" CommandName="ViewRecord" CommandArgument='<%# Eval("MaintenanceId") %>' CssClass="action-button" />
                                <asp:Button ID="btnEdit" runat="server" Text="<%$ Resources:Strings, Common_Edit %>" CommandName="EditRecord" CommandArgument='<%# Eval("MaintenanceId") %>' CssClass="action-button" />
                                <asp:Button ID="btnDelete" runat="server" Text="<%$ Resources:Strings, Common_Delete %>" CommandName="DeleteRecord" CommandArgument='<%# Eval("MaintenanceId") %>' CssClass="action-button" OnClientClick="return confirm('Are you sure you want to delete this maintenance record?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </uc:ContentBox>

        <div class="pagination">
            <asp:Literal ID="litPagination" runat="server" />
        </div>

        <!-- Add/Edit Maintenance Form -->
        <uc:ContentBox ID="cbMaintenanceForm" runat="server" HeaderText="<%$ Resources:Strings, Maintenance_AddEditHeader %>" HeaderColor="Blue" ContentColor="White" Visible="false">
            <ContentTemplate>
                <asp:Panel ID="pnlMaintenanceForm" runat="server" DefaultButton="btnSave">
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Common_Bus") %> <span class="form-required">*</span></span>
                        <asp:DropDownList ID="ddlBus" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvBus" runat="server" ControlToValidate="ddlBus" InitialValue="" ErrorMessage="<%$ Resources:Strings, Maintenance_BusRequired %>" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="MaintenanceForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Common_Mechanic") %></span>
                        <asp:DropDownList ID="ddlMechanic" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Maintenance_MaintenanceDate") %> <span class="form-required">*</span></span>
                        <asp:TextBox ID="txtMaintenanceDate" runat="server" CssClass="form-control" Width="120" placeholder="<%$ Resources:Strings, Auto_Fleet_Maintenance_1 %>" />
                        <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtMaintenanceDate" ErrorMessage="<%$ Resources:Strings, Maintenance_DateRequired %>" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="MaintenanceForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Maintenance_NextServiceDate") %></span>
                        <asp:TextBox ID="txtNextDate" runat="server" CssClass="form-control" Width="120" placeholder="<%$ Resources:Strings, Auto_Fleet_Maintenance_2 %>" />
                    </div>
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Common_Type") %> <span class="form-required">*</span></span>
                        <asp:TextBox ID="txtType" runat="server" CssClass="form-control" MaxLength="100" />
                        <asp:RequiredFieldValidator ID="rfvType" runat="server" ControlToValidate="txtType" ErrorMessage="<%$ Resources:Strings, Maintenance_TypeRequired %>" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="MaintenanceForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Maintenance_FoundIssue") %></span>
                        <asp:TextBox ID="txtFoundIssue" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                    </div>
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Maintenance_ServiceResult") %></span>
                        <asp:TextBox ID="txtServiceResult" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                    </div>
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Common_MileageKm") %></span>
                        <asp:TextBox ID="txtMileage" runat="server" CssClass="form-control" Width="120" />
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
                        <asp:TextBox ID="txtCost" runat="server" CssClass="form-control" Width="120" Text="0.00" />
                        <asp:RequiredFieldValidator ID="rfvCost" runat="server" ControlToValidate="txtCost" ErrorMessage="<%$ Resources:Strings, Maintenance_CostRequired %>" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="MaintenanceForm" />
                    </div>
                    <div class="form-row" style="margin-top: 15px;">
                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Strings, Common_Save %>" CssClass="action-button" OnClick="btnSave_Click" ValidationGroup="MaintenanceForm" />
                        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Strings, Common_Cancel %>" CssClass="action-button" OnClick="btnCancel_Click" CausesValidation="false" />
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </uc:ContentBox>

        <!-- Maintenance Detail View -->
        <uc:ContentBox ID="cbMaintenanceDetail" runat="server" HeaderText="<%$ Resources:Strings, Maintenance_Details %>" HeaderColor="Gray" ContentColor="Gray" Visible="false">
            <ContentTemplate>
                <asp:Literal ID="litDetail" runat="server" />
                <div style="margin-top: 15px;">
                    <asp:Button ID="btnDetailClose" runat="server" Text="<%$ Resources:Strings, Common_Close %>" CssClass="action-button" OnClick="btnDetailClose_Click" />
                </div>
            </ContentTemplate>
        </uc:ContentBox>

        <p align="right" style="margin-top: 20px;"><a href="#top" style="font-size: 8pt; color: #000000;"><%= Localization.GetHtml("Common_BackToTop") %></a></p>
    </div>

</asp:Content>
