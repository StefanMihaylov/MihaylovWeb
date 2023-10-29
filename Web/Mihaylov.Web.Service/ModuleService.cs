using System.Collections.Generic;
using Mihaylov.Common.Host.AssemblyVersion.Interfaces;
using Mihaylov.Web.Service.Interfaces;
using Mihaylov.Common.Host.AssemblyVersion.Models;

namespace Mihaylov.Web.Service
{
    public class ModuleService : IModuleService
    {
        private readonly IModuleAssemblyService _module;

        public ModuleService(IModuleAssemblyService module)
        {
            _module = module;
        }

        public IEnumerable<ModuleInfo> GetModuleVersions()
        {
            var result = new List<ModuleInfo>()
            {
                _module.GetModuleInfo()
            };

            return result;
        }
    }
}
