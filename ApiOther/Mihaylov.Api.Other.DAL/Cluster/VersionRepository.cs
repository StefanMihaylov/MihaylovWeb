using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Cluster;
using Mihaylov.Api.Other.Database.Cluster;
using DB = Mihaylov.Api.Other.Database.Cluster.Models;

namespace Mihaylov.Api.Other.DAL.Cluster
{
    public class VersionRepository : BaseClusterRepository, IVersionRepository
    {
        public VersionRepository(ILoggerFactory loggerFactory, MihaylovOtherClusterDbContext dbContext)
            : base(loggerFactory, dbContext)
        {
        }

        public async Task<IEnumerable<VersionHistory>> GetVersionsAsync(int applicationId, int size)
        {
            var query = _dbContext.ApplicationVersions.AsNoTracking()
                                            .Where(v => v.ApplicationId == applicationId)
                                            .OrderByDescending(v => v.CreatedOn)
                                            .Take(size);

            var historyList = await query.ProjectToType<VersionHistory>()
                                          .ToListAsync()
                                          .ConfigureAwait(false);

            return historyList;
        }

        public async Task<AppVersion> AddOrUpdateAsync(AppVersion model, int applicationId)
        {
            model.Version = model.Version?.Trim();
            model.HelmVersion = model.HelmVersion?.Trim();
            model.HelmAppVersion = model.HelmAppVersion?.Trim();

            try
            {
                var dbModel = await _dbContext.ApplicationVersions
                                .Where(t => t.ApplicationId == applicationId &&
                                            t.VersionId == model.Id)
                                .FirstOrDefaultAsync()
                                .ConfigureAwait(false);

                if (dbModel == null)
                {
                    dbModel = new DB.ApplicationVersion();
                    await _dbContext.ApplicationVersions.AddAsync(dbModel).ConfigureAwait(false);
                }

                dbModel = model.Adapt(dbModel);
                dbModel.ApplicationId = applicationId;

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                return dbModel.Adapt<AppVersion>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in add/update application version. Error: {Message}", ex.Message);
                throw;
            }
        }
    }
}
