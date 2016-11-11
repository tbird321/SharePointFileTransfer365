// Decompiled with JetBrains decompiler
// Type: OrbitOne.SharePoint.Importer.SharePoint.IDocumentLibraryRepository
// Assembly: OrbitOne.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\OrbitOne.SharePoint.Importer.exe

using OrbitOne.SharePoint.Importer.Domain;
using System.Collections.Generic;

namespace OrbitOne.SharePoint.Importer.SharePoint
{
    public interface IDocumentLibraryRepository
    {
        CreateFileResult CreateFile(ImportFile importFile);

        void CreateFolder(ImportFolder folder);

        IImportValidator CreateValidator();

        IList<NameSourcePair> GetFilenamesWithSource();
    }
}
