using System;
using System.IO;
using System.Threading.Tasks;
using FileSystemAnalyzer.Core.Models;
using FileSystemAnalyzer.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileSystemAnalyzer.Tests
{
    [TestClass]
    public class DirectoryScannerTests
    {
        private string _testDirectoryPath;
        private DirectoryScanner _scanner;
        
        [TestInitialize]
        public void Initialize()
        {
            _scanner = new DirectoryScanner();
            
            // Create a temporary test directory
            _testDirectoryPath = Path.Combine(Path.GetTempPath(), "FileSystemAnalyzerTests_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testDirectoryPath);
            
            // Create test structure
            // Root
            // ├── SubDir1
            // │   ├── File1.txt (10 bytes)
            // │   └── File2.txt (20 bytes)
            // └── SubDir2
            //     └── File3.txt (30 bytes)
            
            string subDir1 = Path.Combine(_testDirectoryPath, "SubDir1");
            string subDir2 = Path.Combine(_testDirectoryPath, "SubDir2");
            Directory.CreateDirectory(subDir1);
            Directory.CreateDirectory(subDir2);
            
            File.WriteAllText(Path.Combine(subDir1, "File1.txt"), "1234567890");
            File.WriteAllText(Path.Combine(subDir1, "File2.txt"), "12345678901234567890");
            File.WriteAllText(Path.Combine(subDir2, "File3.txt"), "123456789012345678901234567890");
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
        public async Task ScanDirectory_ValidPath_ReturnsCorrectStructure()
        {
            // Act
            DirectoryNode rootNode = await _scanner.ScanDirectoryAsync(_testDirectoryPath);
            
            // Assert
            Assert.IsNotNull(rootNode, "Root node should not be null");
            Assert.AreEqual(Path.GetFileName(_testDirectoryPath), rootNode.Name, "Root name should match");
            Assert.AreEqual(2, rootNode.Subdirectories.Count, "Root should have 2 subdirectories");
            Assert.AreEqual(0, rootNode.Files.Count, "Root should have 0 files");
            
            // Find SubDir1
            DirectoryNode subDir1 = rootNode.Subdirectories.Find(d => d.Name == "SubDir1");
            Assert.IsNotNull(subDir1, "SubDir1 should exist");
            Assert.AreEqual(2, subDir1.Files.Count, "SubDir1 should have 2 files");
            Assert.AreEqual(30, subDir1.Size, "SubDir1 total size should be 30 bytes");
            
            // Find SubDir2
            DirectoryNode subDir2 = rootNode.Subdirectories.Find(d => d.Name == "SubDir2");
            Assert.IsNotNull(subDir2, "SubDir2 should exist");
            Assert.AreEqual(1, subDir2.Files.Count, "SubDir2 should have 1 file");
            Assert.AreEqual(30, subDir2.Size, "SubDir2 total size should be 30 bytes");
            
            // Verify root size (total of all subdirectories)
            Assert.AreEqual(60, rootNode.Size, "Root total size should be 60 bytes");
        }
        
        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public async Task ScanDirectory_InvalidPath_ThrowsException()
        {
            // Act - should throw DirectoryNotFoundException
            await _scanner.ScanDirectoryAsync(Path.Combine(_testDirectoryPath, "NonExistentDirectory"));
        }
    }
} 