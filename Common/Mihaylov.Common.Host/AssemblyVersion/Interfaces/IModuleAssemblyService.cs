using Mihaylov.Common.Host.AssemblyVersion.Models;

namespace Mihaylov.Common.Host.AssemblyVersion.Interfaces
{
    public interface IModuleAssemblyService
    {
        ModuleInfo GetModuleInfo();
    }
}