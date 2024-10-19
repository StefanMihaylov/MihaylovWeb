namespace Mihaylov.Api.Site.Contracts.Models
{
    public class PersonStatisticsPairGeneric<T>
    {
        public T Key { get; set; }

        public int Value { get; set; }
    }
}
