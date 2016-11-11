// Decompiled with JetBrains decompiler
// Type: OrbitOne.SharePoint.Importer.SharePoint.SharePointUserRepository
// Assembly: OrbitOne.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\OrbitOne.SharePoint.Importer.exe

using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace OrbitOne.SharePoint.Importer.SharePoint
{
    public class SharePointUserRepository
    {
        private string m_url;

        public SharePointUserRepository(string url)
        {
            this.m_url = url;
        }

        public IList<OrbitOne.SharePoint.Importer.Domain.User> GetUsers()
        {
            using (ClientContext clientContext = new ClientContext(this.m_url))
            {
                ListItemCollection items = clientContext.Web.SiteUserInfoList.GetItems(new CamlQuery()
                {
                    ViewXml = "<View/>"
                });
                clientContext.Load<ListItemCollection>(items, new Expression<Func<ListItemCollection, object>>[0]);
                clientContext.ExecuteQuery();
                return (IList<OrbitOne.SharePoint.Importer.Domain.User>)items.Select<ListItem, OrbitOne.SharePoint.Importer.Domain.User>((Expression<Func<ListItem, OrbitOne.SharePoint.Importer.Domain.User>>)(user => new OrbitOne.SharePoint.Importer.Domain.User()
                {
                    Id = Convert.ToInt32(user["ID"]),
                    Name = user["Name"].ToString()
                })).ToList<OrbitOne.SharePoint.Importer.Domain.User>();
            }
        }
    }
}
