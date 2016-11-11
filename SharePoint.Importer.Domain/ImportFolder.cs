// Decompiled with JetBrains decompiler
// Type: OrbitOne.SharePoint.Importer.Domain.ImportFolder
// Assembly: OrbitOne.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\OrbitOne.SharePoint.Importer.exe

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace OrbitOne.SharePoint.Importer.Domain
{
    [DebuggerDisplay("{OriginalFullName}")]
    public class ImportFolder : ImportItem, IImportFolder
    {
        private IList<ImportItem> m_items;

        public DirectoryInfo SourceDirectory { get; set; }

        public bool IsRoot
        {
            get
            {
                return this.Parent == null;
            }
        }

        public IList<ImportFile> Files
        {
            get
            {
                return (IList<ImportFile>)this.m_items.OfType<ImportFile>().ToList<ImportFile>();
            }
        }

        public IList<ImportFolder> Folders
        {
            get
            {
                return (IList<ImportFolder>)this.m_items.OfType<ImportFolder>().ToList<ImportFolder>();
            }
        }

        public override string OriginalName
        {
            get
            {
                return this.SourceDirectory.Name;
            }
        }

        public override string OriginalFullName
        {
            get
            {
                return this.SourceDirectory.FullName;
            }
        }

        public override bool IsFile
        {
            get
            {
                return false;
            }
        }

        public override long Size
        {
            get
            {
                return this.m_items.Sum<ImportItem>((Func<ImportItem, long>)(item => item.Size));
            }
            set
            {
            }
        }

        public override DateTime Created
        {
            get
            {
                return this.SourceDirectory.CreationTime;
            }
        }

        public override DateTime Modified
        {
            get
            {
                return this.SourceDirectory.LastWriteTime;
            }
        }

        public int FileCount
        {
            get
            {
                return this.Folders.Sum<ImportFolder>((Func<ImportFolder, int>)(f => f.FileCount)) + this.Files.Count<ImportFile>();
            }
        }

        public ImportFolder()
        {
            this.m_items = (IList<ImportItem>)new List<ImportItem>();
        }

        public ImportFolder(ImportFolder importFolder)
          : this()
        {
            this.SourceDirectory = importFolder.SourceDirectory;
            this.OriginalParent = importFolder.OriginalParent;
        }

        public void Add(ImportItem item)
        {
            item.Parent = this;
            if (item.OriginalParent == null)
                item.OriginalParent = this;
            this.m_items.Add(item);
        }
    }
}
