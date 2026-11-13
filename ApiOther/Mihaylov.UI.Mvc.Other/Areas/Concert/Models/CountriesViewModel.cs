using Mihaylov.Api.Other.Client;

namespace Mihaylov.UI.Mvc.Other.Areas.Concert.Models;

public record CountriesViewModel(CountryExtendedBaseGrid Countries, ConcertQueryRequest Request);
