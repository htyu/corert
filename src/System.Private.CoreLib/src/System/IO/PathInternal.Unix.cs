// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Text;

namespace System.IO
{
    /// <summary>Contains internal path helpers that are shared between many projects.</summary>
    internal static partial class PathInternal
    {
        internal const char DirectorySeparatorChar = '/';
        internal const char AltDirectorySeparatorChar = '/';
        internal const char VolumeSeparatorChar = '/';
        internal const char PathSeparator = ':';

        internal const string DirectorySeparatorCharAsString = "/";

        // There is only one invalid path character in Unix
        private const char InvalidPathChar = '\0';

        internal const string ParentDirectoryPrefix = @"../";

        /// <summary>Returns a value indicating if the given path contains invalid characters.</summary>
        internal static bool HasIllegalCharacters(string path)
        {
            Debug.Assert(path != null);
            return path.IndexOf(InvalidPathChar) >= 0;
        }

        internal static int GetRootLength(string path)
        {
            return path.Length > 0 && IsDirectorySeparator(path[0]) ? 1 : 0;
        }

        internal static bool IsDirectorySeparator(char c)
        {
            // The alternate directory separator char is the same as the directory separator,
            // so we only need to check one.
            Debug.Assert(DirectorySeparatorChar == AltDirectorySeparatorChar);
            return c == DirectorySeparatorChar;
        }

        /// <summary>
        /// Returns true if the path is too long
        /// </summary>
        internal static bool IsPathTooLong(string fullPath)
        {
            return fullPath.Length >= Interop.Sys.MaxPath;
        }

        /// <summary>
        /// Returns true if the directory is too long
        /// </summary>
        internal static bool IsDirectoryTooLong(string fullPath)
        {
            return fullPath.Length >= Interop.Sys.MaxPath;
        }

        /// <summary>
        /// Normalize separators in the given path. Compresses forward slash runs.
        /// </summary>
        internal static string NormalizeDirectorySeparators(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;

            // Make a pass to see if we need to normalize so we can potentially skip allocating
            bool normalized = true;

            for (int i = 0; i < path.Length; i++)
            {
                if (IsDirectorySeparator(path[i])
                    && (i + 1 < path.Length && IsDirectorySeparator(path[i + 1])))
                {
                    normalized = false;
                    break;
                }
            }

            if (normalized) return path;

            StringBuilder builder = new StringBuilder(path.Length);

            for (int i = 0; i < path.Length; i++)
            {
                char current = path[i];

                // Skip if we have another separator following
                if (IsDirectorySeparator(current)
                    && (i + 1 < path.Length && IsDirectorySeparator(path[i + 1])))
                    continue;

                builder.Append(current);
            }

            return builder.ToString();
        }
        
        /// <summary>
        /// Returns true if the character is a directory or volume separator.
        /// </summary>
        /// <param name="ch">The character to test.</param>
        internal static bool IsDirectoryOrVolumeSeparator(char ch)
        {
            // The directory separator, volume separator, and the alternate directory
            // separator should be the same on Unix, so we only need to check one.
            Debug.Assert(DirectorySeparatorChar == AltDirectorySeparatorChar);
            Debug.Assert(DirectorySeparatorChar == VolumeSeparatorChar);
            return ch == DirectorySeparatorChar;
        }

        internal static bool HasInvalidVolumeSeparator(string path)
        {
            // This is only ever true for Windows
            return false;
        }

        internal static bool IsPartiallyQualified(string path)
        {
            // This is much simpler than Windows where paths can be rooted, but not fully qualified (such as Drive Relative)
            // As long as the path is rooted in Unix it doesn't use the current directory and therefore is fully qualified.
            return !Path.IsPathRooted(path);
        }
    }
}
