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
    public class ConcertRepository : BaseShowRepository, IConcertRepository
    {
        public ConcertRepository(ILoggerFactory loggerFactory, MihaylovOtherShowDbContext dbContext)
            : base(loggerFactory, dbContext)
        {
        }

        public async Task<BaseGrid<ConcertExtended>> GetAllAsync(BaseGridRequest request)
        {
            try
            {
                var query = _dbContext.Concerts.AsNoTracking()
                                        .Include(c => c.Location)
                                        .Include(c => c.TicketProvider)
                                        .Include(c => c.ConcertBands)
                                            .ThenInclude(cb => cb.Band)
                                        .OrderByDescending(c => c.Date)
                                        .AsQueryable();

                var result = await query.GetPageAsync(request, q => q.ProjectToType<ConcertExtended>()).ConfigureAwait(false);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in getting all Concerts. Error: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<ConcertExtended> AddOrUpdateAsync(Concert model)
        {
            try
            {
                model.Name = model.Name?.Trim();

                var dbModel = await _dbContext.Concerts
                    .Include(c => c.ConcertBands)
                    .Where(t => t.ConcertId == model.Id)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);

                if (dbModel == null)
                {
                    dbModel = new DB.Concert();
                    _dbContext.Concerts.Add(dbModel);
                }

                dbModel = model.Adapt(dbModel);

                if (dbModel.ConcertBands?.Any() == true)
                {
                    _dbContext.ConcertBands.RemoveRange(dbModel.ConcertBands);
                    dbModel.ConcertBands.Clear();
                }

                if (model.Bands?.Any() == true)
                {
                    var bands = model.Bands.Select(b => new DB.ConcertBand()
                    {
                        BandId = b.Id,
                       // ConcertId = model.Id,
                        Order = 0
                    })
                    .ToList();

                    for (int i = 0; i < bands.Count; i++)
                    {
                        var band = bands[i];
                        band.Order = i + 1;

                        dbModel.ConcertBands.Add(band);
                    }
                }

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                return dbModel.Adapt<ConcertExtended>();

            }
            catch (Exception ex)
            {
                _logger.LogError("Error in add/update band. Error: {Message}", ex.Message);
                throw;
            }
        }
    }
}
