using System.Collections.Generic;

namespace Mihaylov.Common
{
    /// <summary>
    /// A page of items together with its paging information
    /// </summary>
    /// <typeparam name="T">The type of the items.</typeparam>
    public class BaseGrid<T> where T : class
    {
        /// <summary>
        /// Gets or sets the items of the current page.
        /// </summary>
        public IEnumerable<T> Data { get; set; }

        /// <summary>
        /// Gets or sets the paging information.
        /// </summary>
        public Pager Pager { get; set; }
    }
}
