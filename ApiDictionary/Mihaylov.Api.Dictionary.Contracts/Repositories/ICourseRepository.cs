using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Dictionary.Contracts.Models;

namespace Mihaylov.Api.Dictionary.Contracts.Repositories
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync();

        Task<IEnumerable<Language>> GetAllLanguagesAsync();

        Task<IEnumerable<LearningSystem>> GetAllLearningSystemsAsync();

        Task<IEnumerable<Level>> GetAllLevelsAsync();
    }
}