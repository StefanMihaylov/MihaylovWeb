using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mihaylov.Common;

namespace Mihaylov.Api.Site.Client
{
    public partial interface ISiteApiClient : IModuleInfoService
    {
        Task<PersonGrid> PersonsAsync(IGridRequest request, int? pageSize);
    }

    public partial class SiteApiClient : ISiteApiClient
    {
        public const string API_CLIENT = "SiteApiClient";

        public SiteApiClient(IHttpClientFactory httpClientFactory)
            : this(httpClientFactory.CreateClient(API_CLIENT))
        {
            BaseUrl = _httpClient.BaseAddress.AbsoluteUri;
        }

        public async Task<IModuleInfo> GetInfoAsync()
        {
            return await this.ModuleGetInfoAsync();
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

    public partial class Pager : IPager
    {
    }

    public partial class GridRequest : IGridRequest, IPagerRequest
    {
        public long? PersonId => null;

        public bool? IsNewPerson => null;
    }

    public interface IGridRequest : IQueryRequest
    {
        public int? AccountTypeId { get; }

        public int? StatusId { get; }

        public string Name { get; }

        public string AccountName { get; }

        public string AccountNameExact { get; }

        public int? Page { get; }

        public long? PersonId { get; }

        public bool? IsNewPerson { get; }
    }

    public class SiteSettings : BaseApiSettings, IBaseApiSettings
    {
    }

    public static class SiteApiClientExtensions
    {
        public static IServiceCollection AddSiteApiClient(this IServiceCollection services, Action<OptionsBuilder<SiteSettings>> bindSettings)
        {
            services.AddApiClient<ISiteApiClient, SiteApiClient, SiteSettings>(SiteApiClient.API_CLIENT, bindSettings);

            return services;
        }
    }
}
