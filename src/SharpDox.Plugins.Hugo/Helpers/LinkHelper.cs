// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkHelper.cs" company="CatenaLogic">
//   Copyright (c) 2008 - 2017 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SharpDox.Plugins.Hugo
{
    using Model.Repository;

    internal static class LinkHelper
    {
        public static string CreateLocalLink(SDType type, string rootPath, string rootPrefix)
        {
            var relativeFileName = type.ResolvePath(rootPath, true);
            return CreateLocalLinkFromRelativePath(relativeFileName, rootPrefix);
        }

        public static string CreateLocalLink(string fileName, string rootPath, string rootPrefix)
        {
            var relativeFileName = Catel.IO.Path.GetRelativePath(fileName, rootPath);
            return CreateLocalLinkFromRelativePath(relativeFileName, rootPrefix);
        }

        private static string CreateLocalLinkFromRelativePath(string relativeFileName, string rootPrefix)
        {
            // {{< relref "<#= RootPrefix #>/<#= namespaceType.ResolvePath(RootPath, true).Replace("\\", "/") #>" >}}

            var prefix = rootPrefix ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                prefix += "/";
            }

            if (string.IsNullOrWhiteSpace(relativeFileName))
            {
                prefix = string.Empty;
                relativeFileName = "#";
            }
            else
            {
                relativeFileName = relativeFileName.Replace("\\", "/");
            }

            return $"{{{{< relref \"{prefix}{relativeFileName}\" >}}}}";
        }
    }
}