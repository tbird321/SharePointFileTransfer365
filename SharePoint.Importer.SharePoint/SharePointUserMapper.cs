// Decompiled with JetBrains decompiler
// Type: SharepointFileTransfer.SharePoint.Importer.SharePoint.SharePointUserMapper
// Assembly: SharepointFileTransfer.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\SharepointFileTransfer.SharePoint.Importer.exe

using SharepointFileTransfer.SharePoint.Importer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharepointFileTransfer.SharePoint.Importer.SharePoint
{
    public class SharePointUserMapper : IUserMapper
    {
        private SharePointUserRepository m_repository;
        private IDictionary<string, User> m_mappings;

        public SharePointUserMapper(string siteCollectionUrl)
        {
            this.m_repository = new SharePointUserRepository(siteCollectionUrl);
            this.m_mappings = (IDictionary<string, User>)this.m_repository.GetUsers().ToDictionary<User, string, User>((Func<User, string>)(user => user.Name.ToUpperInvariant()), (Func<User, User>)(user => user));
        }

        public User Map(string username)
        {
            return this.m_mappings[username.ToUpperInvariant()];
        }
    }
}
