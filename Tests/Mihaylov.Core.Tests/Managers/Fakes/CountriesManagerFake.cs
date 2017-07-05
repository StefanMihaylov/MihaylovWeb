using System.Collections.Generic;
using Mihaylov.Core.Interfaces;
using Mihaylov.Core.Managers;
using Mihaylov.Data.Models.Repositories;
using Ninject.Extensions.Logging;

namespace Mihaylov.Core.Tests.Managers.Fakes
{
    internal class CountriesManagerFake : CountriesManager
    {
        public CountriesManagerFake(ICountriesProvider countryProvider, ILogger logger) 
            : base(countryProvider, logger)
        {
        }

        public ICountriesProvider ExposedProvider
        {
            get
            {
                return this.provider;
            }
        }

        public ILogger ExposedLogger
        {
            get
            {
                return this.logger;
            }
        }

        public IDictionary<int, Country> ExposedDictionaryById
        {
            get
            {
                return this.countriesById;
            }
        }

        public IDictionary<string, Country> ExposedDictionaryByName
        {
            get
            {
                return this.countriesByName;
            }
        }
    }
}
