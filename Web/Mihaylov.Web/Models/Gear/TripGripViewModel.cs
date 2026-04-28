using System.Collections.Generic;
using Mihaylov.Api.Gear.Client;

namespace Mihaylov.Web.Models.Gear;

public record TripGripViewModel(IEnumerable<Trip> Trips);
