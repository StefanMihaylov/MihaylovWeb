using Google.Apis.Drive.v3.Data;
namespace Mihaylov.GoogleDrive.Models
{
    public class PhotoInfo
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public long? Size { get; set; }

        public string MimeType { get; set; }


        public static implicit operator PhotoInfo(File file)
        {
            if (file == null)
            {
                return null;
            }

           // string thumbNailUrl = file.ThumbnailLink;

            string url = string.Empty;
            string link = file.WebContentLink;
            if (!string.IsNullOrWhiteSpace(link))
            {
                int index = link.LastIndexOf("&");
                url = link.Substring(0, index);
            }

            var result = new PhotoInfo()
            {
                Id = file.Id,
                Name = file.Name,
                Url = url,
                Size = file.Size,
                MimeType = file.MimeType,
            };

            return result;
        }
    }
}
