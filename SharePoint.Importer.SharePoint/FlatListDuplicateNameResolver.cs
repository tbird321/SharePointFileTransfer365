// Decompiled with JetBrains decompiler
// Type: OrbitOne.SharePoint.Importer.SharePoint.FlatListDuplicateNameResolver
// Assembly: OrbitOne.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\OrbitOne.SharePoint.Importer.exe

using OrbitOne.SharePoint.Importer.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OrbitOne.SharePoint.Importer.SharePoint
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
