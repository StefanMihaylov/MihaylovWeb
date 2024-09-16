namespace Mihaylov.Api.Site.Database.Models
{
    public class MediaFileExtension
    {
        public int ExtensionId {  get; set; }
        
        public string Name { get; set; }

        public bool IsImage { get; set; }
    }
}
