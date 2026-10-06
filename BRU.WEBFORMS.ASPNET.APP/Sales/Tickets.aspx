<%@ Page Title="<%$ Resources:Strings, Auto_Sales_Tickets_32 %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Tickets.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Sales.Tickets" %>
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
        .search-box { display: inline-block; margin-left: 20px; }
        .search-input { padding: 4px 8px; border: 1px solid #1447AE; font-family: Tahoma, Arial, sans-serif; font-size: 9pt; }
        .filter-section { background-color: #F2F2F2; border: 1px solid #CCCCCC; padding: 10px; margin-bottom: 15px; }
        .filter-row { margin-bottom: 8px; }
        .filter-label { display: inline-block; width: 120px; font-weight: bold; font-size: 9pt; }
        .filter-control { display: inline-block; }
        .data-table { width: 100%; border-collapse: collapse; margin-top: 10px; font-size: 9pt; }
        .data-table th { background-color: #1447AE; color: #FFFFFF; padding: 8px; text-align: left; border: 1px solid #0A2E7A; }
        .data-table td { padding: 8px; border: 1px solid #CCCCCC; background-color: #FFFFFF; }
        .data-table tr:hover td { background-color: #EDF2FB; }
        .status-active { color: #008000; font-weight: bold; }
        .status-inactive { color: #CC0000; font-weight: bold; }
        .price-high { color: #CC0000; font-weight: bold; }
        .price-medium { color: #FF6600; font-weight: bold; }
        .price-low { color: #008000; font-weight: bold; }
        .stock-high { color: #008000; font-weight: bold; }
        .stock-medium { color: #FF6600; font-weight: bold; }
        .stock-low { color: #CC0000; font-weight: bold; }
        .stock-out { color: #CC0000; font-weight: bold; }
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
        @media (max-width: 760px) { .content-page { padding: 8px; } .page-title { font-size: 14pt; } .form-label, .filter-label { display: block; width: auto; margin: 0 0 4px; } .form-control, select, input[type='text'] { width: 100%; max-width: 100%; box-sizing: border-box; } .action-bar { display: flex; flex-wrap: wrap; gap: 6px; } }
    </style>

    <div class="content-page">
        <a name="top"></a>
        <div class="page-title"><%= Localization.GetHtml("Auto_Sales_Tickets_33") %></div>
        <div class="page-divider"></div>

        <asp:Panel ID="pnlError" runat="server" CssClass="error-message" Visible="false">
            <asp:Literal ID="litError" runat="server" />
        </asp:Panel>

        <asp:Panel ID="pnlSuccess" runat="server" CssClass="success-message" Visible="false">
            <asp:Literal ID="litSuccess" runat="server" />
        </asp:Panel>

        <div class="stats-summary">
            <div class="stats-item">
                <span class="stats-label"><%= Localization.GetHtml("Auto_Sales_Tickets_34") %></span>
                <asp:Literal ID="litTotalTickets" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label"><%= Localization.GetHtml("Auto_Sales_Tickets_35") %></span>
                <asp:Literal ID="litActiveTickets" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label"><%= Localization.GetHtml("Auto_Sales_Tickets_36") %></span>
                <asp:Literal ID="litLowStock" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label"><%= Localization.GetHtml("Auto_Sales_Tickets_37") %></span>
                <asp:Literal ID="litOutOfStock" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label"><%= Localization.GetHtml("Auto_Sales_Tickets_38") %></span>
                <asp:Literal ID="litAvgPrice" runat="server" Text="0.00" />
            </div>
        </div>

        <div class="action-bar">
            <asp:Button ID="btnAddTicket" runat="server" Text="<%$ Resources:Strings, Auto_Sales_Tickets_1 %>" CssClass="action-button" OnClick="btnAddTicket_Click" />
            <asp:Button ID="btnRefresh" runat="server" Text="<%$ Resources:Strings, Auto_Sales_Tickets_2 %>" CssClass="action-button" OnClick="btnRefresh_Click" />
            <div class="search-box">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="search-input" Placeholder="Search by name or type..." />
                <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Strings, Auto_Sales_Tickets_3 %>" CssClass="action-button" OnClick="btnSearch_Click" />
            </div>
        </div>

        <uc:ContentBox ID="cbFilters" runat="server" HeaderText="Filter Options" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <div class="filter-section">
                    <div class="filter-row">
                        <span class="filter-label"><%= Localization.GetHtml("Auto_Sales_Tickets_39") %></span>
                        <span class="filter-control">
                            <asp:DropDownList ID="ddlTypeFilter" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTypeFilter_SelectedIndexChanged">
                                <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Tickets_4 %>" Value="" />
                            </asp:DropDownList>
                        </span>
                    </div>
                    <div class="filter-row">
                        <span class="filter-label"><%= Localization.GetHtml("Auto_Sales_Tickets_40") %></span>
                        <span class="filter-control">
                            <asp:TextBox ID="txtPriceFrom" runat="server" CssClass="form-control" Width="80" Placeholder="From" />
                            <asp:TextBox ID="txtPriceTo" runat="server" CssClass="form-control" Width="80" Placeholder="To" />
                            <asp:Button ID="btnApplyPriceFilter" runat="server" Text="<%$ Resources:Strings, Auto_Sales_Tickets_5 %>" CssClass="action-button" OnClick="btnApplyPriceFilter_Click" />
                        </span>
                    </div>
                    <div class="filter-row">
                        <span class="filter-label"><%= Localization.GetHtml("Auto_Sales_Tickets_41") %></span>
                        <span class="filter-control">
                            <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlStatusFilter_SelectedIndexChanged">
                                <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Tickets_6 %>" Value="" />
                                <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Tickets_7 %>" Value="1" />
                                <asp:ListItem Text="<%$ Resources:Strings, Auto_Sales_Tickets_8 %>" Value="0" />
                            </asp:DropDownList>
                        </span>
                    </div>
                </div>
            </ContentTemplate>
        </uc:ContentBox>

        <uc:ContentBox ID="cbTicketsList" runat="server" HeaderText="Ticket Types" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <asp:GridView ID="gvTickets" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="20" AllowSorting="true" CssClass="data-table" GridLines="Both" PagerStyle-CssClass="pagination" OnPageIndexChanging="gvTickets_PageIndexChanging" OnSorting="gvTickets_Sorting" OnRowCommand="gvTickets_RowCommand" OnRowDataBound="gvTickets_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="TicketId" HeaderText="<%$ Resources:Strings, Auto_Sales_Tickets_9 %>" SortExpression="ticket_id" ReadOnly="true" />
                        <asp:BoundField DataField="TicketName" HeaderText="<%$ Resources:Strings, Auto_Sales_Tickets_10 %>" SortExpression="ticket_name" />
                        <asp:BoundField DataField="TicketType" HeaderText="<%$ Resources:Strings, Auto_Sales_Tickets_11 %>" SortExpression="ticket_type" />
                        <asp:BoundField DataField="Zone" HeaderText="<%$ Resources:Strings, Auto_Sales_Tickets_12 %>" />
                        <asp:TemplateField HeaderText="<%$ Resources:Strings, Auto_Sales_Tickets_13 %>" SortExpression="price">
                            <ItemTemplate>
                                <asp:Literal ID="litPrice" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ValidDays" HeaderText="<%$ Resources:Strings, Auto_Sales_Tickets_14 %>" SortExpression="valid_days" />
                        <asp:TemplateField HeaderText="<%$ Resources:Strings, Auto_Sales_Tickets_15 %>" SortExpression="available_count">
                            <ItemTemplate>
                                <asp:Literal ID="litStock" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="IssueDate" HeaderText="<%$ Resources:Strings, Auto_Sales_Tickets_16 %>" SortExpression="issue_date" DataFormatString="{0:dd.MM.yyyy}" />
                        <asp:BoundField DataField="ExpiryDate" HeaderText="<%$ Resources:Strings, Auto_Sales_Tickets_17 %>" SortExpression="expiry_date" DataFormatString="{0:dd.MM.yyyy}" />
                        <asp:TemplateField HeaderText="<%$ Resources:Strings, Auto_Sales_Tickets_18 %>" SortExpression="is_active">
                            <ItemTemplate>
                                <asp:Literal ID="litStatus" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Strings, Auto_Sales_Tickets_19 %>">
                            <ItemTemplate>
                                <asp:Button ID="btnView" runat="server" Text="<%$ Resources:Strings, Auto_Sales_Tickets_20 %>"<%= Localization.GetHtml("Auto_Sales_Tickets_42") %><%# Eval("TicketId") %>' CssClass="action-button" />
                                <asp:Button ID="btnEdit" runat="server" Text="<%$ Resources:Strings, Auto_Sales_Tickets_21 %>"<%= Localization.GetHtml("Auto_Sales_Tickets_43") %><%# Eval("TicketId") %>' CssClass="action-button" />
                                <asp:Button ID="btnDelete" runat="server" Text="<%$ Resources:Strings, Auto_Sales_Tickets_22 %>"<%= Localization.GetHtml("Auto_Sales_Tickets_44") %><%# Eval("TicketId") %><%= Localization.GetHtml("Auto_Sales_Tickets_45") %><%$ Resources:Strings, Common_DeleteTicketConfirm %>');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </uc:ContentBox>

        <div class="pagination">
            <asp:Literal ID="litPagination" runat="server" />
        </div>

        <!-- Add/Edit Ticket Form -->
        <uc:ContentBox ID="cbTicketForm" runat="server" HeaderText="Add / Edit Ticket Type" HeaderColor="Blue" ContentColor="White" Visible="false">
            <ContentTemplate>
                <asp:Panel ID="pnlTicketForm" runat="server" DefaultButton="btnSave">
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Auto_Sales_Tickets_46") %><span class="form-required">*</span></span>
                        <asp:TextBox ID="txtTicketName" runat="server" CssClass="form-control" MaxLength="120" />
                        <asp:RequiredFieldValidator ID="rfvTicketName" runat="server" ControlToValidate="txtTicketName" ErrorMessage="<%$ Resources:Strings, Auto_Sales_Tickets_23 %>" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="TicketForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Auto_Sales_Tickets_47") %><span class="form-required">*</span></span>
                        <asp:TextBox ID="txtTicketType" runat="server" CssClass="form-control" MaxLength="40" />
                        <asp:RequiredFieldValidator ID="rfvTicketType" runat="server" ControlToValidate="txtTicketType" ErrorMessage="<%$ Resources:Strings, Auto_Sales_Tickets_24 %>" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="TicketForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Auto_Sales_Tickets_48") %></span>
                        <asp:TextBox ID="txtZone" runat="server" CssClass="form-control" MaxLength="40" />
                    </div>
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Auto_Sales_Tickets_49") %><span class="form-required">*</span></span>
                        <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" Width="120" />
                        <asp:RequiredFieldValidator ID="rfvPrice" runat="server" ControlToValidate="txtPrice" ErrorMessage="<%$ Resources:Strings, Auto_Sales_Tickets_25 %>" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="TicketForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Auto_Sales_Tickets_50") %><span class="form-required">*</span></span>
                        <asp:TextBox ID="txtValidDays" runat="server" CssClass="form-control" Width="80" />
                        <asp:RequiredFieldValidator ID="rfvValidDays" runat="server" ControlToValidate="txtValidDays" ErrorMessage="<%$ Resources:Strings, Auto_Sales_Tickets_26 %>" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="TicketForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Auto_Sales_Tickets_51") %><span class="form-required">*</span></span>
                        <asp:TextBox ID="txtAvailableCount" runat="server" CssClass="form-control" Width="120" />
                        <asp:RequiredFieldValidator ID="rfvAvailableCount" runat="server" ControlToValidate="txtAvailableCount" ErrorMessage="<%$ Resources:Strings, Auto_Sales_Tickets_27 %>" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="TicketForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Auto_Sales_Tickets_52") %></span>
                        <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="form-control" Width="120" placeholder="<%$ Resources:Strings, Auto_Sales_Tickets_28 %>" />
                    </div>
                    <div class="form-row">
                        <span class="form-label"><%= Localization.GetHtml("Auto_Sales_Tickets_53") %></span>
                        <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                    </div>
                    <div class="form-row" style="margin-top: 15px;">
                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Strings, Auto_Sales_Tickets_29 %>" CssClass="action-button" OnClick="btnSave_Click" ValidationGroup="TicketForm" />
                        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Strings, Auto_Sales_Tickets_30 %>" CssClass="action-button" OnClick="btnCancel_Click" CausesValidation="false" />
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </uc:ContentBox>

        <!-- Ticket Detail View -->
        <uc:ContentBox ID="cbTicketDetail" runat="server" HeaderText="Ticket Details" HeaderColor="Gray" ContentColor="Gray" Visible="false">
            <ContentTemplate>
                <asp:Literal ID="litDetail" runat="server" />
                <div style="margin-top: 15px;">
                    <asp:Button ID="btnDetailClose" runat="server" Text="<%$ Resources:Strings, Auto_Sales_Tickets_31 %>" CssClass="action-button" OnClick="btnDetailClose_Click" />
                </div>
            </ContentTemplate>
        </uc:ContentBox>

        <p align="right" style="margin-top: 20px;"><a href="#top" style="font-size: 8pt; color: #000000;"><%= Localization.GetHtml("Auto_Sales_Tickets_54") %></a></p>
    </div>

</asp:Content>
