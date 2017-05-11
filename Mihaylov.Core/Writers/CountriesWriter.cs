using Mihaylov.Core.Interfaces;
using Mihaylov.Data.Models.Interfaces;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Writers
{
    public class CountriesWriter : ICountriesWriter
    {
        private readonly ICountriesRepository repository;

        public CountriesWriter(ICountriesRepository countryRepository)
        {
            this.repository = countryRepository;
        }

        public Country Add(Country inputCountry)
        {
            Country country = this.repository.AddCountry(inputCountry);
            return country;
        }

        public Country Add(string countryName)
        {
            Country newCountry = new Country() { Name = countryName.Trim() };
            Country country = this.Add(newCountry);
            return country;
        }
    }
}
