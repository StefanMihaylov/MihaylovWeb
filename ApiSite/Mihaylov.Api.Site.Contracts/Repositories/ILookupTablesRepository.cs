using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Contracts.Repositories
{
    public interface ILookupTablesRepository
    {
        Task<IEnumerable<Ethnicity>> GetAllEthnicitiesAsync();

        Task<IEnumerable<Orientation>> GetAllOrientationsAsync();

        Task<IEnumerable<AccountType>> GetAllAccountTypesAsync();
    }
}
