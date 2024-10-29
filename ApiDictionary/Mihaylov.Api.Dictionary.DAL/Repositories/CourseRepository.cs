using System.Collections.Generic;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Dictionary.Contracts.Models;
using Mihaylov.Api.Dictionary.Contracts.Repositories;
using Mihaylov.Api.Dictionary.Database;

namespace Mihaylov.Api.Dictionary.DAL.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ILogger _logger;
        private readonly DictionaryDbContext _context;

        public CourseRepository(ILoggerFactory loggerFactory, DictionaryDbContext context)
        {
            _logger = loggerFactory.CreateLogger(this.GetType().Name);
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            var query = _context.Courses.Include(c => c.Level)
                                        .Include(c => c.LearningSystem)
                                            .ThenInclude(s => s.Language)
                                        .AsNoTracking();

            var courses = await query.ProjectToType<Course>()
                                     .ToListAsync()
                                     .ConfigureAwait(false);
            return courses;
        }

        public async Task<IEnumerable<Level>> GetAllLevelsAsync()
        {
            var query = _context.Levels.AsNoTracking();

            var levels = await query.ProjectToType<Level>()
                                    .ToListAsync()
                                    .ConfigureAwait(false);

            return levels;
        }

        public async Task<IEnumerable<Language>> GetAllLanguagesAsync()
        {
            var query = _context.Languages.AsNoTracking();

            var languages = await query.ProjectToType<Language>()
                                                 .ToListAsync()
                                                 .ConfigureAwait(false);

            return languages;
        }

        public async Task<IEnumerable<LearningSystem>> GetAllLearningSystemsAsync()
        {
            var query = _context.LearningSystems.Include(s => s.Language)
                                                .AsNoTracking();

            var learningSystems = await query.ProjectToType<LearningSystem>()
                                                 .ToListAsync()
                                                 .ConfigureAwait(false);

            return learningSystems;
        }
    }
}
