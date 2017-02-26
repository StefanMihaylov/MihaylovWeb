using System.Collections.Generic;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Interfaces
{
    public interface IEthnicitiesManager
    {
        IEnumerable<Ethnicity> GetAllEthnicities();

        Ethnicity GetById(int id);

        Ethnicity GetByName(string name);
    }
}