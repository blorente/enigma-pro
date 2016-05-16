using Enigma.Cryptography;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace enigma_pro
{
    internal class XmlHandler
    {
        private static string _mSRecentKeyFilePath;

        /// <summary>
        /// Export ListView entries into raw XML-Files
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="saveFileDialog"></param>
        public static void ExportToXml(ListView listView, SaveFileDialog saveFileDialog)
        {
            XmlWriterSettings xmlSettings = new XmlWriterSettings
            {
                Indent = true,
                NewLineOnAttributes = true,
                CloseOutput = true
            };

            XmlWriter xmlWriter = XmlWriter.Create(saveFileDialog.FileName, xmlSettings);
            xmlWriter.WriteStartElement("pwlist");

            for (int index = 0; index < listView.Items.Count; index++)
            {
                xmlWriter.WriteStartElement("pwentry");

                xmlWriter.WriteStartElement("title");
                xmlWriter.WriteString(listView.Items[index].SubItems[0].Text);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("username");
                xmlWriter.WriteString(listView.Items[index].SubItems[1].Text);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("password");
                xmlWriter.WriteString(listView.Items[index].SubItems[2].Text);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("url");
                xmlWriter.WriteString(listView.Items[index].SubItems[3].Text);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("notes");
                xmlWriter.WriteString(listView.Items[index].SubItems[4].Text);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.Close();
        }
        /// <summary>
        /// Import ListView entries from raw XML-Files
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="openFileDialog"></param>
        public static void ImportFromXml(ListView listView, OpenFileDialog openFileDialog)
        {
            XDocument xDoc = XDocument.Load(openFileDialog.FileName);

            foreach (var item in xDoc.Descendants("pwentry"))
            {
                ListViewItem lvItem = new ListViewItem(new string[]
                {
                    item.Element("title")?.Value,
                    item.Element("username")?.Value,
                    item.Element("password")?.Value,
                    item.Element("url")?.Value,
                    item.Element("notes")?.Value
                });
                listView.Items.Add(lvItem);
            }
        }
        /// <summary>
        /// Export ListView entries into encrypted XML-Files for saveDialog
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="saveFileDialog"></param>
        public static void ExportEncryptedToXml(ListView listView, SaveFileDialog saveFileDialog)
        {
            string sEncryptionKey = null;

            XmlWriterSettings xmlSettings = new XmlWriterSettings
            {
                Indent = true,
                NewLineOnAttributes = true,
                CloseOutput = true
            };

            if (File.Exists(DialogManager.GetKeyFilePath()))
            {
                StreamReader reader = new StreamReader(DialogManager.GetKeyFilePath());
                sEncryptionKey = reader.ReadLine();
                reader.Close();
            }
            if (File.Exists(DialogManager.GetKeyFilePathSave()))
            {
                StreamReader reader = new StreamReader(DialogManager.GetKeyFilePathSave());
                sEncryptionKey = reader.ReadLine();
                reader.Close();
            }
            XmlWriter xmlWriter = XmlWriter.Create(saveFileDialog.FileName, xmlSettings);
            xmlWriter.WriteStartElement("pwlist");

            for (int index = 0; index < listView.Items.Count; index++)
            {
                xmlWriter.WriteStartElement("pwentry");

                xmlWriter.WriteStartElement("title");
                xmlWriter.WriteString(Cryptography.AES_Encrypt(listView.Items[index].SubItems[0].Text, sEncryptionKey));
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("username");
                xmlWriter.WriteString(Cryptography.AES_Encrypt(listView.Items[index].SubItems[1].Text, sEncryptionKey));
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("password");
                xmlWriter.WriteString(Cryptography.AES_Encrypt(listView.Items[index].SubItems[2].Text, sEncryptionKey));
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("url");
                xmlWriter.WriteString(Cryptography.AES_Encrypt(listView.Items[index].SubItems[3].Text, sEncryptionKey));
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("notes");
                xmlWriter.WriteString(Cryptography.AES_Encrypt(listView.Items[index].SubItems[4].Text, sEncryptionKey));
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.Close();
        }
        /// <summary>
        /// Overload: Export ListView entries into encrypted XML-Files
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="sFileName"></param>
        public static void ExportEncryptedToXml(ListView listView, string sFileName)
        {
            if (string.IsNullOrEmpty(sFileName)) return;

            string sEncryptionKey = null;

            XmlWriterSettings xmlSettings = new XmlWriterSettings
            {
                Indent = true,
                NewLineOnAttributes = true,
                CloseOutput = true
            };

            if (File.Exists(DialogManager.GetKeyFilePath()))
            {
                StreamReader reader = new StreamReader(DialogManager.GetKeyFilePath());
                sEncryptionKey = reader.ReadLine();
                reader.Close();
            }
            if (File.Exists(DialogManager.GetKeyFilePathSave()))
            {
                StreamReader reader = new StreamReader(DialogManager.GetKeyFilePathSave());
                sEncryptionKey = reader.ReadLine();
                reader.Close();
            }

            XmlWriter xmlWriter = XmlWriter.Create(sFileName, xmlSettings);
            xmlWriter.WriteStartElement("pwlist");

            for (int index = 0; index < listView.Items.Count; index++)
            {
                xmlWriter.WriteStartElement("pwentry");

                xmlWriter.WriteStartElement("title");
                xmlWriter.WriteString(Cryptography.AES_Encrypt(listView.Items[index].SubItems[0].Text, sEncryptionKey));
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("username");
                xmlWriter.WriteString(Cryptography.AES_Encrypt(listView.Items[index].SubItems[1].Text, sEncryptionKey));
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("password");
                xmlWriter.WriteString(Cryptography.AES_Encrypt(listView.Items[index].SubItems[2].Text, sEncryptionKey));
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("url");
                xmlWriter.WriteString(Cryptography.AES_Encrypt(listView.Items[index].SubItems[3].Text, sEncryptionKey));
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("notes");
                xmlWriter.WriteString(Cryptography.AES_Encrypt(listView.Items[index].SubItems[4].Text, sEncryptionKey));
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.Close();
        }
        /// <summary>
        /// Import ListView entries from encrypted XML-Files
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="openFileDialog"></param>
        public static void ImportEncryptedFromXml(ListView listView, OpenFileDialog openFileDialog)
        {
            XDocument xDoc = XDocument.Load(openFileDialog.FileName);

            if (!File.Exists(DialogManager.GetKeyFilePath())) return;

            StreamReader reader = new StreamReader(DialogManager.GetKeyFilePath());
            string sEncryptionKey = reader.ReadLine();
            reader.Close();

            foreach (var item in xDoc.Descendants("pwentry"))
            {
                ListViewItem lvItem = new ListViewItem(new string[]
                {
                    Cryptography.AES_Decrypt(item.Element("title")?.Value, sEncryptionKey),
                    Cryptography.AES_Decrypt(item.Element("username")?.Value, sEncryptionKey),
                    Cryptography.AES_Decrypt(item.Element("password")?.Value, sEncryptionKey),
                    Cryptography.AES_Decrypt(item.Element("url")?.Value, sEncryptionKey),
                    Cryptography.AES_Decrypt(item.Element("notes")?.Value, sEncryptionKey)
                });
                listView.Items.Add(lvItem);
            }
        }
        /// <summary>
        /// Save MainWindow settings XML-File
        /// </summary>
        /// <param name="Window"></param>
        /// <param name="sFilename"></param>
        public static void SaveConfigXml(Form Window, string sFilename)
        {
            XmlWriterSettings xmlSettings = new XmlWriterSettings
            {
                Indent = true,
                NewLineOnAttributes = true
            };

            XmlWriter xmlWriter = XmlWriter.Create(sFilename, xmlSettings);
            xmlWriter.WriteStartElement("mainWindow");

            xmlWriter.WriteStartElement("settings");

            xmlWriter.WriteStartElement("width");
            xmlWriter.WriteString(Window.Width.ToString());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("height");
            xmlWriter.WriteString(Window.Height.ToString());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("left");
            xmlWriter.WriteString(Window.Location.X.ToString());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("top");
            xmlWriter.WriteString(Window.Location.Y.ToString());
            xmlWriter.WriteEndElement();

            if (File.Exists(DialogManager.GetKeyFilePath()))
            {
                xmlWriter.WriteStartElement("recentKeyPath");
                xmlWriter.WriteString(DialogManager.GetKeyFilePath());
                xmlWriter.WriteEndElement();
            }
            if (File.Exists(DialogManager.GetKeyFilePathSave()))
            {
                xmlWriter.WriteStartElement("recentKeyPath");
                xmlWriter.WriteString(DialogManager.GetKeyFilePathSave());
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();
            xmlWriter.Close();
        }
        /// <summary>
        /// Load MainWindow settings XML-File
        /// </summary>
        /// <param name="Window"></param>
        /// <param name="sFilename"></param>
        public static void LoadConfigXml(Form Window, string sFilename)
        {
            if (!File.Exists(sFilename)) { return; }
            using (XmlReader xmlReader = XmlReader.Create(sFilename))
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.ReadToFollowing("width"))
                        Window.Width = xmlReader.ReadElementContentAsInt();
                    if (xmlReader.ReadToFollowing("height"))
                        Window.Height = xmlReader.ReadElementContentAsInt();
                    if (xmlReader.ReadToFollowing("left"))
                        Window.Left = xmlReader.ReadElementContentAsInt();
                    if (xmlReader.ReadToFollowing("top"))
                        Window.Top = xmlReader.ReadElementContentAsInt();
                    if (xmlReader.ReadToFollowing("recentKeyPath"))
                        _mSRecentKeyFilePath = xmlReader.ReadElementContentAsString();
                }
                xmlReader.Close();
            }
        }
        /// <summary>
        /// Try to retrieve recent keyfile path
        /// </summary>
        /// <returns></returns>
        public static string GetRecentKeyFilePath()
        {
            return string.IsNullOrEmpty(_mSRecentKeyFilePath) ? @"F:\pwsafe.enigma" : _mSRecentKeyFilePath;
        }
    }
}