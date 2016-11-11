// Decompiled with JetBrains decompiler
// Type: OrbitOne.SharePoint.Importer.FileSystem.XmlUserMapper
// Assembly: OrbitOne.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\OrbitOne.SharePoint.Importer.exe

using OrbitOne.SharePoint.Importer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace OrbitOne.SharePoint.Importer.FileSystem
{
    public class XmlUserMapper : IUserMapper
    {
        private IDictionary<string, User> m_mapping;

        public XmlUserMapper(string filename)
        {
            XDocument.Load(filename).Elements((XName)"UserMapping").ToDictionary<XElement, string, User>((Func<XElement, string>)(map => map.Element((XName)"username").Value.ToUpperInvariant()), (Func<XElement, User>)(map => new User()
            {
                Id = Convert.ToInt32(map.Element((XName)"userId").Value),
                Name = map.Element((XName)"username").Value
            }));
        }

        public User Map(string username)
        {
            return this.m_mapping[username.ToUpperInvariant()];
        }
    }
}
