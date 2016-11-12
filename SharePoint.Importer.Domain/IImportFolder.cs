// Decompiled with JetBrains decompiler
// Type: SharepointFileTransfer.SharePoint.Importer.Domain.IImportFolder
// Assembly: SharepointFileTransfer.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\SharepointFileTransfer.SharePoint.Importer.exe

using System.Collections.Generic;

namespace SharepointFileTransfer.SharePoint.Importer.Domain
{
    public interface IImportFolder
    {
        IList<ImportFile> Files { get; }

        IList<ImportFolder> Folders { get; }

        void Add(ImportItem item);
    }
}
