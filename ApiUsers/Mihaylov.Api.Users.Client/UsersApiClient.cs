using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mihaylov.Common;

namespace Mihaylov.Api.Users.Client
{
    public partial interface IUsersApiClient
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
    }

    public partial class ModuleInfo : IModuleInfo
    {
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

    public static class DependencyInjection
    {
        public static IServiceCollection AddUsersApiClient(this IServiceCollection services, Action<OptionsBuilder<UsersSettings>> bindSettings)
        {
            services.AddApiClient<IUsersApiClient, UsersApiClient, UsersSettings>(UsersApiClient.API_CLIENT, bindSettings);

            return services;
        }
    }
}
