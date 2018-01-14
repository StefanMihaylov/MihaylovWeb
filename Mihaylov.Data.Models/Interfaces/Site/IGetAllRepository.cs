using System.Collections.Generic;
using Mihaylov.Common.Database.Interfaces;

namespace Mihaylov.Data.Interfaces.Site
{
    public interface IGetAllRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
    }
}
