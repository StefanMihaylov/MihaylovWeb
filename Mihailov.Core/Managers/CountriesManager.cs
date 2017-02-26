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

            Initialize();
        }

        public IEnumerable<Country> GetAllUnits()
        {
            IEnumerable<Country> units = this.countriesById.Values;
            return units;
        }

        public Country GetById(int id)
        {
            Country country;
            if (this.countriesById.TryGetValue(id, out country))
            {
                return country;
            }
            else
            {
                throw new ApplicationException($"Country with id: {id} was not found");
            }
        }

        public Country GetByName(string name)
        {
            name = name.Trim();
            Country country = this.countriesByName.GetOrAdd(name,
                newName =>
                {
                    Country newCountry = new Country() { Name = newName };
                    Country savedCountry = this.provider.AddCountry(newCountry);

                    this.countriesById.TryAdd(savedCountry.Id, savedCountry);

                    return savedCountry;
                });

            return country;
        }

        private void Initialize()
        {
            IEnumerable<Country> countries = this.provider.GetAll();
            foreach (var country in countries)
            {
                this.countriesById.TryAdd(country.Id, country);
                this.countriesByName.TryAdd(country.Name, country);
            }
        }
    }
}
