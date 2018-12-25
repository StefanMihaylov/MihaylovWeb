using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Mihaylov.Dictionaries.Core.Interfaces;
using Mihaylov.Dictionaries.Data.Interfaces;
using Mihaylov.Dictionaries.Data.Models;
using DAL = Mihaylov.Dictionaries.Database.Models;

namespace Mihaylov.Dictionaries.Core.Managers
{
    public class RecordsManager : IRecordsManager
    {
        private readonly IRecordsData recordsData;

        private readonly Lazy<ConcurrentDictionary<int, IEnumerable<PrepositionType>>> prepositionTypesByLanguageId;
        private readonly Lazy<ConcurrentDictionary<int, RecordType>> recordTypesById;

        public RecordsManager(IRecordsData recordsData)
        {
            this.recordsData = recordsData;

            this.prepositionTypesByLanguageId = new Lazy<ConcurrentDictionary<int, IEnumerable<PrepositionType>>>(() =>
            {
                var prepositionTypes = this.recordsData.PrepositionTypes.GetAll()
                                                           .GroupBy(k => k.LanguageId)
                                                           .ToDictionary(k => k.Key, v => (IEnumerable<PrepositionType>)v);

                return new ConcurrentDictionary<int, IEnumerable<PrepositionType>>(prepositionTypes);
            });

            this.recordTypesById = new Lazy<ConcurrentDictionary<int, RecordType>>(() =>
            {
                var recordTypes = this.recordsData.RecordTypes.GetAll().ToDictionary(k => k.Id, v => v);
                return new ConcurrentDictionary<int, RecordType>(recordTypes);
            });
        }

        public Record AddRecord(Record inputRecord)
        {
            IEnumerable<DAL.RecordType> allRecordTypes = this.recordsData.RecordTypes.All().ToList();
            Record record = this.recordsData.Records.AddRecord(inputRecord, allRecordTypes);
            return record;
        }

        public IEnumerable<Record> GetRecords(RecordSearchModel searchModel)
        {
            IEnumerable<Record> records = this.recordsData.Records.Search(searchModel);
            return records;
        }

        public IEnumerable<PrepositionType> GetAllPrepositionTypes()
        {
            IEnumerable<PrepositionType> prepositionTypes = this.recordsData.PrepositionTypes.GetAll();
            return prepositionTypes;
        }

        public IEnumerable<PrepositionType> GetAllPrepositionTypes(int languageId)
        {
            if (this.prepositionTypesByLanguageId.Value.TryGetValue(languageId, out IEnumerable<PrepositionType> prepositionTypes))
            {
                return prepositionTypes;
            }
            else
            {
                throw new ApplicationException($"The prepositions for LanguageId {languageId} does not exist.");
            }
        }

        public IEnumerable<RecordType> GetAllRecordTypes()
        {
            IEnumerable<RecordType> recordTypes = this.recordTypesById.Value.Values;
            return recordTypes;
        }
    }
}
