using System;

namespace SharepointFileTransfer.SharePoint.Importer.Domain
{
    public class ItemProcessedEventArgs : EventArgs
    {
        public ImportItem Item { get; private set; }

        public string Location { get; set; }

        public ItemProcessedEventArgs(ImportItem item, string location)
        {
            this.Item = item;
            this.Location = location;
        }
    }
}
