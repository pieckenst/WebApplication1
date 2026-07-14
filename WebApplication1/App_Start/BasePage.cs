using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BRU.WEBFORMS.ASPNET.APP.Controls;

namespace BRU.WEBFORMS.ASPNET.APP
{
    /// <summary>
    /// Base page class for all content pages.
    /// Provides common functionality and easier content overrides via Web.config.
    /// </summary>
    public class BasePage : Page
    {
        /// <summary>
        /// Key used for Web.config page overrides (Page.{PageContentKey}.{controlId}.{property}).
        /// Defaults to the code-behind class name without leading underscore.
        /// </summary>
        protected virtual string PageContentKey
        {
            get
            {
                string name = GetType().Name;
                return name.StartsWith("_") ? name.Substring(1) : name;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            string configTitle = SiteConfig.GetPageTitle(PageContentKey);
            if (!string.IsNullOrEmpty(configTitle))
                Title = configTitle;
            else if (string.IsNullOrEmpty(Title) || Title == "Untitled Page")
                Title = SiteConfig.DefaultPageTitle;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                LoadPageContent();
                ApplyPageContentFromConfig();
                ApplyLiteralOverridesFromConfig();
                SetLastUpdatedIfPresent();
            }
        }

        /// <summary>
        /// Override to set default page content in code-behind.
        /// Web.config values override these defaults when present.
        /// </summary>
        protected virtual void LoadPageContent()
        {
        }

        /// <summary>
        /// Applies ContentBox HeaderText and ContentHtml from Web.config.
        /// </summary>
        protected virtual void ApplyPageContentFromConfig()
        {
            ApplyContentBoxConfig(this, PageContentKey);
        }

        /// <summary>
        /// Logs (via <see cref="System.Diagnostics.Trace"/>) that this page is
        /// loading its content. Call at the top of LoadPageContent.
        /// </summary>
        protected void LogPageContentLoad()
        {
            System.Diagnostics.Trace.TraceInformation(
                "LoadPageContent: page '{0}'", PageContentKey);
        }

        /// <summary>
        /// Returns the centrally-configured ContentHtml for a control on this
        /// page (App_Data/PageContent.xml first, then Web.config) if present;
        /// otherwise returns <paramref name="defaultHtml"/>. Use this inside
        /// LoadPageContent so a page keeps an inline default while the central
        /// content file takes priority. Logs which source was used.
        /// </summary>
        protected string GetConfiguredContent(string controlId, string defaultHtml)
        {
            string configured = SiteConfig.GetContentHtml(PageContentKey, controlId);
            bool fromConfig = !string.IsNullOrWhiteSpace(configured);
            LogContentSource(controlId, "ContentHtml", fromConfig);
            return fromConfig ? configured : defaultHtml;
        }

        /// <summary>
        /// Returns the centrally-configured HeaderText for a control on this
        /// page (App_Data/PageContent.xml first, then Web.config) if present;
        /// otherwise returns <paramref name="defaultHeader"/>. Logs which source
        /// was used.
        /// </summary>
        protected string GetConfiguredHeader(string controlId, string defaultHeader)
        {
            string configured = SiteConfig.GetHeaderText(PageContentKey, controlId);
            bool fromConfig = !string.IsNullOrWhiteSpace(configured);
            LogContentSource(controlId, "HeaderText", fromConfig);
            return fromConfig ? configured : defaultHeader;
        }

        private void LogContentSource(string controlId, string propertyName, bool fromConfig)
        {
            System.Diagnostics.Trace.TraceInformation(
                "PageContent: {0}.{1}.{2} <- {3}",
                PageContentKey,
                controlId,
                propertyName,
                fromConfig ? "central config (PageContent.xml/Web.config)" : "code-behind default");
        }

        /// <summary>
        /// Sets the content of a ContentBox control by ID.
        /// </summary>
        protected void SetContent(string contentBoxId, string headerText, string contentHtml)
        {
            var control = FindControlRecursive(this, contentBoxId) as ContentBox;
            if (control == null)
                return;

            if (headerText != null)
                control.HeaderText = headerText;
            if (contentHtml != null)
                control.ContentHtml = contentHtml;
        }

        /// <summary>
        /// Sets the content of a ContentBox with specified colors.
        /// </summary>
        protected void SetContent(string contentBoxId, string headerText, string contentHtml,
            HeaderColorStyle headerColor, ContentColorStyle contentColor)
        {
            var control = FindControlRecursive(this, contentBoxId) as ContentBox;
            if (control == null)
                return;

            if (headerText != null)
                control.HeaderText = headerText;
            if (contentHtml != null)
                control.ContentHtml = contentHtml;

            control.HeaderColor = headerColor;
            control.ContentColor = contentColor;
        }

        /// <summary>
        /// Sets a literal control's text by ID.
        /// </summary>
        protected void SetLiteral(string literalId, string text)
        {
            var control = FindControlRecursive(this, literalId) as Literal;
            if (control != null)
                control.Text = text;
        }

        protected Control FindControlRecursive(Control root, string id)
        {
            if (root.ID == id)
                return root;

            foreach (Control child in root.Controls)
            {
                Control found = FindControlRecursive(child, id);
                if (found != null)
                    return found;
            }

            return null;
        }

        private void ApplyContentBoxConfig(Control root, string pageKey)
        {
            if (root is ContentBox contentBox && !string.IsNullOrEmpty(root.ID))
            {
                string headerText = SiteConfig.GetHeaderText(pageKey, root.ID);
                if (!string.IsNullOrEmpty(headerText))
                    contentBox.HeaderText = headerText;

                string contentHtml = SiteConfig.GetContentHtml(pageKey, root.ID);
                if (!string.IsNullOrEmpty(contentHtml))
                    contentBox.ContentHtml = contentHtml;
            }

            foreach (Control child in root.Controls)
                ApplyContentBoxConfig(child, pageKey);
        }

        private void ApplyLiteralOverridesFromConfig()
        {
            ApplyLiteralConfig(this, PageContentKey);
        }

        private void ApplyLiteralConfig(Control root, string pageKey)
        {
            var literal = root as Literal;
            if (literal != null && !string.IsNullOrEmpty(root.ID))
            {
                string text = SiteConfig.GetLiteralText(pageKey, root.ID);
                if (!string.IsNullOrEmpty(text))
                    literal.Text = text;
            }

            foreach (Control child in root.Controls)
                ApplyLiteralConfig(child, pageKey);
        }

        private void SetLastUpdatedIfPresent()
        {
            var literal = FindControlRecursive(this, "litLastUpdated") as Literal;
            if (literal == null || !string.IsNullOrEmpty(literal.Text))
                return;

            literal.Text = "Last Updated: " + DateTime.Now.ToString("MM/dd/yyyy");
        }
    }
}
