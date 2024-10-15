namespace Mihaylov.Api.Site.Contracts.Models
{
    public class Ethnicity
    {
        public const int NameMaxLength = 30;
        public const int OtherNameMaxLength = 100;

        public int Id { get; set; }

        public string Name { get; set; }

        public string OtherNames { get; set; }
    }
}
