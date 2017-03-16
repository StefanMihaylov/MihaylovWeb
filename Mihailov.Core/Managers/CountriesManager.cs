using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Mihaylov.Core.Interfaces;
using Mihaylov.Data.Models.Repositories;
using Ninject.Extensions.Logging;

namespace Mihaylov.Core.Managers
{
    public class CountriesManager : ICountriesManager
    {
        private readonly ICountriesProvider provider;
        private readonly ILogger logger;

        private readonly ConcurrentDictionary<int, Country> countriesById;
        private readonly ConcurrentDictionary<string, Country> countriesByName;

        public CountriesManager(ICountriesProvider countryProvider, ILogger logger)
        {
            this.provider = countryProvider;
            this.logger = logger;

            this.countriesById = new ConcurrentDictionary<int, Country>();
            this.countriesByName = new ConcurrentDictionary<string, Country>(StringComparer.OrdinalIgnoreCase);
        }

        public IEnumerable<Country> GetAllCountries()
        {
            IEnumerable<Country> countries = this.countriesByName.Values;
            return countries;
        }

        public Country GetById(int id)
        {
            Country country = this.countriesById.GetOrAdd(id, (newId) =>
            {
                try
                {
                    Country newCountry = this.provider.GetById(newId);
                    return newCountry;
                }
                catch (Exception ex)
                {
                    this.logger.Error(ex, "GetById");
                    return null;
                }
            });

            if (country == null)
            {
                this.countriesById.TryRemove(id, out country);
            }

            return country;
        }

        public Country GetByName(string name)
        {
            string key = name.Trim();
            Country country = this.countriesByName.GetOrAdd(key, (newName) =>
            {
                try
                {
                    Country newCountry = this.provider.GetByName(newName);
                    return newCountry;
                }
                catch (Exception ex)
                {
                    this.logger.Error(ex, "GetByName");
                    return null;
                }
            });

            if (country == null)
            {
                this.countriesByName.TryRemove(key, out country);
            }

            return country;
        }
    }
}
