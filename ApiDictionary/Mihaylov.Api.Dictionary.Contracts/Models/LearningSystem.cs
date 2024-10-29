namespace Mihaylov.Api.Dictionary.Contracts.Models
{
    public class LearningSystem
    {
        public const int NameMaxLength = 50;

        public int Id { get; set; }

        public string Name { get; set; }

        public int LanguageId { get; set; }

        public string Language { get; set; }
    }
}
