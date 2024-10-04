using System.Threading.Tasks;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Models.Base;

namespace Mihaylov.Api.Site.Contracts.Managers
{
    public interface IPersonsManager
    {
        Task<Grid<Person>> GetAllPersonsAsync(GridRequest request);

        Task<Person> GetByIdAsync(long id);

        //Person GetByName(string name);

        Task<PersonStatistics> GetStaticticsAsync();
    }
}
