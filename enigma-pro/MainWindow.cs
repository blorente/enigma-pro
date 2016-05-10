using System;
using System.Drawing;
using System.Windows.Forms;

namespace enigma_pro
{
    public partial class MainWindow : Form
    {
        private DialogManager mAboutDlg = null;
        private DialogManager mListView = null;
        private DialogManager mDialog = null;

        public MainWindow()
        {
            InitializeComponent();

            mDialog = new DialogManager();
            if (mDialog != null)
                mDialog.AddNewLabel(this, "Welcome!");
        }

        private void SetMenuItemProperty(MenuItem menuItem, bool Toggle)
        {
            menuItem.Enabled = Toggle;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mAboutDlg = new DialogManager();
            if (mAboutDlg != null)
                mAboutDlg.InitializeAboutDialog();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void deleteEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mListView != null)
            {
                foreach (ListViewItem item in mListView.MLView.SelectedItems)
                {
                    if (item.Selected)
                        mListView.MLView.Items.Remove(item);
                }

                mListView.FillListViewItemColors();
            }
        }

        private void addNewEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mListView != null)
                mListView.InitializeAddNewEntry();
        }

        private void MainWindow_SizeChanged(object sender, EventArgs e)
        {
            if (mListView != null)
                mListView.MColumnNotes.Width = -2;
        }

        private void copyUsernameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mListView != null)
                mListView.CopyUsernameToClipboard();
        }

        private void openURLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mListView != null)
                mListView.OpenURL();
        }

        private void copyPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mListView != null)
                mListView.CopyPasswordToClipboard();
        }

        private void newDBMenuItem_Click(object sender, EventArgs e)
        {
            addEntryMenuItem.Enabled = true;
            editViewEntryMenuItem.Enabled = true;
            delEntryMenuItem.Enabled = true;
            duplicateEntryMenuItem.Enabled = true;
            cpUsernameMenuItem.Enabled = true;
            cpPasswordMenuItem.Enabled = true;
            openURLMenuItem.Enabled = true;

            newDBMenuItem.Enabled = false;
            mDialog.MLabel.Visible = false;

            mListView = new DialogManager();
            if (mListView != null)
            {
                mListView.InitializeListView(this, new Size(this.Width - 48, this.Height - 86));
                DialogManager.SetWindowTheme(mListView.MLView.Handle, "Explorer", null);
            }
        }

        private void quitMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addEntryMenuItem_Click(object sender, EventArgs e)
        {
            if (mListView != null)
                mListView.InitializeAddNewEntry();
        }

        private void editViewEntryMenuItem_Click(object sender, EventArgs e)
        {
            if (mListView != null)
                mListView.InitializeEditEntry();
        }

        private void delEntryMenuItem_Click(object sender, EventArgs e)
        {
            if (mListView != null)
                mListView.DeleteSelectedEntry();
        }

        private void cpUsernameMenuItem_Click(object sender, EventArgs e)
        {
            if (mListView != null)
                mListView.CopyUsernameToClipboard();
        }

        private void cpPasswordMenuItem_Click(object sender, EventArgs e)
        {
            if (mListView != null)
                mListView.CopyPasswordToClipboard();
        }

        private void openURLMenuItem_Click(object sender, EventArgs e)
        {
            if (mListView != null)
                mListView.OpenURL();
        }

        private void aboutMenuItem_Click(object sender, EventArgs e)
        {
            mAboutDlg = new DialogManager();
            if (mAboutDlg != null)
                mAboutDlg.InitializeAboutDialog();
        }

        private void entriesMenuItem_Select(object sender, EventArgs e)
        {
            if (mListView != null)
            {
                if (mListView.MLView.SelectedItems.Count == 1)
                {
                    SetMenuItemProperty(editViewEntryMenuItem, true);
                    SetMenuItemProperty(delEntryMenuItem, true);
                    SetMenuItemProperty(duplicateEntryMenuItem, true);
                    SetMenuItemProperty(cpUsernameMenuItem, true);
                    SetMenuItemProperty(cpPasswordMenuItem, true);
                    SetMenuItemProperty(openURLMenuItem, true);
                }
                else if (mListView.MLView.SelectedItems.Count > 1)
                {
                    SetMenuItemProperty(editViewEntryMenuItem, false);
                    SetMenuItemProperty(duplicateEntryMenuItem, false);
                    SetMenuItemProperty(cpUsernameMenuItem, false);
                    SetMenuItemProperty(cpPasswordMenuItem, false);
                    SetMenuItemProperty(openURLMenuItem, false);
                }
                else
                {
                    SetMenuItemProperty(editViewEntryMenuItem, false);
                    SetMenuItemProperty(delEntryMenuItem, false);
                    SetMenuItemProperty(duplicateEntryMenuItem, false);
                    SetMenuItemProperty(cpUsernameMenuItem, false);
                    SetMenuItemProperty(cpPasswordMenuItem, false);
                    SetMenuItemProperty(openURLMenuItem, false);
                }
            }
            else
            {
                SetMenuItemProperty(addEntryMenuItem, false);
                SetMenuItemProperty(editViewEntryMenuItem, false);
                SetMenuItemProperty(delEntryMenuItem, false);
                SetMenuItemProperty(duplicateEntryMenuItem, false);
                SetMenuItemProperty(cpUsernameMenuItem, false);
                SetMenuItemProperty(cpPasswordMenuItem, false);
                SetMenuItemProperty(openURLMenuItem, false);
            }
        }

        private void closeDBMenuItem_Click(object sender, EventArgs e)
        {
            if (mListView != null && this.Controls.Contains(mListView.MLView))
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to close this Database?", "Close?", MessageBoxButtons.OKCancel,
                                                                                                                   MessageBoxIcon.Question,
                                                                                                                   MessageBoxDefaultButton.Button1);
                if (dialogResult == DialogResult.OK)
                {
                    this.Controls.Remove(mListView.MLView);
                    mListView.MLView.Dispose();
                    mListView = null;

                    SetMenuItemProperty(newDBMenuItem, true);
                    mDialog.MLabel.Visible = true;
                }
            }
        }

        private void duplicateEntryMenuItem_Click(object sender, EventArgs e)
        {
            if (mListView != null)
                mListView.DuplicateEntry();
        }
    }
}
