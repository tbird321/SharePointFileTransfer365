﻿using log4net;
using log4net.Config;
using SharepointFileTransfer.SharePoint.Importer.CommandLineParsing;
using SharepointFileTransfer.SharePoint.Importer.Domain;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharepointFileTransfer.SharePoint.Importer.CommandLine
{
    internal class Program
    {
        private static ILog log = LogManager.GetLogger(typeof(Program));

        private static void Main(string[] args)
        {
            Program.InitLogging();
            ImportSettings settings = Program.GetSettings((IEnumerable<string>)args);
            if (!Program.Verify(settings))
            {
                Program.PrintUsage();
                Program.log.Error((object)"Settings are not valid, Import aborted.");
            }
            else
                new DocumentImporter(settings).Execute();
            Program.log.Info((object)"Import completed.");
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Required Arguments:");
            Console.WriteLine();           
            Console.WriteLine("-file\t\t The file that you want to import.");
            Console.WriteLine("-folder\t\t The directory that you want to import.");
            Console.WriteLine("-site\t\t The url of the site you cwant to import to.");
            Console.WriteLine("-documentlibrary\t The name of the document library you want to import to");
            Console.WriteLine();
            Console.WriteLine("Optional arguments");
            Console.WriteLine();
            Console.WriteLine("-CreateFolders:\t\t If present, folders will be created in the target\n\t\t\t document library. If not, all files will be imported\n\t\t\t to the root.");
            Console.WriteLine("-ImportHiddenFiles\t If present, hidden files will be imported.\n\t\t\t By default the are skipped.");
            Console.WriteLine("-CreateEmptyFolders\t If present, empty folders will be created in the\n\t\t\t target document library. By default they are skipped.");
            Console.WriteLine("-MovedFolder\t\t If present, all imported files will be moved to the\n\t\t\t specified folder");
            Console.WriteLine("-OverwriteFile\t\t If present files will be overwritten");
        }

        private static void InitLogging()
        {
            XmlConfigurator.Configure();
        }

        private static bool Verify(ImportSettings settings)
        {
            bool flag = true;
            if (settings == null)
            {
                Program.log.Error((object)"Settings are null");
                return false;
            }
            if (string.IsNullOrEmpty(settings.DocumentLibrary))
            {
                Program.log.Error((object)"documentlibrary is required");
                flag = false;
            }
            if (string.IsNullOrEmpty(settings.SiteUrl))
            {
                Program.log.Error((object)"site url is required");
                flag = false;
            }
            if (string.IsNullOrEmpty(settings.SourceFolder))
            {
                Program.log.Error((object)"folder is required");
                flag = false;
            }
            else if (!new DirectoryInfo(settings.SourceFolder).Exists)
            {
                Program.log.Error((object)("Source folder does not exist: " + settings.SourceFolder));
                flag = false;
            }
            if (settings.MoveFiles)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(settings.ArchiveFolder);
                if (!directoryInfo.Exists)
                {
                    Program.log.Error((object)("Target directory does not exist: " + settings.ArchiveFolder));
                    flag = false;
                }
                else if (directoryInfo.GetFileSystemInfos().Length > 0)
                {
                    Program.log.Error((object)("Target directory is not empty: " + settings.ArchiveFolder));
                    flag = false;
                }
            }
            return flag;
        }

        private static ImportSettings GetSettings(IEnumerable<string> args)
        {
            ImportSettings importSettings = new ImportSettings();
            try
            {
                CommandLineArguments valueToPopulate = new CommandLineArguments();
                valueToPopulate.ParseArguments(args, '-', ':');
                importSettings.DocumentLibrary = valueToPopulate.documentlibrary;
                importSettings.SiteUrl = valueToPopulate.site;
                importSettings.SourceFolder = valueToPopulate.folder;
                importSettings.ArchiveFolder = valueToPopulate.Archive;
                importSettings.CreateFolders = valueToPopulate.CreateFolders;
                importSettings.ImportHiddenFiles = valueToPopulate.ImportHiddenFiles;
                importSettings.CreateEmptyFolders = valueToPopulate.CreateEmptyFolders;
                importSettings.LoggingEnabled = !valueToPopulate.nolog;
                importSettings.Username = valueToPopulate.Username;
                importSettings.Password = valueToPopulate.Password;
                importSettings.Domain = valueToPopulate.Domain;
                importSettings.file = valueToPopulate.file;
                importSettings.OverwriteFile = valueToPopulate.OverwriteFile;
                importSettings.Mode = !valueToPopulate.WhatIf ? (!valueToPopulate.Analyse ? ImportMode.Execute : ImportMode.Analyse) : ImportMode.WhatIf;
                importSettings.AuthenticationMode = string.IsNullOrEmpty(valueToPopulate.authenticationmode) || !Enum.IsDefined(typeof(AuthenticationMode), (object)valueToPopulate.authenticationmode) ? AuthenticationMode.Windows : (AuthenticationMode)Enum.Parse(typeof(AuthenticationMode), valueToPopulate.authenticationmode, true);
            }
            catch (Exception ex)
            {
                Program.log.Error((object)ex);
            }
            return importSettings;
        }
    }
}
