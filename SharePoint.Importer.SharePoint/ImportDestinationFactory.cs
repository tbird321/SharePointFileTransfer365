using SharepointFileTransfer.SharePoint.Importer.Domain;

namespace SharepointFileTransfer.SharePoint.Importer.SharePoint
{
    public class ImportDestinationFactory
    {
        private readonly ImportSettings m_settings;

        public ImportDestinationFactory(ImportSettings settings)
        {
            this.m_settings = settings;
        }

        public IImportDestination Create()
        {
            if (!this.m_settings.CreateFolders)
                return (IImportDestination)new FlatListSharePointDestination(this.m_settings);
            return (IImportDestination)new DefaultSharePointDestination(this.m_settings);
        }
    }
}
