namespace Mihaylov.Api.Gear.Core.Application.Queries.GetTrips;

public class Trip
{
    public long Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public int Year { get; set; } = DateTime.Now.Year;

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
