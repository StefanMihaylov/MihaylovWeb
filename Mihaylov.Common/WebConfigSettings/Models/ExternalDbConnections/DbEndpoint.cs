using System;
using Mihaylov.Common.WebConfigSettings.Interfaces.Models;

namespace Mihaylov.Common.WebConfigSettings.Models.ExternalDbConnections
{
    public class DbEndpoint : IDbEndpoint
    {
        public DbEndpoint(IDbEndpoint endpoint)
            : this(endpoint.DbName, endpoint.IPAdress, endpoint.Port)
        {
        }

        public DbEndpoint(string dbName, string iPAdress, int? port = null)
        {
            this.DbName = dbName;
            this.IPAdress = iPAdress;
            this.Port = port;
        }

        public string DbName { get; set; }

        public string IPAdress { get; set; }

        public int? Port { get; set; }
    }
}
