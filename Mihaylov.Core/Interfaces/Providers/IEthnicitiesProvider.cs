using System.Collections.Generic;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Interfaces
{
    public interface IEthnicitiesProvider
    {
        IEnumerable<Ethnicity> GetAllEthnicities();
    }
}