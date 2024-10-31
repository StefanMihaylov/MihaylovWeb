using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Mihaylov.Common.Generic.AssemblyVersion;

namespace Mihaylov.Api.Site.Client
{
    public partial interface ISiteApiClient
    {
        public void AddToken(string token);

        Task<PersonGrid> PersonsAsync(IGridRequest request, int? pageSize);
    }

    public partial class SiteApiClient : ISiteApiClient
    {
        public const string SITE_API_CLIENT_NAME = "SiteApiClient";

        public SiteApiClient(IHttpClientFactory httpClientFactory)
            : this(httpClientFactory.CreateClient(SITE_API_CLIENT_NAME))
        {
            _baseUrl = _httpClient.BaseAddress.AbsoluteUri;
        }

        public void AddToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public Task<PersonGrid> PersonsAsync(IGridRequest request, int? pageSize)
        {
            return PersonsAsync(request.AccountTypeId, request.StatusId, request.Name,
                request.AccountName, request.AccountNameExact, request.Page, pageSize);
        }
    }

    public partial class ModuleInfo : IModuleInfo
    {
    }


    public partial class GridRequest : IGridRequest
    {
        public long? PersonId => null;

        public bool? IsNewPerson => null;
    }

    public interface IGridRequest
    {
        public int? AccountTypeId { get; }

        public int? StatusId { get; }

        public string Name { get; }

        public string AccountName { get; }

        public string AccountNameExact { get; }

        public int? Page { get; set; }

        public long? PersonId { get; }

        public bool? IsNewPerson { get; }
    }
}
