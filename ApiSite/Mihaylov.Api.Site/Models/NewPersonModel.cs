namespace Mihaylov.Api.Site.Models
{
    public class NewPersonModel
    {
        public int? AccountTypeId { get; set; }

        public int? StatusId { get; set; }

        public string Username { get; set; }

        public bool? IsPreview { get; set; }
    }
}
