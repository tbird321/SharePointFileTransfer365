// Decompiled with JetBrains decompiler
// Type: SharepointFileTransfer.SharePoint.Importer.Domain.ImportFile
// Assembly: SharepointFileTransfer.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\SharepointFileTransfer.SharePoint.Importer.exe

using System;
using System.Diagnostics;
using System.IO;

namespace SharepointFileTransfer.SharePoint.Importer.Domain
{
    [DebuggerDisplay("{OriginalFullName}")]
    public class ImportFile : ImportItem
    {
        private FileInfo m_sourceFile;
        private long m_fileSize;
        private string m_originalName;
        private string m_originalFullName;

        public FileInfo SourceFile
        {
            get
            {
                return this.m_sourceFile;
            }
            set
            {
                this.m_sourceFile = value;
                if (value == null)
                    return;
                this.m_fileSize = value.Length;
                this.m_originalName = value.Name;
                this.m_originalFullName = value.FullName;
            }
        }

        public override long Size
        {
            get
            {
                return this.m_fileSize;
            }
            set
            {
                this.m_fileSize = value;
            }
        }

        public override DateTime Created
        {
            get
            {
                return this.SourceFile.CreationTime;
            }
        }

        public override DateTime Modified
        {
            get
            {
                return this.SourceFile.LastWriteTime;
            }
        }

        public string Extension
        {
            get
            {
                return Path.GetExtension(this.Name);
            }
        }

        public override string OriginalName
        {
            get
            {
                return this.m_originalName;
            }
        }

        public override string OriginalFullName
        {
            get
            {
                return this.m_originalFullName;
            }
        }

        public override bool IsFile
        {
            get
            {
                return true;
            }
        }

        public ImportFile()
        {
        }

        public ImportFile(ImportFile original)
        {
            this.SourceFile = original.SourceFile;
            this.Name = original.Name;
            this.CreatedBy = original.CreatedBy;
            this.ModifiedBy = original.ModifiedBy;
            this.MetaData = original.MetaData;
            this.OriginalParent = original.OriginalParent;
            this.m_fileSize = original.Size;
        }

        public Stream OpenRead()
        {
            return (Stream)this.SourceFile.OpenRead();
        }
    }
}
