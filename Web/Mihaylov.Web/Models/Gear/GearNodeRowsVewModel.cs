using System.Collections.Generic;
using System.Linq;
using Mihaylov.Api.Gear.Client;

namespace Mihaylov.Web.Models.Gear;

public record GearNodeRowsVewModel(IEnumerable<GearNode> AllNodes, long? ParentId, int Level, bool? NonPacked)
{
    public IEnumerable<GearNode> Nodes => AllNodes.Where(n => n.ParentId == ParentId).OrderByDescending(n => (int)n.NodeType).ThenBy(n => n.Id);
}
