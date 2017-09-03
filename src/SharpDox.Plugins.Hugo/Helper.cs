// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Helper.cs" company="CatenaLogic">
//   Copyright (c) 2008 - 2017 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SharpDox.Plugins.Hugo
{
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class Helper
    {
        public static string TransformLinkToken(string linkType, string identifier)
        {
            var link = string.Empty;

            if (linkType == "image")
            {
                link = $"/images/{identifier}";
            }
            else if (linkType == "namespace")
            {
                link = $"#/{linkType}/{identifier}";
            }
            else if (linkType == "type")
            {
                // For now, just return empty navigation to current type
                link = "#";
                //link = $"#/{linkType}/{identifier.RemoveIllegalPathChars()}/index";
            }
            else if (linkType == "article")
            {
                link = $"#/{linkType}/{identifier}";
            }
            else // Member
            {
                var identifierWithoutPrefix = identifier
                    .Replace("field.", "")
                    .Replace("event.", "")
                    .Replace("property.", "");

                var regEx = new Regex(@"\.(?![^\(]*\))");
                var splittedId = regEx.Split(identifierWithoutPrefix);
                link = $"#/type/{string.Join(".", splittedId.Take(splittedId.Length - 1)).Trim('.').RemoveIllegalPathChars()}/{identifier.RemoveIllegalHtmlIdChars()}";
            }

            return link;
        }
    }
}