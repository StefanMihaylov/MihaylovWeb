using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Contracts.Repositories
{
    public interface IConfigurationRepository
    {
        Task<DefaultFilter> AddDefaultFilterAsync(DefaultFilter input);

        Task<IEnumerable<DefaultFilter>> GetAllDefaultFiltersAsync();

        Task<DefaultFilter> GetDefaultFilterAsync();
    }
}