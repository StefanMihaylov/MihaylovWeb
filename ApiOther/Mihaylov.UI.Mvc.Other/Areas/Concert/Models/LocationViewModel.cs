using Mihaylov.Api.Other.Client;

namespace Mihaylov.UI.Mvc.Other.Areas.Concert.Models;

public record LocationViewModel(LocationBaseGrid Locations, ConcertQueryRequest Request);
