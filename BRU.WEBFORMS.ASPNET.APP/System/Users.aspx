<%@ Page Title="<%$ Resources:Strings, AdminUsers_Heading %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.SystemPages.Users" %>
<asp:Content ID="MainContent1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .admin-page { padding:18px; max-width:1500px; margin:0 auto; box-sizing:border-box; font-family:Tahoma,Verdana,Arial,sans-serif; color:#111; }
        .page-title { color:#000080; font-size:16pt; font-weight:bold; }
        .page-divider { height:2px; background:#000080; margin:7px 0 12px; }
        .page-intro { color:#444; font-size:10pt; line-height:1.5; margin:0 0 14px; }
        .stats-grid { display:grid; grid-template-columns:repeat(auto-fit,minmax(165px,1fr)); gap:12px; margin-bottom:16px; }
        .stat-card { background:#edf2fb; border:1px solid #9bb5e8; padding:12px 14px; min-height:70px; box-sizing:border-box; }
        .stat-label { display:block; color:#1447ae; font-size:9pt; font-weight:bold; margin-bottom:8px; }
        .stat-value { font-size:21pt; color:#000080; font-weight:bold; }
        .toolbar,.admin-panel { background:#f1f4fa; border:1px solid #a7b9d9; padding:12px; margin-bottom:16px; }
        .toolbar { display:flex; flex-wrap:wrap; align-items:center; gap:8px; }
        .admin-panel { background:#fff; }
        .panel-heading { background:#1447ae; color:#fff; font-weight:bold; margin:-12px -12px 14px; padding:9px 12px; }
        .admin-page input[type=text],.admin-page input[type=password],.admin-page input[type=email],.admin-page select,.admin-page textarea { max-width:100%; box-sizing:border-box; border:1px solid #829bc8; padding:6px 8px; font:9pt Tahoma,Arial,sans-serif; }
        .search-input { width:310px; }
        .form-grid { display:grid; grid-template-columns:repeat(2,minmax(240px,1fr)); gap:14px 18px; }
        .form-field { display:flex; flex-direction:column; align-items:flex-start; gap:5px; min-width:0; }
        .form-field > input,.form-field > select,.form-field > textarea { width:100%; }
        .field-label { font-weight:bold; font-size:9pt; color:#202d44; }
        .field-hint { color:#666; font-size:8pt; line-height:1.4; }
        .role-checklist { display:grid; grid-template-columns:repeat(auto-fit,minmax(230px,1fr)); gap:6px 14px; width:100%; }
        .role-checklist label { margin-left:4px; }
        .check-field { display:flex; align-items:flex-start; gap:7px; }
        .role-section { grid-column:1 / -1; border-top:1px solid #ddd; padding-top:12px; }
        .form-actions { display:flex; flex-wrap:wrap; gap:8px; margin-top:15px; }
        .action-button { display:inline-block; border:1px solid #2459c3; border-radius:2px; background:#1447ae; color:#fff; padding:7px 12px; font:9pt Tahoma,Arial,sans-serif; text-decoration:none; cursor:pointer; }
        .action-button:hover { background:#2459c3; }
        .action-button.secondary { background:#e7edf8; color:#123b85; border-color:#9bb5e8; }
        .action-button.danger { background:#a52222; border-color:#831919; }
        .action-button:disabled { opacity:.55; cursor:not-allowed; }
        .message { padding:10px 12px; margin:10px 0 15px; font-size:9pt; border:1px solid; }
        .message-error { background:#ffe8e8; border-color:#cc3333; color:#8a1111; }
        .message-success { background:#e8f7e8; border-color:#5a9d5a; color:#155b15; }
        .message-info { background:#edf4ff; border-color:#8aa8d8; color:#163a70; }
        .data-table { width:100%; border-collapse:collapse; font-size:9pt; }
        .data-table th { background:#1447ae; color:#fff; text-align:left; padding:8px; border:1px solid #0a2e7a; }
        .data-table td { background:#fff; vertical-align:top; padding:8px; border:1px solid #d0d0d0; }
        .data-table tr:hover td { background:#f5f8ff; }
        .table-wrap { overflow-x:auto; border:1px solid #a7b9d9; }
        .status-pill { display:inline-block; padding:3px 7px; border-radius:2px; font-size:8pt; font-weight:bold; }
        .status-active { background:#e4f6e4; color:#176417; border:1px solid #a8d5a8; }
        .status-inactive { background:#f5e5e5; color:#8a2020; border:1px solid #d9b0b0; }
        .role-chip { display:inline-block; background:#eaf0fc; border:1px solid #c4d3ef; color:#173e81; padding:2px 5px; margin:0 3px 3px 0; font-size:8pt; }
        .role-chip.inactive { background:#f1f1f1; border-color:#d2d2d2; color:#666; }
        .muted { color:#666; font-size:8pt; }
        @media (max-width:760px) { .admin-page { padding:9px; } .form-grid { grid-template-columns:minmax(0,1fr); } .role-section { grid-column:auto; } .search-input { width:100%; } .data-table { min-width:980px; } }
    </style>
    <div class="admin-page">
        <div class="page-title"><%= Localization.GetHtml("AdminUsers_Heading") %></div>
        <div class="page-divider"></div>
        <p class="page-intro"><%= Localization.GetHtml("AdminUsers_Intro") %></p>
        <asp:Panel ID="pnlError" runat="server" CssClass="message message-error" Visible="false"><asp:Literal ID="litError" runat="server" /></asp:Panel>
        <asp:Panel ID="pnlSuccess" runat="server" CssClass="message message-success" Visible="false"><asp:Literal ID="litSuccess" runat="server" /></asp:Panel>

        <div class="stats-grid">
            <div class="stat-card"><span class="stat-label"><%= Localization.GetHtml("AdminUsers_Total") %></span><span class="stat-value"><asp:Literal ID="litTotalUsers" runat="server" Text="0" /></span></div>
            <div class="stat-card"><span class="stat-label"><%= Localization.GetHtml("AdminUsers_ActiveCount") %></span><span class="stat-value"><asp:Literal ID="litActiveUsers" runat="server" Text="0" /></span></div>
            <div class="stat-card"><span class="stat-label"><%= Localization.GetHtml("AdminUsers_InactiveCount") %></span><span class="stat-value"><asp:Literal ID="litInactiveUsers" runat="server" Text="0" /></span></div>
            <div class="stat-card"><span class="stat-label"><%= Localization.GetHtml("AdminUsers_Administrators") %></span><span class="stat-value"><asp:Literal ID="litAdminUsers" runat="server" Text="0" /></span></div>
        </div>

        <div class="toolbar">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="search-input" MaxLength="120" placeholder="<%$ Resources:Strings, AdminUsers_SearchPlaceholder %>" />
            <asp:DropDownList ID="ddlStatusFilter" runat="server">
                <asp:ListItem Text="<%$ Resources:Strings, AdminUsers_AllStatuses %>" Value="" />
                <asp:ListItem Text="<%$ Resources:Strings, AdminUsers_Active %>" Value="active" />
                <asp:ListItem Text="<%$ Resources:Strings, AdminUsers_Inactive %>" Value="inactive" />
            </asp:DropDownList>
            <asp:Button ID="btnApplyFilters" runat="server" CssClass="action-button secondary" Text="<%$ Resources:Strings, AdminUsers_Apply %>" OnClick="btnApplyFilters_Click" />
            <asp:Button ID="btnNewUser" runat="server" CssClass="action-button" Text="<%$ Resources:Strings, AdminUsers_New %>" OnClick="btnNewUser_Click" />
        </div>

        <asp:Panel ID="pnlEditor" runat="server" CssClass="admin-panel" Visible="false">
            <div class="panel-heading"><asp:Literal ID="litEditorTitle" runat="server" /></div>
            <asp:HiddenField ID="hfUserId" runat="server" Value="0" />
            <div class="form-grid">
                <div class="form-field">
                    <span class="field-label"><%= Localization.GetHtml("AdminUsers_Login") %></span>
                    <asp:TextBox ID="txtLogin" runat="server" MaxLength="80" autocomplete="off" />
                    <span class="field-hint"><%= Localization.GetHtml("AdminUsers_LoginHint") %></span>
                </div>
                <div class="form-field">
                    <span class="field-label"><%= Localization.GetHtml("AdminUsers_Employee") %></span>
                    <asp:DropDownList ID="ddlEmployee" runat="server" />
                    <span class="field-hint"><%= Localization.GetHtml("AdminUsers_EmployeeHint") %></span>
                </div>
                <div class="form-field">
                    <span class="field-label"><%= Localization.GetHtml("AdminUsers_Email") %></span>
                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="150" TextMode="Email" />
                </div>
                <div class="form-field">
                    <span class="field-label"><%= Localization.GetHtml("AdminUsers_Phone") %></span>
                    <asp:TextBox ID="txtPhone" runat="server" MaxLength="30" />
                </div>
                <div class="form-field">
                    <span class="field-label"><%= Localization.GetHtml("AdminUsers_Password") %></span>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" MaxLength="100" autocomplete="new-password" />
                    <span class="field-hint"><%= Localization.GetHtml("AdminUsers_PasswordHint") %></span>
                    <span class="field-hint"><asp:Literal ID="litPasswordOptional" runat="server" /></span>
                </div>
                <div class="form-field">
                    <span class="field-label"><%= Localization.GetHtml("AdminUsers_ConfirmPassword") %></span>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" MaxLength="100" autocomplete="new-password" />
                </div>
                <div class="form-field check-field">
                    <asp:CheckBox ID="chkUserActive" runat="server" />
                    <span><span class="field-label"><%= Localization.GetHtml("AdminUsers_ActiveAccount") %></span><br /><span class="field-hint"><%= Localization.GetHtml("AdminUsers_ActiveHelp") %></span></span>
                </div>
                <div class="form-field role-section">
                    <span class="field-label"><%= Localization.GetHtml("AdminUsers_Roles") %></span>
                    <asp:CheckBoxList ID="cblUserRoles" runat="server" CssClass="role-checklist" RepeatLayout="Flow" RepeatDirection="Vertical" />
                    <span class="field-hint"><%= Localization.GetHtml("AdminUsers_RolesHint") %></span>
                    <asp:Panel ID="pnlInactiveRoleNote" runat="server" CssClass="message message-info" Visible="false"><asp:Literal ID="litInactiveRoleNote" runat="server" /></asp:Panel>
                </div>
            </div>
            <div class="form-actions">
                <asp:Button ID="btnSaveUser" runat="server" CssClass="action-button" Text="<%$ Resources:Strings, AdminCommon_Save %>" OnClick="btnSaveUser_Click" />
                <asp:Button ID="btnCancelEdit" runat="server" CssClass="action-button secondary" Text="<%$ Resources:Strings, AdminCommon_Cancel %>" CausesValidation="false" OnClick="btnCancelEdit_Click" />
            </div>
        </asp:Panel>

        <div class="table-wrap">
            <asp:GridView ID="gvUsers" runat="server" CssClass="data-table" AutoGenerateColumns="false" GridLines="None" AllowPaging="true" PageSize="20" DataKeyNames="UserId"
                EmptyDataText="<%$ Resources:Strings, AdminUsers_Empty %>" OnRowCommand="gvUsers_RowCommand" OnRowDataBound="gvUsers_RowDataBound" OnPageIndexChanging="gvUsers_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="UserId" HeaderText="ID" />
                    <asp:BoundField DataField="Login" HeaderText="<%$ Resources:Strings, AdminUsers_Login %>" />
                    <asp:BoundField DataField="EmployeeName" HeaderText="<%$ Resources:Strings, AdminUsers_Employee %>" />
                    <asp:BoundField DataField="Email" HeaderText="<%$ Resources:Strings, AdminUsers_Email %>" />
                    <asp:BoundField DataField="PhoneNumber" HeaderText="<%$ Resources:Strings, AdminUsers_Phone %>" />
                    <asp:TemplateField HeaderText="<%$ Resources:Strings, AdminUsers_Roles %>"><ItemTemplate><asp:Literal ID="litRowRoles" runat="server" /></ItemTemplate></asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Strings, AdminUsers_Status %>"><ItemTemplate><asp:Label ID="lblRowStatus" runat="server" /></ItemTemplate></asp:TemplateField>
                    <asp:BoundField DataField="CreatedAt" HeaderText="<%$ Resources:Strings, AdminUsers_Created %>" DataFormatString="{0:dd.MM.yyyy HH:mm}" />
                    <asp:BoundField DataField="LastLoginAt" HeaderText="<%$ Resources:Strings, AdminUsers_LastLogin %>" DataFormatString="{0:dd.MM.yyyy HH:mm}" NullDisplayText="—" />
                    <asp:TemplateField HeaderText="<%$ Resources:Strings, AdminCommon_Actions %>">
                        <ItemTemplate>
                            <asp:Button ID="btnEditUser" runat="server" CssClass="action-button" Text="<%$ Resources:Strings, AdminCommon_Edit %>" CommandName="EditUser" CommandArgument='<%# Eval("UserId") %>' CausesValidation="false" />
                            <asp:Button ID="btnToggleActive" runat="server" CssClass="action-button secondary" CommandName="ToggleActive" CommandArgument='<%# Eval("UserId") %>' CausesValidation="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>