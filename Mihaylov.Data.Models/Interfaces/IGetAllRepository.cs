using System.Collections.Generic;

namespace Mihaylov.Data.Models.Interfaces
{
    public interface IGetAllRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
    }
}
