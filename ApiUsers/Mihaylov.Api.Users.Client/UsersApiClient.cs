using System.Net.Http;
using Mihaylov.Common.Host.Abstract.AssemblyVersion;
using Mihaylov.Common.Host.Abstract.Authorization;

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
