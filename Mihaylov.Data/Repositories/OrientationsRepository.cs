using System.Collections.Generic;
using System.Linq;
using Mihaylov.Common.Base;
using Mihaylov.Data.Models.Interfaces;
using Mihaylov.Data.Models.Repositories;
using Mihaylov.Database.Models.Interfaces;
using DAL = Mihaylov.Database;

namespace Mihaylov.Data.Repositories
{
    public class OrientationsRepository: GenericRepository<DAL.OrientationType, IMihaylovDbContext>, IGetAllRepository<Orientation>
    {
        public OrientationsRepository(IMihaylovDbContext context) 
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
