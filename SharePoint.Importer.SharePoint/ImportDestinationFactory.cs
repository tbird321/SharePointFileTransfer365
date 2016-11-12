// Decompiled with JetBrains decompiler
// Type: SharepointFileTransfer.SharePoint.Importer.SharePoint.ImportDestinationFactory
// Assembly: SharepointFileTransfer.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\SharepointFileTransfer.SharePoint.Importer.exe

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
