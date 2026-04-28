namespace Mihaylov.Api.Gear.Core.Domain.Entities;

public class Trip
{
    public long TripId { get; set; }

    public string Title { get; set; } = string.Empty;

    public int Year { get; set; } = DateTime.Now.Year;

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<GearNode> Nodes { get; set; } = new List<GearNode>();
}
