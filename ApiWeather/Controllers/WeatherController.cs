using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeatherApi.Interfaces;

namespace WeatherApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IWeatherService _weatherService;

        public WeatherController(ILoggerFactory loggerFactory, IWeatherService weatherService)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _weatherService = weatherService;
        }

        [HttpGet]
        public async Task<IActionResult> Current(string city, bool? metricUnits)
        {
            try
            {
                var response = await _weatherService.CurrentAsync(city, metricUnits ?? true).ConfigureAwait(false);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Forecast(string city, bool? metricUnits)
        {
            try
            {
                var response = await _weatherService.ForecastAsync(city, 3, metricUnits ?? true).ConfigureAwait(false);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}