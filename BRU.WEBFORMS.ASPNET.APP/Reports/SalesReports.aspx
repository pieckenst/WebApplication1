<%@ Page Title="<%$ Resources:Strings, Auto_Reports_SalesReports_25 %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SalesReports.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Reports.SalesReports" %>
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
        <div class="page-title"><%= Localization.GetHtml("Auto_Reports_SalesReports_26") %></div>
        <div class="page-divider"></div>
        <asp:Label ID="lblError" runat="server" CssClass="error-message" Visible="false" />
        <uc:ContentBox ID="cbSalesFilters" runat="server" HeaderText="Filter Options" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <div class="filter-section">
                    <div class="filter-row"><span class="filter-label"><%= Localization.GetHtml("Auto_Reports_SalesReports_27") %></span><asp:TextBox ID="txtDateFrom" runat="server" TextMode="Date" CssClass="form-control" /></div>
                    <div class="filter-row"><span class="filter-label"><%= Localization.GetHtml("Auto_Reports_SalesReports_28") %></span><asp:TextBox ID="txtDateTo" runat="server" TextMode="Date" CssClass="form-control" /></div>
                    <div class="filter-row"><span class="filter-label"><%= Localization.GetHtml("Auto_Reports_SalesReports_29") %></span><asp:DropDownList ID="ddlChannel" runat="server" CssClass="form-control"><asp:ListItem Text="<%$ Resources:Strings, Auto_Reports_SalesReports_1 %>" Value="" /><asp:ListItem Text="<%$ Resources:Strings, Auto_Reports_SalesReports_2 %>" Value="Касса" /><asp:ListItem Text="<%$ Resources:Strings, Auto_Reports_SalesReports_3 %>" Value="Кондуктор" /><asp:ListItem Text="<%$ Resources:Strings, Auto_Reports_SalesReports_4 %>" Value="Валидатор" /><asp:ListItem Text="<%$ Resources:Strings, Auto_Reports_SalesReports_5 %>" Value="QR" /><asp:ListItem Text="<%$ Resources:Strings, Auto_Reports_SalesReports_6 %>" Value="Онлайн" /></asp:DropDownList></div>
                    <div class="filter-row"><span class="filter-label"><%= Localization.GetHtml("Auto_Reports_SalesReports_30") %></span><asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control"><asp:ListItem Text="<%$ Resources:Strings, Auto_Reports_SalesReports_7 %>" Value="" /><asp:ListItem Text="<%$ Resources:Strings, Auto_Reports_SalesReports_8 %>" Value="Завершена" /><asp:ListItem Text="<%$ Resources:Strings, Auto_Reports_SalesReports_9 %>" Value="Отменена" /><asp:ListItem Text="<%$ Resources:Strings, Auto_Reports_SalesReports_10 %>" Value="Возврат" /></asp:DropDownList></div>
                    <asp:Button ID="btnApply" runat="server" Text="<%$ Resources:Strings, Auto_Reports_SalesReports_11 %>" CssClass="action-button" OnClick="btnApply_Click" />
                    <asp:Button ID="btnExport" runat="server" Text="<%$ Resources:Strings, Auto_Reports_SalesReports_12 %>" CssClass="action-button" OnClick="btnExport_Click" CausesValidation="false" />
                </div>
            </ContentTemplate>
        </uc:ContentBox>
        <div class="stats-summary">
            <div class="stats-item"><span class="stats-label"><%= Localization.GetHtml("Auto_Reports_SalesReports_31") %></span> <asp:Literal ID="litSaleCount" runat="server" Text="0" /></div>
            <div class="stats-item"><span class="stats-label"><%= Localization.GetHtml("Auto_Reports_SalesReports_32") %></span> <asp:Literal ID="litTicketCount" runat="server" Text="0" /></div>
            <div class="stats-item"><span class="stats-label"><%= Localization.GetHtml("Auto_Reports_SalesReports_33") %></span> <asp:Literal ID="litGross" runat="server" Text="0.00" /></div>
            <div class="stats-item"><span class="stats-label"><%= Localization.GetHtml("Auto_Reports_SalesReports_34") %></span> <asp:Literal ID="litRefunds" runat="server" Text="0.00" /></div>
            <div class="stats-item"><span class="stats-label"><%= Localization.GetHtml("Auto_Reports_SalesReports_35") %></span> <asp:Literal ID="litNet" runat="server" Text="0.00" /></div>
        </div>
        <uc:ContentBox ID="cbSalesList" runat="server" HeaderText="Sales Transactions" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <asp:GridView ID="gvSales" runat="server" AutoGenerateColumns="false" AllowPaging="false" CssClass="data-table" GridLines="Both" EmptyDataText="<%$ Resources:Strings, Auto_Reports_SalesReports_13 %>">
                    <Columns>
                        <asp:BoundField DataField="SaleDate" HeaderText="<%$ Resources:Strings, Auto_Reports_SalesReports_14 %>" DataFormatString="{0:dd.MM.yyyy HH:mm}" />
                        <asp:BoundField DataField="RouteNumber" HeaderText="<%$ Resources:Strings, Auto_Reports_SalesReports_15 %>" />
                        <asp:BoundField DataField="TicketName" HeaderText="<%$ Resources:Strings, Auto_Reports_SalesReports_16 %>" />
                        <asp:BoundField DataField="TicketQuantity" HeaderText="<%$ Resources:Strings, Auto_Reports_SalesReports_17 %>" />
                        <asp:BoundField DataField="SalePrice" HeaderText="<%$ Resources:Strings, Auto_Reports_SalesReports_18 %>" DataFormatString="{0:N2}" />
                        <asp:BoundField DataField="TotalAmount" HeaderText="<%$ Resources:Strings, Auto_Reports_SalesReports_19 %>" DataFormatString="{0:N2}" />
                        <asp:BoundField DataField="SaleChannel" HeaderText="<%$ Resources:Strings, Auto_Reports_SalesReports_20 %>" />
                        <asp:BoundField DataField="SaleStatus" HeaderText="<%$ Resources:Strings, Auto_Reports_SalesReports_21 %>" />
                        <asp:BoundField DataField="PaymentStatus" HeaderText="<%$ Resources:Strings, Auto_Reports_SalesReports_22 %>" />
                    </Columns>
                </asp:GridView>
                <div class="pagination">
                    <asp:LinkButton ID="btnPreviousPage" runat="server" Text="<%$ Resources:Strings, Auto_Reports_SalesReports_23 %>" OnClick="btnPreviousPage_Click" CausesValidation="false" />
                    <asp:Label ID="lblPageInfo" runat="server" />
                    <asp:LinkButton ID="btnNextPage" runat="server" Text="<%$ Resources:Strings, Auto_Reports_SalesReports_24 %>" OnClick="btnNextPage_Click" CausesValidation="false" />
                </div>
            </ContentTemplate>
        </uc:ContentBox>
    </div>
</asp:Content>
