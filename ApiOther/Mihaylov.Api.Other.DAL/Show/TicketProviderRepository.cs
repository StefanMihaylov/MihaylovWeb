using System;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Other.Contracts.Show.Interfaces;
using Mihaylov.Api.Other.Contracts.Show.Models;
using Mihaylov.Api.Other.Database.Shows;
using Mihaylov.Common;
using DB = Mihaylov.Api.Other.Database.Shows.Models;

namespace Mihaylov.Api.Other.DAL.Show
{
    public class TicketProviderRepository : BaseShowRepository, ITicketProviderRepository
    {
        public TicketProviderRepository(ILoggerFactory loggerFactory, MihaylovOtherShowDbContext dbContext)
            : base(loggerFactory, dbContext)
        {
        }

        public async Task<BaseGrid<TicketProvider>> GetAllAsync(BaseGridRequest request)
        {
            try
            {
                var query = _dbContext.TicketProviders.AsNoTracking()
                                        .OrderBy(t => t.Name)
                                        .AsQueryable();

                var result = await query.GetPageAsync(request, q => q.ProjectToType<TicketProvider>()).ConfigureAwait(false);

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

            if (model.Id == 0)
            {
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
