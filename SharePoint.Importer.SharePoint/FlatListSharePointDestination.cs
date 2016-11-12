// Decompiled with JetBrains decompiler
// Type: OrbitOne.SharePoint.Importer.SharePoint.FlatListSharePointDestination
// Assembly: OrbitOne.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\OrbitOne.SharePoint.Importer.exe

using log4net;
using OrbitOne.SharePoint.Importer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrbitOne.SharePoint.Importer.SharePoint
{
    public class FlatListSharePointDestination : SharePointImportDestination
    {
        protected static readonly ILog Log = LogManager.GetLogger(typeof(FlatListSharePointDestination));
        private FlatListDuplicateNameResolver m_nameResolver;

        private FlatListDuplicateNameResolver NameResolver
        {
            get
            {
                if (this.m_nameResolver == null)
                    this.m_nameResolver = new FlatListDuplicateNameResolver(this.ExistingFilenames.Select<NameSourcePair, string>((Func<NameSourcePair, string>)(p => p.Name)));
                return this.m_nameResolver;
            }
        }

        public FlatListSharePointDestination(ImportSettings settings)
          : base(settings)
        {
        }

        public override void Import(ImportItem importItem)
        {
            ImportFolder importFolder = new ImportFolder(importItem as ImportFolder);
            foreach (ImportFile file1 in this.GetFiles(importItem as ImportFolder))
            {
                ImportFile file2 = new ImportFile(file1);
                file2.Name = this.NameResolver.ResolveName(file1);
                importFolder.Add((ImportItem)file2);
                this.ImportFile(file1, file2);
            }
        }

        private void ImportFile(ImportFile orig, ImportFile file)
        {
            if (this.FileExists(file) && !ImportSettings.OverwriteFile)
            {
                System.Diagnostics.Debug.WriteLine("File already exists and is skipped: " + file.OriginalFullName);
                FlatListSharePointDestination.Log.Warn((object)("File already exists and is skipped: " + file.OriginalFullName));
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("START Processing " + file.OriginalFullName);
                FlatListSharePointDestination.Log.Info((object)("START Processing " + file.OriginalFullName));
                CreateFileResult file1 = this.DocumentLibraryRepository.CreateFile(file);
                this.ExistingFilenames.Add(new NameSourcePair()
                {
                    Name = file.Name,
                    Source = file.OriginalFullName
                });
                System.Diagnostics.Debug.WriteLine("End Processing " + file.OriginalFullName);
                if (!file1.Succeeded)
                    return;
                this.RaiseItemProcessed((ImportItem)orig, file1.Location);
            }
        }

        private bool FileExists(ImportFile file)
        {
            return this.ExistingFilenames.Any<NameSourcePair>((Func<NameSourcePair, bool>)(f => f.Source.Equals(file.OriginalFullName, StringComparison.InvariantCultureIgnoreCase)));
        }

        private IEnumerable<ImportFile> GetFiles(ImportFolder folder)
        {
            foreach (ImportFolder folder1 in (IEnumerable<ImportFolder>)folder.Folders)
            {
                foreach (ImportFile file in this.GetFiles(folder1))
                    yield return file;
            }
            foreach (ImportFile file in (IEnumerable<ImportFile>)folder.Files)
                yield return file;
        }
    }
}
