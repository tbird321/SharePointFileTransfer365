// Decompiled with JetBrains decompiler
// Type: SharepointFileTransfer.SharePoint.Importer.FileSystem.FileSystemSource
// Assembly: SharepointFileTransfer.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\SharepointFileTransfer.SharePoint.Importer.exe

using log4net;
using SharepointFileTransfer.SharePoint.Importer.Domain;
using System.Collections.Generic;
using System.IO;

namespace SharepointFileTransfer.SharePoint.Importer.FileSystem
{
    public class FileSystemSource : IImportSource
    {
        private static ILog log = LogManager.GetLogger(typeof(FileSystemSource));
        private readonly ImportSettings m_settings;
        private IImportValidator m_validator;
        private FileNameConverter m_filenameConverter;

        public IMetaDataProvider MetaDataProvider { get; set; }

        public FileSystemSource(ImportSettings settings, IImportValidator validator)
        {
            FileSystemSource.log.Info((object)("Initializing data source: File system: " + settings.SourceFolder));
            this.m_settings = settings;
            this.m_validator = validator;
            this.MetaDataProvider = (IMetaDataProvider)new NullUserInformationManager();
            this.m_filenameConverter = new FileNameConverter()
            {
                MaximumFileNameLenght = validator.MaximumFileNameLength,
                IllegalCharacters = validator.IllegalCharacters
            };
        }

        public ImportItem LoadItems()
        {
            return this.Load(this.m_settings.SourceFolder);
        }

        private ImportItem Load(string name)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(name);
            ImportFolder importFolder = new ImportFolder();
            importFolder.SourceDirectory = directoryInfo;
            importFolder.Name = this.m_filenameConverter.Convert(directoryInfo.Name);
            importFolder.CreatedBy = this.MetaDataProvider.GetAuthor(directoryInfo.FullName);
            importFolder.ModifiedBy = this.MetaDataProvider.GetEditor(directoryInfo.FullName);
            importFolder.MetaData = this.MetaDataProvider.GetMetaData(directoryInfo.FullName);
            ImportFolder directory1 = importFolder;
            ValidationResult result1 = this.m_validator.Validate(directory1);
            if (result1.IsValid)
            {
                foreach (DirectoryInfo directory2 in directoryInfo.GetDirectories())
                {
                    if ((directory2.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden || this.m_settings.ImportHiddenFiles)
                        directory1.Add(this.Load(directory2.FullName));
                }
                foreach (FileInfo file1 in directoryInfo.GetFiles())
                {
                    if ((file1.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden || this.m_settings.ImportHiddenFiles)
                    {
                        ImportFile importFile = new ImportFile();
                        importFile.SourceFile = file1;
                        importFile.Name = this.m_filenameConverter.Convert(file1.Name);
                        importFile.CreatedBy = this.MetaDataProvider.GetAuthor(file1.FullName);
                        importFile.ModifiedBy = this.MetaDataProvider.GetEditor(file1.FullName);
                        importFile.MetaData = this.MetaDataProvider.GetMetaData(file1.FullName);
                        ImportFile file2 = importFile;
                        ValidationResult result2 = this.m_validator.Validate(file2);
                        if (result2.IsValid)
                            directory1.Add((ImportItem)file2);
                        else
                            this.Log(result2);
                    }
                }
            }
            else
                this.Log(result1);
            return (ImportItem)directory1;
        }

        private void Log(ValidationResult result)
        {
            foreach (string error in (IEnumerable<string>)result.Errors)
                FileSystemSource.log.Error((object)(error + "  " + result.Source));
            foreach (string warning in (IEnumerable<string>)result.Warnings)
                FileSystemSource.log.Warn((object)(warning + "  " + result.Source));
        }
    }
}
