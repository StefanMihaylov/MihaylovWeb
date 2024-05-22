using System;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Other.Contracts.Base.Models;
using Mihaylov.Api.Other.Contracts.Show.Interfaces;
using Mihaylov.Api.Other.Contracts.Show.Models;
using Mihaylov.Api.Other.Database.Shows;
using DB = Mihaylov.Api.Other.Database.Shows.Models;

namespace Mihaylov.Api.Other.Data.Show.Repositories
{
    public class TicketProviderRepository : BaseShowRepository, ITicketProviderRepository
    {
        public TicketProviderRepository(ILoggerFactory loggerFactory, MihaylovOtherShowDbContext dbContext)
            : base(loggerFactory, dbContext)
        {
        }

        public async Task<Grid<TicketProvider>> GetAllAsync(GridRequest request)
        {
            try
            {
                var query = _dbContext.TicketProviders.AsNoTracking()
                                        .OrderBy(t => t.Name)
                                        .AsQueryable();

                if (request.Page.HasValue && request.PageSize.HasValue)
                {
                    query = query.Skip((request.Page.Value - 1) * request.PageSize.Value)
                                 .Take(request.PageSize.Value)
                                 .AsQueryable();
                }

                var providers = await query.ProjectToType<TicketProvider>()
                                           .ToListAsync()
                                           .ConfigureAwait(false);

                var result = new Grid<TicketProvider>()
                {
                    Data = providers,
                    Pager = new Pager(request, _dbContext.TicketProviders.Count()),
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in getting all TicketProviders. Error: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<TicketProvider> AddOrUpdateAsync(TicketProvider model)
        {
            model.Name = model.Name?.Trim();
            model.Url = model.Url?.Trim();

            try
            {
                var exists = await _dbContext.TicketProviders
                                      .Where(b => b.Name == model.Name)
                                      .AnyAsync()
                                      .ConfigureAwait(false);

                if (exists)
                {
                    throw new ArgumentException($"TicketProvider '{model.Name}' already exists.", nameof(model.Name));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in add/update TicketProvider validation. Error: {Message}", ex.Message);
                throw;
            }

            try
            {
                var dbModel = await _dbContext.TicketProviders
                            .Where(t => t.TickerProviderId == model.Id)
                            .FirstOrDefaultAsync()
                            .ConfigureAwait(false);

                if (dbModel == null)
                {
                    dbModel = new DB.TicketProvider();
                    await _dbContext.TicketProviders.AddAsync(dbModel).ConfigureAwait(false);
                }

                dbModel = model.Adapt(dbModel);

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                return dbModel.Adapt<TicketProvider>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in add/update Location. Error: {Message}", ex.Message);
                throw;
            }
        }
    }
}
