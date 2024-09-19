using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Repositories;
using Mihaylov.Api.Site.Database;
using Mihaylov.Common.Abstract.Databases;
using DB = Mihaylov.Api.Site.Database.Models;

namespace Mihaylov.Api.Site.DAL.Repositories
{
    public class CollectionRepository : ICollectionRepository
    {
        private readonly ILogger _logger;
        private readonly SiteDbContext _context;

        public CollectionRepository(ILoggerFactory loggerFactory, SiteDbContext context)
        {
            _logger = loggerFactory.CreateLogger(this.GetType().Name);
            _context = context;
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            var personsQuery = _context.Persons.Where(p => p.CountryId.HasValue)
                                               .GroupBy(p => p.CountryId)
                                               .Select(gc => new
                                               {
                                                   CountryId = gc.Key.Value,
                                                   Count = gc.Count()
                                               })
                                               .Where(gc => gc.Count > 9)
                                               .AsQueryable();

            // var counts = personsQuery.ToList();

            var query = _context.Countries.AsNoTracking()
                                          .LeftOuterJoin(personsQuery, c => c.CountryId, pc => pc.CountryId,
                                          pc => new
                                          {
                                              Country = pc.Left,
                                              Count = pc.Right.Count
                                          })
                                          .OrderByDescending(gl => gl.Count)
                                          .ThenBy(gl => gl.Country.Name)
                                          .Select(gl => gl.Country)
                                          .AsQueryable();

            var countries = await query.ProjectToType<Country>()
                                               .ToListAsync()
                                               .ConfigureAwait(false);
            return countries;
        }

        public async Task<Country> GetCountryByNameAsync(string name)
        {
            var query = _context.Countries.AsNoTracking()
                                          .Where(c => c.Name == name ||
                                                      (c.AlternativeNames != null && c.AlternativeNames.Contains(name)));

            var country = await query.ProjectToType<Country>()
                                               .FirstOrDefaultAsync()
                                               .ConfigureAwait(false);
            return country;
        }

        public async Task<IDictionary<int, IEnumerable<CountryState>>> GetAllStatesAsync()
        {
            var query = _context.CountryStates.AsNoTracking()
                                          .GroupBy(s => s.CountryId)
                                          .Select(g => new
                                          {
                                              Key = g.Key,
                                              List = g.AsQueryable().Adapt<IEnumerable<CountryState>>(),
                                          });

            var states = await query.ToDictionaryAsync(g => g.Key, g => g.List.ToList().AsEnumerable())
                                         .ConfigureAwait(false);
            return states;
        }

        public async Task<IEnumerable<Ethnicity>> GetAllEthnicitiesAsync()
        {
            var query = _context.Ethnicities.AsNoTracking();

            var ethnicities = await query.ProjectToType<Ethnicity>()
                                                 .ToListAsync()
                                                 .ConfigureAwait(false);

            return ethnicities;
        }

        public async Task<IEnumerable<Orientation>> GetAllOrientationsAsync()
        {
            var query = _context.Orientations.AsNoTracking();

            var preference = await query.ProjectToType<Orientation>()
                                                .ToListAsync()
                                                .ConfigureAwait(false);
            return preference;
        }

        public async Task<IEnumerable<AccountType>> GetAllAccountTypesAsync()
        {
            var query = _context.AccountTypes.AsNoTracking();

            var accountTypes = await query.ProjectToType<AccountType>()
                                                .ToListAsync()
                                                .ConfigureAwait(false);
            return accountTypes;
        }

        public async Task<AccountType> AddAccountTypeAsync(AccountType input)
        {
            input.Name = input.Name?.Trim();

            try
            {
                var dbModel = await _context.AccountTypes
                                .Where(t => t.AccountTypeId == input.Id)
                                .FirstOrDefaultAsync()
                                .ConfigureAwait(false);

                if (dbModel == null)
                {
                    dbModel = new DB.AccountType();
                    _context.AccountTypes.Add(dbModel);
                }

                dbModel = input.Adapt(dbModel);

                await _context.SaveChangesAsync().ConfigureAwait(false);

                return dbModel.Adapt<AccountType>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in add/update AccountType. Error: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<AccountStatus>> GetAllAccountStatesAsync()
        {
            var query = _context.AccountStates.AsNoTracking();

            var units = await query.ProjectToType<AccountStatus>()
                                   .ToListAsync()
                                   .ConfigureAwait(false);

            return units;
        }
    }
}
