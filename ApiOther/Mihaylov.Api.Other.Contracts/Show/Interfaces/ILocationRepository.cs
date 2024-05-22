using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Base.Models;
using Mihaylov.Api.Other.Contracts.Show.Models;

namespace Mihaylov.Api.Other.Contracts.Show.Interfaces
{
    public interface ILocationRepository
    {
        Task<Grid<Location>> GetAllAsync(GridRequest request);
     
        Task<Location> AddOrUpdateAsync(Location model);
    }
}