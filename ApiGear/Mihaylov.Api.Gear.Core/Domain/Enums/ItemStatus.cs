namespace Mihaylov.Api.Gear.Core.Domain.Enums;

public enum ItemStatus
{
    Active = 1,

    Broken = 2,

    Lost = 3,

    Retired = 4,

    Ignored = 5,
}

public class ItemStatusDb
{
    public int ItemStatusId { get; set; }

    public string Name { get; set; } = string.Empty;
}
