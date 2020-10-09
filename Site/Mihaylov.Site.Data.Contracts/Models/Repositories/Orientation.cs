using Mihaylov.Common.Mapping;
using Mihaylov.Site.Data.Models.Base;
using DAL = Mihaylov.Site.Database.Models;

namespace Mihaylov.Site.Data.Models
{
    public class Orientation : LookupTable, IMapFrom<DAL.OrientationType>
    {

    }
}
