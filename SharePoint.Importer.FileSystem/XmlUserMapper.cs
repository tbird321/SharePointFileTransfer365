using SharepointFileTransfer.SharePoint.Importer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SharepointFileTransfer.SharePoint.Importer.FileSystem
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
