using System.Collections.Generic;
using System;

namespace Mihaylov.Web.Models.Media
{
    public class MovePicsViewModel
    {
        public string Dir { get; set; }

        public IEnumerable<Guid> Files { get; set; }        
    }
}
