using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Other.Contracts.Show.Interfaces;
using Mihaylov.Api.Other.Contracts.Show.Models;
using Mihaylov.Api.Other.Database.Shows;
using Dbs = Mihaylov.Api.Other.Database.Shows.Models;


namespace Mihaylov.Api.Other.DAL.Show;

public class ConcertTypeRepository : BaseShowRepository, IConcertTypeRepository
{
    public ConcertTypeRepository(ILoggerFactory loggerFactory, MihaylovOtherShowDbContext dbContext)
        : base(loggerFactory, dbContext)
    {
    }

    public async Task<IEnumerable<ConcertType>> GetAllAsync()
    {
        try
        {
            var query = _dbContext.ConcertTypes.AsNoTracking()
                            .OrderBy(c => c.Name)
                            .Select(c => new DbConcertTypeExt
                            {
                                ConcertTypeId = c.ConcertTypeId,
                                Name = c.Name,
                                ConcertsCount = c.Concerts.Count()
                            })
                            .AsQueryable();

            var concertTypes = await query.ProjectToType<ConcertType>()
                        .ToListAsync()
                        .ConfigureAwait(false);

            return concertTypes;

        }
        catch (Exception ex)
        {
            _logger.LogError("Error in getting all concert types. Error: {Message}", ex.Message);
            throw;
        }
    }

    public async Task<ConcertType> AddOrUpdateAsync(ConcertType model)
    {
        model.Name = model.Name?.Trim();

        if (model.Id == 0)
        {
            try
            {
                var exists = await _dbContext.ConcertTypes
                                      .Where(b => b.Name == model.Name)
                                      .AnyAsync()
                                      .ConfigureAwait(false);

                if (exists)
                {
                    throw new ArgumentException($"ConcertType '{model.Name}' already exists.", nameof(model.Name));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in add/update ConcertType validation. Error: {Message}", ex.Message);
                throw;
            }
        }

        try
        {
            var dbModel = await _dbContext.ConcertTypes
                        .Where(t => t.ConcertTypeId == model.Id)
                        .FirstOrDefaultAsync()
                        .ConfigureAwait(false);

            if (dbModel == null)
            {
                dbModel = new Dbs.ConcertType();
                await _dbContext.ConcertTypes.AddAsync(dbModel).ConfigureAwait(false);
            }

            dbModel = model.Adapt(dbModel);

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return dbModel.Adapt<ConcertType>();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in add/update ConcertType. Error: {Message}", ex.Message);
            throw;
        }
    }
}

public class DbConcertTypeExt : Dbs.ConcertType
{
    public int ConcertsCount { get; set; }
}
