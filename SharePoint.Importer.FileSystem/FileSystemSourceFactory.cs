using SharepointFileTransfer.SharePoint.Importer.Domain;

namespace SharepointFileTransfer.SharePoint.Importer.FileSystem
{
    public class FileSystemSourceFactory
    {
        private readonly ImportSettings m_settings;

        public FileSystemSourceFactory(ImportSettings settings)
        {
            this.m_settings = settings;
        }

        public IImportSource CreateWith(IImportValidator validator)
        {
            return (IImportSource)new FileSystemSource(this.m_settings, validator);
        }
    }
}
