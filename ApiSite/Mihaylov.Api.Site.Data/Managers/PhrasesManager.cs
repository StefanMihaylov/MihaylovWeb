using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Site.Contracts.Repositories;
using Mihaylov.Api.Site.Contracts.Managers;
using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Data.Managers
{
    public class PhrasesManager : IPhrasesManager
    {
        private readonly IQuizRepository repository;
        private readonly ILogger logger;
        // private readonly IMessageBus messageBus;

        private readonly Lazy<ConcurrentDictionary<int, QuizPhrase>> phrasesById;

        public PhrasesManager(IQuizRepository phrasesRepository, ILoggerFactory loggerFactory)
        {
            this.repository = phrasesRepository;
            this.logger = loggerFactory.CreateLogger(this.GetType().Name);
            //this.messageBus = messageBus;

            this.phrasesById = new Lazy<ConcurrentDictionary<int, QuizPhrase>>(() =>
            {
                var phraseList = this.repository.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                IDictionary<int, QuizPhrase> dictionary = phraseList.ToDictionary(p => p.Id);
                return new ConcurrentDictionary<int, QuizPhrase>(dictionary);
            });

            //this.messageBus.Attach(typeof(Phrase), this.HandleMessage);
        }

        public IEnumerable<QuizPhrase> GetAllPhrases()
        {
            IEnumerable<QuizPhrase> phrases = this.phrasesById.Value.Values
                                                          .OrderBy(p => p.OrderId);
            return phrases;
        }

        //private void HandleMessage(Message message)
        //{
        //    if (message == null || !this.phrasesById.IsValueCreated)
        //    {
        //        return;
        //    }

        //    if (message.Data is Phrase phrase)
        //    {
        //        if (message.ActionType == MessageActionType.Add ||
        //           (message.ActionType == MessageActionType.Update && this.phrasesById.Value.ContainsKey(phrase.Id)))
        //        {
        //            this.phrasesById.Value.AddOrUpdate(phrase.Id, (id) => phrase, (updateId, existingPhrase) => phrase);
        //        }
        //    }
        //}
    }
}
