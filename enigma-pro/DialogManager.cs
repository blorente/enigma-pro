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
        private TextBox mTitleTBox;
        private TextBox mUserNameTBox;
        private TextBox mPasswordTBox;
        private TextBox mPasswordRptTBox;
        private TextBox mURLTBox;
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
        private ColumnHeader mColumnURL;

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
        public ColumnHeader MColumnURL
        {
            get { return mColumnURL; }
            set { mColumnURL = value; }
        }

        private void AddNewEntry(string Title, string Username, string URL)
        {
            ListViewItem LVItems = new ListViewItem(MID.ToString());
            this.MID++;

            LVItems.SubItems.Add(Title);
            LVItems.SubItems.Add(Username);
            LVItems.SubItems.Add(URL);

            MLView.Items.Add(LVItems);
            this.mColumnID.Width = -2;
            this.mColumnTitle.Width = -2;
            this.mColumnUsername.Width = -2;
            this.MColumnURL.Width = -2;
            this.FillListViewItemColors();
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
            MColumnURL = new ColumnHeader();

            MLView.ColumnWidthChanged += new ColumnWidthChangedEventHandler(OnColumnWidthChanged);

            mColumnID.Text = "ID";
            mColumnID.Width = 40;
            mColumnTitle.Text = "Title";
            mColumnTitle.Width = 40;
            mColumnUsername.Text = "Username";
            MColumnURL.Text = "URL";
            MColumnURL.Width = -2;

            MLView.Location = new Point(16, 30);
            MLView.Size = new Size(695, 460);
            MLView.Anchor = (((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right);
            MLView.Columns.AddRange(new ColumnHeader[]
            {
                mColumnID,
                mColumnTitle,
                mColumnUsername,
                MColumnURL
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

            mTitleTBox = new TextBox();
            mUserNameTBox = new TextBox();
            mPasswordTBox = new TextBox();
            mPasswordRptTBox = new TextBox();
            mURLTBox = new TextBox();

            mAddEntryBtn = new Button();
            mCancelBtn = new Button();

            // Add Entry Dialog
            mEntryDlg.Size = new Size(500, 240);
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

            // Add Entry Button
            mAddEntryBtn.Location = new Point(310, 170);
            mAddEntryBtn.Text = "Add";
            mAddEntryBtn.Click += new EventHandler(OnAddEntryBtnClicked);

            // Cancel Button
            mCancelBtn.Location = new Point(395, 170);
            mCancelBtn.Text = "Cancel";
            mCancelBtn.Click += new EventHandler(OnCancelEntryBtnClicked);

            mEntryDlg.Controls.AddRange(new Control[]
            {
                mTitleLbl,
                mUserNameLbl,
                mPasswordLbl,
                mPasswordRptLbl,
                mURLLbl,
                mTitleTBox,
                mUserNameTBox,
                mPasswordTBox,
                mPasswordRptTBox,
                mURLTBox,
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
            AddNewEntry(mTitleTBox.Text, mUserNameTBox.Text, mURLTBox.Text);
            mEntryDlg.Close();
        }
    }
}
