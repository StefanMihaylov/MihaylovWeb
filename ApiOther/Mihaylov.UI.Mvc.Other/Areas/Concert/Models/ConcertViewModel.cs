using Mihaylov.Api.Other.Client;

namespace Mihaylov.UI.Mvc.Other.Areas.Concert.Models;

public record ConcertViewModel(ConcertExtendedBaseGrid Concerts, AddConcertVewModel Input, ConcertQueryRequest Request);
