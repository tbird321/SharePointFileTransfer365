// Decompiled with JetBrains decompiler
// Type: SharepointFileTransfer.SharePoint.Importer.CommandLine.CommandLineArguments
// Assembly: SharepointFileTransfer.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\SharepointFileTransfer.SharePoint.Importer.exe

using SharepointFileTransfer.SharePoint.Importer.CommandLineParsing;

namespace SharepointFileTransfer.SharePoint.Importer.CommandLine
{
    public class CommandLineArguments
    {
        [Required]
        public string folder { get; set; }

        [Required]
        public string site { get; set; }

        [Required]
        public string documentlibrary { get; set; }

        public bool OverwriteFile { get; set; }
        public string Archive { get; set; }

        public bool CreateFolders { get; set; }

        public bool ImportHiddenFiles { get; set; }

        public bool CreateEmptyFolders { get; set; }

        public string Domain { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool WhatIf { get; set; }

        public bool Analyse { get; set; }

        public bool nolog { get; set; }

        public string authenticationmode { get; set; }
    }
}
