using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Site.Contracts.Repositories;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Database;
using DAL = Mihaylov.Api.Site.Database.Models;
using Mihaylov.Common.Mapping;

namespace Mihaylov.Api.Site.Data.Repositories
{
    public class QuizRepository : IQuizRepository
    {
        private readonly SiteDbContext _context;

        public QuizRepository(SiteDbContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<QuizPhrase>> GetAllAsync()
        {
            var phrases = await this._context.QuizPhrases
                                             .OrderBy(p => p.OrderId)
                                             .To<QuizPhrase>()
                                             .ToListAsync()
                                             .ConfigureAwait(false);
            return phrases;
        }

        public async Task<QuizPhrase> AddOrUpdatePhraseAsync(QuizPhrase inputPhrase)
        {
            DAL.QuizPhrase phrase;

            if (inputPhrase.Id == 0)
            {
                phrase = new DAL.QuizPhrase();
                this._context.QuizPhrases.Add(phrase);
            }
            else
            {
                phrase = await this._context.QuizPhrases.Where(a => a.PhraseId == inputPhrase.Id)
                                                    .FirstOrDefaultAsync()
                                                    .ConfigureAwait(false);
            }

           // inputPhrase.Update(phrase);

            //public void Update(DAL.Phrase phrase)
            //{
            //    if (!this.OrderId.HasValue)
            //    {
            //        throw new ArgumentNullException(nameof(this.OrderId));
            //    }

            //    phrase.Text = this.Text;
            //    phrase.OrderId = this.OrderId.Value;
            //}

            // await this._context.SaveChangesAsync().ConfigureAwait(false);

           // QuizPhrase phraseDTO = await this.GetByIdAsync(phrase.PhraseId).ConfigureAwait(false);
            return null;
        }

        public async Task<int> GetMaxOrderIdAsync()
        {
            int maxOrderId = await this._context.QuizPhrases
                                                .Select(p => p.OrderId)
                                                .DefaultIfEmpty()
                                                .MaxAsync()
                                                .ConfigureAwait(false);

            return maxOrderId;
        }
    }
}
