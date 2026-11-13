using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Show.Models;
using Mihaylov.Common;

namespace Mihaylov.Api.Other.Contracts.Show.Interfaces
{
    public interface ILocationRepository
    {
        Task<BaseGrid<Location>> GetAllAsync(BaseGridRequest request);
     
        Task<Location> AddOrUpdateAsync(Location model);
    }
}