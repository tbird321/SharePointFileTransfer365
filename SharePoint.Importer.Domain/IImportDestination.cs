﻿// Decompiled with JetBrains decompiler
// Type: OrbitOne.SharePoint.Importer.Domain.IImportDestination
// Assembly: OrbitOne.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\OrbitOne.SharePoint.Importer.exe

using System;

namespace OrbitOne.SharePoint.Importer.Domain
{
    public interface IImportDestination
    {
        event EventHandler<ItemProcessedEventArgs> ItemProcessed;

        void Import(ImportItem importFile);

        IImportValidator GetValidator();
    }
}