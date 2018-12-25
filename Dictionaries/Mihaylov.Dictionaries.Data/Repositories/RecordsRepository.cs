using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mihaylov.Common.Database;
using Mihaylov.Common.Mapping;
using Mihaylov.Dictionaries.Data.Interfaces;
using Mihaylov.Dictionaries.Data.Models;
using Mihaylov.Dictionaries.Database.Interfaces;
using DAL = Mihaylov.Dictionaries.Database.Models;

namespace Mihaylov.Data.Repositories.Dictionaries
{
    public class RecordsRepository : GenericRepository<DAL.Record, IDictionariesDbContext>, IRecordsRepository
    {
        public RecordsRepository(IDictionariesDbContext context)
            : base(context)
        {
        }

        public IEnumerable<Record> Search(RecordSearchModel searchModel)
        {
            if (searchModel == null)
            {
                throw new ArgumentNullException(nameof(searchModel));
            }

            IQueryable<DAL.Record> query = this.All();

            if (searchModel.Id.HasValue)
            {
                query = query.Where(r => r.RecordId == searchModel.Id.Value);
            }

            if (searchModel.CourseId.HasValue)
            {
                query = query.Where(r => r.CourseId == searchModel.CourseId.Value);
            }

            if (searchModel.ModuleNumber.HasValue)
            {
                query = query.Where(r => r.ModuleNumber.HasValue)
                             .Where(r => r.ModuleNumber.Value == searchModel.ModuleNumber.Value);
            }

            if (searchModel.RecordTypes != null && searchModel.RecordTypes.Any())
            {
                var recordTypes = searchModel.RecordTypes.ToList();
                // query = query.Where(r => recordTypes.Intersect(r.RecordTypes.AsQueryable()));
            }

            IEnumerable<Record> record = query.To<Record>()
                                              .AsQueryable();
            return record;
        }

        public Record AddRecord(Record inputRecord, IEnumerable<DAL.RecordType> allRecordTypes)
        {
            DAL.Record record = inputRecord.Create();

            if (inputRecord.RecordTypes.Any())
            {
                record.RecordRecordTypes.Clear();

                IEnumerable<int> recordTypeIds = inputRecord.RecordTypes.Select(r => r.Id).ToList();
                IEnumerable<DAL.RecordType> recordTypes = allRecordTypes.Where(r => recordTypeIds.Contains(r.RecordTypeId));
                foreach (var recordType in recordTypes)
                {
                   // record.RecordRecordTypes.Add(recordType);
                }
            }

            this.Add(record);
            this.Context.SaveChanges();

            Record recondDto = Mapper.Map<Record>(record);
            return recondDto;
        }
    }
}
