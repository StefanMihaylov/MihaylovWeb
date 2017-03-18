namespace Mihaylov.Common.WebConfigSettings.Models.XmlElements
{
    using System.Collections.Generic;
    using System.Configuration;
    using DbConnectionElementParts;

    [ConfigurationCollection(typeof(DbConnectionElement))]
    public class DbConnectionCollection : ConfigurationElementCollection, IEnumerable<DbConnectionElement>
    {
        public new IEnumerator<DbConnectionElement> GetEnumerator()
        {
            int count = this.Count;
            for (int i = 0; i < count; i++)
            {
                yield return this.BaseGet(i) as DbConnectionElement;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new DbConnectionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DbConnectionElement)element).Name;
        }
    }
}
