using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mihaylov.Api.Gear.Client;

namespace Mihaylov.Web.Models.Gear;

public record InventoryGridViewModel(IEnumerable<Inventory> Items, IEnumerable<SelectListItem> Categories,
    IEnumerable<SelectListItem> Brands, IEnumerable<SelectListItem> Shops, IEnumerable<SelectListItem> Currencies,
    IEnumerable<SelectListItem> Statuses, bool ActiveOnly);
