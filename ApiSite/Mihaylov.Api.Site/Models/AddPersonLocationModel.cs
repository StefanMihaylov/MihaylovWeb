namespace Mihaylov.Api.Site.Models
{
    public class AddPersonLocationModel
    {
        public int? CountryStateId { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        public string Details { get; set; }


        public bool IsEmpty()
        {
            return !CountryStateId.HasValue && string.IsNullOrEmpty(Region) &&
                string.IsNullOrEmpty(City) && string.IsNullOrEmpty(Details);
        }
    }
}
