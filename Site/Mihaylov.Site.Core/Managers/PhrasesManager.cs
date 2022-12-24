using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Mihaylov.Site.Core.Interfaces;
using Mihaylov.Site.Data.Interfaces;
using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Core.Managers
{
    public class PhrasesManager : IPhrasesManager
    {
        private readonly IPhrasesRepository repository;
        private readonly ILogger logger;
        // private readonly IMessageBus messageBus;

        private readonly Lazy<ConcurrentDictionary<int, Phrase>> phrasesById;

        public PhrasesManager(IPhrasesRepository phrasesRepository, ILoggerFactory loggerFactory)
        {
            this.repository = phrasesRepository;
            this.logger = loggerFactory.CreateLogger(this.GetType().Name);
            //this.messageBus = messageBus;

            this.phrasesById = new Lazy<ConcurrentDictionary<int, Phrase>>(() =>
            {
                var phraseList = this.repository.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                IDictionary<int, Phrase> dictionary = phraseList.ToDictionary(p => p.Id);
                return new ConcurrentDictionary<int, Phrase>(dictionary);
            });

            //this.messageBus.Attach(typeof(Phrase), this.HandleMessage);
        }

        public IEnumerable<Phrase> GetAllPhrases()
        {
            IEnumerable<Phrase> phrases = this.phrasesById.Value.Values
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
