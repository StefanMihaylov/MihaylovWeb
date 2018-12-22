using System.Collections.Generic;
using Mihaylov.Data.Models.Dictionaries;

namespace Mihaylov.Web.ViewModels.Dictionaries
{
    public class ChooseCourseExtendedModel : ChooseCourseModel
    {
        public IEnumerable<Course> Courses { get; set; }
    }
}