<%@ Page Title="<%$ Resources:Strings, Auto_Sales_Payments_28 %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Payments.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Sales.Payments" %>
<%@ Register TagPrefix="uc" TagName="ContentBox" Src="~/Controls/ContentBox.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        .content-page { padding: 18px; font-family: Tahoma, Verdana, Arial, sans-serif; max-width: 1480px; margin: 0 auto; box-sizing: border-box; }
        .content-page > * + * { margin-top: 16px; }
        .content-page input, .content-page select, .content-page textarea { box-sizing: border-box; max-width: 100%; }
        .action-bar { display: flex; flex-wrap: wrap; align-items: center; gap: 8px; padding: 12px; margin-bottom: 18px; }
        .data-table { display: block; overflow-x: auto; white-space: nowrap; }
        @media (max-width: 760px) { .content-page { padding: 10px; } .data-table { font-size: 8pt; } }
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
        .pay-pending { color: #FF6600; font-weight: bold; }
        .pay-paid { color: #008000; font-weight: bold; }
        .pay-error { color: #CC0000; font-weight: bold; }
        .pay-refunded { color: #993399; font-weight: bold; }
        .ctrl-ok { color: #008000; font-weight: bold; }
        .ctrl-pending { color: #FF6600; font-weight: bold; }
        .ctrl-error { color: #CC0000; font-weight: bold; }
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
        .data-table { display: block; overflow-x: auto; white-space: nowrap; }
        @media (max-width: 760px) { .content-page { padding: 8px; } .page-title { font-size: 14pt; } .form-label { display: block; width: auto; margin: 0 0 4px; } .form-control, select, input[type='text'], input[type='date'] { width: 100%; max-width: 100%; box-sizing: border-box; } .action-bar { display: flex; flex-wrap: wrap; gap: 6px; } }
    </style>

    <div class="content-page">
        <a name="top"></a>
        <div class="page-title"><%= Localization.GetHtml("Auto_Sales_Payments_29") %></div>
        <div class="page-divider"></div>

        <asp:Panel ID="pnlError" runat="server" CssClass="error-message" Visible="false">
            <asp:Literal ID="litError" runat="server" />
        </asp:Panel>

        <asp:Panel ID="pnlSuccess" runat="server" CssClass="success-message" Visible="false">
            <asp:Literal ID="litSuccess" runat="server" />
        </asp:Panel>

        <div class="stats-summary">
            <div class="stats-item">
                <span class="stats-label"><%= Localization.GetHtml("Auto_Sales_Payments_30") %></span>
                <asp:Literal ID="litTotalPayments" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label"><%= Localization.GetHtml("Auto_Sales_Payments_31") %></span>
                <asp:Literal ID="litPaid" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label"><%= Localization.GetHtml("Auto_Sales_Payments_32") %></span>
                <asp:Literal ID="litPending" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label"><%= Localization.GetHtml("Auto_Sales_Payments_33") %></span>
                <asp:Literal ID="litErrors" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label"><%= Localization.GetHtml("Auto_Sales_Payments_34") %></span>
                <asp:Literal ID="litTotalAmount" runat="server" Text="0.00" />
            </div>
        </div>

        <div class="action-bar">
            <asp:Button ID="btnNewPayment" runat="server" Text="<%$ Resources:Strings, Auto_Sales_Payments_1 %>" CssClass="action-button" OnClick="btnNewPayment_Click" />
            <asp:Button ID="btnRefresh" runat="server" Text="<%$ Resources:Strings, Auto_Sales_Payments_2 %>" CssClass="action-button" OnClick="btnRefresh_Click" />
        </div>

        <uc:ContentBox ID="cbFilters" runat="server" HeaderText="Filter Payments" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <div class="filter-section">
                    <div class="filter-row">
                        <span class="filter-label"><%= Localization.GetHtml("Auto_Sales_Payments_35") %></span>
                        <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlStatusFilter_SelectedIndexChanged">
                            <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Payments_3 %>" Value="" />
                            <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Payments_4 %>" Value="Ожидает" />
                            <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Payments_5 %>" Value="Оплачена" />
                            <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Payments_6 %>" Value="Ошибка" />
                            <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Payments_7 %>" Value="Возврат" />
                        </asp:DropDownList>
                    </div>
                </div>
            </ContentTemplate>
        </uc:ContentBox>

        <uc:ContentBox ID="cbPaymentsList" runat="server" HeaderText="Payment Records" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <asp:GridView ID="gvPayments" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="20" AllowSorting="true" CssClass="data-table" GridLines="Both" PagerStyle-CssClass="pagination" OnPageIndexChanging="gvPayments_PageIndexChanging" OnSorting="gvPayments_Sorting" OnRowCommand="gvPayments_RowCommand" OnRowDataBound="gvPayments_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="PaymentId" HeaderText="<%$ Resources:Strings, Auto_Sales_Payments_8 %>" SortExpression="payment_id" ReadOnly="true" />
                        <asp:BoundField DataField="SaleId" HeaderText="<%$ Resources:Strings, Auto_Sales_Payments_9 %>" SortExpression="sale_id" />
                        <asp:BoundField DataField="PaymentDate" HeaderText="<%$ Resources:Strings, Auto_Sales_Payments_10 %>" SortExpression="payment_date" DataFormatString="{0:dd.MM.yyyy HH:mm}" />
                        <asp:BoundField DataField="Amount" HeaderText="<%$ Resources:Strings, Auto_Sales_Payments_11 %>" SortExpression="amount" DataFormatString="{0:F2}" />
                        <asp:BoundField DataField="PaymentMethod" HeaderText="<%$ Resources:Strings, Auto_Sales_Payments_12 %>" SortExpression="payment_method" />
                        <asp:TemplateField HeaderText="<%$ Resources:Strings, Auto_Sales_Payments_13 %>" SortExpression="payment_status">
                            <ItemTemplate>
                                <asp:Literal ID="litPayStatus" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="TransactionId" HeaderText="<%$ Resources:Strings, Auto_Sales_Payments_14 %>" SortExpression="transaction_id" />
                        <asp:TemplateField HeaderText="<%$ Resources:Strings, Auto_Sales_Payments_15 %>" SortExpression="control_status">
                            <ItemTemplate>
                                <asp:Literal ID="litCtrlStatus" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Strings, Auto_Sales_Payments_16 %>">
                            <ItemTemplate>
                                <asp:Button ID="btnView" runat="server" Text="<%$ Resources:Strings, Auto_Sales_Payments_17 %>"<%= Localization.GetHtml("Auto_Sales_Payments_36") %><%# Eval("PaymentId") %>' CssClass="action-button" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </uc:ContentBox>

        <div class="pagination">
            <asp:Literal ID="litPagination" runat="server" />
        </div>

        <!-- New Payment Form -->
        <uc:ContentBox ID="cbPaymentForm" runat="server" HeaderText="Create New Payment" HeaderColor="Blue" ContentColor="White" Visible="false">
            <ContentTemplate>
                <asp:Panel ID="pnlPaymentForm" runat="server" DefaultButton="btnSavePayment">
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Auto_Sales_Payments_37") %><span class="form-required">*</span></span>
                        <asp:TextBox ID="txtSaleId" runat="server" CssClass="form-control" Width="120" />
                        <asp:RequiredFieldValidator ID="rfvSaleId" runat="server" ControlToValidate="txtSaleId" ErrorMessage="<%$ Resources:Strings, Auto_Sales_Payments_18 %>" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="PaymentForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Auto_Sales_Payments_38") %><span class="form-required">*</span></span>
                        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" Width="120" />
                        <asp:RequiredFieldValidator ID="rfvAmount" runat="server" ControlToValidate="txtAmount" ErrorMessage="<%$ Resources:Strings, Auto_Sales_Payments_19 %>" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="PaymentForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Auto_Sales_Payments_39") %><span class="form-required">*</span></span>
                        <asp:DropDownList ID="ddlMethod" runat="server" CssClass="form-control">
                            <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Payments_20 %>" Value="Наличные" />
                            <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Payments_21 %>" Value="Банковская карта" />
                            <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Payments_22 %>" Value="Электронный кошелек" />
                            <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Payments_23 %>" Value="QR-код" />
                            <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Payments_24 %>" Value="Безналичный расчет" />
                        </asp:DropDownList>
                    </div>
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Auto_Sales_Payments_40") %></span>
                        <asp:TextBox ID="txtTransactionId" runat="server" CssClass="form-control" MaxLength="80" />
                    </div>
                    <div class="form-row" style="margin-top: 15px;">
                        <asp:Button ID="btnSavePayment" runat="server" Text="<%$ Resources:Strings, Auto_Sales_Payments_25 %>" CssClass="action-button" OnClick="btnSavePayment_Click" ValidationGroup="PaymentForm" />
                        <asp:Button ID="btnCancelPayment" runat="server" Text="<%$ Resources:Strings, Auto_Sales_Payments_26 %>" CssClass="action-button" OnClick="btnCancelPayment_Click" CausesValidation="false" />
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </uc:ContentBox>

        <!-- Payment Detail View -->
        <uc:ContentBox ID="cbPaymentDetail" runat="server" HeaderText="Payment Details" HeaderColor="Gray" ContentColor="Gray" Visible="false">
            <ContentTemplate>
                <asp:Literal ID="litPaymentDetail" runat="server" />
                <div style="margin-top: 15px;">
                    <asp:Button ID="btnDetailClose" runat="server" Text="<%$ Resources:Strings, Auto_Sales_Payments_27 %>" CssClass="action-button" OnClick="btnDetailClose_Click" />
                </div>
            </ContentTemplate>
        </uc:ContentBox>

        <p align="right" style="margin-top: 20px;"><a href="#top" style="font-size: 8pt; color: #000000;"><%= Localization.GetHtml("Auto_Sales_Payments_41") %></a></p>
    </div>

</asp:Content>
