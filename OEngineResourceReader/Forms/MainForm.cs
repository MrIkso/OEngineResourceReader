using BCnEncoder.Shared.ImageFiles;
using OEngineResourceReader.FontParser;
using OEngineResourceReader.Language;
using OEngineResourceReader.Texture;
using OEngineResourceReader.Utils;
using System.Diagnostics;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using static OEngineResourceReader.Texture.TextureProcessor;
using Timer = System.Windows.Forms.Timer;

namespace OEngineResourceReader.Forms
{
    public partial class MainForm : Form
    {
        private TextureProcessor? _textureProcessor;
        private TextProcessor _textProcessor = new TextProcessor();
        private TextureInfo? _textureInfo;
        private DdsFile? _ddsFile;
        private FontData? _loadedFont;
        private string textureFilePath = string.Empty;
        private string textFilePath = string.Empty;
        private string fontFilePath = string.Empty;
        private string newFileTempPath = string.Empty;
        private string fontTexturePath = string.Empty;
        private Timer filterTimer;
        private string _currentRootPath = string.Empty;

        private bool _isBlackBackground = true;
        private Color previewTextureBgColor = Color.Black;

        // for tree
        private const string DummyNodeText = "Loading...";
        private bool _isTreeViewLoading = false;
        private Timer _clickTimer = new();
        private TreeNode _lastClickedNode;
        private bool _isDoubleClick = false;
        private bool _isTranslationChanged = false;
        private bool _isOldFontFile = false;
        private bool _isApplyDecryptionToFileTree = false;
        private Bitmap? _fontBitmap;

        public MainForm()
        {
            InitializeComponent();
        }

        #region Form Events
        private void MainForm_Load(object sender, EventArgs e)
        {
            _textureProcessor = new TextureProcessor();
            UpdateTitle();
            mainSplitContainer.Panel1Collapsed = true;
            filterTimer = new Timer
            {
                Interval = 500
            };
            filterTimer.Tick += FilterTimer_Tick;

            _clickTimer.Interval = SystemInformation.DoubleClickTime;
            _clickTimer.Tick += ClickTimer_Tick;
            fileTreeView.TreeViewNodeSorter = new NodeSorter();
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data == null || !e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }

            string[]? filePaths = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (filePaths == null || filePaths.Length == 0)
            {
                return;
            }

            var filePath = filePaths[0];

            if (File.Exists(filePath))
            {
                LoadFile(filePath);
            }
            else if (Directory.Exists(filePath))
            {
                LoadDirectory(filePath);
            }
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data == null)
            {
                return;
            }

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _textureProcessor = null;
            _textureInfo = null;
            preivewTextureBox.Bitmap?.Dispose();
            if (!string.IsNullOrEmpty(newFileTempPath) || File.Exists(newFileTempPath))
            {
                File.Delete(newFileTempPath);
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            preivewTextureBox.Refresh();
        }
        #endregion

        #region Menu Items Clicks  
        private void openResourceDirectoryMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog diag = new FolderBrowserDialog();
            if (diag.ShowDialog() == DialogResult.OK)
            {
                string folder = diag.SelectedPath;
                LoadDirectory(folder);
            }
        }

        private void openTextureFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "All Files|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    FileTypeChecker fileTypeChecker = new(openFileDialog.FileName);
                    if (fileTypeChecker.Type == FileTypeChecker.FileType.Texture)
                    {
                        textureFilePath = openFileDialog.FileName;
                        LoadTexture();
                    }
                    else
                    {
                        MessageBox.Show("The selected file is not a valid texture file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        private void exportTextureStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textureFilePath))
            {
                MessageBox.Show("Please load a texture file first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_ddsFile == null)
            {
                MessageBox.Show("No texture information available to export.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = Path.GetDirectoryName(textureFilePath) ?? Environment.CurrentDirectory;
                saveFileDialog.FileName = Path.GetFileNameWithoutExtension(textureFilePath) + ".dds";
                saveFileDialog.Filter = "DDS Files|*.dds|All Files|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _textureProcessor?.SaveAsDds(_ddsFile, saveFileDialog.FileName);
                    MessageBox.Show("Texture exported successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void replaceTextureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "DDS Files|*.dds|All Files|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var ddsFile = openFileDialog.FileName;
                    ReplaceTexture(ddsFile);
                }
            }
        }

        private void saveModifedTextureMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = Path.GetDirectoryName(textureFilePath) ?? Environment.CurrentDirectory;
                saveFileDialog.FileName = Path.GetFileName(textureFilePath);
                saveFileDialog.Filter = "All Files|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (string.IsNullOrEmpty(newFileTempPath) || !File.Exists(newFileTempPath))
                    {
                        MessageBox.Show("No modified texture available to save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    try
                    {
                        File.Move(newFileTempPath, saveFileDialog.FileName, true);
                        MessageBox.Show("Modified texture saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        newFileTempPath = string.Empty;
                        saveModifedTextureMenuItem.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to save modified texture: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void loadTextMenuItem_Click(object sender, EventArgs e)
        {
            LoadText();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helpers.ShowFormAtTopRight(this, new HelpForm());
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"OEngine Resource Reader by MrIkso and Sent_DeZ\nVersion {Helpers.GetApplicationVersion()}", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region States
        private void SetFormLoadingState(bool isLoading)
        {
            this.Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
        }

        private void SetEnabledState(bool enabled)
        {
            exportTextureStripMenuItem.Enabled = enabled;
            replaceTextureToolStripMenuItem.Enabled = enabled;
        }
        #endregion

        private void UpdateTitle(string path = "")
        {
            if (string.IsNullOrEmpty(path))
            {
                this.Text = $"OEngine Resource Reader v{Helpers.GetApplicationVersion()}";
            }
            else
            {
                this.Text = $"OEngine Resource Reader v{Helpers.GetApplicationVersion()} - {Path.GetFileName(path)}";
            }
        }

        private void mainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = mainTabControl.SelectedIndex;
            if (selectedIndex != -1)
            {
                switch (selectedIndex)
                {
                    case 0: // Texture tab
                        UpdateTitle(textureFilePath);
                        break;
                    case 1: // Text tab
                        UpdateTitle(textFilePath);
                        break;
                    case 2: // Font tab
                        UpdateTitle(fontFilePath);
                        break;
                }
            }
        }

        #region Texture Viewer Manipulation
        private async void LoadTexture()
        {
            if (_textureProcessor == null || string.IsNullOrEmpty(textureFilePath))
                return;

            SetFormLoadingState(true);
            try
            {
                var result = await Task.Run(() =>
                {
                    var info = _textureProcessor.StartParse(textureFilePath);
                    if (info == null)
                    {
                        return null;
                    }

                    var dds = _textureProcessor.CreateDDs(info);
                    if (dds == null)
                    {
                        return null;
                    }

                    var bitmap = Helpers.DdsToBitmap(dds);

                    return new { Info = info, Dds = dds, Bmp = bitmap };
                });

                if (result?.Bmp != null)
                {
                    _textureInfo = result.Info;
                    _ddsFile = result.Dds;

                    UpdateTitle(textureFilePath);
                    preivewTextureBox.BackColor = previewTextureBgColor;
                    SetTextureInfo();
                    ShowTexture();
                    SetEnabledState(true);
                }
                else
                {
                    MessageBox.Show("Failed to load and process the texture.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SetEnabledState(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading the texture: {ex.Message}\n{ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetFormLoadingState(false);
            }
        }

        private void SetTextureInfo()
        {
            if (_textureInfo == null)
            {
                MessageBox.Show("No texture information available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            textureResolutionLabel.Text = $"{_textureInfo.Width}x{_textureInfo.Height}";
            textureSizeLabel.Text = $"Size: {Sizer.Suffix(_textureInfo.SurfaceSize, 3)}";
            textureFormatLabel.Text = $"Texture Format: {Helpers.MapDxgiToReadtable(_textureInfo.DxgiFormat)}";
        }

        private void ShowTexture()
        {
            if (_textureInfo == null)
            {
                MessageBox.Show("No texture information available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _ddsFile = _textureProcessor?.CreateDDs(_textureInfo);
            if (_ddsFile == null)
            {
                MessageBox.Show("Failed to create DDS file from texture information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Bitmap bitmap = Helpers.DdsToBitmap(_ddsFile);
            if (bitmap != null)
            {
                preivewTextureBox.Bitmap?.Dispose();
                preivewTextureBox.Bitmap = bitmap;
                mainTabControl.SelectedTab = textureTabPage;
            }
            else
            {
                MessageBox.Show("Failed to convert DDS to Bitmap.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ReplaceTexture(string newDdsPath)
        {
            SetFormLoadingState(true);
            try
            {
                var result = await Task.Run(() =>
                {
                    var newInfo = _textureProcessor.GenereateNewTextureInfo(newDdsPath);
                    if (newInfo == null) return null;

                    var tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                    bool success = _textureProcessor.ReplaceTextureInCookedFile(newInfo, textureFilePath, tempFilePath);
                    if (!success)
                    {
                        if (File.Exists(tempFilePath))
                        {
                            File.Delete(tempFilePath);
                        }
                        return null;
                    }

                    var newDds = _textureProcessor.CreateDDs(newInfo);
                    var newBitmap = (newDds != null) ? Helpers.DdsToBitmap(newDds) : null;

                    return new { Info = newInfo, Bmp = newBitmap, TempPath = tempFilePath };
                });

                if (result?.Bmp != null)
                {
                    _textureInfo = result.Info;

                    if (!string.IsNullOrEmpty(newFileTempPath) && File.Exists(newFileTempPath))
                    {
                        File.Delete(newFileTempPath);
                    }
                    newFileTempPath = result.TempPath;

                    SetTextureInfo();

                    preivewTextureBox.Bitmap?.Dispose();
                    preivewTextureBox.Bitmap = result.Bmp;

                    saveModifedTextureMenuItem.Enabled = true;
                    MessageBox.Show("Texture replaced successfully. You can now save the modified file.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to replace texture.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}\n{ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetFormLoadingState(false);
            }
        }

        private void preivewTextureBox_ZoomChanged(object sender, ImageView.PictureBox.ZoomEventArgs e)
        {
            toolStripLabelZoom.Text = $"Zoom: {(int)(e.Zoom * 100.0f)}%";
        }

        private void changeBgTextureViewerButton_Click(object sender, EventArgs e)
        {
            if (_isBlackBackground)
            {
                previewTextureBgColor = Color.White;
                changeBgTextureViewerButton.Text = "W";
            }
            else
            {
                previewTextureBgColor = Color.Black;
                changeBgTextureViewerButton.Text = "B";
            }
            preivewTextureBox.BackColor = previewTextureBgColor;
            _isBlackBackground = !_isBlackBackground;
        }

        #endregion

        #region Text Data Grid Manipulation
        private async void SetTextDataToGrid(string filePath)
        {
            if (CheckIfTranslationChanged())
            {
                return;
            }

            SetFormLoadingState(true);
            try
            {
                var result = await Task.Run(() =>
                {
                    bool success = true;
                    if (_textProcessor.TextDictionary.Count == 0)
                    {
                        success = _textProcessor.ProcessText(filePath);
                    }
                    if (success)
                    {
                        int totalWordCount = 0;
                        if (_textProcessor.TextDictionary != null)
                        {
                            foreach (string value in _textProcessor.TextDictionary.Values)
                            {
                                totalWordCount += Helpers.CountWords(value);
                            }
                        }
                        return new
                        {
                            Success = true,
                            Data = _textProcessor.TextDictionary,
                            Version = _textProcessor.FileVersion,
                            WordCount = totalWordCount
                        };
                    }
                    return new { Success = false, Data = (Dictionary<int, string>?)null, Version = 0, WordCount = 0 };
                });

                if (result.Success && result.Data != null && result.Data.Count > 0)
                {
                    this.textFilePath = filePath;
                    UpdateTitle(textFilePath);
                    textDataGridView.SuspendLayout();

                    textDataGridView.Rows.Clear();
                    textDataGridView.Columns.Clear();

                    textDataGridView.AllowUserToAddRows = false;
                    textDataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    textDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    var keyColumn = new DataGridViewTextBoxColumn
                    {
                        Name = "Key",
                        HeaderText = "Key",
                        ReadOnly = true,
                        Visible = false,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                    };
                    var lineNumColumn = new DataGridViewTextBoxColumn
                    {
                        Name = "LineNum",
                        HeaderText = "Line",
                        ReadOnly = true,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                    };
                    var valueColumn = new DataGridViewTextBoxColumn
                    {
                        Name = "Value",
                        HeaderText = "Original Text",
                        ReadOnly = true,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    };
                    var translationColumn = new DataGridViewTextBoxColumn
                    {
                        Name = "Translation",
                        HeaderText = "Translation",
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    };

                    textDataGridView.Columns.AddRange([keyColumn, lineNumColumn, valueColumn, translationColumn]);

                    var rowsToAdd = new List<DataGridViewRow>();
                    foreach (var item in result.Data)
                    {
                        var row = new DataGridViewRow();
                        row.CreateCells(textDataGridView, item.Key, item.Key + 1, item.Value, "");
                        rowsToAdd.Add(row);
                    }
                    textDataGridView.Rows.AddRange(rowsToAdd.ToArray());

                    textInfoLabel.Text = $"File Version: {result.Version} | Text Lines Count: {result.Data.Count} | Total Words: {result.WordCount}";
                    textTableLayoutPanel.Enabled = true;

                    mainTabControl.SelectedTab = textTabPage;

                    textDataGridView.ResumeLayout();
                }
                else
                {
                    MessageBox.Show("No text data found or file is invalid.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while processing the file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetFormLoadingState(false);
            }
        }

        private void textDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == textDataGridView.Columns["Translation"].Index)
            {
                var translationCell = textDataGridView.Rows[e.RowIndex].Cells["Translation"];
                /* if (translationCell.Value == null || string.IsNullOrWhiteSpace(translationCell.Value.ToString()))
                 {
                     translationCell.Value = textDataGridView.Rows[e.RowIndex].Cells["Value"].Value;
                 }*/
                saveTextButton.Enabled = true;
                _isTranslationChanged = true;
            }
        }

        private void saveTextButton_Click(object sender, EventArgs e)
        {
            var newtextDictionary = new Dictionary<int, string>();
            foreach (DataGridViewRow row in textDataGridView.Rows)
            {
                if (row.Cells["Key"].Value != null && row.Cells["Translation"].Value != null)
                {
                    int key = Convert.ToInt32(row.Cells["Key"].Value);
                    string? translation = row.Cells["Translation"].Value?.ToString();
                    if (string.IsNullOrWhiteSpace(translation))
                    {
                        translation = row.Cells["Value"].Value.ToString(); // Use original text if translation is empty
                    }
                    if (translation == null)
                    {
                        translation = string.Empty;
                    }
                    newtextDictionary[key] = translation;
                }
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = Path.GetDirectoryName(textFilePath) ?? Environment.CurrentDirectory;
                saveFileDialog.FileName = Path.GetFileName(textFilePath);
                saveFileDialog.Filter = "All Files|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _textProcessor?.GenerateTextFile(newtextDictionary, saveFileDialog.FileName);
                        MessageBox.Show("Text file saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _isTranslationChanged = false;
                        saveTextButton.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to save text file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        #endregion

        #region Timers 
        private void FilterTimer_Tick(object? sender, EventArgs e)
        {
            filterTimer.Stop();

            if (!string.IsNullOrEmpty(_currentRootPath))
            {
                LoadTreeViewFromDirectory(_currentRootPath);
            }
        }
        private void ClickTimer_Tick(object? sender, EventArgs e)
        {
            _clickTimer.Stop();
            if (!_isDoubleClick)
            {
                HandleNodeAction(_lastClickedNode, isDoubleClick: false, mouseButton: MouseButtons.Left);
            }
        }

        private bool CheckIfTranslationChanged()
        {
            if (_isTranslationChanged)
            {
                var result = MessageBox.Show("You have unsaved changes. Do you want to save them before loading a new file?",
                                             "Unsaved Changes",
                                             MessageBoxButtons.YesNoCancel,
                                             MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    saveTextButton.PerformClick();
                }
                else if (result == DialogResult.No)
                {
                    _isTranslationChanged = false;
                    saveTextButton.Enabled = false;
                }
                else if (result == DialogResult.Cancel)
                {
                    return true; // User chose to cancel the operation
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region File and Directory Loading
        private void LoadFile(string filePath)
        {
            FileTypeChecker fileTypeChecker = new(filePath);
            switch (fileTypeChecker.Type)
            {
                case FileTypeChecker.FileType.Texture:
                    textureFilePath = filePath;
                    LoadTexture();
                    return;
                case FileTypeChecker.FileType.Font:
                    if (FontProcessor.Load(filePath))
                    {
                        LoadFont(filePath);
                        return;
                    }
                    return;

                case FileTypeChecker.FileType.Materials:
                case FileTypeChecker.FileType.Animation:
                case FileTypeChecker.FileType.Level:
                case FileTypeChecker.FileType.Geometry:
                case FileTypeChecker.FileType.Collision:
                case FileTypeChecker.FileType.Object:
                case FileTypeChecker.FileType.Shader:
                case FileTypeChecker.FileType.SettingsVfx:
                case FileTypeChecker.FileType.SettingsGlobalEntityValue:
                case FileTypeChecker.FileType.Enemies:
                case FileTypeChecker.FileType.DreamShards:
                case FileTypeChecker.FileType.AchievementDefinition:
                case FileTypeChecker.FileType.Challenges:
                case FileTypeChecker.FileType.EntitySettingsResource:
                case FileTypeChecker.FileType.Cooked:
                    //case FileTypeChecker.FileType.Unknown:
                    MessageBox.Show("The selected file is not supported.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                default:
                    // MessageBox.Show("The selected file is not supported.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }

            if (_textProcessor.ProcessText(filePath))
            {
                SetTextDataToGrid(filePath);
                return;
            }
            else
            {
                MessageBox.Show("The selected file is not supported.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadText()
        {
            if (CheckIfTranslationChanged())
            {
                return;
            }
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "All Files|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SetTextDataToGrid(openFileDialog.FileName);
                }
            }
        }

        private void LoadDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                MessageBox.Show("The specified directory does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            LoadTreeViewFromDirectory(directoryPath);
            mainSplitContainer.Panel1Collapsed = false;
        }

        public void LoadTreeViewFromDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                MessageBox.Show("The specified directory does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _currentRootPath = directoryPath;

            fileTreeView.Nodes.Clear();
            var rootNode = new TreeNode(Path.GetFileName(directoryPath)) { Tag = directoryPath };
            fileTreeView.Nodes.Add(rootNode);

            rootNode.Nodes.Add(new TreeNode(DummyNodeText));
            rootNode.Expand();
        }
        #endregion
        private void extFilterTextBox_TextChanged(object sender, EventArgs e)
        {
            filterTimer.Stop();
            filterTimer.Start();
        }
        private void searchTextTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(searchTextTextBox.Text))
            {
                string searchText = searchTextTextBox.Text.ToLower();

                foreach (DataGridViewRow row in textDataGridView.Rows)
                {
                    bool matchesOriginal = row.Cells["Value"].Value != null &&
                                           row.Cells["Value"].Value.ToString()!.ToLower().Contains(searchText);

                    bool matchesTranslation = row.Cells["Translation"].Value != null &&
                                              row.Cells["Translation"].Value.ToString()!.ToLower().Contains(searchText);

                    row.Visible = matchesOriginal || matchesTranslation;
                }
            }
            else
            {
                foreach (DataGridViewRow row in textDataGridView.Rows)
                {
                    row.Visible = true;
                }
            }
        }

        #region TreeView Actions
        private async void fileTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            var nodeToExpand = e.Node;
            if (nodeToExpand?.Nodes.Count == 1 && nodeToExpand.Nodes[0].Text == DummyNodeText)
            {
                if (_isTreeViewLoading)
                {
                    e.Cancel = true;
                    return;
                }

                _isTreeViewLoading = true;
                this.Cursor = Cursors.WaitCursor;

                nodeToExpand.Nodes.Clear();
                await PopulateNodeAsync(nodeToExpand);

                this.BeginInvoke(new Action(() =>
                {
                    if (!nodeToExpand.IsExpanded)
                    {
                        nodeToExpand.Expand();
                    }

                    fileTreeView.SelectedNode = nodeToExpand;

                    if (!fileTreeView.Focused)
                    {
                        fileTreeView.Focus();
                    }

                    this.Cursor = Cursors.Default;
                    _isTreeViewLoading = false;

                }));
            }
        }

        private void fileTreeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && fileTreeView.SelectedNode != null)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;

                var selectedNode = fileTreeView.SelectedNode;
                if (selectedNode.Tag is string path && Directory.Exists(path))
                {
                    selectedNode.Toggle();
                }

                else if (selectedNode.Tag is string filePath && File.Exists(filePath))
                {
                    HandleFileDoubleClick(filePath);
                }
            }
        }

        private void fileTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            if (_isTreeViewLoading) return;

            var clickedNode = fileTreeView.GetNodeAt(e.Location);
            if (clickedNode == null)
                return;

            if (clickedNode != null && clickedNode.Bounds.Contains(e.Location))
            {
                fileTreeView.SelectedNode = clickedNode;
            }

            if (e.Button == MouseButtons.Right)
            {
                HandleNodeAction(clickedNode, isDoubleClick: false, mouseButton: e.Button);
            }
            else if (e.Button == MouseButtons.Left)
            {
                _lastClickedNode = clickedNode;

                if (e.Clicks == 1)
                {
                    _isDoubleClick = false;
                    _clickTimer.Start();
                }
                else if (e.Clicks == 2)
                {
                    _isDoubleClick = true;
                    _clickTimer.Stop();
                    HandleNodeAction(_lastClickedNode, isDoubleClick: true, mouseButton: e.Button);
                }
            }
        }


        private async Task PopulateNodeAsync(TreeNode node)
        {
            var path = node.Tag as string;
            if (path == null)
                return;

            string filterText = extFilterTextBox.Text;

            var items = await Task.Run(() =>
            {
                var filters = filterText.Split([',', ';', ' '], StringSplitOptions.RemoveEmptyEntries)
                                        .Select(f => f.Trim().Replace("*", ""))
                                        .ToList();


                var content = Helpers.GetFilteredDirectoryContent(path, filters);
                return content;
            });

            if (items.HasValue)
            {
                foreach (var dir in items.Value.Directories)
                {
                    string originalName = Path.GetFileName(dir);
                    var dirNode = new TreeNode(originalName) { Tag = dir };

                    if (_isApplyDecryptionToFileTree)
                    {
                        string decryptedName = ResourceNameDecryptor.Decrypt(originalName);
                        if (!originalName.Equals(decryptedName, StringComparison.OrdinalIgnoreCase))
                        {
                            dirNode.Text = $"{decryptedName} ({originalName})";
                            dirNode.ToolTipText = $"Decrypted: {decryptedName}\nOriginal: {originalName}";
                        }
                    }

                    dirNode.Nodes.Add(new TreeNode(DummyNodeText));
                    node.Nodes.Add(dirNode);
                }

                foreach (var file in items.Value.Files)
                {
                    string originalName = Path.GetFileName(file);
                    var fileNode = new TreeNode(originalName) { Tag = file };

                    if (_isApplyDecryptionToFileTree)
                    {
                        string decryptedName = ResourceNameDecryptor.Decrypt(originalName);
                        if (!originalName.Equals(decryptedName, StringComparison.OrdinalIgnoreCase))
                        {
                            fileNode.Text = $"{decryptedName} ({originalName})";
                            fileNode.ToolTipText = $"Decrypted: {decryptedName}\nOriginal: {originalName}";
                        }
                    }

                    node.Nodes.Add(fileNode);
                }
            }
        }

        private void HandleNodeAction(TreeNode node, bool isDoubleClick, MouseButtons mouseButton)
        {
            if (node == null) return;

            if (mouseButton == MouseButtons.Right)
            {
                // logic for right-click context menu
                if (node.Tag is string path)
                {
                    ContextMenuStrip contextMenu = new ContextMenuStrip();
                    contextMenu.Items.Add("Open in Explorer", null, (s, args) => Helpers.OpenFileInExplorer(path));
                    contextMenu.Items.Add("Copy Path", null, (s, args) => Clipboard.SetText(path));
                    if (contextMenu.Items.Count > 0)
                    {
                        contextMenu.Show(fileTreeView, fileTreeView.PointToClient(Cursor.Position));
                    }
                }
            }
            else if (mouseButton == MouseButtons.Left && isDoubleClick)
            {
                // logic for double-click action
                if (node.Tag is string path)
                {
                    if (File.Exists(path))
                    {
                        HandleFileDoubleClick(path);
                    }
                    /*else if (Directory.Exists(path))
                    {
                        node.Toggle();
                    }*/
                }
            }
        }
        #endregion
        private void HandleFileDoubleClick(string filePath)
        {
            if (!Directory.Exists(filePath))
            {
                LoadFile(filePath);
            }
        }

        private void importTextDataButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(textFilePath) ?? Environment.CurrentDirectory;
                openFileDialog.FileName = Path.GetFileNameWithoutExtension(textFilePath) + "_exported.json";
                openFileDialog.Filter = "JSON Files|*.json";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    SetJsonDataToGrid(filePath);
                }
            }
        }

        private async void SetJsonDataToGrid(string filePath)
        {
            SetFormLoadingState(true);
            try
            {
                var textDictionary = await Task.Run(() => TextProcessor.ImportTextFromJson(filePath));
                if (textDictionary != null && textDictionary.Count > 0)
                {
                    foreach (DataGridViewRow row in textDataGridView.Rows)
                    {
                        if (row.Cells["Key"].Value != null && int.TryParse(row.Cells["Key"].Value.ToString(), out int key))
                        {
                            if (textDictionary.TryGetValue(key, out string? translation))
                            {
                                var originalText = row.Cells["Value"].Value?.ToString();
                                if (!string.Equals(originalText, translation, StringComparison.Ordinal))
                                {
                                    row.Cells["Translation"].Value = translation;
                                }
                            }
                        }
                    }
                    saveTextButton.Enabled = true;
                    _isTranslationChanged = true;
                }
                else
                {
                    MessageBox.Show("No valid data found in the JSON file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load JSON data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetFormLoadingState(false);
            }
        }

        private static readonly JsonSerializerOptions CachedJsonSerializerOptions = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        private void ExportTextToJson(string savePath)
        {
            if (textDataGridView.Rows.Count == 0)
            {
                Debug.WriteLine("No text entries to export.");
                return;
            }

            var exportData = new Dictionary<string, string>();
            foreach (DataGridViewRow row in textDataGridView.Rows)
            {
                if (row.Cells["Key"].Value != null)
                {
                    string key = row.Cells["Key"].Value.ToString()!;
                    string? translation = row.Cells["Translation"].Value?.ToString();
                    string original = row.Cells["Value"].Value?.ToString() ?? string.Empty;

                    // If translation is empty or null, use the original text
                    exportData[key] = string.IsNullOrWhiteSpace(translation) ? original : translation;
                }
            }

            string jsonString = JsonSerializer.Serialize(exportData, CachedJsonSerializerOptions);
            File.WriteAllText(savePath, jsonString);
        }

        private async void exportTextDataButton_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = Path.GetDirectoryName(textFilePath) ?? Environment.CurrentDirectory;
                saveFileDialog.FileName = Path.GetFileName(textFilePath) + "_exported.json";
                saveFileDialog.Filter = "JSON Files|*.json";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SetFormLoadingState(true);
                    try
                    {
                        await Task.Run(() => ExportTextToJson(saveFileDialog.FileName));
                        MessageBox.Show("Text data exported successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to export text data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        SetFormLoadingState(false);
                    }
                }
            }
        }

        // taken from https://stackoverflow.com/a/57454490
        private void fileTreeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node != null && e.Node.TreeView != null && e.Node == e.Node.TreeView.SelectedNode)
            {
                Font font = e.Node.NodeFont ?? e.Node.TreeView.Font;
                Rectangle r = e.Bounds;
                r.Offset(0, 1);
                Brush brush = e.Node.TreeView.Focused ? SystemBrushes.Highlight : Brushes.Gray;
                e.Graphics.FillRectangle(brush, e.Bounds);
                TextRenderer.DrawText(e.Graphics, e.Node.Text, font, r, SystemColors.HighlightText, TextFormatFlags.GlyphOverhangPadding);
            }
            else
            {
                e.DrawDefault = true;
            }
        }

        #region Font Manipulation
        private async void LoadFont(string fontPath, bool forceFoad = false)
        {
            if (fontPath.EndsWith("fnt.Font.gen"))
            {
                _isOldFontFile = true;
            }
            else
            {
                _isOldFontFile = false;
            }

            SetFormLoadingState(true);
            try
            {
                _loadedFont = FontProcessor.GetFont;
                if (_loadedFont == null)
                {
                    if (!FontProcessor.Load(fontPath))
                    {
                        MessageBox.Show("Failed to load font data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    _loadedFont = FontProcessor.GetFont;
                }
                else if (forceFoad)
                {
                    if (!FontProcessor.Load(fontPath))
                    {
                        MessageBox.Show("Failed to load font data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    _loadedFont = FontProcessor.GetFont;
                }

                StringBuilder sb = new StringBuilder();
                if (!_isOldFontFile)
                {
                    sb.AppendLine($"File Version: {_loadedFont.FileVersion}");
                    sb.AppendLine($"Font Version: {_loadedFont.FontVersion}");
                    sb.AppendLine($"Unknown: {_loadedFont.UseUnicode}");
                }
                sb.AppendLine($"Base: {_loadedFont.Base}");
                sb.AppendLine($"Line Height: {_loadedFont.LineHeight}");
                sb.AppendLine($"Texture Width: {_loadedFont.TextureWidth}");
                sb.AppendLine($"Texture Height: {_loadedFont.TextureHeight}");
                sb.AppendLine($"Pages: {_loadedFont.Pages}");
                sb.AppendLine($"Glyphs count: {_loadedFont.GlyphsCount}");
                sb.AppendLine($"Kerning count: {_loadedFont.KerningCount}");
                sb.AppendLine($"Max Y Offset: {_loadedFont.MaxYOffset}");
                sb.AppendLine($"Min Y Offset: {_loadedFont.MinYOffset}");
                sb.AppendLine($"Texture file on font binary: {_loadedFont.TexturePath}");

                if (_isOldFontFile)
                {
                    sb.AppendLine("This is an old font file format. It may not be fully compatible with the current version of the font parser.");
                    sb.AppendLine("The file is .fnt file from BMFont.");
                }

                string fontDirectoryPath = Path.GetDirectoryName(fontPath);
                fontFilePath = fontPath;
                ClearGlyphInfo();
                fontTexturePath = Path.Combine(fontDirectoryPath, _loadedFont.TexturePath + ".Texture.dxt");

                if (!File.Exists(fontTexturePath))
                {
                    fontTexturePath = await Task.Run(() => Helpers.FindTextureFileForFont(_loadedFont.TexturePath, fontPath));
                }

                if (File.Exists(fontTexturePath))
                {
                    try
                    {
                        var info = await Task.Run(() => _textureProcessor?.StartParse(fontTexturePath));
                        DdsFile? dds = await Task.Run(() => _textureProcessor?.CreateDDs(info));

                        if (dds != null)
                        {
                            _fontBitmap?.Dispose();
                            _fontBitmap = null;

                            _fontBitmap = await Task.Run(() => Helpers.DdsToBitmap(dds));

                            fontTexturePreview.Bitmap = _fontBitmap;
                            sb.AppendLine($"Real found texture file: {fontTexturePath}");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while processing font texture: {ex.Message}\n{ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                fontInfoTextBox.Text = sb.ToString();
                mainTabControl.SelectedTab = fontTabPage;
                UpdateTitle(fontPath);
                exportFontConfigurationToBMToolStripMenuItem.Enabled = true;
                generateNewFontFromBMFontToolStripMenuItem.Enabled = true;
                // FontProcessor.SaveFontData(_loadedFont, "new_font.bin");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading the font: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetFormLoadingState(false);
            }
        }

        private void previewFontTextBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(previewFontTextBox.Text))
            {
                fontPreviewTextPb.Image = null;
                return;
            }
            if (_fontBitmap != null)
            {
                Bitmap? drawedText = _loadedFont?.RenderText(previewFontTextBox.Text, _fontBitmap, Color.Black);
                if (drawedText != null)
                {
                    fontPreviewTextPb.Image = drawedText;
                }
            }
        }

        private void SetGlyphInfoToTextBox(Glyph? glyph)
        {
            if (glyph == null && _loadedFont == null && _fontBitmap == null)
            {
                return;
            }
            char character = _loadedFont.FindCharacterForGlyphIndex(_loadedFont.Glyphs.IndexOf(glyph));
            string charDisplay = character != char.MaxValue ? character.ToString() : "N/A";

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Found glyph for character '{character}' (ID: {(int)character})");
            sb.AppendLine($"Index in Glyph Array: {_loadedFont.LookupTable[character]}");
            sb.AppendLine($"Position: (X: {glyph.X}, Y: {glyph.Y})");
            sb.AppendLine($"Size: (W: {glyph.Width}, H: {glyph.Height})");
            sb.AppendLine($"XOffset: {glyph.XOffset}");
            sb.AppendLine($"YOffset: {glyph.YOffset}");
            sb.AppendLine($"XAdvance: {glyph.XAdvance}");
            sb.AppendLine($"Page: {glyph.Page}");
            glyphInfoTextBox.Text = sb.ToString();

            glyphPreviewBox.Bitmap?.Dispose();
            Bitmap? glyphBitmap = _loadedFont.GetGlyphBitmap(glyph, _fontBitmap);
            if (glyphBitmap != null)
            {
                glyphPreviewBox.Bitmap = glyphBitmap;
            }
        }

        private void fontTexturePreview_PixelCoordinatesChanged(object sender, ImageView.PictureBox.CoordinatesEventArgs e)
        {
            if (_fontBitmap != null)
            {
                float x = e.PixelCoordinates.X;
                float y = e.PixelCoordinates.Y;

                Glyph? glyph = _loadedFont?.GetGlyphAtPixel(x, y);
                if (glyph != null)
                {
                    SetGlyphInfoToTextBox(glyph);
                }
            }
        }

        private void ClearGlyphInfo()
        {
            previewFontTextBox.Text = string.Empty;
            glyphInfoTextBox.Clear();

            _fontBitmap?.Dispose();
            _fontBitmap = null;

            glyphPreviewBox.Zoom = 1.0f;
            glyphPreviewBox.Bitmap?.Dispose();

            fontTexturePreview.SizeMode = ImageView.SizeMode.BestFit;
            fontTexturePreview.Bitmap?.Dispose();
            fontTexturePreview.Bitmap = null;

            fontPreviewTextPb.Image?.Dispose();
        }

        private void loadFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Files|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadFont(openFileDialog.FileName, true);
            }
        }

        private void exportFontConfigurationToBMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string data = Helpers.GenerateBmtConfigFile(_loadedFont, "");
            if (string.IsNullOrEmpty(data))
            {
                MessageBox.Show("Failed to generate BMTool configuration data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = Path.GetDirectoryName(fontFilePath) ?? Environment.CurrentDirectory;
                saveFileDialog.FileName = Path.GetFileNameWithoutExtension(fontFilePath) + "_exported.bmfc";
                saveFileDialog.Filter = "BMTool Configuration|*.bmfc";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(saveFileDialog.FileName, data);
                        MessageBox.Show("Font configuration exported successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to export font configuration: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void generateNewFontFromBMFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_loadedFont == null)
            {
                MessageBox.Show("No font loaded to generate new font data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_isOldFontFile)
            {
                MessageBox.Show("You can`t generate a new font from an old font file format.\n" +
                    "Please manually replace content from .fnt file generated from BMFont. Don't forget that texture filename on .fnt file must be same as orignal!\n" +
                    "Next replace font texture to new on this program", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "BMFont Files|*.fnt";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFile = openFileDialog.FileName;
                    GenereteNewBinaryFontFile(selectedFile);
                }
            }
        }

        private void GenereteNewBinaryFontFile(string openNewFontFilePath)
        {
            FontData? newFontData = null;
            try
            {
                newFontData = FontProcessor.GenerateFontFromBmFont(openNewFontFilePath);
                if (newFontData == null)
                {
                    MessageBox.Show("Failed to generate new font data from BMFont file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to generate new font data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = Path.GetDirectoryName(fontFilePath) ?? Environment.CurrentDirectory;
                saveFileDialog.FileName = Path.GetFileName(fontFilePath);
                saveFileDialog.Filter = "All Files|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string newFontTexturePath = Path.Combine(Path.GetDirectoryName(openNewFontFilePath), newFontData.TexturePath);
                        newFontData.FileVersion = _loadedFont.FileVersion;
                        newFontData.FontVersion = _loadedFont.FontVersion;
                        newFontData.UseUnicode = _loadedFont.UseUnicode;
                        newFontData.TexturePath = _loadedFont.TexturePath;

                        FontProcessor.SaveFontData(newFontData, saveFileDialog.FileName);
                        var newInfo = _textureProcessor.GenereateNewTextureInfo(newFontTexturePath);
                        if (newInfo == null)
                            return;

                        bool success = _textureProcessor.ReplaceTextureInCookedFile(fontTexturePath, newFontTexturePath);
                        if (success)
                        {
                            MessageBox.Show("New font data generated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadFont(saveFileDialog.FileName, true);
                            generateNewFontFromBMFontToolStripMenuItem.Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show("Failed to replace texture in the font file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to generate new font data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        #endregion

        private void applyFilterButton_Click(object sender, EventArgs e)
        {
            /* filterTimer.Stop();
             filterTimer.Start();*/
            if (!string.IsNullOrEmpty(_currentRootPath))
            {
                LoadTreeViewFromDirectory(_currentRootPath);
            }
        }

        private void extFilterTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (extFilterTextBox.Focused)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    applyFilterButton.PerformClick();
                }
            }
        }

        #region Decrypt Resource Table
        private void DecryptResourceTable(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    string[] encryptedContent = File.ReadAllLines(filePath);
                    StringBuilder sb = new();
                    foreach (var line in encryptedContent)
                    {
                        sb.Append(line);
                        sb.Append("->");
                        sb.Append(ResourceNameDecryptor.Decrypt(line));
                        sb.Append(Environment.NewLine);
                    }
                    string decryptedFileName = filePath + "_decrypted.txt";

                    if (File.Exists(decryptedFileName))
                    {
                        File.Delete(decryptedFileName);
                    }

                    File.WriteAllText(decryptedFileName, sb.ToString());

                    MessageBox.Show($"Resource table decrypted successfully.\nSaved as: {decryptedFileName}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to decrypt resource table: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("The specified file does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void decryptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "UsedRscList|*.ot";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DecryptResourceTable(openFileDialog.FileName);
                }
            }
        }
        #endregion

        private void enableDecryptionToTreeVisualToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            _isApplyDecryptionToFileTree = !_isApplyDecryptionToFileTree;
            enableDecryptionToTreeVisualToolStripMenuItem.Checked = _isApplyDecryptionToFileTree;

            if (!string.IsNullOrEmpty(_currentRootPath))
            {
                LoadTreeViewFromDirectory(_currentRootPath);
            }
        }

        private void gotoLineTextTextBox_TextChanged(object sender, EventArgs e)
        {
            string inputText = gotoLineTextTextBox.Text;
            if (string.IsNullOrEmpty(inputText))
            {
                GoToRow(1);
                return;
            }
            else if (int.TryParse(inputText, out int lineNumber) && lineNumber > 0)
            {
                GoToRow(lineNumber);
            }
            
        }

        private void GoToRow(int lineNumber)
        {
            if (lineNumber > 0)
            {
                var insibleRows = textDataGridView.Rows.Cast<DataGridViewRow>().Where(row => !row.Visible);
                foreach (DataGridViewRow row in insibleRows)
                {
                    row.Visible = true;
                }

                if (lineNumber <= textDataGridView.Rows.Count)
                {
                    textDataGridView.ClearSelection();
                    textDataGridView.Rows[lineNumber - 1].Selected = true;
                    textDataGridView.FirstDisplayedScrollingRowIndex = lineNumber - 1;
                }
                else
                {
                    return;
                }
            }
        }
    }
}
