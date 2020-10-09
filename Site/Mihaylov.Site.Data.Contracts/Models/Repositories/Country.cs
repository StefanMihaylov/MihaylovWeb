using Mihaylov.Common.Mapping;
using Mihaylov.Site.Data.Models.Base;
using DAL = Mihaylov.Site.Database.Models;

namespace Mihaylov.Site.Data.Models
{
    public class Country : LookupTable, IMapFrom<DAL.Country>, IMapTo<DAL.Country>
    {
        public string CountryCode { get; set; }
    }
}
