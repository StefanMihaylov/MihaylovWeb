using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Api.Other.Client;
using Mihaylov.Web.Models;

namespace Mihaylov.Web.Controllers;

public class WeatherController(IOtherApiClient client) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var weather = await client.ForecastCachedAsync().ConfigureAwait(false);

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
        var response = await client.CurrentByCityAsync(city).ConfigureAwait(false);

        return PartialView("_CurrentWeatherRow", response);
    }

    [HttpPost]
    public async Task<IActionResult> AddNewForecastCity(string city)
    {
        var response = await client.ForecastByCityAsync(city).ConfigureAwait(false);

        return PartialView("_ForecastWeatherRow", response);
    }
}
