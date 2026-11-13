using System.Collections.Generic;
using Mihaylov.Api.Other.Client;

namespace Mihaylov.UI.Mvc.Other.Areas.Concert.Models;

public record BandViewModel(BandExtendedBaseGrid Bands, IEnumerable<CountryExtended> Countries, ConcertQueryRequest Request);
