using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Base.Models;
using Mihaylov.Api.Other.Contracts.Show.Models;

namespace Mihaylov.Api.Other.Contracts.Show.Interfaces
{
    public interface IConcertRepository
    {
        Task<Grid<ConcertExtended>> GetAllAsync(GridRequest request);

        Task<ConcertExtended> AddOrUpdateAsync(Concert model);
    }
}