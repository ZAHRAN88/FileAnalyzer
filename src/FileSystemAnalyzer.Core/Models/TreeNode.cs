using System;
using System.Collections.Generic;

namespace FileSystemAnalyzer.Core.Models
{
    /// <summary>
    /// Base class for all tree nodes in the file system
    /// </summary>
    public abstract class TreeNode
    {
        /// <summary>
        /// Gets or sets the name of the node
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the full path of the node
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the size of the node in bytes
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// Gets or sets the parent node
        /// </summary>
        public DirectoryNode? Parent { get; set; }

        /// <summary>
        /// Constructor for TreeNode
        /// </summary>
        /// <param name="name">The name of the node</param>
        /// <param name="path">The full path of the node</param>
        protected TreeNode(string name, string path)
        {
            Name = name;
            Path = path;
            Size = 0;
        }

        /// <summary>
        /// Gets the formatted size string (KB, MB, GB)
        /// </summary>
        /// <returns>Formatted size string</returns>
        public string GetFormattedSize()
        {
            const long KB = 1024;
            const long MB = KB * 1024;
            const long GB = MB * 1024;

            return Size switch
            {
                < KB => $"{Size} B",
                < MB => $"{Math.Round((double)Size / KB, 2)} KB",
                < GB => $"{Math.Round((double)Size / MB, 2)} MB",
                _ => $"{Math.Round((double)Size / GB, 2)} GB"
            };
        }
    }
} 