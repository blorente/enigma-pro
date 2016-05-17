using Enigma.Cryptography;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace enigma_pro
{
    public class DialogManager
    {
        private const int _mMinimumColumnWidth = 60;
        private static bool _mIsPasswordShown;
        private static bool _mKeySet, _mKeyGet;
        private static string _mKeyPath, _mKeyPathSave;

        // Add Label
        private Label _mCustomLabel;

        // Add / Edit Entry
        private Form _mEntryDlg;
        private Label _mTitleLbl;
        private Label _mUserNameLbl;
        private Label _mPasswordLbl;
        private Label _mPasswordRptLbl;
        private Label _mUrlLbl;
        private Label _mNotesLbl;
        private TextBox _mTitleTBox;
        private TextBox _mUserNameTBox;
        private TextBox _mPasswordTBox;
        private TextBox _mPasswordRptTBox;
        private TextBox _mUrltBox;
        private RichTextBox _mNotesTBox;
        private Button _mShowPasswordBtn;
        private Button _mConfirmEntryBtn;
        private Button _mCancelBtn;

        // Change Master-Key Dialog
        private Form _mMasterKeyForm;
        private Label _mKeyLabel;
        private ComboBox _mKeyFileComboBox;
        private Button _mKeyFileLocationBtn;
        private Button _mSetKeyBtn;
        private Button _mCancelKeyBtn;

        // About-Dialog Components
        private Form _mAboutDlg;
        private Button _mCloseBtn;
        private Label _mInfoLbl;
        private Label _mCaptionLbl;
        private LinkLabel _mLinkLbl;

        // ListView Components
        private ListView _mLView;
        private ContextMenuStrip _mContextMenu;
        private ToolStripSeparator _mToolStripSeparator;
        private ToolStripMenuItem _mCopyUsernameToolStripMenuItem;
        private ToolStripMenuItem _mCopyPasswordToolStripMenuItem;
        private ToolStripMenuItem _mOpenUrlToolStripMenuItem;
        private ToolStripMenuItem _mAddEntryToolStripMenuItem;
        private ToolStripMenuItem _mEditViewEntryToolStripMenuItem;
        private ToolStripMenuItem _mDuplicateEntryToolStripMenuItem;
        private ToolStripMenuItem _mDeleteEntryToolStripMenuItem;
        private ColumnHeader _mColumnTitle;
        private ColumnHeader _mColumnUsername;
        private ColumnHeader _mColumnPassword;
        private ColumnHeader _mColumnUrl;
        private ColumnHeader _mColumnNotes;

        public Label MLabel
        {
            get { return _mCustomLabel; }
            set { _mCustomLabel = value; }
        }
        public ListView MLView
        {
            get { return _mLView; }
            set { _mLView = value; }
        }
        public ColumnHeader MColumnNotes
        {
            get { return _mColumnNotes; }
            set { _mColumnNotes = value; }
        }

        public static bool MKeySet
        {
            get { return _mKeySet; }
            set { _mKeySet = value; }
        }
        public static bool MKeyGet
        {
            get { return _mKeyGet; }
            set { _mKeyGet = value; }
        }

        //------------------------------------------------------------------------------------
        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);
        public static bool CheckUrlValid(string urlInput)
        {
            Uri uriResult;
            return Uri.TryCreate(urlInput, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
        public static void SetMenuItemProperty(MenuItem menuItem, bool toggle)
        {
            menuItem.Enabled = toggle;
        }
        public static void AddNewPicture(Form window, Size imageSize, string imagePath)
        {
            PictureBox pictureBox = new PictureBox
            {
                ImageLocation = imagePath,
                Location = new Point(0, 0),
                Size = imageSize
            };

            window.Controls.Add(pictureBox);
        }
        public static string GetKeyFilePathSave()
        {
            return _mKeyPathSave;
        }
        public static string GetKeyFilePath()
        {
            return _mKeyPath;
        }
        public void FitColumnWidth()
        {
            _mColumnTitle.Width = -2;
            _mColumnUsername.Width = -2;
            _mColumnPassword.Width = -2;
            _mColumnUrl.Width = -2;
            _mColumnNotes.Width = -2;
        }
        public void FillListViewItemColors()
        {
            foreach (ListViewItem item in _mLView.Items)
                item.BackColor = item.Index % 2 == 0 ? Color.White : Color.FromArgb(246, 246, 246);
        }
        public void CopyUsernameToClipboard()
        {
            foreach (ListViewItem item in _mLView.SelectedItems)
            {
                if (!string.IsNullOrEmpty(item.SubItems[1].Text))
                    Clipboard.SetText(item.SubItems[1].Text);
            }
        }
        public void CopyPasswordToClipboard()
        {
            foreach (ListViewItem item in _mLView.SelectedItems)
            {
                if (!string.IsNullOrEmpty(item.SubItems[2].Text))
                    Clipboard.SetText(item.SubItems[2].Text);
            }
        }
        public void OpenUrl()
        {
            foreach (ListViewItem item in _mLView.SelectedItems)
            {
                if (CheckUrlValid(item.SubItems[3].Text))
                    System.Diagnostics.Process.Start(item.SubItems[3].Text);
            }
        }
        public void DeleteSelectedEntry()
        {
            foreach (ListViewItem item in _mLView.SelectedItems)
            {
                DialogResult dialogResult = MessageBox.Show(
                    $"Are you sure you want to delete the entry \"{item.SubItems[0].Text}\"?", @"Delete?", MessageBoxButtons.OKCancel,
                                                                                                          MessageBoxIcon.Question,
                                                                                                          MessageBoxDefaultButton.Button1);
                if (dialogResult == DialogResult.OK)
                    item.Remove();
            }

            FillListViewItemColors();
        }
        public void DuplicateEntry()
        {
            // Get Selected Entry Items
            string title = string.Empty;
            string username = string.Empty;
            string password = string.Empty;
            string url = string.Empty;
            string notes = string.Empty;
            
            foreach (ListViewItem item in _mLView.SelectedItems)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (string.IsNullOrEmpty(item.SubItems[i].Text)) continue;
                    title = item.SubItems[0].Text;
                    username = item.SubItems[1].Text;
                    password = item.SubItems[2].Text;
                    url = item.SubItems[3].Text;
                    notes = item.SubItems[4].Text;
                }
            }

            ListViewItem lvItems = new ListViewItem(title);

            lvItems.SubItems.Add(username);
            lvItems.SubItems.Add(password);
            lvItems.SubItems.Add(url);
            lvItems.SubItems.Add(notes);

            _mLView.Items.Add(lvItems);

            FitColumnWidth();
            FillListViewItemColors();
        }
        public void AddNewLabel(Form window, string caption)
        {
            _mCustomLabel = new Label
            {
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Text = caption,
                AutoSize = false
            };

            window.Controls.Add(_mCustomLabel);
        }
        public void InitializeAboutDialog()
        {
            // Initialize Components
            _mAboutDlg = new Form();
            _mCloseBtn = new Button();
            _mInfoLbl = new Label();
            _mCaptionLbl = new Label();
            _mLinkLbl = new LinkLabel();

            // About Dialog
            _mAboutDlg.Size = new Size(320, 270);
            _mAboutDlg.Text = "About enigma-pro";
            _mAboutDlg.StartPosition = FormStartPosition.CenterScreen;
            _mAboutDlg.FormBorderStyle = FormBorderStyle.FixedDialog;
            _mAboutDlg.ShowInTaskbar = false;
            _mAboutDlg.MaximizeBox = false;
            _mAboutDlg.MinimizeBox = false;
            _mAboutDlg.HelpButton = true;
            _mAboutDlg.CancelButton = _mCloseBtn;

            // Close Button
            _mCloseBtn.Location = new Point(220, 200);
            _mCloseBtn.Text = "Close";

            // Info Label
            _mInfoLbl.Location = new Point(5, 80);
            _mInfoLbl.AutoSize = true;
            _mInfoLbl.Text = "enigma-pro is distributed under the term of the GNU General\nPublic License (GPL) version 2 or (at your option) version\n3.";
            _mInfoLbl.Text += "\n\nUsing:\n- Visual Studio 2013\n- C#";

            // Title Label
            _mCaptionLbl.Location = new Point(60, 25);
            _mCaptionLbl.AutoSize = true;
            _mCaptionLbl.Font = new Font("Arial", 12, FontStyle.Bold);
            _mCaptionLbl.Text = "Enigma-Pro 1.0.0";

            // Github Label
            _mLinkLbl.Location = new Point(5, 60);
            _mLinkLbl.AutoSize = true;
            _mLinkLbl.Text = "https://github.com/3n16m4/enigma-pro";
            _mLinkLbl.LinkClicked += new LinkLabelLinkClickedEventHandler(OnLinkLblClicked);

            // Add Controls to Dialog
            _mAboutDlg.Controls.AddRange(new Control[]
            {
                _mCloseBtn,
                _mInfoLbl,
                _mCaptionLbl,
                _mLinkLbl
            });

            // Display initial Dialog
            _mAboutDlg.ShowDialog();
        }
        public void InitializeListView(Form window, Size listViewSize)
        {
            if (window == null) throw new ArgumentNullException(nameof(window));

            // Initialize Components
            _mLView = new ListView();
            _mContextMenu = new ContextMenuStrip();

            _mToolStripSeparator = new ToolStripSeparator();
            _mCopyUsernameToolStripMenuItem = new ToolStripMenuItem();
            _mCopyPasswordToolStripMenuItem = new ToolStripMenuItem();
            _mOpenUrlToolStripMenuItem = new ToolStripMenuItem();
            _mAddEntryToolStripMenuItem = new ToolStripMenuItem();
            _mEditViewEntryToolStripMenuItem = new ToolStripMenuItem();
            _mDuplicateEntryToolStripMenuItem = new ToolStripMenuItem();
            _mDeleteEntryToolStripMenuItem = new ToolStripMenuItem();

            _mColumnTitle = new ColumnHeader();
            _mColumnUsername = new ColumnHeader();
            _mColumnPassword = new ColumnHeader();
            _mColumnUrl = new ColumnHeader();
            _mColumnNotes = new ColumnHeader();

            _mToolStripSeparator.Size = new Size(196, 6);

            _mCopyUsernameToolStripMenuItem.Image = Properties.Resources.username_copy;
            _mCopyUsernameToolStripMenuItem.ShortcutKeys = (Keys.Control | Keys.B);
            _mCopyUsernameToolStripMenuItem.Size = new Size(199, 22);
            _mCopyUsernameToolStripMenuItem.Text = "Copy Username";
            _mCopyUsernameToolStripMenuItem.Click += new EventHandler(OnCopyUsernameToolStripMenuItemClicked);

            _mCopyPasswordToolStripMenuItem.Image = Properties.Resources.password_copy;
            _mCopyPasswordToolStripMenuItem.ShortcutKeys = (Keys.Control | Keys.C);
            _mCopyPasswordToolStripMenuItem.Size = new Size(199, 22);
            _mCopyPasswordToolStripMenuItem.Text = "Copy Password";
            _mCopyPasswordToolStripMenuItem.Click += new EventHandler(OnCopyPasswordToolStripMenuItemClicked);

            _mOpenUrlToolStripMenuItem.Image = Properties.Resources.globe_africa;
            _mOpenUrlToolStripMenuItem.ShortcutKeys = (Keys.Control | Keys.U);
            _mOpenUrlToolStripMenuItem.Size = new Size(199, 22);
            _mOpenUrlToolStripMenuItem.Text = "Open URL";
            _mOpenUrlToolStripMenuItem.Click += new EventHandler(OnOpenUrlToolStripMenuItemClicked);

            _mAddEntryToolStripMenuItem.Image = Properties.Resources.entry_new;
            _mAddEntryToolStripMenuItem.ShortcutKeys = (Keys.Control | Keys.Y);
            _mAddEntryToolStripMenuItem.Size = new Size(199, 22);
            _mAddEntryToolStripMenuItem.Text = "Add Entry";
            _mAddEntryToolStripMenuItem.Click += new EventHandler(OnAddEntryToolStripMenuItemClicked);

            _mEditViewEntryToolStripMenuItem.Image = Properties.Resources.entry_edit;
            _mEditViewEntryToolStripMenuItem.ShortcutKeys = (Keys.Control | Keys.E);
            _mEditViewEntryToolStripMenuItem.Size = new Size(199, 22);
            _mEditViewEntryToolStripMenuItem.Text = "Edit/View Entry";
            _mEditViewEntryToolStripMenuItem.Click += new EventHandler(OnEditEntryToolStripMenuItemClicked);

            _mDuplicateEntryToolStripMenuItem.Image = Properties.Resources.entry_clone;
            _mDuplicateEntryToolStripMenuItem.ShortcutKeys = (Keys.Control | Keys.K);
            _mDuplicateEntryToolStripMenuItem.Size = new Size(199, 22);
            _mDuplicateEntryToolStripMenuItem.Text = "Clone Entry";
            _mDuplicateEntryToolStripMenuItem.Click += new EventHandler(OnDuplicateEntryToolStripMenuItemClicked);

            _mDeleteEntryToolStripMenuItem.Image = Properties.Resources.entry_delete;
            _mDeleteEntryToolStripMenuItem.ShortcutKeys = (Keys.Control | Keys.D);
            _mDeleteEntryToolStripMenuItem.Size = new Size(199, 22);
            _mDeleteEntryToolStripMenuItem.Text = "Delete Entry";
            _mDeleteEntryToolStripMenuItem.Click += new EventHandler(OnDeleteEntryToolStripMenuItemClicked);

            _mContextMenu.Size = new Size(200, 142);
            _mContextMenu.Opening += new CancelEventHandler(OnContextMenuOpening);
            _mContextMenu.Items.AddRange(new ToolStripItem[]
            {
                _mCopyUsernameToolStripMenuItem,
                _mCopyPasswordToolStripMenuItem,
                _mOpenUrlToolStripMenuItem,
                _mToolStripSeparator,
                _mAddEntryToolStripMenuItem,
                _mEditViewEntryToolStripMenuItem,
                _mDuplicateEntryToolStripMenuItem,
                _mDeleteEntryToolStripMenuItem
            });

            _mColumnTitle.Text = "Title";
            _mColumnTitle.Width = 40;
            _mColumnUsername.Text = "Username";
            _mColumnPassword.Text = "Password";
            _mColumnUrl.Text = "URL";
            _mColumnUrl.Width = 40;
            _mColumnNotes.Text = "Notes";
            _mColumnNotes.Width = -2;

            _mLView.ContextMenuStrip = _mContextMenu;
            _mLView.ColumnWidthChanged += new ColumnWidthChangedEventHandler(OnListViewColumnWidthChanged);
            _mLView.KeyDown += new KeyEventHandler(OnListViewKeyDown);
            _mLView.Location = new Point(16, 15);
            _mLView.Size = listViewSize;
            _mLView.Anchor = (((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right);
            _mLView.Columns.AddRange(new ColumnHeader[]
            {
                _mColumnTitle,
                _mColumnUsername,
                _mColumnPassword,
                _mColumnUrl,
                _mColumnNotes
            });
            _mLView.View = View.Details;
            _mLView.FullRowSelect = true;
            _mLView.MultiSelect = true;
            _mLView.UseCompatibleStateImageBehavior = false;

            window.Controls.Add(_mLView);
        }
        public void InitializeAddNewEntry()
        {
            // Initialize Components
            _mEntryDlg = new Form();

            _mTitleLbl = new Label(); ;
            _mUserNameLbl = new Label();
            _mPasswordLbl = new Label();
            _mPasswordRptLbl = new Label();
            _mUrlLbl = new Label();
            _mNotesLbl = new Label();

            _mTitleTBox = new TextBox();
            _mUserNameTBox = new TextBox();
            _mPasswordTBox = new TextBox();
            _mPasswordRptTBox = new TextBox();
            _mUrltBox = new TextBox();
            _mNotesTBox = new RichTextBox();

            _mShowPasswordBtn = new Button();
            _mConfirmEntryBtn = new Button();
            _mCancelBtn = new Button();

            // Add Entry Dialog
            _mEntryDlg.Size = new Size(490, 390);
            _mEntryDlg.Text = "Add Entry";
            _mEntryDlg.StartPosition = FormStartPosition.CenterScreen;
            _mEntryDlg.FormBorderStyle = FormBorderStyle.FixedDialog;
            _mEntryDlg.ShowInTaskbar = false;
            _mEntryDlg.MaximizeBox = false;
            _mEntryDlg.MinimizeBox = false;
            _mEntryDlg.AcceptButton = _mConfirmEntryBtn;
            _mEntryDlg.CancelButton = _mCancelBtn;

            // Title Label
            _mTitleLbl.Location = new Point(5, 60);
            _mTitleLbl.Text = "Title:\t";

            // Username Label
            _mUserNameLbl.Location = new Point(5, 90);
            _mUserNameLbl.Text = "Username:\t";

            // Password Label
            _mPasswordLbl.Location = new Point(5, 120);
            _mPasswordLbl.Text = "Password:\t";

            // Password Repeat Label
            _mPasswordRptLbl.Location = new Point(5, 150);
            _mPasswordRptLbl.Text = "Repeat:\t";

            // URL Label
            _mUrlLbl.Location = new Point(5, 180);
            _mUrlLbl.Text = "URL:\t";

            // Notes Label
            _mNotesLbl.Location = new Point(5, 210);
            _mNotesLbl.Text = "Notes:\t";

            // Title TextBox
            _mTitleTBox.Location = new Point(115, 60);
            _mTitleTBox.Size = new Size(350, 20);

            // Username TextBox
            _mUserNameTBox.Location = new Point(115, 90);
            _mUserNameTBox.Size = new Size(350, 20);

            // Password TextBox
            _mPasswordTBox.Location = new Point(115, 120);
            _mPasswordTBox.Size = new Size(322, 20);
            _mPasswordTBox.UseSystemPasswordChar = true;

            // Hide/Show Password
            _mShowPasswordBtn.Location = new Point(441, 120);
            _mShowPasswordBtn.Size = new Size(24, 24);
            _mShowPasswordBtn.Image = Image.FromFile("share/icons/16x16/lock.png");
            _mShowPasswordBtn.Click += new EventHandler(OnPasswordMaskClicked);

            // Password Repeat TextBox
            _mPasswordRptTBox.Location = new Point(115, 150);
            _mPasswordRptTBox.Size = new Size(322, 20);
            _mPasswordRptTBox.UseSystemPasswordChar = true;

            // URL TextBox
            _mUrltBox.Location = new Point(115, 180);
            _mUrltBox.Size = new Size(350, 20);

            // Notes RichTextBox
            _mNotesTBox.Location = new Point(115, 210);
            _mNotesTBox.Size = new Size(350, 100);
            _mNotesTBox.Multiline = true;
            _mNotesTBox.AcceptsTab = true;
            _mNotesTBox.WordWrap = true;

            // Add Entry Button
            _mConfirmEntryBtn.Location = new Point(305, 320);
            _mConfirmEntryBtn.Text = "OK";
            _mConfirmEntryBtn.Click += new EventHandler(OnAddEntryBtnClicked);

            // Cancel Button
            _mCancelBtn.Location = new Point(390, 320);
            _mCancelBtn.Text = "Cancel";

            _mEntryDlg.Controls.AddRange(new Control[]
            {
                _mTitleLbl,
                _mUserNameLbl,
                _mPasswordLbl,
                _mPasswordRptLbl,
                _mUrlLbl,
                _mNotesLbl,
                _mTitleTBox,
                _mUserNameTBox,
                _mPasswordTBox,
                _mPasswordRptTBox,
                _mUrltBox,
                _mNotesTBox,
                _mShowPasswordBtn,
                _mConfirmEntryBtn,
                _mCancelBtn
            });

            AddNewPicture(_mEntryDlg, new Size(475, 50), "share/banners/add-entry-banner.png");
            _mEntryDlg.ShowDialog();
        }
        public void InitializeEditEntry()
        {
            // Initialize Components
            _mEntryDlg = new Form();

            _mTitleLbl = new Label(); ;
            _mUserNameLbl = new Label();
            _mPasswordLbl = new Label();
            _mPasswordRptLbl = new Label();
            _mUrlLbl = new Label();
            _mNotesLbl = new Label();

            _mTitleTBox = new TextBox();
            _mUserNameTBox = new TextBox();
            _mPasswordTBox = new TextBox();
            _mPasswordRptTBox = new TextBox();
            _mUrltBox = new TextBox();
            _mNotesTBox = new RichTextBox();

            _mShowPasswordBtn = new Button();
            _mConfirmEntryBtn = new Button();
            _mCancelBtn = new Button();
            
            foreach (ListViewItem item in _mLView.SelectedItems)
            {
                _mTitleTBox.Text = item.SubItems[0].Text;
                _mUserNameTBox.Text = item.SubItems[1].Text;
                _mPasswordTBox.Text = item.SubItems[2].Text;
                _mPasswordRptTBox.Text = item.SubItems[2].Text;
                _mUrltBox.Text = item.SubItems[3].Text;
                _mNotesTBox.Text = item.SubItems[4].Text;
            }

            // Edit Entry Dialog
            _mEntryDlg.Size = new Size(490, 390);
            _mEntryDlg.Text = "Edit Entry";
            _mEntryDlg.StartPosition = FormStartPosition.CenterScreen;
            _mEntryDlg.FormBorderStyle = FormBorderStyle.FixedDialog;
            _mEntryDlg.ShowInTaskbar = false;
            _mEntryDlg.MaximizeBox = false;
            _mEntryDlg.MinimizeBox = false;
            _mEntryDlg.AcceptButton = _mConfirmEntryBtn;
            _mEntryDlg.CancelButton = _mCancelBtn;

            // Title Label
            _mTitleLbl.Location = new Point(5, 60);
            _mTitleLbl.Text = "Title:\t";

            // Username Label
            _mUserNameLbl.Location = new Point(5, 90);
            _mUserNameLbl.Text = "Username:\t";

            // Password Label
            _mPasswordLbl.Location = new Point(5, 120);
            _mPasswordLbl.Text = "Password:\t";

            // Password Repeat Label
            _mPasswordRptLbl.Location = new Point(5, 150);
            _mPasswordRptLbl.Text = "Repeat:\t";

            // URL Label
            _mUrlLbl.Location = new Point(5, 180);
            _mUrlLbl.Text = "URL:\t";

            // Notes Label
            _mNotesLbl.Location = new Point(5, 210);
            _mNotesLbl.Text = "Notes:\t";

            // Title TextBox
            _mTitleTBox.Location = new Point(115, 60);
            _mTitleTBox.Size = new Size(350, 20);

            // Username TextBox
            _mUserNameTBox.Location = new Point(115, 90);
            _mUserNameTBox.Size = new Size(350, 20);

            // Password TextBox
            _mPasswordTBox.Location = new Point(115, 120);
            _mPasswordTBox.Size = new Size(322, 20);
            _mPasswordTBox.UseSystemPasswordChar = true;

            // Hide/Show Password
            _mShowPasswordBtn.Location = new Point(441, 120);
            _mShowPasswordBtn.Size = new Size(24, 24);
            _mShowPasswordBtn.Image = Image.FromFile("share/icons/16x16/lock.png");
            _mShowPasswordBtn.Click += new EventHandler(OnPasswordMaskClicked);

            // Password Repeat TextBox
            _mPasswordRptTBox.Location = new Point(115, 150);
            _mPasswordRptTBox.Size = new Size(322, 20);
            _mPasswordRptTBox.UseSystemPasswordChar = true;

            // URL TextBox
            _mUrltBox.Location = new Point(115, 180);
            _mUrltBox.Size = new Size(350, 20);

            // Notes RichTextBox
            _mNotesTBox.Location = new Point(115, 210);
            _mNotesTBox.Size = new Size(350, 100);
            _mNotesTBox.Multiline = true;
            _mNotesTBox.AcceptsTab = true;
            _mNotesTBox.WordWrap = true;

            // Edit Entry Button
            _mConfirmEntryBtn.Location = new Point(305, 320);
            _mConfirmEntryBtn.Text = "OK";
            _mConfirmEntryBtn.Click += new EventHandler(OnEditEntryBtnClicked);

            // Cancel Button
            _mCancelBtn.Location = new Point(390, 320);
            _mCancelBtn.Text = "Cancel";

            _mEntryDlg.Controls.AddRange(new Control[]
            {
                _mTitleLbl,
                _mUserNameLbl,
                _mPasswordLbl,
                _mPasswordRptLbl,
                _mUrlLbl,
                _mNotesLbl,
                _mTitleTBox,
                _mUserNameTBox,
                _mPasswordTBox,
                _mPasswordRptTBox,
                _mUrltBox,
                _mNotesTBox,
                _mShowPasswordBtn,
                _mConfirmEntryBtn,
                _mCancelBtn
            });

            AddNewPicture(_mEntryDlg, new Size(475, 50), "share/banners/edit-entry-banner.png");
            if (_mLView.SelectedItems.Count > 0)
                _mEntryDlg.ShowDialog();
        }
        public void InitializeSetKeyFile()
        {
            _mMasterKeyForm = new Form();

            _mKeyLabel = new Label();
            _mKeyFileComboBox = new ComboBox();
            _mKeyFileLocationBtn = new Button();
            _mSetKeyBtn = new Button();
            _mCancelKeyBtn = new Button();

            // Set Key-File Dialog
            _mMasterKeyForm.Size = new Size(340, 190);
            _mMasterKeyForm.Text = "Create new Key File";
            _mMasterKeyForm.StartPosition = FormStartPosition.CenterScreen;
            _mMasterKeyForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            _mMasterKeyForm.ShowInTaskbar = false;
            _mMasterKeyForm.MaximizeBox = false;
            _mMasterKeyForm.MinimizeBox = false;
            _mMasterKeyForm.AcceptButton = _mSetKeyBtn;
            _mMasterKeyForm.CancelButton = _mCancelKeyBtn;

            // Key Label
            _mKeyLabel.Location = new Point(5, 60);
            _mKeyLabel.Font = new Font("Arial", _mKeyLabel.Font.Size, FontStyle.Bold);
            _mKeyLabel.Text = "Key File:";
            _mKeyLabel.AutoSize = true;

            // Key File ComboBox
            _mKeyFileComboBox.Location = new Point(7, 75);
            _mKeyFileComboBox.Size = new Size(280, 20);
            _mKeyFileComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            string[] sDrives = Directory.GetLogicalDrives();
            foreach (string drive in sDrives)
                _mKeyFileComboBox.Items.Add(drive + "pwsafe.enigma");
            _mKeyFileComboBox.SelectedIndex = 0;

            // Key File Location Button
            _mKeyFileLocationBtn.Location = new Point(290, 74);
            _mKeyFileLocationBtn.Size = new Size(24, 24);
            _mKeyFileLocationBtn.Image = Image.FromFile(@"share/icons/16x16/file_save_as.png");
            _mKeyFileLocationBtn.Click += new EventHandler(OnSetKeyFileBtnClicked);

            // Set Key Button
            _mSetKeyBtn.Location = new Point(160, 120);
            _mSetKeyBtn.Text = "OK";
            _mSetKeyBtn.Click += new EventHandler(OnSaveKeyFileBtnClicked);

            // Cancel Button
            _mCancelKeyBtn.Location = new Point(240, 120);
            _mCancelKeyBtn.Text = "Cancel";

            _mMasterKeyForm.Controls.AddRange(new Control[]
            {
                _mKeyLabel,
                _mKeyFileComboBox,
                _mKeyFileLocationBtn,
                _mSetKeyBtn,
                _mCancelKeyBtn
            });

            AddNewPicture(_mMasterKeyForm, new Size(475, 50), @"share/banners/create-key-banner.png");
            _mMasterKeyForm.ShowDialog();
        }
        public void InitializeGetKeyFile()
        {
            _mMasterKeyForm = new Form();

            _mKeyLabel = new Label();
            _mKeyFileComboBox = new ComboBox();
            _mKeyFileLocationBtn = new Button();
            _mSetKeyBtn = new Button();
            _mCancelKeyBtn = new Button();

            // Change Master-Key Dialog
            _mMasterKeyForm.Size = new Size(340, 190);
            _mMasterKeyForm.Text = "Open Database";
            _mMasterKeyForm.StartPosition = FormStartPosition.CenterScreen;
            _mMasterKeyForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            _mMasterKeyForm.ShowInTaskbar = false;
            _mMasterKeyForm.MaximizeBox = false;
            _mMasterKeyForm.MinimizeBox = false;
            _mMasterKeyForm.AcceptButton = _mSetKeyBtn;
            _mMasterKeyForm.CancelButton = _mCancelKeyBtn;

            // Key Label
            _mKeyLabel.Location = new Point(5, 60);
            _mKeyLabel.Font = new Font("Arial", _mKeyLabel.Font.Size, FontStyle.Bold);
            _mKeyLabel.Text = "Key File:";
            _mKeyLabel.AutoSize = true;

            // Key File ComboBox
            _mKeyFileComboBox.Location = new Point(7, 75);
            _mKeyFileComboBox.Size = new Size(280, 20);
            _mKeyFileComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _mKeyFileComboBox.Items.Add(XmlHandler.GetRecentKeyFilePath());
            _mKeyFileComboBox.SelectedIndex = 0;

            // Key File Location Button
            _mKeyFileLocationBtn.Location = new Point(290, 74);
            _mKeyFileLocationBtn.Size = new Size(24, 24);
            _mKeyFileLocationBtn.Image = Image.FromFile(@"share/icons/16x16/file_save_as.png");
            _mKeyFileLocationBtn.Click += new EventHandler(OnGetKeyFileBtnClicked);

            // Set Key Button
            _mSetKeyBtn.Location = new Point(160, 120);
            _mSetKeyBtn.Text = "OK";
            _mSetKeyBtn.Click += new EventHandler(OnLoadKeyFileBtnClicked);

            // Cancel Button
            _mCancelKeyBtn.Location = new Point(240, 120);
            _mCancelKeyBtn.Text = "Cancel";

            _mMasterKeyForm.Controls.AddRange(new Control[]
            {
                _mKeyLabel,
                _mKeyFileComboBox,
                _mKeyFileLocationBtn,
                _mSetKeyBtn,
                _mCancelKeyBtn
            });

            AddNewPicture(_mMasterKeyForm, new Size(475, 50), @"share/banners/open-key-banner.png");
            _mMasterKeyForm.ShowDialog();
        }
        //------------------------------------------------------------------------------------
        private void OnCopyUsernameToolStripMenuItemClicked(object sender, EventArgs e)
        {
            CopyUsernameToClipboard();
        }
        private void OnCopyPasswordToolStripMenuItemClicked(object sender, EventArgs e)
        {
            CopyPasswordToClipboard();
        }
        private void OnOpenUrlToolStripMenuItemClicked(object sender, EventArgs e)
        {
            OpenUrl();
        }
        private void OnAddEntryToolStripMenuItemClicked(object sender, EventArgs e)
        {
            InitializeAddNewEntry();
        }
        private void OnEditEntryToolStripMenuItemClicked(object sender, EventArgs e)
        {
            InitializeEditEntry();
        }
        private void OnDuplicateEntryToolStripMenuItemClicked(object sender, EventArgs e)
        {
            DuplicateEntry();
        }
        private void OnDeleteEntryToolStripMenuItemClicked(object sender, EventArgs e)
        {
            DeleteSelectedEntry();
        }
        private void OnPasswordMaskClicked(object sender, EventArgs e)
        {
            if (_mIsPasswordShown == false)
            {
                // Unlock - Show Password
                _mIsPasswordShown = true;
                _mShowPasswordBtn.Image = Image.FromFile("share/icons/16x16/lock_open.png");
                _mPasswordTBox.UseSystemPasswordChar = false;
                _mPasswordRptTBox.UseSystemPasswordChar = false;
            }
            else
            {
                // Lock - Hide Password
                _mIsPasswordShown = false;
                _mShowPasswordBtn.Image = Image.FromFile("share/icons/16x16/lock.png");
                _mPasswordTBox.UseSystemPasswordChar = true;
                _mPasswordRptTBox.UseSystemPasswordChar = true;
            }
        }
        private void OnContextMenuOpening(object sender, CancelEventArgs e)
        {
            if (_mLView.SelectedItems.Count == 1)
            {
                _mCopyUsernameToolStripMenuItem.Enabled = true;
                _mCopyPasswordToolStripMenuItem.Enabled = true;
                _mEditViewEntryToolStripMenuItem.Enabled = true;
                _mDuplicateEntryToolStripMenuItem.Enabled = true;
                _mOpenUrlToolStripMenuItem.Enabled = true;
                _mDeleteEntryToolStripMenuItem.Enabled = true;
            }
            else if (_mLView.SelectedItems.Count > 1)
            {
                _mCopyUsernameToolStripMenuItem.Enabled = false;
                _mCopyPasswordToolStripMenuItem.Enabled = false;
                _mEditViewEntryToolStripMenuItem.Enabled = false;
                _mDuplicateEntryToolStripMenuItem.Enabled = false;
                _mOpenUrlToolStripMenuItem.Enabled = false;
            }
            else
            {
                _mCopyUsernameToolStripMenuItem.Enabled = false;
                _mCopyPasswordToolStripMenuItem.Enabled = false;
                _mEditViewEntryToolStripMenuItem.Enabled = false;
                _mDuplicateEntryToolStripMenuItem.Enabled = false;
                _mOpenUrlToolStripMenuItem.Enabled = false;
                _mDeleteEntryToolStripMenuItem.Enabled = false;
            }
        }
        private void OnListViewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.A || !e.Control) return;
            foreach (ListViewItem item in _mLView.Items)
                item.Selected = true;
        }
        private void OnListViewColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if (_mLView.Columns[e.ColumnIndex].Width < _mMinimumColumnWidth)
                _mLView.Columns[e.ColumnIndex].Width = _mMinimumColumnWidth;
        }
        private void OnLinkLblClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Open Github URL
            _mLinkLbl.LinkVisited = true;
            System.Diagnostics.Process.Start(_mLinkLbl.Text);
        }
        private void OnSetKeyFileBtnClicked(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "Key Files|*.enigma",
                Title = "Save as..."
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
            _mKeyFileComboBox.Items[0] = saveFileDialog.FileName;
        }
        private void OnGetKeyFileBtnClicked(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Key File|*.enigma",
                Title = "Open..."
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            _mKeyFileComboBox.Items[0] = openFileDialog.FileName;
        }
        private void OnSaveKeyFileBtnClicked(object sender, EventArgs e)
        {
            SaveKeyFile();
            _mMasterKeyForm.Close();
        }
        private void OnLoadKeyFileBtnClicked(object sender, EventArgs e)
        {
            _mKeyPath = _mKeyFileComboBox.SelectedItem.ToString();

            _mKeyGet = true;
            _mMasterKeyForm.Close();
        }
        private void OnAddEntryBtnClicked(object sender, EventArgs e)
        {
            if (_mPasswordTBox.Text == _mPasswordRptTBox.Text)
            {
                AddNewEntry(_mTitleTBox.Text, _mUserNameTBox.Text, _mPasswordTBox.Text, _mUrltBox.Text, _mNotesTBox.Text);
                _mEntryDlg.Close();
            }
            else
                MessageBox.Show("Password and repeated password don't match!", _mEntryDlg.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void OnEditEntryBtnClicked(object sender, EventArgs e)
        {
            if (_mPasswordTBox.Text == _mPasswordRptTBox.Text)
            {
                EditEntry(_mTitleTBox.Text, _mUserNameTBox.Text, _mPasswordTBox.Text, _mUrltBox.Text, _mNotesTBox.Text);
                _mEntryDlg.Close();
            }
            else
                MessageBox.Show("Password and repeated password don't match!", _mEntryDlg.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        //------------------------------------------------------------------------------------
        private void SaveKeyFile()
        {
            _mKeyPathSave = _mKeyFileComboBox.SelectedItem.ToString();
            try
            {
                string sSecretKey = Cryptography.GenerateKey();
                GCHandle gch = GCHandle.Alloc(sSecretKey, GCHandleType.Pinned);
                StreamWriter fsSecretKey = new StreamWriter(_mKeyPathSave);
                fsSecretKey.WriteLine(sSecretKey);
                fsSecretKey.Close();
                // free the key from memory
                Cryptography.ZeroMemory(gch.AddrOfPinnedObject(), sSecretKey.Length * 2);
                gch.Free();
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }

            _mKeySet = true;
        }
        private void AddNewEntry(string title, string username, string password, string url, string notes)
        {
            ListViewItem lvItems = new ListViewItem(title);

            lvItems.SubItems.Add(username);
            lvItems.SubItems.Add(password);
            if (url.StartsWith("http://") || url.StartsWith("https://"))
                lvItems.SubItems.Add(url);
            else
            {
                string formattedUrl = url.Insert(0, "http://");
                lvItems.SubItems.Add(formattedUrl);
            }
            lvItems.SubItems.Add(notes);

            _mLView.Items.Add(lvItems);

            FitColumnWidth();
            FillListViewItemColors();
        }
        private void EditEntry(string title, string username, string password, string url, string notes)
        {
            foreach (ListViewItem item in _mLView.SelectedItems)
            {
                item.SubItems[0].Text = title;
                item.SubItems[1].Text = username;
                item.SubItems[2].Text = password;
                if (url.StartsWith("http://") || url.StartsWith("https://"))
                    item.SubItems[3].Text = url;
                else
                {
                    string formattedUrl = url.Insert(0, "http://");
                    item.SubItems[3].Text = formattedUrl;
                }
                item.SubItems[4].Text = notes;
            }

            FitColumnWidth();
            FillListViewItemColors();
        }
    }
}