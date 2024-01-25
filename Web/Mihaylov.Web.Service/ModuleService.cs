using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mihaylov.Api.Users.Client;
using Mihaylov.Api.Weather.Client;
using Mihaylov.Common.Host.AssemblyVersion.Interfaces;
using Mihaylov.Common.Host.AssemblyVersion.Models;
using Mihaylov.Web.Service.Interfaces;
using Mihaylov.Web.Service.Models;

namespace Mihaylov.Web.Service
{
    public class ModuleService : IModuleService
    {
        private readonly IModuleAssemblyService _module;
        private readonly IUsersApiClient _usersApiClient;
        private readonly IWeatherApiClient _weatherApiClient;

        public ModuleService(IModuleAssemblyService module, IUsersApiClient usersApiClient, IWeatherApiClient weatherApiClient)
        {
            _module = module;
            _usersApiClient = usersApiClient;
            _weatherApiClient = weatherApiClient;
        }

        public async Task<IEnumerable<ModuleInfoModel>> GetModuleVersionsAsync()
        {
            var modules = new List<IModuleInfo>()
            {
                await _usersApiClient.GetInfoAsync().ConfigureAwait(false),
                await _weatherApiClient.GetInfoAsync().ConfigureAwait(false),
            };

            var result = new List<ModuleInfoModel>()
            {
                new ModuleInfoModel(_module.GetModuleInfo()),
            };

            result.AddRange(modules.OrderByDescending(m => m.BuildDate).Select(m => new ModuleInfoModel(m)));

            return result;
        }
    }
}
