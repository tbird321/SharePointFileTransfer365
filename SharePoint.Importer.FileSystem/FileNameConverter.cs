// Decompiled with JetBrains decompiler
// Type: OrbitOne.SharePoint.Importer.FileSystem.FileNameConverter
// Assembly: OrbitOne.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\OrbitOne.SharePoint.Importer.exe

using System.IO;

namespace OrbitOne.SharePoint.Importer.FileSystem
{
    public class FileNameConverter
    {
        public char[] IllegalCharacters { get; set; }

        public int MaximumFileNameLenght { get; set; }

        public string Convert(string filename)
        {
            return this.Shorten(this.StripIllegalCharacters(filename));
        }

        private string StripIllegalCharacters(string filename)
        {
            string str = filename.Trim('.', ' ');
            foreach (char illegalCharacter in this.IllegalCharacters)
                str = str.Replace(illegalCharacter.ToString(), string.Empty);
            while (str.Contains(".."))
                str = str.Replace("..", ".");
            return str;
        }

        private string Shorten(string filename)
        {
            if (filename.Length <= this.MaximumFileNameLenght)
                return filename;
            string withoutExtension = Path.GetFileNameWithoutExtension(filename);
            string extension = Path.GetExtension(filename);
            return withoutExtension.Substring(0, this.MaximumFileNameLenght - extension.Length) + extension;
        }
    }
}
