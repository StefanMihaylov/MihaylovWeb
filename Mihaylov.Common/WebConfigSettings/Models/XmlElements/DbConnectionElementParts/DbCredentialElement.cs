namespace Mihaylov.Common.WebConfigSettings.Models.XmlElements.DbConnectionElementParts
{
    using System.Configuration;
    using Interfaces.Models;

    public class DbCredentialElement : ConfigurationElement, IDbCredential
    {
        // <credential username="Pesho" cipheredPassword="asdsdijw8ejxuhuxhwuh"/>

        public const string USER_NAME_ATTRIBUTE = "username";
        public const string CIPHERED_PASSWORD_ATTRIBUTE = "cipheredPassword";

        [ConfigurationProperty(USER_NAME_ATTRIBUTE, IsRequired = true)]
        public string UserName
        {
            get { return (string)this[USER_NAME_ATTRIBUTE]; }
        }

        [ConfigurationProperty(CIPHERED_PASSWORD_ATTRIBUTE, IsRequired = true)]
        public string CipheredPassword
        {
            get { return (string)this[CIPHERED_PASSWORD_ATTRIBUTE]; }
        }
    }
}
