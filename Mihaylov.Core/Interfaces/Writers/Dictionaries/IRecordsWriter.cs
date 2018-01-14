using Mihaylov.Data.Models.Dictionaries;

namespace Mihaylov.Core.Interfaces.Dictionaries
{
    public interface IRecordsWriter
    {
        Record AddRecord(Record record);
    }
}