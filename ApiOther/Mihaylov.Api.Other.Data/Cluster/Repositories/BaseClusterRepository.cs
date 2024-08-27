using Microsoft.Extensions.Logging;
using Mihaylov.Api.Other.Database.Cluster;

namespace Mihaylov.Api.Other.Data.Cluster.Repositories
{
    public abstract class BaseClusterRepository
    {
        protected readonly ILogger _logger;
        protected readonly MihaylovOtherClusterDbContext _dbContext;

        public BaseClusterRepository(ILoggerFactory loggerFactory, MihaylovOtherClusterDbContext dbContext)
        {
            _logger = loggerFactory.CreateLogger(this.GetType());
            _dbContext = dbContext;
        }
    }
}
