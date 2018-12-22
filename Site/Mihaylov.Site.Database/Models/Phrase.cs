using System;
using System.Collections.Generic;

namespace Mihaylov.Site.Database.Models
{
    public partial class Phrase
    {
        public int PhraseId { get; set; }
        public string Text { get; set; }
        public int OrderId { get; set; }
    }
}
