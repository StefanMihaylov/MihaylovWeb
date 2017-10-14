using System.Collections.Generic;
using System.Linq;
using Mihaylov.Core.Interfaces.Dictionaries;
using Mihaylov.Data.Interfaces.Dictionaries;
using Mihaylov.Data.Models.Dictionaries;

namespace Mihaylov.Core.Providers.Dictionaries
{
    public class CoursesInfoProvider : ICoursesInfoProvider
    {
        private readonly IGetAllRepository<Course> coursesRepository;
        private readonly IGetAllRepository<Language> languagesRepository;
        private readonly IGetAllRepository<LearningSystem> learningSystemsRepository;
        private readonly IGetAllRepository<Level> levelsRepository;

        public CoursesInfoProvider(IGetAllRepository<Course> coursesRepository, IGetAllRepository<Language> languagesRepository,
            IGetAllRepository<LearningSystem> learningSystemsRepository, IGetAllRepository<Level> levelsRepository)
        {
            this.coursesRepository = coursesRepository;
            this.languagesRepository = languagesRepository;
            this.learningSystemsRepository = learningSystemsRepository;
            this.levelsRepository = levelsRepository;
        }

        public IEnumerable<Course> GetAllCourses()
        {
            IEnumerable<Course> courses = this.coursesRepository.GetAll().ToList();
            return courses;
        }

        public IEnumerable<Language> GetAllLanguages()
        {
            IEnumerable<Language> languages = this.languagesRepository.GetAll().ToList();
            return languages;
        }

        public IEnumerable<LearningSystem> GetAllLearningSystems()
        {
            IEnumerable<LearningSystem> learningSystems = this.learningSystemsRepository.GetAll().ToList();
            return learningSystems;
        }

        public IEnumerable<Level> GetAllLevels()
        {
            IEnumerable<Level> levels = this.levelsRepository.GetAll().ToList();
            return levels;
        }
    }
}
