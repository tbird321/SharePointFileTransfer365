using System.Collections.Generic;

namespace SharepointFileTransfer.SharePoint.Importer.Domain
{
    public class ImportFileSystem
    {
        private IEnumerable<ImportItem> m_items;

        public IEnumerable<ImportItem> Items
        {
            get
            {
                return this.m_items;
            }
        }
    }
}
