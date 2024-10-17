using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mihaylov.Web.Services
{
    public interface IModuleService
    {
        Task<IEnumerable<ModuleInfoModel>> GetModuleVersionsAsync();
    }
}
