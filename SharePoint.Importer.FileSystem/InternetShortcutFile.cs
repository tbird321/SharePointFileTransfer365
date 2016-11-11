// Decompiled with JetBrains decompiler
// Type: OrbitOne.SharePoint.Importer.FileSystem.InternetShortcutFile
// Assembly: OrbitOne.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\OrbitOne.SharePoint.Importer.exe

using System;
using System.IO;

namespace OrbitOne.SharePoint.Importer.FileSystem
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
