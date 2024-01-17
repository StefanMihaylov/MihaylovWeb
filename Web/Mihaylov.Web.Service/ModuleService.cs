using System.Collections.Generic;
using Mihaylov.Common.Host.AssemblyVersion.Interfaces;
using Mihaylov.Web.Service.Interfaces;
using Mihaylov.Common.Host.AssemblyVersion.Models;
using Users = Mihaylov.Api.Users.Client;
using System.Threading.Tasks;
using System;

namespace Mihaylov.Web.Service
{
    public class ModuleService : IModuleService
    {
        private readonly IModuleAssemblyService _module;
        private readonly Users.IUsersApiClient _usersApiClient;

        public ModuleService(IModuleAssemblyService module, Users.IUsersApiClient usersApiClient)
        {
            _module = module;
            _usersApiClient = usersApiClient;
        }

        public async Task<IEnumerable<ModuleInfo>> GetModuleVersionsAsync()
        {
            var result = new List<ModuleInfo>()
            {
                _module.GetModuleInfo(),
                await MapAsync(_usersApiClient.GetInfoAsync).ConfigureAwait(false),
            };

            return result;
        }

        private async Task<ModuleInfo> MapAsync<T>(Func<Task<T>> method) where T : class
        {
            var input = await method().ConfigureAwait(false);

            var inputType = typeof(T);
            var moduleName = (string)inputType.GetProperty("ModuleName").GetValue(input, null);
            var version = (string)inputType.GetProperty("Version").GetValue(input, null);
            var framework = (string)inputType.GetProperty("Framework").GetValue(input, null);
            var buildDate = (string)inputType.GetProperty("BuildDate").GetValue(input, null);

            return new ModuleInfo(moduleName, version, framework, buildDate);
        }
    }
}
