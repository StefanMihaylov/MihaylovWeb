using Mihaylov.Common.Mapping;
using Mihaylov.Site.Data.Models.Base;
using DAL = Mihaylov.Site.Database.Models;

namespace Mihaylov.Site.Data.Models
{
    public class State : LookupTable, IMapFrom<DAL.State>
    {
        public int CountryId { get; set; }

        public string StateCode { get; set; }
    }    
}
