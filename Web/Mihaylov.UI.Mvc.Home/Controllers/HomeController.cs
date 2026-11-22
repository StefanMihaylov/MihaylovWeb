using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Common;
using Mihaylov.UI.Mvc.Home.Models.Home;
using Mihaylov.UI.Mvc.Home.Models.Media;

namespace Mihaylov.UI.Mvc.Home.Controllers
{
    public class HomeController(IServiceScopeFactory factory, IModuleCatalog catalog) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [AdminAuthorize]
        public async Task<IActionResult> ModuleInfo()
        {
            var modulesInfo = await catalog.GetModuleVersionsAsync(factory).ConfigureAwait(false);

            var result = modulesInfo.OrderByDescending(m => m.ServerModuleInfo.BuildDate)
                                    .Select(m => new ModuleInfoModel(m))
                                    .ToList();

            return View(result);
        }

        [AdminAuthorize]
        public async Task<IActionResult> Dependencies()
        {
            var dependencies = await catalog.GetModuleDependenciesAsync(factory).ConfigureAwait(false);

            var result = catalog.Modules
                                .Zip(dependencies, (module, component) => new ModuleDependenciesModel(module.Name, component))
                                .ToList();

            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}