using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mihaylov.Api.Gear.Client;

namespace Mihaylov.UI.Mvc.Gear.Areas.Gear.Models.Gear;

public record TripViewModel(TripFull Trip, IEnumerable<SelectListItem> Types, IEnumerable<SelectListItem> Groups, 
    IEnumerable<SelectListItem> Categories, IEnumerable<SelectListItem> Items, IEnumerable<SelectListItem> Parents,
    bool NonPacked);
