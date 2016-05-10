using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace enigma_pro
{
    public class DialogManager
    {
        private const int mMinimumColumnWidth = 60;
        private static int mID;
        private static bool mIsPasswordShown = false;

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
        private ColumnHeader mColumnID;
        private ColumnHeader mColumnTitle;
        private ColumnHeader mColumnUsername;
        private ColumnHeader mColumnPassword;
        private ColumnHeader mColumnURL;
        private ColumnHeader mColumnNotes;

        public int MID
        {
            get { return mID; }
            set { mID = value; }
        }
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

        //------------------------------------------------------------------------------------
        public static bool CheckURLValid(string URLInput)
        {
            Uri uriResult;
            return Uri.TryCreate(URLInput, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
        public void FitColumnWidth()
        {
            this.mColumnID.Width = -2;
            this.mColumnTitle.Width = -2;
            this.mColumnUsername.Width = -2;
            this.mColumnPassword.Width = -2;
            this.mColumnURL.Width = -2;
            this.mColumnNotes.Width = -2;
        }
        public void FillListViewItemColors()
        {
            for (int i = 0; i < mLView.Items.Count; i++)
            {
                if (mLView.Items[i].Index % 2 == 0)
                    mLView.Items[i].BackColor = Color.White;
                else
                    mLView.Items[i].BackColor = Color.LightGray;
            }
        }
        public void CopyUsernameToClipboard()
        {
            ListView.SelectedListViewItemCollection selectedLVItem = mLView.SelectedItems;

            foreach (ListViewItem item in selectedLVItem)
            {
                if (!string.IsNullOrEmpty(item.SubItems[2].Text))
                    Clipboard.SetText(item.SubItems[2].Text);
            }
        }
        public void CopyPasswordToClipboard()
        {
            ListView.SelectedListViewItemCollection selectedLVItem = mLView.SelectedItems;

            foreach (ListViewItem item in selectedLVItem)
            {
                if (!string.IsNullOrEmpty(item.SubItems[3].Text))
                    Clipboard.SetText(item.SubItems[3].Text);
            }
        }
        public void OpenURL()
        {
            ListView.SelectedListViewItemCollection selectedLVItem = mLView.SelectedItems;

            foreach (ListViewItem item in selectedLVItem)
            {
                if (CheckURLValid(item.SubItems[4].Text))
                    System.Diagnostics.Process.Start(item.SubItems[4].Text);
            }
        }
        public void DeleteSelectedEntry()
        {
            ListView listview = mLView;
            foreach (ListViewItem item in listview.SelectedItems)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this entry?", "Delete?", MessageBoxButtons.OKCancel,
                                                                                                                          MessageBoxIcon.Question,
                                                                                                                          MessageBoxDefaultButton.Button1);
                if (dialogResult == DialogResult.OK)
                    item.Remove();
            }

            this.FillListViewItemColors();
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
                        Title = item.SubItems[1].Text;
                        Username = item.SubItems[2].Text;
                        Password = item.SubItems[3].Text;
                        URL = item.SubItems[4].Text;
                        Notes = item.SubItems[5].Text;
                    }
                }
            }

            ListViewItem LVItems = new ListViewItem(mID.ToString());
            mID++;
            LVItems.SubItems.Add(Title);
            LVItems.SubItems.Add(Username);
            LVItems.SubItems.Add(Password);
            LVItems.SubItems.Add(URL);
            LVItems.SubItems.Add(Notes);

            mLView.Items.Add(LVItems);

            this.FitColumnWidth();
            this.FillListViewItemColors();
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
            mAboutDlg.MaximizeBox = false;
            mAboutDlg.MinimizeBox = false;
            mAboutDlg.HelpButton = true;
            mAboutDlg.CancelButton = mCloseBtn;

            // Close Button
            mCloseBtn.Location = new Point(220, 200);
            mCloseBtn.Text = "Close";
            mCloseBtn.Click += new EventHandler(OnCloseBtnClicked);

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

            mColumnID = new ColumnHeader();
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
            mOpenURLToolStripMenuItem.Click += new EventHandler(OnOpenURLToolStripMenuItemClicked);

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
            mDuplicateEntryToolStripMenuItem.Text = "Duplicate Entry";
            mDuplicateEntryToolStripMenuItem.Click += new EventHandler(OnDuplicateEntryToolStripMenuItemClicked);

            mDeleteEntryToolStripMenuItem.Image = Properties.Resources.entry_delete;
            mDeleteEntryToolStripMenuItem.ShortcutKeys = (Keys.Control | Keys.D);
            mDeleteEntryToolStripMenuItem.Size = new Size(199, 22);
            mDeleteEntryToolStripMenuItem.Text = "Delete Entry";
            mDeleteEntryToolStripMenuItem.Click += new EventHandler(OnDeleteEntryToolStripMenuItemClicked);

            mContextMenu.Opening += new System.ComponentModel.CancelEventHandler(OnContextMenuOpening);
            mContextMenu.Size = new Size(200, 142);
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

            mColumnID.Text = "ID";
            mColumnID.Width = 40;
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
            mLView.Location = new Point(16, 15);
            mLView.Size = ListViewSize;
            mLView.Anchor = (((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right);
            mLView.Columns.AddRange(new ColumnHeader[]
            {
                mColumnID,
                mColumnTitle,
                mColumnUsername,
                mColumnPassword,
                mColumnURL,
                mColumnNotes
            });
            mLView.View = View.Details;
            mLView.FullRowSelect = true;
            mLView.MultiSelect = true;
            mLView.GridLines = true;
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
            mEntryDlg.Size = new Size(495, 350);
            mEntryDlg.Text = "Add Entry";
            mEntryDlg.StartPosition = FormStartPosition.CenterScreen;
            mEntryDlg.FormBorderStyle = FormBorderStyle.FixedDialog;
            mEntryDlg.MaximizeBox = false;
            mEntryDlg.MinimizeBox = false;
            mEntryDlg.AcceptButton = mConfirmEntryBtn;
            mEntryDlg.CancelButton = mCancelBtn;

            // Title Label
            mTitleLbl.Location = new Point(5, 20);
            mTitleLbl.Text = "Title:\t";

            // Username Label
            mUserNameLbl.Location = new Point(5, 50);
            mUserNameLbl.Text = "Username:\t";

            // Password Label
            mPasswordLbl.Location = new Point(5, 80);
            mPasswordLbl.Text = "Password:\t";

            // Password Repeat Label
            mPasswordRptLbl.Location = new Point(5, 110);
            mPasswordRptLbl.Text = "Repeat:\t";

            // URL Label
            mURLLbl.Location = new Point(5, 140);
            mURLLbl.Text = "URL:\t";

            // Notes Label
            mNotesLbl.Location = new Point(5, 170);
            mNotesLbl.Text = "Notes:\t";

            // Title TextBox
            mTitleTBox.Location = new Point(120, 20);
            mTitleTBox.Size = new Size(350, 20);

            // Username TextBox
            mUserNameTBox.Location = new Point(120, 50);
            mUserNameTBox.Size = new Size(350, 20);

            // Password TextBox
            mPasswordTBox.Location = new Point(120, 80);
            mPasswordTBox.Size = new Size(325, 20);
            mPasswordTBox.UseSystemPasswordChar = true;

            // Hide/Show Password
            mShowPasswordBtn.Location = new Point(450, 80);
            mShowPasswordBtn.Size = new Size(24, 24);
            mShowPasswordBtn.Image = Image.FromFile("share/icons/16x16/lock.png");
            mShowPasswordBtn.Click += new EventHandler(OnPasswordMaskClicked);

            // Password Repeat TextBox
            mPasswordRptTBox.Location = new Point(120, 110);
            mPasswordRptTBox.Size = new Size(325, 20);
            mPasswordRptTBox.UseSystemPasswordChar = true;

            // URL TextBox
            mURLTBox.Location = new Point(120, 140);
            mURLTBox.Size = new Size(350, 20);

            // Notes RichTextBox
            mNotesTBox.Location = new Point(120, 170);
            mNotesTBox.Size = new Size(350, 100);
            mNotesTBox.Multiline = true;
            mNotesTBox.AcceptsTab = true;
            mNotesTBox.WordWrap = true;

            // Add Entry Button
            mConfirmEntryBtn.Location = new Point(310, 280);
            mConfirmEntryBtn.Text = "OK";
            mConfirmEntryBtn.Click += new EventHandler(OnAddEntryBtnClicked);

            // Cancel Button
            mCancelBtn.Location = new Point(395, 280);
            mCancelBtn.Text = "Cancel";
            mCancelBtn.Click += new EventHandler(OnCancelEntryBtnClicked);

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
                mTitleTBox.Text = item.SubItems[1].Text;
                mUserNameTBox.Text = item.SubItems[2].Text;
                mPasswordTBox.Text = item.SubItems[3].Text;
                mPasswordRptTBox.Text = item.SubItems[3].Text;
                mURLTBox.Text = item.SubItems[4].Text;
                mNotesTBox.Text = item.SubItems[5].Text;
            }

            // Add Entry Dialog
            mEntryDlg.Size = new Size(495, 350);
            mEntryDlg.Text = "Edit Entry";
            mEntryDlg.StartPosition = FormStartPosition.CenterScreen;
            mEntryDlg.FormBorderStyle = FormBorderStyle.FixedDialog;
            mEntryDlg.MaximizeBox = false;
            mEntryDlg.MinimizeBox = false;
            mEntryDlg.AcceptButton = mConfirmEntryBtn;
            mEntryDlg.CancelButton = mCancelBtn;

            // Title Label
            mTitleLbl.Location = new Point(5, 20);
            mTitleLbl.Text = "Title:\t";

            // Username Label
            mUserNameLbl.Location = new Point(5, 50);
            mUserNameLbl.Text = "Username:\t";

            // Password Label
            mPasswordLbl.Location = new Point(5, 80);
            mPasswordLbl.Text = "Password:\t";

            // Password Repeat Label
            mPasswordRptLbl.Location = new Point(5, 110);
            mPasswordRptLbl.Text = "Repeat:\t";

            // URL Label
            mURLLbl.Location = new Point(5, 140);
            mURLLbl.Text = "URL:\t";

            // Notes Label
            mNotesLbl.Location = new Point(5, 170);
            mNotesLbl.Text = "Notes:\t";

            // Title TextBox
            mTitleTBox.Location = new Point(120, 20);
            mTitleTBox.Size = new Size(350, 20);

            // Username TextBox
            mUserNameTBox.Location = new Point(120, 50);
            mUserNameTBox.Size = new Size(350, 20);

            // Password TextBox
            mPasswordTBox.Location = new Point(120, 80);
            mPasswordTBox.Size = new Size(325, 20);
            mPasswordTBox.UseSystemPasswordChar = true;

            // Hide/Show Password
            mShowPasswordBtn.Location = new Point(450, 80);
            mShowPasswordBtn.Size = new Size(24, 24);
            mShowPasswordBtn.Image = Image.FromFile("share/icons/16x16/lock.png");
            mShowPasswordBtn.Click += new EventHandler(OnPasswordMaskClicked);

            // Password Repeat TextBox
            mPasswordRptTBox.Location = new Point(120, 110);
            mPasswordRptTBox.Size = new Size(325, 20);
            mPasswordRptTBox.UseSystemPasswordChar = true;

            // URL TextBox
            mURLTBox.Location = new Point(120, 140);
            mURLTBox.Size = new Size(350, 20);

            // Notes RichTextBox
            mNotesTBox.Location = new Point(120, 170);
            mNotesTBox.Size = new Size(350, 100);
            mNotesTBox.Multiline = true;
            mNotesTBox.AcceptsTab = true;
            mNotesTBox.WordWrap = true;

            // Edit Entry Button
            mConfirmEntryBtn.Location = new Point(310, 280);
            mConfirmEntryBtn.Text = "OK";
            mConfirmEntryBtn.Click += new EventHandler(OnEditEntryBtnClicked);

            // Cancel Button
            mCancelBtn.Location = new Point(395, 280);
            mCancelBtn.Text = "Cancel";
            mCancelBtn.Click += new EventHandler(OnCancelEntryBtnClicked);

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

            if (mLView.SelectedItems.Count > 0)
                mEntryDlg.ShowDialog();
        }
        //------------------------------------------------------------------------------------
        private void OnCopyUsernameToolStripMenuItemClicked(object sender, EventArgs e)
        {
            this.CopyUsernameToClipboard();
        }
        private void OnCopyPasswordToolStripMenuItemClicked(object sender, EventArgs e)
        {
            this.CopyPasswordToClipboard();
        }
        private void OnOpenURLToolStripMenuItemClicked(object sender, EventArgs e)
        {
            this.OpenURL();
        }
        private void OnAddEntryToolStripMenuItemClicked(object sender, EventArgs e)
        {
            this.InitializeAddNewEntry();
        }
        private void OnEditEntryToolStripMenuItemClicked(object sender, EventArgs e)
        {
            this.InitializeEditEntry();
        }
        private void OnDuplicateEntryToolStripMenuItemClicked(object sender, EventArgs e)
        {
            this.DuplicateEntry();
        }
        private void OnDeleteEntryToolStripMenuItemClicked(object sender, EventArgs e)
        {
            this.DeleteSelectedEntry();
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
            if (mLView.SelectedItems.Count > 0)
            {
                mCopyUsernameToolStripMenuItem.Enabled = true;
                mCopyPasswordToolStripMenuItem.Enabled = true;
                mEditViewEntryToolStripMenuItem.Enabled = true;
                mDuplicateEntryToolStripMenuItem.Enabled = true;
                mOpenURLToolStripMenuItem.Enabled = true;
                mDeleteEntryToolStripMenuItem.Enabled = true;
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
        private void OnCloseBtnClicked(object sender, EventArgs e)
        {
            mAboutDlg.Close();
        }
        private void OnCancelEntryBtnClicked(object sender, EventArgs e)
        {
            mEntryDlg.Close();
        }
        private void OnAddEntryBtnClicked(object sender, EventArgs e)
        {
            if (mPasswordTBox.Text == mPasswordRptTBox.Text)
            {
                AddNewEntry(mTitleTBox.Text, mUserNameTBox.Text, mPasswordTBox.Text, mURLTBox.Text, mNotesTBox.Text);
                mEntryDlg.Close();
            }
            else
                MessageBox.Show("Password and repeated password don't match!", this.mEntryDlg.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void OnEditEntryBtnClicked(object sender, EventArgs e)
        {
            if (mPasswordTBox.Text == mPasswordRptTBox.Text)
            {
                EditEntry(mTitleTBox.Text, mUserNameTBox.Text, mPasswordTBox.Text, mURLTBox.Text, mNotesTBox.Text);
                mEntryDlg.Close();
            }
            else
                MessageBox.Show("Password and repeated password don't match!", this.mEntryDlg.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        //------------------------------------------------------------------------------------
        private void AddNewEntry(string Title, string Username, string Password, string URL, string Notes)
        {
            ListViewItem LVItems = new ListViewItem(MID.ToString());
            mID++;

            LVItems.SubItems.Add(Title);
            LVItems.SubItems.Add(Username);
            LVItems.SubItems.Add(Password);
            LVItems.SubItems.Add(URL);
            LVItems.SubItems.Add(Notes);

            mLView.Items.Add(LVItems);

            this.FitColumnWidth();
            this.FillListViewItemColors();
        }
        private void EditEntry(string Title, string Username, string Password, string URL, string Notes)
        {
            ListView.SelectedListViewItemCollection selectedLVItem = mLView.SelectedItems;

            foreach (ListViewItem item in selectedLVItem)
            {
                item.SubItems[1].Text = Title;
                item.SubItems[2].Text = Username;
                item.SubItems[3].Text = Password;
                item.SubItems[4].Text = URL;
                item.SubItems[5].Text = Notes;
            }

            this.FitColumnWidth();
            this.FillListViewItemColors();
        }
    }
}