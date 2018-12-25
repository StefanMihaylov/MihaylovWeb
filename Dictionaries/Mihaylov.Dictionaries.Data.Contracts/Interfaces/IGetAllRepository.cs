using System.Collections.Generic;

namespace Mihaylov.Dictionaries.Data.Interfaces
{
    public interface IGetAllRepository<T> where T : class // : IRepository<T2> where T2 : class
    {
        IEnumerable<T> GetAll();
    }
}