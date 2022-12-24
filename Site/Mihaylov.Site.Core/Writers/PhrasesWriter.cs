using System.Threading.Tasks;
using Mihaylov.Site.Core.Interfaces;
using Mihaylov.Site.Data.Interfaces;
using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Core.Writers
{
    public class PhrasesWriter : IPhrasesWriter
    {
        private readonly IPhrasesRepository repository;
        // private readonly IMessageBus messageBus;

        public PhrasesWriter(IPhrasesRepository phrasesRepository)
        {
            this.repository = phrasesRepository;    
        }

        public async Task<Phrase> AddOrUpdateAsync(Phrase inputPhrase)
        {
            if (!inputPhrase.OrderId.HasValue)
            {
                int maxOrderId = await this.repository.GetMaxOrderId();

                inputPhrase.OrderId = maxOrderId + 1;
            }

            Phrase phrase = await this.repository.AddOrUpdatePhraseAsync(inputPhrase);//, out bool isNewPhrase);

            bool isNewPhrase = false; // TODO;
            //MessageActionType action = isNewPhrase ? MessageActionType.Add : MessageActionType.Update;
            //this.messageBus.SendMessage(phrase, this, action);

            return phrase;
        }
    }
}