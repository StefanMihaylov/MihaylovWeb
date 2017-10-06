using System;
using System.Collections.Concurrent;
using Mihaylov.Common.Validations;
using Mihaylov.Core.Interfaces.Site;
using Mihaylov.Data.Models.Site;
using Ninject.Extensions.Logging;

namespace Mihaylov.Core.Managers.Site
{
    public class CountriesManager : ICountriesManager
    {
        protected readonly ICountriesProvider provider;
        protected readonly ILogger logger;

        protected readonly ConcurrentDictionary<int, Country> countriesById;
        protected readonly ConcurrentDictionary<string, Country> countriesByName;

        public CountriesManager(ICountriesProvider countryProvider, ILogger logger)
        {
            ParameterValidation.IsNotNull(countryProvider, nameof(countryProvider));
            ParameterValidation.IsNotNull(logger, nameof(logger));

            this.provider = countryProvider;
            this.logger = logger;

            this.countriesById = new ConcurrentDictionary<int, Country>();
            this.countriesByName = new ConcurrentDictionary<string, Country>(StringComparer.OrdinalIgnoreCase);
        }

        //public IEnumerable<Country> GetAllCountries()
        //{
        //    IEnumerable<Country> countries = this.countriesByName.Values;
        //    return countries;
        //}

        public Country GetById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException("Id must be positive");
            }

            try
            {
                Country country = this.countriesById.GetOrAdd(id, (newId) =>
                {
                    Country newCountry = this.provider.GetById(newId);
                    return newCountry;
                });

                return country;
            }
            catch (ApplicationException)
            {
                return null;
            }
        }

        public Country GetByName(string name)
        {
            this.logger.Debug($"Manager: Get country by name: {name}");
            try
            {
                string key = name.Trim();
                Country country = this.countriesByName.GetOrAdd(key, (newName) =>
                {
                    this.logger.Debug($"Provider: Get country by name: {name}");

                    Country newCountry = this.provider.GetByName(newName);

                   this.logger.Debug($"Provider: Found country by name ({name}): {newCountry?.Name}");

                    return newCountry;
                });

                return country;
            }
            catch (ApplicationException)
            {
                return null;
            }
        }
    }
}
