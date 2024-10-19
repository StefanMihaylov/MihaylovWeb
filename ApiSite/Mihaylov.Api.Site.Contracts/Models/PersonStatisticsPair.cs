namespace Mihaylov.Api.Site.Contracts.Models
{
    public class PersonStatisticsPair
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public PersonStatisticsPair()
        {                
        }

        public PersonStatisticsPair(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
