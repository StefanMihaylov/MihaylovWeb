namespace Mihaylov.Common
{
    /// <summary>
    /// Paging part of a grid request. Extend it with the filters of a specific grid.
    /// </summary>
    public class BaseGridRequest : IPagerRequest
    {
        /// <inheritdoc />
        public int? Page { get; set; }

        /// <inheritdoc />
        public int? PageSize { get; set; }
    }
}
