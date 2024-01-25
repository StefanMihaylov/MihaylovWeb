using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Web.Service.Models;

namespace Mihaylov.Web.Service.Interfaces
{
    public interface IModuleService
    {
        Task<IEnumerable<ModuleInfoModel>> GetModuleVersionsAsync();
    }
}