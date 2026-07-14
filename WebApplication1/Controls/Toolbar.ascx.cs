using System.Web.UI;
using System.Web.UI.WebControls;

namespace BRU.WEBFORMS.ASPNET.APP.Controls
{
    /// <summary>
    /// Toolbar user control for Windows XP styled website.
    /// Logo and home link are configured via Web.config appSettings (see SiteConfig).
    /// </summary>
    public partial class Toolbar : UserControl
    {
        protected global::System.Web.UI.WebControls.HyperLink hlHome;
        protected global::System.Web.UI.WebControls.Image imgLogo;

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
