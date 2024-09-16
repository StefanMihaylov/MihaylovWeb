namespace Mihaylov.Api.Site.Contracts.Models
{
    public class QuizQuestion
    {
        public const int ValueMaxLength = 200;

        public int Id { get; set; }

        public string Value { get; set; }
    }
}
