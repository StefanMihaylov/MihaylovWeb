using System.Collections.Generic;
using System.Linq;
using Mihaylov.Common.Database;
using Mihaylov.Common.Mapping;
using Mihaylov.Dictionaries.Data.Interfaces;
using Mihaylov.Dictionaries.Data.Models;
using Mihaylov.Dictionaries.Database.Interfaces;
using DAL = Mihaylov.Dictionaries.Database.Models;

namespace Mihaylov.Data.Repositories.Dictionaries
{
    class LearningSystemsRepository : GenericRepository<DAL.LearningSystem, IDictionariesDbContext>, IGetAllRepository<LearningSystem>
    {
        public LearningSystemsRepository(IDictionariesDbContext context) 
            : base(context)
        {
        }

        public IEnumerable<LearningSystem> GetAll()
        {
            IEnumerable<LearningSystem> system = this.All()
                                                 .To<LearningSystem>()
                                                 .AsQueryable();
            return system;
        }
    }
}
