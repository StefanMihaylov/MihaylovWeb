using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Mihaylov.WebUI.Areas.Identity.IdentityHostingStartup))]
namespace Mihaylov.WebUI.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}
