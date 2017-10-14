using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Mihaylov.Core.Interfaces.Dictionaries;
using Mihaylov.Data.Models.Dictionaries;

namespace Mihaylov.Core.Managers.Dictionaries
{
    public class CoursesInfoManager : ICoursesInfoManager
    {
        private readonly ICoursesInfoProvider coursesInfoProvider;

        private readonly Lazy<ConcurrentDictionary<int, Course>> coursesById;
        private readonly Lazy<ConcurrentDictionary<int, Language>> languagesById;
        private readonly Lazy<ConcurrentDictionary<int, LearningSystem>> learningSystemsById;
        private readonly Lazy<ConcurrentDictionary<int, Level>> levelsById;

        public CoursesInfoManager(ICoursesInfoProvider coursesInfoProvider)
        {
            this.coursesInfoProvider = coursesInfoProvider;

            this.coursesById = new Lazy<ConcurrentDictionary<int, Course>>(() =>
            {
                var courses = this.coursesInfoProvider.GetAllCourses().ToDictionary(k => k.Id, v => v);
                return new ConcurrentDictionary<int, Course>(courses);
            });

            this.languagesById = new Lazy<ConcurrentDictionary<int, Language>>(() =>
            {
                var languages = this.coursesInfoProvider.GetAllLanguages().ToDictionary(k => k.Id, v => v);
                return new ConcurrentDictionary<int, Language>(languages);
            });

            this.learningSystemsById = new Lazy<ConcurrentDictionary<int, LearningSystem>>(() =>
            {
                var learningSystems = this.coursesInfoProvider.GetAllLearningSystems().ToDictionary(k => k.Id, v => v);
                return new ConcurrentDictionary<int, LearningSystem>(learningSystems);
            });

            this.levelsById = new Lazy<ConcurrentDictionary<int, Level>>(() =>
            {
                var levels = this.coursesInfoProvider.GetAllLevels().ToDictionary(k => k.Id, v => v);
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
