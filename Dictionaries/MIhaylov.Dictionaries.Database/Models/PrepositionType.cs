using System;
using System.Collections.Generic;

namespace Mihaylov.Dictionaries.Database.Models
{
    public partial class PrepositionType
    {
        public int PrepositionTypeId { get; set; }
        public int LanguageId { get; set; }
        public string Value { get; set; }

        public virtual Language Language { get; set; }
    }
}
