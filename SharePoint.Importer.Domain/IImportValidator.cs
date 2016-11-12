// Decompiled with JetBrains decompiler
// Type: SharepointFileTransfer.SharePoint.Importer.Domain.IImportValidator
// Assembly: SharepointFileTransfer.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\SharepointFileTransfer.SharePoint.Importer.exe

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
