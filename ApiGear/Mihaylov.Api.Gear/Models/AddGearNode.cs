using System.ComponentModel.DataAnnotations;
using Mihaylov.Api.Gear.Core.Domain.Enums;

namespace Mihaylov.Api.Gear.Models;

public class AddGearNode
{
    public long? Id { get; set; }

    [Required]
    public long? TripId { get; set; }

    public long? ParentId { get; set; }

    [Required]
    public NodeType? NodeType { get; set; }

    public int? GroupId { get; set; }
    public int? CategoryId { get; set; }
    public long? InventoryItemId { get; set; }

    [Required]
    public int? Quantity { get; set; }

    [Required]
    public bool? IsPacked { get; set; }

    [Required]
    public bool? IsExcluded { get; set; }

    [Required]
    public bool? IsRequired { get; set; }
}
