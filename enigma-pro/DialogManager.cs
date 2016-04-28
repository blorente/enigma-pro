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
        private Label mLabel;

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

        public Label MLabel
        {
            get { return mLabel; }
            set { mLabel = value; }
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
            mAboutDlg.MaximizeBox = false;
            mAboutDlg.MinimizeBox = false;
            mAboutDlg.HelpButton = true;
            mAboutDlg.FormBorderStyle = FormBorderStyle.FixedDialog;

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
            mAboutDlg.Controls.Add(mCloseBtn);
            mAboutDlg.Controls.Add(mInfoLbl);
            mAboutDlg.Controls.Add(mCaptionLbl);
            mAboutDlg.Controls.Add(mLinkLbl);

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
            MLView.Columns.AddRange(new ColumnHeader[] {
            mColumnID,
            mColumnTitle,
            mColumnUsername,
            MColumnURL});
            MLView.View = View.Details;
            MLView.FullRowSelect = true;
            MLView.MultiSelect = true;
            MLView.UseCompatibleStateImageBehavior = false;

            Window.Controls.Add(MLView);
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
    }
}
