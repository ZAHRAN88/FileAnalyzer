using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FileSystemAnalyzer.Core.Models;

namespace FileSystemAnalyzer.Core.Services
{
    /// <summary>
    /// Service for calculating file hashes and detecting duplicates
    /// </summary>
    public class FileHasher
    {
        /// <summary>
        /// Event raised when a file hash is calculated
        /// </summary>
        public event EventHandler<string>? FileHashed;

        /// <summary>
        /// Event raised when duplicate files are found
        /// </summary>
        public event EventHandler<(string, string)>? DuplicateFound;

        /// <summary>
        /// Event raised when the hash progress changes
        /// </summary>
        public event EventHandler<int>? HashProgressChanged;

        /// <summary>
        /// Gets or sets a flag indicating whether the hashing is in progress
        /// </summary>
        public bool IsHashing { get; private set; }

        /// <summary>
        /// Gets or sets a flag indicating whether the hashing should be canceled
        /// </summary>
        public bool CancelHashing { get; set; }

        /// <summary>
        /// Calculates hashes for all files in the directory tree and identifies duplicates
        /// </summary>
        /// <param name="rootNode">The root directory node</param>
        /// <returns>A dictionary mapping hash values to lists of duplicate files</returns>
        public async Task<Dictionary<string, List<FileNode>>> FindDuplicatesAsync(DirectoryNode rootNode)
        {
            IsHashing = true;
            CancelHashing = false;
            
            // Dictionary to store hash -> files mapping
            Dictionary<string, List<FileNode>> hashToFiles = new Dictionary<string, List<FileNode>>();
            
            try
            {
                await HashFilesRecursivelyAsync(rootNode, hashToFiles);
                
                // Filter to include only hashes with multiple files (duplicates)
                Dictionary<string, List<FileNode>> duplicates = new Dictionary<string, List<FileNode>>();
                
                foreach (var hash in hashToFiles.Keys)
                {
                    if (hashToFiles[hash].Count > 1)
                    {
                        duplicates[hash] = hashToFiles[hash];
                        
                        // Mark files as duplicates (the first one is considered the original)
                        for (int i = 1; i < hashToFiles[hash].Count; i++)
                        {
                            hashToFiles[hash][i].IsDuplicate = true;
                            
                            DuplicateFound?.Invoke(this, (hashToFiles[hash][0].Path, hashToFiles[hash][i].Path));
                        }
                    }
                }
                
                return duplicates;
            }
            finally
            {
                IsHashing = false;
            }
        }

        /// <summary>
        /// Recursively calculate hashes for all files in the directory tree
        /// </summary>
        /// <param name="node">The current directory node</param>
        /// <param name="hashToFiles">Dictionary mapping hash values to file nodes</param>
        private async Task HashFilesRecursivelyAsync(DirectoryNode node, Dictionary<string, List<FileNode>> hashToFiles)
        {
            if (CancelHashing)
            {
                return;
            }

            // Process files in this directory
            int totalFiles = node.Files.Count;
            for (int i = 0; i < totalFiles; i++)
            {
                if (CancelHashing)
                {
                    return;
                }

                FileNode file = node.Files[i];
                
                try
                {
                    // Calculate hash
                    string hash = await CalculateFileHashAsync(file.Path);
                    file.Hash = hash;
                    
                    // Add to dictionary
                    if (!hashToFiles.ContainsKey(hash))
                    {
                        hashToFiles[hash] = new List<FileNode>();
                    }
                    
                    hashToFiles[hash].Add(file);
                    
                    FileHashed?.Invoke(this, file.Path);
                    HashProgressChanged?.Invoke(this, (i + 1) * 100 / totalFiles);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error hashing file: {file.Path}, Error: {ex.Message}");
                }
            }

            // Process subdirectories
            foreach (DirectoryNode subDir in node.Subdirectories)
            {
                if (CancelHashing)
                {
                    return;
                }
                
                await HashFilesRecursivelyAsync(subDir, hashToFiles);
            }
        }

        /// <summary>
        /// Calculates the hash for a file
        /// </summary>
        /// <param name="filePath">The path of the file</param>
        /// <returns>The hash value as a string</returns>
        private async Task<string> CalculateFileHashAsync(string filePath)
        {
            using (var md5 = MD5.Create())
            using (var stream = File.OpenRead(filePath))
            {
                byte[] hashBytes = await md5.ComputeHashAsync(stream);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }
    }
} 