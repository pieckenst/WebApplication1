<%@ Page Title="Payment Processing - Autopark Management System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Payments.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Sales.Payments" %>
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
    </style>

    <div class="content-page">
        <a name="top"></a>
        <div class="page-title">Payment Processing</div>
        <div class="page-divider"></div>

        <asp:Panel ID="pnlError" runat="server" CssClass="error-message" Visible="false">
            <asp:Literal ID="litError" runat="server" />
        </asp:Panel>

        <asp:Panel ID="pnlSuccess" runat="server" CssClass="success-message" Visible="false">
            <asp:Literal ID="litSuccess" runat="server" />
        </asp:Panel>

        <div class="stats-summary">
            <div class="stats-item">
                <span class="stats-label">Total Payments:</span>
                <asp:Literal ID="litTotalPayments" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label">Paid:</span>
                <asp:Literal ID="litPaid" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label">Pending:</span>
                <asp:Literal ID="litPending" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label">Errors:</span>
                <asp:Literal ID="litErrors" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label">Total Amount:</span>
                <asp:Literal ID="litTotalAmount" runat="server" Text="0.00" />
            </div>
        </div>

        <div class="action-bar">
            <asp:Button ID="btnNewPayment" runat="server" Text="New Payment" CssClass="action-button" OnClick="btnNewPayment_Click" />
            <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="action-button" OnClick="btnRefresh_Click" />
        </div>

        <uc:ContentBox ID="cbFilters" runat="server" HeaderText="Filter Payments" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <div class="filter-section">
                    <div class="filter-row">
                        <span class="filter-label">Status:</span>
                        <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlStatusFilter_SelectedIndexChanged">
                            <asp:ListItem Text="All" Value="" />
                            <asp:ListItem Text="Ожидает" Value="Ожидает" />
                            <asp:ListItem Text="Оплачена" Value="Оплачена" />
                            <asp:ListItem Text="Ошибка" Value="Ошибка" />
                            <asp:ListItem Text="Возврат" Value="Возврат" />
                        </asp:DropDownList>
                    </div>
                </div>
            </ContentTemplate>
        </uc:ContentBox>

        <uc:ContentBox ID="cbPaymentsList" runat="server" HeaderText="Payment Records" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <asp:GridView ID="gvPayments" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="20" AllowSorting="true" CssClass="data-table" GridLines="Both" PagerStyle-CssClass="pagination" OnPageIndexChanging="gvPayments_PageIndexChanging" OnSorting="gvPayments_Sorting" OnRowCommand="gvPayments_RowCommand" OnRowDataBound="gvPayments_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="PaymentId" HeaderText="Payment ID" SortExpression="payment_id" ReadOnly="true" />
                        <asp:BoundField DataField="SaleId" HeaderText="Sale ID" SortExpression="sale_id" />
                        <asp:BoundField DataField="PaymentDate" HeaderText="Date" SortExpression="payment_date" DataFormatString="{0:dd.MM.yyyy HH:mm}" />
                        <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="amount" DataFormatString="{0:F2}" />
                        <asp:BoundField DataField="PaymentMethod" HeaderText="Method" SortExpression="payment_method" />
                        <asp:TemplateField HeaderText="Status" SortExpression="payment_status">
                            <ItemTemplate>
                                <asp:Literal ID="litPayStatus" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="TransactionId" HeaderText="Transaction ID" SortExpression="transaction_id" />
                        <asp:TemplateField HeaderText="Control" SortExpression="control_status">
                            <ItemTemplate>
                                <asp:Literal ID="litCtrlStatus" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:Button ID="btnView" runat="server" Text="View" CommandName="ViewPayment" CommandArgument='<%# Eval("PaymentId") %>' CssClass="action-button" />
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
                        <span class="form-label">Sale ID <span class="form-required">*</span></span>
                        <asp:TextBox ID="txtSaleId" runat="server" CssClass="form-control" Width="120" />
                        <asp:RequiredFieldValidator ID="rfvSaleId" runat="server" ControlToValidate="txtSaleId" ErrorMessage="Sale ID is required" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="PaymentForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Amount <span class="form-required">*</span></span>
                        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" Width="120" />
                        <asp:RequiredFieldValidator ID="rfvAmount" runat="server" ControlToValidate="txtAmount" ErrorMessage="Amount is required" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="PaymentForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Method <span class="form-required">*</span></span>
                        <asp:DropDownList ID="ddlMethod" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Наличные" Value="Наличные" />
                            <asp:ListItem Text="Банковская карта" Value="Банковская карта" />
                            <asp:ListItem Text="Электронный кошелек" Value="Электронный кошелек" />
                            <asp:ListItem Text="QR-код" Value="QR-код" />
                            <asp:ListItem Text="Безналичный расчет" Value="Безналичный расчет" />
                        </asp:DropDownList>
                    </div>
                    <div class="form-row">
                        <span class="form-label">Transaction ID</span>
                        <asp:TextBox ID="txtTransactionId" runat="server" CssClass="form-control" MaxLength="80" />
                    </div>
                    <div class="form-row" style="margin-top: 15px;">
                        <asp:Button ID="btnSavePayment" runat="server" Text="Create Payment" CssClass="action-button" OnClick="btnSavePayment_Click" ValidationGroup="PaymentForm" />
                        <asp:Button ID="btnCancelPayment" runat="server" Text="Cancel" CssClass="action-button" OnClick="btnCancelPayment_Click" CausesValidation="false" />
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </uc:ContentBox>

        <!-- Payment Detail View -->
        <uc:ContentBox ID="cbPaymentDetail" runat="server" HeaderText="Payment Details" HeaderColor="Gray" ContentColor="Gray" Visible="false">
            <ContentTemplate>
                <asp:Literal ID="litPaymentDetail" runat="server" />
                <div style="margin-top: 15px;">
                    <asp:Button ID="btnDetailClose" runat="server" Text="Close" CssClass="action-button" OnClick="btnDetailClose_Click" />
                </div>
            </ContentTemplate>
        </uc:ContentBox>

        <p align="right" style="margin-top: 20px;"><a href="#top" style="font-size: 8pt; color: #000000;">Back to top &#9650;</a></p>
    </div>

</asp:Content>
