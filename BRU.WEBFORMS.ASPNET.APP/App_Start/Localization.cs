using System;
using System.Globalization;
using System.Web;
using System.Web.UI;

namespace BRU.WEBFORMS.ASPNET.APP
{
    public static class Localization
    {
        public const string English = "en";
        public const string Russian = "ru";

        public static string Language
        {
            get { return NormalizeLanguage(SystemSettingsStore.GetValue("Language")); }
        }

        public static CultureInfo Culture
        {
            get { return new CultureInfo(Language == Russian ? "ru-RU" : "en-US"); }
        }

        public static string NormalizeLanguage(string language)
        {
            return string.Equals(language, Russian, StringComparison.OrdinalIgnoreCase)
                ? Russian
                : English;
        }

        public static string Get(string key)
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;

            object value = HttpContext.GetGlobalResourceObject("Strings", key, Culture);
            return value == null ? key : Convert.ToString(value, Culture);
        }

        public static string GetHtml(string key)
        {
            return HttpUtility.HtmlEncode(Get(key));
        }

        public static string GetFormat(string key, params object[] arguments)
        {
            return string.Format(Culture, Get(key), arguments);
        }

        public static string GetPageTitle(string pageKey, string fallback)
        {
            string key = "Title_" + pageKey;
            object value = HttpContext.GetGlobalResourceObject("Strings", key, Culture);
            return value == null ? fallback : Convert.ToString(value, Culture);
        }
    }
}