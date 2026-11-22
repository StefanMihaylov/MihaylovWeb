using System.Collections.Generic;
using Mihaylov.Api.Gear.Client;

namespace Mihaylov.UI.Mvc.Gear.Areas.Gear.Models.Gear;

public record TripGripViewModel(IEnumerable<Trip> Trips);
