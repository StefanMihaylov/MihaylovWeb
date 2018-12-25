using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Mihaylov.Dictionaries.Core.Interfaces;
using Mihaylov.Dictionaries.Data.Interfaces;
using Mihaylov.Dictionaries.Data.Models;

namespace Mihaylov.Dictionaries.Core.Managers
{
    public class CoursesInfoManager : ICoursesInfoManager
    {
        private readonly IGetAllRepository<Course> coursesRepository;
        private readonly IGetAllRepository<Language> languagesRepository;
        private readonly IGetAllRepository<LearningSystem> learningSystemsRepository;
        private readonly IGetAllRepository<Level> levelsRepository;

        private readonly Lazy<ConcurrentDictionary<int, Course>> coursesById;
        private readonly Lazy<ConcurrentDictionary<int, Language>> languagesById;
        private readonly Lazy<ConcurrentDictionary<int, LearningSystem>> learningSystemsById;
        private readonly Lazy<ConcurrentDictionary<int, Level>> levelsById;

        public CoursesInfoManager(IGetAllRepository<Course> coursesRepository, IGetAllRepository<Language> languagesRepository,
            IGetAllRepository<LearningSystem> learningSystemsRepository, IGetAllRepository<Level> levelsRepository)
        {
            this.coursesRepository = coursesRepository;
            this.languagesRepository = languagesRepository;
            this.learningSystemsRepository = learningSystemsRepository;
            this.levelsRepository = levelsRepository;

            this.coursesById = new Lazy<ConcurrentDictionary<int, Course>>(() =>
            {
                var courses = this.coursesRepository.GetAll().ToDictionary(k => k.Id, v => v);
                return new ConcurrentDictionary<int, Course>(courses);
            });

            this.languagesById = new Lazy<ConcurrentDictionary<int, Language>>(() =>
            {
                var languages = this.languagesRepository.GetAll().ToDictionary(k => k.Id, v => v);
                return new ConcurrentDictionary<int, Language>(languages);
            });

            this.learningSystemsById = new Lazy<ConcurrentDictionary<int, LearningSystem>>(() =>
            {
                var learningSystems = this.learningSystemsRepository.GetAll().ToDictionary(k => k.Id, v => v);
                return new ConcurrentDictionary<int, LearningSystem>(learningSystems);
            });

            this.levelsById = new Lazy<ConcurrentDictionary<int, Level>>(() =>
            {
                var levels = this.levelsRepository.GetAll().ToDictionary(k => k.Id, v => v);
                return new ConcurrentDictionary<int, Level>(levels);
            });
        }

        public IEnumerable<Course> GetAllCourses()
        {
            return this.coursesById.Value.Values;
        }

        public IEnumerable<Language> GetAllLanguages()
        {
            return this.languagesById.Value.Values;
        }

        public IEnumerable<LearningSystem> GetAllLearningSystems()
        {
            return this.learningSystemsById.Value.Values;
        }

        public IEnumerable<Level> GetAllLevels()
        {
            return this.levelsById.Value.Values;
        }

        public Course GetCourseById(int id, bool returnNull = false)
        {
            if (this.coursesById.Value.TryGetValue(id, out Course course))
            {
                return course;
            }
            else
            {
                if (returnNull)
                {
                    return null;
                }
                else
                {
                    throw new ApplicationException($"Course with id: {id} does not exist.");
                }
            }
        }

        public Language GetLanguagebyId(int id, bool returnNull = false)
        {
            if (this.languagesById.Value.TryGetValue(id, out Language language))
            {
                return language;
            }
            else
            {
                if (returnNull)
                {
                    return null;
                }
                else
                {
                    throw new ApplicationException($"Language with id: {id} does not exist.");
                }
            }
        }

        public LearningSystem GetLearningSystemById(int id, bool returnNull = false)
        {
            if (this.learningSystemsById.Value.TryGetValue(id, out LearningSystem learningSystem))
            {
                return learningSystem;
            }
            else
            {
                if (returnNull)
                {
                    return null;
                }
                else
                {
                    throw new ApplicationException($"LearningSystem with id: {id} does not exist.");
                }
            }
        }

        public Level GetLevelById(int id, bool returnNull = false)
        {
            if (this.levelsById.Value.TryGetValue(id, out Level level))
            {
                return level;
            }
            else
            {
                if (returnNull)
                {
                    return null;
                }
                else
                {
                    throw new ApplicationException($"Level with id: {id} does not exist.");
                }
            }
        }
    }
}
