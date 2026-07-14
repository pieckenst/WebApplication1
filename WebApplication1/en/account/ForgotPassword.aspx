<%@ Page Title="Reset Password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.en.account.ForgotPassword" %>

<asp:Content ID="cHead" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/Content/xpcss/auth.css") %>">
</asp:Content>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">

    <div class="auth-page">
        <div class="auth-card">
            <div class="auth-card-title">
                <img src="<%= ResolveUrl("~/en/images/mstoolbar_icp.gif") %>" alt="" height="16" onerror="this.style.display='none'" />
                <span>Reset Password</span>
            </div>

            <div class="auth-card-body">

                <asp:Panel ID="pnlStatus" runat="server" CssClass="auth-status" Visible="false">
                    <asp:Literal ID="litStatus" runat="server" />
                </asp:Panel>

                <p class="auth-intro">Enter your account email and we'll send you a link to reset your password.</p>

                <div class="field-row-stacked">
                    <label for="txtEmail">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="auth-input"
                        autocomplete="email" placeholder="you@example.com" />
                </div>

                <div class="auth-actions">
                    <button id="btnReset" runat="server" type="submit" onserverclick="btnReset_Click" class="auth-primary">Send reset link</button>
                </div>

                <p class="auth-foot">Remembered it?
                    <a href="<%= ResolveUrl("~/en/account/Login.aspx") %>">Back to sign in</a>.
                </p>

            </div>
        </div>
    </div>

</asp:Content>
