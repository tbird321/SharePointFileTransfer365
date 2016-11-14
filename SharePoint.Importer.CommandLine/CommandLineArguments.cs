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
        public string file { get; set; }
    }
}
