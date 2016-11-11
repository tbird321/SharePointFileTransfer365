// Decompiled with JetBrains decompiler
// Type: OrbitOne.SharePoint.Importer.Domain.IImportValidator
// Assembly: OrbitOne.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\OrbitOne.SharePoint.Importer.exe

namespace OrbitOne.SharePoint.Importer.Domain
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
