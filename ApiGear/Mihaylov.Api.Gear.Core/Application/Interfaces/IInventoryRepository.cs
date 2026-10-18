using Mihaylov.Api.Gear.Core.Application.Models;
using Mihaylov.Api.Gear.Core.Application.Queries;

namespace Mihaylov.Api.Gear.Core.Application.Interfaces;

public interface IInventoryRepository
{
    Task<long> CreateAsync(Inventory model, CancellationToken ct);

    Task<IEnumerable<Inventory>> GetAllAsync(GetInventoriesQuery request, CancellationToken ct);
}
