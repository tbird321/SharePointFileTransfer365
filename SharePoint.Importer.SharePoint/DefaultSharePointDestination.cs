using log4net;
using SharepointFileTransfer.SharePoint.Importer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharepointFileTransfer.SharePoint.Importer.SharePoint
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
