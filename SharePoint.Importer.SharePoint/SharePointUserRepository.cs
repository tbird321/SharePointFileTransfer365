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
