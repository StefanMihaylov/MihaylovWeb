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
    public class LocationRepository : BaseShowRepository, ILocationRepository
    {
        public LocationRepository(ILoggerFactory loggerFactory, MihaylovOtherShowDbContext dbContext)
            : base(loggerFactory, dbContext)
        {
        }

        public async Task<BaseGrid<Location>> GetAllAsync(BaseGridRequest request)
        {
            try
            {
                var query = _dbContext.Locations.AsNoTracking()
                                        .GroupJoin(_dbContext.Concerts
                                            .GroupBy(c => c.LocationId)
                                            .Select(gc => new
                                            {
                                                LocationId = gc.Key,
                                                Count = gc.Count()
                                            }),
                                            c => c.LocationId,
                                            gc => gc.LocationId,
                                            (c, gc) => new
                                            {
                                                Location = c,
                                                Count = gc
                                            })
                                        .SelectMany(gc => gc.Count.DefaultIfEmpty(),
                                        (gc, count) => new
                                        {
                                            Location = gc.Location,
                                            Count = count.Count
                                        })
                                        .OrderByDescending(gl => gl.Count)
                                        .ThenBy(gl => gl.Location.Name)
                                        .Select(gl => gl.Location)
                                        .AsQueryable();

                var result = await query.GetPageAsync(request, q => q.ProjectToType<Location>()).ConfigureAwait(false);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in getting all locations. Error: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<Location> AddOrUpdateAsync(Location model)
        {
            model.Name = model.Name?.Trim();

            if (model.Id == 0)
            {
                try
                {
                    var exists = await _dbContext.Locations
                                          .Where(b => b.Name == model.Name)
                                          .AnyAsync()
                                          .ConfigureAwait(false);

                    if (exists)
                    {
                        throw new ArgumentException($"Location '{model.Name}' already exists.", nameof(model.Name));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error in add/update Location validation. Error: {Message}", ex.Message);
                    throw;
                }
            }

            try
            {
                var dbModel = await _dbContext.Locations
                            .Where(t => t.LocationId == model.Id)
                            .FirstOrDefaultAsync()
                            .ConfigureAwait(false);

                if (dbModel == null)
                {
                    dbModel = new DB.Location();
                    await _dbContext.Locations.AddAsync(dbModel).ConfigureAwait(false);
                }

                dbModel = model.Adapt(dbModel);

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                return dbModel.Adapt<Location>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in add/update Location. Error: {Message}", ex.Message);
                throw;
            }
        }
    }
}
