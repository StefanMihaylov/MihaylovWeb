using System.Collections.Generic;
using System.Linq;
using Mihaylov.Common.Database;
using Mihaylov.Common.Mapping;
using Mihaylov.Site.Data.Interfaces;
using Mihaylov.Site.Data.Models;
using Mihaylov.Site.Database.Interfaces;
using DAL = Mihaylov.Site.Database;

namespace Mihaylov.Site.Data.Repositories
{
    public class PhrasesRepository : GenericRepository<DAL.Phras, ISiteDbContext>, IPhrasesRepository
    {
        public PhrasesRepository(ISiteDbContext context)
            : base(context)
        {
        }

        public IEnumerable<Phrase> GetAll()
        {
            IEnumerable<Phrase> phrases = this.All()
                                              .OrderBy(p => p.OrderId)
                                              .To<Phrase>()
                                              .AsQueryable();
            return phrases;
        }

        public Phrase GetById(int id)
        {
            Phrase phrase = this.All()
                                .Where(p => p.PhraseId == id)
                                .To<Phrase>()
                                .FirstOrDefault();
            return phrase;
        }

        public Phrase AddOrUpdatePhrase(Phrase inputPhrase, out bool isNew)
        {
            DAL.Phras phrase;

            if (inputPhrase.Id == 0)
            {
                phrase = new DAL.Phras();
                this.Add(phrase);
                isNew = true;
            }
            else
            {
                phrase = base.GetById(inputPhrase.Id);
                isNew = false;
            }

            inputPhrase.Update(phrase);

            this.Context.SaveChanges();

            Phrase phraseDTO = this.GetById(phrase.PhraseId);
            return phraseDTO;
        }
    }
}
