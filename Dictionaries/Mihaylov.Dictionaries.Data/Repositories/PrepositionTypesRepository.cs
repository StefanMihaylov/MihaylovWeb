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
    public class PrepositionTypesRepository : GenericRepository<DAL.PrepositionType, IDictionariesDbContext>, IPrepositionTypesRepository
    {
        public PrepositionTypesRepository(IDictionariesDbContext context) 
            : base(context)
        {
        }

        public IEnumerable<PrepositionType> GetAll()
        {
            IEnumerable<PrepositionType> prepositions = this.All()
                                                            .To<PrepositionType>()
                                                            .AsQueryable();
            return prepositions;
        }
    }
}
