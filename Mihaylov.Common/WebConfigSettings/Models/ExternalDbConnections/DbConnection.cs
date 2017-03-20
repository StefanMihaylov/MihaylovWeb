using System;
using Mihaylov.Common.WebConfigSettings.Interfaces.Models;
using Mihaylov.Common.WebConfigSettings.Models.XmlElements.DbConnectionElementParts;

namespace Mihaylov.Common.WebConfigSettings.Models.ExternalDbConnections
{
    public class DbConnection : IDbConnection
    {
        public DbConnection(DbConnectionElement connection) :
            this(connection.Name, connection.ExternalSettings, connection.IsCodeFirst, connection.Endpoint, connection.Credential)
        {
        }

        public DbConnection(string name, string externalSettings, bool? isCodeFirst, IDbEndpoint endpoint, IDbCredential credential)
        {
            this.Name = name;
            this.ExternalSettings = externalSettings;
            this.IsCodeFirst = isCodeFirst ?? false;
            this.Endpoint = endpoint;
            this.Credential = credential;
        }

        public IDbCredential Credential { get; set; }

        public IDbEndpoint Endpoint { get; set; }

        public string ExternalSettings { get; set; }

        public bool IsCodeFirst { get; set; }

        public string Name { get; set; }
    }
}
