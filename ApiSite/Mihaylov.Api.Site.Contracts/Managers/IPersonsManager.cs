using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Contracts.Managers
{
    public interface IPersonsManager
    {
        Task<IEnumerable<Person>> GetAllPersonsAsync(bool descOrder = false, int? pageNumber = null, int? pageSize = null);

        Person GetById(long id);

        Person GetByName(string name);

        Task<PersonStatistics> GetStaticticsAsync();
    }
}
