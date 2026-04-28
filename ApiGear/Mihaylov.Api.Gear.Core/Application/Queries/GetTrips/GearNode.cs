using Mihaylov.Api.Gear.Core.Application.Queries.GetInventory;
using Mihaylov.Api.Gear.Core.Domain.Enums;

namespace Mihaylov.Api.Gear.Core.Application.Queries.GetTrips;

public class GearNode
{
    public long Id { get; set; }

    public long TripId { get; set; }

    public long? ParentId { get; set; }

    public NodeType NodeType { get; set; }

    public int? GroupId { get; set; }
    public string? GroupName { get; set; }

    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }

    public long? InventoryItemId { get; set; }
    public string? InventoryItem { get; set; }
    public bool? IsInventoryItemActive { get; set; }
    public IEnumerable<KitContentItem>? InventoryKitContents { get; set; }

    public int Quantity { get; set; }
    public bool IsPacked { get; set; }
    public bool IsExcluded { get; set; }
    public bool IsRequired { get; set; }
}
