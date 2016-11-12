using SharepointFileTransfer.SharePoint.Importer.Domain;
using System;
using System.Collections.Generic;

namespace SharepointFileTransfer.SharePoint.Importer.SharePoint
{
    public abstract class SharePointImportDestination : IImportDestination
    {
        protected ImportSettings ImportSettings { get; }
        protected IDocumentLibraryRepository DocumentLibraryRepository;
        private IList<NameSourcePair> m_ExistingFilenames;

        protected IList<NameSourcePair> ExistingFilenames
        {
            get
            {
                if (this.m_ExistingFilenames == null)
                    this.m_ExistingFilenames = this.DocumentLibraryRepository.GetFilenamesWithSource();
                return this.m_ExistingFilenames;
            }
        }

        public event EventHandler<ItemProcessedEventArgs> ItemProcessed;

        public SharePointImportDestination(ImportSettings settings)
        {
            this.DocumentLibraryRepository = (IDocumentLibraryRepository)new SharepointFileTransfer.SharePoint.Importer.SharePoint.DocumentLibraryRepository(settings);
            ImportSettings = settings;
        }

        public abstract void Import(ImportItem importItem);

        public IImportValidator GetValidator()
        {
            return this.DocumentLibraryRepository.CreateValidator();
        }

        protected void RaiseItemProcessed(ImportItem item, string location)
        {
            if (this.ItemProcessed == null)
                return;
            this.ItemProcessed((object)this, new ItemProcessedEventArgs(item, location));
        }
    }
}
