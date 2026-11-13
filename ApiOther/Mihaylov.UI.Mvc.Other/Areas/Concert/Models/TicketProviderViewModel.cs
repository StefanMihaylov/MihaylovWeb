using Mihaylov.Api.Other.Client;

namespace Mihaylov.UI.Mvc.Other.Areas.Concert.Models;

public record TicketProviderViewModel(TicketProviderBaseGrid Providers, ConcertQueryRequest Request);
