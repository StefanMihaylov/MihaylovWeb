using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Common.Host.AssemblyVersion.Models;

namespace Mihaylov.Web.Service.Interfaces
{
    public interface IModuleService
    {
        Task<IEnumerable<ModuleInfo>> GetModuleVersionsAsync();
    }
}