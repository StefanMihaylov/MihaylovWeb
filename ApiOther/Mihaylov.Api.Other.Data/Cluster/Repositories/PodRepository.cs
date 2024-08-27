using System;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Cluster.Models;
using Mihaylov.Api.Other.Database.Cluster;
using DB = Mihaylov.Api.Other.Database.Cluster.Models;

namespace Mihaylov.Api.Other.Data.Cluster.Repositories
{
    public class PodRepository : BaseClusterRepository, IPodRepository
    {
        public PodRepository(ILoggerFactory loggerFactory, MihaylovOtherClusterDbContext dbContext)
            : base(loggerFactory, dbContext)
        {
        }

        public async Task<Pod> AddOrUpdateAsync(Pod model, int applicationId)
        {
            model.Name = model.Name?.Trim();

            try
            {
                var dbModel = await _dbContext.ApplicationPods
                                .Where(t => t.ApplicationId == applicationId &&
                                            t.ApplicationPodId == model.Id)
                                .FirstOrDefaultAsync()
                                .ConfigureAwait(false);

                if (dbModel == null)
                {
                    dbModel = new DB.ApplicationPod();
                    await _dbContext.ApplicationPods.AddAsync(dbModel).ConfigureAwait(false);
                }

                dbModel = model.Adapt(dbModel);
                dbModel.ApplicationId = applicationId;

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                return dbModel.Adapt<Pod>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in add/update application Pod. Error: {Message}", ex.Message);
                throw;
            }
        }
    }
}
