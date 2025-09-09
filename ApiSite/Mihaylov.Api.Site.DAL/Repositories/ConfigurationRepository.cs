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
using DB = Mihaylov.Api.Site.Database.Models;

namespace Mihaylov.Api.Site.DAL.Repositories
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly ILogger _logger;
        private readonly SiteDbContext _context;

        public ConfigurationRepository(ILoggerFactory loggerFactory, SiteDbContext context)
        {
            _logger = loggerFactory.CreateLogger(this.GetType().Name);
            _context = context;
        }

        public async Task<IEnumerable<DefaultFilter>> GetAllDefaultFiltersAsync()
        {
            var query = _context.DefaultFilters.AsNoTracking();

            var filters = await query.ProjectToType<DefaultFilter>()
                                                 .ToListAsync()
                                                 .ConfigureAwait(false);

            return filters;
        }

        public async Task<DefaultFilter> GetDefaultFilterAsync()
        {
            var query = _context.DefaultFilters.AsNoTracking()
                                .Where(f => f.IsEnabled)
                                .OrderBy(f => f.DefaultFilterId);

            var filter = await query.ProjectToType<DefaultFilter>()
                                                 .FirstOrDefaultAsync()
                                                 .ConfigureAwait(false);

            return filter;
        }

        public async Task<DefaultFilter> AddDefaultFilterAsync(DefaultFilter input)
        {
            try
            {
                var dbModel = await _context.DefaultFilters
                                .Where(t => t.DefaultFilterId == input.Id)
                                .FirstOrDefaultAsync()
                                .ConfigureAwait(false);

                if (dbModel == null)
                {
                    dbModel = new DB.DefaultFilter();
                    _context.DefaultFilters.Add(dbModel);
                }

                dbModel = input.Adapt(dbModel);

                await _context.SaveChangesAsync().ConfigureAwait(false);

                return dbModel.Adapt<DefaultFilter>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in add/update DefaultFilter. Error: {Message}", ex.Message);
                throw;
            }
        }
    }
}
