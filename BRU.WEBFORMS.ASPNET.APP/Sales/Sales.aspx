<%@ Page Title="<%$ Resources:Strings, Auto_Sales_Sales_41 %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Sales.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Sales.SalesPage" %>
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
        .filter-control { display: inline-block; }
        .data-table { width: 100%; border-collapse: collapse; margin-top: 10px; font-size: 9pt; }
        .data-table th { background-color: #1447AE; color: #FFFFFF; padding: 8px; text-align: left; border: 1px solid #0A2E7A; }
        .data-table td { padding: 8px; border: 1px solid #CCCCCC; background-color: #FFFFFF; }
        .data-table tr:hover td { background-color: #EDF2FB; }
        .status-created { color: #0066CC; font-weight: bold; }
        .status-completed { color: #008000; font-weight: bold; }
        .status-cancelled { color: #CC0000; font-weight: bold; }
        .status-refunded { color: #993399; font-weight: bold; }
        .pay-pending { color: #FF6600; font-weight: bold; }
        .pay-paid { color: #008000; font-weight: bold; }
        .pay-error { color: #CC0000; font-weight: bold; }
        .pay-refunded { color: #993399; font-weight: bold; }
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
        .date-input { padding: 4px 8px; border: 1px solid #1447AE; font-family: Tahoma, Arial, sans-serif; font-size: 9pt; width: 120px; }
        .data-table { display: block; overflow-x: auto; white-space: nowrap; }
        @media (max-width: 760px) { .content-page { padding: 8px; } .page-title { font-size: 14pt; } .form-label, .filter-label { display: block; width: auto; margin: 0 0 4px; } .form-control, select, input[type='text'], input[type='date'] { width: 100%; max-width: 100%; box-sizing: border-box; } .action-bar { display: flex; flex-wrap: wrap; gap: 6px; } }
    </style>

    <div class="content-page">
        <a name="top"></a>
        <div class="page-title"><%= Localization.GetHtml("Auto_Sales_Sales_42") %></div>
        <div class="page-divider"></div>

        <asp:Panel ID="pnlError" runat="server" CssClass="error-message" Visible="false">
            <asp:Literal ID="litError" runat="server" />
        </asp:Panel>

        <asp:Panel ID="pnlSuccess" runat="server" CssClass="success-message" Visible="false">
            <asp:Literal ID="litSuccess" runat="server" />
        </asp:Panel>

        <div class="stats-summary">
            <div class="stats-item">
                <span class="stats-label"><%= Localization.GetHtml("Auto_Sales_Sales_43") %></span>
                <asp:Literal ID="litTotalSales" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label"><%= Localization.GetHtml("Auto_Sales_Sales_44") %></span>
                <asp:Literal ID="litCompleted" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label"><%= Localization.GetHtml("Auto_Sales_Sales_45") %></span>
                <asp:Literal ID="litTotalTickets" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label"><%= Localization.GetHtml("Auto_Sales_Sales_46") %></span>
                <asp:Literal ID="litTotalRevenue" runat="server" Text="0.00" />
            </div>
            <div class="stats-item">
                <span class="stats-label"><%= Localization.GetHtml("Auto_Sales_Sales_47") %></span>
                <asp:Literal ID="litAvgSale" runat="server" Text="0.00" />
            </div>
        </div>

        <div class="action-bar">
            <asp:Button ID="btnNewSale" runat="server" Text="<%$ Resources:Strings, Auto_Sales_Sales_1 %>" CssClass="action-button" OnClick="btnNewSale_Click" />
            <asp:Button ID="btnRefresh" runat="server" Text="<%$ Resources:Strings, Auto_Sales_Sales_2 %>" CssClass="action-button" OnClick="btnRefresh_Click" />
        </div>

        <uc:ContentBox ID="cbFilters" runat="server" HeaderText="Filter Sales" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <div class="filter-section">
                    <div class="filter-row">
                        <span class="filter-label"><%= Localization.GetHtml("Auto_Sales_Sales_48") %></span>
                        <span class="filter-control">
                            <asp:TextBox ID="txtDateFrom" runat="server" CssClass="date-input" placeholder="<%$ Resources:Strings, Auto_Sales_Sales_3 %>" />
                            <span style="margin: 0 5px;"><%= Localization.GetHtml("Auto_Sales_Sales_49") %></span>
                            <asp:TextBox ID="txtDateTo" runat="server" CssClass="date-input" placeholder="<%$ Resources:Strings, Auto_Sales_Sales_4 %>" />
                            <asp:Button ID="btnApplyDateRange" runat="server" Text="<%$ Resources:Strings, Auto_Sales_Sales_5 %>" CssClass="action-button" OnClick="btnApplyDateRange_Click" />
                        </span>
                    </div>
                    <div class="filter-row">
                        <span class="filter-label"><%= Localization.GetHtml("Auto_Sales_Sales_50") %></span>
                        <span class="filter-control">
                            <asp:DropDownList ID="ddlChannelFilter" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlChannelFilter_SelectedIndexChanged">
                                <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Sales_6 %>" Value="" />
                                <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Sales_7 %>" Value="Касса" />
                                <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Sales_8 %>" Value="Кондуктор" />
                                <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Sales_9 %>" Value="Валидатор" />
                                <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Sales_10 %>" Value="QR" />
                                <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Sales_11 %>" Value="Онлайн" />
                            </asp:DropDownList>
                        </span>
                    </div>
                    <div class="filter-row">
                        <span class="filter-label"><%= Localization.GetHtml("Auto_Sales_Sales_51") %></span>
                        <span class="filter-control">
                            <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlStatusFilter_SelectedIndexChanged">
                                <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Sales_12 %>" Value="" />
                                <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Sales_13 %>" Value="Создана" />
                                <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Sales_14 %>" Value="Завершена" />
                                <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Sales_15 %>" Value="Отменена" />
                                <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Sales_16 %>" Value="Возврат" />
                            </asp:DropDownList>
                        </span>
                    </div>
                </div>
            </ContentTemplate>
        </uc:ContentBox>

        <uc:ContentBox ID="cbSalesList" runat="server" HeaderText="Sales Records" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <asp:GridView ID="gvSales" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="20" AllowSorting="true" CssClass="data-table" GridLines="Both" PagerStyle-CssClass="pagination" OnPageIndexChanging="gvSales_PageIndexChanging" OnSorting="gvSales_Sorting" OnRowCommand="gvSales_RowCommand" OnRowDataBound="gvSales_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="SaleId" HeaderText="<%$ Resources:Strings, Auto_Sales_Sales_17 %>" SortExpression="sale_id" ReadOnly="true" />
                        <asp:BoundField DataField="SaleDate" HeaderText="<%$ Resources:Strings, Auto_Sales_Sales_18 %>" SortExpression="sale_date" DataFormatString="{0:dd.MM.yyyy HH:mm}" />
                        <asp:BoundField DataField="TicketName" HeaderText="<%$ Resources:Strings, Auto_Sales_Sales_19 %>" SortExpression="ticket_name" />
                        <asp:BoundField DataField="TicketType" HeaderText="<%$ Resources:Strings, Auto_Sales_Sales_20 %>" SortExpression="ticket_type" />
                        <asp:BoundField DataField="TicketQuantity" HeaderText="<%$ Resources:Strings, Auto_Sales_Sales_21 %>" SortExpression="ticket_quantity" />
                        <asp:BoundField DataField="SalePrice" HeaderText="<%$ Resources:Strings, Auto_Sales_Sales_22 %>" SortExpression="sale_price" DataFormatString="{0:F2}" />
                        <asp:BoundField DataField="SaleTotal" HeaderText="<%$ Resources:Strings, Auto_Sales_Sales_23 %>" SortExpression="sale_total" DataFormatString="{0:F2}" />
                        <asp:BoundField DataField="SaleChannel" HeaderText="<%$ Resources:Strings, Auto_Sales_Sales_24 %>" SortExpression="sale_channel" />
                        <asp:TemplateField HeaderText="<%$ Resources:Strings, Auto_Sales_Sales_25 %>" SortExpression="sale_status">
                            <ItemTemplate>
                                <asp:Literal ID="litSaleStatus" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Strings, Auto_Sales_Sales_26 %>" SortExpression="payment_status">
                            <ItemTemplate>
                                <asp:Literal ID="litPayStatus" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CashierName" HeaderText="<%$ Resources:Strings, Auto_Sales_Sales_27 %>" SortExpression="cashier_name" />
                        <asp:TemplateField HeaderText="<%$ Resources:Strings, Auto_Sales_Sales_28 %>">
                            <ItemTemplate>
                                <asp:Button ID="btnView" runat="server" Text="<%$ Resources:Strings, Auto_Sales_Sales_29 %>"<%= Localization.GetHtml("Auto_Sales_Sales_52") %><%# Eval("SaleId") %>' CssClass="action-button" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </uc:ContentBox>

        <div class="pagination">
            <asp:Literal ID="litPagination" runat="server" />
        </div>

        <!-- New Sale Form -->
        <uc:ContentBox ID="cbSaleForm" runat="server" HeaderText="Create New Sale" HeaderColor="Blue" ContentColor="White" Visible="false">
            <ContentTemplate>
                <asp:Panel ID="pnlSaleForm" runat="server" DefaultButton="btnSaveSale">
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Auto_Sales_Sales_53") %><span class="form-required">*</span></span>
                        <asp:DropDownList ID="ddlTicket" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvTicket" runat="server" ControlToValidate="ddlTicket" InitialValue="" ErrorMessage="<%$ Resources:Strings, Auto_Sales_Sales_30 %>" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="SaleForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Auto_Sales_Sales_54") %><span class="form-required">*</span></span>
                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" Width="80" Text="1" />
                        <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="txtQuantity" ErrorMessage="<%$ Resources:Strings, Auto_Sales_Sales_31 %>" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="SaleForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Auto_Sales_Sales_55") %><span class="form-required">*</span></span>
                        <asp:TextBox ID="txtSalePrice" runat="server" CssClass="form-control" Width="120" />
                        <asp:RequiredFieldValidator ID="rfvSalePrice" runat="server" ControlToValidate="txtSalePrice" ErrorMessage="<%$ Resources:Strings, Auto_Sales_Sales_32 %>" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="SaleForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Auto_Sales_Sales_56") %><span class="form-required">*</span></span>
                        <asp:DropDownList ID="ddlChannel" runat="server" CssClass="form-control">
                            <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Sales_33 %>" Value="Касса" />
                            <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Sales_34 %>" Value="Кондуктор" />
                            <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Sales_35 %>" Value="Валидатор" />
                            <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Sales_36 %>" Value="QR" />
                            <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Sales_37 %>" Value="Онлайн" />
                        </asp:DropDownList>
                    </div>
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Auto_Sales_Sales_57") %></span>
                        <asp:DropDownList ID="ddlCashier" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-row" style="margin-top: 15px;">
                        <asp:Button ID="btnSaveSale" runat="server" Text="<%$ Resources:Strings, Auto_Sales_Sales_38 %>" CssClass="action-button" OnClick="btnSaveSale_Click" ValidationGroup="SaleForm" />
                        <asp:Button ID="btnCancelSale" runat="server" Text="<%$ Resources:Strings, Auto_Sales_Sales_39 %>" CssClass="action-button" OnClick="btnCancelSale_Click" CausesValidation="false" />
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </uc:ContentBox>

        <!-- Sale Detail View -->
        <uc:ContentBox ID="cbSaleDetail" runat="server" HeaderText="Sale Details" HeaderColor="Gray" ContentColor="Gray" Visible="false">
            <ContentTemplate>
                <asp:Literal ID="litSaleDetail" runat="server" />
                <div style="margin-top: 15px;">
                    <asp:Button ID="btnDetailClose" runat="server" Text="<%$ Resources:Strings, Auto_Sales_Sales_40 %>" CssClass="action-button" OnClick="btnDetailClose_Click" />
                </div>
            </ContentTemplate>
        </uc:ContentBox>

        <p align="right" style="margin-top: 20px;"><a href="#top" style="font-size: 8pt; color: #000000;"><%= Localization.GetHtml("Auto_Sales_Sales_58") %></a></p>
    </div>

</asp:Content>
