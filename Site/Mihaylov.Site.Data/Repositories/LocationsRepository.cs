using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
                                              .Select(Country.FromDb)
                                              .ToListAsync()
                                              .ConfigureAwait(false);
            return countries;
        }

        public async Task<IEnumerable<State>> GetAllStatesAsync()
        {
            var states = await this._context.States
                                            .Select(State.FromDb)
                                            .ToListAsync()
                                            .ConfigureAwait(false);
            return states;
        }

        public async Task<Country> GetCountryByIdAsync(int id)
        {
            Country country = await this._context.Countries
                                                 .Where(p => p.Id == id)
                                                 .Select(Country.FromDb)
                                                 .FirstOrDefaultAsync()
                                                 .ConfigureAwait(false);
            return country;
        }

        public async Task<Country> GetCountryByNameAsync(string name)
        {
           // var filtered = await this._context.Countries.Where(p => p.Name == name).ToListAsync();

            Country country = await this._context.Countries
                                  .Where(p => p.Name == name)
                                  .Select(Country.FromDb)
                                  .FirstOrDefaultAsync()
                                  .ConfigureAwait(false);

            return country;
        }

        public async Task<Country> AddCountryAsync(Country inputCountry)
        {
            DAL.Country country = inputCountry.Create();

            this._context.Countries.Add(country);

            await this._context.SaveChangesAsync().ConfigureAwait(false);

            return country;
        }
    }
}
