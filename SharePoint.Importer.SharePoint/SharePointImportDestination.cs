// Decompiled with JetBrains decompiler
// Type: OrbitOne.SharePoint.Importer.SharePoint.SharePointImportDestination
// Assembly: OrbitOne.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\OrbitOne.SharePoint.Importer.exe

using OrbitOne.SharePoint.Importer.Domain;
using System;
using System.Collections.Generic;

namespace OrbitOne.SharePoint.Importer.SharePoint
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
            this.DocumentLibraryRepository = (IDocumentLibraryRepository)new OrbitOne.SharePoint.Importer.SharePoint.DocumentLibraryRepository(settings);
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
