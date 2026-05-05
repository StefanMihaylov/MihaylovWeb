using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Show.Models;

namespace Mihaylov.Api.Other.Contracts.Show.Interfaces
{
    public interface IConcertTypeRepository
    {
        Task<IEnumerable<ConcertType>> GetAllAsync();

        Task<ConcertType> AddOrUpdateAsync(ConcertType model);

    }
}