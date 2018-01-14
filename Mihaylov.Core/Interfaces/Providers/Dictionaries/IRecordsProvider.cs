using System.Collections.Generic;
using Mihaylov.Data.Models.Dictionaries;

namespace Mihaylov.Core.Interfaces.Dictionaries
{
    public interface IRecordsProvider
    {
        IEnumerable<PrepositionType> GetAllPrepositionTypes();
        IEnumerable<RecordType> GetAllRecordTypes();
        IEnumerable<Record> GetRecords(RecordSearchModel searchModel);
        Record AddRecord(Record inputRecord);
    }
}