using System;
using System.Collections.Generic;
using System.Text;

namespace Mihaylov.Api.Site.Contracts.Models.Base
{
    public class Pager
    {
        public int? Page { get; private set; }

        public int? PageSize { get; private set; }

        public int Count { get; private set; }

        public int? PageMax => GetMaxPage(PageSize, Count);


        public Pager(int? page, int? pageSize, int count)
        {
            Page = page;
            PageSize = pageSize;
            Count = count;
        }

        public static int? GetMaxPage(int? pageSize, int count)
        {
            if (!pageSize.HasValue)
            {
                return null;
            }

            int maxPageCount = count / pageSize.Value;
            if (count % pageSize.Value > 0)
            {
                maxPageCount++;
            }

            return maxPageCount;
        }
    }
}
