using System.Collections.Generic;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Interfaces
{
    public interface IUnitsManager
    {
        IEnumerable<Unit> GetAllUnits();

        Unit GetById(int id);

        Unit GetByName(string name);
    }
}
