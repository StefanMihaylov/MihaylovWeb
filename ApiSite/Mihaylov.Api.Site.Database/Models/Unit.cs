namespace Mihaylov.Api.Site.Database.Models
{
    public class Unit
    {
        public int UnitId { get; set; }

        public string Name { get; set; }

        public decimal? ConversionRate { get; set; }

        public int? BaseUnitId { get; set; }

        public Unit BaseUnit { get; set; }
    }
}
