using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Contracts.Managers
{
    public interface IConfigurationManager
    {
        Task<IEnumerable<DefaultFilter>> GetAllDefaultFiltersAsync();

        Task<DefaultFilter> GetDefaultFilterAsync();
    }
}