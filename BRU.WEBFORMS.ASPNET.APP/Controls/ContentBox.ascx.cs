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
    /// Supports both ContentHtml and nested ContentTemplate controls.
    /// </summary>
    [ParseChildren(true)]
    [PersistChildren(false)]
    public partial class ContentBox : UserControl
    {
        protected global::System.Web.UI.WebControls.Panel pnlHeader;
        protected global::System.Web.UI.WebControls.Panel pnlContent;
        protected global::System.Web.UI.WebControls.Literal litHeader;
        protected global::System.Web.UI.WebControls.Literal litContent;
        protected global::System.Web.UI.WebControls.PlaceHolder phContent;

        private HeaderColorStyle _headerColor = HeaderColorStyle.Blue;
        private ContentColorStyle _contentColor = ContentColorStyle.White;

        private bool _templateInstantiated;

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate ContentTemplate
        {
            get;
            set;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!_templateInstantiated && ContentTemplate != null)
            {
                ContentTemplate.InstantiateIn(phContent);
                _templateInstantiated = true;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            ApplyStyles();
        }

        protected void ApplyStyles()
        {
            pnlHeader.CssClass =
                "content-box-header content-box-header-" +
                _headerColor.ToString().ToLowerInvariant();

            pnlContent.CssClass =
                "content-box-content content-box-content-" +
                _contentColor.ToString().ToLowerInvariant();
        }

        public string HeaderText
        {
            get { return litHeader.Text; }
            set { litHeader.Text = value; }
        }

        public string ContentHtml
        {
            get { return litContent.Text; }
            set { litContent.Text = value; }
        }

        public HeaderColorStyle HeaderColor
        {
            get { return _headerColor; }
            set { _headerColor = value; }
        }

        public ContentColorStyle ContentColor
        {
            get { return _contentColor; }
            set { _contentColor = value; }
        }

        /// <summary>
        /// Finds a control anywhere inside the ContentTemplate.
        /// </summary>
        public Control FindContentControl(string id)
        {
            return FindControlRecursive(phContent, id);
        }

        /// <summary>
        /// Finds a typed control anywhere inside the ContentTemplate.
        /// </summary>
        public T FindContentControl<T>(string id) where T : Control
        {
            return FindControlRecursive(phContent, id) as T;
        }

        private Control FindControlRecursive(Control root, string id)
        {
            if (root == null)
            {
                return null;
            }

            Control found = root.FindControl(id);

            if (found != null)
            {
                return found;
            }

            foreach (Control child in root.Controls)
            {
                found = FindControlRecursive(child, id);

                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }
    }
}