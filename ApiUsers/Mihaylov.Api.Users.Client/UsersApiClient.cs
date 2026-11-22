using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Common;

namespace Mihaylov.Api.Users.Client
{
    public partial interface IUsersApiClient : IModuleInfoService
    {
    }

    public partial class UsersApiClient : IUsersApiClient
    {
        public const string API_CLIENT = "UsersApiClient";

        public UsersApiClient(IHttpClientFactory httpClientFactory)
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

    public partial class LoginRequestModel
    {
        public ClaimType? ClaimTypesEnum
        {
            get
            {
                return (ClaimType?)this.ClaimTypes;
            }
            set
            {
                this.ClaimTypes = (int?)value;
            }
        }
    }

    public class UsersSettings : BaseApiSettings, IBaseApiSettings
    {
    }

    public static class UsersApiClientExtensions
    {
        public static IServiceCollection AddUsersApiClient(this IServiceCollection services, string moduleName)
        {
            services.AddApiClient<IUsersApiClient, UsersApiClient, UsersSettings>(UsersApiClient.API_CLIENT, moduleName);

            return services;
        }
    }
}
