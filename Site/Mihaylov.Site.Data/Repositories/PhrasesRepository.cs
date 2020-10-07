using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Common.Mapping;
using Mihaylov.Site.Data.Interfaces;
using Mihaylov.Site.Data.Models;
using Mihaylov.Site.Database;
using DAL = Mihaylov.Site.Database.Models;

namespace Mihaylov.Site.Data.Repositories
{
    public class PhrasesRepository : IPhrasesRepository
    {
        private readonly SiteDbContext _context;

        public PhrasesRepository(SiteDbContext context)
        {
            this._context = context;
        }


        public async Task<IEnumerable<Phrase>> GetAllAsync()
        {
            var phrases = await this._context.Phrases
                                             .OrderBy(p => p.OrderId)
                                             .To<Phrase>()
                                             .ToListAsync()
                                             .ConfigureAwait(false);
            return phrases;
        }

        public async Task<Phrase> GetByIdAsync(int id)
        {
            Phrase phrase = await this._context.Phrases
                                .Where(p => p.Id == id)
                                .To<Phrase>()
                                .FirstOrDefaultAsync()
                                .ConfigureAwait(false);
            return phrase;
        }

        public async Task<Phrase> AddOrUpdatePhraseAsync(Phrase inputPhrase)
        {
            DAL.Phrase phrase;
            bool isNew;

            if (inputPhrase.Id == 0)
            {
                phrase = new DAL.Phrase();
                this._context.Phrases.Add(phrase);
                isNew = true;
            }
            else
            {
                phrase = await this._context.Phrases.Where(a => a.Id == inputPhrase.Id)
                                                    .FirstOrDefaultAsync()
                                                    .ConfigureAwait(false);
                isNew = false;
            }

            inputPhrase.Update(phrase);

            await this._context.SaveChangesAsync().ConfigureAwait(false);

            Phrase phraseDTO = await this.GetByIdAsync(phrase.Id).ConfigureAwait(false);
            return phraseDTO;
        }

        public async Task<int> GetMaxOrderId()
        {
            int maxOrderId = await this._context.Phrases
                                                .Select(p => p.OrderId)
                                                .DefaultIfEmpty()
                                                .MaxAsync()
                                                .ConfigureAwait(false);

            return maxOrderId;
        }
    }
}
