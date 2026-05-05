using System.Collections.Generic;
using Mihaylov.Api.Other.Client;

namespace Mihaylov.Web.Models.Concerts;

public record ConcertMainModel(ConcertExtendedGrid Concerts, BandViewModel Bands,
    LocationGrid Locations, TicketProviderGrid TicketProviders, CountryExtendedGrid Countries,
    IEnumerable<ConcertType> ConcertTypes, AddConcertVewModel Input, string ActiveTab);
