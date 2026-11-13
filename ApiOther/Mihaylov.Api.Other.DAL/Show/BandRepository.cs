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
    internal class BandRepository : BaseShowRepository, IBandRepository
    {
        public BandRepository(ILoggerFactory loggerFactory, MihaylovOtherShowDbContext dbContext)
            : base(loggerFactory, dbContext)
        {
        }

        public async Task<BaseGrid<BandExtended>> GetAllAsync(BaseGridRequest request)
        {
            try
            {
                var query = _dbContext.Bands.AsNoTracking()
                             .Include(b => b.Country)
                            .GroupJoin(_dbContext.ConcertBands
                                .GroupBy(cb => cb.BandId)
                                .Select(gcb => new BankRank()
                                {
                                    BandId = gcb.Key,
                                    Rank = gcb.Sum(a => 100.0m / a.Order),
                                    Count = gcb.Count(),
                                }),
                                b => b.BandId,
                                cb => cb.BandId,
                                (b, cb) => new
                                {
                                    Band = b,
                                    RankList = cb,
                                })
                            .SelectMany(gcb => gcb.RankList.DefaultIfEmpty(),
                               (gcb, cb) => new
                               {
                                   Band = gcb.Band,
                                   Count = cb.Count ?? 0,
                                   BandRank = cb.Rank ?? 9999,
                               })
                            .OrderByDescending(g => g.BandRank)
                            .ThenBy(g => g.Band.Name)
                            .Select(p => new DbBandExt
                            {
                                Band = p.Band,
                                Count = p.Count
                            })
                            .AsQueryable();

                var result = await query.GetPageAsync(request, q => q.ProjectToType<BandExtended>()).ConfigureAwait(false);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in getting all bands. Error: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<Band> AddOrUpdateAsync(Band model)
        {
            model.Name = model.Name?.Trim();

            if (model.Id == 0)
            {
                try
                {
                    var exists = await _dbContext.Bands
                                          .Where(b => b.Name == model.Name)
                                          .AnyAsync()
                                          .ConfigureAwait(false);

                    if (exists)
                    {
                        throw new ArgumentException($"Band '{model.Name}' already exists.", nameof(model.Name));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error in add/update band validation. Error: {Message}", ex.Message);
                    throw;
                }
            }

            try
            {
                var dbModel = await _dbContext.Bands
                                .Where(t => t.BandId == model.Id)
                                .FirstOrDefaultAsync()
                                .ConfigureAwait(false);

                if (dbModel == null)
                {
                    dbModel = new DB.Band();
                    await _dbContext.Bands.AddAsync(dbModel).ConfigureAwait(false);
                }

                dbModel = model.Adapt(dbModel);

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                return dbModel.Adapt<Band>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in add/update band. Error: {Message}", ex.Message);
                throw;
            }
        }

        private class BankRank
        {
            public int BandId { get; set; }

            public decimal? Rank { get; set; }

            public int? Count { get; set; }
        }
    }

    public class DbBandExt
    {
        public DB.Band Band { get; set; }

        public int Count { get; set; }
    }
}
