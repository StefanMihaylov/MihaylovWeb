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
    public class CoursesRepository : GenericRepository<DAL.Course, IDictionariesDbContext>, IGetAllRepository<Course>
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
