using System;
using System.Collections.Generic;

namespace Mihaylov.Dictionaries.Database.Models
{
    public partial class Level
    {
        public Level()
        {
            Courses = new HashSet<Course>();
        }

        public int LevelId { get; set; }
        public int LearningSystemId { get; set; }
        public string Name { get; set; }
        public string Descrition { get; set; }
        public int? ModulesStartNumber { get; set; }
        public int? ModulesEndNumber { get; set; }

        public virtual LearningSystem LearningSystem { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
