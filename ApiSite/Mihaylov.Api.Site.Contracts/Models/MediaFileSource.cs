namespace Mihaylov.Api.Site.Contracts.Models
{
    public class MediaFileSource
    {
        public const int NameMaxLength = 50;

        public int Id { get; set; }

        public string Name { get; set; }
    }
}
