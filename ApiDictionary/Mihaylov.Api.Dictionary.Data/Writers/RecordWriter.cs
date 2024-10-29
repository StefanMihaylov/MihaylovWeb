using System.Threading.Tasks;
using Mihaylov.Api.Dictionary.Contracts.Models;
using Mihaylov.Api.Dictionary.Contracts.Repositories;
using Mihaylov.Api.Dictionary.Contracts.Writers;

namespace Mihaylov.Api.Dictionary.Data.Writers
{
    public class RecordWriter : IRecordWriter
    {
        private readonly IRecordRepository _repository;

        public RecordWriter(IRecordRepository repository)
        {
            _repository = repository;
        }

        public Task<Record> AddRecordAsync(Record input)
        {
            return _repository.AddRecordAsync(input);
        }
    }
}
