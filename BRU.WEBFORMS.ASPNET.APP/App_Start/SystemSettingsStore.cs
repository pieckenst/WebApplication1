using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Xml.Linq;

namespace BRU.WEBFORMS.ASPNET.APP
{
    public static class SystemSettingsStore
    {
        private static readonly object SyncRoot = new object();
        private static readonly HashSet<string> AllowedKeys = new HashSet<string>(StringComparer.Ordinal)
        {
            "SiteName", "DefaultPageTitle", "CopyrightText", "SiteLogoWidth", "SiteLogoHeight", "SiteLogoAlt", "Language"
        };
        private static Dictionary<string, string> _cachedValues = new Dictionary<string, string>(StringComparer.Ordinal);
        private static DateTime _cachedWriteTimeUtc = DateTime.MinValue;

        public static string GetValue(string key)
        {
            if (!AllowedKeys.Contains(key)) return null;
            string path = GetPath();
            lock (SyncRoot)
            {
                if (!File.Exists(path)) return null;
                DateTime writeTime = File.GetLastWriteTimeUtc(path);
                if (writeTime != _cachedWriteTimeUtc)
                {
                    _cachedValues = ReadValues(path);
                    _cachedWriteTimeUtc = writeTime;
                }
                string value;
                return _cachedValues.TryGetValue(key, out value) ? value : null;
            }
        }

        public static void SaveValues(IDictionary<string, string> values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));
            Dictionary<string, string> sanitized = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (KeyValuePair<string, string> entry in values)
            {
                if (!AllowedKeys.Contains(entry.Key))
                    throw new InvalidOperationException("The setting is not editable through the system UI.");
                sanitized[entry.Key] = entry.Value ?? string.Empty;
            }

            string path = GetPath();
            string tempPath = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
            lock (SyncRoot)
            {
                try
                {
                    XDocument document = new XDocument(new XElement("settings",
                        sanitized.OrderBy(entry => entry.Key, StringComparer.Ordinal)
                            .Select(entry => new XElement("setting",
                                new XAttribute("key", entry.Key),
                                new XAttribute("value", entry.Value)))));
                    document.Save(tempPath, SaveOptions.DisableFormatting);
                    if (File.Exists(path))
                        File.Replace(tempPath, path, null);
                    else
                        File.Move(tempPath, path);
                    _cachedValues = sanitized;
                    _cachedWriteTimeUtc = File.GetLastWriteTimeUtc(path);
                }
                finally
                {
                    if (File.Exists(tempPath)) File.Delete(tempPath);
                }
            }
        }

        private static Dictionary<string, string> ReadValues(string path)
        {
            Dictionary<string, string> values = new Dictionary<string, string>(StringComparer.Ordinal);
            XDocument document = XDocument.Load(path, LoadOptions.None);
            if (document.Root == null || document.Root.Name != "settings")
                throw new InvalidDataException("System settings file has an invalid root element.");
            foreach (XElement element in document.Root.Elements("setting"))
            {
                string key = (string)element.Attribute("key");
                string value = (string)element.Attribute("value");
                if (AllowedKeys.Contains(key) && !values.ContainsKey(key))
                    values.Add(key, value ?? string.Empty);
            }
            return values;
        }

        private static string GetPath()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/SystemSettings.xml");
            if (string.IsNullOrEmpty(path))
                throw new InvalidOperationException("The application data path is unavailable.");
            return path;
        }
    }
}
