namespace Mihaylov.Common.WebConfigSettings.Models.XmlElements.DbConnectionElementParts
{
    using System.Configuration;

    public class DbConnectionElement : ConfigurationElement
    {
        /*
         <add name="MPSS_Prod">
            <endpoint IP="10.10.10.10" port="4445" dbName="MPSS1"/>
            <credential username="Pesho" cipheredPassword="asdsdijw8ejxuhuxhwuh"/>
         </add>
       */

        public const string NAME_ATTRIBUTE = "name";
        public const string ENDPOINT_ELEMENT = "endpoint";
        public const string CREDENTIAL_ELEMENT = "credential";

        [ConfigurationProperty(NAME_ATTRIBUTE, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this[NAME_ATTRIBUTE]; }
        }

        [ConfigurationProperty(ENDPOINT_ELEMENT, IsRequired = true)]
        public DbEndpointElement Endpoint
        {
            get { return (DbEndpointElement)this[ENDPOINT_ELEMENT]; }
        }

        [ConfigurationProperty(CREDENTIAL_ELEMENT, IsRequired = true)]
        public DbCredentialElement Credential
        {
            get { return (DbCredentialElement)this[CREDENTIAL_ELEMENT]; }
        }
    }
}
