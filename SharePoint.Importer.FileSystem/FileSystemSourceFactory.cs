// Decompiled with JetBrains decompiler
// Type: OrbitOne.SharePoint.Importer.FileSystem.FileSystemSourceFactory
// Assembly: OrbitOne.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\OrbitOne.SharePoint.Importer.exe

using OrbitOne.SharePoint.Importer.Domain;

namespace OrbitOne.SharePoint.Importer.FileSystem
{
    public class FileSystemSourceFactory
    {
        private readonly ImportSettings m_settings;

        public FileSystemSourceFactory(ImportSettings settings)
        {
            this.m_settings = settings;
        }

        public IImportSource CreateWith(IImportValidator validator)
        {
            return (IImportSource)new FileSystemSource(this.m_settings, validator);
        }
    }
}
