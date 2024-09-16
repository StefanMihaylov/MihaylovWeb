namespace Mihaylov.Api.Site.Database.Models
{
    public class PersonDetail
    {
        public long PersonId { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string OtherNames { get; set; }

        public Person Person { get; set; }
    }
}
