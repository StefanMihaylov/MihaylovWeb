using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Other.Client;
using Mihaylov.Api.Site.Client;
using Mihaylov.Api.Users.Client;
using Mihaylov.Api.Weather.Client;
using Mihaylov.Common.Generic.AssemblyVersion;
using Mihaylov.Common.Host.AssemblyVersion.Interfaces;
using AsModels = Mihaylov.Common.Host.AssemblyVersion.Models;

namespace Mihaylov.Web.Services
{
    public class ModuleService : IModuleService
    {
        private readonly ILogger _logger;
        private readonly IModuleAssemblyService _module;
        private readonly IUsersApiClient _usersApiClient;
        private readonly IWeatherApiClient _weatherApiClient;
        private readonly IOtherApiClient _otherApiClient;
        private readonly ISiteApiClient _siteApiClient;

        public ModuleService(ILoggerFactory loggerFactory, IModuleAssemblyService module, IUsersApiClient usersApiClient,
           IWeatherApiClient weatherApiClient, IOtherApiClient otherApiClient, ISiteApiClient siteApiClient)
        {
            _logger = loggerFactory.CreateLogger(this.GetType());
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
                await TryActionAsync(_usersApiClient.ModuleGetInfoAsync,"Users").ConfigureAwait(false),
                await TryActionAsync(_weatherApiClient.ModuleGetInfoAsync,"Weather").ConfigureAwait(false),
                await TryActionAsync(_otherApiClient.ModuleGetInfoAsync,"Other").ConfigureAwait(false),
                await TryActionAsync(_siteApiClient.ModuleGetInfoAsync,"Site").ConfigureAwait(false),
            };

            var result = new List<ModuleInfoModel>()
            {
                new ModuleInfoModel(_module.GetModuleInfo()),
            };

            result.AddRange(modules.OrderByDescending(m => m.BuildDate).Select(m => new ModuleInfoModel(m)));

            return result;
        }

        private async Task<IModuleInfo> TryActionAsync<T>(Func<Task<T>> action, string name) where T : IModuleInfo
        {
            try
            {
                var model = await action().ConfigureAwait(false);
                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Get Module Info failed. Error: {ex.Message}");
                return new AsModels.ModuleInfo(name, null, null, null, null, null);
            }
        }
    }
}
