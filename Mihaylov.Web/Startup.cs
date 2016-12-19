using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Mihaylov.Web.Startup))]
namespace Mihaylov.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
