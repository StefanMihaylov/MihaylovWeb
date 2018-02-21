using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using log4net.Core;
using Mihaylov.Common.MessageBus;
using Mihaylov.Common.MessageBus.Interfaces;
using Mihaylov.Common.MessageBus.Models;
using Mihaylov.Core.Interfaces.Site;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Managers.Site
{
    public class PhrasesManager : IPhrasesManager
    {
        private readonly IPhrasesProvider provider;
        private readonly ILogger logger;
        private readonly IMessageBus messageBus;

        private readonly Lazy<ConcurrentDictionary<int, Phrase>> phrasesById;

        public PhrasesManager(IPhrasesProvider phrasesProvider, ILogger logger, IMessageBus messageBus)
        {
            this.provider = phrasesProvider;
            this.logger = logger;
            this.messageBus = messageBus;

            this.phrasesById = new Lazy<ConcurrentDictionary<int, Phrase>>(() =>
            {
                IDictionary<int, Phrase> dictionary = this.provider.GetAllPhrases().ToDictionary(p => p.Id);
                return new ConcurrentDictionary<int, Phrase>(dictionary);
            });

            this.messageBus.Attach(typeof(Phrase), this.HandleMessage);
        }

        public IEnumerable<Phrase> GetAllPhrases()
        {
            IEnumerable<Phrase> phrases = this.phrasesById.Value
                                                          .Values
                                                          .OrderBy(p => p.OrderId);
            return phrases;
        }

        private void HandleMessage(Message message)
        {
            if (message == null || !this.phrasesById.IsValueCreated)
            {
                return;
            }

            if (message.Data is Phrase phrase)
            {
                if (message.ActionType == MessageActionType.Add ||
                   (message.ActionType == MessageActionType.Update && this.phrasesById.Value.ContainsKey(phrase.Id)))
                {
                    this.phrasesById.Value.AddOrUpdate(phrase.Id, (id) => phrase, (updateId, existingPhrase) => phrase);
                }
            }
        }
    }
}
