using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Other.Client;
using Mihaylov.Common.Host.Authorization;
using Mihaylov.Web.Areas;
using Mihaylov.Web.Models.Concerts;

namespace Mihaylov.Web.Controllers
{
    [JwtAuthorize(Roles = UserConstants.AdminRole)]
    public class ConcertController : Controller
    {
        private readonly ILogger _logger;
        private readonly IOtherApiClient _client;

        public ConcertController(ILoggerFactory loggerFactory, IOtherApiClient client)
        {
            _logger = loggerFactory.CreateLogger(this.GetType());
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            if (!page.HasValue || (page <= 0))
            {
                return Redirect($"/Concert/{nameof(Index)}?page=1");
            }

            ConcertMainModel viewModel = await FillModel(null, page).ConfigureAwait(false);

            var maxPage = Math.Max(viewModel.Concerts.Pager.PageMax ?? 0, viewModel.Bands.Pager.PageMax ?? 0);
            maxPage = Math.Max(maxPage, viewModel.Locations.Pager.PageMax ?? 0);

            if (maxPage > 0 && page > maxPage)
            {
                return Redirect($"/Concert/{nameof(Index)}?page={maxPage}");
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

            ConcertMainModel viewModel = await FillModel(inputModel, 1).ConfigureAwait(false);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddConcertVewModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                ConcertMainModel viewModel = await FillModel(inputModel, 1).ConfigureAwait(false);
                return View(nameof(IndexRedirect), viewModel);
            }

            try
            {
                var concert = new ConcertModel()
                {
                    Id = 0,
                    Date = inputModel.Date.Value,
                    Name = inputModel.Name,
                    Price = (double)inputModel.Price.Value,
                    Currency = (CurrencyType)inputModel.Currency.Value,
                    LocationId = inputModel.Location.Value,
                    TicketProviderId = inputModel.TicketProvider.Value,
                    BandIds = inputModel.BandIds.Where(b => b.HasValue)
                                            .Select(b => b.Value)
                                            .ToList(),
                };

                _client.AddToken(Request.GetToken());
                var result = await _client.ConcertAsync(concert).ConfigureAwait(false);

                return RedirectToAction(nameof(Index));
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

                ConcertMainModel viewModel = await FillModel(null, 1).ConfigureAwait(false);
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
        public async Task<IActionResult> Band(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var band = new BandModel()
                {
                    Name = name
                };

                try
                {
                    _client.AddToken(Request.GetToken());
                    var result = await _client.BandAsync(band).ConfigureAwait(false);
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

                    ConcertMainModel viewModel = await FillModel(null, 1).ConfigureAwait(false);
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

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Location(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var location = new LocationModel()
                {
                    Name = name
                };

                _client.AddToken(Request.GetToken());
                var result = await _client.LocationAsync(location).ConfigureAwait(false);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> TicketProvider(AddTicketProviderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            var provider = new TicketProviderModel()
            {
                Name = model.Name,
                Url = model.Url,
            };

            _client.AddToken(Request.GetToken());
            var result = await _client.TicketProviderAsync(provider).ConfigureAwait(false);

            return RedirectToAction(nameof(Index));
        }


        private async Task<ConcertMainModel> FillModel(AddConcertVewModel inputModel, int? page)
        {
            if (inputModel == null)
            {
                inputModel = new AddConcertVewModel()
                {
                    Date = DateTime.Now.Date,
                };
            }

            _client.AddToken(Request.GetToken());
            var concerts = await _client.ConcertsAsync(page, 20).ConfigureAwait(false);
            var allBands = await _client.BandsAsync(null, null).ConfigureAwait(false);
            var bands = await _client.BandsAsync(page, 10).ConfigureAwait(false);
            var allLocations = await _client.LocationsAsync(null, null).ConfigureAwait(false);
            var locations = await _client.LocationsAsync(page, 10).ConfigureAwait(false);
            var ticketProviders = await _client.TicketProvidersAsync(null, null).ConfigureAwait(false);

            var currencies = Enum.GetValues(typeof(CurrencyType))
                                  .Cast<CurrencyType>()
                                  .Select(v => new SelectListItem(v.ToString(), ((int)v).ToString()))
                                  .ToList();

            inputModel.Currencies = currencies;
            inputModel.Bands = allBands.Data;
            inputModel.Locations = allLocations.Data;
            inputModel.TicketProviders = ticketProviders.Data;

            var viewModel = new ConcertMainModel()
            {
                Concerts = concerts,
                Bands = bands,
                Locations = locations,
                TicketProviders = ticketProviders,

                Input = inputModel
            };

            return viewModel;
        }
    }
}
