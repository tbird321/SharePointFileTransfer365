using log4net;
using SharepointFileTransfer.SharePoint.Importer.Domain;
using SharepointFileTransfer.SharePoint.Importer.FileSystem;
using SharepointFileTransfer.SharePoint.Importer.SharePoint;
using System;

namespace SharepointFileTransfer.SharePoint.Importer
{
    public class DocumentImporter
    {
        private static ILog log = LogManager.GetLogger(typeof(DocumentImporter));
        private readonly ImportSettings m_settings;

        public DocumentImporter(ImportSettings settings)
        {
            this.m_settings = settings;
        }

        public void Execute()
        {
            DocumentImporter.log.Info((object)"Configuring application");
            DocumentImporter.log.Info((object)"Initializing Sharepoint Connection");
            IImportDestination importDestination = new ImportDestinationFactory(this.m_settings).Create();
            DocumentImporter.log.Info((object)"Initializing Validation");
            IImportValidator validator = importDestination.GetValidator();
            if (!validator.IsValid)
            {
                DocumentImporter.log.Error((object)("The document library " + this.m_settings.DocumentLibrary + " does not exist"));
            }
            else
            {
                IImportSource with = new FileSystemSourceFactory(this.m_settings).CreateWith(validator);
                PostImportFileProcessor importFileProcessor = new PostImportFileProcessor(this.m_settings);
                importDestination.ItemProcessed += (EventHandler<ItemProcessedEventArgs>)((s, args) => DocumentImporter.log.Info((object)("END Processing " + args.Item.OriginalFullName)));
                if (this.m_settings.MoveFiles)
                    importDestination.ItemProcessed += new EventHandler<ItemProcessedEventArgs>(importFileProcessor.MoveItem);
                ImportItem importItem = with.LoadItems();
                DocumentImporter.DisplayImportStatistics(importItem);
                if (this.m_settings.Mode == ImportMode.Execute)
                {
                    DocumentImporter.log.Info((object)"Start Import");
                    importDestination.Import(importItem);
                    DocumentImporter.log.Info((object)"Import finished");
                }
                else
                {
                    int mode = (int)this.m_settings.Mode;
                }
            }
        }

        private static void DisplayImportStatistics(ImportItem items)
        {
            ImportStatistics importStatistics = new ImportStatistics(items);
            DocumentImporter.log.Info((object)("Number of files: " + (object)importStatistics.NumberOfFiles));
            DocumentImporter.log.Info((object)("Number of folders: " + (object)importStatistics.NumberOfDirectories));
            DocumentImporter.log.Info((object)("Number of empty folders: " + (object)importStatistics.NumberOfEmptyDirectories));
            DocumentImporter.log.Info((object)("Total file size: " + DocumentImporter.GetSizeString(importStatistics.TotalFileSize)));
        }

        private static string GetSizeString(long bytes)
        {
            if ((double)bytes >= 1073741824.0)
                return string.Format("{0:##.##}", (object)((double)bytes / 1073741824.0)) + " GB";
            if ((double)bytes >= 1048576.0)
                return string.Format("{0:##.##}", (object)((double)bytes / 1048576.0)) + " MB";
            if (bytes >= 1024L)
                return string.Format("{0:##.##}", (object)(bytes / 1024L)) + " KB";
            return bytes.ToString() + " bytes";
        }
    }
}
