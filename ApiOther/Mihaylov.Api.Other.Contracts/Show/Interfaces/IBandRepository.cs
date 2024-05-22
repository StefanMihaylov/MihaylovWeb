using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Base.Models;
using Mihaylov.Api.Other.Contracts.Show.Models;

namespace Mihaylov.Api.Other.Contracts.Show.Interfaces
{
    public interface IBandRepository
    {
        Task<Grid<BandExtended>> GetAllAsync(GridRequest request);
       
        Task<Band> AddOrUpdateAsync(Band model);
    }
}