<%@ Page Title="Create Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="BRU.WEBFORMS.ASPNET.APP.en.account.Register" %>

<asp:Content ID="cHead" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/Content/xpcss/auth.css") %>">
</asp:Content>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">

    <div class="auth-page">
        <div class="auth-card">
            <div class="auth-card-title">
                <img src="<%= ResolveUrl("~/en/images/mstoolbar_icp.gif") %>" alt="" height="16" onerror="this.style.display='none'" />
                <span>Create Account</span>
            </div>

            <div class="auth-card-body">

                <asp:Panel ID="pnlStatus" runat="server" CssClass="auth-status" Visible="false">
                    <asp:Literal ID="litStatus" runat="server" />
                </asp:Panel>

                <p class="auth-intro">Create a new Windows Update Restored account.</p>

                <div class="field-row-stacked">
                    <label for="txtName">Display name</label>
                    <asp:TextBox ID="txtName" runat="server" CssClass="auth-input" autocomplete="name" placeholder="Your name" />
                </div>

                <div class="field-row-stacked">
                    <label for="txtEmail">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="auth-input"
                        autocomplete="email" placeholder="you@example.com" />
                </div>

                <div class="field-row-stacked">
                    <label for="txtPassword">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="auth-input"
                        autocomplete="new-password" placeholder="Choose a strong password" />
                </div>

                <div class="field-row-stacked">
                    <label for="txtConfirm">Confirm password</label>
                    <asp:TextBox ID="txtConfirm" runat="server" TextMode="Password" CssClass="auth-input"
                        autocomplete="new-password" placeholder="Re-enter your password" />
                </div>

                <div class="field-row auth-terms">
                    <asp:CheckBox ID="chkTerms" runat="server" />
                    <label for="<%= chkTerms.ClientID %>">I agree to the
                        <a href="<%= ResolveUrl("~/en/Legal.aspx") %>">Terms</a> and
                        <a href="<%= ResolveUrl("~/en/Privacy.aspx") %>">Privacy Policy</a>.
                    </label>
                </div>

                <div class="auth-actions">
                    <button id="btnRegister" runat="server" type="submit" onserverclick="btnRegister_Click" class="auth-primary">Create Account</button>
                </div>

                <div class="auth-sep"><span>or sign up with</span></div>

                <div class="auth-oauth">
                    <button id="btnGoogle" runat="server" type="submit" data-provider="Google" onserverclick="OAuth_Click">Google</button>
                    <button id="btnMicrosoft" runat="server" type="submit" data-provider="Microsoft" onserverclick="OAuth_Click">Microsoft</button>
                    <button id="btnGitHub" runat="server" type="submit" data-provider="GitHub" onserverclick="OAuth_Click">GitHub</button>
                    <button id="btnDiscord" runat="server" type="submit" data-provider="Discord" onserverclick="OAuth_Click">Discord</button>
                    <button id="btnApple" runat="server" type="submit" data-provider="Apple" onserverclick="OAuth_Click">Apple</button>
                    <button id="btnFacebook" runat="server" type="submit" data-provider="Facebook" onserverclick="OAuth_Click">Facebook</button>
                </div>

                <p class="auth-foot">Already have an account?
                    <a href="<%= ResolveUrl("~/en/account/Login.aspx") %>">Sign in</a>.
                </p>

            </div>
        </div>
    </div>

</asp:Content>
