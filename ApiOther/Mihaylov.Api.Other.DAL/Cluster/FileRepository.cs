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

namespace Mihaylov.Api.Other.DAL.Cluster
{
    public class FileRepository : BaseClusterRepository, IFileRepository
    {
        public FileRepository(ILoggerFactory loggerFactory, MihaylovOtherClusterDbContext dbContext)
            : base(loggerFactory, dbContext)
        {
        }

        public async Task<DeploymentFile> AddOrUpdateAsync(DeploymentFile model, int applicationId)
        {
            model.Name = model.Name?.Trim();

            try
            {
                var dbModel = await _dbContext.DeploymentFiles
                                .Where(t => t.ApplicationId == applicationId &&
                                            t.FileId == model.Id)
                                .FirstOrDefaultAsync()
                                .ConfigureAwait(false);

                if (dbModel == null)
                {
                    dbModel = new DB.DeploymentFile();
                    await _dbContext.DeploymentFiles.AddAsync(dbModel).ConfigureAwait(false);
                }

                dbModel = model.Adapt(dbModel);
                dbModel.ApplicationId = applicationId;

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                return dbModel.Adapt<DeploymentFile>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in add/update deployment file. Error: {Message}", ex.Message);
                throw;
            }
        }
    }
}
