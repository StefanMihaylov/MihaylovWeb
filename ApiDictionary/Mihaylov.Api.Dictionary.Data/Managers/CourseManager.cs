using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Dictionary.Contracts.Managers;
using Mihaylov.Api.Dictionary.Contracts.Models;
using Mihaylov.Api.Dictionary.Contracts.Repositories;

namespace Mihaylov.Api.Dictionary.Data.Managers
{
    public class CourseManager : ICourseManager
    {
        private readonly ICourseRepository _repository;

        public CourseManager(ICourseRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return _repository.GetAllCoursesAsync();
        }

        public Task<IEnumerable<Language>> GetAllLanguagesAsync()
        {
            return _repository.GetAllLanguagesAsync();
        }

        public Task<IEnumerable<LearningSystem>> GetAllLearningSystemsAsync()
        {
            return _repository.GetAllLearningSystemsAsync();
        }

        public Task<IEnumerable<Level>> GetAllLevelsAsync()
        {
            return _repository.GetAllLevelsAsync();
        }
    }
}
