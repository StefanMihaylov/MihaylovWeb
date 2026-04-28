namespace Mihaylov.Api.Gear.Core.Application.Queries.GetTrips
{
    public class TripFull : Trip
    {
        public IEnumerable<GearNode> Nodes { get; set; } = new List<GearNode>();
    }
}
