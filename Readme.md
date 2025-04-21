# File System Directory Tree Analyzer

## Project Overview
A C# Windows application that provides a comprehensive analysis of file system directories. This project creates a visual tree representation of directory structures while offering advanced features like size calculation, wildcard search, and duplicate file detection.

## Team Members
- Mohamed Osama Zahran
- Mohamed Elsayed Zahran
- Ali Ibrahim Fahmy

## Features
### 1. Directory Tree Visualization
- Displays a hierarchical view of directories and files
- Uses ASCII characters for tree structure representation
- Shows proper indentation for nested directories
- Includes file/directory names and types
- Real-time directory structure updates

### 2. Directory Size Analysis
- Calculates total size of directories
- Provides size information in human-readable format (KB, MB, GB)
- Shows individual file sizes
- Generates size distribution statistics
- Asynchronous size calculation for large directories

### 3. File Search Capabilities
- Supports wildcard pattern matching (*, ?)
- Allows searching by file extensions
- Enables searching by file name patterns
- Includes case-sensitive/insensitive search options
- Regular expression support

### 4. Duplicate File Detection
- Uses MD5/SHA-256 hashing for file comparison
- Identifies duplicate files across directories
- Groups duplicate files together
- Shows file locations and sizes
- Parallel processing for faster duplicate detection

## Technical Requirements
- .NET 8.0 or later
- Visual Studio 2022 or later
- Windows 10/11
- NuGet Packages:
  - System.IO
  - System.Security.Cryptography
  - System.Threading.Tasks

## Project Structure
```
FileSystemAnalyzer/
├── src/
│   ├── FileSystemAnalyzer.Core/
│   │   ├── Models/
│   │   │   ├── DirectoryNode.cs
│   │   │   ├── FileNode.cs
│   │   │   └── TreeNode.cs
│   │   ├── Services/
│   │   │   ├── DirectoryScanner.cs
│   │   │   ├── FileHasher.cs
│   │   │   └── SizeCalculator.cs
│   │   └── Utilities/
│   │       ├── PathHelper.cs
│   │       └── FormatHelper.cs
│   ├── FileSystemAnalyzer.UI/
│   │   ├── Forms/
│   │   │   ├── MainForm.cs
│   │   │   ├── SearchForm.cs
│   │   │   └── DuplicatesForm.cs
│   │   └── Controls/
│   │       └── DirectoryTreeView.cs
│   └── FileSystemAnalyzer.Tests/
│       ├── DirectoryScannerTests.cs
│       └── FileHasherTests.cs
├── FileSystemAnalyzer.sln
└── README.md
```

## Class Structure
```csharp
public class DirectoryNode
{
    public string Path { get; set; }
    public List<FileNode> Files { get; set; }
    public List<DirectoryNode> Subdirectories { get; set; }
    public long TotalSize { get; set; }
}

public class FileNode
{
    public string Name { get; set; }
    public string Path { get; set; }
    public long Size { get; set; }
    public string Hash { get; set; }
}
```

## Implementation Details

The project has been implemented with the following structure:

### Core Library (FileSystemAnalyzer.Core)
Contains the domain models and core functionality:

1. **Models**
   - `TreeNode`: Base abstract class for all file system nodes
   - `DirectoryNode`: Represents a directory with subdirectories and files
   - `FileNode`: Represents a file with size and hash information

2. **Services**
   - `DirectoryScanner`: Recursively scans directories and builds the tree structure
   - `FileHasher`: Calculates file hashes and identifies duplicates
   - `SizeCalculator`: Provides size statistics and analysis

3. **Utilities**
   - `PathHelper`: Helper methods for path manipulation and wildcard matching

### UI Application (FileSystemAnalyzer.UI)
Provides a Windows Forms user interface:

1. **Forms**
   - `MainForm`: The main application window with directory tree, file listings, and statistics
   - `SearchForm`: Dialog for searching files using various criteria

2. **Features Implemented**
   - Directory tree visualization with proper indentation
   - File size calculation in human-readable format
   - Duplicate file detection using MD5 hashing
   - File search with wildcard pattern matching
   - Size statistics and analysis

### Tests (FileSystemAnalyzer.Tests)
Contains unit tests for the core functionality:

1. **Test Classes**
   - `DirectoryScannerTests`: Tests for directory scanning functionality
   - `FileHasherTests`: Tests for file hashing and duplicate detection

## How to Build and Run

1. **Requirements**
   - .NET 8.0 SDK or later
   - Visual Studio 2022 or later (or any IDE supporting .NET development)

2. **Build Instructions**
   ```bash
   # Clone the repository
   git clone [repository-url]
   
   # Navigate to the project directory
   cd file-system-directory-tree
   
   # Build the solution
   dotnet build
   
   # Run the application
   dotnet run --project src/FileSystemAnalyzer.UI/FileSystemAnalyzer.UI.csproj
   ```

3. **Run Tests**
   ```bash
   dotnet test
   ```

## Project Status
This project is fully functional with all of the initially planned features implemented:
- ✅ Directory tree visualization
- ✅ Directory size analysis
- ✅ File search capability
- ✅ Duplicate file detection

## Installation
1. Clone the repository
```bash
git clone [repository-url]
```
2. Open `FileSystemAnalyzer.sln` in Visual Studio
3. Restore NuGet packages
4. Build the solution

## Usage
1. Run the application
2. Select a directory to analyze
3. Use the following features:
   - Tree View: Displays directory structure
   - Size Analysis: Shows size distribution
   - Search: Find files using patterns
   - Duplicate Detection: Identify duplicate files

## Example Output
```
Directory: C:\Users\Documents
├── Projects\
│   ├── Source\
│   │   ├── Program.cs (2.5 KB)
│   │   └── Utils.cs (1.2 KB)
│   └── Docs\
│       └── README.md (0.5 KB)
└── Images\
    ├── photo1.jpg (1.5 MB)
    └── photo2.jpg (1.5 MB) [Duplicate]

Total Size: 3.0 MB
Duplicate Files Found: 1 group
```

## Development Guidelines
1. Code Style
   - Follow C# coding conventions
   - Use meaningful variable and method names
   - Add XML documentation comments
   - Implement exception handling

2. Best Practices
   - Use async/await for I/O operations
   - Implement IDisposable where necessary
   - Use SOLID principles
   - Write unit tests for core functionality

## Future Enhancements
- [ ] WPF/XAML modern UI implementation
- [ ] Export results to various formats (JSON, XML, CSV)
- [ ] Advanced filtering options
- [ ] Real-time directory monitoring using FileSystemWatcher
- [ ] Network directory support
- [ ] Compression analysis
- [ ] File type statistics
- [ ] Integration with Windows context menu

## Contributing
1. Fork the repository
2. Create a new branch for your feature
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License
This project is licensed under the MIT License - see the LICENSE file for details.

## Contact
For any queries regarding the project, please contact:
- Mohamed Osama Zahran
- Mohamed Elsayed Zahran
- Ali Ibrahim Fahmy

---
*Last Updated: [Current Date]*
