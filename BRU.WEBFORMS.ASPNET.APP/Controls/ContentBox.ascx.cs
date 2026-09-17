using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BRU.WEBFORMS.ASPNET.APP.Controls
{
    public enum HeaderColorStyle
    {
        Blue,
        Gray,
        Red
    }

    public enum ContentColorStyle
    {
        White,
        Gray,
        Yellow
    }

    /// <summary>
    /// Reusable content box control for Windows XP styled website.
    /// Header, colors, and HTML can be set in .aspx markup or overridden via Web.config.
    /// </summary>
    public partial class ContentBox : UserControl
    {
        protected global::System.Web.UI.WebControls.Panel pnlHeader;
        protected global::System.Web.UI.WebControls.Panel pnlContent;
        protected global::System.Web.UI.WebControls.Literal litHeader;
        protected global::System.Web.UI.WebControls.Literal litContent;

        private HeaderColorStyle _headerColor = HeaderColorStyle.Blue;
        private ContentColorStyle _contentColor = ContentColorStyle.White;

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            ApplyStyles();
        }

        protected void ApplyStyles()
        {
            pnlHeader.CssClass = "content-box-header content-box-header-" + _headerColor.ToString().ToLower();
            pnlContent.CssClass = "content-box-content content-box-content-" + _contentColor.ToString().ToLower();
        }

        /// <summary>
        /// Gets or sets the header text.
        /// </summary>
        public string HeaderText
        {
            get { return litHeader.Text; }
            set { litHeader.Text = value; }
        }

        /// <summary>
        /// Gets or sets the content HTML.
        /// </summary>
        public string ContentHtml
        {
            get { return litContent.Text; }
            set { litContent.Text = value; }
        }

        /// <summary>
        /// Gets or sets the header background color (HeaderColor="Blue" in .aspx).
        /// </summary>
        public HeaderColorStyle HeaderColor
        {
            get { return _headerColor; }
            set { _headerColor = value; }
        }

        /// <summary>
        /// Gets or sets the content background color (ContentColor="White" in .aspx).
        /// </summary>
        public ContentColorStyle ContentColor
        {
            get { return _contentColor; }
            set { _contentColor = value; }
        }
    }
}
