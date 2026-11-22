using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mihaylov.Common;

namespace Mihaylov.Api.Gear.Client
{
    public partial interface IGearApiClient : IModuleInfoService
    {
    }

    public partial class GearApiClient : IGearApiClient
    {
        public const string API_CLIENT = "GearApiClient";

        public GearApiClient(IHttpClientFactory httpClientFactory)
            : this(httpClientFactory.CreateClient(API_CLIENT))
        {
            BaseUrl = _httpClient.BaseAddress?.AbsoluteUri;
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

    public interface ISmallItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public partial class Group : ISmallItem
    {
    }

    public partial class Brand : ISmallItem
    {
    }

    public partial class Shop : ISmallItem
    {
    }

    public partial class Category : ISmallItem
    {
    }


    public class GearSettings : BaseApiSettings, IBaseApiSettings
    {
    }

    public static class GearApiClientExtensions
    {
        public static IServiceCollection AddGearApiClient(this IServiceCollection services, string moduleName)
        {
            services.AddApiClient<IGearApiClient, GearApiClient, GearSettings>(GearApiClient.API_CLIENT, moduleName);

            return services;
        }
    }
}
