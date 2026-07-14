using System;
using System.Collections.Generic;
using System.IO;
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
    /// The file is cached and hot-reloaded: the parsed content is kept in
    /// memory and automatically re-read on the next request whenever the file's
    /// last-write time or size changes on disk, so edits take effect without
    /// recompiling or restarting.
    ///
    /// Web.config <c>Page.{Key}.{Control}.{Property}</c> settings still take
    /// precedence over this file (see <see cref="SiteConfig"/>).
    /// </summary>
    public static class PageContentStore
    {
        private const string VirtualPath = "~/App_Data/PageContent.xml";

        private static readonly object _sync = new object();
        private static Dictionary<string, string> _cache;
        private static string _cachedPath;
        private static DateTime _cachedWriteUtc;
        private static long _cachedLength = -1;

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

        /// <summary>
        /// Returns the parsed content, reloading it if the underlying file has
        /// changed since it was last read (hot reload).
        /// </summary>
        private static Dictionary<string, string> GetData()
        {
            string path = ResolvePath();

            DateTime writeUtc = DateTime.MinValue;
            long length = -1;
            bool exists = !string.IsNullOrEmpty(path) && File.Exists(path);
            if (exists)
            {
                var info = new FileInfo(path);
                writeUtc = info.LastWriteTimeUtc;
                length = info.Length;
            }

            var current = _cache;
            if (current != null && path == _cachedPath && writeUtc == _cachedWriteUtc && length == _cachedLength)
                return current;

            lock (_sync)
            {
                if (_cache != null && path == _cachedPath && writeUtc == _cachedWriteUtc && length == _cachedLength)
                    return _cache;

                var data = Load(path, exists);
                _cache = data;
                _cachedPath = path;
                _cachedWriteUtc = writeUtc;
                _cachedLength = length;
                return data;
            }
        }

        private static string ResolvePath()
        {
            return HostingEnvironment.IsHosted ? HostingEnvironment.MapPath(VirtualPath) : null;
        }

        private static Dictionary<string, string> Load(string path, bool exists)
        {
            var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (string.IsNullOrEmpty(path) || !exists)
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
