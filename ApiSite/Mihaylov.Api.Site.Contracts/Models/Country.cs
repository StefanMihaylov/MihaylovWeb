namespace Mihaylov.Api.Site.Contracts.Models
{
    public class Country
    {
        public const int NameMaxLength = 100;

        public int Id { get; set; }

        public string Name { get; set; }

        public string TwoLetterCode { get; set; }

        public string ThreeLetterCode { get; set; }

        public string AlternativeNames { get; set; }
    }
}
