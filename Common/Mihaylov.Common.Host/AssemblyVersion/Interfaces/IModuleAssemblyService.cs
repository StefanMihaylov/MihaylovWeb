using Mihaylov.Common.Generic.AssemblyVersion;

namespace Mihaylov.Common.Host.AssemblyVersion.Interfaces
{
    public interface IModuleAssemblyService
    {
        IModuleInfo GetModuleInfo();
    }
}