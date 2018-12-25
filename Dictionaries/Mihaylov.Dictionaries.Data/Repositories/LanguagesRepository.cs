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
    public class LanguagesRepository : GenericRepository<DAL.Language, IDictionariesDbContext>, IGetAllRepository<Language>
    {
        public LanguagesRepository(IDictionariesDbContext context) 
            : base(context)
        {
        }

        public IEnumerable<Language> GetAll()
        {
            IEnumerable<Language> language = this.All()
                                                 .To<Language>()
                                                 .AsQueryable();
            return language;
        }
    }
}
