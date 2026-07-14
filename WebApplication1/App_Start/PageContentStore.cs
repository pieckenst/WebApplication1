using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using System.Xml.Linq;

namespace BRU.WEBFORMS.ASPNET.APP
{
    /// <summary>
    /// Central store for per-page content. Reads a single file,
    /// <c>~/App_Data/PageContent.xml</c>, so every page's ContentBox HTML,
    /// header text, literal text and title can be edited in one place without
    /// touching any page code-behind.
    ///
    /// The file is cached and automatically reloaded when it changes on disk,
    /// so edits take effect without recompiling or restarting.
    ///
    /// Web.config <c>Page.{Key}.{Control}.{Property}</c> settings still take
    /// precedence over this file (see <see cref="SiteConfig"/>).
    /// </summary>
    public static class PageContentStore
    {
        private const string CacheKey = "PageContentStore.Data";
        private const string VirtualPath = "~/App_Data/PageContent.xml";

        /// <summary>
        /// Gets a stored value for the given page/control/property, or null when
        /// not present. Use a null or empty controlId for page-level values
        /// (e.g. the page Title).
        /// </summary>
        public static string GetValue(string pageKey, string controlId, string propertyName)
        {
            string value;
            return GetData().TryGetValue(BuildKey(pageKey, controlId, propertyName), out value) ? value : null;
        }

        private static string BuildKey(string pageKey, string controlId, string propertyName)
        {
            return (pageKey ?? string.Empty) + "|" + (controlId ?? string.Empty) + "|" + (propertyName ?? string.Empty);
        }

        private static Dictionary<string, string> GetData()
        {
            var cache = HttpRuntime.Cache;
            var data = cache != null ? cache[CacheKey] as Dictionary<string, string> : null;
            if (data != null)
                return data;

            data = Load();

            if (cache != null)
            {
                string path = ResolvePath();
                if (path != null && File.Exists(path))
                    cache.Insert(CacheKey, data, new CacheDependency(path));
                else
                    cache.Insert(CacheKey, data);
            }

            return data;
        }

        private static string ResolvePath()
        {
            return HostingEnvironment.IsHosted ? HostingEnvironment.MapPath(VirtualPath) : null;
        }

        private static Dictionary<string, string> Load()
        {
            var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            string path = ResolvePath();
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                return data;

            XDocument doc;
            try
            {
                doc = XDocument.Load(path);
            }
            catch (Exception ex)
            {
                // Don't crash the site over a bad content file, but make the
                // failure visible so operators know overrides aren't applied
                // (instead of silently falling back to code defaults).
                System.Diagnostics.Trace.TraceError(
                    "PageContentStore: failed to load '" + path + "': " + ex);
                return data;
            }

            if (doc.Root == null)
                return data;

            foreach (var page in doc.Root.Elements("page"))
            {
                string pageKey = (string)page.Attribute("key");
                if (string.IsNullOrEmpty(pageKey))
                    continue;

                string title = (string)page.Attribute("title");
                if (!string.IsNullOrEmpty(title))
                    data[BuildKey(pageKey, null, "Title")] = title;

                foreach (var box in page.Elements("box"))
                {
                    string id = (string)box.Attribute("id");
                    if (string.IsNullOrEmpty(id))
                        continue;

                    string header = (string)box.Attribute("header");
                    if (header != null)
                        data[BuildKey(pageKey, id, "HeaderText")] = header;

                    data[BuildKey(pageKey, id, "ContentHtml")] = GetInnerContent(box);
                }

                foreach (var literal in page.Elements("literal"))
                {
                    string id = (string)literal.Attribute("id");
                    if (string.IsNullOrEmpty(id))
                        continue;

                    data[BuildKey(pageKey, id, "Text")] = GetInnerContent(literal);
                }
            }

            return data;
        }

        private static string GetInnerContent(XElement element)
        {
            return element.Value == null ? null : element.Value.Trim();
        }
    }
}
