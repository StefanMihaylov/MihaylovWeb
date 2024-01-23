using System.Net.Http;
using Mihaylov.Common.Host.AssemblyVersion.Models;

namespace Mihaylov.Api.Users.Client
{
    public partial class UsersApiClient : IUsersApiClient
    {
        public const string USERS_API_CLIENT_NAME = "UsersApiClient";

        public UsersApiClient(IHttpClientFactory httpClientFactory)
            : this(httpClientFactory.CreateClient(USERS_API_CLIENT_NAME))
        {
            _baseUrl = _httpClient.BaseAddress.AbsoluteUri;
        }
    }

    public partial class ModuleInfo : IModuleInfo
    {
    }
}
