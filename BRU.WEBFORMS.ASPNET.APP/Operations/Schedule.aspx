<%@ Page Title="Schedule Management - Autopark Management System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Schedule.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Operations.Schedule" %>
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
        .data-table { width: 100%; border-collapse: collapse; margin-top: 10px; font-size: 9pt; }
        .data-table th { background-color: #1447AE; color: #FFFFFF; padding: 8px; text-align: left; border: 1px solid #0A2E7A; }
        .data-table td { padding: 8px; border: 1px solid #CCCCCC; background-color: #FFFFFF; }
        .data-table tr:hover td { background-color: #EDF2FB; }
        .status-planned { color: #0066CC; font-weight: bold; }
        .status-inprogress { color: #FF6600; font-weight: bold; }
        .status-completed { color: #008000; font-weight: bold; }
        .status-cancelled { color: #CC0000; font-weight: bold; }
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
        .date-picker-section { background-color: #F2F2F2; border: 1px solid #CCCCCC; padding: 10px; margin-bottom: 15px; }
        .date-input { padding: 4px 8px; border: 1px solid #1447AE; font-family: Tahoma, Arial, sans-serif; font-size: 9pt; width: 120px; }
        .route-num { font-weight: bold; color: #1447AE; }
        .schedule-time { font-weight: bold; color: #000080; }
        .detail-section { margin-bottom: 15px; }
        .detail-row { margin-bottom: 6px; font-size: 9pt; }
        .detail-label { display: inline-block; width: 150px; font-weight: bold; color: #666666; }
        .detail-value { color: #000000; }
    </style>

    <div class="content-page">
        <a name="top"></a>
        <div class="page-title">Schedule Management</div>
        <div class="page-divider"></div>

        <asp:Panel ID="pnlError" runat="server" CssClass="error-message" Visible="false">
            <asp:Literal ID="litError" runat="server" />
        </asp:Panel>

        <asp:Panel ID="pnlSuccess" runat="server" CssClass="success-message" Visible="false">
            <asp:Literal ID="litSuccess" runat="server" />
        </asp:Panel>

        <div class="stats-summary">
            <div class="stats-item">
                <span class="stats-label">Today's Trips:</span>
                <asp:Literal ID="litTodayTrips" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label">Planned:</span>
                <asp:Literal ID="litPlanned" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label">In Progress:</span>
                <asp:Literal ID="litInProgress" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label">Completed:</span>
                <asp:Literal ID="litCompleted" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label">Cancelled:</span>
                <asp:Literal ID="litCancelled" runat="server" Text="0" />
            </div>
        </div>

        <div class="action-bar">
            <asp:Button ID="btnToday" runat="server" Text="Today's Schedule" CssClass="action-button" OnClick="btnToday_Click" />
            <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="action-button" OnClick="btnRefresh_Click" />
            <div style="display: inline-block; margin-left: 20px;">
                <span class="form-label">Route:</span>
                <asp:DropDownList ID="ddlRouteFilter" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlRouteFilter_SelectedIndexChanged">
                    <asp:ListItem Text="All Routes" Value="" />
                </asp:DropDownList>
            </div>
        </div>

        <div class="date-picker-section">
            <div class="form-row">
                <span class="form-label">Date From:</span>
                <asp:TextBox ID="txtDateFrom" runat="server" CssClass="date-input" placeholder="dd.MM.yyyy" />
                <span class="form-label" style="margin-left: 20px;">Date To:</span>
                <asp:TextBox ID="txtDateTo" runat="server" CssClass="date-input" placeholder="dd.MM.yyyy" />
                <asp:Button ID="btnApplyDateRange" runat="server" Text="Apply Range" CssClass="action-button" OnClick="btnApplyDateRange_Click" />
            </div>
        </div>

        <uc:ContentBox ID="cbScheduleList" runat="server" HeaderText="Trip Schedule" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <asp:GridView ID="gvSchedule" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="20" AllowSorting="true" CssClass="data-table" GridLines="Both" PagerStyle-CssClass="pagination" OnPageIndexChanging="gvSchedule_PageIndexChanging" OnSorting="gvSchedule_Sorting" OnRowDataBound="gvSchedule_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="ScheduleId" HeaderText="ID" SortExpression="schedule_id" ReadOnly="true" />
                        <asp:TemplateField HeaderText="Route" SortExpression="route_num">
                            <ItemTemplate>
                                <span class="route-num"><%# Eval("RouteNum") %></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RouteName" HeaderText="Route Name" SortExpression="route_name" />
                        <asp:BoundField DataField="ServiceDate" HeaderText="Date" SortExpression="service_date" DataFormatString="{0:dd.MM.yyyy}" />
                        <asp:TemplateField HeaderText="Departure" SortExpression="departure_time">
                            <ItemTemplate>
                                <span class="schedule-time"><%# Eval("DepartureTime") %></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Arrival" SortExpression="arrival_time">
                            <ItemTemplate>
                                <span class="schedule-time"><%# Eval("ArrivalTime") %></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="TripMinutes" HeaderText="Duration (min)" SortExpression="trip_minutes" />
                        <asp:BoundField DataField="FleetNumber" HeaderText="Bus" SortExpression="fleet_number" />
                        <asp:BoundField DataField="BusModel" HeaderText="Model" SortExpression="bus_model" />
                        <asp:BoundField DataField="DriverName" HeaderText="Driver" SortExpression="driver_name" />
                        <asp:BoundField DataField="AvailableSeatNum" HeaderText="Seats" SortExpression="available_seat_num" />
                        <asp:TemplateField HeaderText="Status" SortExpression="schedule_status">
                            <ItemTemplate>
                                <asp:Literal ID="litStatus" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </uc:ContentBox>

        <div class="pagination">
            <asp:Literal ID="litPagination" runat="server" />
        </div>

        <p align="right" style="margin-top: 20px;"><a href="#top" style="font-size: 8pt; color: #000000;">Back to top &#9650;</a></p>
    </div>

</asp:Content>
