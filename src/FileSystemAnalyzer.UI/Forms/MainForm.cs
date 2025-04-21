using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileSystemAnalyzer.Core.Models;
using FileSystemAnalyzer.Core.Services;
using FileSystemAnalyzer.Core.Utilities;

namespace FileSystemAnalyzer.UI.Forms
{
    public partial class MainForm : Form
    {
        private readonly DirectoryScanner _directoryScanner;
        private readonly FileHasher _fileHasher;
        private readonly SizeCalculator _sizeCalculator;
        private DirectoryNode? _rootNode;
        private Dictionary<string, List<FileNode>>? _duplicateFiles;
        private readonly Color _accentColor = Color.FromArgb(0, 120, 215);
        private readonly Color _secondaryColor = Color.FromArgb(243, 243, 243);
        
        public MainForm()
        {
            InitializeComponent();
            
            _directoryScanner = new DirectoryScanner();
            _fileHasher = new FileHasher();
            _sizeCalculator = new SizeCalculator();
            
            // Subscribe to events
            _directoryScanner.DirectoryScanned += DirectoryScanner_DirectoryScanned;
            _directoryScanner.FileScanned += DirectoryScanner_FileScanned;
            _directoryScanner.ScanProgressChanged += DirectoryScanner_ScanProgressChanged;
            
            _fileHasher.FileHashed += FileHasher_FileHashed;
            _fileHasher.DuplicateFound += FileHasher_DuplicateFound;
            _fileHasher.HashProgressChanged += FileHasher_HashProgressChanged;
            
            // Apply modern UI styles
            ApplyModernStyles();
        }
        
        private void ApplyModernStyles()
        {
            // Form properties
            this.Text = "File System Analyzer";
            this.Icon = SystemIcons.Application;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1000, 700);
            
            // Style the controls
            foreach (Control control in this.Controls)
            {
                if (control is Button button)
                {
                    StyleButton(button);
                }
                else if (control is TextBox textBox)
                {
                    StyleTextBox(textBox);
                }
                else if (control is ListView listView)
                {
                    StyleListView(listView);
                }
                else if (control is TreeView treeView)
                {
                    StyleTreeView(treeView);
                }
                else if (control is TabControl tabControl)
                {
                    StyleTabControl(tabControl);
                }
                else if (control is ProgressBar progressBar)
                {
                    StyleProgressBar(progressBar);
                }
            }
            
            // Apply modern font
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            
            // Set tooltips for buttons
            SetTooltips();
        }
        
        private void StyleButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = _accentColor;
            button.BackColor = _accentColor;
            button.ForeColor = Color.White;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            button.Cursor = Cursors.Hand;
            button.Padding = new Padding(5);
            button.UseVisualStyleBackColor = false;
        }
        
        private void StyleTextBox(TextBox textBox)
        {
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Font = new Font("Segoe UI", 9F);
            textBox.BackColor = Color.White;
        }
        
        private void StyleListView(ListView listView)
        {
            listView.FullRowSelect = true;
            listView.GridLines = false;
            listView.View = View.Details;
            listView.BorderStyle = BorderStyle.FixedSingle;
            listView.BackColor = Color.White;
            listView.Font = new Font("Segoe UI", 9F);
        }
        
        private void StyleTreeView(TreeView treeView)
        {
            treeView.BorderStyle = BorderStyle.FixedSingle;
            treeView.BackColor = Color.White;
            treeView.Font = new Font("Segoe UI", 9F);
            treeView.ShowLines = true;
            treeView.ItemHeight = 22;
            treeView.HideSelection = false;
        }
        
        private void StyleTabControl(TabControl tabControl)
        {
            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.ItemSize = new Size(120, 30);
            tabControl.SizeMode = TabSizeMode.Fixed;
            tabControl.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            
            // Hook up the DrawItem event handler
            tabControl.DrawItem += TabControl_DrawItem;
        }
        
        private void TabControl_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (sender is not TabControl tabControl) return;
            
            var tabPage = tabControl.TabPages[e.Index];
            var tabRect = tabControl.GetTabRect(e.Index);
            
            // Selected tab appearance
            if (e.State == DrawItemState.Selected)
            {
                using (var brushSelected = new SolidBrush(_accentColor))
                using (var textBrush = new SolidBrush(Color.White))
                {
                    e.Graphics.FillRectangle(brushSelected, tabRect);
                    
                    var textFormat = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    
                    e.Graphics.DrawString(tabPage.Text, tabControl.Font, textBrush, tabRect, textFormat);
                }
            }
            // Unselected tab appearance
            else
            {
                using (var brushUnselected = new SolidBrush(_secondaryColor))
                using (var textBrush = new SolidBrush(Color.DarkGray))
                {
                    e.Graphics.FillRectangle(brushUnselected, tabRect);
                    
                    var textFormat = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    
                    e.Graphics.DrawString(tabPage.Text, tabControl.Font, textBrush, tabRect, textFormat);
                }
            }
        }
        
        private void StyleProgressBar(ProgressBar progressBar)
        {
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.Height = 18;
        }
        
        private void SetTooltips()
        {
            ToolTip toolTip = new ToolTip
            {
                AutoPopDelay = 5000,
                InitialDelay = 1000,
                ReshowDelay = 500,
                ShowAlways = true
            };
            
            toolTip.SetToolTip(btnBrowse, "Browse for a directory to analyze");
            toolTip.SetToolTip(btnScan, "Scan the selected directory");
            toolTip.SetToolTip(btnFindDuplicates, "Find duplicate files");
            toolTip.SetToolTip(btnSearch, "Search for files");
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Select a directory to analyze";
                folderBrowserDialog.UseDescriptionForTitle = true;
                
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    txtDirectoryPath.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private async void btnScan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDirectoryPath.Text) || !Directory.Exists(txtDirectoryPath.Text))
            {
                MessageBox.Show("Please select a valid directory.", "Invalid Directory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Disable UI during scan
            SetControlsEnabled(false);
            progressBar.Value = 0;
            lblStatus.Text = "Scanning directory...";
            
            try
            {
                // Clear previous results
                treeViewDirectory.Nodes.Clear();
                listViewFiles.Items.Clear();
                listViewDuplicates.Items.Clear();
                
                // Start the scan
                _rootNode = await _directoryScanner.ScanDirectoryAsync(txtDirectoryPath.Text);
                
                // Update status
                lblStatus.Text = "Building tree view...";
                
                // Populate tree view
                System.Windows.Forms.TreeNode rootTreeNode = new System.Windows.Forms.TreeNode(_rootNode.Name) { Tag = _rootNode };
                treeViewDirectory.Nodes.Add(rootTreeNode);
                PopulateTreeNode(rootTreeNode, _rootNode);
                rootTreeNode.Expand();
                
                // Update status
                lblStatus.Text = "Calculating size statistics...";
                
                // Calculate size statistics
                var sizeStats = await _sizeCalculator.CalculateSizeStatisticsAsync(_rootNode);
                UpdateSizeStatistics(sizeStats);
                
                // Update status
                lblStatus.Text = "Finding largest files...";
                
                // Find largest files
                var largestFiles = await _sizeCalculator.FindLargestFilesAsync(_rootNode);
                UpdateLargestFiles(largestFiles);
                
                // Update status
                lblStatus.Text = "Scan complete";
                
                // Enable "Find Duplicates" button if there are files
                btnFindDuplicates.Enabled = _rootNode.GetTotalFileCount() > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error scanning directory: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Scan failed";
            }
            finally
            {
                // Re-enable UI
                SetControlsEnabled(true);
            }
        }

        private async void btnFindDuplicates_Click(object sender, EventArgs e)
        {
            if (_rootNode == null)
            {
                return;
            }
            
            // Disable UI during hash calculation
            SetControlsEnabled(false);
            progressBar.Value = 0;
            lblStatus.Text = "Finding duplicate files...";
            
            try
            {
                // Clear previous results
                listViewDuplicates.Items.Clear();
                
                // Find duplicates
                _duplicateFiles = await _fileHasher.FindDuplicatesAsync(_rootNode);
                
                // Update duplicate file list
                UpdateDuplicateFiles(_duplicateFiles);
                
                lblStatus.Text = $"Found {_duplicateFiles.Count} duplicate file groups";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error finding duplicates: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Duplicate search failed";
            }
            finally
            {
                // Re-enable UI
                SetControlsEnabled(true);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (_rootNode == null) return;
            
            SearchForm searchForm = new SearchForm(_rootNode);
            searchForm.ShowDialog();
        }

        private void PopulateTreeNode(System.Windows.Forms.TreeNode treeNode, DirectoryNode directoryNode)
        {
            // Add subdirectories
            foreach (DirectoryNode subDir in directoryNode.Subdirectories)
            {
                System.Windows.Forms.TreeNode subTreeNode = new System.Windows.Forms.TreeNode(subDir.ToString()) { Tag = subDir };
                treeNode.Nodes.Add(subTreeNode);
                PopulateTreeNode(subTreeNode, subDir);
            }
            
            // Add files
            foreach (FileNode file in directoryNode.Files)
            {
                System.Windows.Forms.TreeNode fileTreeNode = new System.Windows.Forms.TreeNode(file.ToString()) { Tag = file };
                treeNode.Nodes.Add(fileTreeNode);
            }
        }

        private void UpdateSizeStatistics(List<SizeCalculator.DirSizeEntry> sizeStats)
        {
            listViewSizeStats.Items.Clear();
            
            foreach (var entry in sizeStats)
            {
                var item = new ListViewItem(entry.Name);
                item.SubItems.Add(FormatHelper.FormatSize(entry.Size));
                item.SubItems.Add($"{entry.Percentage}%");
                item.SubItems.Add(entry.FileCount.ToString());
                item.Tag = entry.Path;
                
                // Apply alternating row colors
                item.BackColor = listViewSizeStats.Items.Count % 2 == 0 ? Color.White : _secondaryColor;
                
                listViewSizeStats.Items.Add(item);
            }
        }

        private void UpdateLargestFiles(List<FileNode> largestFiles)
        {
            listViewFiles.Items.Clear();
            
            foreach (var file in largestFiles)
            {
                var item = new ListViewItem(file.Name);
                item.SubItems.Add(file.GetFormattedSize());
                item.SubItems.Add(PathHelper.ShortenPath(Path.GetDirectoryName(file.Path), 60));
                item.Tag = file;
                
                // Apply alternating row colors
                item.BackColor = listViewFiles.Items.Count % 2 == 0 ? Color.White : _secondaryColor;
                
                listViewFiles.Items.Add(item);
            }
        }

        private void UpdateDuplicateFiles(Dictionary<string, List<FileNode>> duplicateFiles)
        {
            listViewDuplicates.Items.Clear();
            
            foreach (var hash in duplicateFiles.Keys)
            {
                var files = duplicateFiles[hash];
                long totalSize = files.Sum(f => f.Size);
                long wastedSize = totalSize - files[0].Size;
                
                var groupItem = new ListViewItem($"Group: {files.Count} files");
                groupItem.Font = new Font(listViewDuplicates.Font, FontStyle.Bold);
                groupItem.BackColor = Color.FromArgb(220, 230, 241);
                groupItem.ForeColor = Color.FromArgb(20, 66, 114);
                groupItem.SubItems.Add(FormatHelper.FormatSize(totalSize));
                groupItem.SubItems.Add(FormatHelper.FormatSize(wastedSize));
                groupItem.SubItems.Add(files[0].Extension);
                groupItem.Tag = files;
                
                listViewDuplicates.Items.Add(groupItem);
                
                // Add individual files in the group
                int fileIndex = 0;
                foreach (var file in files)
                {
                    var fileItem = new ListViewItem($"  {file.Name}");
                    fileItem.SubItems.Add(file.GetFormattedSize());
                    
                    // Mark original vs duplicate with color coding
                    if (fileIndex == 0)
                    {
                        fileItem.SubItems.Add("Original");
                        fileItem.ForeColor = Color.DarkGreen;
                    }
                    else
                    {
                        fileItem.SubItems.Add("Duplicate");
                        fileItem.ForeColor = Color.DarkRed;
                    }
                    
                    fileItem.SubItems.Add(PathHelper.ShortenPath(Path.GetDirectoryName(file.Path), 60));
                    fileItem.Tag = file;
                    
                    // Apply alternating row colors within groups
                    fileItem.BackColor = fileIndex % 2 == 0 ? Color.White : _secondaryColor;
                    
                    listViewDuplicates.Items.Add(fileItem);
                    fileIndex++;
                }
            }
        }

        private void SetControlsEnabled(bool enabled)
        {
            btnScan.Enabled = enabled;
            btnBrowse.Enabled = enabled;
            btnFindDuplicates.Enabled = enabled && _rootNode != null;
            btnSearch.Enabled = enabled && _rootNode != null;
        }

        private void DirectoryScanner_DirectoryScanned(object sender, string e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => lblStatus.Text = $"Scanning: {PathHelper.ShortenPath(e, 60)}"));
            }
            else
            {
                lblStatus.Text = $"Scanning: {PathHelper.ShortenPath(e, 60)}";
            }
        }

        private void DirectoryScanner_FileScanned(object sender, string e)
        {
            // Optionally update UI for each file scanned (might be too frequent)
        }

        private void DirectoryScanner_ScanProgressChanged(object sender, int e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => {
                    progressBar.Value = Math.Min(e, 100);
                    // Change progressbar color based on progress
                    if (e < 30)
                        progressBar.ForeColor = Color.Red;
                    else if (e < 70)
                        progressBar.ForeColor = Color.Orange;
                    else
                        progressBar.ForeColor = Color.Green;
                }));
            }
            else
            {
                progressBar.Value = Math.Min(e, 100);
                // Change progressbar color based on progress
                if (e < 30)
                    progressBar.ForeColor = Color.Red;
                else if (e < 70)
                    progressBar.ForeColor = Color.Orange;
                else
                    progressBar.ForeColor = Color.Green;
            }
        }

        private void FileHasher_FileHashed(object sender, string e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => lblStatus.Text = $"Hashing: {PathHelper.ShortenPath(e, 60)}"));
            }
            else
            {
                lblStatus.Text = $"Hashing: {PathHelper.ShortenPath(e, 60)}";
            }
        }

        private void FileHasher_DuplicateFound(object sender, (string, string) e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => lblStatus.Text = $"Duplicate found: {Path.GetFileName(e.Item2)}"));
            }
            else
            {
                lblStatus.Text = $"Duplicate found: {Path.GetFileName(e.Item2)}";
            }
        }

        private void FileHasher_HashProgressChanged(object sender, int e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => {
                    progressBar.Value = Math.Min(e, 100);
                    // Change progressbar color based on progress
                    if (e < 30)
                        progressBar.ForeColor = Color.Red;
                    else if (e < 70)
                        progressBar.ForeColor = Color.Orange;
                    else
                        progressBar.ForeColor = Color.Green;
                }));
            }
            else
            {
                progressBar.Value = Math.Min(e, 100);
                // Change progressbar color based on progress
                if (e < 30)
                    progressBar.ForeColor = Color.Red;
                else if (e < 70)
                    progressBar.ForeColor = Color.Orange;
                else
                    progressBar.ForeColor = Color.Green;
            }
        }

        private void treeViewDirectory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag is DirectoryNode dirNode)
            {
                // Update file list to show files in the selected directory
                listViewFiles.Items.Clear();
                
                int index = 0;
                foreach (var file in dirNode.Files)
                {
                    var item = new ListViewItem(file.Name);
                    item.SubItems.Add(file.GetFormattedSize());
                    item.SubItems.Add(file.IsDuplicate ? "Duplicate" : "");
                    item.SubItems.Add(file.LastModifiedTime.ToString());
                    item.Tag = file;
                    
                    // Apply alternating row colors
                    item.BackColor = index % 2 == 0 ? Color.White : _secondaryColor;
                    
                    listViewFiles.Items.Add(item);
                    index++;
                }
            }
        }
    }

    // Helper class for formatting
    public static class FormatHelper
    {
        public static string FormatSize(long bytes)
        {
            const long KB = 1024;
            const long MB = KB * 1024;
            const long GB = MB * 1024;

            return bytes switch
            {
                < KB => $"{bytes} B",
                < MB => $"{Math.Round((double)bytes / KB, 2)} KB",
                < GB => $"{Math.Round((double)bytes / MB, 2)} MB",
                _ => $"{Math.Round((double)bytes / GB, 2)} GB"
            };
        }
    }
} 