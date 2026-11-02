namespace Mihaylov.Common
{
    /// <summary>
    /// Paging information returned with a grid. The page count is not stored here, the UI calculates it
    /// from <see cref="Count"/> and <see cref="PageSize"/>.
    /// </summary>
    public class Pager : IPager
    {
        /// <inheritdoc />
        public int Page { get; private set; }

        /// <inheritdoc />
        public int? PageSize { get; private set; }

        /// <inheritdoc />
        public int Count { get; private set; }

        /// <summary>
        /// Initializes an empty pager, for serialization.
        /// </summary>
        public Pager()
        {
        }

        /// <summary>
        /// Initializes a pager with explicit values.
        /// </summary>
        /// <param name="page">The current page number, starting at 1.</param>
        /// <param name="pageSize">The number of items per page, or <c>null</c>.</param>
        /// <param name="count">The total number of items across all pages.</param>
        public Pager(int page, int? pageSize, int count)
        {
            Page = page;
            PageSize = pageSize;
            Count = count;
        }
    }
}
