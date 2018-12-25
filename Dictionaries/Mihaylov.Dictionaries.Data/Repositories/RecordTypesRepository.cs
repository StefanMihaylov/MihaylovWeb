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
