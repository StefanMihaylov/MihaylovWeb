using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mihaylov.Common.Base;
using Mihaylov.Data.Models.Interfaces;
using Mihaylov.Data.Models.Repositories;
using Mihaylov.Database.Models.Interfaces;
using DAL = Mihaylov.Database;

namespace Mihaylov.Data.Repositories
{
    public class AnswerTypeRepository : GenericRepository<DAL.AnswerType, IMihaylovDbContext>, IGetAllRepository<AnswerType>
    {
        public AnswerTypeRepository(IMihaylovDbContext context) 
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
