namespace Mihaylov.Api.Site.Contracts.Models
{
    public class PersonDetail
    {
        public const int NameMaxLength = 50;
        public const int OtherNamesMaxLength = 200;

        public long Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string OtherNames { get; set; }
    }
}
