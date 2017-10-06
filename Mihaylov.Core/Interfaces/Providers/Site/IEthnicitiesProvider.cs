using System.Collections.Generic;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Interfaces.Site
{
    public interface IEthnicitiesProvider
    {
        IEnumerable<Ethnicity> GetAllEthnicities();
    }
}