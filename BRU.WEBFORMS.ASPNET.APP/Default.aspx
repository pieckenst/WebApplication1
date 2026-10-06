<%@ Page Title="Dashboard - Autopark Management System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Default" %>
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
        .dashboard-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
            gap: 15px;
            margin-top: 15px;
        }
        .stat-card {
            background-color: #EDF2FB;
            border: 1px solid #1447AE;
            padding: 15px;
            border-radius: 3px;
        }
        .stat-card-header {
            font-size: 10pt;
            font-weight: bold;
            color: #1447AE;
            margin-bottom: 10px;
        }
        .stat-card-value {
            font-size: 24pt;
            font-weight: bold;
            color: #000080;
        }
        .stat-card-subtext {
            font-size: 8pt;
            color: #666666;
            margin-top: 5px;
        }
        .status-good { color: #008000; }
        .status-warning { color: #FF6600; }
        .status-error { color: #CC0000; }
        .quick-actions {
            margin-top: 20px;
        }
        .action-button {
            display: inline-block;
            padding: 8px 16px;
            background-color: #1447AE;
            color: #FFFFFF;
            text-decoration: none;
            border: 1px solid #2459C3;
            border-radius: 3px;
            margin-right: 10px;
            margin-bottom: 10px;
            font-family: Tahoma, Arial, sans-serif;
            font-size: 9pt;
        }
        .action-button:hover {
            background-color: #2459C3;
        }
        .data-table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 15px;
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
        .error-message {
            background-color: #FFE6E6;
            border: 1px solid #CC0000;
            color: #CC0000;
            padding: 10px;
            margin-bottom: 15px;
            font-size: 10pt;
        }
        .alert-item {
            padding: 8px 0;
            border-bottom: 1px solid #CCCCCC;
        }
        .alert-item:last-child {
            border-bottom: none;
        }
        .alert-icon {
            margin-right: 8px;
        }
    </style>
    
    <div class="content-page">
        <a name="top"></a>
        <div class="page-title"><%= Localization.GetHtml("Default_Heading") %></div>
        <div class="page-divider"></div>
        
        <asp:Panel ID="pnlError" runat="server" CssClass="error-message" Visible="false">
            <asp:Literal ID="litError" runat="server" />
        </asp:Panel>
        
        <p class="text-regular" style="margin-top: 0; margin-bottom: 0;">
            <%= Localization.GetHtml("Default_Welcome") %>
            <br><br>
            <span class="text-small"><b><asp:Literal ID="litLastUpdated" runat="server" /></b></span>
        </p>
        
        <!-- Dashboard Statistics -->
        <div class="dashboard-grid">
            <div class="stat-card">
                <div class="stat-card-header"><%= Localization.GetHtml("Default_ActiveBuses") %></div>
                <div class="stat-card-value">
                    <asp:Literal ID="litActiveBuses" runat="server" Text="0" />
                </div>
                <div class="stat-card-subtext"><%= Localization.GetHtml("Default_OperationalFleet") %></div>
            </div>
            
            <div class="stat-card">
                <div class="stat-card-header"><%= Localization.GetHtml("Default_ActiveEmployees") %></div>
                <div class="stat-card-value">
                    <asp:Literal ID="litActiveEmployees" runat="server" Text="0" />
                </div>
                <div class="stat-card-subtext"><%= Localization.GetHtml("Default_CurrentlyWorking") %></div>
            </div>
            
            <div class="stat-card">
                <div class="stat-card-header"><%= Localization.GetHtml("Default_ActiveRoutes") %></div>
                <div class="stat-card-value">
                    <asp:Literal ID="litActiveRoutes" runat="server" Text="0" />
                </div>
                <div class="stat-card-subtext"><%= Localization.GetHtml("Default_RoutesInService") %></div>
            </div>
            
            <div class="stat-card">
                <div class="stat-card-header"><%= Localization.GetHtml("Default_TodaySchedule") %></div>
                <div class="stat-card-value">
                    <asp:Literal ID="litTodaySchedule" runat="server" Text="0" />
                </div>
                <div class="stat-card-subtext"><%= Localization.GetHtml("Default_ScheduledTripsToday") %></div>
            </div>
            
            <div class="stat-card">
                <div class="stat-card-header"><%= Localization.GetHtml("Default_TodaySales") %></div>
                <div class="stat-card-value">
                    <asp:Literal ID="litTodaySales" runat="server" Text="0" />
                </div>
                <div class="stat-card-subtext"><%= Localization.GetHtml("Default_TicketsSoldToday") %></div>
            </div>
            
            <div class="stat-card">
                <div class="stat-card-header"><%= Localization.GetHtml("Default_TodayRevenue") %></div>
                <div class="stat-card-value">
                    <asp:Literal ID="litTodayRevenue" runat="server" Text="0.00" />
                </div>
                <div class="stat-card-subtext"><%= Localization.GetHtml("Default_TotalRevenueByn") %></div>
            </div>
            
            <div class="stat-card">
                <div class="stat-card-header"><%= Localization.GetHtml("Default_BusesAttention") %></div>
                <div class="stat-card-value">
                    <asp:Literal ID="litBusesAttention" runat="server" Text="0" />
                </div>
                <div class="stat-card-subtext"><%= Localization.GetHtml("Default_MaintenanceRequired") %></div>
            </div>
        </div>
        
        <!-- System Alerts -->
        <uc:ContentBox ID="cbAlerts" runat="server" HeaderText="<%$ Resources:Strings, Default_SystemAlerts %>" HeaderColor="Red" ContentColor="Yellow" />
        
        <!-- Quick Actions -->
        <div class="quick-actions">
            <asp:HyperLink ID="hlBuses" runat="server" CssClass="action-button" NavigateUrl="~/Fleet/Buses.aspx" Text="<%$ Resources:Strings, Default_ManageBuses %>" />
            <asp:HyperLink ID="hlEmployees" runat="server" CssClass="action-button" NavigateUrl="~/Personnel/Employees.aspx" Text="<%$ Resources:Strings, Default_ManageEmployees %>" />
            <asp:HyperLink ID="hlRoutes" runat="server" CssClass="action-button" NavigateUrl="~/Operations/Routes.aspx" Text="<%$ Resources:Strings, Default_ManageRoutes %>" />
            <asp:HyperLink ID="hlSales" runat="server" CssClass="action-button" NavigateUrl="~/Sales/Sales.aspx" Text="<%$ Resources:Strings, Default_SalesTickets %>" />
            <asp:HyperLink ID="hlReports" runat="server" CssClass="action-button" NavigateUrl="~/Reports/Analytics.aspx" Text="<%$ Resources:Strings, Default_Reports %>" />
        </div>
        
        <!-- Recent Activity -->
        <uc:ContentBox ID="cbRecentActivity" runat="server" HeaderText="<%$ Resources:Strings, Default_RecentActivity %>" HeaderColor="Blue" ContentColor="White" />
        
        <!-- Maintenance Overview -->
        <uc:ContentBox ID="cbMaintenance" runat="server" HeaderText="<%$ Resources:Strings, Default_MaintenanceOverview %>" HeaderColor="Gray" ContentColor="Gray" />
        
        <p align="right" style="margin-top: 20px;"><a href="#top" style="font-size: 8pt; color: #000000;"><%= Localization.GetHtml("Common_BackToTop") %></a></p>
    </div>
    
</asp:Content>