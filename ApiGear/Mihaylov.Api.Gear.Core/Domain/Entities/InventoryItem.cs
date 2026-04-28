using Mihaylov.Api.Gear.Core.Domain.Enums;
using Mihaylov.Api.Gear.Core.Domain.Lookups;

namespace Mihaylov.Api.Gear.Core.Domain.Entities;

public class InventoryItem
{
    public long InventoryItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public int? BrandId { get; set; }
    public Brand? Brand { get; set; }

    public int? ShopId { get; set; }
    public Shop? Shop { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public decimal? Price { get; set; }
    public int? CurrencyId { get; set; }
    public Currency? Currency { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public int ItemStatusId { get; set; }
    public ItemStatusDb ItemStatus { get; set; } = null!;

    public List<KitContentItem>? KitContents { get; set; }
}

public class KitContentItem
{
    public string Name { get; set; } = string.Empty;
}
