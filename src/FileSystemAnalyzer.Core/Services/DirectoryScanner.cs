using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FileSystemAnalyzer.Core.Models;

namespace FileSystemAnalyzer.Core.Services
{
    /// <summary>
    /// Service for scanning directories and building a directory tree
    /// </summary>
    public class DirectoryScanner
    {
        /// <summary>
        /// Event raised when a directory is scanned
        /// </summary>
        public event EventHandler<string>? DirectoryScanned;

        /// <summary>
        /// Event raised when a file is scanned
        /// </summary>
        public event EventHandler<string>? FileScanned;

        /// <summary>
        /// Event raised when the scan progress changes
        /// </summary>
        public event EventHandler<int>? ScanProgressChanged;

        /// <summary>
        /// Gets or sets a flag indicating whether the scan is in progress
        /// </summary>
        public bool IsScanning { get; private set; }

        /// <summary>
        /// Gets or sets a flag indicating whether the scan should be canceled
        /// </summary>
        public bool CancelScan { get; set; }

        /// <summary>
        /// Scans a directory and returns a DirectoryNode representing the directory tree
        /// </summary>
        /// <param name="rootPath">The root directory path to scan</param>
        /// <returns>A DirectoryNode representing the directory tree</returns>
        public async Task<DirectoryNode> ScanDirectoryAsync(string rootPath)
        {
            if (!Directory.Exists(rootPath))
            {
                throw new DirectoryNotFoundException($"Directory not found: {rootPath}");
            }

            IsScanning = true;
            CancelScan = false;
            
            try
            {
                DirectoryInfo rootDirInfo = new DirectoryInfo(rootPath);
                string rootName = rootDirInfo.Name;
                
                DirectoryNode rootNode = new DirectoryNode(rootName, rootPath);
                
                await ScanDirectoryInternalAsync(rootNode);
                
                return rootNode;
            }
            finally
            {
                IsScanning = false;
            }
        }

        /// <summary>
        /// Internal method to recursively scan a directory
        /// </summary>
        /// <param name="node">The DirectoryNode to populate</param>
        private async Task ScanDirectoryInternalAsync(DirectoryNode node)
        {
            if (CancelScan)
            {
                return;
            }

            DirectoryInfo dirInfo = new DirectoryInfo(node.Path);
            
            // Scan files
            try
            {
                foreach (FileInfo fileInfo in dirInfo.GetFiles())
                {
                    if (CancelScan)
                    {
                        return;
                    }

                    try
                    {
                        FileNode fileNode = new FileNode(fileInfo.Name, fileInfo.FullName, fileInfo.Length);
                        node.AddFile(fileNode);
                        
                        FileScanned?.Invoke(this, fileInfo.FullName);
                    }
                    catch (Exception ex)
                    {
                        // Log or handle exception for individual files
                        Console.WriteLine($"Error scanning file: {fileInfo.FullName}, Error: {ex.Message}");
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Skip directories we don't have access to
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error scanning files in directory: {node.Path}, Error: {ex.Message}");
            }

            // Scan subdirectories
            try
            {
                DirectoryInfo[] subDirs = dirInfo.GetDirectories();
                
                for (int i = 0; i < subDirs.Length; i++)
                {
                    if (CancelScan)
                    {
                        return;
                    }

                    DirectoryInfo subDirInfo = subDirs[i];
                    
                    try
                    {
                        DirectoryNode subDirNode = new DirectoryNode(subDirInfo.Name, subDirInfo.FullName);
                        node.AddSubdirectory(subDirNode);
                        
                        DirectoryScanned?.Invoke(this, subDirInfo.FullName);
                        
                        // Report progress
                        ScanProgressChanged?.Invoke(this, (i + 1) * 100 / subDirs.Length);
                        
                        // Process subdirectory
                        await Task.Run(() => ScanDirectoryInternalAsync(subDirNode));
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // Skip directories we don't have access to
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error scanning subdirectory: {subDirInfo.FullName}, Error: {ex.Message}");
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Skip directories we don't have access to
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting subdirectories: {node.Path}, Error: {ex.Message}");
            }
        }
    }
} 