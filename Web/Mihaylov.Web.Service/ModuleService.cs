using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Users.Client;
using Mihaylov.Common.Host.AssemblyVersion.Interfaces;
using Mihaylov.Common.Host.AssemblyVersion.Models;
using Mihaylov.Web.Service.Interfaces;

namespace Mihaylov.Web.Service
{
    public class ModuleService : IModuleService
    {
        private readonly IModuleAssemblyService _module;
        private readonly IUsersApiClient _usersApiClient;

        public ModuleService(IModuleAssemblyService module, IUsersApiClient usersApiClient)
        {
            _module = module;
            _usersApiClient = usersApiClient;
        }

        public async Task<IEnumerable<IModuleInfo>> GetModuleVersionsAsync()
        {
            var result = new List<IModuleInfo>()
            {
                _module.GetModuleInfo(),
                await _usersApiClient.GetInfoAsync().ConfigureAwait(false),
            };

            return result;
        }
    }
}
