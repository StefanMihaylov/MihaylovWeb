using System.Collections.Generic;
using System.Linq;
using Mihaylov.Common.Database;
using Mihaylov.Data.Interfaces.Site;
using Mihaylov.Data.Models.Site;
using Mihaylov.Database.Interfaces;
using DAL = Mihaylov.Database.Site;

namespace Mihaylov.Data.Repositories.Site
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
