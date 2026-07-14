using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BRU.WEBFORMS.ASPNET.APP.Controls
{
    /// <summary>
    /// Toolbar user control for Windows XP styled website.
    /// Logo and home link are configured via Web.config appSettings (see SiteConfig),
    /// and can be overridden per-page via the public properties or markup attributes.
    /// </summary>
    public partial class Toolbar : UserControl
    {
        protected global::System.Web.UI.WebControls.HyperLink hlHome;
        protected global::System.Web.UI.WebControls.Image imgLogo;

        protected void Page_Load(object sender, EventArgs e)
        {
            ApplyConfigDefaults();
        }

        /// <summary>
        /// Applies logo and home-link settings from SiteConfig (Web.config) unless
        /// they have already been provided via markup attributes or the override
        /// properties (which are set before this control's Load fires).
        /// </summary>
        private void ApplyConfigDefaults()
        {
            if (string.IsNullOrEmpty(hlHome.NavigateUrl))
                hlHome.NavigateUrl = ResolveUrl(SiteConfig.HomeUrl);

            if (string.IsNullOrEmpty(imgLogo.ImageUrl))
                imgLogo.ImageUrl = ResolveUrl(SiteConfig.LogoUrl);

            if (imgLogo.Width.IsEmpty)
                imgLogo.Width = Unit.Pixel(SiteConfig.LogoWidth);

            if (imgLogo.Height.IsEmpty)
                imgLogo.Height = Unit.Pixel(SiteConfig.LogoHeight);

            if (string.IsNullOrEmpty(imgLogo.AlternateText))
                imgLogo.AlternateText = SiteConfig.LogoAltText;
        }

        #region Public Properties for Override

        /// <summary>
        /// Gets or sets the logo image URL. Overrides Web.config setting.
        /// </summary>
        public string LogoImageUrl
        {
            get { return imgLogo.ImageUrl; }
            set { imgLogo.ImageUrl = value; }
        }

        /// <summary>
        /// Gets or sets the logo width. Overrides Web.config setting.
        /// </summary>
        public Unit LogoWidth
        {
            get { return imgLogo.Width; }
            set { imgLogo.Width = value; }
        }

        /// <summary>
        /// Gets or sets the logo height. Overrides Web.config setting.
        /// </summary>
        public Unit LogoHeight
        {
            get { return imgLogo.Height; }
            set { imgLogo.Height = value; }
        }

        /// <summary>
        /// Gets or sets the logo alternate text. Overrides Web.config setting.
        /// </summary>
        public string LogoAlternateText
        {
            get { return imgLogo.AlternateText; }
            set { imgLogo.AlternateText = value; }
        }

        /// <summary>
        /// Gets or sets the home navigation URL.
        /// </summary>
        public string HomeNavigateUrl
        {
            get { return hlHome.NavigateUrl; }
            set { hlHome.NavigateUrl = value; }
        }

        #endregion
    }
}
