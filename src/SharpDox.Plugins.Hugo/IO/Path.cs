// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Path.cs" company="Catel development team">
//   Copyright (c) 2008 - 2015 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Catel.IO
{
    using System;

    /// <summary>
    /// Static class that implements some path methods
    /// </summary>
    public static class Path
    {
        /// <summary>
        /// Returns a relative path string from a full path.
        /// <para />
        /// The path to convert. Can be either a file or a directory
        /// The base path to truncate to and replace
        /// <para />
        /// Lower case string of the relative path. If path is a directory it's returned 
        /// without a backslash at the end.
        /// <para />
        /// Examples of returned values:
        ///  .\test.txt, ..\test.txt, ..\..\..\test.txt, ., ..
        /// </summary>
        /// <param name="fullPath">Full path to convert to relative path.</param>
        /// <param name="basePath">The base path (a.k.a. working directory). If this parameter is <c>null</c> or empty, the current working directory will be used.</param>
        /// <returns>Relative path.</returns>
        /// <exception cref="ArgumentException">The <paramref name="fullPath"/> is <c>null</c> or whitespace.</exception>
        public static string GetRelativePath(string fullPath, string basePath = null)
        {
            //Argument.IsNotNullOrWhitespace("fullPath", fullPath);

#if !NETFX_CORE && !PCL
            if (string.IsNullOrEmpty(basePath))
            {
                basePath = Environment.CurrentDirectory;
            }
#endif

            fullPath = RemoveTrailingSlashes(fullPath);
            basePath = RemoveTrailingSlashes(basePath);

            // Check if the base path is really the full path (not just a subpath, for example "C:\MyTes" in "C:\MyTest")
            var fullPathWithTrailingBackslash = AppendTrailingSlash(fullPath).ToLower();
            var basePathWithTrailingBackslash = AppendTrailingSlash(basePath).ToLower();

            if (fullPathWithTrailingBackslash.IndexOf(basePathWithTrailingBackslash, StringComparison.OrdinalIgnoreCase) > -1)
            {
                var result = fullPath.Replace(basePath, string.Empty);
                if (result.StartsWith("\\"))
                {
                    result = result.Remove(0, 1);
                }

                return result;
            }

            var backDirs = string.Empty;
            var partialPath = basePath;
            var index = partialPath.LastIndexOf("\\");
            while (index > 0)
            {
                partialPath = AppendTrailingSlash(partialPath.Substring(0, index));
                backDirs = backDirs + "..\\";

                if (fullPathWithTrailingBackslash.IndexOf(partialPath, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    partialPath = RemoveTrailingSlashes(partialPath);
                    fullPath = RemoveTrailingSlashes(fullPath);

                    if (fullPath == partialPath)
                    {
                        // *** Full Directory match and need to replace it all
                        return fullPath.Replace(partialPath, backDirs.Substring(0, backDirs.Length - 1));
                    }
                    else
                    {
                        // *** We're dealing with a file or a start path
                        return fullPath.Replace(partialPath + (fullPath == partialPath ? string.Empty : "\\"), backDirs);
                    }
                }

                partialPath = RemoveTrailingSlashes(partialPath);
                index = partialPath.LastIndexOf("\\", partialPath.Length - 1);
            }

            return fullPath;
        }

        /// <summary>
        /// Appends a trailing backslash (\) to the path.
        /// </summary>
        /// <param name="path">Path to append the trailing backslash to.</param>
        /// <returns>Path including the trailing backslash.</returns>
        /// <exception cref="ArgumentException">The <paramref name="path"/> is <c>null</c> or whitespace.</exception>
        public static string AppendTrailingSlash(string path)
        {
            return AppendTrailingSlash(path, '\\');
        }

        /// <summary>
        /// Appends a trailing slash (\ or /) to the path.
        /// </summary>
        /// <param name="path">Path to append the trailing slash to.</param>
        /// <param name="slash">Slash to append (\ or /).</param>
        /// <returns>Path including the trailing slash.</returns>
        /// <exception cref="ArgumentException">The <paramref name="path"/> is <c>null</c> or whitespace.</exception>
        public static string AppendTrailingSlash(string path, char slash)
        {
            //Argument.IsNotNullOrWhitespace("path", path);

            if (path[path.Length - 1] == slash)
            {
                return path;
            }

            return path + slash;
        }
        
        /// <summary>
        /// Replaces path slashes (\) by url slashes (/).
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Prepared url.</returns>
        /// <exception cref="ArgumentException">The <paramref name="value"/> is <c>null</c> or whitespace.</exception>
        internal static string ReplacePathSlashesByUrlSlashes(string value)
        {
            //Argument.IsNotNullOrWhitespace("value", value);

            return value.Replace("\\", "/");
        }

        /// <summary>
        /// Removes any slashes (\ or /) at the beginning of the string.
        /// </summary>
        /// <param name="value">Value to remove the slashes from.</param>
        /// <returns>Value without slashes.</returns>
        /// <exception cref="ArgumentException">The <paramref name="value"/> is <c>null</c> or whitespace.</exception>
        internal static string RemoveStartSlashes(string value)
        {
            //Argument.IsNotNullOrWhitespace("value", value);

            while ((value.Length > 0) && ((value[0] == '\\') || (value[0] == '/')))
            {
                value = value.Remove(0, 1);
            }

            return value;
        }

        /// <summary>
        /// Removes any slashes (\ or /) at the end of the string.
        /// </summary>
        /// <param name="value">Value to remove the slashes from.</param>
        /// <returns>Value without slashes.</returns>
        /// <exception cref="ArgumentException">The <paramref name="value"/> is <c>null</c> or whitespace.</exception>
        internal static string RemoveTrailingSlashes(string value)
        {
            //Argument.IsNotNullOrWhitespace("value", value);

            while ((value.Length > 0) && ((value[value.Length - 1] == '\\') || (value[value.Length - 1] == '/')))
            {
                value = value.Remove(value.Length - 1, 1);
            }

            return value;
        }

        /// <summary>
        /// Removes any slashes (\ or /) at the beginning and end of the string.
        /// </summary>
        /// <param name="value">Value to remove the slashes from.</param>
        /// <returns>Value without trailing slashes.</returns>
        /// <exception cref="ArgumentException">The <paramref name="value"/> is <c>null</c> or whitespace.</exception>
        internal static string RemoveStartAndTrailingSlashes(string value)
        {
            //Argument.IsNotNullOrWhitespace("value", value);

            value = RemoveStartSlashes(value);
            value = RemoveTrailingSlashes(value);

            return value;
        }
    }
}