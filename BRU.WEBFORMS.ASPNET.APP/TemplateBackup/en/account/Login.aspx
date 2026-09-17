<%@ Page Title="Sign In" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.en.account.Login" %>

<asp:Content ID="cHead" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/Content/xpcss/auth.css") %>">
</asp:Content>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">

    <div class="auth-page">
        <div class="auth-card">
            <div class="auth-card-title">
                <img src="<%= ResolveUrl("~/en/images/mstoolbar_icp.gif") %>" alt="" height="16" onerror="this.style.display='none'" />
                <span>Sign In</span>
            </div>

            <div class="auth-card-body">

                <asp:Panel ID="pnlStatus" runat="server" CssClass="auth-status" Visible="false">
                    <asp:Literal ID="litStatus" runat="server" />
                </asp:Panel>

                <p class="auth-intro">Sign in to your Windows Update Restored account.</p>

                <div class="field-row-stacked">
                    <label for="txtEmail">Email or username</label>
                    <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="auth-input"
                        autocomplete="username" placeholder="you@example.com" />
                </div>

                <div class="field-row-stacked">
                    <label for="txtPassword">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="auth-input"
                        autocomplete="current-password" placeholder="Your password" />
                </div>

                <div class="auth-row-between">
                    <div class="field-row">
                        <asp:CheckBox ID="chkRemember" runat="server" />
                        <label for="<%= chkRemember.ClientID %>">Remember me</label>
                    </div>
                    <a href="<%= ResolveUrl("~/en/account/ForgotPassword.aspx") %>">Forgot password?</a>
                </div>

                <div class="auth-actions">
                    <button id="btnSignIn" runat="server" type="submit" onserverclick="btnSignIn_Click" class="auth-primary">Sign In</button>
                    <button id="btnPasswordless" runat="server" type="submit" onserverclick="btnPasswordless_Click">Email me a one-time code</button>
                </div>

                <fieldset class="auth-mfa">
                    <legend>Two-factor authentication</legend>
                    <p class="auth-hint">If your account has 2FA enabled, enter the 6-digit code from your authenticator app or SMS.</p>
                    <div class="field-row">
                        <label for="txtMfaCode">Verification code</label>
                        <asp:TextBox ID="txtMfaCode" runat="server" CssClass="auth-input auth-code" MaxLength="6"
                            inputmode="numeric" autocomplete="one-time-code" placeholder="123456" />
                        <button id="btnVerifyMfa" runat="server" type="submit" onserverclick="btnVerifyMfa_Click">Verify</button>
                    </div>
                </fieldset>

                <div class="auth-sep"><span>or continue with</span></div>

                <div class="auth-oauth">
                    <button id="btnGoogle" runat="server" type="submit" data-provider="Google" onserverclick="OAuth_Click">Google</button>
                    <button id="btnMicrosoft" runat="server" type="submit" data-provider="Microsoft" onserverclick="OAuth_Click">Microsoft</button>
                    <button id="btnGitHub" runat="server" type="submit" data-provider="GitHub" onserverclick="OAuth_Click">GitHub</button>
                    <button id="btnDiscord" runat="server" type="submit" data-provider="Discord" onserverclick="OAuth_Click">Discord</button>
                    <button id="btnApple" runat="server" type="submit" data-provider="Apple" onserverclick="OAuth_Click">Apple</button>
                    <button id="btnFacebook" runat="server" type="submit" data-provider="Facebook" onserverclick="OAuth_Click">Facebook</button>
                </div>

                <p class="auth-foot">Don't have an account?
                    <a href="<%= ResolveUrl("~/en/account/Register.aspx") %>">Create one</a>.
                </p>

            </div>
        </div>
    </div>

</asp:Content>
