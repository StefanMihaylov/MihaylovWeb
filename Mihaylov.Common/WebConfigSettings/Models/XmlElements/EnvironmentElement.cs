namespace Mihaylov.Common.WebConfigSettings.Models.XmlElements
{
    using System.Configuration;

    public class EnvironmentElement : ConfigurationElement
    {
        public const string NAME_ATTRIBUTE = "name";

        [ConfigurationProperty(NAME_ATTRIBUTE, IsRequired = true)]
        public string Name
        {
            get { return (string)this[NAME_ATTRIBUTE]; }
        }
    }
}
