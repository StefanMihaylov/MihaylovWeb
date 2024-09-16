//using System.Collections.Generic;
//using Microsoft.Extensions.Logging;
//using Mihaylov.Site.Core.Managers;
//using Mihaylov.Site.Data.Interfaces;
//using Mihaylov.Site.Data.Models;

//namespace Mihaylov.Site.Core.Tests.Managers
//{
//    internal class PersonAdditionalInfoManagerFake : CollectionsManager
//    {
//        public PersonAdditionalInfoManagerFake(
//            ILookupTablesRepository lookupTablesRepository,
//            ILocationsRepository locationsRepository,
//            ILoggerFactory loggerFactory)
//            : base(lookupTablesRepository, locationsRepository, loggerFactory)
//        {
//        }

//        public ILookupTablesRepository ExposedLookupTablesRepository
//        {
//            get { return this._lookupTablesRepository; }
//        }

//        public ILogger ExposedLogger
//        {
//            get { return this.logger; }
//        }

//        //public IMessageBus ExposedMessageBus
//        //{
//        //    get { return this.messageBus; }
//        //}

//        public IDictionary<int, AnswerType> ExposedAnswerTypeDictionaryById
//        {
//            get { return this.answerTypesById.Value; }
//        }

//        public IDictionary<string, AnswerType> ExposedAnswerTypeDictionaryByName
//        {
//            get { return this.answerTypesByName.Value; }
//        }

//        public IDictionary<int, Country> ExposedCountryDictionaryById
//        {
//            get { return this.countriesById; }
//        }

//        public IDictionary<string, Country> ExposedCountryDictionaryByName
//        {
//            get { return this.countriesByName; }
//        }
//    }
//}
