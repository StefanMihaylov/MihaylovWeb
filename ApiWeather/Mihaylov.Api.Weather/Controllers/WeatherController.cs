using System;
using System.Collections.Generic;
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
        private readonly IWeatherApiService _service;
        private readonly IWeatherManager _manager;

        public WeatherController(ILoggerFactory loggerFactory, IWeatherApiService service, IWeatherManager manager)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _service = service;
            _manager = manager;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrentWeatherModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> Current([FromQuery] RequestModel model)
        {
            try
            {
                model.AddDefault();

                CurrentWeatherModel response = await _service.CurrentAsync(model.City, model.MetricUnits.Value, model.Language).ConfigureAwait(false);

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

                ForecastWeatherModel response = await _service.ForecastAsync(model.City, 3, model.MetricUnits.Value, model.Language).ConfigureAwait(false);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CurrentWeatherResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> CurrentCached()
        {
            try
            {
                IEnumerable<CurrentWeatherResponse> response = await _manager.GetCurrentWeatherAsync().ConfigureAwait(false);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrentWeatherResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> CurrentByCity(string city)
        {
            try
            {
                CurrentWeatherResponse response = await _manager.GetCurrentWeatherAsync(city).ConfigureAwait(false);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ForecastWeatherResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> ForecastCached()
        {
            try
            {
                IEnumerable<ForecastWeatherResponse> response = await _manager.GetForecastWeatherAsync().ConfigureAwait(false);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ForecastWeatherResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> ForecastByCity(string city)
        {
            try
            {
                ForecastWeatherResponse response = await _manager.GetForecastWeatherAsync(city).ConfigureAwait(false);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
