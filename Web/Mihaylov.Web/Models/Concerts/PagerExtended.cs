using Mihaylov.Api.Other.Client;

namespace Mihaylov.Web.Models.Concerts;

public record PagerExtended(Pager Pager, string ActiveTab);