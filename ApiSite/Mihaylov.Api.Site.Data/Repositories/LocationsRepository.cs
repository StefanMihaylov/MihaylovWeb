using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Repositories;
using Mihaylov.Api.Site.Database;
using Mihaylov.Common.Mapping;
using DAL = Mihaylov.Api.Site.Database.Models;

namespace Mihaylov.Api.Site.Data.Repositories
{
    public class LocationsRepository : ILocationsRepository
    {
        private readonly SiteDbContext _context;

        public LocationsRepository(SiteDbContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            var countries = await this._context.Countries
                                               .To<Country>()
                                               .ToListAsync()
                                               .ConfigureAwait(false);
            return countries;
        }

        public async Task<IDictionary<int, IEnumerable<CountryState>>> GetAllStatesAsync()
        {
            var statesList = await this._context.CountryStates
                                            .To<CountryState>()
                                            .ToListAsync()
                                            .ConfigureAwait(false);

            var states = statesList.GroupBy(s => s.CountryId)
                                   .ToDictionary(g => g.Key, g => g.AsEnumerable());

            return states;
        }

        public async Task<Country> GetCountryByIdAsync(int id)
        {
            Country country = await this._context.Countries
                                                 .Where(p => p.CountryId == id)
                                                 .To<Country>()
                                                 .FirstOrDefaultAsync()
                                                 .ConfigureAwait(false);
            return country;
        }

        public async Task<Country> GetCountryByNameAsync(string name)
        {
            Country country = await this._context.Countries
                                  .Where(p => p.Name == name)
                                  .To<Country>()
                                  .FirstOrDefaultAsync()
                                  .ConfigureAwait(false);

            return country;
        }

        public async Task<Country> AddCountryAsync(Country inputCountry)
        {
            var dalCountry = inputCountry.ToModel<DAL.Country>();

            this._context.Countries.Add(dalCountry);
          //  await this._context.SaveChangesAsync().ConfigureAwait(false);

            return dalCountry.ToModel<Country>();
        }
    }
}
