using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mihaylov.Web.Service.Interfaces;
using Mihaylov.WebUI.Models;

namespace Mihaylov.WebUI.Controllers
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

        public IActionResult Privacy()
        {
            var moduleVersions = _moduleService.GetModuleVersions();
            return View(moduleVersions);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
