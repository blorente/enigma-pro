﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                mDialog.AddNewLabel(this, new Point(329, 229), "Welcome!");
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

        private void newDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addNewEntryToolStripMenuItem.Enabled = true;
            deleteEntryToolStripMenuItem.Enabled = true;
            copyUsernameToolStripMenuItem.Enabled = true;
            copyPasswordToolStripMenuItem.Enabled = true;
            openURLToolStripMenuItem.Enabled = true;

            newDatabaseToolStripMenuItem.Enabled = false;
            mDialog.MLabel.Hide();

            mListView = new DialogManager();
            if (mListView != null)
            {
                mListView.InitializeListView(this);
                this.MaximizeBox = true;
                this.FormBorderStyle = FormBorderStyle.Sizable;
            }
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
                mListView.MColumnURL.Width = -2;
        }
    }
}
