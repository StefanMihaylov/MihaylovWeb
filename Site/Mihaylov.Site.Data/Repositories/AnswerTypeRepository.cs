using System.Collections.Generic;
using System.Linq;
using Mihaylov.Common.Database;
using Mihaylov.Site.Data.Interfaces;
using Mihaylov.Site.Data.Models;
using Mihaylov.Site.Database.Interfaces;
using DAL = Mihaylov.Site.Database;

namespace Mihaylov.Site.Data.Repositories
{
    public class AnswerTypeRepository : GenericRepository<DAL.AnswerType, ISiteDbContext>, IGetAllRepository<AnswerType>
    {
        public AnswerTypeRepository(ISiteDbContext context) 
            : base(context)
        {
        }

        public IEnumerable<AnswerType> GetAll()
        {
            IEnumerable<AnswerType> types = this.All()
                                                .Select(AnswerType.FromDb)
                                                .AsQueryable();
            return types;
        }
    }
}
