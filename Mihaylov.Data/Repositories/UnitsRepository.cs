using System.Collections.Generic;
using System.Linq;
using Mihaylov.Common.Base;
using Mihaylov.Data.Models.Interfaces;
using Mihaylov.Data.Models.Repositories;
using Mihaylov.Database.Models.Interfaces;
using DAL = Mihaylov.Database;

namespace Mihaylov.Data.Repositories
{
    public class UnitsRepository : GenericRepository<DAL.UnitType, IMihaylovDbContext>, IGetAllRepository<Unit>
    {
        public UnitsRepository(IMihaylovDbContext context) 
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