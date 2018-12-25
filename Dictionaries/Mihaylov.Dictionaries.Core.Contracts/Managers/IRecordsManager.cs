using System.Collections.Generic;
using Mihaylov.Dictionaries.Data.Models;

namespace Mihaylov.Dictionaries.Core.Interfaces
{
    public interface IRecordsManager
    {
        IEnumerable<PrepositionType> GetAllPrepositionTypes(int languageId);

        IEnumerable<PrepositionType> GetAllPrepositionTypes();

        IEnumerable<RecordType> GetAllRecordTypes();

        IEnumerable<Record> GetRecords(RecordSearchModel searchModel);

        Record AddRecord(Record inputRecord);
    }
}
