using System;
using System.Collections.Generic;

namespace Mihaylov.Dictionaries.Database.Models
{
    public partial class Language
    {
        public Language()
        {
            LearningSystems = new HashSet<LearningSystem>();
            PrepositionTypes = new HashSet<PrepositionType>();
        }

        public int LanguageId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<LearningSystem> LearningSystems { get; set; }
        public virtual ICollection<PrepositionType> PrepositionTypes { get; set; }
    }
}
