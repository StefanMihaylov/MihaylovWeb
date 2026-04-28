namespace Mihaylov.Api.Gear.Core.Application.Queries.GetCurrencies;

public class Currency
{
    public int Id { get; set; }

    public string Code { get; set; } = string.Empty; // e.g., "USD", "EUR"

    public string Symbol { get; set; } = string.Empty; // e.g., "$", "€"

    public bool IsDefault { get; set; }
}
