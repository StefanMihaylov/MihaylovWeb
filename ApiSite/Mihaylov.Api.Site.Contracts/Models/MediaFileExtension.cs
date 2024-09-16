namespace Mihaylov.Api.Site.Contracts.Models
{
    public class MediaFileExtension
    {
        public const int NameMaxLength = 10;

        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsImage { get; set; }
    }
}
