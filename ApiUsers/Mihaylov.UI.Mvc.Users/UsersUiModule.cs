using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Users.Client;
using Mihaylov.Common;

namespace Mihaylov.UI.Mvc.Users;

public class UsersUiModule : BaseUiModule, IUiModule
{
    public UsersUiModule() : base("Users", typeof(IUsersApiClient), true, "_IdentityMenu")
    {
    }

    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IAccessTokenProvider, CookieTokenProvider>();

        services.AddUsersApiClient(Name);
    }

    public override IEnumerable<MenuItem> Menus => null;
}
