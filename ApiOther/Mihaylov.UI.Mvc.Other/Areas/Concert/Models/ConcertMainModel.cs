using System.Collections.Generic;
using Mihaylov.Api.Other.Client;

namespace Mihaylov.UI.Mvc.Other.Areas.Concert.Models;

public record ConcertMainModel(ConcertExtendedGrid Concerts, BandViewModel Bands,
    LocationGrid Locations, TicketProviderGrid TicketProviders, CountryExtendedGrid Countries,
    IEnumerable<ConcertType> ConcertTypes, AddConcertVewModel Input, string ActiveTab);
