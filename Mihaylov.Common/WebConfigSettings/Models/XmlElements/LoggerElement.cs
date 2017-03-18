namespace Mihaylov.Common.WebConfigSettings.Models.XmlElements
{
    using System.Configuration;

    public class LoggerElement : ConfigurationElement
    {
        public const string PATH_ATTRIBUTE = "path";

        [ConfigurationProperty(PATH_ATTRIBUTE, IsRequired = true)]
        public string Path
        {
            get { return (string)this[PATH_ATTRIBUTE]; }
        }
    }
}
