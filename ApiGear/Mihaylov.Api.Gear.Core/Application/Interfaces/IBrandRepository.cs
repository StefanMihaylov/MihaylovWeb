using Mihaylov.Api.Gear.Core.Application.Models;

namespace Mihaylov.Api.Gear.Core.Application.Interfaces;

public interface IBrandRepository
{
    Task<Brand> CreateAsync(Brand model, CancellationToken ct);

    Task<IEnumerable<Brand>> GetAllAsync(CancellationToken ct);
}
