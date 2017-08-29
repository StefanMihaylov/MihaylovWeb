using System.Collections.Generic;
using System.Linq;
using Mihaylov.Common.Database;
using Mihaylov.Data.Models.Interfaces;
using Mihaylov.Data.Models.Repositories;
using Mihaylov.Database.Models.Interfaces;
using DAL = Mihaylov.Database;

namespace Mihaylov.Data.Repositories
{
    public class EthnicitiesRepository : GenericRepository<DAL.EthnicityType, IMihaylovDbContext>, IGetAllRepository<Ethnicity>
    {
        public EthnicitiesRepository(IMihaylovDbContext context) 
            : base(context)
        {
        }

        public IEnumerable<Ethnicity> GetAll()
        {
            IEnumerable<Ethnicity> ethnicities = this.All()
                                                     .Select(Ethnicity.FromDb)
                                                     .AsQueryable();
            return ethnicities;
        }
    }
}
