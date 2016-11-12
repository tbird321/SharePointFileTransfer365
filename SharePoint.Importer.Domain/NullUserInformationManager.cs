using System.Collections.Generic;

namespace SharepointFileTransfer.SharePoint.Importer.Domain
{
    public class NullUserInformationManager : IMetaDataProvider
    {
        public User GetAuthor(string filename)
        {
            return (User)null;
        }

        public User GetEditor(string filename)
        {
            return (User)null;
        }

        public IDictionary<string, string> GetMetaData(string filename)
        {
            return (IDictionary<string, string>)new Dictionary<string, string>();
        }
    }
}
