<%@ Page Title="Sales Reports - Autopark Management System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SalesReports.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Reports.SalesReports" %>
<%@ Register TagPrefix="uc" TagName="ContentBox" Src="~/Controls/ContentBox.ascx" %>
<asp:Content ID="MainContent1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .content-page { padding: 10px; font-family: Tahoma, Verdana, Arial, sans-serif; }
        .page-title { font-size: 16pt; color: #000080; font-weight: bold; }
        .page-divider { height: 2px; background-color: #000080; margin: 5px 0 15px 0; }
        .action-bar { background-color: #EDF2FB; border: 1px solid #1447AE; padding: 10px; margin-bottom: 15px; }
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
    </style>
    <div class="content-page">
        <div class="page-title">Sales Reports</div>
        <div class="page-divider"></div>
        <asp:Label ID="lblError" runat="server" CssClass="error-message" Visible="false" />
        <uc:ContentBox ID="cbSalesFilters" runat="server" HeaderText="Filter Options" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <div class="filter-section">
                    <div class="filter-row"><span class="filter-label">Sale date from:</span><asp:TextBox ID="txtDateFrom" runat="server" TextMode="Date" CssClass="form-control" /></div>
                    <div class="filter-row"><span class="filter-label">Sale date through:</span><asp:TextBox ID="txtDateTo" runat="server" TextMode="Date" CssClass="form-control" /></div>
                    <div class="filter-row"><span class="filter-label">Channel:</span><asp:DropDownList ID="ddlChannel" runat="server" CssClass="form-control"><asp:ListItem Text="All channels" Value="" /><asp:ListItem Text="Касса" Value="Касса" /><asp:ListItem Text="Кондуктор" Value="Кондуктор" /><asp:ListItem Text="Валидатор" Value="Валидатор" /><asp:ListItem Text="QR" Value="QR" /><asp:ListItem Text="Онлайн" Value="Онлайн" /></asp:DropDownList></div>
                    <div class="filter-row"><span class="filter-label">Sale status:</span><asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control"><asp:ListItem Text="All statuses" Value="" /><asp:ListItem Text="Завершена" Value="Завершена" /><asp:ListItem Text="Отменена" Value="Отменена" /><asp:ListItem Text="Возврат" Value="Возврат" /></asp:DropDownList></div>
                    <asp:Button ID="btnApply" runat="server" Text="Apply Filters" CssClass="action-button" OnClick="btnApply_Click" />
                    <asp:Button ID="btnExport" runat="server" Text="Export CSV" CssClass="action-button" OnClick="btnExport_Click" CausesValidation="false" />
                </div>
            </ContentTemplate>
        </uc:ContentBox>
        <div class="stats-summary">
            <div class="stats-item"><span class="stats-label">Sales:</span> <asp:Literal ID="litSaleCount" runat="server" Text="0" /></div>
            <div class="stats-item"><span class="stats-label">Tickets:</span> <asp:Literal ID="litTicketCount" runat="server" Text="0" /></div>
            <div class="stats-item"><span class="stats-label">Gross:</span> <asp:Literal ID="litGross" runat="server" Text="0.00" /></div>
            <div class="stats-item"><span class="stats-label">Refunds:</span> <asp:Literal ID="litRefunds" runat="server" Text="0.00" /></div>
            <div class="stats-item"><span class="stats-label">Net:</span> <asp:Literal ID="litNet" runat="server" Text="0.00" /></div>
        </div>
        <uc:ContentBox ID="cbSalesList" runat="server" HeaderText="Sales Transactions" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <asp:GridView ID="gvSales" runat="server" AutoGenerateColumns="false" AllowPaging="false" CssClass="data-table" GridLines="Both" EmptyDataText="No sales match the selected filters.">
                    <Columns>
                        <asp:BoundField DataField="SaleDate" HeaderText="Sale time" DataFormatString="{0:dd.MM.yyyy HH:mm}" />
                        <asp:BoundField DataField="RouteNumber" HeaderText="Route" />
                        <asp:BoundField DataField="TicketName" HeaderText="Ticket" />
                        <asp:BoundField DataField="TicketQuantity" HeaderText="Qty" />
                        <asp:BoundField DataField="SalePrice" HeaderText="Unit price" DataFormatString="{0:N2}" />
                        <asp:BoundField DataField="TotalAmount" HeaderText="Total" DataFormatString="{0:N2}" />
                        <asp:BoundField DataField="SaleChannel" HeaderText="Channel" />
                        <asp:BoundField DataField="SaleStatus" HeaderText="Status" />
                        <asp:BoundField DataField="PaymentStatus" HeaderText="Payment" />
                    </Columns>
                </asp:GridView>
                <div class="pagination">
                    <asp:LinkButton ID="btnPreviousPage" runat="server" Text="Previous" OnClick="btnPreviousPage_Click" CausesValidation="false" />
                    <asp:Label ID="lblPageInfo" runat="server" />
                    <asp:LinkButton ID="btnNextPage" runat="server" Text="Next" OnClick="btnNextPage_Click" CausesValidation="false" />
                </div>
            </ContentTemplate>
        </uc:ContentBox>
    </div>
</asp:Content>
