using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.UI.WebControls;
using BRU.WEBFORMS.ASPNET.APP;

namespace BRU.WEBFORMS.ASPNET.APP.SystemPages
{
    public partial class Settings : SecurePage
    {
        protected override string[] RequiredRoles { get { return new[] { "administrator" }; } }

        protected global::System.Web.UI.WebControls.Label lblError;
        protected global::System.Web.UI.WebControls.Label lblSuccess;
        protected global::System.Web.UI.WebControls.TextBox txtSiteName;
        protected global::System.Web.UI.WebControls.TextBox txtDefaultPageTitle;
        protected global::System.Web.UI.WebControls.TextBox txtCopyright;
        protected global::System.Web.UI.WebControls.TextBox txtLogoAlt;
        protected global::System.Web.UI.WebControls.TextBox txtLogoWidth;
        protected global::System.Web.UI.WebControls.TextBox txtLogoHeight;
        protected global::System.Web.UI.WebControls.Button btnSave;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadSettings();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                RequireRole("administrator");
                int logoWidth;
                int logoHeight;
                string siteName = RequiredText(txtSiteName.Text, "Site name", 100);
                string pageTitle = RequiredText(txtDefaultPageTitle.Text, "Default page title", 150);
                string copyright = RequiredText(txtCopyright.Text, "Copyright text", 200);
                string logoAlt = RequiredText(txtLogoAlt.Text, "Logo alternative text", 100);
                if (!int.TryParse(txtLogoWidth.Text, NumberStyles.None, CultureInfo.InvariantCulture, out logoWidth) || logoWidth < 16 || logoWidth > 800)
                    throw new ServiceException("Logo width must be between 16 and 800 pixels.");
                if (!int.TryParse(txtLogoHeight.Text, NumberStyles.None, CultureInfo.InvariantCulture, out logoHeight) || logoHeight < 16 || logoHeight > 240)
                    throw new ServiceException("Logo height must be between 16 and 240 pixels.");

                Dictionary<string, string> settings = new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    { "SiteName", siteName },
                    { "DefaultPageTitle", pageTitle },
                    { "CopyrightText", copyright },
                    { "SiteLogoAlt", logoAlt },
                    { "SiteLogoWidth", logoWidth.ToString(CultureInfo.InvariantCulture) },
                    { "SiteLogoHeight", logoHeight.ToString(CultureInfo.InvariantCulture) }
                };
                SystemSettingsStore.SaveValues(settings);
                ShowSuccess("Settings saved.");
            }
            catch (ServiceException ex)
            {
                ShowError(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                ShowError("The application identity cannot write App_Data/SystemSettings.xml.");
            }
            catch (IOException ex)
            {
                System.Diagnostics.Trace.TraceError("System settings save failed: {0}", ex);
                ShowError("Settings could not be saved. Verify that App_Data is writable by the application identity.");
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
        }

        private static string RequiredText(string value, string fieldName, int maxLength)
        {
            string normalized = (value ?? string.Empty).Trim();
            if (normalized.Length == 0 || normalized.Length > maxLength)
                throw new ServiceException(fieldName + " is required and cannot exceed " + maxLength + " characters.");
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
