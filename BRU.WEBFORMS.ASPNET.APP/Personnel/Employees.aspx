<%@ Page Title="Employee Management - Autopark Management System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Employees.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Personnel.Employees" %>
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
        .status-working { color: #008000; font-weight: bold; }
        .status-vacation { color: #FF6600; font-weight: bold; }
        .status-sickleave { color: #CC6600; font-weight: bold; }
        .status-dismissed { color: #CC0000; font-weight: bold; }
        .pagination { margin-top: 15px; text-align: right; }
        .pagination a { padding: 4px 8px; margin: 0 2px; background-color: #EDF2FB; border: 1px solid #1447AE; text-decoration: none; font-size: 9pt; }
        .pagination a:hover { background-color: #1447AE; color: #FFFFFF; }
        .pagination .current { padding: 4px 8px; margin: 0 2px; background-color: #1447AE; color: #FFFFFF; font-weight: bold; }
        .modal { display: none; position: fixed; top: 0; left: 0; width: 100%; height: 100%; background-color: rgba(0,0,0,0.5); z-index: 1000; }
        .modal-content { background-color: #FFFFFF; margin: 5% auto; padding: 20px; width: 650px; border: 2px solid #1447AE; border-radius: 5px; }
        .modal-header { background-color: #1447AE; color: #FFFFFF; padding: 10px; font-weight: bold; margin: -20px -20px 20px -20px; border-radius: 3px 3px 0 0; }
        .modal-body { padding: 15px 0; }
        .modal-footer { padding: 15px 0 0 0; text-align: right; }
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
        <div class="page-title">Employee Management</div>
        <div class="page-divider"></div>

        <asp:Panel ID="pnlError" runat="server" CssClass="error-message" Visible="false">
            <asp:Literal ID="litError" runat="server" />
        </asp:Panel>

        <asp:Panel ID="pnlSuccess" runat="server" CssClass="success-message" Visible="false">
            <asp:Literal ID="litSuccess" runat="server" />
        </asp:Panel>

        <div class="stats-summary">
            <div class="stats-item">
                <span class="stats-label">Total Employees:</span>
                <asp:Literal ID="litTotalEmployees" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label">Active:</span>
                <asp:Literal ID="litActiveEmployees" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label">On Vacation:</span>
                <asp:Literal ID="litVacationEmployees" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label">Sick Leave:</span>
                <asp:Literal ID="litSickEmployees" runat="server" Text="0" />
            </div>
            <div class="stats-item">
                <span class="stats-label">Dismissed:</span>
                <asp:Literal ID="litDismissedEmployees" runat="server" Text="0" />
            </div>
        </div>

        <div class="action-bar">
            <asp:Button ID="btnAddEmployee" runat="server" Text="Add New Employee" CssClass="action-button" OnClick="btnAddEmployee_Click" />
            <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="action-button" OnClick="btnRefresh_Click" />
            <div class="search-box">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="search-input" Placeholder="Search by name or phone..." />
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="action-button" OnClick="btnSearch_Click" />
            </div>
        </div>

        <uc:ContentBox ID="cbFilters" runat="server" HeaderText="Filter Options" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <div class="filter-section">
                    <div class="filter-row">
                        <span class="filter-label">Status:</span>
                        <span class="filter-control">
                            <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlStatusFilter_SelectedIndexChanged">
                                <asp:ListItem Text="All Statuses" Value="" />
                                <asp:ListItem Text="Working" Value="Работает" />
                                <asp:ListItem Text="On Vacation" Value="Отпуск" />
                                <asp:ListItem Text="Sick Leave" Value="Больничный" />
                                <asp:ListItem Text="Dismissed" Value="Уволен" />
                            </asp:DropDownList>
                        </span>
                    </div>
                    <div class="filter-row">
                        <span class="filter-label">Job:</span>
                        <span class="filter-control">
                            <asp:DropDownList ID="ddlJobFilter" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlJobFilter_SelectedIndexChanged">
                                <asp:ListItem Text="All Jobs" Value="" />
                            </asp:DropDownList>
                        </span>
                    </div>
                    <div class="filter-row">
                        <span class="filter-label">Department:</span>
                        <span class="filter-control">
                            <asp:DropDownList ID="ddlDepartmentFilter" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlDepartmentFilter_SelectedIndexChanged">
                                <asp:ListItem Text="All Departments" Value="" />
                            </asp:DropDownList>
                        </span>
                    </div>
                </div>
            </ContentTemplate>
        </uc:ContentBox>

        <uc:ContentBox ID="cbEmployeesList" runat="server" HeaderText="Personnel Directory" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <asp:GridView ID="gvEmployees" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="20" AllowSorting="true" CssClass="data-table" GridLines="Both" PagerStyle-CssClass="pagination" OnPageIndexChanging="gvEmployees_PageIndexChanging" OnSorting="gvEmployees_Sorting" OnRowCommand="gvEmployees_RowCommand" OnRowDataBound="gvEmployees_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="EmployeeId" HeaderText="ID" SortExpression="employee_id" ReadOnly="true" />
                        <asp:BoundField DataField="EmployeeName" HeaderText="Full Name" SortExpression="employee_name" />
                        <asp:BoundField DataField="JobTitle" HeaderText="Job Title" SortExpression="job_title" />
                        <asp:BoundField DataField="DepartmentName" HeaderText="Department" SortExpression="department_name" />
                        <asp:BoundField DataField="ServiceYears" HeaderText="Years" SortExpression="service_years" />
                        <asp:TemplateField HeaderText="Status" SortExpression="status">
                            <ItemTemplate>
                                <asp:Literal ID="litStatus" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Phone" HeaderText="Phone" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:Button ID="btnView" runat="server" Text="View" CommandName="View" CommandArgument='<%# Eval("EmployeeId") %>' CssClass="action-button" />
                                <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="EditEmp" CommandArgument='<%# Eval("EmployeeId") %>' CssClass="action-button" />
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="DeleteEmp" CommandArgument='<%# Eval("EmployeeId") %>' CssClass="action-button" OnClientClick="return confirm('Are you sure you want to delete this employee?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </uc:ContentBox>

        <div class="pagination">
            <asp:Literal ID="litPagination" runat="server" />
        </div>

        <!-- Add/Edit Employee Modal -->
        <uc:ContentBox ID="cbEmployeeForm" runat="server" HeaderText="Add / Edit Employee" HeaderColor="Blue" ContentColor="White" Visible="false">
            <ContentTemplate>
                <asp:Panel ID="pnlEmployeeForm" runat="server" DefaultButton="btnSave">
                    <div class="form-row">
                        <span class="form-label">Surname <span class="form-required">*</span></span>
                        <asp:TextBox ID="txtSurname" runat="server" CssClass="form-control" MaxLength="60" />
                        <asp:RequiredFieldValidator ID="rfvSurname" runat="server" ControlToValidate="txtSurname" ErrorMessage="Surname is required" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="EmployeeForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Name <span class="form-required">*</span></span>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="60" />
                        <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" ErrorMessage="Name is required" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="EmployeeForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Patronymic</span>
                        <asp:TextBox ID="txtPatronym" runat="server" CssClass="form-control" MaxLength="60" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Employed Date <span class="form-required">*</span></span>
                        <asp:TextBox ID="txtEmployedDate" runat="server" CssClass="form-control" placeholder="dd.MM.yyyy" />
                        <asp:RequiredFieldValidator ID="rfvEmployedDate" runat="server" ControlToValidate="txtEmployedDate" ErrorMessage="Date is required" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="EmployeeForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Job <span class="form-required">*</span></span>
                        <asp:DropDownList ID="ddlJob" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvJob" runat="server" ControlToValidate="ddlJob" InitialValue="" ErrorMessage="Job is required" Display="Dynamic" ForeColor="#CC0000" Font-Size="8pt" ValidationGroup="EmployeeForm" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Department</span>
                        <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Phone</span>
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" MaxLength="30" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Email</span>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="120" />
                    </div>
                    <div class="form-row">
                        <span class="form-label">Status <span class="form-required">*</span></span>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Working" Value="Работает" />
                            <asp:ListItem Text="On Vacation" Value="Отпуск" />
                            <asp:ListItem Text="Sick Leave" Value="Больничный" />
                            <asp:ListItem Text="Dismissed" Value="Уволен" />
                        </asp:DropDownList>
                    </div>
                    <div class="form-row" style="margin-top: 15px;">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="action-button" OnClick="btnSave_Click" ValidationGroup="EmployeeForm" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="action-button" OnClick="btnCancel_Click" CausesValidation="false" />
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </uc:ContentBox>

        <!-- Employee Detail View -->
        <uc:ContentBox ID="cbEmployeeDetail" runat="server" HeaderText="Employee Details" HeaderColor="Gray" ContentColor="Gray" Visible="false">
            <ContentTemplate>
                <asp:Literal ID="litDetailSurname" runat="server" />
                <asp:Literal ID="litDetailName" runat="server" />
                <asp:Literal ID="litDetailPatronym" runat="server" />
                <asp:Literal ID="litDetailEmployedDate" runat="server" />
                <asp:Literal ID="litDetailJob" runat="server" />
                <asp:Literal ID="litDetailDepartment" runat="server" />
                <asp:Literal ID="litDetailPhone" runat="server" />
                <asp:Literal ID="litDetailEmail" runat="server" />
                <asp:Literal ID="litDetailStatus" runat="server" />
                <asp:Literal ID="litDetailServiceYears" runat="server" />
                <div style="margin-top: 15px;">
                    <asp:Button ID="btnDetailClose" runat="server" Text="Close" CssClass="action-button" OnClick="btnDetailClose_Click" />
                </div>
            </ContentTemplate>
        </uc:ContentBox>

        <p align="right" style="margin-top: 20px;"><a href="#top" style="font-size: 8pt; color: #000000;">Back to top &#9650;</a></p>
    </div>

</asp:Content>
