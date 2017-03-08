using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Mihaylov.Core.Interfaces;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Managers
{
    public class CountriesManager : ICountriesManager
    {
        private readonly ICountriesProvider provider;

        private readonly ConcurrentDictionary<int, Country> countriesById;
        private readonly ConcurrentDictionary<string, Country> countriesByName;

        public CountriesManager(ICountriesProvider countryProvider)
        {
            this.provider = countryProvider;
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
                Country newCountry = this.provider.GetById(newId);
                return newCountry;
            });

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
                catch (Exception)
                {
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
