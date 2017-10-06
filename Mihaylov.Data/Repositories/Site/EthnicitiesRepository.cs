using System.Collections.Generic;
using System.Linq;
using Mihaylov.Common.Database;
using Mihaylov.Data.Interfaces.Site;
using Mihaylov.Data.Models.Site;
using Mihaylov.Database.Interfaces;
using DAL = Mihaylov.Database.Site;

namespace Mihaylov.Data.Repositories.Site
{
    public class EthnicitiesRepository : GenericRepository<DAL.EthnicityType, ISiteDbContext>, IGetAllRepository<Ethnicity>
    {
        public EthnicitiesRepository(ISiteDbContext context) 
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
