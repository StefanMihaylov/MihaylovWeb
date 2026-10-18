using Mihaylov.Api.Gear.Core.Application.Models;

namespace Mihaylov.Api.Gear.Core.Application.Interfaces;

public interface IShopRepository
{
    Task<Shop> CreateAsync(Shop model, CancellationToken ct);

    Task<IEnumerable<Shop>> GetAllAsync(CancellationToken ct);
}
