using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mihaylov.Api.Other.Client;
using Mihaylov.Api.Site.Client;
using Mihaylov.Api.Users.Client;
using Mihaylov.Api.Weather.Client;
using Mihaylov.Common.Host.Abstract.AssemblyVersion;

namespace Mihaylov.Web.Services
{
    public class ModuleService : IModuleService
    {
        private readonly IModuleAssemblyService _module;
        private readonly IUsersApiClient _usersApiClient;
        private readonly IWeatherApiClient _weatherApiClient;
        private readonly IOtherApiClient _otherApiClient;
        private readonly ISiteApiClient _siteApiClient;

        public ModuleService(IModuleAssemblyService module, IUsersApiClient usersApiClient, IWeatherApiClient weatherApiClient,
            IOtherApiClient otherApiClient, ISiteApiClient siteApiClient)
        {
            _module = module;
            _usersApiClient = usersApiClient;
            _weatherApiClient = weatherApiClient;
            _otherApiClient = otherApiClient;
            _siteApiClient = siteApiClient;
        }

        public async Task<IEnumerable<ModuleInfoModel>> GetModuleVersionsAsync()
        {
            var modules = new List<IModuleInfo>()
            {
                await _usersApiClient.ModuleGetInfoAsync().ConfigureAwait(false),
                await _weatherApiClient.ModuleGetInfoAsync().ConfigureAwait(false),
                await _otherApiClient.ModuleGetInfoAsync().ConfigureAwait(false),
                await _siteApiClient.ModuleGetInfoAsync().ConfigureAwait(false),
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
