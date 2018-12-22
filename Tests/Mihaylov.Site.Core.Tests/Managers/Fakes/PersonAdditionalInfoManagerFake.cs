using System.Collections.Generic;
using Mihaylov.Common.MessageBus.Interfaces;
using Mihaylov.Site.Core.Managers;
using Mihaylov.Site.Data.Interfaces;
using Mihaylov.Site.Data.Models;
using Ninject.Extensions.Logging;

namespace Mihaylov.Site.Core.Tests.Managers
{
    internal class PersonAdditionalInfoManagerFake : PersonAdditionalInfoManager
    {
        public PersonAdditionalInfoManagerFake(
            IGetAllRepository<AnswerType> answerTypeRepository,
            IGetAllRepository<Ethnicity> ethnicitiesRepository,
            IGetAllRepository<Orientation> orientationRepository,
            IGetAllRepository<Unit> unitRepository,
            ICountriesRepository countryRepository,
            ILogger logger, IMessageBus messageBus)
            : base(answerTypeRepository, ethnicitiesRepository, orientationRepository, unitRepository, countryRepository,
                  logger,messageBus)
        {
        }

        public IGetAllRepository<AnswerType> ExposedAnswerTypeRepository
        {
            get { return this.answerTypeRepository; }
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
