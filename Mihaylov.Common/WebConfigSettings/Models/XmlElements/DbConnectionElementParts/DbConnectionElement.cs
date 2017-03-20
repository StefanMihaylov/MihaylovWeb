namespace Mihaylov.Common.WebConfigSettings.Models.XmlElements.DbConnectionElementParts
{
    using System;
    using System.Configuration;
    using Interfaces.Models;

    public class DbConnectionElement : ConfigurationElement
    {
        /*
         <add name="MPSS_Prod">
            <endpoint IP="10.10.10.10" port="4445" dbName="MPSS1"/>
            <credential username="Pesho" cipheredPassword="asdsdijw8ejxuhuxhwuh"/>
         </add>
       */

        public const string NAME_ATTRIBUTE = "name";
        public const string EXTERNAL_SETTINGS_ATTRIBUTE = "externalSettings";
        public const string IS_CODE_FIRST_ATTRIBUTE = "isCodeFirst";

        public const string ENDPOINT_ELEMENT = "endpoint";
        public const string CREDENTIAL_ELEMENT = "credential";

        [ConfigurationProperty(NAME_ATTRIBUTE, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this[NAME_ATTRIBUTE]; }            
        }

        [ConfigurationProperty(EXTERNAL_SETTINGS_ATTRIBUTE, IsRequired = false)]
        public string ExternalSettings
        {
            get { return (string)this[EXTERNAL_SETTINGS_ATTRIBUTE]; }
        }

        [ConfigurationProperty(IS_CODE_FIRST_ATTRIBUTE, IsRequired = false)]
        public bool? IsCodeFirst
        {
            get { return (bool?)this[IS_CODE_FIRST_ATTRIBUTE]; }
        }

        [ConfigurationProperty(ENDPOINT_ELEMENT, IsRequired = false)]
        public DbEndpointElement Endpoint
        {
            get { return (DbEndpointElement)this[ENDPOINT_ELEMENT]; }
        }

        [ConfigurationProperty(CREDENTIAL_ELEMENT, IsRequired = false)]
        public DbCredentialElement Credential
        {
            get { return (DbCredentialElement)this[CREDENTIAL_ELEMENT]; }
        }
    }
}
