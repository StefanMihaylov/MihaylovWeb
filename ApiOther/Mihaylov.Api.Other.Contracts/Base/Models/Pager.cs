namespace Mihaylov.Api.Other.Contracts.Base.Models
{
    public class Pager
    {
        public int? Page { get; private set; }

        public int? PageSize { get; private set; }

        public int Count { get; private set; }

        public int? PageMax
        {
            get
            {
                if (!Page.HasValue || !PageSize.HasValue)
                {
                    return null;
                }

                var maxPageCount = Count / PageSize.Value;
                if (Count % PageSize.Value > 0)
                {
                    maxPageCount++;
                }

                return maxPageCount;
            }
        }

        public Pager(GridRequest request, int count)
        {
            Page = request.Page;
            PageSize = request.PageSize;
            Count = count;
        }
    }
}
