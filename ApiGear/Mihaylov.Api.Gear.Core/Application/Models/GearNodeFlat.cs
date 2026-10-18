namespace Mihaylov.Api.Gear.Core.Application.Models;

public class GearNodeFlat
{
    public long Id { get; set; }

    public long TripId { get; set; }

    public string? ParentName { get; set; }

    public string? NodeName { get; set; }
}
