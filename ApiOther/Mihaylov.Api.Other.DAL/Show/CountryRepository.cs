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

namespace Mihaylov.Api.Other.DAL.Show;

public class CountryRepository : BaseShowRepository, ICountryRepository
{
    public CountryRepository(ILoggerFactory loggerFactory, MihaylovOtherShowDbContext dbContext)
        : base(loggerFactory, dbContext)
    {
    }

    public async Task<BaseGrid<CountryExtended>> GetAllAsync(BaseGridRequest request)
    {
        try
        {
            var query = _dbContext.Countries.AsNoTracking()
                                    .GroupJoin(_dbContext.Bands
                                            .Where(b => b.CountryId != null)
                                            .GroupBy(b => b.CountryId)
                                            .Select(g => new
                                            {
                                                CountryId = g.Key,
                                                Count = g.Count()
                                            }),
                                        c => c.CountryId,
                                        bc => bc.CountryId,
                                        (c, bc) => new
                                        {
                                            Country = c,
                                            BandCounts = bc
                                        })
                                    .SelectMany(g => g.BandCounts.DefaultIfEmpty(),
                                        (g, bc) => new DbCountryExt
                                        {
                                            Country = g.Country,
                                            BandCount = bc.Count
                                        })
                                    .OrderBy(g => g.Country.Name)
                                    .AsQueryable();

            var result = await query.GetPageAsync(request, q => q.ProjectToType<CountryExtended>()).ConfigureAwait(false);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in getting all countries. Error: {Message}", ex.Message);
            throw;
        }
    }

    public async Task<Country> AddOrUpdateAsync(Country model)
    {
        model.Name = model.Name?.Trim();
        model.Code = model.Code?.Trim();

        if (model.Id == 0)
        {
            try
            {
                var exists = await _dbContext.Countries
                                      .Where(b => b.Name == model.Name)
                                      .AnyAsync()
                                      .ConfigureAwait(false);

                if (exists)
                {
                    throw new ArgumentException($"Country '{model.Name}' already exists.", nameof(model.Name));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in add/update Country validation. Error: {Message}", ex.Message);
                throw;
            }
        }

        try
        {
            var dbModel = await _dbContext.Countries
                        .Where(t => t.CountryId == model.Id)
                        .FirstOrDefaultAsync()
                        .ConfigureAwait(false);

            if (dbModel == null)
            {
                dbModel = new DB.Country();
                await _dbContext.Countries.AddAsync(dbModel).ConfigureAwait(false);
            }

            dbModel = model.Adapt(dbModel);

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return dbModel.Adapt<Country>();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in add/update Country. Error: {Message}", ex.Message);
            throw;
        }
    }
}

public class DbCountryExt
{
    public DB.Country Country { get; set; }

    public int? BandCount { get; set; }
}
