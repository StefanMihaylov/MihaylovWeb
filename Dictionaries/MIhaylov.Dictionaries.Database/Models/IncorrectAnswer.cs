using System;
using System.Collections.Generic;

namespace Mihaylov.Dictionaries.Database.Models
{
    public partial class IncorrectAnswer
    {
        public int TestId { get; set; }
        public int RecordId { get; set; }

        public virtual Record Record { get; set; }
        public virtual Test Test { get; set; }
    }
}
