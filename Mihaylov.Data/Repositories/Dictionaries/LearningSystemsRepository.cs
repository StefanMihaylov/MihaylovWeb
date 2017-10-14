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
