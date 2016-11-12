// Decompiled with JetBrains decompiler
// Type: SharepointFileTransfer.SharePoint.Importer.Domain.ImportSettings
// Assembly: SharepointFileTransfer.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\SharepointFileTransfer.SharePoint.Importer.exe

using System.Net;

namespace SharepointFileTransfer.SharePoint.Importer.Domain
{
    public class ImportSettings
    {
        public bool OverwriteFile { get; set; }
        public bool LoggingEnabled { get; set; }

        public string SourceFolder { get; set; }

        public string DocumentLibrary { get; set; }

        public string SiteUrl { get; set; }

        public bool CreateEmptyFolders { get; set; }

        public string ArchiveFolder { get; set; }

        public ImportMode Mode { get; set; }

        public bool ImportHiddenFiles { get; set; }

        public bool CreateFolders { get; set; }

        public bool MoveFiles
        {
            get
            {
                return !string.IsNullOrEmpty(this.ArchiveFolder);
            }
        }

        public ICredentials Credentials
        {
            get
            {
                if (this.Username != null)
                    return (ICredentials)new NetworkCredential(this.Username, this.Password, this.Domain);
                return CredentialCache.DefaultCredentials;
            }
        }

        public AuthenticationMode AuthenticationMode { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }

        public string Domain { get; set; }
    }
}
