using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Api.Weather.Client;
using Mihaylov.Web.Models;

namespace Mihaylov.Web.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IWeatherApiClient _client;

        public WeatherController(IWeatherApiClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var weather = await _client.ForecastCachedAsync().ConfigureAwait(false);

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
            var response = await _client.CurrentByCityAsync(city).ConfigureAwait(false);

            return PartialView("_CurrentWeatherRow", response);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewForecastCity(string city)
        {
            var response = await _client.ForecastByCityAsync(city).ConfigureAwait(false);

            return PartialView("_ForecastWeatherRow", response);
        }
    }
}
