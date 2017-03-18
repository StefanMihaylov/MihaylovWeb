namespace Mihaylov.Common.WebConfigSettings.Models.XmlElements
{
    using System.Configuration;

    public class EndpointElement : ConfigurationElement
    {
        // <endpoint name="A51_Prod" IP="10.10.10.10" port="4445"/>

        public const string NAME_ATTRIBUTE = "name";
        public const string IP_ATTRIBUTE = "IPAddress";
        public const string PORT_ATTRIBUTE = "port";

        [ConfigurationProperty(NAME_ATTRIBUTE, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this[NAME_ATTRIBUTE]; }
        }

        [ConfigurationProperty(IP_ATTRIBUTE, IsRequired = true)]
        public string IPAddress
        {
            get { return (string)this[IP_ATTRIBUTE]; }
        }

        [ConfigurationProperty(PORT_ATTRIBUTE, IsRequired = false)]
        [IntegerValidator(MinValue = 0, MaxValue = 65535, ExcludeRange = false)]
        public int Port
        {
            get { return (int)this[PORT_ATTRIBUTE]; }
        }
    }
}
