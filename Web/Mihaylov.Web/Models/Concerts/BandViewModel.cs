using System.Collections.Generic;
using Mihaylov.Api.Other.Client;

namespace Mihaylov.Web.Models.Concerts;

public record BandViewModel(BandExtendedGrid Bands, IEnumerable<CountryExtended> Countries);
