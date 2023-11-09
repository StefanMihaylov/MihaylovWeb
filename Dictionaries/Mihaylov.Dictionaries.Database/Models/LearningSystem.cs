using System;
using System.Collections.Generic;

namespace Mihaylov.Dictionaries.Database.Models
{
    public partial class LearningSystem
    {
        public LearningSystem()
        {
            Levels = new HashSet<Level>();
        }

        public int LearningSystemId { get; set; }
        public int LanguageId { get; set; }
        public string Name { get; set; }

        public virtual Language Language { get; set; }
        public virtual ICollection<Level> Levels { get; set; }
    }
}
