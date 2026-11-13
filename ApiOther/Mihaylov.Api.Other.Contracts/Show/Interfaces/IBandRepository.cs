using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Show.Models;
using Mihaylov.Common;

namespace Mihaylov.Api.Other.Contracts.Show.Interfaces
{
    public interface IBandRepository
    {
        Task<BaseGrid<BandExtended>> GetAllAsync(BaseGridRequest request);
       
        Task<Band> AddOrUpdateAsync(Band model);
    }
}