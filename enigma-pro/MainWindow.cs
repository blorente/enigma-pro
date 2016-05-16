using System;
using System.IO;
using System.Windows.Forms;

namespace enigma_pro
{
    public partial class MainWindow : Form
    {
        private DialogManager mAboutDlg;
        private DialogManager mMasterKeyDlg;
        private DialogManager mListView;
        private readonly DialogManager mDialog;
        
        private static string _mSDatabaseFilePath;

        public MainWindow()
        {
            InitializeComponent();

            mDialog = new DialogManager();
            mDialog?.AddNewLabel(this, "Welcome!");
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            //XmlHandler.SaveConfigXml(this, "Config.xml");
        }

        private void ReEnableMenuItems()
        {
            DialogManager.SetMenuItemProperty(addEntryMenuItem, true);
            DialogManager.SetMenuItemProperty(editViewEntryMenuItem, true);
            DialogManager.SetMenuItemProperty(delEntryMenuItem, true);
            DialogManager.SetMenuItemProperty(duplicateEntryMenuItem, true);
            DialogManager.SetMenuItemProperty(cpUsernameMenuItem, true);
            DialogManager.SetMenuItemProperty(cpPasswordMenuItem, true);
            DialogManager.SetMenuItemProperty(openURLMenuItem, true);
        }

        private void MainWindow_SizeChanged(object sender, EventArgs e)
        {
            if (mListView != null)
                mListView.MColumnNotes.Width = -2;
        }

        private void newDBMenuItem_Click(object sender, EventArgs e)
        {
            mMasterKeyDlg = new DialogManager();
            mMasterKeyDlg?.InitializeSetKeyFile();

            if (!DialogManager.MKeySet) return;
            mListView = new DialogManager();
            DatabaseHandler.NewDatabase(mListView, this);
            mDialog.MLabel.Visible = false;
            ReEnableMenuItems();
            DialogManager.SetMenuItemProperty(newDBMenuItem, false);
        }

        private void quitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void addEntryMenuItem_Click(object sender, EventArgs e)
        {
            mListView?.InitializeAddNewEntry();
        }

        private void editViewEntryMenuItem_Click(object sender, EventArgs e)
        {
            mListView?.InitializeEditEntry();
        }

        private void delEntryMenuItem_Click(object sender, EventArgs e)
        {
            mListView?.DeleteSelectedEntry();
        }

        private void cpUsernameMenuItem_Click(object sender, EventArgs e)
        {
            mListView?.CopyUsernameToClipboard();
        }

        private void cpPasswordMenuItem_Click(object sender, EventArgs e)
        {
            mListView?.CopyPasswordToClipboard();
        }

        private void openURLMenuItem_Click(object sender, EventArgs e)
        {
            mListView?.OpenUrl();
        }

        private void aboutMenuItem_Click(object sender, EventArgs e)
        {
            mAboutDlg = new DialogManager();
            mAboutDlg?.InitializeAboutDialog();
        }

        private void entriesMenuItem_Select(object sender, EventArgs e)
        {
            if (mListView != null)
            {
                if (mListView.MLView.SelectedItems.Count == 1)
                {
                    DialogManager.SetMenuItemProperty(editViewEntryMenuItem, true);
                    DialogManager.SetMenuItemProperty(delEntryMenuItem, true);
                    DialogManager.SetMenuItemProperty(duplicateEntryMenuItem, true);
                    DialogManager.SetMenuItemProperty(cpUsernameMenuItem, true);
                    DialogManager.SetMenuItemProperty(cpPasswordMenuItem, true);
                    DialogManager.SetMenuItemProperty(openURLMenuItem, true);
                }
                else if (mListView.MLView.SelectedItems.Count > 1)
                {
                    DialogManager.SetMenuItemProperty(editViewEntryMenuItem, false);
                    DialogManager.SetMenuItemProperty(duplicateEntryMenuItem, false);
                    DialogManager.SetMenuItemProperty(cpUsernameMenuItem, false);
                    DialogManager.SetMenuItemProperty(cpPasswordMenuItem, false);
                    DialogManager.SetMenuItemProperty(openURLMenuItem, false);
                }
                else
                {
                    DialogManager.SetMenuItemProperty(editViewEntryMenuItem, false);
                    DialogManager.SetMenuItemProperty(delEntryMenuItem, false);
                    DialogManager.SetMenuItemProperty(duplicateEntryMenuItem, false);
                    DialogManager.SetMenuItemProperty(cpUsernameMenuItem, false);
                    DialogManager.SetMenuItemProperty(cpPasswordMenuItem, false);
                    DialogManager.SetMenuItemProperty(openURLMenuItem, false);
                }
            }
            else
            {
                DialogManager.SetMenuItemProperty(addEntryMenuItem, false);
                DialogManager.SetMenuItemProperty(editViewEntryMenuItem, false);
                DialogManager.SetMenuItemProperty(delEntryMenuItem, false);
                DialogManager.SetMenuItemProperty(duplicateEntryMenuItem, false);
                DialogManager.SetMenuItemProperty(cpUsernameMenuItem, false);
                DialogManager.SetMenuItemProperty(cpPasswordMenuItem, false);
                DialogManager.SetMenuItemProperty(openURLMenuItem, false);
            }
        }

        private void closeDBMenuItem_Click(object sender, EventArgs e)
        {
            if (mListView == null || !Controls.Contains(mListView.MLView)) return;

            DatabaseHandler.CloseDatabase(mListView.MLView, this);
            DialogManager.SetMenuItemProperty(newDBMenuItem, true);
            mDialog.MLabel.Visible = true;
            mListView = null;
            Text = "Enigma-Pro";
        }

        private void duplicateEntryMenuItem_Click(object sender, EventArgs e)
        {
            mListView?.DuplicateEntry();
        }

        private void saveDBMenuItem_Click(object sender, EventArgs e)
        {
            if (mListView == null || mListView.MLView.Items.Count <= 0) return;
            XmlHandler.ExportEncryptedToXml(mListView.MLView, _mSDatabaseFilePath);
        }

        private void openDBMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Enigma DB-File (*.edb)|*.edb",
                Title = "Open Database File"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // If Database exists -> close current db
                if (mListView != null && Controls.Contains(mListView.MLView))
                {
                    DatabaseHandler.CloseDatabase(mListView.MLView, this);
                    DialogManager.SetMenuItemProperty(newDBMenuItem, true);
                    mDialog.MLabel.Visible = true;
                }

                // Create new Database
                mListView = new DialogManager();
                DatabaseHandler.OpenDatabase(mListView, this, openFileDialog);

                mDialog.MLabel.Visible = false;
                ReEnableMenuItems();
                DialogManager.SetMenuItemProperty(newDBMenuItem, false);

                _mSDatabaseFilePath = Path.GetFullPath(openFileDialog.FileName);
                this.Text = $"{Path.GetFileName(openFileDialog.FileName)} - Enigma-Pro";
            }
        }

        private void dbMenuItem_Select(object sender, EventArgs e)
        {
            DialogManager.SetMenuItemProperty(closeDBMenuItem, mListView != null);
            DialogManager.SetMenuItemProperty(importDatabaseMenuItem, mListView != null);
            DialogManager.SetMenuItemProperty(exportDatabaseMenuItem, mListView != null);
            DialogManager.SetMenuItemProperty(saveDBMenuItem, mListView != null);
            DialogManager.SetMenuItemProperty(saveAsDBMenuItem, mListView != null);
        }

        private void changeMasterKeyMenuItem_Click(object sender, EventArgs e)
        {
            //TODO: NOT NEEDED!!!
        }

        private void saveAsDBMenuItem_Click(object sender, EventArgs e)
        {
            // If any item exists in listview
            if (mListView == null || mListView.MLView.Items.Count <= 0) return;
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Enigma DB-File (*.edb)|*.edb",
                Title = "Save Database File"
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
            XmlHandler.ExportEncryptedToXml(mListView.MLView, saveFileDialog);

            _mSDatabaseFilePath = Path.GetFullPath(saveFileDialog.FileName);
            this.Text = $"{Path.GetFileName(saveFileDialog.FileName)} - MainWindow";
        }

        private void importToXMLFileMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "XML File (*.*)|*.xml",
                Title = "Open File"
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            XmlHandler.ImportFromXml(mListView.MLView, openFileDialog);
        }

        private void exportToXMLFileMenuItem_Click(object sender, EventArgs e)
        {
            // If any item exists in listview
            if (mListView == null || mListView.MLView.Items.Count <= 0) return;
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "XML File (*.*)|*.xml",
                Title = "Save As"
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
            XmlHandler.ExportToXml(mListView.MLView, saveFileDialog);
        }
    }
}