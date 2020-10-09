using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Common.Mapping;
using Mihaylov.Site.Data.Interfaces;
using Mihaylov.Site.Data.Models;
using Mihaylov.Site.Database;
using DAL = Mihaylov.Site.Database.Models;

namespace Mihaylov.Site.Data.Repositories
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

        public async Task<IDictionary<int, IEnumerable<State>>> GetAllStatesAsync()
        {
            var statesList = await this._context.States
                                            .To<State>()
                                            .ToListAsync()
                                            .ConfigureAwait(false);

            var states = statesList.GroupBy(s => s.CountryId)
                                   .ToDictionary(g => g.Key, g => g.AsEnumerable());

            return states;
        }

        public async Task<Country> GetCountryByIdAsync(int id)
        {
            Country country = await this._context.Countries
                                                 .Where(p => p.Id == id)
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
            await this._context.SaveChangesAsync().ConfigureAwait(false);

            return dalCountry.ToModel<Country>();
        }
    }
}
