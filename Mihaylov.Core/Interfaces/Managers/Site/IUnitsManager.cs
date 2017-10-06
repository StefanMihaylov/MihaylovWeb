using System.Collections.Generic;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Interfaces.Site
{
    public interface IUnitsManager
    {
        IEnumerable<Unit> GetAllUnits();

        Unit GetById(int id);

        Unit GetByName(string name);
    }
}
