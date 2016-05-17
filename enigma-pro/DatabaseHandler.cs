using System.Drawing;
using System.Windows.Forms;

namespace enigma_pro
{
    internal class DatabaseHandler
    {
        public static void NewDatabase(DialogManager dialogManager, Form window)
        {
            dialogManager.InitializeListView(window, new Size(window.Width - 48, window.Height - 86));
            DialogManager.SetWindowTheme(dialogManager.MLView.Handle, "Explorer", null);
        }
        public static void OpenDatabase(DialogManager dialogManager, Form window, OpenFileDialog openFileDialog)
        {
            dialogManager.InitializeListView(window, new Size(window.Width - 48, window.Height - 86));
            DialogManager.SetWindowTheme(dialogManager.MLView.Handle, "Explorer", null);

            XmlHandler.ImportEncryptedFromXml(dialogManager.MLView, openFileDialog);
        }
        public static void CloseDatabase(ListView listView, Form window)
        {
            DialogResult dialogResult = MessageBox.Show("The current Database will be closed. Are you sure you want to proceed?", "Close?", MessageBoxButtons.OKCancel,
                                                                                                               MessageBoxIcon.Question,
                                                                                                               MessageBoxDefaultButton.Button1);
            if (dialogResult != DialogResult.OK) return;
            window.Controls.Remove(listView);
            listView.Dispose();
        }
    }
}