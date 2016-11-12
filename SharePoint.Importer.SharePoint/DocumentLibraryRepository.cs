// Decompiled with JetBrains decompiler
// Type: OrbitOne.SharePoint.Importer.SharePoint.DocumentLibraryRepository
// Assembly: OrbitOne.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\OrbitOne.SharePoint.Importer.exe

using log4net;
using Microsoft.SharePoint.Client;
using OrbitOne.SharePoint.Importer.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security;

namespace OrbitOne.SharePoint.Importer.SharePoint
{
    public class DocumentLibraryRepository : IDocumentLibraryRepository
    {
        private static ILog log = LogManager.GetLogger(typeof(DocumentLibraryRepository));
        private const bool ShouldOverwrite = false;
        private readonly ImportSettings m_settings;
        private string m_serverRelativeListUrl;
        private IDictionary<string, string> m_availableFields;
        private readonly Func<ClientContext> CreateContext;
        private bool m_Initialized;

        public string ApplicationUrl { get; set; }

        public long MaximumFileSize
        {
            get
            {
                return 209715200;
            }
        }

        public DocumentLibraryRepository(ImportSettings settings)
        {
            DocumentLibraryRepository libraryRepository = this;
            this.m_settings = settings;
            this.CreateContext = (Func<ClientContext>)(() =>
            {
                var ctx = new ClientContext(libraryRepository.m_settings.SiteUrl);
                //Provide count and pwd for connecting to the source
                var securePass = new SecureString();
                foreach (char c in settings.Password.ToCharArray()) securePass.AppendChar(c);
                ctx.Credentials = new SharePointOnlineCredentials(settings.Username, securePass);
                return ctx;
            });
            Uri uri = new Uri(settings.SiteUrl);
            this.ApplicationUrl = string.Format("{0}://{1}", (object)uri.Scheme, (object)uri.Host);
        }

        private void EnsureInitialized()
        {
            if (this.m_Initialized)
                return;
            using (ClientContext context = this.CreateContext())
            {
                this.m_availableFields = this.GetAvailableFields(context);
                this.EnsureColumn("_Source", context);
                List byTitle = context.Web.Lists.GetByTitle(this.m_settings.DocumentLibrary);
                context.Load<Web>(context.Web, new Expression<Func<Web, object>>[1]
                {
          (Expression<Func<Web, object>>) (web => web.ServerRelativeUrl)
                });
                context.Load<List>(byTitle, new Expression<Func<List, object>>[0]);
                context.Load<Folder>(byTitle.RootFolder, new Expression<Func<Folder, object>>[0]);
                context.ExecuteQuery();
                this.m_serverRelativeListUrl = byTitle.RootFolder.ServerRelativeUrl;
            }
            this.m_Initialized = true;
        }

        private void EnsureColumn(string fieldName, ClientContext context)
        {
            if (this.m_availableFields.ContainsKey(fieldName))
                return;
            Field internalNameOrTitle = context.Site.RootWeb.Fields.GetByInternalNameOrTitle(fieldName);
            context.Load<Field>(internalNameOrTitle, new Expression<Func<Field, object>>[0]);
            context.Web.Lists.GetByTitle(this.m_settings.DocumentLibrary).Fields.Add(internalNameOrTitle);
            context.ExecuteQuery();
        }

        public CreateFileResult CreateFile(ImportFile file)
        {
            this.EnsureInitialized();
            using (ClientContext context = this.CreateContext())
            {
                string str = this.ApplicationUrl + this.m_serverRelativeListUrl + file.Parent.ServerRelativePath;
                bool flag1 = false;
                bool flag2 = true;
                try
                {
                    this.CreateFile(file, context);
                    flag1 = true;
                    this.ApplyMetaData(file, context);
                }
                catch (Exception ex)
                {
                    flag2 = false;
                    DocumentLibraryRepository.log.Error((object)ex);
                    if (flag1)
                    {
                        DocumentLibraryRepository.log.Info((object)("removing " + str));
                        this.DeleteFile(file, context);
                    }
                }
                return new CreateFileResult()
                {
                    Succeeded = flag2,
                    Location = str
                };
            }
        }

        private void DeleteFile(ImportFile file, ClientContext context)
        {
            string serverRelativeUrl = this.m_serverRelativeListUrl + file.ServerRelativePath;
            context.Web.GetFileByServerRelativeUrl(serverRelativeUrl).DeleteObject();
            context.ExecuteQuery();
        }

        private void ApplyMetaData(ImportFile importFile, ClientContext context)
        {
            string serverRelativeUrl1 = this.m_serverRelativeListUrl + importFile.ServerRelativePath;
            Microsoft.SharePoint.Client.File serverRelativeUrl2 = context.Web.GetFileByServerRelativeUrl(serverRelativeUrl1);
            context.Load<Microsoft.SharePoint.Client.File>(serverRelativeUrl2, new Expression<Func<Microsoft.SharePoint.Client.File, object>>[2]
            {
        (Expression<Func<Microsoft.SharePoint.Client.File, object>>) (f => f.ListItemAllFields),
        (Expression<Func<Microsoft.SharePoint.Client.File, object>>) (f => f.ServerRelativeUrl)
            });
            ListItem listItemAllFields = serverRelativeUrl2.ListItemAllFields;
            this.MapMembers(importFile, listItemAllFields);
            listItemAllFields.Update();
            context.ExecuteQuery();
        }

        private void CreateFile(ImportFile importFile, ClientContext context)
        {
            string serverRelativeUrl = this.m_serverRelativeListUrl + importFile.ServerRelativePath;
            using (Stream stream = importFile.OpenRead())
            {
                if (this.m_settings.Mode != ImportMode.Execute)
                    return;
                DocumentLibraryRepository.log.Info((object)("Saving file to SharePoint: " + this.ApplicationUrl + serverRelativeUrl));
                Microsoft.SharePoint.Client.File.SaveBinaryDirect(context, serverRelativeUrl, stream, false);
                DocumentLibraryRepository.log.Info((object)"Succeeded");
            }
        }

        private void MapMembers(ImportFile importFile, ListItem listItem)
        {
            listItem["Created"] = (object)importFile.Created;
            listItem["Modified"] = (object)importFile.Modified;
            if (importFile.ModifiedBy != null)
                listItem["Editor"] = (object)new FieldUserValue()
                {
                    LookupId = importFile.ModifiedBy.Id
                };
            if (importFile.CreatedBy != null)
                listItem["Author"] = (object)new FieldUserValue()
                {
                    LookupId = importFile.CreatedBy.Id
                };
            listItem["_Source"] = (object)importFile.OriginalFullName;
            foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>)importFile.MetaData)
            {
                if (this.m_availableFields.ContainsKey(keyValuePair.Key))
                    listItem[keyValuePair.Key] = (object)keyValuePair.Value;
                else
                    DocumentLibraryRepository.log.Warn((object)("Could not import " + keyValuePair.Key + ". Field not found"));
            }
        }

        public void CreateFolder(ImportFolder folder)
        {
            this.EnsureInitialized();
            using (ClientContext clientContext = this.CreateContext())
            {
                string serverRelativeUrl1 = this.m_serverRelativeListUrl + folder.Parent.ServerRelativePath;
                Folder serverRelativeUrl2 = clientContext.Web.GetFolderByServerRelativeUrl(serverRelativeUrl1);
                clientContext.Load<Folder>(serverRelativeUrl2, new Expression<Func<Folder, object>>[2]
                {
          (Expression<Func<Folder, object>>) (f => f.Folders),
          (Expression<Func<Folder, object>>) (f => f.Name)
                });
                clientContext.ExecuteQuery();
                if (!serverRelativeUrl2.Folders.ToList<Folder>().Any<Folder>((Func<Folder, bool>)(f => f.Name.Equals(folder.Name))) && this.m_settings.Mode == ImportMode.Execute)
                    serverRelativeUrl2.Folders.Add(this.m_serverRelativeListUrl + folder.ServerRelativePath);
                serverRelativeUrl2.Update();
                clientContext.ExecuteQuery();
            }
        }

        public IImportValidator CreateValidator()
        {
            return (IImportValidator)new DefaultValidator()
            {
                BlockedFileExtensions = (IList<string>)Constants.BlockedFileExtensions,
                IllegalCharacters = Constants.IllegalCharacters,
                MaximumFileSize = this.MaximumFileSize,
                DocumentLibraryExists = this.DocumentLibraryExists()
            };
        }

        private bool DocumentLibraryExists()
        {
            using (ClientContext clientContext = this.CreateContext())
            {
                try
                {
                    List byTitle = clientContext.Web.Lists.GetByTitle(this.m_settings.DocumentLibrary);
                    clientContext.Load<List>(byTitle, new Expression<Func<List, object>>[1]
                    {
            (Expression<Func<List, object>>) (l => l.Title)
                    });
                    clientContext.ExecuteQuery();
                    return byTitle.Title == this.m_settings.DocumentLibrary;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        private IDictionary<string, string> GetAvailableFields(ClientContext context)
        {
            List byTitle = context.Web.Lists.GetByTitle(this.m_settings.DocumentLibrary);
            context.Load<List>(byTitle, new Expression<Func<List, object>>[1]
            {
        (Expression<Func<List, object>>) (l => l.Fields)
            });
            context.ExecuteQuery();
            return (IDictionary<string, string>)byTitle.Fields.ToDictionary<Field, string, string>((Func<Field, string>)(field => field.InternalName), (Func<Field, string>)(field => field.TypeAsString));
        }

        public IList<NameSourcePair> GetFilenamesWithSource()
        {
            this.EnsureInitialized();
            using (ClientContext clientContext = this.CreateContext())
            {

                List oList = clientContext.Web.Lists.GetByTitle(this.m_settings.DocumentLibrary);

                CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml = "<View><RowLimit>5000</RowLimit></View>";
                ListItemCollection collListItem = oList.GetItems(camlQuery);

                clientContext.Load(collListItem,
                        items => items.Include(
                            item =>item.FieldValuesAsText));

                clientContext.ExecuteQuery();
                List<NameSourcePair> nameSourcePairList = new List<NameSourcePair>();

                nameSourcePairList.AddRange((IEnumerable<NameSourcePair>)collListItem.ToList<ListItem>().Select<ListItem, NameSourcePair>((Func<ListItem, NameSourcePair>)(i => new NameSourcePair()
                {
                    Name = i.FieldValuesAsText["FileLeafRef"],
                    Source = i.FieldValuesAsText["_Source"] == null ? string.Empty : i.FieldValuesAsText["_Source"]
                })).ToList<NameSourcePair>());

                return nameSourcePairList;

            }
        }
    }
}
