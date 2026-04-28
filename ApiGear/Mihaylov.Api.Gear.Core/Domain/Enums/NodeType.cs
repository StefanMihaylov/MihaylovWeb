namespace Mihaylov.Api.Gear.Core.Domain.Enums;

public enum NodeType
{
    Group = 1,

    Category = 2,

    Item = 3,
}

public class NodeTypeDb
{
    public int NodeTypeId { get; set; }

    public string Name { get; set; } = string.Empty;
}
