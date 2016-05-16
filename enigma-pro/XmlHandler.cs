using Enigma.Cryptography;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace enigma_pro
{
    internal class XmlHandler
    {
        /// <summary>
        /// Save & Load ListView entries into XML-Files
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

        public static void ExportEncryptedToXml(ListView listView, string sFileName)
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

        public static void SaveConfigXml(Form Window, string sFilename)
        {
            XmlWriterSettings xmlSettings = new XmlWriterSettings
            {
                Indent = true,
                NewLineOnAttributes = true
            };

            XmlWriter xmlWriter = XmlWriter.Create(sFilename, xmlSettings);
            xmlWriter.WriteStartElement(Window.Text);

            xmlWriter.WriteStartElement("properties");

            xmlWriter.WriteStartElement("size");
            xmlWriter.WriteString(Window.Size.ToString());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("startPosition");
            xmlWriter.WriteString(Window.StartPosition.ToString());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();
            xmlWriter.Close();
        }
    }
}