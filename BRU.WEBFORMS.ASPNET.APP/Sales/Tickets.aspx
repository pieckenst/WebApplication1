<%@ Page Title="Ticket Type Management - Autopark Management System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Tickets.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Sales.Tickets" %>
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
    </style>

    <div class="content-page">
        <a name="top"></a>
        <div class="page-title">Ticket Type Management</div>
        <div class="page-divider"></div>

        <asp:Panel ID="pnlError" runat="server" CssClass="error-message" Visible="false">
            <asp:Literal ID="litError" runat="server" />
        </asp:Panel>

        <asp:Panel ID="pnlSuccess" runat="server" CssClass="success-message" Visible="false">
            <asp:Literal ID="litSuccess" runat="server" />
        </asp:Panel>

        <div class="stats-summary">
            <div class="stats-item">
                <span class="stats-label">Total Ticket Types:</span>
                <asp:Literal ID="litTotalTickets" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label">Active:</span>
                <asp:Literal ID="litActiveTickets" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label">Low Stock:</span>
                <asp:Literal ID="litLowStock" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label">Out of Stock:</span>
                <asp:Literal ID="litOutOfStock" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label">Avg Price:</span>
                <asp:Literal ID="litAvgPrice" runat="server" Text="0.00" />
            </div>
        </div>

        <div class="action-bar">
            <asp:Button ID="btnAddTicket" runat="server" Text="Add New Ticket Type" CssClass="action-button" OnClick="btnAddTicket_Click" />
            <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="action-button" OnClick="btnRefresh_Click" />
            <div class="search-box">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="search-input" Placeholder="Search by name or type..." />
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="action-button" OnClick="btnSearch_Click" />
            </div>
        </div>

        <uc:ContentBox ID="cbFilters" runat="server" HeaderText="Filter Options" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <div class="filter-section">
                    <div class="filter-row">
                        <span class="filter-label">Ticket Type:</span>
                        <span class="filter-control">
                            <asp:DropDownList ID="ddlTypeFilter" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTypeFilter_SelectedIndexChanged">
                                <asp:ListItem Text="All Types" Value="" />
                            </asp:DropDownList>
                        </span>
                    </div>
                    <div class="filter-row">
                        <span class="filter-label">Price Range:</span>
                        <span class="filter-control">
                            <asp:TextBox ID="txtPriceFrom" runat="server" CssClass="form-control" Width="80" Placeholder="From" />
                            <asp:TextBox ID="txtPriceTo" runat="server" CssClass="form-control" Width="80" Placeholder="To" />
                            <asp:Button ID="btnApplyPriceFilter" runat="server" Text="Apply" CssClass="action-button" OnClick="btnApplyPriceFilter_Click" />
                        </span>
                    </div>
                    <div class="filter-row">
                        <span class="filter-label">Status:</span>
                        <span class="filter-control">
                            <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlStatusFilter_SelectedIndexChanged">
                                <asp:ListItem Text="All" Value="" />
                                <asp:ListItem Text="Active" Value="1" />
                                <asp:ListItem Text="Inactive" Value="0" />
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
                        <asp:BoundField DataField="TicketId" HeaderText="ID" SortExpression="ticket_id" ReadOnly="true" />
                        <asp:BoundField DataField="TicketName" HeaderText="Ticket Name" SortExpression="ticket_name" />
                        <asp:BoundField DataField="TicketType" HeaderText="Type" SortExpression="ticket_type" />
                        <asp:BoundField DataField="Zone" HeaderText="Zone" />
                        <asp:TemplateField HeaderText="Price" SortExpression="price">
                            <ItemTemplate>
                                <asp:Literal ID="litPrice" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ValidDays" HeaderText="Valid (days)" SortExpression="valid_days" />
                        <asp:TemplateField HeaderText="Available" SortExpression="available_count">
                            <ItemTemplate>
                                <asp:Literal ID="litStock" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="IssueDate" HeaderText="Issue Date" SortExpression="issue_date" DataFormatString="{0:dd.MM.yyyy}" />
                        <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" SortExpression="expiry_date" DataFormatString="{0:dd.MM.yyyy}" />
                        <asp:TemplateField HeaderText="Status" SortExpression="is_active">
                            <ItemTemplate>
                                <asp:Literal ID="litStatus" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:Button ID="btnView" runat="server" Text="View" CommandName="ViewTicket" CommandArgument='<%# Eval("TicketId") %>' CssClass="action-button" />
                                <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="EditTicket" CommandArgument='<%# Eval("TicketId") %>' CssClass="action-button" />
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="DeleteTicket" CommandArgument='<%# Eval("TicketId") %>' CssClass="action-button" OnClientClick="return confirm('Are you sure you want to delete this ticket type?');" />
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
                        <span class="form-label">Ticket Name <span class="form-required">*</span></span>
                        <asp:TextBox ID="txtTicketName" runat="server" CssClass="form-control" MaxLength="120" />
                        <asp:RequiredFieldValidator ID="rfvTicketName" runat="server" ControlToValidate="txtTicketName" ErrorMessage="Name is required" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="TicketForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Ticket Type <span class="form-required">*</span></span>
                        <asp:TextBox ID="txtTicketType" runat="server" CssClass="form-control" MaxLength="40" />
                        <asp:RequiredFieldValidator ID="rfvTicketType" runat="server" ControlToValidate="txtTicketType" ErrorMessage="Type is required" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="TicketForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Zone</span>
                        <asp:TextBox ID="txtZone" runat="server" CssClass="form-control" MaxLength="40" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Price <span class="form-required">*</span></span>
                        <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" Width="120" />
                        <asp:RequiredFieldValidator ID="rfvPrice" runat="server" ControlToValidate="txtPrice" ErrorMessage="Price is required" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="TicketForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Valid Days <span class="form-required">*</span></span>
                        <asp:TextBox ID="txtValidDays" runat="server" CssClass="form-control" Width="80" />
                        <asp:RequiredFieldValidator ID="rfvValidDays" runat="server" ControlToValidate="txtValidDays" ErrorMessage="Valid days required" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="TicketForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Available Count <span class="form-required">*</span></span>
                        <asp:TextBox ID="txtAvailableCount" runat="server" CssClass="form-control" Width="120" />
                        <asp:RequiredFieldValidator ID="rfvAvailableCount" runat="server" ControlToValidate="txtAvailableCount" ErrorMessage="Count is required" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="TicketForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Expiry Date</span>
                        <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="form-control" Width="120" placeholder="dd.MM.yyyy" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Active</span>
                        <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                    </div>
                    <div class="form-row" style="margin-top: 15px;">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="action-button" OnClick="btnSave_Click" ValidationGroup="TicketForm" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="action-button" OnClick="btnCancel_Click" CausesValidation="false" />
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </uc:ContentBox>

        <!-- Ticket Detail View -->
        <uc:ContentBox ID="cbTicketDetail" runat="server" HeaderText="Ticket Details" HeaderColor="Gray" ContentColor="Gray" Visible="false">
            <ContentTemplate>
                <asp:Literal ID="litDetail" runat="server" />
                <div style="margin-top: 15px;">
                    <asp:Button ID="btnDetailClose" runat="server" Text="Close" CssClass="action-button" OnClick="btnDetailClose_Click" />
                </div>
            </ContentTemplate>
        </uc:ContentBox>

        <p align="right" style="margin-top: 20px;"><a href="#top" style="font-size: 8pt; color: #000000;">Back to top &#9650;</a></p>
    </div>

</asp:Content>
