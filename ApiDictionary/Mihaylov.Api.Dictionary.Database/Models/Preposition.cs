namespace Mihaylov.Api.Dictionary.Database.Models
{
    public class Preposition
    {
        public int PrepositionId { get; set; }

        public int LanguageId { get; set; }

        public string Value { get; set; }

        public Language Language { get; set; }
    }
}
