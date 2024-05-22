using System.Collections.Generic;

namespace Mihaylov.Api.Other.Contracts.Base.Models
{
    public class Grid<T> where T : class
    {
        public IEnumerable<T> Data { get; set; }

        public Pager Pager { get; set; }
    }
}
