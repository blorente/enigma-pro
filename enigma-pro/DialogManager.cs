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
        private const int mMinimumColumnWidth = 60;
        private static bool mIsPasswordShown;
        private static bool _mKeySet;
        private static string _mKeyPath, _mKeyPathSave;

        // Add Label
        private Label mCustomLabel;

        // Add / Edit Entry
        private Form mEntryDlg;
        private Label mTitleLbl;
        private Label mUserNameLbl;
        private Label mPasswordLbl;
        private Label mPasswordRptLbl;
        private Label mURLLbl;
        private Label mNotesLbl;
        private TextBox mTitleTBox;
        private TextBox mUserNameTBox;
        private TextBox mPasswordTBox;
        private TextBox mPasswordRptTBox;
        private TextBox mURLTBox;
        private RichTextBox mNotesTBox;
        private Button mShowPasswordBtn;
        private Button mConfirmEntryBtn;
        private Button mCancelBtn;

        // Change Master-Key Dialog
        private Form mMasterKeyForm;
        private Label mKeyLabel;
        private ComboBox mKeyFileComboBox;
        private Button mKeyFileLocationBtn;
        private Button mSetKeyBtn;
        private Button mCancelKeyBtn;

        // About-Dialog Components
        private Form mAboutDlg;
        private Button mCloseBtn;
        private Label mInfoLbl;
        private Label mCaptionLbl;
        private LinkLabel mLinkLbl;

        // ListView Components
        private ListView mLView;
        private ContextMenuStrip mContextMenu;
        private ToolStripSeparator mToolStripSeparator;
        private ToolStripMenuItem mCopyUsernameToolStripMenuItem;
        private ToolStripMenuItem mCopyPasswordToolStripMenuItem;
        private ToolStripMenuItem mOpenURLToolStripMenuItem;
        private ToolStripMenuItem mAddEntryToolStripMenuItem;
        private ToolStripMenuItem mEditViewEntryToolStripMenuItem;
        private ToolStripMenuItem mDuplicateEntryToolStripMenuItem;
        private ToolStripMenuItem mDeleteEntryToolStripMenuItem;
        private ColumnHeader mColumnTitle;
        private ColumnHeader mColumnUsername;
        private ColumnHeader mColumnPassword;
        private ColumnHeader mColumnURL;
        private ColumnHeader mColumnNotes;

        public Label MLabel
        {
            get { return mCustomLabel; }
            set { mCustomLabel = value; }
        }
        public ListView MLView
        {
            get { return mLView; }
            set { mLView = value; }
        }
        public ColumnHeader MColumnNotes
        {
            get { return mColumnNotes; }
            set { mColumnNotes = value; }
        }

        public static bool MKeySet
        {
            get { return _mKeySet; }
            set { _mKeySet = value; }
        }

        //------------------------------------------------------------------------------------
        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);
        public static bool CheckURLValid(string URLInput)
        {
            Uri uriResult;
            return Uri.TryCreate(URLInput, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
        public static void SetMenuItemProperty(MenuItem menuItem, bool Toggle)
        {
            menuItem.Enabled = Toggle;
        }
        public static void AddNewPicture(Form Window, Size ImageSize, string ImagePath)
        {
            PictureBox pictureBox = new PictureBox();
            pictureBox.ImageLocation = ImagePath;
            pictureBox.Location = new Point(0, 0);
            pictureBox.Size = ImageSize;

            Window.Controls.Add(pictureBox);
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
            mColumnTitle.Width = -2;
            mColumnUsername.Width = -2;
            mColumnPassword.Width = -2;
            mColumnURL.Width = -2;
            mColumnNotes.Width = -2;
        }
        public void FillListViewItemColors()
        {
            foreach (ListViewItem item in mLView.Items)
                item.BackColor = item.Index % 2 == 0 ? Color.White : Color.FromArgb(246, 246, 246);
        }
        public void CopyUsernameToClipboard()
        {
            ListView.SelectedListViewItemCollection selectedLVItem = mLView.SelectedItems;

            foreach (ListViewItem item in selectedLVItem)
            {
                if (!string.IsNullOrEmpty(item.SubItems[1].Text))
                    Clipboard.SetText(item.SubItems[1].Text);
            }
        }
        public void CopyPasswordToClipboard()
        {
            ListView.SelectedListViewItemCollection selectedLVItem = mLView.SelectedItems;

            foreach (ListViewItem item in selectedLVItem)
            {
                if (!string.IsNullOrEmpty(item.SubItems[2].Text))
                    Clipboard.SetText(item.SubItems[2].Text);
            }
        }
        public void OpenUrl()
        {
            ListView.SelectedListViewItemCollection selectedLVItem = mLView.SelectedItems;

            foreach (ListViewItem item in selectedLVItem)
            {
                if (CheckURLValid(item.SubItems[3].Text))
                    System.Diagnostics.Process.Start(item.SubItems[3].Text);
            }
        }
        public void DeleteSelectedEntry()
        {
            ListView listview = mLView;
            foreach (ListViewItem item in listview.SelectedItems)
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
            string Title = string.Empty;
            string Username = string.Empty;
            string Password = string.Empty;
            string URL = string.Empty;
            string Notes = string.Empty;

            ListView.SelectedListViewItemCollection selectedLVItem = mLView.SelectedItems;

            foreach (ListViewItem item in selectedLVItem)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (!string.IsNullOrEmpty(item.SubItems[i].Text))
                    {
                        Title = item.SubItems[0].Text;
                        Username = item.SubItems[1].Text;
                        Password = item.SubItems[2].Text;
                        URL = item.SubItems[3].Text;
                        Notes = item.SubItems[4].Text;
                    }
                }
            }

            ListViewItem LVItems = new ListViewItem(Title);

            LVItems.SubItems.Add(Username);
            LVItems.SubItems.Add(Password);
            LVItems.SubItems.Add(URL);
            LVItems.SubItems.Add(Notes);

            mLView.Items.Add(LVItems);

            FitColumnWidth();
            FillListViewItemColors();
        }
        public void AddNewLabel(Form Window, string Caption)
        {
            mCustomLabel = new Label();

            mCustomLabel.TextAlign = ContentAlignment.MiddleCenter;
            mCustomLabel.Dock = DockStyle.Fill;
            mCustomLabel.Text = Caption;
            mCustomLabel.AutoSize = false;

            Window.Controls.Add(mCustomLabel);
        }
        public void InitializeAboutDialog()
        {
            // Initialize Components
            mAboutDlg = new Form();
            mCloseBtn = new Button();
            mInfoLbl = new Label();
            mCaptionLbl = new Label();
            mLinkLbl = new LinkLabel();

            // About Dialog
            mAboutDlg.Size = new Size(320, 270);
            mAboutDlg.Text = "About enigma-pro";
            mAboutDlg.StartPosition = FormStartPosition.CenterScreen;
            mAboutDlg.FormBorderStyle = FormBorderStyle.FixedDialog;
            mAboutDlg.ShowInTaskbar = false;
            mAboutDlg.MaximizeBox = false;
            mAboutDlg.MinimizeBox = false;
            mAboutDlg.HelpButton = true;
            mAboutDlg.CancelButton = mCloseBtn;

            // Close Button
            mCloseBtn.Location = new Point(220, 200);
            mCloseBtn.Text = "Close";

            // Info Label
            mInfoLbl.Location = new Point(5, 80);
            mInfoLbl.AutoSize = true;
            mInfoLbl.Text = "enigma-pro is distributed under the term of the GNU General\nPublic License (GPL) version 2 or (at your option) version\n3.";
            mInfoLbl.Text += "\n\nUsing:\n- Visual Studio 2013\n- C#";

            // Title Label
            mCaptionLbl.Location = new Point(60, 25);
            mCaptionLbl.AutoSize = true;
            mCaptionLbl.Font = new Font("Arial", 12, FontStyle.Bold);
            mCaptionLbl.Text = "Enigma-Pro 1.0.0";

            // Github Label
            mLinkLbl.Location = new Point(5, 60);
            mLinkLbl.AutoSize = true;
            mLinkLbl.Text = "https://github.com/3n16m4/enigma-pro";
            mLinkLbl.LinkClicked += new LinkLabelLinkClickedEventHandler(OnLinkLblClicked);

            // Add Controls to Dialog
            mAboutDlg.Controls.AddRange(new Control[]
            {
                mCloseBtn,
                mInfoLbl,
                mCaptionLbl,
                mLinkLbl
            });

            // Display initial Dialog
            mAboutDlg.ShowDialog();
        }
        public void InitializeListView(Form Window, Size ListViewSize)
        {
            // Initialize Components
            mLView = new ListView();
            mContextMenu = new ContextMenuStrip();

            mToolStripSeparator = new ToolStripSeparator();
            mCopyUsernameToolStripMenuItem = new ToolStripMenuItem();
            mCopyPasswordToolStripMenuItem = new ToolStripMenuItem();
            mOpenURLToolStripMenuItem = new ToolStripMenuItem();
            mAddEntryToolStripMenuItem = new ToolStripMenuItem();
            mEditViewEntryToolStripMenuItem = new ToolStripMenuItem();
            mDuplicateEntryToolStripMenuItem = new ToolStripMenuItem();
            mDeleteEntryToolStripMenuItem = new ToolStripMenuItem();

            mColumnTitle = new ColumnHeader();
            mColumnUsername = new ColumnHeader();
            mColumnPassword = new ColumnHeader();
            mColumnURL = new ColumnHeader();
            mColumnNotes = new ColumnHeader();

            mToolStripSeparator.Size = new Size(196, 6);

            mCopyUsernameToolStripMenuItem.Image = Properties.Resources.username_copy;
            mCopyUsernameToolStripMenuItem.ShortcutKeys = (Keys.Control | Keys.B);
            mCopyUsernameToolStripMenuItem.Size = new Size(199, 22);
            mCopyUsernameToolStripMenuItem.Text = "Copy Username";
            mCopyUsernameToolStripMenuItem.Click += new EventHandler(OnCopyUsernameToolStripMenuItemClicked);

            mCopyPasswordToolStripMenuItem.Image = Properties.Resources.password_copy;
            mCopyPasswordToolStripMenuItem.ShortcutKeys = (Keys.Control | Keys.C);
            mCopyPasswordToolStripMenuItem.Size = new Size(199, 22);
            mCopyPasswordToolStripMenuItem.Text = "Copy Password";
            mCopyPasswordToolStripMenuItem.Click += new EventHandler(OnCopyPasswordToolStripMenuItemClicked);

            mOpenURLToolStripMenuItem.Image = Properties.Resources.globe_africa;
            mOpenURLToolStripMenuItem.ShortcutKeys = (Keys.Control | Keys.U);
            mOpenURLToolStripMenuItem.Size = new Size(199, 22);
            mOpenURLToolStripMenuItem.Text = "Open URL";
            mOpenURLToolStripMenuItem.Click += new EventHandler(OnOpenUrlToolStripMenuItemClicked);

            mAddEntryToolStripMenuItem.Image = Properties.Resources.entry_new;
            mAddEntryToolStripMenuItem.ShortcutKeys = (Keys.Control | Keys.Y);
            mAddEntryToolStripMenuItem.Size = new Size(199, 22);
            mAddEntryToolStripMenuItem.Text = "Add Entry";
            mAddEntryToolStripMenuItem.Click += new EventHandler(OnAddEntryToolStripMenuItemClicked);

            mEditViewEntryToolStripMenuItem.Image = Properties.Resources.entry_edit;
            mEditViewEntryToolStripMenuItem.ShortcutKeys = (Keys.Control | Keys.E);
            mEditViewEntryToolStripMenuItem.Size = new Size(199, 22);
            mEditViewEntryToolStripMenuItem.Text = "Edit/View Entry";
            mEditViewEntryToolStripMenuItem.Click += new EventHandler(OnEditEntryToolStripMenuItemClicked);

            mDuplicateEntryToolStripMenuItem.Image = Properties.Resources.entry_clone;
            mDuplicateEntryToolStripMenuItem.ShortcutKeys = (Keys.Control | Keys.K);
            mDuplicateEntryToolStripMenuItem.Size = new Size(199, 22);
            mDuplicateEntryToolStripMenuItem.Text = "Clone Entry";
            mDuplicateEntryToolStripMenuItem.Click += new EventHandler(OnDuplicateEntryToolStripMenuItemClicked);

            mDeleteEntryToolStripMenuItem.Image = Properties.Resources.entry_delete;
            mDeleteEntryToolStripMenuItem.ShortcutKeys = (Keys.Control | Keys.D);
            mDeleteEntryToolStripMenuItem.Size = new Size(199, 22);
            mDeleteEntryToolStripMenuItem.Text = "Delete Entry";
            mDeleteEntryToolStripMenuItem.Click += new EventHandler(OnDeleteEntryToolStripMenuItemClicked);

            mContextMenu.Size = new Size(200, 142);
            mContextMenu.Opening += new CancelEventHandler(OnContextMenuOpening);
            mContextMenu.Items.AddRange(new ToolStripItem[]
            {
                mCopyUsernameToolStripMenuItem,
                mCopyPasswordToolStripMenuItem,
                mOpenURLToolStripMenuItem,
                mToolStripSeparator,
                mAddEntryToolStripMenuItem,
                mEditViewEntryToolStripMenuItem,
                mDuplicateEntryToolStripMenuItem,
                mDeleteEntryToolStripMenuItem
            });

            mColumnTitle.Text = "Title";
            mColumnTitle.Width = 40;
            mColumnUsername.Text = "Username";
            mColumnPassword.Text = "Password";
            mColumnURL.Text = "URL";
            mColumnURL.Width = 40;
            mColumnNotes.Text = "Notes";
            mColumnNotes.Width = -2;

            mLView.ContextMenuStrip = mContextMenu;
            mLView.ColumnWidthChanged += new ColumnWidthChangedEventHandler(OnListViewColumnWidthChanged);
            mLView.KeyDown += new KeyEventHandler(OnListViewKeyDown);
            mLView.Location = new Point(16, 15);
            mLView.Size = ListViewSize;
            mLView.Anchor = (((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right);
            mLView.Columns.AddRange(new ColumnHeader[]
            {
                mColumnTitle,
                mColumnUsername,
                mColumnPassword,
                mColumnURL,
                mColumnNotes
            });
            mLView.View = View.Details;
            mLView.FullRowSelect = true;
            mLView.MultiSelect = true;
            mLView.UseCompatibleStateImageBehavior = false;

            Window.Controls.Add(mLView);
        }
        public void InitializeAddNewEntry()
        {
            // Initialize Components
            mEntryDlg = new Form();

            mTitleLbl = new Label(); ;
            mUserNameLbl = new Label();
            mPasswordLbl = new Label();
            mPasswordRptLbl = new Label();
            mURLLbl = new Label();
            mNotesLbl = new Label();

            mTitleTBox = new TextBox();
            mUserNameTBox = new TextBox();
            mPasswordTBox = new TextBox();
            mPasswordRptTBox = new TextBox();
            mURLTBox = new TextBox();
            mNotesTBox = new RichTextBox();

            mShowPasswordBtn = new Button();
            mConfirmEntryBtn = new Button();
            mCancelBtn = new Button();

            // Add Entry Dialog
            mEntryDlg.Size = new Size(490, 390);
            mEntryDlg.Text = "Add Entry";
            mEntryDlg.StartPosition = FormStartPosition.CenterScreen;
            mEntryDlg.FormBorderStyle = FormBorderStyle.FixedDialog;
            mEntryDlg.ShowInTaskbar = false;
            mEntryDlg.MaximizeBox = false;
            mEntryDlg.MinimizeBox = false;
            mEntryDlg.AcceptButton = mConfirmEntryBtn;
            mEntryDlg.CancelButton = mCancelBtn;

            // Title Label
            mTitleLbl.Location = new Point(5, 60);
            mTitleLbl.Text = "Title:\t";

            // Username Label
            mUserNameLbl.Location = new Point(5, 90);
            mUserNameLbl.Text = "Username:\t";

            // Password Label
            mPasswordLbl.Location = new Point(5, 120);
            mPasswordLbl.Text = "Password:\t";

            // Password Repeat Label
            mPasswordRptLbl.Location = new Point(5, 150);
            mPasswordRptLbl.Text = "Repeat:\t";

            // URL Label
            mURLLbl.Location = new Point(5, 180);
            mURLLbl.Text = "URL:\t";

            // Notes Label
            mNotesLbl.Location = new Point(5, 210);
            mNotesLbl.Text = "Notes:\t";

            // Title TextBox
            mTitleTBox.Location = new Point(115, 60);
            mTitleTBox.Size = new Size(350, 20);

            // Username TextBox
            mUserNameTBox.Location = new Point(115, 90);
            mUserNameTBox.Size = new Size(350, 20);

            // Password TextBox
            mPasswordTBox.Location = new Point(115, 120);
            mPasswordTBox.Size = new Size(322, 20);
            mPasswordTBox.UseSystemPasswordChar = true;

            // Hide/Show Password
            mShowPasswordBtn.Location = new Point(441, 120);
            mShowPasswordBtn.Size = new Size(24, 24);
            mShowPasswordBtn.Image = Image.FromFile("share/icons/16x16/lock.png");
            mShowPasswordBtn.Click += new EventHandler(OnPasswordMaskClicked);

            // Password Repeat TextBox
            mPasswordRptTBox.Location = new Point(115, 150);
            mPasswordRptTBox.Size = new Size(322, 20);
            mPasswordRptTBox.UseSystemPasswordChar = true;

            // URL TextBox
            mURLTBox.Location = new Point(115, 180);
            mURLTBox.Size = new Size(350, 20);

            // Notes RichTextBox
            mNotesTBox.Location = new Point(115, 210);
            mNotesTBox.Size = new Size(350, 100);
            mNotesTBox.Multiline = true;
            mNotesTBox.AcceptsTab = true;
            mNotesTBox.WordWrap = true;

            // Add Entry Button
            mConfirmEntryBtn.Location = new Point(305, 320);
            mConfirmEntryBtn.Text = "OK";
            mConfirmEntryBtn.Click += new EventHandler(OnAddEntryBtnClicked);

            // Cancel Button
            mCancelBtn.Location = new Point(390, 320);
            mCancelBtn.Text = "Cancel";

            mEntryDlg.Controls.AddRange(new Control[]
            {
                mTitleLbl,
                mUserNameLbl,
                mPasswordLbl,
                mPasswordRptLbl,
                mURLLbl,
                mNotesLbl,
                mTitleTBox,
                mUserNameTBox,
                mPasswordTBox,
                mPasswordRptTBox,
                mURLTBox,
                mNotesTBox,
                mShowPasswordBtn,
                mConfirmEntryBtn,
                mCancelBtn
            });

            AddNewPicture(mEntryDlg, new Size(475, 50), "share/banners/add-entry-banner.png");
            mEntryDlg.ShowDialog();
        }
        public void InitializeEditEntry()
        {
            // Initialize Components
            mEntryDlg = new Form();

            mTitleLbl = new Label(); ;
            mUserNameLbl = new Label();
            mPasswordLbl = new Label();
            mPasswordRptLbl = new Label();
            mURLLbl = new Label();
            mNotesLbl = new Label();

            mTitleTBox = new TextBox();
            mUserNameTBox = new TextBox();
            mPasswordTBox = new TextBox();
            mPasswordRptTBox = new TextBox();
            mURLTBox = new TextBox();
            mNotesTBox = new RichTextBox();

            mShowPasswordBtn = new Button();
            mConfirmEntryBtn = new Button();
            mCancelBtn = new Button();

            ListView.SelectedListViewItemCollection selectedLVItem = mLView.SelectedItems;

            foreach (ListViewItem item in selectedLVItem)
            {
                mTitleTBox.Text = item.SubItems[0].Text;
                mUserNameTBox.Text = item.SubItems[1].Text;
                mPasswordTBox.Text = item.SubItems[2].Text;
                mPasswordRptTBox.Text = item.SubItems[2].Text;
                mURLTBox.Text = item.SubItems[3].Text;
                mNotesTBox.Text = item.SubItems[4].Text;
            }

            // Add Entry Dialog
            mEntryDlg.Size = new Size(490, 390);
            mEntryDlg.Text = "Edit Entry";
            mEntryDlg.StartPosition = FormStartPosition.CenterScreen;
            mEntryDlg.FormBorderStyle = FormBorderStyle.FixedDialog;
            mEntryDlg.ShowInTaskbar = false;
            mEntryDlg.MaximizeBox = false;
            mEntryDlg.MinimizeBox = false;
            mEntryDlg.AcceptButton = mConfirmEntryBtn;
            mEntryDlg.CancelButton = mCancelBtn;

            // Title Label
            mTitleLbl.Location = new Point(5, 60);
            mTitleLbl.Text = "Title:\t";

            // Username Label
            mUserNameLbl.Location = new Point(5, 90);
            mUserNameLbl.Text = "Username:\t";

            // Password Label
            mPasswordLbl.Location = new Point(5, 120);
            mPasswordLbl.Text = "Password:\t";

            // Password Repeat Label
            mPasswordRptLbl.Location = new Point(5, 150);
            mPasswordRptLbl.Text = "Repeat:\t";

            // URL Label
            mURLLbl.Location = new Point(5, 180);
            mURLLbl.Text = "URL:\t";

            // Notes Label
            mNotesLbl.Location = new Point(5, 210);
            mNotesLbl.Text = "Notes:\t";

            // Title TextBox
            mTitleTBox.Location = new Point(115, 60);
            mTitleTBox.Size = new Size(350, 20);

            // Username TextBox
            mUserNameTBox.Location = new Point(115, 90);
            mUserNameTBox.Size = new Size(350, 20);

            // Password TextBox
            mPasswordTBox.Location = new Point(115, 120);
            mPasswordTBox.Size = new Size(322, 20);
            mPasswordTBox.UseSystemPasswordChar = true;

            // Hide/Show Password
            mShowPasswordBtn.Location = new Point(441, 120);
            mShowPasswordBtn.Size = new Size(24, 24);
            mShowPasswordBtn.Image = Image.FromFile("share/icons/16x16/lock.png");
            mShowPasswordBtn.Click += new EventHandler(OnPasswordMaskClicked);

            // Password Repeat TextBox
            mPasswordRptTBox.Location = new Point(115, 150);
            mPasswordRptTBox.Size = new Size(322, 20);
            mPasswordRptTBox.UseSystemPasswordChar = true;

            // URL TextBox
            mURLTBox.Location = new Point(115, 180);
            mURLTBox.Size = new Size(350, 20);

            // Notes RichTextBox
            mNotesTBox.Location = new Point(115, 210);
            mNotesTBox.Size = new Size(350, 100);
            mNotesTBox.Multiline = true;
            mNotesTBox.AcceptsTab = true;
            mNotesTBox.WordWrap = true;

            // Edit Entry Button
            mConfirmEntryBtn.Location = new Point(305, 320);
            mConfirmEntryBtn.Text = "OK";
            mConfirmEntryBtn.Click += new EventHandler(OnEditEntryBtnClicked);

            // Cancel Button
            mCancelBtn.Location = new Point(390, 320);
            mCancelBtn.Text = "Cancel";

            mEntryDlg.Controls.AddRange(new Control[]
            {
                mTitleLbl,
                mUserNameLbl,
                mPasswordLbl,
                mPasswordRptLbl,
                mURLLbl,
                mNotesLbl,
                mTitleTBox,
                mUserNameTBox,
                mPasswordTBox,
                mPasswordRptTBox,
                mURLTBox,
                mNotesTBox,
                mShowPasswordBtn,
                mConfirmEntryBtn,
                mCancelBtn
            });

            AddNewPicture(mEntryDlg, new Size(475, 50), "share/banners/edit-entry-banner.png");
            if (mLView.SelectedItems.Count > 0)
                mEntryDlg.ShowDialog();
        }
        public void InitializeSetKeyFile()
        {
            mMasterKeyForm = new Form();

            mKeyLabel = new Label();
            mKeyFileComboBox = new ComboBox();
            mKeyFileLocationBtn = new Button();
            mSetKeyBtn = new Button();
            mCancelKeyBtn = new Button();

            // Change Master-Key Dialog
            mMasterKeyForm.Size = new Size(340, 190);
            mMasterKeyForm.Text = "Create new Key File";
            mMasterKeyForm.StartPosition = FormStartPosition.CenterScreen;
            mMasterKeyForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            mMasterKeyForm.ShowInTaskbar = false;
            mMasterKeyForm.MaximizeBox = false;
            mMasterKeyForm.MinimizeBox = false;
            mMasterKeyForm.AcceptButton = mSetKeyBtn;
            mMasterKeyForm.CancelButton = mCancelKeyBtn;

            // Key Label
            mKeyLabel.Location = new Point(5, 60);
            mKeyLabel.Font = new Font("Arial", mKeyLabel.Font.Size, FontStyle.Bold);
            mKeyLabel.Text = "Key File:";
            mKeyLabel.AutoSize = true;

            // Key File ComboBox
            mKeyFileComboBox.Location = new Point(7, 75);
            mKeyFileComboBox.Size = new Size(280, 20);
            mKeyFileComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            mKeyFileComboBox.Items.Add(@"F:\pwsafe.enigma"); // TODO: Get All drive directories
            mKeyFileComboBox.SelectedIndex = 0;

            // Key File Location Button
            mKeyFileLocationBtn.Location = new Point(290, 74);
            mKeyFileLocationBtn.Size = new Size(24, 24);
            mKeyFileLocationBtn.Image = Image.FromFile(@"share/icons/16x16/file_save_as.png");
            mKeyFileLocationBtn.Click += new EventHandler(OnSetKeyFileBtnClicked);

            // Set Key Button
            mSetKeyBtn.Location = new Point(160, 120);
            mSetKeyBtn.Text = "OK";
            mSetKeyBtn.Click += new EventHandler(OnSaveKeyFileBtnClicked);

            // Cancel Button
            mCancelKeyBtn.Location = new Point(240, 120);
            mCancelKeyBtn.Text = "Cancel";

            mMasterKeyForm.Controls.AddRange(new Control[]
            {
                mKeyLabel,
                mKeyFileComboBox,
                mKeyFileLocationBtn,
                mSetKeyBtn,
                mCancelKeyBtn
            });

            AddNewPicture(mMasterKeyForm, new Size(475, 50), @"share/banners/enter-key-banner.png");
            mMasterKeyForm.ShowDialog();
        }
        public void InitializeGetKeyFile()
        {
            mMasterKeyForm = new Form();

            mKeyLabel = new Label();
            mKeyFileComboBox = new ComboBox();
            mKeyFileLocationBtn = new Button();
            mSetKeyBtn = new Button();
            mCancelKeyBtn = new Button();

            // Change Master-Key Dialog
            mMasterKeyForm.Size = new Size(340, 190);
            mMasterKeyForm.Text = "Open Database";
            mMasterKeyForm.StartPosition = FormStartPosition.CenterScreen;
            mMasterKeyForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            mMasterKeyForm.ShowInTaskbar = false;
            mMasterKeyForm.MaximizeBox = false;
            mMasterKeyForm.MinimizeBox = false;
            mMasterKeyForm.AcceptButton = mSetKeyBtn;
            mMasterKeyForm.CancelButton = mCancelKeyBtn;

            // Key Label
            mKeyLabel.Location = new Point(5, 60);
            mKeyLabel.Font = new Font("Arial", mKeyLabel.Font.Size, FontStyle.Bold);
            mKeyLabel.Text = "Key File:";
            mKeyLabel.AutoSize = true;

            // Key File ComboBox
            mKeyFileComboBox.Location = new Point(7, 75);
            mKeyFileComboBox.Size = new Size(280, 20);
            mKeyFileComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            mKeyFileComboBox.Items.Add(@"F:\pwsafe.enigma"); // TODO: Get All drive directories
            mKeyFileComboBox.SelectedIndex = 0;

            // Key File Location Button
            mKeyFileLocationBtn.Location = new Point(290, 74);
            mKeyFileLocationBtn.Size = new Size(24, 24);
            mKeyFileLocationBtn.Image = Image.FromFile(@"share/icons/16x16/file_save_as.png");
            mKeyFileLocationBtn.Click += new EventHandler(OnGetKeyFileBtnClicked);

            // Set Key Button
            mSetKeyBtn.Location = new Point(160, 120);
            mSetKeyBtn.Text = "OK";
            mSetKeyBtn.Click += new EventHandler(OnLoadKeyFileBtnClicked);

            // Cancel Button
            mCancelKeyBtn.Location = new Point(240, 120);
            mCancelKeyBtn.Text = "Cancel";

            mMasterKeyForm.Controls.AddRange(new Control[]
            {
                mKeyLabel,
                mKeyFileComboBox,
                mKeyFileLocationBtn,
                mSetKeyBtn,
                mCancelKeyBtn
            });

            AddNewPicture(mMasterKeyForm, new Size(475, 50), @"share/banners/enter-key-banner.png");
            mMasterKeyForm.ShowDialog();
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
            if (mIsPasswordShown == false)
            {
                // Unlock - Show Password
                mIsPasswordShown = true;
                mShowPasswordBtn.Image = Image.FromFile("share/icons/16x16/lock_open.png");
                mPasswordTBox.UseSystemPasswordChar = false;
                mPasswordRptTBox.UseSystemPasswordChar = false;
            }
            else
            {
                // Lock - Hide Password
                mIsPasswordShown = false;
                mShowPasswordBtn.Image = Image.FromFile("share/icons/16x16/lock.png");
                mPasswordTBox.UseSystemPasswordChar = true;
                mPasswordRptTBox.UseSystemPasswordChar = true;
            }
        }
        private void OnContextMenuOpening(object sender, CancelEventArgs e)
        {
            if (mLView.SelectedItems.Count == 1)
            {
                mCopyUsernameToolStripMenuItem.Enabled = true;
                mCopyPasswordToolStripMenuItem.Enabled = true;
                mEditViewEntryToolStripMenuItem.Enabled = true;
                mDuplicateEntryToolStripMenuItem.Enabled = true;
                mOpenURLToolStripMenuItem.Enabled = true;
                mDeleteEntryToolStripMenuItem.Enabled = true;
            }
            else if (mLView.SelectedItems.Count > 1)
            {
                mCopyUsernameToolStripMenuItem.Enabled = false;
                mCopyPasswordToolStripMenuItem.Enabled = false;
                mEditViewEntryToolStripMenuItem.Enabled = false;
                mDuplicateEntryToolStripMenuItem.Enabled = false;
                mOpenURLToolStripMenuItem.Enabled = false;
            }
            else
            {
                mCopyUsernameToolStripMenuItem.Enabled = false;
                mCopyPasswordToolStripMenuItem.Enabled = false;
                mEditViewEntryToolStripMenuItem.Enabled = false;
                mDuplicateEntryToolStripMenuItem.Enabled = false;
                mOpenURLToolStripMenuItem.Enabled = false;
                mDeleteEntryToolStripMenuItem.Enabled = false;
            }
        }
        private void OnListViewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Control)
            {
                foreach (ListViewItem item in mLView.Items)
                    item.Selected = true;
            }
        }
        private void OnListViewColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if (mLView.Columns[e.ColumnIndex].Width < mMinimumColumnWidth)
                mLView.Columns[e.ColumnIndex].Width = mMinimumColumnWidth;
        }
        private void OnLinkLblClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Open Github URL
            mLinkLbl.LinkVisited = true;
            System.Diagnostics.Process.Start(mLinkLbl.Text);
        }
        private void OnSetKeyFileBtnClicked(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "Key Files|*.enigma",
                Title = "Save as..."
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                mKeyFileComboBox.Items[0] = saveFileDialog.FileName;
        }
        private void OnGetKeyFileBtnClicked(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Key File|*.enigma",
                Title = "Open..."
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                mKeyFileComboBox.Items[0] = openFileDialog.FileName;
        }
        private void OnSaveKeyFileBtnClicked(object sender, EventArgs e)
        {
            _mKeyPathSave = mKeyFileComboBox.Items[0].ToString();
            StreamWriter fsSecretKey = new StreamWriter(_mKeyPathSave);

            string sSecretKey = Cryptography.GenerateKey();
            // For additional security Pin the key
            // A new GCHandle that protects the object from garbage collection.
            // This GCHandle must be released with Free when it is no longer needed.
            GCHandle gch = GCHandle.Alloc(sSecretKey, GCHandleType.Pinned);
            fsSecretKey.WriteLine(sSecretKey);
            fsSecretKey.Close();
            // Free the key from memory
            Cryptography.ZeroMemory(gch.AddrOfPinnedObject(), sSecretKey.Length * 2);
            gch.Free();
            _mKeySet = true;
            mMasterKeyForm.Close();
        }
        private void OnLoadKeyFileBtnClicked(object sender, EventArgs e)
        {
            _mKeyPath = mKeyFileComboBox.Items[0].ToString();

            mMasterKeyForm.Close();
        }
        private void OnAddEntryBtnClicked(object sender, EventArgs e)
        {
            if (mPasswordTBox.Text == mPasswordRptTBox.Text)
            {
                AddNewEntry(mTitleTBox.Text, mUserNameTBox.Text, mPasswordTBox.Text, mURLTBox.Text, mNotesTBox.Text);
                mEntryDlg.Close();
            }
            else
                MessageBox.Show("Password and repeated password don't match!", mEntryDlg.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void OnEditEntryBtnClicked(object sender, EventArgs e)
        {
            if (mPasswordTBox.Text == mPasswordRptTBox.Text)
            {
                EditEntry(mTitleTBox.Text, mUserNameTBox.Text, mPasswordTBox.Text, mURLTBox.Text, mNotesTBox.Text);
                mEntryDlg.Close();
            }
            else
                MessageBox.Show("Password and repeated password don't match!", mEntryDlg.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        //------------------------------------------------------------------------------------
        private void AddNewEntry(string Title, string Username, string Password, string URL, string Notes)
        {
            ListViewItem LVItems = new ListViewItem(Title);

            LVItems.SubItems.Add(Username);
            LVItems.SubItems.Add(Password);
            if (URL.StartsWith("http://") || URL.StartsWith("https://"))
                LVItems.SubItems.Add(URL);
            else
            {
                string formattedUrl = URL.Insert(0, "http://");
                LVItems.SubItems.Add(formattedUrl);
            }
            LVItems.SubItems.Add(Notes);

            mLView.Items.Add(LVItems);

            FitColumnWidth();
            FillListViewItemColors();
        }
        private void EditEntry(string Title, string Username, string Password, string URL, string Notes)
        {
            ListView.SelectedListViewItemCollection selectedLVItem = mLView.SelectedItems;

            foreach (ListViewItem item in selectedLVItem)
            {
                item.SubItems[0].Text = Title;
                item.SubItems[1].Text = Username;
                item.SubItems[2].Text = Password;
                if (URL.StartsWith("http://") || URL.StartsWith("https://"))
                    item.SubItems[3].Text = URL;
                else
                {
                    string FormattedURL = URL.Insert(0, "http://");
                    item.SubItems[3].Text = FormattedURL;
                }
                item.SubItems[4].Text = Notes;
            }

            FitColumnWidth();
            FillListViewItemColors();
        }
    }
}