<%@ Page Title="Maintenance Records - Autopark Management System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Maintenance.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Fleet.Maintenance" %>
<%@ Register TagPrefix="uc" TagName="ContentBox" Src="~/Controls/ContentBox.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        .content-page { padding: 10px; font-family: Tahoma, Verdana, Arial, sans-serif; }
        .page-title { font-size: 16pt; color: #000080; font-weight: bold; }
        .page-divider { height: 2px; background-color: #000080; margin: 5px 0 15px 0; }
        .text-regular { font-size: 10pt; line-height: 1.5; }
        .text-small { font-size: 8pt; color: #666666; }
        .action-bar { background-color: #EDF2FB; border: 1px solid #1447AE; padding: 10px; margin-bottom: 15px; }
        .action-button { display: inline-block; padding: 6px 12px; background-color: #1447AE; color: #FFFFFF; text-decoration: none; border: 1px solid #2459C3; border-radius: 3px; margin-right: 8px; margin-bottom: 5px; font-family: Tahoma, Arial, sans-serif; font-size: 9pt; }
        .action-button:hover { background-color: #2459C3; }
        .filter-section { background-color: #F2F2F2; border: 1px solid #CCCCCC; padding: 10px; margin-bottom: 15px; }
        .filter-row { margin-bottom: 8px; }
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
        .form-row { margin-bottom: 12px; }
        .form-label { display: inline-block; width: 180px; font-weight: bold; font-size: 9pt; }
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
    </style>

    <div class="content-page">
        <a name="top"></a>
        <div class="page-title">Maintenance Records</div>
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
                <span class="stats-label">Total Records:</span>
                <asp:Literal ID="litTotalRecords" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label">Operational:</span>
                <asp:Literal ID="litOperational" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label">Needs Attention:</span>
                <asp:Literal ID="litNeedsAttention" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label">Not Operational:</span>
                <asp:Literal ID="litNotOperational" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label">Total Cost:</span>
                <asp:Literal ID="litTotalCost" runat="server" Text="0.00" />
            </div>
        </div>

        <div class="action-bar">
            <asp:Button ID="btnAddRecord" runat="server" Text="Add Maintenance Record" CssClass="action-button" OnClick="btnAddRecord_Click" />
            <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="action-button" OnClick="btnRefresh_Click" />
        </div>

        <uc:ContentBox ID="cbFilters" runat="server" HeaderText="Filter Records" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <div class="filter-section">
                    <div class="filter-row">
                        <span class="filter-label">Roadworthiness:</span>
                        <asp:DropDownList ID="ddlRwFilter" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlRwFilter_SelectedIndexChanged">
                            <asp:ListItem Text="All" Value="" />
                            <asp:ListItem Text="Исправен" Value="Исправен" />
                            <asp:ListItem Text="Требует внимания" Value="Требует внимания" />
                            <asp:ListItem Text="Неисправен" Value="Неисправен" />
                        </asp:DropDownList>
                    </div>
                    <div class="filter-row">
                        <span class="filter-label">Bus:</span>
                        <asp:DropDownList ID="ddlBusFilter" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlBusFilter_SelectedIndexChanged">
                            <asp:ListItem Text="All Buses" Value="" />
                        </asp:DropDownList>
                    </div>
                </div>
            </ContentTemplate>
        </uc:ContentBox>

        <uc:ContentBox ID="cbMaintenanceList" runat="server" HeaderText="Maintenance Records" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <asp:GridView ID="gvMaintenance" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="20" AllowSorting="true" CssClass="data-table" GridLines="Both" PagerStyle-CssClass="pagination" OnPageIndexChanging="gvMaintenance_PageIndexChanging" OnSorting="gvMaintenance_Sorting" OnRowCommand="gvMaintenance_RowCommand" OnRowDataBound="gvMaintenance_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="MaintenanceId" HeaderText="ID" SortExpression="maintenance_id" ReadOnly="true" />
                        <asp:BoundField DataField="FleetNumber" HeaderText="Bus" SortExpression="fleet_number" />
                        <asp:BoundField DataField="MaintenanceDate" HeaderText="Date" SortExpression="maintenance_date" DataFormatString="{0:dd.MM.yyyy}" />
                        <asp:BoundField DataField="MaintenanceType" HeaderText="Type" SortExpression="maintenance_type" />
                        <asp:BoundField DataField="MileageKm" HeaderText="Mileage" SortExpression="mileage_km" />
                        <asp:TemplateField HeaderText="Roadworthiness" SortExpression="roadworthiness">
                            <ItemTemplate>
                                <asp:Literal ID="litRw" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cost" SortExpression="maintenance_cost">
                            <ItemTemplate>
                                <asp:Literal ID="litCost" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="NextMaintenanceDate" HeaderText="Next Service" SortExpression="next_maintenance_date" DataFormatString="{0:dd.MM.yyyy}" />
                        <asp:BoundField DataField="DaysFromLastService" HeaderText="Days Ago" SortExpression="days_from_last_service" />
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:Button ID="btnView" runat="server" Text="View" CommandName="ViewRecord" CommandArgument='<%# Eval("MaintenanceId") %>' CssClass="action-button" />
                                <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="EditRecord" CommandArgument='<%# Eval("MaintenanceId") %>' CssClass="action-button" />
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="DeleteRecord" CommandArgument='<%# Eval("MaintenanceId") %>' CssClass="action-button" OnClientClick="return confirm('Are you sure you want to delete this maintenance record?');" />
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
        <uc:ContentBox ID="cbMaintenanceForm" runat="server" HeaderText="Add / Edit Maintenance Record" HeaderColor="Blue" ContentColor="White" Visible="false">
            <ContentTemplate>
                <asp:Panel ID="pnlMaintenanceForm" runat="server" DefaultButton="btnSave">
                    <div class="form-row">
                        <span class="form-label">Bus <span class="form-required">*</span></span>
                        <asp:DropDownList ID="ddlBus" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvBus" runat="server" ControlToValidate="ddlBus" InitialValue="" ErrorMessage="Bus is required" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="MaintenanceForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Mechanic</span>
                        <asp:DropDownList ID="ddlMechanic" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Maintenance Date <span class="form-required">*</span></span>
                        <asp:TextBox ID="txtMaintenanceDate" runat="server" CssClass="form-control" Width="120" placeholder="dd.MM.yyyy" />
                        <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtMaintenanceDate" ErrorMessage="Date is required" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="MaintenanceForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Next Service Date</span>
                        <asp:TextBox ID="txtNextDate" runat="server" CssClass="form-control" Width="120" placeholder="dd.MM.yyyy" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Type <span class="form-required">*</span></span>
                        <asp:TextBox ID="txtType" runat="server" CssClass="form-control" MaxLength="100" />
                        <asp:RequiredFieldValidator ID="rfvType" runat="server" ControlToValidate="txtType" ErrorMessage="Type is required" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="MaintenanceForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Found Issue</span>
                        <asp:TextBox ID="txtFoundIssue" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Service Result</span>
                        <asp:TextBox ID="txtServiceResult" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Mileage (km)</span>
                        <asp:TextBox ID="txtMileage" runat="server" CssClass="form-control" Width="120" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Roadworthiness <span class="form-required">*</span></span>
                        <asp:DropDownList ID="ddlRoadworthiness" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Исправен" Value="Исправен" />
                            <asp:ListItem Text="Требует внимания" Value="Требует внимания" />
                            <asp:ListItem Text="Неисправен" Value="Неисправен" />
                        </asp:DropDownList>
                    </div>
                    <div class="form-row">
                        <span class="form-label">Cost <span class="form-required">*</span></span>
                        <asp:TextBox ID="txtCost" runat="server" CssClass="form-control" Width="120" Text="0.00" />
                        <asp:RequiredFieldValidator ID="rfvCost" runat="server" ControlToValidate="txtCost" ErrorMessage="Cost is required" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="MaintenanceForm" />
                    </div>
                    <div class="form-row" style="margin-top: 15px;">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="action-button" OnClick="btnSave_Click" ValidationGroup="MaintenanceForm" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="action-button" OnClick="btnCancel_Click" CausesValidation="false" />
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </uc:ContentBox>

        <!-- Maintenance Detail View -->
        <uc:ContentBox ID="cbMaintenanceDetail" runat="server" HeaderText="Maintenance Details" HeaderColor="Gray" ContentColor="Gray" Visible="false">
            <ContentTemplate>
                <asp:Literal ID="litDetail" runat="server" />
                <div style="margin-top: 15px;">
                    <asp:Button ID="btnDetailClose" runat="server" Text="Close" CssClass="action-button" OnClick="btnDetailClose_Click" />
                </div>
            </ContentTemplate>
        </uc:ContentBox>

        <p align="right" style="margin-top: 20px;"><a href="#top" style="font-size: 8pt; color: #000000;">Back to top &#9650;</a></p>
    </div>

</asp:Content>
