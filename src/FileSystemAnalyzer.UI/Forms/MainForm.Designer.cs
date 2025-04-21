using System.Windows.Forms;

namespace FileSystemAnalyzer.UI.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.treeViewDirectory = new System.Windows.Forms.TreeView();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageFiles = new System.Windows.Forms.TabPage();
            this.listViewFiles = new System.Windows.Forms.ListView();
            this.columnHeaderFileName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderSize = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderLocation = new System.Windows.Forms.ColumnHeader();
            this.tabPageSizeStats = new System.Windows.Forms.TabPage();
            this.listViewSizeStats = new System.Windows.Forms.ListView();
            this.columnHeaderDirectory = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderDirSize = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderPercentage = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderFileCount = new System.Windows.Forms.ColumnHeader();
            this.tabPageDuplicates = new System.Windows.Forms.TabPage();
            this.listViewDuplicates = new System.Windows.Forms.ListView();
            this.columnHeaderDupName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderDupSize = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderDupType = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderDupLocation = new System.Windows.Forms.ColumnHeader();
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnFindDuplicates = new System.Windows.Forms.Button();
            this.btnScan = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtDirectoryPath = new System.Windows.Forms.TextBox();
            this.lblDirectoryPath = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageFiles.SuspendLayout();
            this.tabPageSizeStats.SuspendLayout();
            this.tabPageDuplicates.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 65);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.treeViewDirectory);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tabControl);
            this.splitContainerMain.Size = new System.Drawing.Size(1024, 563);
            this.splitContainerMain.SplitterDistance = 341;
            this.splitContainerMain.TabIndex = 0;
            // 
            // treeViewDirectory
            // 
            this.treeViewDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewDirectory.Location = new System.Drawing.Point(0, 0);
            this.treeViewDirectory.Name = "treeViewDirectory";
            this.treeViewDirectory.Size = new System.Drawing.Size(341, 563);
            this.treeViewDirectory.TabIndex = 0;
            this.treeViewDirectory.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewDirectory_AfterSelect);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageFiles);
            this.tabControl.Controls.Add(this.tabPageSizeStats);
            this.tabControl.Controls.Add(this.tabPageDuplicates);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(679, 563);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageFiles
            // 
            this.tabPageFiles.Controls.Add(this.listViewFiles);
            this.tabPageFiles.Location = new System.Drawing.Point(4, 24);
            this.tabPageFiles.Name = "tabPageFiles";
            this.tabPageFiles.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFiles.Size = new System.Drawing.Size(671, 535);
            this.tabPageFiles.TabIndex = 0;
            this.tabPageFiles.Text = "Files";
            this.tabPageFiles.UseVisualStyleBackColor = true;
            // 
            // listViewFiles
            // 
            this.listViewFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderFileName,
            this.columnHeaderSize,
            this.columnHeaderLocation});
            this.listViewFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewFiles.FullRowSelect = true;
            this.listViewFiles.GridLines = true;
            this.listViewFiles.Location = new System.Drawing.Point(3, 3);
            this.listViewFiles.Name = "listViewFiles";
            this.listViewFiles.Size = new System.Drawing.Size(665, 529);
            this.listViewFiles.TabIndex = 0;
            this.listViewFiles.UseCompatibleStateImageBehavior = false;
            this.listViewFiles.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderFileName
            // 
            this.columnHeaderFileName.Text = "File Name";
            this.columnHeaderFileName.Width = 200;
            // 
            // columnHeaderSize
            // 
            this.columnHeaderSize.Text = "Size";
            this.columnHeaderSize.Width = 100;
            // 
            // columnHeaderLocation
            // 
            this.columnHeaderLocation.Text = "Location";
            this.columnHeaderLocation.Width = 350;
            // 
            // tabPageSizeStats
            // 
            this.tabPageSizeStats.Controls.Add(this.listViewSizeStats);
            this.tabPageSizeStats.Location = new System.Drawing.Point(4, 24);
            this.tabPageSizeStats.Name = "tabPageSizeStats";
            this.tabPageSizeStats.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSizeStats.Size = new System.Drawing.Size(671, 535);
            this.tabPageSizeStats.TabIndex = 1;
            this.tabPageSizeStats.Text = "Size Statistics";
            this.tabPageSizeStats.UseVisualStyleBackColor = true;
            // 
            // listViewSizeStats
            // 
            this.listViewSizeStats.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderDirectory,
            this.columnHeaderDirSize,
            this.columnHeaderPercentage,
            this.columnHeaderFileCount});
            this.listViewSizeStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewSizeStats.FullRowSelect = true;
            this.listViewSizeStats.GridLines = true;
            this.listViewSizeStats.Location = new System.Drawing.Point(3, 3);
            this.listViewSizeStats.Name = "listViewSizeStats";
            this.listViewSizeStats.Size = new System.Drawing.Size(665, 529);
            this.listViewSizeStats.TabIndex = 0;
            this.listViewSizeStats.UseCompatibleStateImageBehavior = false;
            this.listViewSizeStats.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderDirectory
            // 
            this.columnHeaderDirectory.Text = "Directory";
            this.columnHeaderDirectory.Width = 250;
            // 
            // columnHeaderDirSize
            // 
            this.columnHeaderDirSize.Text = "Size";
            this.columnHeaderDirSize.Width = 100;
            // 
            // columnHeaderPercentage
            // 
            this.columnHeaderPercentage.Text = "Percentage";
            this.columnHeaderPercentage.Width = 100;
            // 
            // columnHeaderFileCount
            // 
            this.columnHeaderFileCount.Text = "File Count";
            this.columnHeaderFileCount.Width = 100;
            // 
            // tabPageDuplicates
            // 
            this.tabPageDuplicates.Controls.Add(this.listViewDuplicates);
            this.tabPageDuplicates.Location = new System.Drawing.Point(4, 24);
            this.tabPageDuplicates.Name = "tabPageDuplicates";
            this.tabPageDuplicates.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDuplicates.Size = new System.Drawing.Size(671, 535);
            this.tabPageDuplicates.TabIndex = 2;
            this.tabPageDuplicates.Text = "Duplicates";
            this.tabPageDuplicates.UseVisualStyleBackColor = true;
            // 
            // listViewDuplicates
            // 
            this.listViewDuplicates.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderDupName,
            this.columnHeaderDupSize,
            this.columnHeaderDupType,
            this.columnHeaderDupLocation});
            this.listViewDuplicates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewDuplicates.FullRowSelect = true;
            this.listViewDuplicates.GridLines = true;
            this.listViewDuplicates.Location = new System.Drawing.Point(3, 3);
            this.listViewDuplicates.Name = "listViewDuplicates";
            this.listViewDuplicates.Size = new System.Drawing.Size(665, 529);
            this.listViewDuplicates.TabIndex = 0;
            this.listViewDuplicates.UseCompatibleStateImageBehavior = false;
            this.listViewDuplicates.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderDupName
            // 
            this.columnHeaderDupName.Text = "File/Group Name";
            this.columnHeaderDupName.Width = 200;
            // 
            // columnHeaderDupSize
            // 
            this.columnHeaderDupSize.Text = "Size";
            this.columnHeaderDupSize.Width = 100;
            // 
            // columnHeaderDupType
            // 
            this.columnHeaderDupType.Text = "Type";
            this.columnHeaderDupType.Width = 100;
            // 
            // columnHeaderDupLocation
            // 
            this.columnHeaderDupLocation.Text = "Location";
            this.columnHeaderDupLocation.Width = 250;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.btnSearch);
            this.panelTop.Controls.Add(this.btnFindDuplicates);
            this.panelTop.Controls.Add(this.btnScan);
            this.panelTop.Controls.Add(this.btnBrowse);
            this.panelTop.Controls.Add(this.txtDirectoryPath);
            this.panelTop.Controls.Add(this.lblDirectoryPath);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1024, 65);
            this.panelTop.TabIndex = 1;
            // 
            // btnSearch
            // 
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(937, 20);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnFindDuplicates
            // 
            this.btnFindDuplicates.Enabled = false;
            this.btnFindDuplicates.Location = new System.Drawing.Point(818, 20);
            this.btnFindDuplicates.Name = "btnFindDuplicates";
            this.btnFindDuplicates.Size = new System.Drawing.Size(113, 23);
            this.btnFindDuplicates.TabIndex = 4;
            this.btnFindDuplicates.Text = "Find Duplicates";
            this.btnFindDuplicates.UseVisualStyleBackColor = true;
            this.btnFindDuplicates.Click += new System.EventHandler(this.btnFindDuplicates_Click);
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(737, 20);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(75, 23);
            this.btnScan.TabIndex = 3;
            this.btnScan.Text = "Scan";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(656, 20);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtDirectoryPath
            // 
            this.txtDirectoryPath.Location = new System.Drawing.Point(112, 21);
            this.txtDirectoryPath.Name = "txtDirectoryPath";
            this.txtDirectoryPath.Size = new System.Drawing.Size(538, 23);
            this.txtDirectoryPath.TabIndex = 1;
            // 
            // lblDirectoryPath
            // 
            this.lblDirectoryPath.AutoSize = true;
            this.lblDirectoryPath.Location = new System.Drawing.Point(12, 24);
            this.lblDirectoryPath.Name = "lblDirectoryPath";
            this.lblDirectoryPath.Size = new System.Drawing.Size(94, 15);
            this.lblDirectoryPath.TabIndex = 0;
            this.lblDirectoryPath.Text = "Directory Path:";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.progressBar});
            this.statusStrip.Location = new System.Drawing.Point(0, 628);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1024, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(39, 17);
            this.lblStatus.Text = "Ready";
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(200, 16);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 650);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.statusStrip);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "File System Directory Tree Analyzer";
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPageFiles.ResumeLayout(false);
            this.tabPageSizeStats.ResumeLayout(false);
            this.tabPageDuplicates.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SplitContainer splitContainerMain;
        private TreeView treeViewDirectory;
        private TabControl tabControl;
        private TabPage tabPageFiles;
        private TabPage tabPageSizeStats;
        private Panel panelTop;
        private Button btnSearch;
        private Button btnFindDuplicates;
        private Button btnScan;
        private Button btnBrowse;
        private TextBox txtDirectoryPath;
        private Label lblDirectoryPath;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;
        private ToolStripProgressBar progressBar;
        private ListView listViewFiles;
        private ColumnHeader columnHeaderFileName;
        private ColumnHeader columnHeaderSize;
        private ColumnHeader columnHeaderLocation;
        private ListView listViewSizeStats;
        private ColumnHeader columnHeaderDirectory;
        private ColumnHeader columnHeaderDirSize;
        private ColumnHeader columnHeaderPercentage;
        private ColumnHeader columnHeaderFileCount;
        private TabPage tabPageDuplicates;
        private ListView listViewDuplicates;
        private ColumnHeader columnHeaderDupName;
        private ColumnHeader columnHeaderDupSize;
        private ColumnHeader columnHeaderDupType;
        private ColumnHeader columnHeaderDupLocation;
    }
} 