using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mihaylov.Web.Models;
using Mihaylov.Web.Services;

namespace Mihaylov.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IModuleService _moduleService;

        public HomeController(ILogger<HomeController> logger, IModuleService moduleService)
        {
            _logger = logger;
            _moduleService = moduleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ModuleInfo()
        {
            var moduleVersions = await _moduleService.GetModuleVersionsAsync().ConfigureAwait(false);
            return View(moduleVersions);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
