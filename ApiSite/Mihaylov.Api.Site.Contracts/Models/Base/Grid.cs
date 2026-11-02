using Mihaylov.Common;

namespace Mihaylov.Api.Site.Contracts.Models.Base
{
    public class Grid<T> : BaseGrid<T> where T : class
    {
        public GridRequest Request { get; set; }
    }
}
