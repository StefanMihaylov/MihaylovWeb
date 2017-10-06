using System.Collections.Generic;
using System.Linq;
using Mihaylov.Common.Database;
using Mihaylov.Data.Interfaces.Site;
using Mihaylov.Data.Models.Site;
using Mihaylov.Database.Interfaces;
using DAL = Mihaylov.Database.Site;

namespace Mihaylov.Data.Repositories.Site
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
