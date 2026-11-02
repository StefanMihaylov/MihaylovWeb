namespace Mihaylov.Common
{
    /// <summary>
    /// Read-only paging information of a grid. The API <see cref="Pager"/> and the pager classes generated for
    /// the API clients can both implement it, so shared UI code can work with either.
    /// </summary>
    public interface IPager
    {
        /// <summary>
        /// Gets the total number of items across all pages.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the current page number, starting at 1.
        /// </summary>
        int Page { get; }

        /// <summary>
        /// Gets the number of items per page, or <c>null</c> when none was requested.
        /// </summary>
        int? PageSize { get; }
    }
}