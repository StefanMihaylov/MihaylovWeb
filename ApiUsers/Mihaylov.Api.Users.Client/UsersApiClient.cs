using System.Net.Http;
using System.Net.Http.Headers;
using Mihaylov.Common.Host.Abstract.AssemblyVersion;
using Mihaylov.Common.Host.Abstract.Authorization;

namespace Mihaylov.Api.Users.Client
{
    public partial interface IUsersApiClient
    {
        public void AddToken (string token);
    }

    public partial class UsersApiClient : IUsersApiClient
    {
        public const string USERS_API_CLIENT_NAME = "UsersApiClient";

        public UsersApiClient(IHttpClientFactory httpClientFactory)
            : this(httpClientFactory.CreateClient(USERS_API_CLIENT_NAME))
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
}
