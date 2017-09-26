// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkExtensions.cs" company="CatenaLogic">
//   Copyright (c) 2008 - 2017 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SharpDox.Plugins.Hugo
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Model.Repository;
    using Model.Repository.Members;
    using Steps;

    public static class LinkExtensions
    {
        private static readonly List<string> NewlineReplacements = new List<string>();

        static LinkExtensions()
        {
            for (int i = 25; i > 2; i--)
            {
                var newlinesToReplace = string.Empty;

                for (int j = 0; j < i; j++)
                {
                    newlinesToReplace += '\n';
                }

                NewlineReplacements.Add(newlinesToReplace);
            }
        }

        public static string ResolvePath(this SDMemberBase member)
        {
            return "./home.md";
        }

        public static string ResolvePath(this SDType type, string rootPath = "/", bool makeRelative = false)
        {
            if (type.IsProjectStranger)
            {
                // Probably a generic type (e.g. Catel.Disposable<Object>, cannot resolve assembly
                return string.Empty;
            }

            return ResolvePath(type.Namespace, type.Name, rootPath, makeRelative);
        }

        public static string ResolvePath(this SDNamespace typeNamespace, string typeName, string rootPath = "/", bool makeRelative = false)
        {
            return ResolvePath(typeNamespace?.Fullname, typeNamespace?.Assemblyname, typeName, rootPath, makeRelative);
        }

        public static string ResolvePath(this string typeNamespace, string assemblyName, string typeName, string rootPath = "/", bool makeRelative = false)
        {
            var typeNamespaceFullName = typeNamespace.Replace("GlobalNamespace", string.Empty);

            if (string.IsNullOrWhiteSpace(assemblyName))
            {
                // We can't resolve it
                return string.Empty;
            }

            // A directory consists of [assembly]/[namespace] (e.g. catel-core/catel/logging/)
            var assemblyNameForPath = $"{assemblyName.RemoveIllegalPathChars()}";
            var namespaceForPath = string.Join(Path.DirectorySeparatorChar.ToString(), typeNamespaceFullName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries));
            var fileNameForPath = typeName?.RemoveIllegalPathChars() ?? "_index";

            var fileName = Path.Combine(rootPath, assemblyNameForPath, namespaceForPath, fileNameForPath + ".md");
            if (makeRelative && !string.Equals(rootPath, "/"))
            {
                fileName = Catel.IO.Path.GetRelativePath(fileName, rootPath);
            }

            return fileName;
        }

        public static string CleanUp(this string content)
        {
            // Replace < and >
            content = content.Replace("<", "&lt;").Replace(">", "&gt;");

            foreach (var newlineReplacement in NewlineReplacements)
            {
                content = content.Replace(newlineReplacement, "\n\n");
            }

            return content;
        }
    }
}