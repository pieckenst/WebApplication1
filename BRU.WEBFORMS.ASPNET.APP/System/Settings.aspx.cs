using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.UI.WebControls;
using BRU.WEBFORMS.ASPNET.APP;
using BRU.WEBFORMS.ASPNET.APP.Services;

namespace BRU.WEBFORMS.ASPNET.APP.SystemPages
{
    public partial class Settings : SecurePage
    {
        private bool _templateControlsResolved;

        protected override string[] RequiredRoles { get { return new[] { "administrator" }; } }

        protected global::System.Web.UI.WebControls.Label lblError;
        protected global::System.Web.UI.WebControls.Label lblSuccess;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbSettings;
        protected global::System.Web.UI.WebControls.Panel pnlSettings;
        protected global::System.Web.UI.WebControls.TextBox txtSiteName;
        protected global::System.Web.UI.WebControls.TextBox txtDefaultPageTitle;
        protected global::System.Web.UI.WebControls.TextBox txtCopyright;
        protected global::System.Web.UI.WebControls.TextBox txtLogoAlt;
        protected global::System.Web.UI.WebControls.TextBox txtLogoWidth;
        protected global::System.Web.UI.WebControls.TextBox txtLogoHeight;
        protected global::System.Web.UI.WebControls.DropDownList ddlLanguage;
        protected global::System.Web.UI.WebControls.Button btnSave;

        protected void Page_Load(object sender, EventArgs e)
        {
            EnsureTemplateControlsResolved();
            if (!IsPostBack)
            {
                LoadSettings();
                string successKey = Session["Settings.SuccessKey"] as string;
                if (!string.IsNullOrEmpty(successKey))
                {
                    Session.Remove("Settings.SuccessKey");
                    ShowSuccess(Localization.Get(successKey));
                }
            }
        }

        private void EnsureTemplateControlsResolved()
        {
            if (_templateControlsResolved)
                return;

            pnlSettings = cbSettings.FindContentControl<Panel>("pnlSettings");
            txtSiteName = cbSettings.FindContentControl<TextBox>("txtSiteName");
            txtDefaultPageTitle = cbSettings.FindContentControl<TextBox>("txtDefaultPageTitle");
            txtCopyright = cbSettings.FindContentControl<TextBox>("txtCopyright");
            txtLogoAlt = cbSettings.FindContentControl<TextBox>("txtLogoAlt");
            txtLogoWidth = cbSettings.FindContentControl<TextBox>("txtLogoWidth");
            txtLogoHeight = cbSettings.FindContentControl<TextBox>("txtLogoHeight");
            ddlLanguage = cbSettings.FindContentControl<DropDownList>("ddlLanguage");
            btnSave = cbSettings.FindContentControl<Button>("btnSave");

            if (pnlSettings == null || txtSiteName == null || txtDefaultPageTitle == null || txtCopyright == null ||
                txtLogoAlt == null || txtLogoWidth == null || txtLogoHeight == null || ddlLanguage == null || btnSave == null)
                throw new InvalidOperationException("Settings controls were not created inside the ContentBox template.");

            _templateControlsResolved = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                RequireRole("administrator");
                string previousLanguage = Localization.Language;
                int logoWidth;
                int logoHeight;
                string siteName = RequiredText(txtSiteName.Text, "Site name", 100);
                string pageTitle = RequiredText(txtDefaultPageTitle.Text, "Default page title", 150);
                string copyright = RequiredText(txtCopyright.Text, "Copyright text", 200);
                string logoAlt = RequiredText(txtLogoAlt.Text, "Logo alternative text", 100);
                if (!int.TryParse(txtLogoWidth.Text, NumberStyles.None, CultureInfo.InvariantCulture, out logoWidth) || logoWidth < 16 || logoWidth > 800)
                    throw new ServiceException(Localization.GetFormat("Settings_LogoWidthError", 16, 800));
                if (!int.TryParse(txtLogoHeight.Text, NumberStyles.None, CultureInfo.InvariantCulture, out logoHeight) || logoHeight < 16 || logoHeight > 240)
                    throw new ServiceException(Localization.GetFormat("Settings_LogoHeightError", 16, 240));

                Dictionary<string, string> settings = new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    { "SiteName", siteName },
                    { "DefaultPageTitle", pageTitle },
                    { "CopyrightText", copyright },
                    { "SiteLogoAlt", logoAlt },
                    { "SiteLogoWidth", logoWidth.ToString(CultureInfo.InvariantCulture) },
                    { "SiteLogoHeight", logoHeight.ToString(CultureInfo.InvariantCulture) },
                    { "Language", Localization.NormalizeLanguage(ddlLanguage.SelectedValue) }
                };
                SystemSettingsStore.SaveValues(settings);
                if (!string.Equals(previousLanguage, Localization.Language, StringComparison.Ordinal))
                {
                    Session["Settings.SuccessKey"] = "Settings_LanguageSaved";
                    Response.Redirect(Request.RawUrl, false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }
                ShowSuccess(Localization.Get("Settings_Saved"));
            }
            catch (ServiceException ex)
            {
                ShowError(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                ShowError(Localization.Get("Settings_WriteDenied"));
            }
            catch (IOException ex)
            {
                System.Diagnostics.Trace.TraceError("System settings save failed: {0}", ex);
                ShowError(Localization.Get("Settings_WriteFailed"));
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        private void LoadSettings()
        {
            txtSiteName.Text = SiteConfig.SiteName;
            txtDefaultPageTitle.Text = SiteConfig.DefaultPageTitle;
            txtCopyright.Text = SiteConfig.CopyrightText;
            txtLogoAlt.Text = SiteConfig.LogoAltText;
            txtLogoWidth.Text = SiteConfig.LogoWidth.ToString(CultureInfo.InvariantCulture);
            txtLogoHeight.Text = SiteConfig.LogoHeight.ToString(CultureInfo.InvariantCulture);
            ddlLanguage.SelectedValue = Localization.Language;
        }

        private static string RequiredText(string value, string fieldName, int maxLength)
        {
            string normalized = (value ?? string.Empty).Trim();
            if (normalized.Length == 0 || normalized.Length > maxLength)
                throw new ServiceException(Localization.GetFormat("Settings_FieldRequired", fieldName, maxLength));
            return normalized;
        }

        private void ShowError(string message)
        {
            lblError.Text = Server.HtmlEncode(message);
            lblError.Visible = true;
            lblSuccess.Visible = false;
        }

        private void ShowSuccess(string message)
        {
            lblSuccess.Text = Server.HtmlEncode(message);
            lblSuccess.Visible = true;
            lblError.Visible = false;
        }
    }
}
