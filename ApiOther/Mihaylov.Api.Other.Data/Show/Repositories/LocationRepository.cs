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
    public class LocationRepository : BaseShowRepository, ILocationRepository
    {
        public LocationRepository(ILoggerFactory loggerFactory, MihaylovOtherShowDbContext dbContext)
            : base(loggerFactory, dbContext)
        {
        }

        public async Task<Grid<Location>> GetAllAsync(GridRequest request)
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

                if (request.Page.HasValue && request.PageSize.HasValue)
                {
                    query = query.Skip((request.Page.Value - 1) * request.PageSize.Value)
                                 .Take(request.PageSize.Value)
                                 .AsQueryable();
                }

                var locations = await query.ProjectToType<Location>()
                                        .ToListAsync()
                                        .ConfigureAwait(false);

                var result = new Grid<Location>()
                {
                    Data = locations,
                    Pager = new Pager(request, _dbContext.Locations.Count()),
                };

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
