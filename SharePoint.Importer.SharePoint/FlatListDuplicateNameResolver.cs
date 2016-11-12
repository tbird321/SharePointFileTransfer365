using SharepointFileTransfer.SharePoint.Importer.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharepointFileTransfer.SharePoint.Importer.SharePoint
{
    public class FlatListDuplicateNameResolver
    {
        private readonly IList<string> m_filenames;

        public FlatListDuplicateNameResolver()
          : this(Enumerable.Empty<string>())
        {
        }

        public FlatListDuplicateNameResolver(IEnumerable<string> existingFiles)
        {
            this.m_filenames = (IList<string>)existingFiles.ToList<string>();
        }

        public string ResolveName(ImportFile file)
        {
            int num = 0;
            IEnumerable<string> source = this.m_filenames.Where<string>((Func<string, bool>)(name => name.StartsWith(Path.GetFileNameWithoutExtension(file.Name), StringComparison.InvariantCultureIgnoreCase)));
            string uniqueName = file.Name;
            string withoutExtension = Path.GetFileNameWithoutExtension(file.Name);
            string extension = Path.GetExtension(file.Name);
            while (source.Any<string>((Func<string, bool>)(s => s.Equals(uniqueName, StringComparison.InvariantCultureIgnoreCase))))
            {
                ++num;
                uniqueName = string.Format("{0}_{1}{2}", (object)withoutExtension, (object)num, (object)extension);
            }
            this.m_filenames.Add(uniqueName);
            return uniqueName;
        }
    }
}
