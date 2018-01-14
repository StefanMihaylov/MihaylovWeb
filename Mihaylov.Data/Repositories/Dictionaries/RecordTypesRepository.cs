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
    public class RecordTypesRepository : GenericRepository<DAL.RecordType, IDictionariesDbContext>, IRecordTypesRepository
    {
        public RecordTypesRepository(IDictionariesDbContext context)
            : base(context)
        {
        }

        public IEnumerable<RecordType> GetAll()
        {
            IEnumerable<RecordType> records = this.All()
                                           .To<RecordType>()
                                           .AsQueryable();
            return records;
        }
    }
}
