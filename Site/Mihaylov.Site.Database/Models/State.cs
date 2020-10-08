using Mihaylov.Site.Database.Models.Base;

namespace Mihaylov.Site.Database.Models
{
    public class State : LookupTable
    {
        public int CountryId { get; set; }

        public string StateCode { get; set; }

        public virtual Country Country { get; set; }
    }
}
