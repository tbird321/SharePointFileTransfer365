using System;
using System.Collections.Generic;

namespace SharepointFileTransfer.SharePoint.Importer.Domain
{
    public abstract class ImportItem
    {
        private IDictionary<string, string> m_metaData = (IDictionary<string, string>)new Dictionary<string, string>();

        public abstract string OriginalName { get; }

        public abstract string OriginalFullName { get; }

        public ImportFolder Parent { get; set; }

        public ImportFolder OriginalParent { get; set; }

        public abstract bool IsFile { get; }

        public abstract long Size { get; set; }

        public User CreatedBy { get; set; }

        public User ModifiedBy { get; set; }

        public abstract DateTime Created { get; }

        public abstract DateTime Modified { get; }

        public IDictionary<string, string> MetaData
        {
            get
            {
                return this.m_metaData;
            }
            set
            {
                this.m_metaData = value;
            }
        }

        public string Name { get; set; }

        public string ServerRelativePath
        {
            get
            {
                if (this.Parent == null)
                    return "";
                return this.Parent.ServerRelativePath + "/" + this.Name;
            }
        }

        public string RelativeFilePath
        {
            get
            {
                if (this.OriginalParent == null)
                    return "";
                return (this.OriginalParent.RelativeFilePath + "\\" + this.OriginalName).TrimStart('\\');
            }
        }
    }
}
