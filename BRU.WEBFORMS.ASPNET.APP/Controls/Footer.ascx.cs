using System;
using System.Web.UI;

namespace BRU.WEBFORMS.ASPNET.APP.Controls
{
    /// <summary>
    /// Footer user control for Windows XP styled website.
    /// Copyright text is configured via Web.config appSettings (see SiteConfig),
    /// and can be overridden per-page via the CopyrightText property.
    /// </summary>
    public partial class Footer : UserControl
    {
        protected global::System.Web.UI.WebControls.Literal litCopyright;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(litCopyright.Text))
                litCopyright.Text = SiteConfig.CopyrightText;
        }

        /// <summary>
        /// Gets or sets the copyright text. Overrides Web.config setting.
        /// </summary>
        public string CopyrightText
        {
            get { return litCopyright.Text; }
            set { litCopyright.Text = value; }
        }
    }
}
