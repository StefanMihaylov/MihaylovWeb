using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
    }

    public partial class ModuleInfo : IModuleInfo
    {
    }

    public partial class Pager : IPager
    {
    }

    public class OrdersSettings : BaseApiSettings, IBaseApiSettings
    {
    }

    public static class OtherApiClientExtensions
    {
        public static IServiceCollection AddOtherApiClient(this IServiceCollection services, Action<OptionsBuilder<OrdersSettings>> bindSettings)
        {
            services.AddApiClient<IOtherApiClient, OtherApiClient, OrdersSettings>(OtherApiClient.API_CLIENT, bindSettings);

            return services;
        }
    }
}
