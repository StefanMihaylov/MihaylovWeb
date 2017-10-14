using System.Collections.Generic;
using System.Linq;
using Mihaylov.Common.Database;
using Mihaylov.Common.Mapping;
using Mihaylov.Data.Interfaces.Dictionaries;
using Mihaylov.Data.Models.Dictionaries;
using Mihaylov.Database.Interfaces;
using DAL = Mihaylov.Database.Dictionaries;

namespace Mihaylov.Data.Repositories.Dictionaries
{
    public class LevelsRepository : GenericRepository<DAL.Level, IDictionariesDbContext>, IGetAllRepository<Level>
    {
        public LevelsRepository(IDictionariesDbContext context) 
            : base(context)
        {
        }

        public IEnumerable<Level> GetAll()
        {
            IEnumerable<Level> level = this.All()
                                           .To<Level>()
                                           .AsQueryable();
            return level;
        }
    }
}
