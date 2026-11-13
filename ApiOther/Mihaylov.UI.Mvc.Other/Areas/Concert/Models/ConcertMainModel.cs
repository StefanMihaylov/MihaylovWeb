using System.Collections.Generic;
using Mihaylov.Api.Other.Client;

namespace Mihaylov.UI.Mvc.Other.Areas.Concert.Models;

public record ConcertMainModel(ConcertQueryRequest Request, ConcertViewModel Concerts, BandViewModel Bands,
    LocationViewModel Locations, TicketProviderViewModel TicketProviders, CountriesViewModel Countries,
    IEnumerable<ConcertType> ConcertTypes);
