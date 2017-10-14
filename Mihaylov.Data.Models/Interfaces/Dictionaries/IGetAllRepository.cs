using System.Collections.Generic;
using Mihaylov.Common.Database.Interfaces;
using Mihaylov.Data.Models.Dictionaries;

namespace Mihaylov.Data.Interfaces.Dictionaries
{
    public interface IGetAllRepository<T> where T : class // : IRepository<T2> where T2 : class
    {
        IEnumerable<T> GetAll();
    }
}