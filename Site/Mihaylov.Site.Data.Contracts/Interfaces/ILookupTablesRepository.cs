using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Data.Interfaces
{
    public interface ILookupTablesRepository
    {
        Task<IEnumerable<Ethnicity>> GetAllEthnicitiesAsync();

        Task<IEnumerable<Orientation>> GetAllOrientationsAsync();
    }
}