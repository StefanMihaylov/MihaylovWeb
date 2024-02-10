using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Web.Models;
using Mihaylov.Web.Service.Interfaces;

namespace Mihaylov.Web.Controllers
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
            var weather = await _weatherService.GetForecastWeatherAsync().ConfigureAwait(false);

            var result = new WeatherModel()
            {
                Current = weather.Select(w => w.Current),
                Forecast = weather,
            };

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewCurrentCity(string city)
        {
            var response = await _weatherService.GetCurrentWeatherAsync(city).ConfigureAwait(false);

            return PartialView("_CurrentWeatherRow", response);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewForecastCity(string city)
        {
            var response = await _weatherService.GetForecastWeatherAsync(city).ConfigureAwait(false);

            return PartialView("_ForecastWeatherRow", response);
        }
    }
}
