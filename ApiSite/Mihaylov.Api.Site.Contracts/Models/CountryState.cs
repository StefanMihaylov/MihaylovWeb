namespace Mihaylov.Api.Site.Contracts.Models
{
    public class CountryState
    {
        public const int NameMaxLength = 50;

        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int CountryId { get; set; }
    }
}
