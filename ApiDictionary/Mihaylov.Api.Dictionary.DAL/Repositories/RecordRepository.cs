using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Dictionary.Contracts.Models;
using Mihaylov.Api.Dictionary.Contracts.Repositories;
using Mihaylov.Api.Dictionary.Database;
using DB = Mihaylov.Api.Dictionary.Database.Models;

namespace Mihaylov.Api.Dictionary.DAL.Repositories
{
    public class RecordRepository : IRecordRepository
    {
        private readonly ILogger _logger;
        private readonly DictionaryDbContext _context;

        public RecordRepository(ILoggerFactory loggerFactory, DictionaryDbContext context)
        {
            _logger = loggerFactory.CreateLogger(this.GetType().Name);
            _context = context;
        }

        public async Task<IEnumerable<Record>> GetRecordsAsync(RecordSearchModel searchModel)
        {
            var query = GetRecordQuery().AsNoTracking();

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

            var records = await query.ProjectToType<Record>()
                                     .ToListAsync()
                                     .ConfigureAwait(false);

            return records;
        }

        public async Task<Record> AddRecordAsync(Record input)
        {
            input.Original = input.Original?.Trim();
            input.Translation = input.Translation?.Trim();
            input.Comment = input.Comment?.Trim();

            try
            {
                var dbModel = await GetRecordQuery()
                                    .Where(t => t.RecordId == input.Id)
                                    .FirstOrDefaultAsync()
                                    .ConfigureAwait(false);

                if (dbModel == null)
                {
                    dbModel = new DB.Record();
                    _context.Records.Add(dbModel);
                }

                dbModel = input.Adapt(dbModel);

                await _context.SaveChangesAsync().ConfigureAwait(false);

                return dbModel.Adapt<Record>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in add/update QuizQuestion. Error: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<RecordType>> GetAllRecordTypesAsync()
        {
            var query = _context.RecordTypes.AsNoTracking();

            var types = await query.ProjectToType<RecordType>()
                                   .ToListAsync()
                                   .ConfigureAwait(false);

            return types;
        }

        public async Task<IEnumerable<Preposition>> GetAllPrepositionsAsync()
        {
            var query = _context.Prepositions.Include(p => p.Language)
                                             .AsNoTracking();

            var prepositions = await query.ProjectToType<Preposition>()
                                          .ToListAsync()
                                          .ConfigureAwait(false);

            return prepositions;
        }

        private IQueryable<DB.Record> GetRecordQuery()
        {
            return _context.Records.Include(x => x.RecordType)
                                   .Include(x => x.Preposition)
                                   .Include(x => x.Course)
                                   .AsQueryable();
        }
    }
}
