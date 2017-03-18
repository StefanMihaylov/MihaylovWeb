namespace Mihaylov.Common.WebConfigSettings.Models.XmlElements
{
    using System.Collections.Generic;
    using System.Configuration;

    [ConfigurationCollection(typeof(EndpointElement))]
    public class EndpointCollection : ConfigurationElementCollection, IEnumerable<EndpointElement>
    {
        public new IEnumerator<EndpointElement> GetEnumerator()
        {
            int count = this.Count;
            for (int i = 0; i < count; i++)
            {
                yield return this.BaseGet(i) as EndpointElement;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new EndpointElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EndpointElement)element).Name;
        }
    }
}
