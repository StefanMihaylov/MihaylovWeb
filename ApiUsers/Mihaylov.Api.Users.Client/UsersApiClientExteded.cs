using System.Net.Http;

namespace Mihaylov.Api.Users.Client
{
    public partial class UsersApiClient : IUsersApiClient
    {
        public const string USERS_API_CLIENT_NAME = "UsersApiClient";

        public UsersApiClient(IHttpClientFactory httpClientFactory)
            :this(httpClientFactory.CreateClient(USERS_API_CLIENT_NAME))
        {            
        }
    }
}
