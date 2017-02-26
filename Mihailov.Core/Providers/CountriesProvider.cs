using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public Country AddCountry(Country inputCountry)
        {
            Country country = this.repository.AddCountry(inputCountry);
            return country;
        }
    }
}
