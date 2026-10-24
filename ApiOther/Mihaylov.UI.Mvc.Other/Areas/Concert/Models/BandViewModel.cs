using System.Collections.Generic;
using Mihaylov.Api.Other.Client;

namespace Mihaylov.UI.Mvc.Other.Areas.Concert.Models;

public record BandViewModel(BandExtendedGrid Bands, IEnumerable<CountryExtended> Countries);
