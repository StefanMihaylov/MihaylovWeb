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

namespace Mihaylov.Api.Other.DAL.Show
{
    public class ConcertRepository : BaseShowRepository, IConcertRepository
    {
        public ConcertRepository(ILoggerFactory loggerFactory, MihaylovOtherShowDbContext dbContext)
            : base(loggerFactory, dbContext)
        {
        }

        public async Task<Grid<ConcertExtended>> GetAllAsync(GridRequest request)
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

                if (request.Page.HasValue && request.PageSize.HasValue)
                {
                    query = query.Skip((request.Page.Value - 1) * request.PageSize.Value)
                                 .Take(request.PageSize.Value)
                                 .AsQueryable();
                }

                var concerts = await query.ProjectToType<ConcertExtended>()
                                        .ToListAsync()
                                        .ConfigureAwait(false);

                var result = new Grid<ConcertExtended>()
                {
                    Data = concerts,
                    Pager = new Pager(request, _dbContext.Concerts.Count()),
                };

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
                    .Where(t => t.ConcertId == model.Id)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);

                if (dbModel == null)
                {
                    dbModel = new DB.Concert();
                    _dbContext.Concerts.Add(dbModel);
                }

                dbModel = model.Adapt(dbModel);

                dbModel.ConcertBands.Clear();
                if (model.Bands.Any())
                {
                    var bands = model.Bands.Select(b => new DB.ConcertBand()
                    {
                        BandId = b.Id,
                        ConcertId = model.Id,
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
