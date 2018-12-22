using System;
using System.Collections.Generic;

namespace Mihaylov.Dictionaries.Database.Models
{
    public partial class Course
    {
        public Course()
        {
            Records = new HashSet<Record>();
            Tests = new HashSet<Test>();
        }

        public int CourseId { get; set; }
        public int LevelId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual Level Level { get; set; }
        public virtual ICollection<Record> Records { get; set; }
        public virtual ICollection<Test> Tests { get; set; }
    }
}
