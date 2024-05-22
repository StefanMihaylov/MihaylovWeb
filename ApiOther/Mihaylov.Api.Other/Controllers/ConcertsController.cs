using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Other.Contracts.Base.Models;
using Mihaylov.Api.Other.Contracts.Show.Interfaces;
using Mihaylov.Api.Other.Contracts.Show.Models;
using Mihaylov.Api.Other.Extensions;
using Mihaylov.Api.Other.Models.Concerts;
using Mihaylov.Common.Host.Authorization;

namespace Mihaylov.Api.Other.Controllers
{
    [JwtAuthorize(Roles = UserConstants.AdminRole)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [TypeFilter(typeof(ErrorFilter))]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public class ConcertsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConcertService _service;

        public ConcertsController(ILoggerFactory loggerFactory, IConcertService service)
        {
            _logger = loggerFactory.CreateLogger(this.GetType());
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Grid<ConcertExtended>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Concerts([FromQuery]GridRequest request)
        {
            Grid<ConcertExtended> concerts = await _service.GetConcertsAsync(request).ConfigureAwait(false);

            return Ok(concerts);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ConcertExtended), StatusCodes.Status200OK)]
        public async Task<IActionResult> Concert(ConcertModel model)
        {
            var request = new Concert()
            {
                Id = model?.Id ?? 0,
                Date = model.Date.Value,
                Name = model?.Name,
                LocationId = model.LocationId.Value,
                Price = model.Price.Value,
                Currency = model.Currency.Value,
                TicketProviderId = model.TicketProviderId.Value,
                Bands = model.BandIds.Select(b => new Band() { Id = b }),
            };

            var concert = await _service.AddOrUpdateConcertAsync(request).ConfigureAwait(false);

            return Ok(concert);
        }

        [HttpGet]
        [ProducesResponseType(typeof(Grid<BandExtended>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Bands([FromQuery] GridRequest request)
        {
            Grid<BandExtended> bands = await _service.GetBandsAsync(request).ConfigureAwait(false);

            return Ok(bands);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Band), StatusCodes.Status200OK)]
        public async Task<IActionResult> Band(BandModel model)
        {
            var request = new Band()
            {
                Id = model?.Id ?? 0,
                Name = model?.Name,
            };

            Band band = await _service.AddOrUpdateBandAsync(request).ConfigureAwait(false);

            return Ok(band);
        }

        [HttpGet]
        [ProducesResponseType(typeof(Grid<Location>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Locations([FromQuery] GridRequest request)
        {
            Grid<Location> locations = await _service.GetLocationsAsync(request).ConfigureAwait(false);

            return Ok(locations);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Location), StatusCodes.Status200OK)]
        public async Task<IActionResult> Location(LocationModel model)
        {
            var request = new Location()
            {
                Id = model?.Id ?? 0,
                Name = model?.Name,
            };

            Location location = await _service.AddOrUpdateLocationAsync(request).ConfigureAwait(false);

            return Ok(location);
        }

        [HttpGet]
        [ProducesResponseType(typeof(Grid<TicketProvider>), StatusCodes.Status200OK)]
        public async Task<IActionResult> TicketProviders([FromQuery] GridRequest request)
        {
            Grid<TicketProvider> providers = await _service.GetTicketProvidersAsync(request).ConfigureAwait(false);

            return Ok(providers);
        }

        [HttpPost]
        [ProducesResponseType(typeof(TicketProvider), StatusCodes.Status200OK)]
        public async Task<IActionResult> TicketProvider(TicketProviderModel model)
        {
            var request = new TicketProvider()
            {
                Id = model?.Id ?? 0,
                Name = model?.Name,
                Url = model?.Url,
            };

            TicketProvider provider = await _service.AddOrUpdateTicketProviderAsync(request).ConfigureAwait(false);

            return Ok(provider);
        }
    }
}
