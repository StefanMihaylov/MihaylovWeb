using System;
using System.Collections.Generic;
using Mihaylov.Site.Database.Models.Base;

namespace Mihaylov.Site.Database.Models
{
    public class AnswerType : LookupTable
    {
        public bool IsAsked { get; set; }
    }
}
