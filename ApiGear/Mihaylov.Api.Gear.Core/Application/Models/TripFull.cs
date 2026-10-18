namespace Mihaylov.Api.Gear.Core.Application.Models;

public class TripFull : Trip
{
    public IEnumerable<GearNode> Nodes { get; set; } = new List<GearNode>();
}
