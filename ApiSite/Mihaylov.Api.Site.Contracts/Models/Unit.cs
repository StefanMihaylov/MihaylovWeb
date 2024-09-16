namespace Mihaylov.Api.Site.Contracts.Models
{
    public class Unit
    {
        public const int NameMaxLength = 20;

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal? ConversionRate { get; set; }

        public int? BaseUnitId { get; set; }
    }
}
