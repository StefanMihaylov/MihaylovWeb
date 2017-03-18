namespace Mihaylov.Common.WebConfigSettings.Models.XmlElements
{
    using System.Configuration;

    public class DbTemplateElement : ConfigurationElement
    {
        // <dbConnectionTemplate name="MPSS" template="metadata=res://*/MpssDbModel.csdl|res://*/MpssDbModel.ssdl|res://*/MpssDbModel.msl;provider=Oracle.ManagedDataAccess.Client;provider connection string=&quot;DATA SOURCE={0}:{1}/{2};PASSWORD={4};USER ID={3}&quot;"/>

        public const string NAME_ATTRIBUTE = "name";
        public const string TEMPLATE_ATTRIBUTE = "template";

        [ConfigurationProperty(NAME_ATTRIBUTE, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this[NAME_ATTRIBUTE]; }
        }

        [ConfigurationProperty(TEMPLATE_ATTRIBUTE, IsRequired = true)]
        public string Template
        {
            get { return (string)this[TEMPLATE_ATTRIBUTE]; }
        }
    }
}
