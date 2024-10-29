using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Dictionary.Contracts.Models;

namespace Mihaylov.Api.Dictionary.Contracts.Repositories
{
    public interface IRecordRepository
    {
        Task<IEnumerable<Record>> GetRecordsAsync(RecordSearchModel searchModel);

        Task<Record> AddRecordAsync(Record input);

        Task<IEnumerable<RecordType>> GetAllRecordTypesAsync();

        Task<IEnumerable<Preposition>> GetAllPrepositionsAsync();
    }
}