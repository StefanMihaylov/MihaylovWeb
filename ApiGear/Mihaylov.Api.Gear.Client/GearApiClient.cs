using System.Net.Http.Headers;
using Mihaylov.Common.Generic.AssemblyVersion;

namespace Mihaylov.Api.Gear.Client;

public partial interface IGearApiClient
{
    public void AddToken(string token);
}

public partial class GearApiClient : IGearApiClient
{
    public const string GEAR_API_CLIENT_NAME = "GearApiClient";

    public GearApiClient(IHttpClientFactory httpClientFactory)
        : this(httpClientFactory.CreateClient(GEAR_API_CLIENT_NAME))
    {
        BaseUrl = _httpClient.BaseAddress?.AbsoluteUri;
    }

    public void AddToken(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}

public partial class ModuleInfo : IModuleInfo;

public interface ISmallItem
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public partial class Group : ISmallItem;
public partial class Brand : ISmallItem;
public partial class Shop: ISmallItem;
public partial class Category : ISmallItem;


