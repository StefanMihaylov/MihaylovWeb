using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Cluster.Models;
using Mihaylov.Api.Other.Database.Cluster;
using DB = Mihaylov.Api.Other.Database.Cluster.Models;

namespace Mihaylov.Api.Other.DAL.Cluster
{
    public class ParserSettingRepository : BaseClusterRepository, IParserSettingRepository
    {
        public ParserSettingRepository(ILoggerFactory loggerFactory, MihaylovOtherClusterDbContext dbContext)
            : base(loggerFactory, dbContext)
        {
        }

        public async Task<IEnumerable<ParserSetting>> GetAllAsync()
        {
            var query = _dbContext.ParserSettings.AsNoTracking()
                      .Include(c => c.Application);

            var settings = await query.ProjectToType<ParserSetting>()
                                    .ToListAsync()
                                    .ConfigureAwait(false);
            return settings;
        }

        public async Task<ParserSetting> AddOrUpdateAsync(ParserSetting model)
        {
            model.VersionSelector = model.VersionSelector?.Trim();
            model.VersionCommand = model.VersionCommand?.Trim() ?? string.Empty;
            model.ReleaseDateSelector = model.ReleaseDateSelector?.Trim();
            model.ReleaseDateCommand = model.ReleaseDateCommand?.Trim() ?? string.Empty;

            try
            {
                var dbModel = await _dbContext.ParserSettings
                                .Where(t => t.ApplicationId == model.ApplicationId &&
                                            t.ParserSettingId == model.Id)
                                .FirstOrDefaultAsync()
                                .ConfigureAwait(false);

                if (dbModel == null)
                {
                    dbModel = new DB.ParserSetting();
                    await _dbContext.ParserSettings.AddAsync(dbModel).ConfigureAwait(false);
                }

                dbModel = model.Adapt(dbModel);

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                return dbModel.Adapt<ParserSetting>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in add/update parser setting. Error: {Message}", ex.Message);
                throw;
            }
        }
    }
}
