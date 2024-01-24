using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Weather.Contracts.Interfaces;
using Mihaylov.Api.Weather.Contracts.Models;
using Mihaylov.Api.Weather.Models;

namespace Mihaylov.Api.Weather.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("/api/[controller]/[action]")]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IWeatherApiService _weatherService;

        public WeatherController(ILoggerFactory loggerFactory, IWeatherApiService weatherService)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _weatherService = weatherService;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrentWeatherModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> Current([FromQuery]RequestModel model)
        {
            try
            {
                model.AddDefault();

                var response = await _weatherService.CurrentAsync(model.City, model.MetricUnits.Value, model.Language).ConfigureAwait(false);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ForecastWeatherModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> Forecast([FromQuery] RequestModel model)
        {
            try
            {
                model.AddDefault();

                var response = await _weatherService.ForecastAsync(model.City, 3, model.MetricUnits.Value, model.Language).ConfigureAwait(false);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
