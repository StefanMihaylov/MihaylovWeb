using System.Collections.Generic;
using System.Linq;
using Mihaylov.Core.Interfaces.Dictionaries;
using Mihaylov.Data.Interfaces;
using Mihaylov.Data.Models.Dictionaries;
using DAL = Mihaylov.Database.Dictionaries;

namespace Mihaylov.Core.Writers.Dictionaries
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
