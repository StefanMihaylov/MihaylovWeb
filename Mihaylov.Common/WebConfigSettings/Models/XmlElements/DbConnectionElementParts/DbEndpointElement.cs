namespace Mihaylov.Common.WebConfigSettings.Models.XmlElements.DbConnectionElementParts
{
    using System;
    using System.Configuration;
    using Interfaces.Models;

    public class DbEndpointElement : ConfigurationElement, IDbEndpoint
    {
        // <endpoint IP="10.10.10.10" port="4445" dbName="MPSS1"/>

        public const string IP_ATTRIBUTE = "IPAddress";
        public const string PORT_ATTRIBUTE = "port";
        public const string DB_NAME_ATTRIBUTE = "dbName";


        [ConfigurationProperty(IP_ATTRIBUTE, IsRequired = true)]
        public string IPAdress
        {
            get { return (string)this[IP_ATTRIBUTE]; }
        }

        [ConfigurationProperty(PORT_ATTRIBUTE, IsRequired = false)]
        public int? Port
        {
            get { return (int?)this[PORT_ATTRIBUTE]; }
        }

        [ConfigurationProperty(DB_NAME_ATTRIBUTE, IsRequired = true)]
        public string DbName
        {
            get { return (string)this[DB_NAME_ATTRIBUTE]; }
        }
    }
}
