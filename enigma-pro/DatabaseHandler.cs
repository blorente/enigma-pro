using System.Drawing;
using System.Windows.Forms;

namespace enigma_pro
{
    class DatabaseHandler
    {
        public static void NewDatabase(DialogManager dialogManager, Form Window)
        {
            // Create new ListView
            dialogManager.InitializeListView(Window, new Size(Window.Width - 48, Window.Height - 86));
            DialogManager.SetWindowTheme(dialogManager.MLView.Handle, "Explorer", null);
        }
        public static void OpenDatabase(DialogManager dialogManager, Form Window, OpenFileDialog openFileDialog)
        {
            dialogManager.InitializeGetKeyFile();
            dialogManager.InitializeListView(Window, new Size(Window.Width - 48, Window.Height - 86));
            DialogManager.SetWindowTheme(dialogManager.MLView.Handle, "Explorer", null);

            XmlHandler.ImportEncryptedFromXml(dialogManager.MLView, openFileDialog);
        }
        public static void CloseDatabase(ListView listView, Form Window)
        {
            DialogResult dialogResult = MessageBox.Show("The current Database will be closed. Are you sure you want to proceed?", "Close?", MessageBoxButtons.OKCancel,
                                                                                                               MessageBoxIcon.Question,
                                                                                                               MessageBoxDefaultButton.Button1);
            if (dialogResult != DialogResult.OK) return;
            Window.Controls.Remove(listView);
            listView.Dispose();
        }
    }
}