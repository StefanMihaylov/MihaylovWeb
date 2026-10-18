using Mihaylov.Api.Gear.Core.Application.Models;

namespace Mihaylov.Api.Gear.Core.Application.Interfaces;

public interface IGearNodeRepository
{
    Task<long> CreateAsync(GearNode model, CancellationToken ct);

    Task AlterAsync(long noteId, CancellationToken ct);

    Task DeleteAsync(long noteId, CancellationToken ct);

    Task<IEnumerable<GearNodeFlat>> GetByTripIdAsync(long tripId, CancellationToken ct);
}
