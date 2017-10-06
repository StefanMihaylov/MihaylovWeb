using System.Collections.Generic;

namespace Mihaylov.Data.Interfaces.Site
{
    public interface IGetAllRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
    }
}
