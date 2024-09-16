using System.Threading.Tasks;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Repositories;
using Mihaylov.Api.Site.Contracts.Writers;

namespace Mihaylov.Api.Site.Data.Writers
{
    public class CountriesWriter : ICountriesWriter
    {
        private readonly ILocationsRepository repository;

        public CountriesWriter(ILocationsRepository locationsRepository)
        {
            this.repository = locationsRepository;
        }

        public async Task<Country> AddAsync(Country inputCountry)
        {
            Country country = await this.repository.AddCountryAsync(inputCountry).ConfigureAwait(false);
            return country;
        }

        public async Task<Country> AddAsync(string countryName)
        {
            Country newCountry = new Country() { Name = countryName.Trim() };
            Country country = await this.AddAsync(newCountry).ConfigureAwait(false);
            return country;
        }
    }
}
