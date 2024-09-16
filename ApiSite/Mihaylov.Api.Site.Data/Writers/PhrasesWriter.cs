using System.Threading.Tasks;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Repositories;
using Mihaylov.Api.Site.Contracts.Writers;

namespace Mihaylov.Api.Site.Data.Writers
{
    public class PhrasesWriter : IPhrasesWriter
    {
        private readonly IQuizRepository repository;
        // private readonly IMessageBus messageBus;

        public PhrasesWriter(IQuizRepository phrasesRepository)
        {
            this.repository = phrasesRepository;
        }

        public async Task<QuizPhrase> AddOrUpdateAsync(QuizPhrase inputPhrase)
        {
            if (!inputPhrase.OrderId.HasValue)
            {
                int maxOrderId = await this.repository.GetMaxOrderIdAsync().ConfigureAwait(false);

                inputPhrase.OrderId = maxOrderId + 1;
            }

            QuizPhrase phrase = await this.repository.AddOrUpdatePhraseAsync(inputPhrase);//, out bool isNewPhrase);

            bool isNewPhrase = false; // TODO;
            //MessageActionType action = isNewPhrase ? MessageActionType.Add : MessageActionType.Update;
            //this.messageBus.SendMessage(phrase, this, action);

            return phrase;
        }
    }
}
