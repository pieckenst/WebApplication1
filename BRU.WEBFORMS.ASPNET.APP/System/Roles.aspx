<%@ Page Title="<%$ Resources:Strings, AdminRoles_Heading %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Roles.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.SystemPages.Roles" %>
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
        .admin-page input[type=text],.admin-page select,.admin-page textarea { max-width:100%; box-sizing:border-box; border:1px solid #829bc8; padding:6px 8px; font:9pt Tahoma,Arial,sans-serif; }
        .admin-page select { height:auto; min-height:30px; padding:6px 30px 6px 8px; background-position:right 8px center; background-size:16px 16px; line-height:1.2; }
        .admin-page select:active { background-position:right 8px center; background-size:16px 16px; }
        .admin-page select:focus { background-position:right 8px center; background-size:16px 16px; }
        .search-input { width:310px; }
        .form-grid { display:grid; grid-template-columns:repeat(2,minmax(240px,1fr)); gap:14px 18px; }
        .form-field { display:flex; flex-direction:column; align-items:flex-start; gap:5px; min-width:0; }
        .form-field > input,.form-field > textarea { width:100%; }
        .field-label { font-weight:bold; font-size:9pt; color:#202d44; }
        .field-hint { color:#666; font-size:8pt; line-height:1.4; }
        .permissions-section { grid-column:1 / -1; border-top:1px solid #ddd; padding-top:12px; }
        .permission-list { display:grid; grid-template-columns:repeat(auto-fit,minmax(300px,1fr)); gap:7px 14px; width:100%; margin-top:8px; }
        .permission-list label { margin-left:4px; }
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
        .table-wrap { overflow-x:auto; border:1px solid #a7b9d9; }
        .data-table { width:100%; border-collapse:collapse; font-size:9pt; }
        .data-table th { background:#1447ae; color:#fff; text-align:left; padding:8px; border:1px solid #0a2e7a; }
        .data-table td { background:#fff; vertical-align:top; padding:8px; border:1px solid #d0d0d0; }
        .data-table tr:hover td { background:#f5f8ff; }
        .status-pill { display:inline-block; padding:3px 7px; border-radius:2px; font-size:8pt; font-weight:bold; }
        .status-active { background:#e4f6e4; color:#176417; border:1px solid #a8d5a8; }
        .status-inactive { background:#f5e5e5; color:#8a2020; border:1px solid #d9b0b0; }
        .system-pill { display:inline-block; background:#fff0d7; color:#7c4a00; border:1px solid #e4c28a; padding:3px 6px; font-size:8pt; }
        @media (max-width:760px) { .admin-page { padding:9px; } .form-grid { grid-template-columns:minmax(0,1fr); } .permissions-section { grid-column:auto; } .search-input { width:100%; } .data-table { min-width:760px; } }
    </style>
    <div class="admin-page">
        <div class="page-title"><%= Localization.GetHtml("AdminRoles_Heading") %></div>
        <div class="page-divider"></div>
        <p class="page-intro"><%= Localization.GetHtml("AdminRoles_Intro") %></p>
        <asp:Panel ID="pnlError" runat="server" CssClass="message message-error" Visible="false"><asp:Literal ID="litError" runat="server" /></asp:Panel>
        <asp:Panel ID="pnlSuccess" runat="server" CssClass="message message-success" Visible="false"><asp:Literal ID="litSuccess" runat="server" /></asp:Panel>

        <div class="stats-grid">
            <div class="stat-card"><span class="stat-label"><%= Localization.GetHtml("AdminRoles_Total") %></span><span class="stat-value"><asp:Literal ID="litTotalRoles" runat="server" Text="0" /></span></div>
            <div class="stat-card"><span class="stat-label"><%= Localization.GetHtml("AdminRoles_ActiveCount") %></span><span class="stat-value"><asp:Literal ID="litActiveRoles" runat="server" Text="0" /></span></div>
            <div class="stat-card"><span class="stat-label"><%= Localization.GetHtml("AdminRoles_InactiveCount") %></span><span class="stat-value"><asp:Literal ID="litInactiveRoles" runat="server" Text="0" /></span></div>
        </div>

        <div class="toolbar">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="search-input" MaxLength="100" placeholder="<%$ Resources:Strings, AdminRoles_SearchPlaceholder %>" />
            <asp:DropDownList ID="ddlRoleStatus" runat="server">
                <asp:ListItem Text="<%$ Resources:Strings, AdminRoles_AllStatuses %>" Value="" />
                <asp:ListItem Text="<%$ Resources:Strings, AdminRoles_Active %>" Value="active" />
                <asp:ListItem Text="<%$ Resources:Strings, AdminRoles_Inactive %>" Value="inactive" />
            </asp:DropDownList>
            <asp:Button ID="btnApplyFilters" runat="server" CssClass="action-button secondary" Text="<%$ Resources:Strings, AdminRoles_Apply %>" OnClick="btnApplyFilters_Click" />
            <asp:Button ID="btnNewRole" runat="server" CssClass="action-button" Text="<%$ Resources:Strings, AdminRoles_New %>" OnClick="btnNewRole_Click" />
        </div>

        <asp:Panel ID="pnlEditor" runat="server" CssClass="admin-panel" Visible="false">
            <div class="panel-heading"><asp:Literal ID="litEditorTitle" runat="server" /></div>
            <asp:HiddenField ID="hfRoleId" runat="server" Value="0" />
            <div class="form-grid">
                <div class="form-field">
                    <span class="field-label"><%= Localization.GetHtml("AdminRoles_Name") %></span>
                    <asp:TextBox ID="txtRoleName" runat="server" MaxLength="50" autocomplete="off" />
                    <span class="field-hint"><%= Localization.GetHtml("AdminRoles_NameHint") %></span>
                </div>
                <div class="form-field">
                    <span class="field-label"><%= Localization.GetHtml("AdminRoles_Description") %></span>
                    <asp:TextBox ID="txtRoleDescription" runat="server" MaxLength="300" TextMode="MultiLine" Rows="3" />
                </div>
                <div class="form-field">
                    <div><asp:CheckBox ID="chkRoleActive" runat="server" /><span class="field-label"><%= Localization.GetHtml("AdminRoles_Active") %></span></div>
                    <span class="field-hint"><%= Localization.GetHtml("AdminRoles_ActiveHint") %></span>
                </div>
                <div class="form-field permissions-section">
                    <span class="field-label"><%= Localization.GetHtml("AdminRoles_Permissions") %></span>
                    <span class="field-hint"><%= Localization.GetHtml("AdminRoles_PermissionsHint") %></span>
                    <asp:CheckBoxList ID="cblPermissions" runat="server" CssClass="permission-list" RepeatLayout="Flow" RepeatDirection="Vertical" />
                    <asp:Panel ID="pnlSystemRoleNote" runat="server" CssClass="message message-info" Visible="false"><asp:Literal ID="litSystemRoleNote" runat="server" /></asp:Panel>
                </div>
            </div>
            <div class="form-actions">
                <asp:Button ID="btnSaveRole" runat="server" CssClass="action-button" Text="<%$ Resources:Strings, AdminCommon_Save %>" OnClick="btnSaveRole_Click" />
                <asp:Button ID="btnCancelEdit" runat="server" CssClass="action-button secondary" Text="<%$ Resources:Strings, AdminCommon_Cancel %>" CausesValidation="false" OnClick="btnCancelEdit_Click" />
            </div>
        </asp:Panel>

        <div class="table-wrap">
            <asp:GridView ID="gvRoles" runat="server" CssClass="data-table" AutoGenerateColumns="false" GridLines="None" DataKeyNames="RoleId"
                EmptyDataText="<%$ Resources:Strings, AdminRoles_Empty %>" OnRowCommand="gvRoles_RowCommand" OnRowDataBound="gvRoles_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="RoleId" HeaderText="ID" />
                    <asp:BoundField DataField="RoleName" HeaderText="<%$ Resources:Strings, AdminRoles_Name %>" />
                    <asp:BoundField DataField="Description" HeaderText="<%$ Resources:Strings, AdminRoles_Description %>" />
                    <asp:BoundField DataField="UserCount" HeaderText="<%$ Resources:Strings, AdminRoles_UserCount %>" />
                    <asp:BoundField DataField="PermissionCount" HeaderText="<%$ Resources:Strings, AdminRoles_PermissionCount %>" />
                    <asp:TemplateField HeaderText="<%$ Resources:Strings, AdminRoles_Status %>"><ItemTemplate><asp:Label ID="lblRoleStatus" runat="server" /></ItemTemplate></asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Strings, AdminCommon_Actions %>">
                        <ItemTemplate>
                            <asp:Button ID="btnEditRole" runat="server" CssClass="action-button" Text="<%$ Resources:Strings, AdminCommon_Edit %>" CommandName="EditRole" CommandArgument='<%# Eval("RoleId") %>' CausesValidation="false" />
                            <asp:Button ID="btnDeleteRole" runat="server" CssClass="action-button danger" Text="<%$ Resources:Strings, AdminCommon_Delete %>" CommandName="DeleteRole" CommandArgument='<%# Eval("RoleId") %>' CausesValidation="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>