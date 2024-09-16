namespace Mihaylov.Api.Site.Contracts.Models
{
    public class PersonLocation
    {
        public const int RegionMaxLength = 100;
        public const int CityMaxLength = 50;
        public const int DetailsMaxLength = 250;

        public long Id { get; set; }

        public int? CountryStateId { get; set; }

        public string CountryState { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        public string Details { get; set; }
    }
}
