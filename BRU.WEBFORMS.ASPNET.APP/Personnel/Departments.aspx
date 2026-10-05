<%@ Page Title="Departments - Autopark Management System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Departments.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.Personnel.Departments" %>
<%@ Register TagPrefix="uc" TagName="ContentBox" Src="~/Controls/ContentBox.ascx" %>
<asp:Content ID="MainContent1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .content-page { padding: 10px; font-family: Tahoma, Verdana, Arial, sans-serif; }
        .page-title { font-size: 16pt; color: #000080; font-weight: bold; }
        .page-divider { height: 2px; background-color: #000080; margin: 5px 0 15px 0; }
        .action-bar { background-color: #EDF2FB; border: 1px solid #1447AE; padding: 10px; margin-bottom: 15px; }
        .action-button { display: inline-block; padding: 6px 12px; background-color: #1447AE; color: #FFFFFF; text-decoration: none; border: 1px solid #2459C3; border-radius: 3px; margin-right: 8px; margin-bottom: 5px; font-family: Tahoma, Arial, sans-serif; font-size: 9pt; }
        .form-row { margin-bottom: 12px; }
        .form-label { display: inline-block; width: 150px; font-weight: bold; font-size: 9pt; }
        .form-control { padding: 4px 8px; border: 1px solid #1447AE; font-family: Tahoma, Arial, sans-serif; font-size: 9pt; width: 300px; }
        .error-message { background-color: #FFE6E6; border: 1px solid #CC0000; color: #CC0000; padding: 10px; margin-bottom: 15px; font-size: 10pt; }
        .success-message { background-color: #E6FFE6; border: 1px solid #008000; color: #008000; padding: 10px; margin-bottom: 15px; font-size: 10pt; }
        .data-table { width: 100%; border-collapse: collapse; margin-top: 10px; font-size: 9pt; }
        .data-table th { background-color: #1447AE; color: #FFFFFF; padding: 8px; text-align: left; border: 1px solid #0A2E7A; }
        .data-table td { padding: 8px; border: 1px solid #CCCCCC; background-color: #FFFFFF; }
        .pagination { margin-top: 15px; text-align: right; }
    </style>
    <div class="content-page">
        <div class="page-title">Department Management</div>
        <div class="page-divider"></div>
        <asp:Label ID="lblError" runat="server" CssClass="error-message" Visible="false" />
        <asp:Label ID="lblSuccess" runat="server" CssClass="success-message" Visible="false" />
        <div class="action-bar">
            <asp:Button ID="btnNew" runat="server" Text="Add Department" CssClass="action-button" OnClick="btnNew_Click" CausesValidation="false" />
            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" MaxLength="100" placeholder="Search departments..." />
            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control"><asp:ListItem Text="All statuses" Value="" /><asp:ListItem Text="Active" Value="1" /><asp:ListItem Text="Inactive" Value="0" /></asp:DropDownList>
            <asp:Button ID="btnApply" runat="server" Text="Apply Filter" CssClass="action-button" OnClick="btnApply_Click" />
        </div>
        <uc:ContentBox ID="cbDepartmentEditor" runat="server" HeaderText="Add / Edit Department" HeaderColor="Blue" ContentColor="White" Visible="false">
            <ContentTemplate>
                <asp:Panel ID="pnlEditor" runat="server" DefaultButton="btnSave" Visible="false">
                    <asp:HiddenField ID="hidDepartmentId" runat="server" Value="0" />
                    <div class="form-row"><span class="form-label">Department name *</span><asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="100" /></div>
                    <div class="form-row"><span class="form-label">Department code *</span><asp:TextBox ID="txtCode" runat="server" CssClass="form-control" MaxLength="20" /></div>
                    <div class="form-row"><span class="form-label">Description</span><asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" MaxLength="500" /></div>
                    <div class="form-row"><asp:Button ID="btnSave" runat="server" Text="Save" CssClass="action-button" OnClick="btnSave_Click" /><asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="action-button" OnClick="btnCancel_Click" CausesValidation="false" /></div>
                </asp:Panel>
            </ContentTemplate>
        </uc:ContentBox>
        <uc:ContentBox ID="cbDepartmentList" runat="server" HeaderText="Departments" HeaderColor="Blue" ContentColor="White">
            <ContentTemplate>
                <asp:GridView ID="gvDepartments" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="25" CssClass="data-table" GridLines="Both" PagerStyle-CssClass="pagination" OnPageIndexChanging="gvDepartments_PageIndexChanging" OnRowCommand="gvDepartments_RowCommand" OnRowDataBound="gvDepartments_RowDataBound" EmptyDataText="No departments match the filters.">
                    <Columns>
                        <asp:BoundField DataField="DepartmentName" HeaderText="Department" />
                        <asp:BoundField DataField="DepartmentCode" HeaderText="Code" />
                        <asp:BoundField DataField="Description" HeaderText="Description" />
                        <asp:BoundField DataField="EmployeeCount" HeaderText="Employees" />
                        <asp:TemplateField HeaderText="Status"><ItemTemplate><%# Eval("StatusDescription") %></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField HeaderText="Actions"><ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" Text="Edit" CssClass="action-button" CommandName="EditDepartment" CommandArgument='<%# Eval("DepartmentId") %>' CausesValidation="false" />
                            <asp:LinkButton ID="btnToggle" runat="server" Text="Deactivate" CssClass="action-button" CommandName="ToggleDepartment" CommandArgument='<%# Eval("DepartmentId") %>' CausesValidation="false" />
                        </ItemTemplate></asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </uc:ContentBox>
    </div>
</asp:Content>
