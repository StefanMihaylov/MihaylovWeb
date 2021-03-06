﻿using System.Collections.Generic;
using Mihaylov.Common.MessageBus.Interfaces;
using Mihaylov.Core.Interfaces.Site;
using Mihaylov.Core.Managers.Site;
using Mihaylov.Data.Models.Site;
using Ninject.Extensions.Logging;

namespace Mihaylov.Core.Tests.Managers.Fakes
{
    internal class PersonAdditionalInfoManagerFake : PersonAdditionalInfoManager
    {
        public PersonAdditionalInfoManagerFake(IPersonAdditionalInfoProvider provider, ILogger logger, IMessageBus messageBus)
            : base(provider, logger, messageBus)
        {
        }

        public IPersonAdditionalInfoProvider ExposedProvider
        {
            get { return this.provider; }
        }

        public ILogger ExposedLogger
        {
            get { return this.logger; }
        }

        public IMessageBus ExposedMessageBus
        {
            get { return this.messageBus; }
        }

        public IDictionary<int, AnswerType> ExposedAnswerTypeDictionaryById
        {
            get { return this.answerTypesById.Value; }
        }

        public IDictionary<string, AnswerType> ExposedAnswerTypeDictionaryByName
        {
            get { return this.answerTypesByName.Value; }
        }

        public IDictionary<int, Country> ExposedCountryDictionaryById
        {
            get { return this.countriesById; }
        }

        public IDictionary<string, Country> ExposedCountryDictionaryByName
        {
            get { return this.countriesByName; }
        }
    }
}
