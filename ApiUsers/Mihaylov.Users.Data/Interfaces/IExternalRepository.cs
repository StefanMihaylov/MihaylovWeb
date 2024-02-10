using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Users.Models.Responses;

namespace Mihaylov.Users.Data.Interfaces
{
    public interface IExternalRepository
    {
        Task<IEnumerable<SchemeModel>> GetExternalAuthenticationSchemesAsync();
    }
}