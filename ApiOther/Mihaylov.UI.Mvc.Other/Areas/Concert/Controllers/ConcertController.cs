using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Other.Client;
using Mihaylov.Common;
using Mihaylov.UI.Mvc.Other.Areas.Concert.Models;

namespace Mihaylov.UI.Mvc.Other.Areas.Concert.Controllers
{
    [AdminAuthorize]
    [Area("Concert")]
    public class ConcertController : Controller
    {
        public const string CONCERTS_TAB = "concerts";
        public const string BANDS_TAB = "bands";
        public const string LOCATIONS_TAB = "locations";
        public const string PROVIDERS_TAB = "providers";
        public const string COUNTRIES_TAB = "countries";
        public const string CONCERT_TYPES_TAB = "concertTypes";

        private readonly ILogger _logger;
        private readonly IOtherApiClient _client;

        public ConcertController(ILoggerFactory loggerFactory, IOtherApiClient client)
        {
            _logger = loggerFactory.CreateLogger(this.GetType());
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Index(ConcertQueryRequest request)
        {
            if (request == null)
            {
                request = new ConcertQueryRequest();
            }

            if (string.IsNullOrWhiteSpace(request.ActiveTab))
            {
                request.ActiveTab = CONCERTS_TAB;
            }

            if (!request.Page.HasValue || (request.Page <= 0))
            {
                request.Page = 1;
            }

            ConcertMainModel viewModel = await FillModel(null, request).ConfigureAwait(false);

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> IndexRedirect(AddConcertVewModel inputModel)
        {
            if (inputModel?.Date.HasValue != true)
            {
                ModelState.Clear();

                inputModel = new AddConcertVewModel()
                {
                    Date = DateTime.Now.Date,
                };
            }

            ConcertMainModel viewModel = await FillModel(inputModel, new ConcertQueryRequest(1, CONCERTS_TAB)).ConfigureAwait(false);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddConcertVewModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                ConcertMainModel viewModel = await FillModel(inputModel, new ConcertQueryRequest(1, CONCERTS_TAB)).ConfigureAwait(false);
                return RedirectToAction(nameof(IndexRedirect), viewModel);
            }

            try
            {
                var concert = new ConcertModel()
                {
                    Id = inputModel.Id ?? 0,
                    Date = inputModel.Date.Value,
                    Name = inputModel.Name,
                    Price = (double)inputModel.Price.Value,
                    Currency = (CurrencyType)inputModel.Currency.Value,
                    ConcertTypeId = inputModel.ConcertTypeId,
                    LocationId = inputModel.Location.Value,
                    TicketProviderId = inputModel.TicketProvider.Value,
                    BandIds = inputModel.BandIds.Where(b => b.HasValue)
                                            .Select(b => b.Value)
                                            .ToList(),
                };

                var result = await _client.ConcertAsync(concert).ConfigureAwait(false);

                return RedirectToAction(nameof(Index), new { page = inputModel.Page, activeTab = CONCERTS_TAB });
            }
            catch (SwaggerException<Dictionary<string, IEnumerable<string>>> ex)
            {
                var builder = new StringBuilder();
                foreach (var errors in ex.Result)
                {
                    builder.Append($"{errors.Key} => {string.Join(", ", errors.Value)}");
                    foreach (var error in errors.Value)
                    {
                        ModelState.AddModelError(errors.Key, error);
                    }
                }

                _logger.LogError(ex, "Error in Add/update band. Error: {message}", builder.ToString());

                ConcertMainModel viewModel = await FillModel(null, new ConcertQueryRequest(1, CONCERTS_TAB)).ConfigureAwait(false);
                return RedirectToAction(nameof(IndexRedirect), viewModel);
            }
            catch (SwaggerException ex)
            {
                _logger.LogError(ex, "Error in Add/update band. Error: {message}", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Add/update band. Error: {message}", ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Band(AddBandViewModel band)
        {
            if (!string.IsNullOrEmpty(band?.Name))
            {
                try
                {
                    var bandModel = new BandModel()
                    {
                        Id = band.Id ?? 0,
                        Name = band.Name,
                        CountryId = band.CountryId,
                    };

                    var result = await _client.BandAsync(bandModel).ConfigureAwait(false);
                }
                catch (SwaggerException<Dictionary<string, IEnumerable<string>>> ex)
                {
                    var builder = new StringBuilder();
                    foreach (var errors in ex.Result)
                    {
                        builder.Append($"{errors.Key} => {string.Join(", ", errors.Value)}");
                        foreach (var error in errors.Value)
                        {
                            ModelState.AddModelError(errors.Key, error);
                        }
                    }

                    _logger.LogError(ex, "Error in Add/update band. Error: {message}", builder.ToString());

                    ConcertMainModel viewModel = await FillModel(null, new ConcertQueryRequest(1, BANDS_TAB)).ConfigureAwait(false);

                    return RedirectToAction(nameof(IndexRedirect), viewModel);
                }
                catch (SwaggerException ex)
                {
                    _logger.LogError(ex, "Error in Add/update band. Error: {message}", ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in Add/update band. Error: {message}", ex.Message);
                }
            }

            return RedirectToAction(nameof(Index), new { page = band.Page, activeTab = BANDS_TAB });
        }

        [HttpPost]
        public async Task<IActionResult> Location(LocationModel location)
        {
            if (!string.IsNullOrEmpty(location?.Name))
            {
                var result = await _client.LocationAsync(location).ConfigureAwait(false);
            }

            return RedirectToAction(nameof(Index), new { activeTab = LOCATIONS_TAB });
        }

        [HttpPost]
        public async Task<IActionResult> TicketProvider(AddTicketProviderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index), new { activeTab = PROVIDERS_TAB });
            }

            var provider = new TicketProviderModel()
            {
                Id = model.Id ?? 0,
                Name = model.Name,
                Url = model.Url,
                IsActive = model.IsActive,
            };

            var result = await _client.TicketProviderAsync(provider).ConfigureAwait(false);

            return RedirectToAction(nameof(Index), new { activeTab = PROVIDERS_TAB });
        }

        [HttpPost]
        public async Task<IActionResult> Country(CountryModel model)
        {
            if (!string.IsNullOrEmpty(model?.Name))
            {
                var result = await _client.CountryAsync(model).ConfigureAwait(false);
            }

            return RedirectToAction(nameof(Index), new { activeTab = COUNTRIES_TAB });
        }

        [HttpPost]
        public async Task<IActionResult> ConcertType(ConcertTypeModel model)
        {
            if (!string.IsNullOrEmpty(model?.Name))
            {
                var result = await _client.ConcertTypeAsync(model).ConfigureAwait(false);
            }

            return RedirectToAction(nameof(Index), new { activeTab = CONCERT_TYPES_TAB });
        }


        private async Task<ConcertMainModel> FillModel(AddConcertVewModel inputModel, ConcertQueryRequest request)
        {
            if (inputModel == null)
            {
                inputModel = new AddConcertVewModel()
                {
                    Date = DateTime.Now.Date,
                };
            }

            if (request == null)
            {
                request = new ConcertQueryRequest();
            }

            if (string.IsNullOrWhiteSpace(request.ActiveTab))
            {
                request.ActiveTab = CONCERTS_TAB;
            }

            var concertsRequest = new ConcertQueryRequest(1, CONCERTS_TAB);
            var bandsRequest = new ConcertQueryRequest(1, BANDS_TAB);
            var locationsRequest = new ConcertQueryRequest(1, LOCATIONS_TAB);
            var ticketsRequest = new ConcertQueryRequest(1, PROVIDERS_TAB);
            var countriesRequest = new ConcertQueryRequest(1, COUNTRIES_TAB);

            if (request.Page.HasValue && request.Page > 0)
            {
                switch (request.ActiveTab)
                {
                    case CONCERTS_TAB:
                        concertsRequest.Page = request.Page;
                        break;
                    case BANDS_TAB:
                        bandsRequest.Page = request.Page;
                        break;
                    case LOCATIONS_TAB:
                        locationsRequest.Page = request.Page;
                        break;
                    case PROVIDERS_TAB:
                        ticketsRequest.Page = request.Page;
                        break;
                    case COUNTRIES_TAB:
                        countriesRequest.Page = request.Page;
                        break;
                }
            }

            var concerts = await _client.ConcertsAsync(concertsRequest.Page, 20).ConfigureAwait(false);

            var allBands = await _client.BandsAsync(null, null).ConfigureAwait(false);
            var bands = await _client.BandsAsync(bandsRequest.Page, 20).ConfigureAwait(false);

            var allLocations = await _client.LocationsAsync(null, null).ConfigureAwait(false);
            var locations = await _client.LocationsAsync(locationsRequest.Page, 20).ConfigureAwait(false);

            var allTicketProviders = await _client.TicketProvidersAsync(null, null).ConfigureAwait(false);
            var ticketProviders = await _client.TicketProvidersAsync(ticketsRequest.Page, 20).ConfigureAwait(false);

            var allCountries = await _client.CountriesAsync(null, null).ConfigureAwait(false);
            var countries = await _client.CountriesAsync(countriesRequest.Page, 20).ConfigureAwait(false);

            var concertTypes = await _client.ConcertTypesAsync().ConfigureAwait(false);

            inputModel.Currencies = EnumExtensions.GetDropdown<CurrencyType>().OrderByDescending(c => c.Value);
            inputModel.ConcertTypes = concertTypes;
            inputModel.Bands = allBands.Data;
            inputModel.Locations = allLocations.Data;
            inputModel.TicketProviders = allTicketProviders.Data.Where(tp => tp.IsActive).ToList();
            inputModel.Page = concertsRequest.Page ?? 1;

            var viewModel = new ConcertMainModel(request,
                new ConcertViewModel(concerts, inputModel, concertsRequest),
                new BandViewModel(bands, allCountries.Data, bandsRequest),
                new LocationViewModel(locations, locationsRequest),
                new TicketProviderViewModel(ticketProviders, ticketsRequest),
                new CountriesViewModel(countries, countriesRequest),
                concertTypes);

            return viewModel;
        }
    }
}
