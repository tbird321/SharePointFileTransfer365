using SharepointFileTransfer.SharePoint.Importer.Domain;
using System.Collections.Generic;

namespace SharepointFileTransfer.SharePoint.Importer.SharePoint
{
    public interface IDocumentLibraryRepository
    {
        CreateFileResult CreateFile(ImportFile importFile);

        void CreateFolder(ImportFolder folder);

        IImportValidator CreateValidator();

        IList<NameSourcePair> GetFilenamesWithSource();
    }
}
