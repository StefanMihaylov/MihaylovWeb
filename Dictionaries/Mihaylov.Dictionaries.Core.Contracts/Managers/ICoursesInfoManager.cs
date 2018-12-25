using System.Collections.Generic;
using Mihaylov.Dictionaries.Data.Models;

namespace Mihaylov.Dictionaries.Core.Interfaces
{
    public interface ICoursesInfoManager
    {
        Course GetCourseById(int id, bool returnNull = false);

        Language GetLanguagebyId(int id, bool returnNull = false);

        LearningSystem GetLearningSystemById(int id, bool returnNull = false);

        Level GetLevelById(int id, bool returnNull = false);

        IEnumerable<Course> GetAllCourses();

        IEnumerable<Language> GetAllLanguages();

        IEnumerable<LearningSystem> GetAllLearningSystems();

        IEnumerable<Level> GetAllLevels();
    }
}
