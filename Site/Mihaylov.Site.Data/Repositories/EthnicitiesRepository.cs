using System.Collections.Generic;
using System.Linq;
using Mihaylov.Common.Database;
using Mihaylov.Site.Data.Interfaces;
using Mihaylov.Site.Data.Models;
using Mihaylov.Site.Database.Interfaces;
using DAL = Mihaylov.Site.Database;

namespace Mihaylov.Site.Data.Repositories
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
