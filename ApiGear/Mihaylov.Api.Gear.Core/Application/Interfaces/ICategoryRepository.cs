using Mihaylov.Api.Gear.Core.Application.Models;

namespace Mihaylov.Api.Gear.Core.Application.Interfaces;

public interface ICategoryRepository
{
    Task<Category> CreateAsync(Category model, CancellationToken ct);

    Task<IEnumerable<Category>> GetAllAsync(CancellationToken ct);
}
