using System.Collections.Generic;
using Mihaylov.Common.Host.AssemblyVersion.Models;

namespace Mihaylov.Web.Service.Interfaces
{
    public interface IModuleService
    {
        IEnumerable<ModuleInfo> GetModuleVersions();
    }
}