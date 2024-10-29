using System.Threading.Tasks;
using Mihaylov.Api.Dictionary.Contracts.Models;

namespace Mihaylov.Api.Dictionary.Contracts.Writers
{
    public interface IRecordWriter
    {
        Task<Record> AddRecordAsync(Record input);
    }
}
