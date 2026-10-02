<%@ Page Title="Route Management - Autopark Management System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Routes.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Operations.Routes" %>
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
        .search-box { display: inline-block; margin-left: 20px; }
        .search-input { padding: 4px 8px; border: 1px solid #1447AE; font-family: Tahoma, Arial, sans-serif; font-size: 9pt; }
        .data-table { width: 100%; border-collapse: collapse; margin-top: 10px; font-size: 9pt; }
        .data-table th { background-color: #1447AE; color: #FFFFFF; padding: 8px; text-align: left; border: 1px solid #0A2E7A; }
        .data-table td { padding: 8px; border: 1px solid #CCCCCC; background-color: #FFFFFF; }
        .data-table tr:hover td { background-color: #EDF2FB; }
        .status-active { color: #008000; font-weight: bold; }
        .status-inactive { color: #CC0000; font-weight: bold; }
        .route-num { font-weight: bold; color: #1447AE; font-size: 10pt; }
        .stop-list { font-size: 8pt; color: #666666; }
        .stop-item { padding: 2px 0; border-bottom: 1px dotted #CCCCCC; }
        .stop-sequence { display: inline-block; width: 25px; text-align: right; font-weight: bold; color: #1447AE; }
        .stop-name { padding-left: 8px; }
        .stop-distance { float: right; color: #999999; }
        .pagination { margin-top: 15px; text-align: right; }
        .pagination a { padding: 4px 8px; margin: 0 2px; background-color: #EDF2FB; border: 1px solid #1447AE; text-decoration: none; font-size: 9pt; }
        .pagination a:hover { background-color: #1447AE; color: #FFFFFF; }
        .pagination .current { padding: 4px 8px; margin: 0 2px; background-color: #1447AE; color: #FFFFFF; font-weight: bold; }
        .form-row { margin-bottom: 12px; }
        .form-label { display: inline-block; width: 150px; font-weight: bold; font-size: 9pt; }
        .form-control { padding: 4px 8px; border: 1px solid #1447AE; font-family: Tahoma, Arial, sans-serif; font-size: 9pt; width: 300px; }
        .form-required { color: #CC0000; }
        .error-message { background-color: #FFE6E6; border: 1px solid #CC0000; color: #CC0000; padding: 10px; margin-bottom: 15px; font-size: 10pt; }
        .success-message { background-color: #E6FFE6; border: 1px solid #008000; color: #008000; padding: 10px; margin-bottom: 15px; font-size: 10pt; }
        .stats-summary { background-color: #EDF2FB; border: 1px solid #1447AE; padding: 10px; margin-bottom: 15px; font-size: 9pt; }
        .stats-item { display: inline-block; margin-right: 20px; }
        .stats-label { font-weight: bold; color: #1447AE; }
        .detail-section { margin-bottom: 15px; }
        .detail-row { margin-bottom: 6px; font-size: 9pt; }
        .detail-label { display: inline-block; width: 150px; font-weight: bold; color: #666666; }
        .detail-value { color: #000000; }
        .tabs { margin-bottom: 10px; }
        .tab { display: inline-block; padding: 6px 16px; background-color: #EDF2FB; border: 1px solid #1447AE; border-bottom: none; margin-right: 2px; font-size: 9pt; cursor: pointer; text-decoration: none; color: #1447AE; font-weight: bold; }
        .tab.active { background-color: #1447AE; color: #FFFFFF; }
    </style>

    <div class="content-page">
        <a name="top"></a>
        <div class="page-title">Route Management</div>
        <div class="page-divider"></div>

        <asp:Panel ID="pnlError" runat="server" CssClass="error-message" Visible="false">
            <asp:Literal ID="litError" runat="server" />
        </asp:Panel>

        <asp:Panel ID="pnlSuccess" runat="server" CssClass="success-message" Visible="false">
            <asp:Literal ID="litSuccess" runat="server" />
        </asp:Panel>

        <div class="stats-summary">
            <div class="stats-item">
                <span class="stats-label">Total Routes:</span>
                <asp:Literal ID="litTotalRoutes" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label">Active Routes:</span>
                <asp:Literal ID="litActiveRoutes" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label">Total Stops:</span>
                <asp:Literal ID="litTotalStops" runat="server" Text="0" />
            </div>
        </div>

        <div class="action-bar">
            <asp:Button ID="btnAddRoute" runat="server" Text="Add New Route" CssClass="action-button" OnClick="btnAddRoute_Click" />
            <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="action-button" OnClick="btnRefresh_Click" />
            <div class="search-box">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="search-input" Placeholder="Search by route number or name..." />
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="action-button" OnClick="btnSearch_Click" />
            </div>
        </div>

        <uc:ContentBox ID="cbRoutesList" runat="server" HeaderText="Active Routes" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <asp:GridView ID="gvRoutes" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="15" AllowSorting="true" CssClass="data-table" GridLines="Both" PagerStyle-CssClass="pagination" OnPageIndexChanging="gvRoutes_PageIndexChanging" OnSorting="gvRoutes_Sorting" OnRowCommand="gvRoutes_RowCommand" OnRowDataBound="gvRoutes_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="RouteId" HeaderText="ID" SortExpression="route_id" ReadOnly="true" />
                        <asp:TemplateField HeaderText="Route #" SortExpression="route_num">
                            <ItemTemplate>
                                <span class="route-num"><%# Eval("RouteNum") %></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RouteName" HeaderText="Route Name" SortExpression="route_name" />
                        <asp:BoundField DataField="StartStop" HeaderText="Start Stop" SortExpression="start_stop" />
                        <asp:BoundField DataField="EndStop" HeaderText="End Stop" SortExpression="end_stop" />
                        <asp:BoundField DataField="StopCount" HeaderText="Stops" SortExpression="stop_count" />
                        <asp:TemplateField HeaderText="Status" SortExpression="is_active">
                            <ItemTemplate>
                                <asp:Literal ID="litStatus" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:Button ID="btnView" runat="server" Text="View Stops" CommandName="ViewStops" CommandArgument='<%# Eval("RouteId") %>' CssClass="action-button" />
                                <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="EditRoute" CommandArgument='<%# Eval("RouteId") %>' CssClass="action-button" />
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="DeleteRoute" CommandArgument='<%# Eval("RouteId") %>' CssClass="action-button" OnClientClick="return confirm('Are you sure you want to delete this route?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </uc:ContentBox>

        <div class="pagination">
            <asp:Literal ID="litPagination" runat="server" />
        </div>

        <!-- Add/Edit Route Form -->
        <uc:ContentBox ID="cbRouteForm" runat="server" HeaderText="Add / Edit Route" HeaderColor="Blue" ContentColor="White" Visible="false">
            <ContentTemplate>
                <asp:Panel ID="pnlRouteForm" runat="server" DefaultButton="btnSaveRoute">
                    <div class="form-row">
                        <span class="form-label">Route Number <span class="form-required">*</span></span>
                        <asp:TextBox ID="txtRouteNum" runat="server" CssClass="form-control" MaxLength="20" Width="100" />
                        <asp:RequiredFieldValidator ID="rfvRouteNum" runat="server" ControlToValidate="txtRouteNum" ErrorMessage="Route number is required" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="RouteForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Route Name <span class="form-required">*</span></span>
                        <asp:TextBox ID="txtRouteName" runat="server" CssClass="form-control" MaxLength="120" />
                        <asp:RequiredFieldValidator ID="rfvRouteName" runat="server" ControlToValidate="txtRouteName" ErrorMessage="Route name is required" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="RouteForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Start Stop <span class="form-required">*</span></span>
                        <asp:TextBox ID="txtStartStop" runat="server" CssClass="form-control" MaxLength="120" />
                        <asp:RequiredFieldValidator ID="rfvStartStop" runat="server" ControlToValidate="txtStartStop" ErrorMessage="Start stop is required" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="RouteForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">End Stop <span class="form-required">*</span></span>
                        <asp:TextBox ID="txtEndStop" runat="server" CssClass="form-control" MaxLength="120" />
                        <asp:RequiredFieldValidator ID="rfvEndStop" runat="server" ControlToValidate="txtEndStop" ErrorMessage="End stop is required" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="RouteForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Active</span>
                        <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                    </div>
                    <div class="form-row" style="margin-top: 15px;">
                        <asp:Button ID="btnSaveRoute" runat="server" Text="Save" CssClass="action-button" OnClick="btnSaveRoute_Click" ValidationGroup="RouteForm" />
                        <asp:Button ID="btnCancelRoute" runat="server" Text="Cancel" CssClass="action-button" OnClick="btnCancelRoute_Click" CausesValidation="false" />
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </uc:ContentBox>

        <!-- Route Stops Detail -->
        <uc:ContentBox ID="cbRouteStops" runat="server" HeaderText="Route Stops" HeaderColor="Gray" ContentColor="Gray" Visible="false">
            <ContentTemplate>
                <asp:Literal ID="litRouteStops" runat="server" />
                <div style="margin-top: 15px;">
                    <asp:Button ID="btnCloseStops" runat="server" Text="Close" CssClass="action-button" OnClick="btnCloseStops_Click" />
                </div>
            </ContentTemplate>
        </uc:ContentBox>

        <p align="right" style="margin-top: 20px;"><a href="#top" style="font-size: 8pt; color: #000000;">Back to top &#9650;</a></p>
    </div>

</asp:Content>
