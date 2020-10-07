using System.Collections.Generic;
using log4net;
using Mihaylov.Common.MessageBus.Interfaces;
using Mihaylov.Site.Core.Managers;
using Mihaylov.Site.Data.Interfaces;
using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Core.Tests.Managers
{
    internal class PersonAdditionalInfoManagerFake : PersonAdditionalInfoManager
    {
        public PersonAdditionalInfoManagerFake(
            ILookupTablesRepository lookupTablesRepository,
            ILocationsRepository locationsRepository,
            ILog logger, IMessageBus messageBus)
            : base(lookupTablesRepository, locationsRepository, logger,messageBus)
        {
        }

        public ILookupTablesRepository ExposedLookupTablesRepository
        {
            get { return this._lookupTablesRepository; }
        }

        public ILog ExposedLogger
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
