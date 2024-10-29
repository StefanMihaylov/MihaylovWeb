using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Dictionary.Contracts.Models;

namespace Mihaylov.Api.Dictionary.Contracts.Managers
{
    public interface IRecordManager
    {
        Task<IEnumerable<Record>> GetRecordsAsync(RecordSearchModel searchModel);

        Task<IEnumerable<Preposition>> GetAllPrepositionsAsync();

        Task<IEnumerable<RecordType>> GetAllRecordTypesAsync();
    }
}
