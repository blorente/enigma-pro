using System;
using System.IO;
using System.Windows.Forms;

namespace enigma_pro
{
    public partial class MainWindow : Form
    {
        private DialogManager _mAboutDlg;
        private DialogManager _mMasterKeyDlg;
        private DialogManager _mListView;
        private readonly DialogManager _mDialog;
        
        private static string _mSDatabaseFilePath;

        public MainWindow()
        {
            InitializeComponent();

            _mDialog = new DialogManager();
            _mDialog?.AddNewLabel(this, "Welcome!");
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            XmlHandler.LoadConfigXml(this, "config.xml");
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            XmlHandler.SaveConfigXml(this, "config.xml");
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
            if (_mListView == null) return;
            _mListView.MColumnNotes.Width = -2;
        }

        private void newDBMenuItem_Click(object sender, EventArgs e)
        {
            _mMasterKeyDlg = new DialogManager();
            _mMasterKeyDlg?.InitializeSetKeyFile();

            if (!DialogManager.MKeySet) return;
            _mListView = new DialogManager();
            DatabaseHandler.NewDatabase(_mListView, this);

            _mDialog.MLabel.Visible = false;
            ReEnableMenuItems();
            DialogManager.SetMenuItemProperty(newDBMenuItem, false);
        }

        private void quitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void addEntryMenuItem_Click(object sender, EventArgs e)
        {
            _mListView?.InitializeAddNewEntry();
        }

        private void editViewEntryMenuItem_Click(object sender, EventArgs e)
        {
            _mListView?.InitializeEditEntry();
        }

        private void delEntryMenuItem_Click(object sender, EventArgs e)
        {
            _mListView?.DeleteSelectedEntry();
        }

        private void cpUsernameMenuItem_Click(object sender, EventArgs e)
        {
            _mListView?.CopyUsernameToClipboard();
        }

        private void cpPasswordMenuItem_Click(object sender, EventArgs e)
        {
            _mListView?.CopyPasswordToClipboard();
        }

        private void openURLMenuItem_Click(object sender, EventArgs e)
        {
            _mListView?.OpenUrl();
        }

        private void aboutMenuItem_Click(object sender, EventArgs e)
        {
            _mAboutDlg = new DialogManager();
            _mAboutDlg?.InitializeAboutDialog();
        }

        private void entriesMenuItem_Select(object sender, EventArgs e)
        {
            if (_mListView != null)
            {
                if (_mListView.MLView.SelectedItems.Count == 1)
                {
                    DialogManager.SetMenuItemProperty(editViewEntryMenuItem, true);
                    DialogManager.SetMenuItemProperty(delEntryMenuItem, true);
                    DialogManager.SetMenuItemProperty(duplicateEntryMenuItem, true);
                    DialogManager.SetMenuItemProperty(cpUsernameMenuItem, true);
                    DialogManager.SetMenuItemProperty(cpPasswordMenuItem, true);
                    DialogManager.SetMenuItemProperty(openURLMenuItem, true);
                }
                else if (_mListView.MLView.SelectedItems.Count > 1)
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
            if (_mListView == null || !Controls.Contains(_mListView.MLView)) return;

            DatabaseHandler.CloseDatabase(_mListView.MLView, this);
            DialogManager.SetMenuItemProperty(newDBMenuItem, true);
            _mDialog.MLabel.Visible = true;
            _mListView = null;
            Text = "Enigma-Pro";
            _mSDatabaseFilePath = "";
        }

        private void duplicateEntryMenuItem_Click(object sender, EventArgs e)
        {
            _mListView?.DuplicateEntry();
        }

        private void saveDBMenuItem_Click(object sender, EventArgs e)
        {
            if (_mListView == null) return;
            if (string.IsNullOrEmpty(_mSDatabaseFilePath))
                MessageBox.Show("Please save the Database to a location first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            XmlHandler.ExportEncryptedToXml(_mListView.MLView, _mSDatabaseFilePath);
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
                if (_mListView != null && Controls.Contains(_mListView.MLView))
                {
                    DatabaseHandler.CloseDatabase(_mListView.MLView, this);
                    DialogManager.SetMenuItemProperty(newDBMenuItem, true);
                    _mDialog.MLabel.Visible = true;
                }
                
                _mMasterKeyDlg = new DialogManager();
                _mMasterKeyDlg?.InitializeGetKeyFile();

                if (!DialogManager.MKeyGet) return;
                _mListView = new DialogManager();
                DatabaseHandler.OpenDatabase(_mListView, this, openFileDialog);

                _mDialog.MLabel.Visible = false;
                ReEnableMenuItems();
                DialogManager.SetMenuItemProperty(newDBMenuItem, false);

                _mSDatabaseFilePath = Path.GetFullPath(openFileDialog.FileName);
                this.Text = $"{Path.GetFileName(openFileDialog.FileName)} - Enigma-Pro";
            }
        }

        private void dbMenuItem_Select(object sender, EventArgs e)
        {
            DialogManager.SetMenuItemProperty(closeDBMenuItem, _mListView != null);
            DialogManager.SetMenuItemProperty(importDatabaseMenuItem, _mListView != null);
            DialogManager.SetMenuItemProperty(exportDatabaseMenuItem, _mListView != null);
            DialogManager.SetMenuItemProperty(saveDBMenuItem, _mListView != null);
            DialogManager.SetMenuItemProperty(saveAsDBMenuItem, _mListView != null);
        }

        private void changeMasterKeyMenuItem_Click(object sender, EventArgs e)
        {
            //TODO: NOT NEEDED!!!
        }

        private void saveAsDBMenuItem_Click(object sender, EventArgs e)
        {
            // If any item exists in listview
            if (_mListView == null) return;
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Enigma DB-File (*.edb)|*.edb",
                Title = "Save Database File"
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
            XmlHandler.ExportEncryptedToXml(_mListView.MLView, saveFileDialog);

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
            XmlHandler.ImportFromXml(_mListView.MLView, openFileDialog);
        }

        private void exportToXMLFileMenuItem_Click(object sender, EventArgs e)
        {
            // If any item exists in listview
            if (_mListView == null || _mListView.MLView.Items.Count <= 0) return;
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "XML File (*.*)|*.xml",
                Title = "Save As"
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
            XmlHandler.ExportToXml(_mListView.MLView, saveFileDialog);
        }
    }
}