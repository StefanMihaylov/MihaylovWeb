using System;
using System.Collections.Generic;
using System.Text;

namespace Mihaylov.Api.Site.Contracts.Models.Base
{
    public class Grid<T> where T : class
    {
        public IEnumerable<T> Data { get; set; }

        public Pager Pager { get; set; }
    }
}
