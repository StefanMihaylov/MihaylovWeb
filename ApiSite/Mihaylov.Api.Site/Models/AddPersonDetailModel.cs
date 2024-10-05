namespace Mihaylov.Api.Site.Models
{
    public class AddPersonDetailModel
    {
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string OtherNames { get; set; }

        
        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(MiddleName) &&
                   string.IsNullOrEmpty(LastName) && string.IsNullOrEmpty(OtherNames);
        }
    }
}
