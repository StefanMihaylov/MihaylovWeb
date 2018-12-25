using Mihaylov.Dictionaries.Data.Models;

namespace Mihaylov.Dictionaries.Core.Interfaces
{
    public interface IRecordsWriter
    {
        Record AddRecord(Record record);
    }
}