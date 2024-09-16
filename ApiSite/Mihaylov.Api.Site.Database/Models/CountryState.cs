namespace Mihaylov.Api.Site.Database.Models
{
    public class CountryState
    {
        public int StateId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int CountryId { get; set; }        

        public virtual Country Country { get; set; }
    }
}
