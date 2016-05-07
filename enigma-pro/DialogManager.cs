using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace enigma_pro
{
    public class DialogManager
    {
        private const int mMinimumColumnWidth = 60;
        private static int mID;

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
        private Button mAddEntryBtn;
        private Button mCancelBtn;

        // About-Dialog Components
        private Form mAboutDlg;
        private Button mCloseBtn;
        private Label mInfoLbl;
        private Label mCaptionLbl;
        private LinkLabel mLinkLbl;

        // ListView Components
        private ListView mLView;
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
            get {   return mColumnNotes; } 
            set  {  mColumnNotes = value;  }
        }

        public static bool CheckURLValid(string URLInput)
        {
            Uri uriResult;
            return Uri.TryCreate(URLInput, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
        public void CopyUsernameToClipboard()
        {
            ListView.SelectedListViewItemCollection selectedLVItem = mLView.SelectedItems;

            foreach (ListViewItem item in selectedLVItem)
                Clipboard.SetText(item.SubItems[2].Text);
        }
        public void CopyPasswordToClipboard()
        {
            ListView.SelectedListViewItemCollection selectedLVItem = mLView.SelectedItems;

            foreach (ListViewItem item in selectedLVItem)
                Clipboard.SetText(item.SubItems[3].Text);
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
        public void AddNewLabel(Form Window, Point Location, string Caption)
        {
            MLabel = new Label();

            MLabel.Location = Location;
            MLabel.AutoSize = true;
            MLabel.Text = Caption;

            Window.Controls.Add(MLabel);
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
        public void InitializeListView(Form Window)
        {
            // Initialize Components
            MLView = new ListView();
            mColumnID = new ColumnHeader();
            mColumnTitle = new ColumnHeader();
            mColumnUsername = new ColumnHeader();
            mColumnPassword = new ColumnHeader();
            mColumnURL = new ColumnHeader();
            MColumnNotes = new ColumnHeader();

            MLView.ColumnWidthChanged += new ColumnWidthChangedEventHandler(OnColumnWidthChanged);

            mColumnID.Text = "ID";
            mColumnID.Width = 40;
            mColumnTitle.Text = "Title";
            mColumnTitle.Width = 40;
            mColumnUsername.Text = "Username";
            mColumnPassword.Text = "Password";
            mColumnURL.Text = "URL";
            mColumnURL.Width = 40;
            MColumnNotes.Text = "Notes";
            MColumnNotes.Width = -2;

            MLView.Location = new Point(16, 15);
            MLView.Size = new Size(695, 475);
            MLView.Anchor = (((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right);
            MLView.Columns.AddRange(new ColumnHeader[]
            {
                mColumnID,
                mColumnTitle,
                mColumnUsername,
                mColumnPassword,
                mColumnURL,
                MColumnNotes
            });
            MLView.View = View.Details;
            MLView.FullRowSelect = true;
            MLView.MultiSelect = true;
            MLView.UseCompatibleStateImageBehavior = false;

            Window.Controls.Add(MLView);
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

            mAddEntryBtn = new Button();
            mCancelBtn = new Button();

            // Add Entry Dialog
            mEntryDlg.Size = new Size(495, 350);
            mEntryDlg.Text = "Add Entry";
            mEntryDlg.StartPosition = FormStartPosition.CenterScreen;
            mEntryDlg.FormBorderStyle = FormBorderStyle.FixedDialog;
            mEntryDlg.MaximizeBox = false;
            mEntryDlg.MinimizeBox = false;
            mEntryDlg.AcceptButton = mAddEntryBtn;
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
            mPasswordTBox.Size = new Size(350, 20);
            mPasswordTBox.UseSystemPasswordChar = true;

            // Password Repeat TextBox
            mPasswordRptTBox.Location = new Point(120, 110);
            mPasswordRptTBox.Size = new Size(350, 20);
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
            mAddEntryBtn.Location = new Point(310, 280);
            mAddEntryBtn.Text = "Add";
            mAddEntryBtn.Click += new EventHandler(OnAddEntryBtnClicked);

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
                mAddEntryBtn,
                mCancelBtn
            });

            mEntryDlg.ShowDialog();
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
        private void OnColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
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
        private void AddNewEntry(string Title, string Username, string Password, string URL, string Notes)
        {
            ListViewItem LVItems = new ListViewItem(MID.ToString());
            this.MID++;

            LVItems.SubItems.Add(Title);
            LVItems.SubItems.Add(Username);
            LVItems.SubItems.Add(Password);
            LVItems.SubItems.Add(URL);
            LVItems.SubItems.Add(Notes);

            MLView.Items.Add(LVItems);

            this.mColumnID.Width = -2;
            this.mColumnTitle.Width = -2;
            this.mColumnUsername.Width = -2;
            this.mColumnPassword.Width = -2;
            this.mColumnURL.Width = -2;
            this.mColumnNotes.Width = -2;
            this.FillListViewItemColors();
        }
    }
}
