using Mihaylov.Core.Interfaces.Dictionaries;
using Mihaylov.Data.Models.Dictionaries;

namespace Mihaylov.Core.Interfaces.Dictionaries
{
    public interface ICoursesInfoManager : ICoursesInfoProvider
    {
        Course GetCourseById(int id, bool returnNull = false);

        Language GetLanguagebyId(int id, bool returnNull = false);

        LearningSystem GetLearningSystemById(int id, bool returnNull = false);

        Level GetLevelById(int id, bool returnNull = false);
    }
}
