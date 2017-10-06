using System.Collections.Generic;
using System.Linq;
using Mihaylov.Common.Database;
using Mihaylov.Data.Interfaces.Site;
using Mihaylov.Data.Models.Site;
using Mihaylov.Database.Interfaces;
using DAL = Mihaylov.Database.Site;

namespace Mihaylov.Data.Repositories.Site
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