// Decompiled with JetBrains decompiler
// Type: SharepointFileTransfer.SharePoint.Importer.SharePoint.SharePointUserRepository
// Assembly: SharepointFileTransfer.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\SharepointFileTransfer.SharePoint.Importer.exe

using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SharepointFileTransfer.SharePoint.Importer.SharePoint
{
    public class SharePointUserRepository
    {
        private string m_url;

        public SharePointUserRepository(string url)
        {
            this.m_url = url;
        }

        public IList<SharepointFileTransfer.SharePoint.Importer.Domain.User> GetUsers()
        {
            using (ClientContext clientContext = new ClientContext(this.m_url))
            {
                ListItemCollection items = clientContext.Web.SiteUserInfoList.GetItems(new CamlQuery()
                {
                    ViewXml = "<View/>"
                });
                clientContext.Load<ListItemCollection>(items, new Expression<Func<ListItemCollection, object>>[0]);
                clientContext.ExecuteQuery();
                return (IList<SharepointFileTransfer.SharePoint.Importer.Domain.User>)items.Select<ListItem, SharepointFileTransfer.SharePoint.Importer.Domain.User>((Expression<Func<ListItem, SharepointFileTransfer.SharePoint.Importer.Domain.User>>)(user => new SharepointFileTransfer.SharePoint.Importer.Domain.User()
                {
                    Id = Convert.ToInt32(user["ID"]),
                    Name = user["Name"].ToString()
                })).ToList<SharepointFileTransfer.SharePoint.Importer.Domain.User>();
            }
        }
    }
}
