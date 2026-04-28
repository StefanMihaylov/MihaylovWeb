using Mihaylov.Api.Gear.Core.Domain.Enums;

namespace Mihaylov.Api.Gear.Core.Application.Queries.GetInventory;

public class Inventory
{
    public long Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public int? BrandId { get; set; }
    public string? Brand { get; set; }

    public int? ShopId { get; set; } 
    public string? Shop { get; set; }

    public int CategoryId { get; set; }
    public string Category { get; set; } = null!;

    public decimal? Price { get; set; } 
    public int? CurrencyId { get; set; } 
    public string? Currency { get; set; }

    public DateTime? PurchaseDate { get; set; }
    public ItemStatus ItemStatus { get; set; }

    public IEnumerable<KitContentItem>? KitContents { get; set; }
}

public class KitContentItem
{
    public string Name { get; set; } = string.Empty;
}
