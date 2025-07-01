namespace OEngineResourceReader.Forms
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
            components = new System.ComponentModel.Container();
            menuStrip = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openResourceDirectoryMenuItem = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            openTextureFileToolStripMenuItem = new ToolStripMenuItem();
            exportTextureStripMenuItem = new ToolStripMenuItem();
            replaceTextureToolStripMenuItem = new ToolStripMenuItem();
            saveModifedTextureMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            loadTextMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            loadFontToolStripMenuItem = new ToolStripMenuItem();
            exportFontConfigurationToBMToolStripMenuItem = new ToolStripMenuItem();
            generateNewFontFromBMFontToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            decryptToolStripMenuItem = new ToolStripMenuItem();
            enableDecryptionToTreeVisualToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutProgramToolStripMenuItem = new ToolStripMenuItem();
            textureTableLayoutPanel = new TableLayoutPanel();
            textureFormatLabel = new Label();
            textureResolutionLabel = new Label();
            textureSizeLabel = new Label();
            toolStripLabelZoom = new Label();
            changeBgTextureViewerButton = new Button();
            preivewTextureBox = new ImageView.PictureBox();
            mainTabControl = new TabControl();
            textureTabPage = new TabPage();
            textTabPage = new TabPage();
            textTableLayoutPanel = new TableLayoutPanel();
            textDataGridView = new DataGridView();
            textInfoLabel = new Label();
            saveTextButton = new Button();
            exportTextDataButton = new Button();
            importTextDataButton = new Button();
            searchTextTextBox = new TextBox();
            fontTabPage = new TabPage();
            fontMainSplitContainer = new SplitContainer();
            fontPreviewSplitContainer = new SplitContainer();
            fontTexturePreview = new ImageView.PictureBox();
            fontRenderGb = new GroupBox();
            fontPreviewTextPb = new PictureBox();
            previewFontTextBox = new TextBox();
            fontInfoTablePanel = new TableLayoutPanel();
            fontInfoGb = new GroupBox();
            fontInfoTextBox = new RichTextBox();
            selectedGlyphGb = new GroupBox();
            selectedGlyphInfoTablePanel = new TableLayoutPanel();
            glyphPreviewBox = new ImageView.PictureBox();
            glyphInfoTextBox = new RichTextBox();
            toolTip1 = new ToolTip(components);
            mainSplitContainer = new SplitContainer();
            tableLayoutPanel1 = new TableLayoutPanel();
            fileTreeView = new TreeView();
            extFilterTextBox = new TextBox();
            applyFilterButton = new Button();
            menuStrip.SuspendLayout();
            textureTableLayoutPanel.SuspendLayout();
            mainTabControl.SuspendLayout();
            textureTabPage.SuspendLayout();
            textTabPage.SuspendLayout();
            textTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)textDataGridView).BeginInit();
            fontTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)fontMainSplitContainer).BeginInit();
            fontMainSplitContainer.Panel1.SuspendLayout();
            fontMainSplitContainer.Panel2.SuspendLayout();
            fontMainSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)fontPreviewSplitContainer).BeginInit();
            fontPreviewSplitContainer.Panel1.SuspendLayout();
            fontPreviewSplitContainer.Panel2.SuspendLayout();
            fontPreviewSplitContainer.SuspendLayout();
            fontRenderGb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)fontPreviewTextPb).BeginInit();
            fontInfoTablePanel.SuspendLayout();
            fontInfoGb.SuspendLayout();
            selectedGlyphGb.SuspendLayout();
            selectedGlyphInfoTablePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)mainSplitContainer).BeginInit();
            mainSplitContainer.Panel1.SuspendLayout();
            mainSplitContainer.Panel2.SuspendLayout();
            mainSplitContainer.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, toolsToolStripMenuItem, aboutToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(882, 28);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openResourceDirectoryMenuItem, toolStripSeparator4, openTextureFileToolStripMenuItem, exportTextureStripMenuItem, replaceTextureToolStripMenuItem, saveModifedTextureMenuItem, toolStripSeparator1, loadTextMenuItem, toolStripSeparator2, loadFontToolStripMenuItem, exportFontConfigurationToBMToolStripMenuItem, generateNewFontFromBMFontToolStripMenuItem, toolStripSeparator3, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // openResourceDirectoryMenuItem
            // 
            openResourceDirectoryMenuItem.Name = "openResourceDirectoryMenuItem";
            openResourceDirectoryMenuItem.Size = new Size(336, 26);
            openResourceDirectoryMenuItem.Text = "Open Resource Directory";
            openResourceDirectoryMenuItem.Click += openResourceDirectoryMenuItem_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(333, 6);
            // 
            // openTextureFileToolStripMenuItem
            // 
            openTextureFileToolStripMenuItem.Name = "openTextureFileToolStripMenuItem";
            openTextureFileToolStripMenuItem.Size = new Size(336, 26);
            openTextureFileToolStripMenuItem.Text = "Open Texture File";
            openTextureFileToolStripMenuItem.Click += openTextureFileToolStripMenuItem_Click;
            // 
            // exportTextureStripMenuItem
            // 
            exportTextureStripMenuItem.Enabled = false;
            exportTextureStripMenuItem.Name = "exportTextureStripMenuItem";
            exportTextureStripMenuItem.Size = new Size(336, 26);
            exportTextureStripMenuItem.Text = "Export To Texture File";
            exportTextureStripMenuItem.Click += exportTextureStripMenuItem_Click;
            // 
            // replaceTextureToolStripMenuItem
            // 
            replaceTextureToolStripMenuItem.Enabled = false;
            replaceTextureToolStripMenuItem.Name = "replaceTextureToolStripMenuItem";
            replaceTextureToolStripMenuItem.Size = new Size(336, 26);
            replaceTextureToolStripMenuItem.Text = "Replace Texture";
            replaceTextureToolStripMenuItem.Click += replaceTextureToolStripMenuItem_Click;
            // 
            // saveModifedTextureMenuItem
            // 
            saveModifedTextureMenuItem.Enabled = false;
            saveModifedTextureMenuItem.Name = "saveModifedTextureMenuItem";
            saveModifedTextureMenuItem.Size = new Size(336, 26);
            saveModifedTextureMenuItem.Text = "Save Modified Texture";
            saveModifedTextureMenuItem.Click += saveModifedTextureMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(333, 6);
            // 
            // loadTextMenuItem
            // 
            loadTextMenuItem.Name = "loadTextMenuItem";
            loadTextMenuItem.Size = new Size(336, 26);
            loadTextMenuItem.Text = "Load Text";
            loadTextMenuItem.Click += loadTextMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(333, 6);
            // 
            // loadFontToolStripMenuItem
            // 
            loadFontToolStripMenuItem.Name = "loadFontToolStripMenuItem";
            loadFontToolStripMenuItem.Size = new Size(336, 26);
            loadFontToolStripMenuItem.Text = "Load Font";
            loadFontToolStripMenuItem.Click += loadFontToolStripMenuItem_Click;
            // 
            // exportFontConfigurationToBMToolStripMenuItem
            // 
            exportFontConfigurationToBMToolStripMenuItem.Enabled = false;
            exportFontConfigurationToBMToolStripMenuItem.Name = "exportFontConfigurationToBMToolStripMenuItem";
            exportFontConfigurationToBMToolStripMenuItem.Size = new Size(336, 26);
            exportFontConfigurationToBMToolStripMenuItem.Text = "Export Font Configuration to BMFont";
            exportFontConfigurationToBMToolStripMenuItem.Click += exportFontConfigurationToBMToolStripMenuItem_Click;
            // 
            // generateNewFontFromBMFontToolStripMenuItem
            // 
            generateNewFontFromBMFontToolStripMenuItem.Enabled = false;
            generateNewFontFromBMFontToolStripMenuItem.Name = "generateNewFontFromBMFontToolStripMenuItem";
            generateNewFontFromBMFontToolStripMenuItem.Size = new Size(336, 26);
            generateNewFontFromBMFontToolStripMenuItem.Text = "Generate new font from .fnt";
            generateNewFontFromBMFontToolStripMenuItem.Click += generateNewFontFromBMFontToolStripMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(333, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(336, 26);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { decryptToolStripMenuItem, enableDecryptionToTreeVisualToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(58, 24);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // decryptToolStripMenuItem
            // 
            decryptToolStripMenuItem.Name = "decryptToolStripMenuItem";
            decryptToolStripMenuItem.Size = new Size(388, 26);
            decryptToolStripMenuItem.Text = "Decrypt paths in UsedRscList.ot";
            decryptToolStripMenuItem.Click += decryptToolStripMenuItem_Click;
            // 
            // enableDecryptionToTreeVisualToolStripMenuItem
            // 
            enableDecryptionToTreeVisualToolStripMenuItem.CheckOnClick = true;
            enableDecryptionToTreeVisualToolStripMenuItem.Name = "enableDecryptionToTreeVisualToolStripMenuItem";
            enableDecryptionToTreeVisualToolStripMenuItem.Size = new Size(392, 26);
            enableDecryptionToTreeVisualToolStripMenuItem.Text = "Enable File Names Decryption in Tree (Visual)";
            enableDecryptionToTreeVisualToolStripMenuItem.CheckStateChanged += enableDecryptionToTreeVisualToolStripMenuItem_CheckStateChanged;
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { helpToolStripMenuItem, aboutProgramToolStripMenuItem });
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(64, 24);
            aboutToolStripMenuItem.Text = "About";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(194, 26);
            helpToolStripMenuItem.Text = "Help";
            helpToolStripMenuItem.Click += helpToolStripMenuItem_Click;
            // 
            // aboutProgramToolStripMenuItem
            // 
            aboutProgramToolStripMenuItem.Name = "aboutProgramToolStripMenuItem";
            aboutProgramToolStripMenuItem.Size = new Size(194, 26);
            aboutProgramToolStripMenuItem.Text = "About Program";
            aboutProgramToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // textureTableLayoutPanel
            // 
            textureTableLayoutPanel.AllowDrop = true;
            textureTableLayoutPanel.ColumnCount = 5;
            textureTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            textureTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            textureTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            textureTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            textureTableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            textureTableLayoutPanel.Controls.Add(textureFormatLabel, 2, 1);
            textureTableLayoutPanel.Controls.Add(textureResolutionLabel, 1, 1);
            textureTableLayoutPanel.Controls.Add(textureSizeLabel, 0, 1);
            textureTableLayoutPanel.Controls.Add(toolStripLabelZoom, 3, 1);
            textureTableLayoutPanel.Controls.Add(changeBgTextureViewerButton, 4, 1);
            textureTableLayoutPanel.Controls.Add(preivewTextureBox, 0, 0);
            textureTableLayoutPanel.Dock = DockStyle.Fill;
            textureTableLayoutPanel.Location = new Point(3, 3);
            textureTableLayoutPanel.Name = "textureTableLayoutPanel";
            textureTableLayoutPanel.RowCount = 2;
            textureTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            textureTableLayoutPanel.RowStyles.Add(new RowStyle());
            textureTableLayoutPanel.Size = new Size(645, 486);
            textureTableLayoutPanel.TabIndex = 1;
            // 
            // textureFormatLabel
            // 
            textureFormatLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            textureFormatLabel.AutoSize = true;
            textureFormatLabel.Location = new Point(309, 461);
            textureFormatLabel.Name = "textureFormatLabel";
            textureFormatLabel.Size = new Size(147, 20);
            textureFormatLabel.TabIndex = 3;
            // 
            // textureResolutionLabel
            // 
            textureResolutionLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            textureResolutionLabel.AutoSize = true;
            textureResolutionLabel.Location = new Point(156, 461);
            textureResolutionLabel.Name = "textureResolutionLabel";
            textureResolutionLabel.Size = new Size(147, 20);
            textureResolutionLabel.TabIndex = 2;
            // 
            // textureSizeLabel
            // 
            textureSizeLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            textureSizeLabel.AutoSize = true;
            textureSizeLabel.Location = new Point(3, 461);
            textureSizeLabel.Name = "textureSizeLabel";
            textureSizeLabel.Size = new Size(147, 20);
            textureSizeLabel.TabIndex = 1;
            // 
            // toolStripLabelZoom
            // 
            toolStripLabelZoom.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            toolStripLabelZoom.AutoSize = true;
            toolStripLabelZoom.Location = new Point(462, 461);
            toolStripLabelZoom.Name = "toolStripLabelZoom";
            toolStripLabelZoom.Size = new Size(147, 20);
            toolStripLabelZoom.TabIndex = 4;
            // 
            // changeBgTextureViewerButton
            // 
            changeBgTextureViewerButton.Dock = DockStyle.Fill;
            changeBgTextureViewerButton.FlatAppearance.BorderSize = 0;
            changeBgTextureViewerButton.Location = new Point(612, 456);
            changeBgTextureViewerButton.Margin = new Padding(0);
            changeBgTextureViewerButton.Name = "changeBgTextureViewerButton";
            changeBgTextureViewerButton.Size = new Size(33, 30);
            changeBgTextureViewerButton.TabIndex = 6;
            changeBgTextureViewerButton.Text = "B";
            toolTip1.SetToolTip(changeBgTextureViewerButton, "Change background on TextureViewer");
            changeBgTextureViewerButton.UseVisualStyleBackColor = true;
            changeBgTextureViewerButton.Click += changeBgTextureViewerButton_Click;
            // 
            // preivewTextureBox
            // 
            preivewTextureBox.AllowDrop = true;
            preivewTextureBox.Bitmap = null;
            textureTableLayoutPanel.SetColumnSpan(preivewTextureBox, 5);
            preivewTextureBox.Dock = DockStyle.Fill;
            preivewTextureBox.DragCursor = null;
            preivewTextureBox.DragMouseButton = MouseButtons.None;
            preivewTextureBox.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            preivewTextureBox.Location = new Point(3, 3);
            preivewTextureBox.Name = "preivewTextureBox";
            preivewTextureBox.Size = new Size(639, 450);
            preivewTextureBox.SizeMode = ImageView.SizeMode.BestFit;
            preivewTextureBox.TabIndex = 7;
            preivewTextureBox.TransparentBackground = null;
            preivewTextureBox.UseBackgroundBrush = false;
            preivewTextureBox.UseZoomCursors = true;
            preivewTextureBox.VerticallScrollStep = 1F;
            preivewTextureBox.WheelScrollLock = false;
            preivewTextureBox.Zoom = 1F;
            preivewTextureBox.ZoomInCursor = null;
            preivewTextureBox.ZoomMouseButton = MouseButtons.Left;
            preivewTextureBox.ZoomOutCursor = null;
            preivewTextureBox.ZoomOutModifier = Keys.Control | Keys.Menu;
            preivewTextureBox.ZoomChanged += preivewTextureBox_ZoomChanged;
            preivewTextureBox.DragDrop += MainForm_DragDrop;
            preivewTextureBox.DragEnter += MainForm_DragEnter;
            // 
            // mainTabControl
            // 
            mainTabControl.AllowDrop = true;
            mainTabControl.Controls.Add(textureTabPage);
            mainTabControl.Controls.Add(textTabPage);
            mainTabControl.Controls.Add(fontTabPage);
            mainTabControl.Dock = DockStyle.Fill;
            mainTabControl.Location = new Point(0, 0);
            mainTabControl.Name = "mainTabControl";
            mainTabControl.SelectedIndex = 0;
            mainTabControl.Size = new Size(659, 525);
            mainTabControl.TabIndex = 2;
            mainTabControl.SelectedIndexChanged += mainTabControl_SelectedIndexChanged;
            // 
            // textureTabPage
            // 
            textureTabPage.BackColor = SystemColors.Control;
            textureTabPage.Controls.Add(textureTableLayoutPanel);
            textureTabPage.Location = new Point(4, 29);
            textureTabPage.Name = "textureTabPage";
            textureTabPage.Padding = new Padding(3);
            textureTabPage.Size = new Size(651, 492);
            textureTabPage.TabIndex = 0;
            textureTabPage.Text = "Textures";
            // 
            // textTabPage
            // 
            textTabPage.BackColor = SystemColors.Control;
            textTabPage.Controls.Add(textTableLayoutPanel);
            textTabPage.Location = new Point(4, 29);
            textTabPage.Name = "textTabPage";
            textTabPage.Padding = new Padding(3);
            textTabPage.Size = new Size(651, 492);
            textTabPage.TabIndex = 1;
            textTabPage.Text = "Text";
            // 
            // textTableLayoutPanel
            // 
            textTableLayoutPanel.ColumnCount = 5;
            textTableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            textTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            textTableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            textTableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            textTableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            textTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            textTableLayoutPanel.Controls.Add(textDataGridView, 0, 1);
            textTableLayoutPanel.Controls.Add(textInfoLabel, 0, 0);
            textTableLayoutPanel.Controls.Add(saveTextButton, 4, 0);
            textTableLayoutPanel.Controls.Add(exportTextDataButton, 3, 0);
            textTableLayoutPanel.Controls.Add(importTextDataButton, 2, 0);
            textTableLayoutPanel.Controls.Add(searchTextTextBox, 1, 0);
            textTableLayoutPanel.Dock = DockStyle.Fill;
            textTableLayoutPanel.Enabled = false;
            textTableLayoutPanel.Location = new Point(3, 3);
            textTableLayoutPanel.Name = "textTableLayoutPanel";
            textTableLayoutPanel.RowCount = 2;
            textTableLayoutPanel.RowStyles.Add(new RowStyle());
            textTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            textTableLayoutPanel.Size = new Size(645, 486);
            textTableLayoutPanel.TabIndex = 0;
            // 
            // textDataGridView
            // 
            textDataGridView.AllowDrop = true;
            textDataGridView.BackgroundColor = SystemColors.Control;
            textDataGridView.BorderStyle = BorderStyle.None;
            textDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            textTableLayoutPanel.SetColumnSpan(textDataGridView, 5);
            textDataGridView.Dock = DockStyle.Fill;
            textDataGridView.GridColor = SystemColors.Control;
            textDataGridView.Location = new Point(3, 38);
            textDataGridView.Name = "textDataGridView";
            textDataGridView.RowHeadersWidth = 51;
            textDataGridView.Size = new Size(639, 445);
            textDataGridView.TabIndex = 0;
            textDataGridView.CellValueChanged += textDataGridView_CellValueChanged;
            textDataGridView.DragDrop += MainForm_DragDrop;
            textDataGridView.DragEnter += MainForm_DragEnter;
            // 
            // textInfoLabel
            // 
            textInfoLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            textInfoLabel.AutoSize = true;
            textInfoLabel.Location = new Point(3, 7);
            textInfoLabel.Name = "textInfoLabel";
            textInfoLabel.Size = new Size(119, 20);
            textInfoLabel.TabIndex = 2;
            textInfoLabel.Text = "Text Not Loaded";
            // 
            // saveTextButton
            // 
            saveTextButton.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            saveTextButton.Enabled = false;
            saveTextButton.Location = new Point(570, 3);
            saveTextButton.Name = "saveTextButton";
            saveTextButton.Size = new Size(72, 29);
            saveTextButton.TabIndex = 1;
            saveTextButton.Text = "Save";
            saveTextButton.UseVisualStyleBackColor = true;
            saveTextButton.Click += saveTextButton_Click;
            // 
            // exportTextDataButton
            // 
            exportTextDataButton.Location = new Point(494, 3);
            exportTextDataButton.Name = "exportTextDataButton";
            exportTextDataButton.Size = new Size(70, 29);
            exportTextDataButton.TabIndex = 3;
            exportTextDataButton.Text = "Export";
            exportTextDataButton.UseVisualStyleBackColor = true;
            exportTextDataButton.Click += exportTextDataButton_Click;
            // 
            // importTextDataButton
            // 
            importTextDataButton.Location = new Point(418, 3);
            importTextDataButton.Name = "importTextDataButton";
            importTextDataButton.Size = new Size(70, 29);
            importTextDataButton.TabIndex = 4;
            importTextDataButton.Text = "Import";
            importTextDataButton.UseVisualStyleBackColor = true;
            importTextDataButton.Click += importTextDataButton_Click;
            // 
            // searchTextTextBox
            // 
            searchTextTextBox.Dock = DockStyle.Fill;
            searchTextTextBox.Location = new Point(128, 3);
            searchTextTextBox.Name = "searchTextTextBox";
            searchTextTextBox.PlaceholderText = "Enter text to filter";
            searchTextTextBox.Size = new Size(284, 27);
            searchTextTextBox.TabIndex = 6;
            searchTextTextBox.TextChanged += searchTextTextBox_TextChanged;
            // 
            // fontTabPage
            // 
            fontTabPage.Controls.Add(fontMainSplitContainer);
            fontTabPage.Location = new Point(4, 29);
            fontTabPage.Name = "fontTabPage";
            fontTabPage.Padding = new Padding(3);
            fontTabPage.Size = new Size(651, 492);
            fontTabPage.TabIndex = 2;
            fontTabPage.Text = "Font";
            fontTabPage.UseVisualStyleBackColor = true;
            // 
            // fontMainSplitContainer
            // 
            fontMainSplitContainer.Dock = DockStyle.Fill;
            fontMainSplitContainer.Location = new Point(3, 3);
            fontMainSplitContainer.Name = "fontMainSplitContainer";
            // 
            // fontMainSplitContainer.Panel1
            // 
            fontMainSplitContainer.Panel1.Controls.Add(fontPreviewSplitContainer);
            // 
            // fontMainSplitContainer.Panel2
            // 
            fontMainSplitContainer.Panel2.Controls.Add(fontInfoTablePanel);
            fontMainSplitContainer.Size = new Size(645, 486);
            fontMainSplitContainer.SplitterDistance = 441;
            fontMainSplitContainer.TabIndex = 1;
            // 
            // fontPreviewSplitContainer
            // 
            fontPreviewSplitContainer.Dock = DockStyle.Fill;
            fontPreviewSplitContainer.Location = new Point(0, 0);
            fontPreviewSplitContainer.Name = "fontPreviewSplitContainer";
            fontPreviewSplitContainer.Orientation = Orientation.Horizontal;
            // 
            // fontPreviewSplitContainer.Panel1
            // 
            fontPreviewSplitContainer.Panel1.Controls.Add(fontTexturePreview);
            // 
            // fontPreviewSplitContainer.Panel2
            // 
            fontPreviewSplitContainer.Panel2.Controls.Add(fontRenderGb);
            fontPreviewSplitContainer.Size = new Size(441, 486);
            fontPreviewSplitContainer.SplitterDistance = 333;
            fontPreviewSplitContainer.TabIndex = 0;
            // 
            // fontTexturePreview
            // 
            fontTexturePreview.AllowDrop = true;
            fontTexturePreview.Bitmap = null;
            fontTexturePreview.Dock = DockStyle.Fill;
            fontTexturePreview.DragCursor = null;
            fontTexturePreview.DragMouseButton = MouseButtons.None;
            fontTexturePreview.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            fontTexturePreview.Location = new Point(0, 0);
            fontTexturePreview.Name = "fontTexturePreview";
            fontTexturePreview.Size = new Size(441, 333);
            fontTexturePreview.SizeMode = ImageView.SizeMode.BestFit;
            fontTexturePreview.TabIndex = 1;
            fontTexturePreview.TransparentBackground = null;
            fontTexturePreview.UseBackgroundBrush = false;
            fontTexturePreview.UseZoomCursors = true;
            fontTexturePreview.VerticallScrollStep = 1F;
            fontTexturePreview.WheelScrollLock = false;
            fontTexturePreview.Zoom = 1F;
            fontTexturePreview.ZoomInCursor = null;
            fontTexturePreview.ZoomMouseButton = MouseButtons.Left;
            fontTexturePreview.ZoomOutCursor = null;
            fontTexturePreview.ZoomOutModifier = Keys.Alt | Keys.Menu;
            fontTexturePreview.PixelCoordinatesChanged += fontTexturePreview_PixelCoordinatesChanged;
            fontTexturePreview.DragDrop += MainForm_DragDrop;
            fontTexturePreview.DragEnter += MainForm_DragEnter;
            // 
            // fontRenderGb
            // 
            fontRenderGb.Controls.Add(fontPreviewTextPb);
            fontRenderGb.Controls.Add(previewFontTextBox);
            fontRenderGb.Dock = DockStyle.Fill;
            fontRenderGb.Location = new Point(0, 0);
            fontRenderGb.Name = "fontRenderGb";
            fontRenderGb.RightToLeft = RightToLeft.No;
            fontRenderGb.Size = new Size(441, 149);
            fontRenderGb.TabIndex = 0;
            fontRenderGb.TabStop = false;
            fontRenderGb.Text = "Live Preview Text";
            // 
            // fontPreviewTextPb
            // 
            fontPreviewTextPb.BackColor = Color.Transparent;
            fontPreviewTextPb.Dock = DockStyle.Fill;
            fontPreviewTextPb.Location = new Point(3, 50);
            fontPreviewTextPb.Name = "fontPreviewTextPb";
            fontPreviewTextPb.Size = new Size(435, 96);
            fontPreviewTextPb.TabIndex = 2;
            fontPreviewTextPb.TabStop = false;
            // 
            // previewFontTextBox
            // 
            previewFontTextBox.Dock = DockStyle.Top;
            previewFontTextBox.Location = new Point(3, 23);
            previewFontTextBox.Name = "previewFontTextBox";
            previewFontTextBox.PlaceholderText = "Enter your text";
            previewFontTextBox.Size = new Size(435, 27);
            previewFontTextBox.TabIndex = 3;
            previewFontTextBox.TextChanged += previewFontTextBox_TextChanged;
            // 
            // fontInfoTablePanel
            // 
            fontInfoTablePanel.ColumnCount = 1;
            fontInfoTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            fontInfoTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            fontInfoTablePanel.Controls.Add(fontInfoGb, 0, 0);
            fontInfoTablePanel.Controls.Add(selectedGlyphGb, 0, 1);
            fontInfoTablePanel.Dock = DockStyle.Fill;
            fontInfoTablePanel.Location = new Point(0, 0);
            fontInfoTablePanel.Name = "fontInfoTablePanel";
            fontInfoTablePanel.RowCount = 2;
            fontInfoTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            fontInfoTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            fontInfoTablePanel.Size = new Size(200, 486);
            fontInfoTablePanel.TabIndex = 0;
            // 
            // fontInfoGb
            // 
            fontInfoGb.BackColor = Color.Transparent;
            fontInfoGb.Controls.Add(fontInfoTextBox);
            fontInfoGb.Dock = DockStyle.Fill;
            fontInfoGb.Location = new Point(3, 3);
            fontInfoGb.Name = "fontInfoGb";
            fontInfoGb.Size = new Size(194, 237);
            fontInfoGb.TabIndex = 2;
            fontInfoGb.TabStop = false;
            fontInfoGb.Text = "Font info";
            // 
            // fontInfoTextBox
            // 
            fontInfoTextBox.BackColor = Color.WhiteSmoke;
            fontInfoTextBox.BorderStyle = BorderStyle.None;
            fontInfoTextBox.Dock = DockStyle.Fill;
            fontInfoTextBox.Location = new Point(3, 23);
            fontInfoTextBox.Name = "fontInfoTextBox";
            fontInfoTextBox.ReadOnly = true;
            fontInfoTextBox.Size = new Size(188, 211);
            fontInfoTextBox.TabIndex = 0;
            fontInfoTextBox.Text = "";
            fontInfoTextBox.WordWrap = false;
            // 
            // selectedGlyphGb
            // 
            selectedGlyphGb.Controls.Add(selectedGlyphInfoTablePanel);
            selectedGlyphGb.Dock = DockStyle.Fill;
            selectedGlyphGb.Location = new Point(3, 246);
            selectedGlyphGb.Name = "selectedGlyphGb";
            selectedGlyphGb.Size = new Size(194, 237);
            selectedGlyphGb.TabIndex = 1;
            selectedGlyphGb.TabStop = false;
            selectedGlyphGb.Text = "Selected Glyph";
            // 
            // selectedGlyphInfoTablePanel
            // 
            selectedGlyphInfoTablePanel.ColumnCount = 1;
            selectedGlyphInfoTablePanel.ColumnStyles.Add(new ColumnStyle());
            selectedGlyphInfoTablePanel.Controls.Add(glyphPreviewBox, 0, 0);
            selectedGlyphInfoTablePanel.Controls.Add(glyphInfoTextBox, 0, 1);
            selectedGlyphInfoTablePanel.Dock = DockStyle.Fill;
            selectedGlyphInfoTablePanel.Location = new Point(3, 23);
            selectedGlyphInfoTablePanel.Name = "selectedGlyphInfoTablePanel";
            selectedGlyphInfoTablePanel.RowCount = 2;
            selectedGlyphInfoTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            selectedGlyphInfoTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            selectedGlyphInfoTablePanel.Size = new Size(188, 211);
            selectedGlyphInfoTablePanel.TabIndex = 0;
            // 
            // glyphPreviewBox
            // 
            glyphPreviewBox.Bitmap = null;
            glyphPreviewBox.Dock = DockStyle.Fill;
            glyphPreviewBox.DragCursor = null;
            glyphPreviewBox.DragMouseButton = MouseButtons.None;
            glyphPreviewBox.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            glyphPreviewBox.Location = new Point(3, 3);
            glyphPreviewBox.Name = "glyphPreviewBox";
            glyphPreviewBox.Size = new Size(188, 99);
            glyphPreviewBox.SizeMode = ImageView.SizeMode.Normal;
            glyphPreviewBox.TabIndex = 0;
            glyphPreviewBox.TransparentBackground = null;
            glyphPreviewBox.UseBackgroundBrush = false;
            glyphPreviewBox.UseZoomCursors = false;
            glyphPreviewBox.VerticallScrollStep = 1F;
            glyphPreviewBox.WheelScrollLock = false;
            glyphPreviewBox.Zoom = 1F;
            glyphPreviewBox.ZoomInCursor = null;
            glyphPreviewBox.ZoomMouseButton = MouseButtons.Left;
            glyphPreviewBox.ZoomOutCursor = null;
            glyphPreviewBox.ZoomOutModifier = Keys.Alt | Keys.Menu;
            // 
            // glyphInfoTextBox
            // 
            glyphInfoTextBox.BackColor = Color.WhiteSmoke;
            glyphInfoTextBox.BorderStyle = BorderStyle.None;
            glyphInfoTextBox.Dock = DockStyle.Fill;
            glyphInfoTextBox.Location = new Point(3, 108);
            glyphInfoTextBox.Name = "glyphInfoTextBox";
            glyphInfoTextBox.ReadOnly = true;
            glyphInfoTextBox.Size = new Size(188, 100);
            glyphInfoTextBox.TabIndex = 1;
            glyphInfoTextBox.Text = "";
            // 
            // mainSplitContainer
            // 
            mainSplitContainer.Dock = DockStyle.Fill;
            mainSplitContainer.Location = new Point(0, 28);
            mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            mainSplitContainer.Panel1.Controls.Add(tableLayoutPanel1);
            // 
            // mainSplitContainer.Panel2
            // 
            mainSplitContainer.Panel2.Controls.Add(mainTabControl);
            mainSplitContainer.Size = new Size(882, 525);
            mainSplitContainer.SplitterDistance = 219;
            mainSplitContainer.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(fileTreeView, 0, 0);
            tableLayoutPanel1.Controls.Add(extFilterTextBox, 0, 1);
            tableLayoutPanel1.Controls.Add(applyFilterButton, 1, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(219, 525);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // fileTreeView
            // 
            tableLayoutPanel1.SetColumnSpan(fileTreeView, 2);
            fileTreeView.Dock = DockStyle.Fill;
            fileTreeView.DrawMode = TreeViewDrawMode.OwnerDrawText;
            fileTreeView.HideSelection = false;
            fileTreeView.Location = new Point(3, 3);
            fileTreeView.Name = "fileTreeView";
            fileTreeView.ShowNodeToolTips = true;
            fileTreeView.Size = new Size(213, 477);
            fileTreeView.TabIndex = 0;
            fileTreeView.BeforeExpand += fileTreeView_BeforeExpand;
            fileTreeView.DrawNode += fileTreeView_DrawNode;
            fileTreeView.KeyDown += fileTreeView_KeyDown;
            fileTreeView.MouseDown += fileTreeView_MouseDown;
            // 
            // extFilterTextBox
            // 
            extFilterTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            extFilterTextBox.Location = new Point(5, 488);
            extFilterTextBox.Margin = new Padding(5, 5, 5, 10);
            extFilterTextBox.Name = "extFilterTextBox";
            extFilterTextBox.PlaceholderText = "Enter text to filter";
            extFilterTextBox.Size = new Size(145, 27);
            extFilterTextBox.TabIndex = 1;
            extFilterTextBox.KeyDown += extFilterTextBox_KeyDown;
            // 
            // applyFilterButton
            // 
            applyFilterButton.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            applyFilterButton.AutoSize = true;
            applyFilterButton.Location = new Point(158, 489);
            applyFilterButton.Name = "applyFilterButton";
            applyFilterButton.Size = new Size(58, 30);
            applyFilterButton.TabIndex = 2;
            applyFilterButton.Text = "Apply";
            applyFilterButton.UseVisualStyleBackColor = true;
            applyFilterButton.Click += applyFilterButton_Click;
            // 
            // MainForm
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(882, 553);
            Controls.Add(mainSplitContainer);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MainFrom";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            DragDrop += MainForm_DragDrop;
            DragEnter += MainForm_DragEnter;
            Resize += MainForm_Resize;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            textureTableLayoutPanel.ResumeLayout(false);
            textureTableLayoutPanel.PerformLayout();
            mainTabControl.ResumeLayout(false);
            textureTabPage.ResumeLayout(false);
            textTabPage.ResumeLayout(false);
            textTableLayoutPanel.ResumeLayout(false);
            textTableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)textDataGridView).EndInit();
            fontTabPage.ResumeLayout(false);
            fontMainSplitContainer.Panel1.ResumeLayout(false);
            fontMainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)fontMainSplitContainer).EndInit();
            fontMainSplitContainer.ResumeLayout(false);
            fontPreviewSplitContainer.Panel1.ResumeLayout(false);
            fontPreviewSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)fontPreviewSplitContainer).EndInit();
            fontPreviewSplitContainer.ResumeLayout(false);
            fontRenderGb.ResumeLayout(false);
            fontRenderGb.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)fontPreviewTextPb).EndInit();
            fontInfoTablePanel.ResumeLayout(false);
            fontInfoGb.ResumeLayout(false);
            selectedGlyphGb.ResumeLayout(false);
            selectedGlyphInfoTablePanel.ResumeLayout(false);
            mainSplitContainer.Panel1.ResumeLayout(false);
            mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)mainSplitContainer).EndInit();
            mainSplitContainer.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openTextureFileToolStripMenuItem;
        private ToolStripMenuItem replaceTextureToolStripMenuItem;
        private ToolStripMenuItem exportTextureStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem aboutProgramToolStripMenuItem;
        private TableLayoutPanel textureTableLayoutPanel;
        private Label textureFormatLabel;
        private Label textureResolutionLabel;
        private Label textureSizeLabel;
        private ToolStripMenuItem saveModifedTextureMenuItem;
        private Label toolStripLabelZoom;
        private ImageView.PictureBox preivewTextureBox;
        private TabControl mainTabControl;
        private TabPage textureTabPage;
        private TabPage textTabPage;
        private TableLayoutPanel textTableLayoutPanel;
        private DataGridView textDataGridView;
        private Button saveTextButton;
        private Label textInfoLabel;
        private Button changeBgTextureViewerButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem loadTextMenuItem;
        private ToolTip toolTip1;
        private SplitContainer mainSplitContainer;
        private TreeView fileTreeView;
        private ToolStripMenuItem openResourceDirectoryMenuItem;
        private TextBox extFilterTextBox;
        private Button exportTextDataButton;
        private Button importTextDataButton;
        private TextBox searchTextTextBox;
        private TabPage fontTabPage;
        private RichTextBox fontInfoTextBox;
        private ImageView.PictureBox fontTexturePreview;
        private PictureBox fontPreviewTextPb;
        private TextBox previewFontTextBox;
        private SplitContainer fontMainSplitContainer;
        private SplitContainer fontPreviewSplitContainer;
        private GroupBox fontRenderGb;
        private GroupBox selectedGlyphGb;
        private TableLayoutPanel selectedGlyphInfoTablePanel;
        private ImageView.PictureBox glyphPreviewBox;
        private RichTextBox glyphInfoTextBox;
        private GroupBox fontInfoGb;
        private TableLayoutPanel fontInfoTablePanel;
        private ToolStripMenuItem loadFontToolStripMenuItem;
        private ToolStripMenuItem exportFontConfigurationToBMToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem generateNewFontFromBMFontToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private TableLayoutPanel tableLayoutPanel1;
        private Button applyFilterButton;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem decryptToolStripMenuItem;
        private ToolStripMenuItem enableDecryptionToTreeVisualToolStripMenuItem;
    }
}