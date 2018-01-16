using System.Collections.Generic;
using System.Linq;
using Mihaylov.Core.Interfaces.Site;
using Mihaylov.Data.Interfaces.Site;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Providers.Site
{
    public class PhrasesProvider : IPhrasesProvider
    {
        private readonly IPhrasesRepository repository;

        public PhrasesProvider(IPhrasesRepository phrasesRepository)
        {
            this.repository = phrasesRepository;
        }

        public IEnumerable<Phrase> GetAllPhrases()
        {
            IEnumerable<Phrase> phrases = this.repository.GetAll()
                                                         .ToList();
            return phrases;
        }
    }
}
