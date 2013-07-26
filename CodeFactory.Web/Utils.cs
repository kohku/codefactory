using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CodeFactory.Web
{
    public class Utils
    {
        /// <summary>
        /// Gets the relative root of the website.
        /// </summary>
        /// <value>A string that ends with a '/'.</value>
        public static string RelativeWebRoot
        {
            get { return VirtualPathUtility.ToAbsolute("~/"); }
        }

        private static Uri _AbsoluteWebRoot;

        /// <summary>
        /// Gets the absolute root of the website.
        /// </summary>
        /// <value>A string that ends with a '/'.</value>
        public static Uri AbsoluteWebRoot
        {
            get
            {
                if (_AbsoluteWebRoot == null)
                {
                    HttpContext context = HttpContext.Current;
                    if (context == null)
                        throw new System.Net.WebException("The current HttpContext is null");

                    _AbsoluteWebRoot = new Uri(context.Request.Url.Scheme + "://" + context.Request.Url.Authority + RelativeWebRoot);
                }
                return _AbsoluteWebRoot;
            }
        }

        /// <summary>
        /// Strips all illegal characters from the specified title.
        /// </summary>
        public static string RemoveIllegalCharacters(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            text = text.Replace(":", string.Empty);
            text = text.Replace("/", string.Empty);
            text = text.Replace("?", string.Empty);
            text = text.Replace("#", string.Empty);
            text = text.Replace("[", string.Empty);
            text = text.Replace("]", string.Empty);
            text = text.Replace("@", string.Empty);
            text = text.Replace(".", string.Empty);
            text = text.Replace("\"", string.Empty);
            text = text.Replace("&", string.Empty);
            text = text.Replace("'", string.Empty);
            text = text.Replace(" ", "-");
            text = RemoveDiacritics(text);

            return HttpUtility.UrlEncode(text).Replace("%", string.Empty);
        }

        private static String RemoveDiacritics(string text)
        {
            String normalized = text.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < normalized.Length; i++)
            {
                Char c = normalized[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString();
        }

        public static string SizeFormat(float size, string formatString)
        {
            if (size < 1024)
                return size.ToString(formatString) + " bytes";

            if (size < Math.Pow(1024, 2))
                return (size / 1024).ToString(formatString) + " kb";

            if (size < Math.Pow(1024, 3))
                return (size / Math.Pow(1024, 2)).ToString(formatString) + " mb";

            if (size < Math.Pow(1024, 4))
                return (size / Math.Pow(1024, 3)).ToString(formatString) + " gb";

            return size.ToString(formatString);
        }

        public static bool IsGuid(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                return false;

            Regex guidRegEx = new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");

            return guidRegEx.IsMatch(expression);
        }

        private static readonly Regex STRIP_HTML = new Regex("<[^>]*>", RegexOptions.Compiled);
        /// <summary>
        /// Strips all HTML tags from the specified string.
        /// </summary>
        /// <param name="html">The string containing HTML</param>
        /// <returns>A string without HTML tags</returns>
        public static string StripHtml(string html)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            return STRIP_HTML.Replace(html, string.Empty);
        }

        private static readonly Regex REGEX_BETWEEN_TAGS = new Regex(@">\s+", RegexOptions.Compiled);
        private static readonly Regex REGEX_LINE_BREAKS = new Regex(@"\n\s+", RegexOptions.Compiled);

        /// <summary>
        /// Removes the HTML whitespace.
        /// </summary>
        /// <param name="html">The HTML.</param>
        public static string RemoveHtmlWhitespace(string html)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            html = REGEX_BETWEEN_TAGS.Replace(html, "> ");
            html = REGEX_LINE_BREAKS.Replace(html, string.Empty);

            return html.Trim();
        }
    }
}
