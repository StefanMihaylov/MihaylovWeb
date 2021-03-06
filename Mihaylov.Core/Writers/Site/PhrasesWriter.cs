﻿using System.Linq;
using Mihaylov.Common.MessageBus.Interfaces;
using Mihaylov.Common.MessageBus.Models;
using Mihaylov.Core.Interfaces.Site;
using Mihaylov.Data.Interfaces.Site;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Writers.Site
{
    public class PhrasesWriter : IPhrasesWriter
    {
        private readonly IPhrasesRepository repository;
        private readonly IMessageBus messageBus;

        public PhrasesWriter(IPhrasesRepository phrasesRepository, IMessageBus messageBus)
        {
            this.repository = phrasesRepository;
            this.messageBus = messageBus;
        }

        public Phrase AddOrUpdate(Phrase inputPhrase)
        {
            if (!inputPhrase.OrderId.HasValue)
            {
                int maxOrderId = this.repository.GetAll()
                                               .Where(p => p.OrderId.HasValue)
                                               .Select(p => p.OrderId.Value)
                                               .DefaultIfEmpty()
                                               .Max();

                inputPhrase.OrderId = maxOrderId + 1;
            }

            Phrase phrase = this.repository.AddOrUpdatePhrase(inputPhrase, out bool isNewPhrase);

            MessageActionType action = isNewPhrase ? MessageActionType.Add : MessageActionType.Update;

            this.messageBus.SendMessage(phrase, this, action);

            return phrase;
        }
    }
}