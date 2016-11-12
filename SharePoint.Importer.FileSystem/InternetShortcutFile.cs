using System;
using System.IO;

namespace SharepointFileTransfer.SharePoint.Importer.FileSystem
{
    public class InternetShortcutFile
    {
        public string Url { get; set; }

        public string FilePath { get; set; }

        public InternetShortcutFile(string url, string filePath)
        {
            this.FilePath = filePath;
            this.Url = url;
        }

        public void Create()
        {
            if (string.IsNullOrEmpty(this.Url))
                throw new ArgumentException("Url can not be empty");
            if (string.IsNullOrEmpty(this.FilePath))
                throw new ArgumentException("FilePath can not be empty");
            if (File.Exists(this.FilePath))
                return;
            using (StreamWriter text = File.CreateText(this.FilePath))
            {
                text.WriteLine("[InternetShortcut]");
                text.WriteLine();
                text.WriteLine("URL=" + this.Url);
            }
        }
    }
}
