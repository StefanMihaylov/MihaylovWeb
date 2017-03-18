namespace Mihaylov.Common.WebConfigSettings.Models.XmlElements
{
    using System.Collections.Generic;
    using System.Configuration;

    [ConfigurationCollection(typeof(DbTemplateElement))]
    public class DbTemplateCollection : ConfigurationElementCollection, IEnumerable<DbTemplateElement>
    {
        public new IEnumerator<DbTemplateElement> GetEnumerator()
        {
            int count = this.Count;
            for (int i = 0; i < count; i++)
            {
                yield return this.BaseGet(i) as DbTemplateElement;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new DbTemplateElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DbTemplateElement)element).Name;
        }
    }
}
