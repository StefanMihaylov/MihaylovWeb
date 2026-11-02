using Microsoft.AspNetCore.SignalR;

namespace Mihaylov.Api.Site.Hubs
{
    public class UpdateProgressHub : Hub
    {
        public const string JS_FUNC_NAME = "updateAccountsLoadingBar";
    }
}
