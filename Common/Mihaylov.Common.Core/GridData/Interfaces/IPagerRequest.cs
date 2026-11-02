namespace Mihaylov.Common
{
    /// <summary>
    /// Read-only paging part of a grid request.
    /// </summary>
    public interface IPagerRequest
    {
        /// <summary>
        /// The requested page number, or <c>null</c> when the caller did not specify one.
        /// </summary>
        public int? Page { get; }

        /// <summary>
        /// The requested number of items per page, or <c>null</c> when the caller did not specify one.
        /// </summary>
        public int? PageSize { get; }
    }
}
