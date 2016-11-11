// Decompiled with JetBrains decompiler
// Type: OrbitOne.SharePoint.Importer.Domain.DefaultValidator
// Assembly: OrbitOne.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\OrbitOne.SharePoint.Importer.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace OrbitOne.SharePoint.Importer.Domain
{
    public class DefaultValidator : IImportValidator
    {
        public long MaximumFileSize { get; set; }

        public IList<string> BlockedFileExtensions { get; set; }

        public int MaximumFileNameLength { get; set; }

        public int MaximumRelativePathLength { get; set; }

        public char[] IllegalCharacters { get; set; }

        public bool DocumentLibraryExists { get; set; }

        public bool IsValid
        {
            get
            {
                return this.DocumentLibraryExists;
            }
        }

        public DefaultValidator(long maximumFileSize, IEnumerable<string> blockedFileExtensions)
        {
            this.IllegalCharacters = new char[0];
            this.MaximumFileNameLength = 123;
            this.MaximumRelativePathLength = 260;
            this.MaximumFileSize = maximumFileSize;
            this.BlockedFileExtensions = (IList<string>)blockedFileExtensions.ToList<string>();
        }

        public DefaultValidator()
          : this(long.MaxValue, Enumerable.Empty<string>())
        {
        }

        public ValidationResult Validate(ImportFile file)
        {
            ValidationResult validationResult = new ValidationResult()
            {
                Source = file.OriginalFullName
            };
            if (this.ExtensionIsBlocked(file.Extension))
                validationResult.AddError("extension is blocked");
            if (!this.NameIsValid(file.Name))
                validationResult.AddWarning("Filename is invalid");
            if (file.Size > this.MaximumFileSize)
                validationResult.AddError("File is too big");
            if (file.ServerRelativePath.Length > this.MaximumRelativePathLength)
                validationResult.AddWarning("File path is too long");
            return validationResult;
        }

        private bool ExtensionIsBlocked(string extension)
        {
            return this.BlockedFileExtensions.Any<string>((Func<string, bool>)(ext => ext == extension));
        }

        private bool NameIsValid(string name)
        {
            if (this.CharactersAreValid(name) && name.Length <= this.MaximumFileNameLength && (!name.StartsWith(".") && !name.EndsWith(".")))
                return !name.Contains("..");
            return false;
        }

        public ValidationResult Validate(ImportFolder directory)
        {
            ValidationResult validationResult = new ValidationResult()
            {
                Source = directory.OriginalFullName
            };
            if (!this.NameIsValid(directory.Name))
                validationResult.AddWarning("Directory name is invalid");
            if (directory.ServerRelativePath.Length > this.MaximumRelativePathLength)
                validationResult.AddError("Full directory name is too long. Maximum length is " + (object)this.MaximumRelativePathLength);
            if (directory.Name == "bin")
                validationResult.AddError("Directory name is blocked: " + directory.Name);
            return validationResult;
        }

        private bool CharactersAreValid(string filename)
        {
            return filename.IndexOfAny(this.IllegalCharacters) == -1;
        }
    }
}
