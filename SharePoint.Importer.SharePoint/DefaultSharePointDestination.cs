// Decompiled with JetBrains decompiler
// Type: OrbitOne.SharePoint.Importer.SharePoint.DefaultSharePointDestination
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
    public class DefaultSharePointDestination : SharePointImportDestination
    {
        protected static readonly ILog Log = LogManager.GetLogger(typeof(DefaultSharePointDestination));

        public DefaultSharePointDestination(ImportSettings settings)
          : base(settings)
        {
        }

        public override void Import(ImportItem importItem)
        {
            this.ImportContents(importItem as ImportFolder);
        }

        public void ImportContents(ImportFolder folder)
        {
            if (!folder.IsRoot)
                this.ImportFolder(folder);
            foreach (ImportFolder folder1 in (IEnumerable<ImportFolder>)folder.Folders)
                this.ImportContents(folder1);
            foreach (ImportFile file in (IEnumerable<ImportFile>)folder.Files)
                this.ImportFile(file);
        }

        public void ImportFolder(ImportFolder folder)
        {
            this.DocumentLibraryRepository.CreateFolder(folder);
        }

        public void ImportFile(ImportFile importFile)
        {
            if (this.FileExists(importFile))
            {
                DefaultSharePointDestination.Log.Warn((object)("File already exists: " + importFile.OriginalFullName));
            }
            else
            {
                DefaultSharePointDestination.Log.Info((object)("START Processing " + importFile.OriginalFullName));
                CreateFileResult file = this.DocumentLibraryRepository.CreateFile(importFile);
                this.ExistingFilenames.Add(new NameSourcePair()
                {
                    Name = importFile.Name,
                    Source = importFile.OriginalFullName
                });
                if (!file.Succeeded)
                    return;
                this.RaiseItemProcessed((ImportItem)importFile, file.Location);
            }
        }

        private bool FileExists(ImportFile importFile)
        {
            return this.ExistingFilenames.Any<NameSourcePair>((Func<NameSourcePair, bool>)(file => file.Source == importFile.OriginalFullName));
        }
    }
}
