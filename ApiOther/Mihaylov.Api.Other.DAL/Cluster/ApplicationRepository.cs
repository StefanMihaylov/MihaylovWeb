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
    public class ApplicationRepository : BaseClusterRepository, IApplicationRepository
    {
        public ApplicationRepository(ILoggerFactory loggerFactory, MihaylovOtherClusterDbContext dbContext)
            : base(loggerFactory, dbContext)
        {
        }

        public async Task<IEnumerable<ApplicationExtended>> GetAllAsync()
        {
            var query = _dbContext.Applications.AsNoTracking()
                                  .Include(c => c.Pods)
                                  .Include(c => c.Files)
                                  .OrderBy(c => c.Order)
                                    .ThenBy(c => c.ApplicationId);

            var applications = await query.ProjectToType<ApplicationExtended>()
                                    .ToListAsync()
                                    .ConfigureAwait(false);

            return applications;
        }

        public async Task<ApplicationExtended> AddOrUpdateAsync(Application model)
        {
            model.Name = model.Name?.Trim();

            try
            {
                var dbModel = await _dbContext.Applications
                                .Where(t => t.ApplicationId == model.Id)
                                .FirstOrDefaultAsync()
                                .ConfigureAwait(false);

                if (dbModel == null)
                {
                    dbModel = new DB.Application();
                    await _dbContext.Applications.AddAsync(dbModel).ConfigureAwait(false);
                }

                dbModel = model.Adapt(dbModel);

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                return dbModel.Adapt<ApplicationExtended>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in add/update application. Error: {Message}", ex.Message);
                throw;
            }
        }
    }
}
