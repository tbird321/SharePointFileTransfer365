using System;

namespace SharepointFileTransfer.SharePoint.Importer.Domain
{
    public interface IImportDestination
    {
        event EventHandler<ItemProcessedEventArgs> ItemProcessed;

        void Import(ImportItem importFile);

        IImportValidator GetValidator();
    }
}
