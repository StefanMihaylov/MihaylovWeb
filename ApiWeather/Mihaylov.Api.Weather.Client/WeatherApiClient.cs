using System.Net.Http;
using Mihaylov.Common.Generic.AssemblyVersion;

namespace Mihaylov.Api.Weather.Client
{
    public partial class WeatherApiClient : IWeatherApiClient
    {
        public const string WEATHER_API_CLIENT_NAME = "WeatherApiClient";

        public WeatherApiClient(IHttpClientFactory httpClientFactory)
            : this(httpClientFactory.CreateClient(WEATHER_API_CLIENT_NAME))
        {
            BaseUrl = _httpClient.BaseAddress.AbsoluteUri;
        }
    }

    public partial class ModuleInfo : IModuleInfo
    {
    }
}
