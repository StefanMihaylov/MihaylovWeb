using Mihaylov.Api.Gear.Core.Application.Models;
using Mihaylov.Api.Gear.Core.Application.Queries;

namespace Mihaylov.Api.Gear.Core.Application.Interfaces;

public interface ITripRepository
{
    Task<Trip> CreateAsync(Trip model, CancellationToken ct);

    Task<IEnumerable<Trip>> GetAllAsync(CancellationToken ct);

    Task<TripFull> GetByIdAsync(GetTripQuery request, CancellationToken ct);

    Task<long> CloneAsync(long tripId, CancellationToken ct);
}
