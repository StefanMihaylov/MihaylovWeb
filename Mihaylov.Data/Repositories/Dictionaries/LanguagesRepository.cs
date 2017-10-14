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
