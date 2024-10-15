using System.Collections.Generic;

namespace Mihaylov.Api.Site.Contracts.Models.Base
{
    public class Grid<T> where T : class
    {
        public GridRequest Request { get; set; }

        public IEnumerable<T> Data { get; set; }

        public Pager Pager { get; set; }        
    }
}
