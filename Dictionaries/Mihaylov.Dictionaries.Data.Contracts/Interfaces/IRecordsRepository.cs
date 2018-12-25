using System.Collections.Generic;
using Mihaylov.Common.Database.Interfaces;
using Mihaylov.Dictionaries.Data.Models;
using DAL = Mihaylov.Dictionaries.Database.Models;

namespace Mihaylov.Dictionaries.Data.Interfaces
{
    public interface IRecordsRepository : IRepository<DAL.Record>
    {
        IEnumerable<Record> Search(RecordSearchModel searchModel);

        Record AddRecord(Record inputRecord, IEnumerable<DAL.RecordType> allRecordTypes);
    }
}
