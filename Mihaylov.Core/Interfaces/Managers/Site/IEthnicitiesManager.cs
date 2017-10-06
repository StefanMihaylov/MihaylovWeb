using System.Collections.Generic;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Interfaces.Site
{
    public interface IEthnicitiesManager
    {
        IEnumerable<Ethnicity> GetAllEthnicities();

        Ethnicity GetById(int id);

        Ethnicity GetByName(string name);
    }
}