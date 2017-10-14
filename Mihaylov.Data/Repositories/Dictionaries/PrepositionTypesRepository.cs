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
    public class PrepositionTypesRepository : GenericRepository<DAL.PrepositionType, IDictionariesDbContext>, IGetAllRepository<PrepositionType>
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
