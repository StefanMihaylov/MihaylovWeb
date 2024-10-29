namespace Mihaylov.Api.Dictionary.Contracts.Models
{
    public class Preposition
    {
        public const int ValueMaxLength = 15;

        public int Id { get; set; }        

        public string Value { get; set; }

        public int LanguageId { get; set; }

        public string Language { get; set; }
    }
}
