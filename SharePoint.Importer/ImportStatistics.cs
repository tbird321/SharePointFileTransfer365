using SharepointFileTransfer.SharePoint.Importer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharepointFileTransfer.SharePoint.Importer
{
    public class ImportStatistics
    {
        public int NumberOfFiles { get; set; }

        public int NumberOfDirectories { get; set; }

        public int NumberOfEmptyDirectories { get; set; }

        public long TotalFileSize { get; set; }

        public IDictionary<string, string> ProblematicFiles { get; set; }

        public ImportStatistics(ImportItem item)
        {
            this.TotalFileSize = item.Size;
            this.NumberOfFiles = this.CountFiles(item as ImportFolder);
            this.NumberOfDirectories = this.CountFolders(item as ImportFolder);
            this.NumberOfEmptyDirectories = this.CountEmptyDirectories(item as ImportFolder);
        }

        private int CountEmptyDirectories(ImportFolder importFolder)
        {
            return importFolder.Folders.Where<ImportFolder>((Func<ImportFolder, bool>)(f => f.Files.Count == 0)).Count<ImportFolder>() + importFolder.Folders.Sum<ImportFolder>((Func<ImportFolder, int>)(f => this.CountEmptyDirectories(f)));
        }

        private int CountFiles(ImportFolder folder)
        {
            return folder.Files.Count + folder.Folders.Sum<ImportFolder>((Func<ImportFolder, int>)(f => this.CountFiles(f)));
        }

        private int CountFolders(ImportFolder folder)
        {
            return folder.Folders.Count + folder.Folders.Sum<ImportFolder>((Func<ImportFolder, int>)(f => this.CountFolders(f)));
        }
    }
}
