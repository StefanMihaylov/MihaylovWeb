using System.Collections.Generic;
using System.Linq;
using Mihaylov.Dictionaries.Core.Interfaces;
using Mihaylov.Dictionaries.Data.Interfaces;
using Mihaylov.Dictionaries.Data.Models;
using DAL = Mihaylov.Dictionaries.Database.Models;

namespace Mihaylov.Dictionaries.Core.Writers
{
    public class RecordsWriter : IRecordsWriter
    {
        private readonly IRecordsData recordsData;

        public RecordsWriter(IRecordsData recordsData)
        {
            this.recordsData = recordsData;
        }

        public Record AddRecord(Record record)
        {
            IEnumerable<DAL.RecordType> recordsTypes = this.recordsData.RecordTypes.All().ToList();
            Record resultRecord = this.recordsData.Records.AddRecord(record, recordsTypes);

            return resultRecord;
        }
    }
}
