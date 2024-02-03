using System.Net.Http;
using Mihaylov.Common.Host.Abstract.AssemblyVersion;

namespace Mihaylov.Api.Weather.Client
{
    public partial class WeatherApiClient : IWeatherApiClient
    {
        public const string WEATHER_API_CLIENT_NAME = "WeatherApiClient";

        public WeatherApiClient(IHttpClientFactory httpClientFactory)
            : this(httpClientFactory.CreateClient(WEATHER_API_CLIENT_NAME))
        {
            _baseUrl = _httpClient.BaseAddress.AbsoluteUri;
        }
    }

    public partial class ModuleInfo : IModuleInfo
    {
    }
}
