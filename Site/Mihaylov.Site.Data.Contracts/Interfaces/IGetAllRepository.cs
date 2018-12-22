using System.Collections.Generic;
using Mihaylov.Common.Database.Interfaces;

namespace Mihaylov.Site.Data.Interfaces
{
    public interface IGetAllRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
    }
}
