using Microsoft.Extensions.Logging;
using Mihaylov.Api.Other.Database.Shows;

namespace Mihaylov.Api.Other.DAL.Show
{
    public abstract class BaseShowRepository
    {
        protected readonly ILogger _logger;
        protected readonly MihaylovOtherShowDbContext _dbContext;

        public BaseShowRepository(ILoggerFactory loggerFactory, MihaylovOtherShowDbContext dbContext)
        {
            _logger = loggerFactory.CreateLogger(this.GetType());
            _dbContext = dbContext;
        }
    }
}
