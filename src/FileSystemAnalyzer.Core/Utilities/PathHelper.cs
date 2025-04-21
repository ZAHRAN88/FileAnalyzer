using System;
using System.IO;
using System.Text.RegularExpressions;

namespace FileSystemAnalyzer.Core.Utilities
{
    /// <summary>
    /// Helper class for working with file paths
    /// </summary>
    public static class PathHelper
    {
        /// <summary>
        /// Checks if a file matches a wildcard pattern
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <param name="pattern">The wildcard pattern</param>
        /// <param name="caseSensitive">Whether the matching is case-sensitive</param>
        /// <returns>True if the file matches the pattern, false otherwise</returns>
        public static bool MatchesWildcard(string fileName, string pattern, bool caseSensitive = false)
        {
            // Convert wildcard pattern to regex
            string regexPattern = WildcardToRegex(pattern);
            
            // Create regex options
            RegexOptions options = RegexOptions.Singleline;
            if (!caseSensitive)
            {
                options |= RegexOptions.IgnoreCase;
            }
            
            // Match using regex
            return Regex.IsMatch(fileName, regexPattern, options);
        }

        /// <summary>
        /// Converts a wildcard pattern to a regular expression
        /// </summary>
        /// <param name="pattern">The wildcard pattern</param>
        /// <returns>A regular expression pattern</returns>
        private static string WildcardToRegex(string pattern)
        {
            return "^" + Regex.Escape(pattern)
                .Replace("\\*", ".*")
                .Replace("\\?", ".")
                + "$";
        }

        /// <summary>
        /// Shortens a path by replacing middle parts with ellipsis if it exceeds the maximum length
        /// </summary>
        /// <param name="path">The path to shorten</param>
        /// <param name="maxLength">The maximum length</param>
        /// <returns>The shortened path</returns>
        public static string ShortenPath(string path, int maxLength)
        {
            if (string.IsNullOrEmpty(path) || path.Length <= maxLength)
            {
                return path;
            }

            string filename = Path.GetFileName(path);
            string directory = Path.GetDirectoryName(path) ?? string.Empty;
            
            int filenameLength = filename.Length;
            int directoryLength = directory.Length;
            
            if (filenameLength >= maxLength - 3)
            {
                // If the filename itself is longer than maxLength, truncate it
                return "..." + filename.Substring(filenameLength - (maxLength - 3));
            }
            
            int remainingLength = maxLength - filenameLength - 3; // 3 for "..."
            
            if (remainingLength <= 0)
            {
                return "..." + filename;
            }
            
            string shortenedDirectory = directory.Substring(0, Math.Min(remainingLength, directoryLength));
            
            return shortenedDirectory + "..." + filename;
        }
    }
} 