using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Mihaylov.Api.Other.Client;
using Mihaylov.Common.Host.Authorization;
using Mihaylov.Web.Areas;
using Mihaylov.Web.Models.Concerts;

namespace Mihaylov.Web.Controllers
{
    [JwtAuthorize(Roles = UserConstants.AdminRole)]
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
        public async Task<IActionResult> Index(int? page, string activeTab)
        {
            if (string.IsNullOrWhiteSpace(activeTab))
            {
                activeTab = CONCERTS_TAB;
            }

            if (!page.HasValue || (page <= 0))
            {
                return Redirect($"/Concert/{nameof(Index)}?page=1&activeTab={activeTab}");
            }

            ConcertMainModel viewModel = await FillModel(null, page, activeTab).ConfigureAwait(false);

            var maxPage = Math.Max(viewModel.Concerts.Pager.PageMax ?? 0, viewModel.Bands.Bands.Pager.PageMax ?? 0);
            maxPage = Math.Max(maxPage, viewModel.Locations.Pager.PageMax ?? 0);

            if (maxPage > 0 && page > maxPage)
            {
                return Redirect($"/Concert/{nameof(Index)}?page={maxPage}&activeTab={activeTab}");
            }

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

            ConcertMainModel viewModel = await FillModel(inputModel, 1, CONCERTS_TAB).ConfigureAwait(false);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddConcertVewModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                ConcertMainModel viewModel = await FillModel(inputModel, 1, CONCERTS_TAB).ConfigureAwait(false);
                return View(nameof(IndexRedirect), viewModel);
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

                _client.AddToken(Request.GetToken());
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

                ConcertMainModel viewModel = await FillModel(null, 1, CONCERTS_TAB).ConfigureAwait(false);
                return View(nameof(IndexRedirect), viewModel);
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

                    _client.AddToken(Request.GetToken());
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

                    ConcertMainModel viewModel = await FillModel(null, 1, BANDS_TAB).ConfigureAwait(false);

                    return View(nameof(IndexRedirect), viewModel);
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
                _client.AddToken(Request.GetToken());
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

            _client.AddToken(Request.GetToken());
            var result = await _client.TicketProviderAsync(provider).ConfigureAwait(false);

            return RedirectToAction(nameof(Index), new { activeTab = PROVIDERS_TAB });
        }

        [HttpPost]
        public async Task<IActionResult> Country(CountryModel model)
        {
            if (!string.IsNullOrEmpty(model?.Name))
            {
                _client.AddToken(Request.GetToken());
                var result = await _client.CountryAsync(model).ConfigureAwait(false);
            }

            return RedirectToAction(nameof(Index), new { activeTab = COUNTRIES_TAB });
        }

        [HttpPost]
        public async Task<IActionResult> ConcertType(ConcertTypeModel model)
        {
            if (!string.IsNullOrEmpty(model?.Name))
            {
                _client.AddToken(Request.GetToken());
                var result = await _client.ConcertTypeAsync(model).ConfigureAwait(false);
            }

            return RedirectToAction(nameof(Index), new { activeTab = CONCERT_TYPES_TAB });
        }


        private async Task<ConcertMainModel> FillModel(AddConcertVewModel inputModel, int? page, string activeTab)
        {
            if (inputModel == null)
            {
                inputModel = new AddConcertVewModel()
                {
                    Date = DateTime.Now.Date,
                };
            }

            if (string.IsNullOrWhiteSpace(activeTab))
            {
                activeTab = CONCERTS_TAB;
            }

            int? concertsPage = 1;
            int? bandsPage = 1;
            int? locationsPage = 1;
            int? ticketsPage = 1;
            int? countriesPage = 1;

            if (page.HasValue && page > 0)
            {
                switch (activeTab)
                {
                    case CONCERTS_TAB:
                        concertsPage = page;
                        break;
                    case BANDS_TAB:
                        bandsPage = page;
                        break;
                    case LOCATIONS_TAB:
                        locationsPage = page;
                        break;
                    case PROVIDERS_TAB:
                        ticketsPage = page;
                        break;
                    case COUNTRIES_TAB:
                        countriesPage = page;
                        break;
                }
            }

            _client.AddToken(Request.GetToken());
            var concerts = await _client.ConcertsAsync(concertsPage, 20).ConfigureAwait(false);
            var allBands = await _client.BandsAsync(null, null).ConfigureAwait(false);
            var bands = await _client.BandsAsync(bandsPage, 20).ConfigureAwait(false);
            var allLocations = await _client.LocationsAsync(null, null).ConfigureAwait(false);
            var locations = await _client.LocationsAsync(locationsPage, 20).ConfigureAwait(false);
            var allTicketProviders = await _client.TicketProvidersAsync(null, null).ConfigureAwait(false);
            var ticketProviders = await _client.TicketProvidersAsync(ticketsPage, 20).ConfigureAwait(false);
            var allCountries = await _client.CountriesAsync(null, null).ConfigureAwait(false);
            var countries = await _client.CountriesAsync(countriesPage, 20).ConfigureAwait(false);
            var concertTypes = await _client.ConcertTypesAsync().ConfigureAwait(false);

            var currencies = Enum.GetValues(typeof(CurrencyType))
                                  .Cast<CurrencyType>()
                                  .Select(v => new SelectListItem(v.ToString(), ((int)v).ToString()))
                                  .OrderByDescending(c => c.Value)
                                  .ToList();

            inputModel.Currencies = currencies;
            inputModel.ConcertTypes = concertTypes;
            inputModel.Bands = allBands.Data;
            inputModel.Locations = allLocations.Data;
            inputModel.TicketProviders = allTicketProviders.Data.Where(tp => tp.IsActive).ToList();
            inputModel.Page = concertsPage ?? 1;

            var viewModel = new ConcertMainModel(concerts, new BandViewModel(bands, allCountries.Data), locations,
                ticketProviders, countries, concertTypes, inputModel, activeTab);

            return viewModel;
        }
    }
}
