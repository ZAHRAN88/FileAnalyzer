using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FileSystemAnalyzer.Core.Models;
using FileSystemAnalyzer.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileSystemAnalyzer.Tests
{
    [TestClass]
    public class FileHasherTests
    {
        private string _testDirectoryPath;
        private DirectoryNode _rootNode;
        private FileHasher _hasher;
        
        [TestInitialize]
        public async Task Initialize()
        {
            _hasher = new FileHasher();
            
            // Create a temporary test directory
            _testDirectoryPath = Path.Combine(Path.GetTempPath(), "FileHasherTests_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testDirectoryPath);
            
            // Create test structure with duplicate files
            // Root
            // ├── Original.txt (content: "test data")
            // ├── Duplicate1.txt (content: "test data")
            // └── Different.txt (content: "different data")
            
            File.WriteAllText(Path.Combine(_testDirectoryPath, "Original.txt"), "test data");
            File.WriteAllText(Path.Combine(_testDirectoryPath, "Duplicate1.txt"), "test data");
            File.WriteAllText(Path.Combine(_testDirectoryPath, "Different.txt"), "different data");
            
            // Scan the directory to build the directory tree
            DirectoryScanner scanner = new DirectoryScanner();
            _rootNode = await scanner.ScanDirectoryAsync(_testDirectoryPath);
        }
        
        [TestCleanup]
        public void Cleanup()
        {
            // Delete test directory
            if (Directory.Exists(_testDirectoryPath))
            {
                try
                {
                    Directory.Delete(_testDirectoryPath, true);
                }
                catch
                {
                    // Ignore errors during cleanup
                }
            }
        }
        
        [TestMethod]
        public async Task FindDuplicates_WithDuplicateFiles_DetectsCorrectly()
        {
            // Act
            Dictionary<string, List<FileNode>> duplicates = await _hasher.FindDuplicatesAsync(_rootNode);
            
            // Assert
            Assert.IsNotNull(duplicates, "Duplicates dictionary should not be null");
            Assert.AreEqual(1, duplicates.Count, "Should find 1 group of duplicates");
            
            // Get the first (and only) group of duplicates
            List<FileNode> duplicateGroup = duplicates.Values.First();
            Assert.AreEqual(2, duplicateGroup.Count, "Duplicate group should contain 2 files");
            
            // Verify the duplicate files are "Original.txt" and "Duplicate1.txt"
            bool originalFound = duplicateGroup.Any(f => f.Name == "Original.txt");
            bool duplicateFound = duplicateGroup.Any(f => f.Name == "Duplicate1.txt");
            
            Assert.IsTrue(originalFound, "Original.txt should be in the duplicate group");
            Assert.IsTrue(duplicateFound, "Duplicate1.txt should be in the duplicate group");
        }
        
        [TestMethod]
        public async Task FindDuplicates_AfterCalculating_SetsHashValues()
        {
            // Act
            await _hasher.FindDuplicatesAsync(_rootNode);
            
            // Assert
            // Verify all files have hash values
            foreach (FileNode file in _rootNode.Files)
            {
                Assert.IsFalse(string.IsNullOrEmpty(file.Hash), $"File {file.Name} should have a hash value");
            }
            
            // Verify duplicate files have the same hash
            FileNode original = _rootNode.Files.Find(f => f.Name == "Original.txt");
            FileNode duplicate = _rootNode.Files.Find(f => f.Name == "Duplicate1.txt");
            FileNode different = _rootNode.Files.Find(f => f.Name == "Different.txt");
            
            Assert.AreEqual(original.Hash, duplicate.Hash, "Original and duplicate should have the same hash");
            Assert.AreNotEqual(original.Hash, different.Hash, "Original and different should have different hashes");
        }
        
        [TestMethod]
        public async Task FindDuplicates_AfterCalculating_SetsIsDuplicateFlag()
        {
            // Act
            await _hasher.FindDuplicatesAsync(_rootNode);
            
            // Assert
            FileNode original = _rootNode.Files.Find(f => f.Name == "Original.txt");
            FileNode duplicate = _rootNode.Files.Find(f => f.Name == "Duplicate1.txt");
            FileNode different = _rootNode.Files.Find(f => f.Name == "Different.txt");
            
            Assert.IsFalse(original.IsDuplicate, "Original file should not be marked as a duplicate");
            Assert.IsTrue(duplicate.IsDuplicate, "Duplicate file should be marked as a duplicate");
            Assert.IsFalse(different.IsDuplicate, "Different file should not be marked as a duplicate");
        }
    }
} 