using Mihaylov.Api.Gear.Core.Application.Models;

namespace Mihaylov.Api.Gear.Core.Application.Interfaces;

public interface ICurrencyRepository
{
    Task<IEnumerable<Currency>> GetAllAsync(CancellationToken ct);
}
