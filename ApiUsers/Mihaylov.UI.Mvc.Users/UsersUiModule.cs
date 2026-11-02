using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Users.Client;
using Mihaylov.Common;

namespace Mihaylov.UI.Mvc.Users;

public class UsersUiModule : BaseUiModule, IUiModule
{
    public UsersUiModule() : base("Users", true, "_IdentityMenu")
    {
    }

    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IAccessTokenProvider, CookieTokenProvider>();

        services.AddUsersApiClient(builder => builder.BindSettings(Name));
    }

    public override async Task<ComponentModuleInfo> GetModuleInfoAsync(IServiceScopeFactory factory)
    {
        return await this.GetModelInfoAsync<IUsersApiClient>(factory).ConfigureAwait(false);
    }

    public override IEnumerable<MenuItem> Menus => null;
}
