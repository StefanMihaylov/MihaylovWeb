using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Show.Models;
using Mihaylov.Common;

namespace Mihaylov.Api.Other.Contracts.Show.Interfaces
{
    public interface IConcertRepository
    {
        Task<BaseGrid<ConcertExtended>> GetAllAsync(BaseGridRequest request);

        Task<ConcertExtended> AddOrUpdateAsync(Concert model);
    }
}