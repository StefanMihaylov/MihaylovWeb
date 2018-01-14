using System.Collections.Generic;
using Mihaylov.Common.Database.Interfaces;
using Mihaylov.Data.Models.Dictionaries;
using DAL = Mihaylov.Database.Dictionaries;

namespace Mihaylov.Data.Interfaces.Dictionaries
{
    public interface IRecordsRepository : IRepository<DAL.Record>
    {
        IEnumerable<Record> Search(RecordSearchModel searchModel);

        Record AddRecord(Record inputRecord, IEnumerable<DAL.RecordType> allRecordTypes);
    }
}
