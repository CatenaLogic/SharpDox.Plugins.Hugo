// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="CatenaLogic">
//   Copyright (c) 2008 - 2017 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SharpDox.Plugins.Hugo
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;

    internal static class StringExtensions
    {
        public static bool IsNonPublic(this string accessibility)
        {
            return !accessibility.Contains("public");
        }

        public static string ToObjectString(this string text)
        {
            var trimmedText = text.Trim();
            if (trimmedText.StartsWith("<pre><code>"))
            {
                trimmedText = trimmedText.Replace("<pre><code>", string.Empty);
                trimmedText = trimmedText.Replace("</code></pre>", string.Empty);
                trimmedText = trimmedText.Trim();
                trimmedText = string.Format("<pre><code class=\"language-csharp line-numbers\">{0}</code></pre>", trimmedText);
            }
            else if (trimmedText.Contains("<pre><code>"))
            {
                trimmedText = trimmedText.Replace("<pre><code>", "<pre><code class=\"language-csharp line-numbers\">");
                trimmedText = trimmedText.Replace(Environment.NewLine + "</code></pre>", "</code></pre>");
            }

            return trimmedText
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace(Environment.NewLine, " \\n");
        }

        public static string RemoveIllegalPathChars(this string text)
        {
            var invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                text = text.Replace(c.ToString(), "-");
            }

            return text;
        }

        public static string RemoveIllegalHtmlIdChars(this string text)
        {
            var invalid = new[] {'.', '(', ')', '<', '>', '[', ']', '{', '}', ','};

            foreach (char c in invalid)
            {
                text = text.Replace(c.ToString(), string.Empty);
            }

            return text.Replace(" ", string.Empty);
        }

        public static string MinifyJson(this string json)
        {
            return Regex.Replace(json, "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1").Replace(Environment.NewLine, string.Empty);
        }
    }
}