using System.Collections.Generic;
using System.Linq;
using Mihaylov.Core.Interfaces.Dictionaries;
using Mihaylov.Data.Interfaces;
using Mihaylov.Data.Models.Dictionaries;
using DAL = Mihaylov.Database.Dictionaries;

namespace Mihaylov.Core.Providers.Dictionaries
{
    public class RecordsProvider : IRecordsProvider
    {
        private readonly IRecordsData recordsData;

        public RecordsProvider(IRecordsData recordsData)
        {
            this.recordsData = recordsData;
        }

        public IEnumerable<RecordType> GetAllRecordTypes()
        {
            IEnumerable<RecordType> recordTypes = this.recordsData.RecordTypes.GetAll().ToList();
            return recordTypes;
        }

        public IEnumerable<PrepositionType> GetAllPrepositionTypes()
        {
            IEnumerable<PrepositionType> prepositionTypes = this.recordsData.PrepositionTypes.GetAll().ToList();
            return prepositionTypes;
        }

        public IEnumerable<Record> GetRecords(RecordSearchModel searchModel)
        {
            IEnumerable<Record> records = this.recordsData.Records.Search(searchModel).ToList();
            return records;
        }

        public Record AddRecord(Record inputRecord)
        {
            IEnumerable<DAL.RecordType> allRecordTypes = this.recordsData.RecordTypes.All().ToList();
            Record record = this.recordsData.Records.AddRecord(inputRecord, allRecordTypes);
            return record;
        }
    }
}
