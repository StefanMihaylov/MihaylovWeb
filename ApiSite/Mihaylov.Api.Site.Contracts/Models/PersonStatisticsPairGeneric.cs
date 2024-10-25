namespace Mihaylov.Api.Site.Contracts.Models
{
    public class PersonStatisticsPairGeneric<T>
    {
        public int? Id { get; set; }

        public int? TypeId { get; set; }

        public T Key { get; set; }

        public int Value { get; set; }
    }
}
