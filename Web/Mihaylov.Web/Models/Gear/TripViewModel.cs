using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mihaylov.Api.Gear.Client;

namespace Mihaylov.Web.Models.Gear;

public record TripViewModel(TripFull Trip, IEnumerable<SelectListItem> Types, IEnumerable<SelectListItem> Groups, 
    IEnumerable<SelectListItem> Categories, IEnumerable<SelectListItem> Items, bool NonPacked);
