namespace Mihaylov.Api.Gear.Core.Domain.Lookups;

public class Currency
{
    public int CurrencyId { get; set; }

    public string Code { get; set; } = string.Empty; // e.g., "USD", "EUR"
    
    public string Symbol { get; set; } = string.Empty; // e.g., "$", "€"
    
    public bool IsDefault { get; set; }
}
