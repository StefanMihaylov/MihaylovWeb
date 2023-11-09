using System;
using System.Collections.Generic;

namespace Mihaylov.Dictionaries.Database.Models
{
    public partial class Test
    {
        public Test()
        {
            IncorrectAnswers = new HashSet<IncorrectAnswer>();
            InversePreviousTest = new HashSet<Test>();
        }

        public int TestId { get; set; }
        public int? PreviousTestId { get; set; }
        public string UserId { get; set; }
        public int CourseId { get; set; }
        public int? ModuleStartNumber { get; set; }
        public int? ModuleEndNumber { get; set; }
        public DateTime StartDate { get; set; }
        public int? CorrectAnswers { get; set; }
        public int? TotalRecords { get; set; }

        public virtual Course Course { get; set; }
        public virtual Test PreviousTest { get; set; }
        public virtual ICollection<IncorrectAnswer> IncorrectAnswers { get; set; }
        public virtual ICollection<Test> InversePreviousTest { get; set; }
    }
}
