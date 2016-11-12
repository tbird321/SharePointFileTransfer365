using log4net;
using SharepointFileTransfer.SharePoint.Importer.Domain;
using System.IO;

namespace SharepointFileTransfer.SharePoint.Importer.FileSystem
{
    public class PostImportFileProcessor
    {
        private static ILog log = LogManager.GetLogger(typeof(PostImportFileProcessor));
        private readonly ImportSettings m_settings;

        public PostImportFileProcessor(ImportSettings settings)
        {
            this.m_settings = settings;
        }

        public void MoveItem(object sender, ItemProcessedEventArgs e)
        {
            string destFileName = e.Item.OriginalFullName.Replace(this.m_settings.SourceFolder, this.m_settings.ArchiveFolder);
            if (!this.m_settings.MoveFiles || !e.Item.IsFile || this.m_settings.Mode != ImportMode.Execute)
                return;
            this.EnsureFolder(e.Item.Parent);
            string filePath = Path.Combine(e.Item.Parent.OriginalFullName, "Moved to SharePoint.url");
            InternetShortcutFile internetShortcutFile = new InternetShortcutFile(e.Location, filePath);
            File.Move(e.Item.OriginalFullName, destFileName);
            internetShortcutFile.Create();
        }

        private void EnsureFolder(ImportFolder folder)
        {
            if (folder == null)
                return;
            string path = folder.OriginalFullName.Replace(this.m_settings.SourceFolder, this.m_settings.ArchiveFolder);
            if (Directory.Exists(path))
                return;
            this.EnsureFolder(folder.Parent);
            Directory.CreateDirectory(path);
            PostImportFileProcessor.log.Info((object)("created directory " + path));
        }
    }
}
