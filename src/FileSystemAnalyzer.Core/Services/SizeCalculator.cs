using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileSystemAnalyzer.Core.Models;

namespace FileSystemAnalyzer.Core.Services
{
    /// <summary>
    /// Service for calculating directory sizes
    /// </summary>
    public class SizeCalculator
    {
        /// <summary>
        /// Represents a directory size entry
        /// </summary>
        public class DirSizeEntry
        {
            /// <summary>
            /// Gets or sets the path of the directory
            /// </summary>
            public string Path { get; set; }
            
            /// <summary>
            /// Gets or sets the name of the directory
            /// </summary>
            public string Name { get; set; }
            
            /// <summary>
            /// Gets or sets the size of the directory
            /// </summary>
            public long Size { get; set; }
            
            /// <summary>
            /// Gets or sets the percentage of the total size
            /// </summary>
            public double Percentage { get; set; }
            
            /// <summary>
            /// Gets or sets the number of files in the directory
            /// </summary>
            public int FileCount { get; set; }
            
            /// <summary>
            /// Constructor for DirSizeEntry
            /// </summary>
            /// <param name="path">The path of the directory</param>
            /// <param name="name">The name of the directory</param>
            /// <param name="size">The size of the directory</param>
            /// <param name="percentage">The percentage of the total size</param>
            /// <param name="fileCount">The number of files in the directory</param>
            public DirSizeEntry(string path, string name, long size, double percentage, int fileCount)
            {
                Path = path;
                Name = name;
                Size = size;
                Percentage = percentage;
                FileCount = fileCount;
            }
        }

        /// <summary>
        /// Calculate size statistics for a directory tree
        /// </summary>
        /// <param name="rootNode">The root directory node</param>
        /// <returns>A list of directory size entries</returns>
        public async Task<List<DirSizeEntry>> CalculateSizeStatisticsAsync(DirectoryNode rootNode)
        {
            return await Task.Run(() =>
            {
                List<DirSizeEntry> results = new List<DirSizeEntry>();
                
                // If the root node has no size, return an empty list
                if (rootNode.Size == 0)
                {
                    return results;
                }
                
                // Add root node
                results.Add(new DirSizeEntry(
                    rootNode.Path,
                    rootNode.Name,
                    rootNode.Size,
                    100.0,
                    rootNode.GetTotalFileCount())
                );
                
                // Add immediate subdirectories
                foreach (DirectoryNode subDir in rootNode.Subdirectories)
                {
                    double percentage = (double)subDir.Size / rootNode.Size * 100;
                    
                    results.Add(new DirSizeEntry(
                        subDir.Path,
                        subDir.Name,
                        subDir.Size,
                        Math.Round(percentage, 2),
                        subDir.GetTotalFileCount())
                    );
                }
                
                // Sort by size (descending)
                return results.OrderByDescending(e => e.Size).ToList();
            });
        }

        /// <summary>
        /// Find the largest files in a directory tree
        /// </summary>
        /// <param name="rootNode">The root directory node</param>
        /// <param name="maxCount">The maximum number of files to return</param>
        /// <returns>A list of the largest files</returns>
        public async Task<List<FileNode>> FindLargestFilesAsync(DirectoryNode rootNode, int maxCount = 10)
        {
            return await Task.Run(() =>
            {
                List<FileNode> allFiles = new List<FileNode>();
                
                // Collect all files recursively
                CollectFilesRecursively(rootNode, allFiles);
                
                // Sort by size (descending) and take the top N
                return allFiles.OrderByDescending(f => f.Size).Take(maxCount).ToList();
            });
        }

        /// <summary>
        /// Recursively collect all files in a directory tree
        /// </summary>
        /// <param name="node">The current directory node</param>
        /// <param name="allFiles">The list to collect files into</param>
        private void CollectFilesRecursively(DirectoryNode node, List<FileNode> allFiles)
        {
            // Add files in this directory
            allFiles.AddRange(node.Files);
            
            // Process subdirectories
            foreach (DirectoryNode subDir in node.Subdirectories)
            {
                CollectFilesRecursively(subDir, allFiles);
            }
        }

        /// <summary>
        /// Calculate file extension statistics
        /// </summary>
        /// <param name="rootNode">The root directory node</param>
        /// <returns>A dictionary mapping file extensions to total size</returns>
        public async Task<Dictionary<string, long>> CalculateExtensionStatisticsAsync(DirectoryNode rootNode)
        {
            return await Task.Run(() =>
            {
                Dictionary<string, long> extensionToSize = new Dictionary<string, long>();
                
                CalculateExtensionStatisticsRecursively(rootNode, extensionToSize);
                
                return extensionToSize;
            });
        }

        /// <summary>
        /// Recursively calculate extension statistics
        /// </summary>
        /// <param name="node">The current directory node</param>
        /// <param name="extensionToSize">Dictionary mapping extensions to total size</param>
        private void CalculateExtensionStatisticsRecursively(DirectoryNode node, Dictionary<string, long> extensionToSize)
        {
            // Process files in this directory
            foreach (FileNode file in node.Files)
            {
                string ext = string.IsNullOrEmpty(file.Extension) ? "(no extension)" : file.Extension.ToLowerInvariant();
                
                if (!extensionToSize.ContainsKey(ext))
                {
                    extensionToSize[ext] = 0;
                }
                
                extensionToSize[ext] += file.Size;
            }
            
            // Process subdirectories
            foreach (DirectoryNode subDir in node.Subdirectories)
            {
                CalculateExtensionStatisticsRecursively(subDir, extensionToSize);
            }
        }
    }
} 