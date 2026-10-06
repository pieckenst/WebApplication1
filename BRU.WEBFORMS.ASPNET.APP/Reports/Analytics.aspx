<%@ Page Title="Business Analytics - Autopark Management System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Analytics.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Reports.Analytics" %>
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
        .error-message { background-color: #FFE6E6; border: 1px solid #CC0000; color: #CC0000; padding: 10px; margin-bottom: 15px; font-size: 10pt; }
        .success-message { background-color: #E6FFE6; border: 1px solid #008000; color: #008000; padding: 10px; margin-bottom: 15px; font-size: 10pt; }
        .stats-summary { background-color: #EDF2FB; border: 1px solid #1447AE; padding: 10px; margin-bottom: 15px; font-size: 9pt; }
        .stats-item { display: inline-block; margin-right: 20px; }
        .stats-label { font-weight: bold; color: #1447AE; }
        .stats-value { font-size: 14pt; font-weight: bold; color: #000080; }
        .chart-bar-container { width: 100%; background-color: #F2F2F2; border: 1px solid #CCCCCC; padding: 10px; margin-top: 5px; }
        .chart-bar-row { margin-bottom: 8px; }
        .chart-bar-label { display: inline-block; width: 150px; font-size: 9pt; font-weight: bold; vertical-align: top; }
        .chart-bar-track { display: inline-block; width: 60%; background-color: #E0E0E0; height: 18px; vertical-align: middle; position: relative; }
        .chart-bar-fill { height: 100%; background-color: #1447AE; }
        .chart-bar-value { display: inline-block; margin-left: 8px; font-size: 9pt; font-weight: bold; vertical-align: middle; }
        .kpi-grid { display: table; width: 100%; border-collapse: separate; border-spacing: 10px; margin-bottom: 15px; }
        .kpi-cell { display: table-cell; width: 25%; background-color: #EDF2FB; border: 1px solid #1447AE; padding: 15px; text-align: center; vertical-align: middle; }
        .kpi-label { font-size: 9pt; color: #1447AE; font-weight: bold; }
        .kpi-value { font-size: 20pt; color: #000080; font-weight: bold; }
        .kpi-sub { font-size: 8pt; color: #666666; }
        .section-header { font-size: 12pt; color: #000080; font-weight: bold; margin: 20px 0 10px 0; border-bottom: 1px solid #000080; padding-bottom: 3px; }
        @media (max-width: 760px) { .content-page { padding: 8px; } .kpi-grid { display: grid; grid-template-columns: repeat(2, minmax(0, 1fr)); gap: 8px; } .kpi-cell { width: auto; display: block; padding: 10px; } .kpi-value { font-size: 16pt; } .data-table { min-width: 620px; } }
        @media (max-width: 420px) { .kpi-grid { grid-template-columns: 1fr; } }
    </style>

    <div class="content-page">
        <a name="top"></a>
        <div class="page-title"><%= Localization.GetHtml("Analytics_Heading") %></div>
        <div class="page-divider"></div>

        <asp:Panel ID="pnlError" runat="server" CssClass="error-message" Visible="false">
            <asp:Literal ID="litError" runat="server" />
        </asp:Panel>

        <asp:Panel ID="pnlSuccess" runat="server" CssClass="success-message" Visible="false">
            <asp:Literal ID="litSuccess" runat="server" />
        </asp:Panel>

        <div class="action-bar">
            <asp:Button ID="btnRefresh" runat="server" Text="Refresh Data" CssClass="action-button" OnClick="btnRefresh_Click" />
            <asp:Button ID="btnExport" runat="server" Text="Export Summary" CssClass="action-button" OnClick="btnExport_Click" />
        </div>

        <!-- KPI Grid -->
        <div class="section-header">Autopark Overview</div>
        <div class="kpi-grid">
            <div class="kpi-cell">
                <div class="kpi-label">Active Buses</div>
                <div class="kpi-value"><asp:Literal ID="litActiveBuses" runat="server" Text="0" /></div>
                <div class="kpi-sub">Operational fleet</div>
            </div>
            <div class="kpi-cell">
                <div class="kpi-label">Active Employees</div>
                <div class="kpi-value"><asp:Literal ID="litActiveEmployees" runat="server" Text="0" /></div>
                <div class="kpi-sub">Working staff</div>
            </div>
            <div class="kpi-cell">
                <div class="kpi-label">Active Routes</div>
                <div class="kpi-value"><asp:Literal ID="litActiveRoutes" runat="server" Text="0" /></div>
                <div class="kpi-sub">In service</div>
            </div>
            <div class="kpi-cell">
                <div class="kpi-label">Today's Trips</div>
                <div class="kpi-value"><asp:Literal ID="litTodaySchedules" runat="server" Text="0" /></div>
                <div class="kpi-sub">Scheduled today</div>
            </div>
        </div>

        <div class="kpi-grid">
            <div class="kpi-cell">
                <div class="kpi-label">Today's Sales</div>
                <div class="kpi-value"><asp:Literal ID="litTodaySales" runat="server" Text="0" /></div>
                <div class="kpi-sub">Transactions</div>
            </div>
            <div class="kpi-cell">
                <div class="kpi-label">Today's Revenue</div>
                <div class="kpi-value"><asp:Literal ID="litTodayRevenue" runat="server" Text="0.00" /></div>
                <div class="kpi-sub">Total amount</div>
            </div>
            <div class="kpi-cell">
                <div class="kpi-label">Need Attention</div>
                <div class="kpi-value"><asp:Literal ID="litNeedAttention" runat="server" Text="0" /></div>
                <div class="kpi-sub">Buses requiring service</div>
            </div>
            <div class="kpi-cell">
                <div class="kpi-label">Avg Sale</div>
                <div class="kpi-value"><asp:Literal ID="litAvgSale" runat="server" Text="0.00" /></div>
                <div class="kpi-sub">Per transaction</div>
            </div>
        </div>

        <!-- Sales by Channel -->
        <div class="section-header">Sales by Channel</div>
        <uc:ContentBox ID="cbSalesByChannel" runat="server" HeaderText="Revenue Distribution by Sales Channel" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <asp:Literal ID="litChannelChart" runat="server" />
                <asp:GridView ID="gvSalesByChannel" runat="server" AutoGenerateColumns="false" CssClass="data-table" GridLines="Both" OnRowDataBound="gvSalesByChannel_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="SaleChannel" HeaderText="Channel" />
                        <asp:BoundField DataField="SaleCount" HeaderText="Sales" />
                        <asp:BoundField DataField="TicketCount" HeaderText="Tickets Sold" />
                        <asp:BoundField DataField="AmountTotal" HeaderText="Total Revenue" DataFormatString="{0:F2}" />
                        <asp:BoundField DataField="AverageTicketPrice" HeaderText="Avg Price" DataFormatString="{0:F2}" />
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </uc:ContentBox>

        <!-- Sales by Route -->
        <div class="section-header">Sales by Route</div>
        <uc:ContentBox ID="cbSalesByRoute" runat="server" HeaderText="Top Performing Routes" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <asp:Literal ID="litRouteChart" runat="server" />
                <asp:GridView ID="gvSalesByRoute" runat="server" AutoGenerateColumns="false" CssClass="data-table" GridLines="Both" OnRowDataBound="gvSalesByRoute_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="RouteNum" HeaderText="Route #" />
                        <asp:BoundField DataField="RouteName" HeaderText="Route Name" />
                        <asp:BoundField DataField="SaleCount" HeaderText="Sales" />
                        <asp:BoundField DataField="TicketCount" HeaderText="Tickets Sold" />
                        <asp:BoundField DataField="AmountTotal" HeaderText="Total Revenue" DataFormatString="{0:F2}" />
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </uc:ContentBox>

        <!-- Sales by Employee -->
        <div class="section-header">Sales by Employee</div>
        <uc:ContentBox ID="cbSalesByEmployee" runat="server" HeaderText="Employee Performance" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <asp:Literal ID="litEmployeeChart" runat="server" />
                <asp:GridView ID="gvSalesByEmployee" runat="server" AutoGenerateColumns="false" CssClass="data-table" GridLines="Both" OnRowDataBound="gvSalesByEmployee_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="EmployeeName" HeaderText="Employee" />
                        <asp:BoundField DataField="JobTitle" HeaderText="Position" />
                        <asp:BoundField DataField="SaleCount" HeaderText="Sales" />
                        <asp:BoundField DataField="AmountTotal" HeaderText="Total Revenue" DataFormatString="{0:F2}" />
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </uc:ContentBox>

        <p align="right" style="margin-top: 20px;"><a href="#top" style="font-size: 8pt; color: #000000;">Back to top &#9650;</a></p>
    </div>

</asp:Content>
