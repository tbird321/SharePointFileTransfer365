using System.IO;

namespace SharepointFileTransfer.SharePoint.Importer.FileSystem
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
