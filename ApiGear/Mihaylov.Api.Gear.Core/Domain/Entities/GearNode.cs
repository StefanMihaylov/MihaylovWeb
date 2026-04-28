using Mihaylov.Api.Gear.Core.Domain.Enums;
using Mihaylov.Api.Gear.Core.Domain.Lookups;

namespace Mihaylov.Api.Gear.Core.Domain.Entities;

public class GearNode
{
    public long GearNodeId { get; set; }

    public long TripId { get; set; }
    public Trip Trip { get; set; } = null!;

    public long? ParentId { get; set; }
    public GearNode? Parent { get; set; }
    public ICollection<GearNode> Children { get; set; } = new List<GearNode>();

    public int NodeTypeId { get; set; }
    public NodeTypeDb NodeType { get; set; } = null!;

    // 1. If NodeType is "Group"
    public int? GroupId { get; set; }
    public Group? Group { get; set; }

    // 2. If NodeType is "Category"
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }

    // 3. If NodeType is "SpecificItem"
    public long? InventoryItemId { get; set; }
    public InventoryItem? InventoryItem { get; set; }

    public int Quantity { get; set; }
    public bool IsPacked { get; set; }
    public bool IsExcluded { get; set; }
    public bool IsRequired { get; set; }
}
