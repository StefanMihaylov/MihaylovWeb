using System.ComponentModel.DataAnnotations;
using Mihaylov.Api.Gear.Client;

namespace Mihaylov.Web.Models.Gear;

public class TripGearNodeViewModel
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

    public bool? IsPacked { get; set; }

    public bool? IsExcluded { get; set; }

    public bool? IsRequired { get; set; }

    public bool? NonPacked { get; set; }
}
