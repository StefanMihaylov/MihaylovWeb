using Microsoft.AspNetCore.SignalR;

namespace Mihaylov.UI.Mvc.Home.Hubs
{
    public class ScanProgressHub : Hub
    {
        public const string HUB_PATH = "/ScanProgressHub";

        public const string JS_FUNC_NAME = "updateLoadingBar";
    }
}
