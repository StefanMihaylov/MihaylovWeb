namespace Mihaylov.Api.Site.Database.Models
{
    public class PersonLocation
    {
        public long PersonId { get; set; }

        public int? CountryStateId { get; set; }

        public virtual CountryState CountryState { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        public string Details { get; set; }

        public Person Person { get; set; }
    }
}