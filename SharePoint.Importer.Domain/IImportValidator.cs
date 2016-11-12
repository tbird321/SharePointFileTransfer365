namespace SharepointFileTransfer.SharePoint.Importer.Domain
{
    public interface IImportValidator
    {
        int MaximumFileNameLength { get; }

        char[] IllegalCharacters { get; set; }

        bool DocumentLibraryExists { get; set; }

        bool IsValid { get; }

        ValidationResult Validate(ImportFile file);

        ValidationResult Validate(ImportFolder directory);
    }
}
