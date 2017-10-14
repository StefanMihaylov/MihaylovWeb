using System.Collections.Generic;
using Mihaylov.Data.Models.Dictionaries;

namespace Mihaylov.Core.Interfaces.Dictionaries
{
    public interface ICoursesInfoProvider
    {
        IEnumerable<Course> GetAllCourses();
        IEnumerable<Language> GetAllLanguages();
        IEnumerable<LearningSystem> GetAllLearningSystems();
        IEnumerable<Level> GetAllLevels();
    }
}