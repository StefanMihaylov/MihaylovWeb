using System.Net.Http;
using System.Net.Http.Headers;
using Mihaylov.Common.Host.Abstract.AssemblyVersion;

namespace Mihaylov.Api.Other.Client
{
    public partial interface IOtherApiClient
    {
        public void AddToken(string token);
    }

    public partial class OtherApiClient : IOtherApiClient
    {
        public const string OTHER_API_CLIENT_NAME = "OtherApiClient";

        public OtherApiClient(IHttpClientFactory httpClientFactory)
            : this(httpClientFactory.CreateClient(OTHER_API_CLIENT_NAME))
        {
            _baseUrl = _httpClient.BaseAddress.AbsoluteUri;
        }

        public void AddToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public partial class ModuleInfo : IModuleInfo
    {
    }
}
