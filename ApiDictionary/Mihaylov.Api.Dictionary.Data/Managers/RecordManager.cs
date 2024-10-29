using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Dictionary.Contracts.Managers;
using Mihaylov.Api.Dictionary.Contracts.Models;
using Mihaylov.Api.Dictionary.Contracts.Repositories;

namespace Mihaylov.Api.Dictionary.Data.Managers
{
    public class RecordManager : IRecordManager
    {
        private readonly IRecordRepository _repository;

        public RecordManager(IRecordRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Record>> GetRecordsAsync(RecordSearchModel searchModel)
        {
            return _repository.GetRecordsAsync(searchModel);
        }

        public Task<IEnumerable<RecordType>> GetAllRecordTypesAsync()
        {
            return _repository.GetAllRecordTypesAsync();
        }

        public Task<IEnumerable<Preposition>> GetAllPrepositionsAsync()
        {
            return _repository.GetAllPrepositionsAsync();
        }
    }
}
