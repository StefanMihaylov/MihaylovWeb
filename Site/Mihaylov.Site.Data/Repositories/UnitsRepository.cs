using System.Collections.Generic;
using System.Linq;
using Mihaylov.Common.Database;
using Mihaylov.Site.Data.Interfaces;
using Mihaylov.Site.Data.Models;
using Mihaylov.Site.Database.Interfaces;
using DAL = Mihaylov.Site.Database.Models;

namespace Mihaylov.Site.Data.Repositories
{
    public class UnitsRepository : GenericRepository<DAL.UnitType, ISiteDbContext>, IGetAllRepository<Unit>
    {
        public UnitsRepository(ISiteDbContext context) 
            : base(context)
        {
        }

        public IEnumerable<Unit> GetAll()
        {
            IEnumerable<Unit> units = this.All()
                                          .Select(Unit.FromDb)
                                          .AsQueryable();
            return units;
        }
    }
}