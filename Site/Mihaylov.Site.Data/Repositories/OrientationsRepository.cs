using System.Collections.Generic;
using System.Linq;
using Mihaylov.Common.Database;
using Mihaylov.Site.Data.Interfaces;
using Mihaylov.Site.Data.Models;
using Mihaylov.Site.Database.Interfaces;
using DAL = Mihaylov.Site.Database;

namespace Mihaylov.Site.Data.Repositories
{
    public class OrientationsRepository: GenericRepository<DAL.OrientationType, ISiteDbContext>, IGetAllRepository<Orientation>
    {
        public OrientationsRepository(ISiteDbContext context) 
            : base(context)
        {
        }

        public IEnumerable<Orientation> GetAll()
        {
            IEnumerable<Orientation> preference = this.All()
                                                        .Select(Orientation.FromDb)
                                                        .AsQueryable();
            return preference;
        }
    }
}
