using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileSystemAnalyzer.Core.Models;
using FileSystemAnalyzer.Core.Utilities;

namespace FileSystemAnalyzer.UI.Forms
{
    public partial class SearchForm : Form
    {
        private readonly DirectoryNode _rootNode;
        private readonly List<FileNode> _searchResults;
        private readonly Color _accentColor = Color.FromArgb(0, 120, 215);
        private readonly Color _secondaryColor = Color.FromArgb(243, 243, 243);
        
        public SearchForm(DirectoryNode rootNode)
        {
            InitializeComponent();
            
            _rootNode = rootNode;
            _searchResults = new List<FileNode>();
            
            // Set up search options
            cmbSearchType.SelectedIndex = 0; // Default to "Name" search
            chkCaseSensitive.Checked = false;
            
            // Register event handlers
            listViewResults.DoubleClick += listViewResults_DoubleClick;
            
            // Apply modern UI styles
            ApplyModernStyles();
        }
        
        private void ApplyModernStyles()
        {
            // Form properties
            this.Text = "Search Files";
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new Size(600, 500);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            
            // Style the buttons
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
                else if (control is ComboBox comboBox)
                {
                    StyleComboBox(comboBox);
                }
                else if (control is CheckBox checkBox)
                {
                    StyleCheckBox(checkBox);
                }
                else if (control is ListView listView)
                {
                    StyleListView(listView);
                }
                else if (control is Label label)
                {
                    StyleLabel(label);
                }
                else if (control is ProgressBar progressBar)
                {
                    StyleProgressBar(progressBar);
                }
            }
            
            // Set tooltips
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
        
        private void StyleComboBox(ComboBox comboBox)
        {
            comboBox.FlatStyle = FlatStyle.Flat;
            comboBox.Font = new Font("Segoe UI", 9F);
            comboBox.BackColor = Color.White;
        }
        
        private void StyleCheckBox(CheckBox checkBox)
        {
            checkBox.FlatStyle = FlatStyle.Flat;
            checkBox.Font = new Font("Segoe UI", 9F);
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
        
        private void StyleLabel(Label label)
        {
            label.Font = new Font("Segoe UI", 9F);
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
            
            toolTip.SetToolTip(btnSearch, "Execute the search with the specified criteria");
            toolTip.SetToolTip(txtSearchPattern, "Enter search pattern (supports wildcards * and ?)");
            toolTip.SetToolTip(cmbSearchType, "Select the type of search to perform");
            toolTip.SetToolTip(chkCaseSensitive, "Match case when searching");
            toolTip.SetToolTip(btnCancel, "Close the search window");
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchPattern.Text))
            {
                MessageBox.Show("Please enter a search pattern.", "Search Pattern Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Disable UI during search
            SetControlsEnabled(false);
            progressBar.Value = 0;
            lblStatus.Text = "Searching...";
            
            try
            {
                // Clear previous results
                listViewResults.Items.Clear();
                _searchResults.Clear();
                
                // Get search options
                string searchPattern = txtSearchPattern.Text;
                bool caseSensitive = chkCaseSensitive.Checked;
                SearchType searchType = (SearchType)cmbSearchType.SelectedIndex;
                
                // Perform search
                await Task.Run(() => Search(_rootNode, searchPattern, searchType, caseSensitive));
                
                // Update results list
                UpdateSearchResults();
                
                lblStatus.Text = $"Found {_searchResults.Count} results";
                
                // Highlight result count based on number of results
                if (_searchResults.Count == 0)
                {
                    lblStatus.ForeColor = Color.Red;
                }
                else if (_searchResults.Count > 100)
                {
                    lblStatus.ForeColor = Color.Green;
                }
                else
                {
                    lblStatus.ForeColor = _accentColor;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during search: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Search failed";
                lblStatus.ForeColor = Color.Red;
            }
            finally
            {
                // Re-enable UI
                SetControlsEnabled(true);
            }
        }

        private void Search(DirectoryNode node, string pattern, SearchType searchType, bool caseSensitive)
        {
            // Process files in this directory
            foreach (FileNode file in node.Files)
            {
                bool match = false;
                
                switch (searchType)
                {
                    case SearchType.Name:
                        match = PathHelper.MatchesWildcard(file.Name, pattern, caseSensitive);
                        break;
                    
                    case SearchType.Extension:
                        match = PathHelper.MatchesWildcard(file.Extension, pattern, caseSensitive);
                        break;
                    
                    case SearchType.Size:
                        if (long.TryParse(pattern, out long size))
                        {
                            match = file.Size >= size; // Match files larger than the specified size
                        }
                        break;
                    
                    case SearchType.Path:
                        match = PathHelper.MatchesWildcard(file.Path, pattern, caseSensitive);
                        break;
                }
                
                if (match)
                {
                    lock (_searchResults)
                    {
                        _searchResults.Add(file);
                    }
                }
            }
            
            // Process subdirectories
            foreach (DirectoryNode subDir in node.Subdirectories)
            {
                Search(subDir, pattern, searchType, caseSensitive);
            }
        }

        private void UpdateSearchResults()
        {
            listViewResults.Items.Clear();
            
            int index = 0;
            foreach (var file in _searchResults)
            {
                var item = new ListViewItem(file.Name);
                item.SubItems.Add(file.GetFormattedSize());
                item.SubItems.Add(file.Extension);
                item.SubItems.Add(PathHelper.ShortenPath(Path.GetDirectoryName(file.Path), 60));
                item.Tag = file;
                
                // Apply alternating row colors
                item.BackColor = index % 2 == 0 ? Color.White : _secondaryColor;
                
                listViewResults.Items.Add(item);
                index++;
            }
        }

        private void SetControlsEnabled(bool enabled)
        {
            btnSearch.Enabled = enabled;
            txtSearchPattern.Enabled = enabled;
            cmbSearchType.Enabled = enabled;
            chkCaseSensitive.Enabled = enabled;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        // Add double-click support for search results
        private void listViewResults_DoubleClick(object sender, EventArgs e)
        {
            if (listViewResults.SelectedItems.Count > 0 && listViewResults.SelectedItems[0].Tag is FileNode file)
            {
                // Show file info in a dialog or open containing folder
                try
                {
                    string folder = Path.GetDirectoryName(file.Path);
                    if (Directory.Exists(folder))
                    {
                        System.Diagnostics.Process.Start("explorer.exe", folder);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    public enum SearchType
    {
        Name,
        Extension,
        Size,
        Path
    }
} 