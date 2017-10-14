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
    public class CoursesRepository : GenericRepository<DAL.Cours, IDictionariesDbContext>, IGetAllRepository<Course>
    {
        public CoursesRepository(IDictionariesDbContext context) 
            : base(context)
        {
        }

        public IEnumerable<Course> GetAll()
        {
            IEnumerable<Course> course = this.All()
                                             .To<Course>()
                                             .AsQueryable();
            return course;
        }
    }
}
