using System;
using System.Collections.Generic;
using System.Linq;

namespace FileSystemAnalyzer.Core.Models
{
    /// <summary>
    /// Represents a directory in the file system tree
    /// </summary>
    public class DirectoryNode : TreeNode
    {
        /// <summary>
        /// Gets or sets the list of files in this directory
        /// </summary>
        public List<FileNode> Files { get; set; }

        /// <summary>
        /// Gets or sets the list of subdirectories in this directory
        /// </summary>
        public List<DirectoryNode> Subdirectories { get; set; }

        /// <summary>
        /// Gets or sets the creation time of the directory
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the last modification time of the directory
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// Constructor for DirectoryNode
        /// </summary>
        /// <param name="name">The name of the directory</param>
        /// <param name="path">The full path of the directory</param>
        public DirectoryNode(string name, string path) : base(name, path)
        {
            Files = new List<FileNode>();
            Subdirectories = new List<DirectoryNode>();
            CreationTime = System.IO.Directory.GetCreationTime(path);
            LastModifiedTime = System.IO.Directory.GetLastWriteTime(path);
        }

        /// <summary>
        /// Adds a file to this directory
        /// </summary>
        /// <param name="file">The file to add</param>
        public void AddFile(FileNode file)
        {
            file.Parent = this;
            Files.Add(file);
            UpdateSize();
        }

        /// <summary>
        /// Adds a subdirectory to this directory
        /// </summary>
        /// <param name="directory">The subdirectory to add</param>
        public void AddSubdirectory(DirectoryNode directory)
        {
            directory.Parent = this;
            Subdirectories.Add(directory);
            UpdateSize();
        }

        /// <summary>
        /// Updates the total size of this directory
        /// </summary>
        public void UpdateSize()
        {
            Size = Files.Sum(f => f.Size) + Subdirectories.Sum(d => d.Size);
            
            // Update parent sizes recursively
            Parent?.UpdateSize();
        }

        /// <summary>
        /// Gets the total number of files in this directory and all subdirectories
        /// </summary>
        /// <returns>Total file count</returns>
        public int GetTotalFileCount()
        {
            return Files.Count + Subdirectories.Sum(d => d.GetTotalFileCount());
        }

        /// <summary>
        /// Gets the total number of directories in this directory and all subdirectories
        /// </summary>
        /// <returns>Total directory count</returns>
        public int GetTotalDirectoryCount()
        {
            return Subdirectories.Count + Subdirectories.Sum(d => d.GetTotalDirectoryCount());
        }

        /// <summary>
        /// Returns a string representation of the directory node
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return $"{Name}/ ({GetFormattedSize()}, {GetTotalFileCount()} files, {GetTotalDirectoryCount()} dirs)";
        }
    }
} 