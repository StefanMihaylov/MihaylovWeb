using System.Collections.Generic;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Interfaces
{
    public interface IOrientationsManager
    {
        IEnumerable<Orientation> GetAllorientations();

        Orientation GetById(int id);

        Orientation GetByName(string name);
    }
}