namespace Mihaylov.Api.Dictionary.Database.Models
{
    public class LearningSystem
    {
        public int LearningSystemId { get; set; }

        public string Name { get; set; }

        public int LanguageId { get; set; }

        public Language Language { get; set; }
    }
}
