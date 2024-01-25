using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Web.Service.Interfaces;
using Mihaylov.WebUI.Models;

namespace Mihaylov.WebUI.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currrent = await _weatherService.GetCurrentWeatherAsync().ConfigureAwait(false);

            var result = new CurrentWeatherModel()
            {
                Current = currrent,
            };

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewCurrentCity(string city)
        {
            var response = await _weatherService.GetCurrentWeatherAsync(city).ConfigureAwait(false);

            return PartialView("_CurrentWeatherRow", response);
        }
    }
}
