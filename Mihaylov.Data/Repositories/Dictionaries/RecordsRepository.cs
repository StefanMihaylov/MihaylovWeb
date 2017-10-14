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
    public class RecordsRepository : GenericRepository<DAL.Record, IDictionariesDbContext>, IGetAllRepository<Record>
    {
        public RecordsRepository(IDictionariesDbContext context) 
            : base(context)
        {
        }

        public IEnumerable<Record> GetAll()
        {
            IEnumerable<Record> record = this.All()
                                           .To<Record>()
                                           .AsQueryable();
            return record;
        }
    }
}
