using System;

namespace FileSystemAnalyzer.Core.Models
{
    /// <summary>
    /// Represents a file in the file system tree
    /// </summary>
    public class FileNode : TreeNode
    {
        /// <summary>
        /// Gets or sets the file extension
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Gets or sets the hash value for duplicate detection
        /// </summary>
        public string? Hash { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this file is a duplicate
        /// </summary>
        public bool IsDuplicate { get; set; }

        /// <summary>
        /// Gets or sets the creation time of the file
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the last modification time of the file
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// Constructor for FileNode
        /// </summary>
        /// <param name="name">The name of the file</param>
        /// <param name="path">The full path of the file</param>
        /// <param name="size">The size of the file in bytes</param>
        public FileNode(string name, string path, long size) : base(name, path)
        {
            Size = size;
            Extension = System.IO.Path.GetExtension(name);
            IsDuplicate = false;
            CreationTime = File.GetCreationTime(path);
            LastModifiedTime = File.GetLastWriteTime(path);
        }

        /// <summary>
        /// Returns a string representation of the file node
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return $"{Name} ({GetFormattedSize()}){(IsDuplicate ? " [Duplicate]" : "")}";
        }
    }
} 