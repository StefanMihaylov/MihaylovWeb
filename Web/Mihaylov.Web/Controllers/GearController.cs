using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Gear.Client;
using Mihaylov.Web.Areas;
using Mihaylov.Web.Models.Configs;
using Mihaylov.Web.Models.Gear;

namespace Mihaylov.Web.Controllers;

public class GearController : Controller
{
    private readonly ILogger _logger;
    private readonly IGearApiClient _client;

    public GearController(ILoggerFactory loggerFactory, IGearApiClient client)
    {
        _logger = loggerFactory.CreateLogger(this.GetType());
        _client = client;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        _client.AddToken(Request.GetToken());
        var inventory = await _client.InventoryListAsync(true).ConfigureAwait(false);

        var trips = await _client.TripListAsync().ConfigureAwait(false);

        var model = new TripGripViewModel(trips);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Clone(long id)
    {
        _client.AddToken(Request.GetToken());
        var newTripId = await _client.TripCloneAsync(id).ConfigureAwait(false);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> SaveTrip(TripAddViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var request = new AddTripModel()
        {
            Id = model.Id ?? 0,
            Title = model.Title,
            Notes = model.Notes,
            Year = model.Year ?? DateTime.UtcNow.Year,
            CreatedAt = model.CreatedAt ?? DateTime.Now
        };

        _client.AddToken(Request.GetToken());
        var item = await _client.TripAddAsync(request).ConfigureAwait(false);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Trip(long id, bool? nonPacked)
    {
        bool isNonPacked = nonPacked ?? false;

        _client.AddToken(Request.GetToken());

        var trip = await _client.TripGetAsync(id, isNonPacked).ConfigureAwait(false);

        var groups = await _client.GroupsAsync().ConfigureAwait(false);
        var categories = await _client.CategoriesAsync().ConfigureAwait(false);
        var items = await _client.InventoryListAsync(true).ConfigureAwait(false);

        var groupsDropDown = groups.Select(v => new SelectListItem(v.Name, v.Id.ToString())).ToList();
        var categoriesDropDown = categories.Select(v => new SelectListItem(v.Name, v.Id.ToString())).ToList();
        var itemsDropDown = items.OrderBy(i => i.CategoryId).Select(v => new SelectListItem($"{v.Name}{(string.IsNullOrWhiteSpace(v.Brand) ? string.Empty : $" ({v.Brand})")} ", v.Id.ToString())).ToList();

        var statusTypes = ViewConstants.GetEnumDropdown<NodeType>().OrderByDescending(d => d.Value);

        var tripModel = new TripViewModel(trip, statusTypes, groupsDropDown, categoriesDropDown, itemsDropDown, isNonPacked);

        return View(tripModel);
    }

    [HttpGet]
    public async Task<IActionResult> Packed(long tripId, bool? nonPacked, long nodeId)
    {
        _client.AddToken(Request.GetToken());
        await _client.TripNodePackedAsync(nodeId).ConfigureAwait(false);

        bool? isNonPacked = nonPacked == true ? nonPacked : null;

        return RedirectToAction(nameof(Trip), new { Id = tripId, NonPacked = isNonPacked });
    }

    [HttpPost]
    public async Task<IActionResult> SaveGearNode(TripGearNodeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var request = new AddGearNode()
        {
            Id = model.Id ?? 0,
            TripId = model.TripId.Value,
            ParentId = model.ParentId,
            NodeType = model.NodeType.Value,
            GroupId = model.GroupId,
            CategoryId = model.CategoryId,
            InventoryItemId = model.InventoryItemId,
            Quantity = model.Quantity.Value,
            IsPacked = model.IsPacked ?? false,
            IsExcluded = model.IsExcluded ?? false,
            IsRequired = model.IsRequired ?? false,
        };

        _client.AddToken(Request.GetToken());
        var itemId = await _client.TripNodeAsync(request).ConfigureAwait(false);

        return RedirectToAction(nameof(Trip), new { Id = request.TripId, NonPacked = (model.NonPacked == true ? model.NonPacked : null) });
    }

    [HttpGet]
    public async Task<IActionResult> DeleteNode(long tripId, bool? nonPacked, long nodeId)
    {
        if (nodeId > 0)
        {
            _client.AddToken(Request.GetToken());
            await _client.TripNodeDeleteAsync(nodeId).ConfigureAwait(false);
        }

        return RedirectToAction(nameof(Trip), new { Id = tripId, NonPacked = (nonPacked == true ? nonPacked : null) });
    }

    [HttpGet]
    public async Task<IActionResult> Inventory(bool? isActive)
    {
        _client.AddToken(Request.GetToken());

        var inventory = await _client.InventoryListAsync(isActive).ConfigureAwait(false);

        var categories = await _client.CategoriesAsync().ConfigureAwait(false);
        var brands = await _client.BrandsAsync().ConfigureAwait(false);
        var shops = await _client.ShopsAsync().ConfigureAwait(false);
        var currencies = await _client.CurrenciesAsync().ConfigureAwait(false);

        var categoriesDropDown = categories.Select(v => new SelectListItem(v.Name, v.Id.ToString())).ToList();
        var brandsDropDown = brands.Select(v => new SelectListItem(v.Name, v.Id.ToString())).ToList();
        var shopsDropDown = shops.Select(v => new SelectListItem(v.Name, v.Id.ToString())).ToList();
        var currenciesDropDown = currencies.Select(v => new SelectListItem(v.Code, v.Id.ToString())).ToList();

        var statusTypes = ViewConstants.GetEnumDropdown<ItemStatus>();

        var viewModel = new InventoryGridViewModel(inventory, categoriesDropDown, brandsDropDown, shopsDropDown,
            currenciesDropDown, statusTypes, isActive ?? false);

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> SaveInventory(InventoryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var reguest = new AddInventoryModel()
        {
            Id = model.Id ?? 0,
            Name = model.Name,
            Description = model.Description,
            BrandId = model.BrandId,
            ShopId = model.ShopId,
            CategoryId = model.CategoryId ?? 0,
            Price = (double?)model.Price,
            CurrencyId = model.CurrencyId,
            PurchaseDate = model.PurchaseDate,
            ItemStatus = (ItemStatus)model.ItemStatus,
            KitContents = model.KitContents?.Select(k => new KitContentItem() { Name = k.Name }),
        };

        _client.AddToken(Request.GetToken());
        var item = await _client.InventoryAddAsync(reguest).ConfigureAwait(false);

        return RedirectToAction(nameof(Inventory));
    }

    [HttpGet]
    public async Task<IActionResult> Lists()
    {
        _client.AddToken(Request.GetToken());
        var currencies = await _client.CurrenciesAsync().ConfigureAwait(false);

        var groups = await _client.GroupsAsync().ConfigureAwait(false);
        var brands = await _client.BrandsAsync().ConfigureAwait(false);
        var shops = await _client.ShopsAsync().ConfigureAwait(false);
        var categories = await _client.CategoriesAsync().ConfigureAwait(false);

        var viewModel = new ListsViewModel(groups, brands, shops, categories);

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> SaveGroup(SmallItemViewModel input)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var request = new Group()
        {
            Id = input.Id ?? 0,
            Name = input.Name.Trim()
        };

        _client.AddToken(Request.GetToken());
        await _client.GroupAsync(request).ConfigureAwait(false);

        return RedirectToAction(nameof(Lists));
    }

    [HttpPost]
    public async Task<IActionResult> SaveBrand(SmallItemViewModel input)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var request = new Brand()
        {
            Id = input.Id ?? 0,
            Name = input.Name.Trim()
        };

        _client.AddToken(Request.GetToken());
        await _client.BrandAsync(request).ConfigureAwait(false);

        return RedirectToAction(nameof(Lists));
    }

    [HttpPost]
    public async Task<IActionResult> SaveShop(SmallItemViewModel input)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var request = new Shop()
        {
            Id = input.Id ?? 0,
            Name = input.Name.Trim()
        };

        _client.AddToken(Request.GetToken());
        await _client.ShopAsync(request).ConfigureAwait(false);

        return RedirectToAction(nameof(Lists));
    }

    [HttpPost]
    public async Task<IActionResult> SaveCategory(SmallItemViewModel input)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var request = new Category()
        {
            Id = input.Id ?? 0,
            Name = input.Name.Trim()
        };

        _client.AddToken(Request.GetToken());
        await _client.CategoryAsync(request).ConfigureAwait(false);

        return RedirectToAction(nameof(Lists));
    }
}
