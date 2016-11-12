namespace SharepointFileTransfer.SharePoint.Importer.Domain
{
    public interface IImportSource
    {
        ImportItem LoadItems();
    }
}
