// Decompiled with JetBrains decompiler
// Type: SharepointFileTransfer.SharePoint.Importer.SharePoint.IDocumentLibraryRepository
// Assembly: SharepointFileTransfer.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\SharepointFileTransfer.SharePoint.Importer.exe

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
