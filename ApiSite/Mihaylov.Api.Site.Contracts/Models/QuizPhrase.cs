namespace Mihaylov.Api.Site.Contracts.Models
{
    public class QuizPhrase
    {
        public const int TextMaxLength = 200;

        public int Id { get; set; }

        public string Text { get; set; }

        public int? OrderId { get; set; }
    }
}
