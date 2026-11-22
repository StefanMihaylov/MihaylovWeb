using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Common;

namespace Mihaylov.Api.Other.Client
{
    public partial interface IOtherApiClient : IModuleInfoService
    {
    }

    public partial class OtherApiClient : IOtherApiClient
    {
        public const string API_CLIENT = "OtherApiClient";

        public OtherApiClient(IHttpClientFactory httpClientFactory)
            : this(httpClientFactory.CreateClient(API_CLIENT))
        {
            BaseUrl = _httpClient.BaseAddress.AbsoluteUri;
        }

        public async Task<IModuleInfo> GetInfoAsync()
        {
            return await this.ModuleGetInfoAsync();
        }

        public async Task<DependencyNode> GetDependenciesAsync()
        {
            var flat = await this.DependenciesGetInfoAsync();
            return flat.ToTreeModel();
        }
    }

    public partial class ModuleInfo : IModuleInfo
    {
    }

    public partial class DependencyNodeFlat : IDependencyNodeFlat
    {
        Common.DependencyKind IDependencyNodeFlat.Kind => (Common.DependencyKind)(int)Kind;
    }

    public partial class NexusNugetImage
    {
        private IEnumerable<DependencyNode> _dependencyNodes;

        public IEnumerable<DependencyNode> DependencyNodes => _dependencyNodes ??= this.Dependencies.ToTreeModels();
    }

    public partial class Pager : IPager
    {
    }

    public class OrdersSettings : BaseApiSettings, IBaseApiSettings
    {
    }

    public static class OtherApiClientExtensions
    {
        public static IServiceCollection AddOtherApiClient(this IServiceCollection services, string moduleName)
        {
            services.AddApiClient<IOtherApiClient, OtherApiClient, OrdersSettings>(OtherApiClient.API_CLIENT, moduleName);

            return services;
        }
    }
}
