using System;
using System.Collections.Generic;
using System.Linq;
using Mihaylov.Core.Interfaces;
using Mihaylov.Data.Models.Interfaces;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Providers
{
    public class CountriesProvider : ICountriesProvider
    {
        private readonly ICountriesRepository repository;

        public CountriesProvider(ICountriesRepository countryRepository)
        {
            this.repository = countryRepository;
        }

        public IEnumerable<Country> GetAll()
        {
            IEnumerable<Country> countries = this.repository.GetAll()
                                                            .ToList();
            return countries;
        }

        public Country GetById(int id)
        {
            Country country = this.repository.GetById(id);

            if(country == null)
            {
                throw new ApplicationException($"Country with Id: {id} was not found");
            }

            return country;
        }

        public Country GetByName(string name)
        {
            Country country = this.repository.GetByName(name);

            if (country == null)
            {
                throw new ApplicationException($"Country with name: {name} was not found");
            }

            return country;
        }
    }
}
